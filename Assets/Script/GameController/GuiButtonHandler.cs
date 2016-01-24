using UnityEngine;
using System.Collections;

public class GuiButtonHandler : MonoBehaviour {

    private MainController mainContoroller;
    private GuiController guiController;
    private Canvas menuOverlayCanvas;


    private void checkInitiGUIController()
    {
        if (guiController == null)
        {
            mainContoroller = GameObject.Find("RigidBodyFPSController").GetComponent<MainController>();
            guiController = mainContoroller.getGuiController();
        }
    }

    //gui menu buttons
    public void click_gameEndButton()
    {
        checkInitiGUIController();
        guiController.setGUIButtonEvent(0);
    }

    public void click_gameEndButton_OK()
    {
        checkInitiGUIController();
        guiController.setGUIButtonEvent(1);
    }

    public void click_gameEndButton_Cancel()
    {
        checkInitiGUIController();
        guiController.setGUIButtonEvent(2);
    }
    
    public void click_menuCloseButton()
    {
        checkInitiGUIController();
        guiController.setGUIButtonEvent(3);
    }

    //item buttons 
    public void click_itemButton1()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(1);
    }
    public void click_itemButton2()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(2);
    }
    public void click_itemButton3()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(3);
    }
    public void click_itemButton4()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(4);
    }
    public void click_itemButton5()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(5);
    }
    public void click_itemButton6()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(6);
    }
    public void click_itemButton7()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(7);
    }
    public void click_itemButton8()
    {
        checkInitiGUIController();
        guiController.setItemSelectEvent(8);
    }
}
