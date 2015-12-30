using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {

    private Texture viewCenterTexture;
    private Rect viewCenterRectangle;

	// Use this for initialization
	void Start () {
        int image_width= Screen.width/15;
        int image_height= image_width;
        int image_x = Screen.width / 2 - image_width / 2;
        int image_y = Screen.height / 2 - image_height / 2;

        viewCenterTexture = (Texture)Resources.Load("Images/hand_b");
        viewCenterRectangle = new Rect(image_x,image_y,image_width,image_height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setViewCenterImage(string imagePath)
    {
        viewCenterTexture = (Texture)Resources.Load(imagePath);
    }

    void OnGUI()
    {
        updateViewCenterImage();
        updateViewLabels();
    }

    private void updateViewCenterImage()
    {

        GUI.DrawTexture(viewCenterRectangle, viewCenterTexture);
    }

    private void updateViewLabels()
    {

    }
}
