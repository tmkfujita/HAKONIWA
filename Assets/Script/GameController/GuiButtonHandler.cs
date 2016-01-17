using UnityEngine;
using System.Collections;

public class GuiButtonHandler : MonoBehaviour {

    private ViewController viewContoroller;
    private GuiController guiController;
    private Canvas menuOverlayCanvas;

    private bool gameEndCheckerFlg = false;

	// Use this for initialization
	void Start () {
        viewContoroller = GameObject.Find("RigidBodyFPSController").GetComponent<ViewController>();
        //guiController = viewContoroller.getGuiController();
        menuOverlayCanvas = GameObject.Find("menuOverlayCanvas").GetComponent<Canvas>();
        menuOverlayCanvas.planeDistance = 30;
    }

    public void click_gameEndButton()
    {
        if (gameEndCheckerFlg) return;

        Debug.Log("game end button clicked");
        //set dialogue
        gameEndCheckerFlg = true;
        dialogueCanvasOnOff(true);
    }

    public void click_gameEndButton_OK()
    {
        Debug.Log("game stop->"+ gameEndCheckerFlg);
        if (!gameEndCheckerFlg) return;
        gameEndCheckerFlg = false;

        //tmp
        dialogueCanvasOnOff(true);
    }
    public void click_gameEndButton_Cancel()
    {
        Debug.Log("canceled->"+gameEndCheckerFlg);
        if (!gameEndCheckerFlg) return;
        gameEndCheckerFlg = false;
        dialogueCanvasOnOff(false);
    }

    public void dialogueCanvasOnOff(bool dialogueOn)
    {
        if (dialogueOn == true)
        {
            menuOverlayCanvas.planeDistance = 12;
        }
        else
        {
            menuOverlayCanvas.planeDistance = 30;
        }
    }

    public void click_menuCloseButton()
    {
        if (gameEndCheckerFlg) return;

        Debug.Log("menu close button clicked");
        viewContoroller.setMenuWindowOff();
    }
}
