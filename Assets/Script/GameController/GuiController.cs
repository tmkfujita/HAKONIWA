using UnityEngine;
using System.Collections;

public class GuiController{

    private Texture viewCenterTexture;
    private Rect viewCenterRectangle;
    private MenuConsoleModes.Mode menuConsoleMode;

    private Camera playerCamera;
    private Camera menuCamera;


    public void initialize(Camera pCamera)
    {
        playerCamera = pCamera;
        menuConsoleMode = MenuConsoleModes.Mode.none;
        menuCamera = GameObject.Find("MenuCamera").GetComponent<Camera>();

        int image_width = Screen.width / 26;
        int image_height = image_width;
        int image_x = Screen.width / 2 - image_width / 2;
        int image_y = Screen.height / 2 - image_height / 2;

        viewCenterTexture = null;
        viewCenterRectangle = new Rect(image_x, image_y, image_width, image_height);

    }
    
    public void setViewCenterImage(string imagePath)
    {
        try
        {
            if (imagePath == "")
            {
                viewCenterTexture = null;
            }
            else
            {
                viewCenterTexture = (Texture)Resources.Load(imagePath);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("setViewCenterImage: imagePath error");
            throw;
        }

    }
    //change menu console diplay
    // 1-> defaultMenuMode
    // 2-> itemConsoleMode
    // others-> none
    public void setMenuConsoleMode(int consoleMode)
    {
        if (consoleMode == 0)
        {
            menuConsoleMode = MenuConsoleModes.Mode.none;
            playerCamera.enabled = true;
            menuCamera.enabled = false;
        }
        else if (consoleMode == 1)
        {
            menuConsoleMode = MenuConsoleModes.Mode.defaultMode;
            playerCamera.enabled = false;
            menuCamera.enabled = true;
        }
        else if(consoleMode == 2)
        {
            menuConsoleMode = MenuConsoleModes.Mode.itemMode;
        }
        else
        {
            menuConsoleMode = MenuConsoleModes.Mode.none;
        }
    }


    public void controlGui()
    {
        updateViewCenterImage();
        updateGUIContents();
    }

    private void updateViewCenterImage()
    {
        if (viewCenterTexture != null)
        {
            GUI.DrawTexture(viewCenterRectangle, viewCenterTexture);
        }
    }

    private void updateGUIContents()
    {
        if (menuConsoleMode == MenuConsoleModes.Mode.defaultMode)
        {

        }else if(menuConsoleMode == MenuConsoleModes.Mode.itemMode)
        {

        }
        else
        {

        }
    }
}

namespace MenuConsoleModes
{
    enum Mode
    {
        none,
        defaultMode,
        itemMode
    }
}