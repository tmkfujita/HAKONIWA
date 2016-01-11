using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class ViewController : MonoBehaviour {

    public Camera playerCamera;
    public bool menuModeFlg = false;
    public float cameraDetectDist=10;
    public float clickableDist = 1.0f;

    private GuiController guiContorller;
    private RaycastHit hit;

    private RigidbodyFirstPersonController rigidBodyFPSController;

	// Use this for initialization
	void Start () {
        try
        {
            guiContorller = new GuiController();

            if (playerCamera != null)
            {
                guiContorller.initialize(playerCamera);
            }

            rigidBodyFPSController = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        }
        catch (System.Exception)
        {
            Debug.Log("error: player camera is not detected");
            throw;
        }
    }
	
	// Update is called once per frame
	void Update () {

        bool mouseLeftButton = Input.GetMouseButtonUp(0);
        bool mouseRightButton = Input.GetMouseButtonUp(1);

        if (mouseRightButton)
        {
            triggerMenuMode();
        }

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
        if (playerCamera != null)
        {
            checkFrontObject(mouseLeftButton);
        }
	}

    void OnGUI()
    {
        guiContorller.controlGui();
    }

    private void triggerMenuMode()
    {
        if (menuModeFlg == true)
        {
            setMenuModeOff();
        }
        else
        {
            setMenuModeOn();
        }
    }

    public void setMenuModeOn()
    {
        //Debug.Log("menu mode on");
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        menuModeFlg = true;
        rigidBodyFPSController.menuModeFlg = true;
        //set gui console mode to defaultMenu mode
        guiContorller.setMenuConsoleMode(1);
    }
    public void setMenuModeOff()
    {
        //Debug.Log("menu mode off");
        menuModeFlg = false;
        rigidBodyFPSController.menuModeFlg = false;
        guiContorller.setMenuConsoleMode(0);
    }

   //control detected object
    private void checkFrontObject(bool mouseLeftButton)

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

                //Debug.Log("clickable mode on:"+ mouseLeftButton);
                if (mouseLeftButton)
                {
                    Debug.Log("grep objecct->" + objectHit.tag);
                }
            }
            else
            {
                //guiContorller.setViewCenterImage("Images/hand_b");
                guiContorller.setViewCenterImage("");
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
    }

    public GuiController getGuiController()
    {
        return guiContorller;
    }
}
