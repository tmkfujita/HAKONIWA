using UnityEngine;
using System.Collections;

public class ViewController : MonoBehaviour {

    public GameObject mainCamera;
    public bool menuModeFlg = false;
    public float cameraDetectDist=10;
    public float clickableDist = 1.0f;


    private GuiController guiContorller;
    private Camera playerCamera;
    private RaycastHit hit;

	// Use this for initialization
	void Start () {
        try
        {
            guiContorller =GetComponent<GuiController>();

            if (mainCamera != null)
            {
                playerCamera = mainCamera.GetComponent<Camera>();
            }
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
            menuModeFlg = true;
            //set gui console mode to defaultMenu mode
            guiContorller.setMenuConsoleMode(1);
        }

        if (menuModeFlg) return;

        if (playerCamera != null)
        {
            checkFrontObject(mouseLeftButton);
        }
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
}
