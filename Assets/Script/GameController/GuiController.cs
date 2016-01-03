using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {

    private Texture viewCenterTexture;
    private Rect viewCenterRectangle;
    private MenuConsoleModes.Mode menuConsoleMode;

	// Use this for initialization
	void Start () {
        menuConsoleMode = MenuConsoleModes.Mode.none;

        int image_width= Screen.width/26;
        int image_height= image_width;
        int image_x = Screen.width / 2 - image_width / 2;
        int image_y = Screen.height / 2 - image_height / 2;
        
        viewCenterTexture = null;
        viewCenterRectangle = new Rect(image_x,image_y,image_width,image_height);
	}
	
	// Update is called once per frame
	void Update () {
	
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
        }
        else if (consoleMode == 1)
        {
            menuConsoleMode = MenuConsoleModes.Mode.defaultMode;
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


    void OnGUI()
    {
        updateViewCenterImage();
        updateViewLabels();
    }

    private void updateViewCenterImage()
    {
        if (viewCenterTexture != null)
        {
            GUI.DrawTexture(viewCenterRectangle, viewCenterTexture);
        }
    }

    private void updateViewLabels()
    {

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