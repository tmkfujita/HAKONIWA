using UnityEngine;
using System.Collections;

public class GuiButtonHandler : MonoBehaviour {

    private ViewController viewContoroller;
    private GuiController guiController;

	// Use this for initialization
	void Start () {
        viewContoroller = GameObject.Find("RigidBodyFPSController").GetComponent<ViewController>();
        guiController = viewContoroller.getGuiController();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void click_gameEndButton()
    {
        Debug.Log("game end button clicked");
    }
    public void click_menuCloseButton()
    {
        Debug.Log("menu close button clicked");

    }
}
