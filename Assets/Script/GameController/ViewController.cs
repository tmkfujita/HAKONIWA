using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

//===================================================
//This Class is the main controller for HAKONIWA game
//what is checking every interaction of game objects.
//===================================================

public class ViewController : MonoBehaviour {

    private RigidbodyFirstPersonController rigidBodyFPSController;
    private GuiController guiContorller;//rendering controller of game items
    private RaycastHit hit;
    private bool menuModeFlg = false;
    private ArrayList gameControllerObjects = new ArrayList();

    public Camera playerCamera;//camera to render player FPS view
    public float cameraDetectDist=10;
    public float clickableDist = 1.0f;

	// initialize all controllers
	void Start () {

        if (playerCamera == null)
        {
            Debug.Log("ERROR: player camera is not set");
            return;
        }

        rigidBodyFPSController = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        guiContorller = new GuiController();
        guiContorller.initialize(playerCamera);

    }
	
	// (main routine) all controllers are controlled here
	void Update () {

        if (playerCamera == null)
        {
            Debug.Log("ERROR: player camera is not set");
            return;
        }

        //check user input events
        bool mouseLeftButton = Input.GetMouseButtonUp(0);
        bool mouseRightButton = Input.GetMouseButtonUp(1);

        //====================================
        //*** GAME ENVIRONMENT CHECK START ***
        //====================================

        //menu mode controller is executed when user touch right mouse button
        if (mouseRightButton==true) changeMenuMode();

        //control cursor mode on the game window
        if (menuModeFlg==true)
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }

        //====================================
        //*** GAME OBJECT CHECK START ***
        //====================================

        GameObject detectedGameObject = null;

        //check game object is clicked in front of the player
        detectedGameObject = checkFrontObject(mouseLeftButton);
        if(detectedGameObject != null)
        {
            //Debug.Log("check routine activated ->" + detectedGameObject.tag);
            
            //check object type

            //if it is a Item, get name and destroy Gameobject
        }



    }

    //gui controller
    void OnGUI()
    {
        guiContorller.controlGui();
    }

    //I'll probably use this method later
    private void checkGameControlObjects()
    {
        foreach (GameObject obj in gameControllerObjects)
        {

        }
    }

    //menu mode is changed when user click the mouse right button
    private void changeMenuMode()
    {
        if (menuModeFlg == true)
        {
            setMenuWindowOff();
        }
        else
        {
            setMenuWindowOn();
        }
    }
    
    public void setMenuWindowOn()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        menuModeFlg = true;
        rigidBodyFPSController.menuModeFlg = true;
        //set gui console mode to defaultMenu mode
        guiContorller.setMenuConsoleMode(1);
    }
    public void setMenuWindowOff()
    {
        menuModeFlg = false;
        rigidBodyFPSController.menuModeFlg = false;
        guiContorller.setMenuConsoleMode(0);
    }
    
   //control detected object
    private GameObject checkFrontObject(bool mouseLeftButton)

    {
        string imagePath = "Images/UI_Touch";

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit, cameraDetectDist))
        {
            Transform objectHit = hit.transform;
            //Debug.Log("distance(" + objectHit.tag + ")" + hit.distance);
            if (hit.distance < clickableDist)
            {
                guiContorller.setViewCenterImage(imagePath);
                if (mouseLeftButton) return objectHit.gameObject;
            }
            else
            {
                //guiContorller.setViewCenterImage("Images/hand_b");
                guiContorller.setViewCenterImage("");
            }
        }
        //Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        return null;
    }

    //===============================================
    //following methods are getter/setter
    //===============================================
    public GuiController getGuiController()
    {
        return guiContorller;
    }
    
}
