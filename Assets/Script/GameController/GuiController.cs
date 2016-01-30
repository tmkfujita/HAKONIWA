using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuiController{

    private MainController mainController;
    private Texture viewCenterTexture;
    private Rect viewCenterRectangle;
    private MenuConsoleModes.Mode menuConsoleMode;

    private Camera playerCamera;
    private Camera menuCamera;

    //dialogue text
    private Text gameDialogueText;
    private Text gameDialogueTextSub;
    public bool isReadingText;
    private int readingCounter;
    private ArrayList currentReadingText;
    private int colorChangeCounter = 0;

    //menu gui parametors
    private Canvas menuCanvas;
    private Canvas menuOverlayCanvas;
    private bool gameEndCheckerFlg = false;
    private const int GAMEEND_DIALOGUE_INACTIVE = 30;
    private const int GAMEEND_DIALOGUE_ACTIVE = 10;

    private Text gameItemInfoText;
    private string selectedItemId = "";

    private bool gameEndTextFlg = false;

    public void initialize(Camera pCamera, MainController vController)
    {
        mainController = vController;
        playerCamera = pCamera;

        menuConsoleMode = MenuConsoleModes.Mode.none;
        menuCamera = GameObject.Find("MenuCamera").GetComponent<Camera>();

        int image_width = Screen.width / 30;
        int image_height = image_width;
        int image_x = Screen.width / 2 - image_width / 2;
        int image_y = Screen.height / 2 - image_height / 2;

        viewCenterTexture = null;
        viewCenterRectangle = new Rect(image_x, image_y, image_width, image_height);

        //init gameCanvas
        gameDialogueText = GameObject.Find("gameDialogueText").GetComponent<Text>();
        gameDialogueTextSub = GameObject.Find("gameDialogueTextSub").GetComponent<Text>();
        isReadingText = false;

        //init item info Text
        gameItemInfoText = GameObject.Find("itemInfoText").GetComponent<Text>();

        //initialize menu overlay canvas
        menuCanvas = GameObject.Find("menuCanvas").GetComponent<Canvas>();

        menuOverlayCanvas = GameObject.Find("menuOverlayCanvas").GetComponent<Canvas>();
        menuOverlayCanvas.planeDistance = GAMEEND_DIALOGUE_INACTIVE;
        gameEndCheckerFlg = false;

        controlCanvasActivation(menuCanvas, false);
        controlCanvasActivation(menuOverlayCanvas, false);

    }
    
    //update GUI objects
    public void controlGui()
    {
        updateViewCenterImage();
        updateGUIContents();
    }
    //draw current center image
    private void updateViewCenterImage()
    {
        if (viewCenterTexture != null)
        {
            GUI.DrawTexture(viewCenterRectangle, viewCenterTexture);
        }
    }

    //for update gui images(now not in use)
    private void updateGUIContents()
    {
        if (isReadingText == true)
        {
            colorChangeCounter = colorChangeCounter + 1;
            if (colorChangeCounter > 159)
            {
                if (colorChangeCounter == 160)
                {
                    gameDialogueTextSub.color = Color.black;
                }
                if (colorChangeCounter == 168)
                {
                    gameDialogueTextSub.color = Color.white;
                }
                if (colorChangeCounter == 178)
                {
                    gameDialogueTextSub.color = Color.black;
                }
                if (colorChangeCounter == 186)
                {
                    gameDialogueTextSub.color = Color.white;
                }
                if (colorChangeCounter > 200)
                {
                    colorChangeCounter = 0;
                }
            }
        }

    }

    //control player center image
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
    //change menu console display
    public void setMenuConsoleMode(int consoleMode)
    {
        //init itempanel
        loadPlayerItems();
        loadCurrentPlayerSelectedItem();

        switch (consoleMode)
        {
            case 0://none
                menuConsoleMode = MenuConsoleModes.Mode.none;
                playerCamera.enabled = true;
                menuCamera.enabled = false;
                controlCanvasActivation(menuCanvas, false);
                controlCanvasActivation(menuOverlayCanvas, false);

                gameEndCheckerFlg = false;
                menuOverlayCanvas.planeDistance = GAMEEND_DIALOGUE_INACTIVE;

                break;
            case 1://default menu mode
                menuConsoleMode = MenuConsoleModes.Mode.defaultMode;
                playerCamera.enabled = false;
                menuCamera.enabled = true;
                controlCanvasActivation(menuCanvas, true);
                controlCanvasActivation(menuOverlayCanvas, false);
                break;
            case 2://item console mode
                menuConsoleMode = MenuConsoleModes.Mode.itemMode;
                break;
            default:
                menuConsoleMode = MenuConsoleModes.Mode.none;
                break;
        }

    }

    private void loadCurrentPlayerSelectedItem()
    {
        Image buttonImage = GameObject.Find("gameItemImage").GetComponent<Image>();
        buttonImage.sprite = Resources.Load("gameItems/gameItemImage/nodata", typeof(Sprite)) as Sprite;

        if (selectedItemId == "")
        {
            return;
        }
        else
        {
            buttonImage.sprite = Resources.Load("gameItems/gameItemImage/"+selectedItemId, typeof(Sprite)) as Sprite;
        }


    }

    private void loadPlayerItems()
    {
        ArrayList itemArr;

        resetItemPanelDatas();

        itemArr = mainController.getGameItemManager().getItemIdList();

        int imagePanelCounter = 1;
        foreach (string id in itemArr)
        {
            //Debug.Log("load item test" + id);
            string imagePath = mainController.getGameItemManager().getItemImagePath(id);

            Image buttonImage = GameObject.Find("itemButton" + imagePanelCounter).GetComponent<Image>();
            buttonImage.sprite = Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            Debug.Log("load image path->" + imagePath +"  to->itemButton"+imagePanelCounter);

            imagePanelCounter++;
        }
    }

    private void resetItemPanelDatas()
    {
        gameItemInfoText.text = "";

        for (int i = 1; i <= 8; i++)
        {
            //Debug.Log("init item images");
            Image buttonImage = GameObject.Find("itemButton" + i).GetComponent<Image>();
            buttonImage.sprite = Resources.Load("gameItems/gameItemImage/nodata", typeof(Sprite)) as Sprite;

        }
    }

    public void setItemSelectEvent(int buttonNo)
    {
        ArrayList itemArr;

        itemArr = mainController.getGameItemManager().getItemIdList();

        int itemButtonCounter = 1;
        foreach (string id in itemArr)
        {
            if(buttonNo == itemButtonCounter)
            {
                string itemTextPath = mainController.getGameItemManager().getItemTextPath(id);
                TextAsset itemText = Resources.Load(itemTextPath, typeof(TextAsset)) as TextAsset;
                //Debug.Log("load text("+ itemTextPath +")->");
                gameItemInfoText.text = itemText.text;

                //set this item
                selectedItemId = id;
                return;
            }
            itemButtonCounter++;
        }
    }

    //gui behaviors when a player clicks any menu buttons 
    public void setGUIButtonEvent(int buttonEventCode)
    {
        if (menuConsoleMode != MenuConsoleModes.Mode.defaultMode) return;
        
        switch (buttonEventCode)
        {
            case 0://Game end button clicked
                if (gameEndCheckerFlg) return;
                Debug.Log("game end button clicked");
                gameEndCheckerFlg = true;
                menuOverlayCanvas.planeDistance = GAMEEND_DIALOGUE_ACTIVE;
                controlCanvasActivation(menuCanvas, false);
                controlCanvasActivation(menuOverlayCanvas, true);
                break;
            case 1://Game end ok button clicked
                Debug.Log("game stop->" + gameEndCheckerFlg);
                if (!gameEndCheckerFlg) return;
                gameEndCheckerFlg = false;
                controlCanvasActivation(menuCanvas, true);
                controlCanvasActivation(menuOverlayCanvas, false);
                menuOverlayCanvas.planeDistance = GAMEEND_DIALOGUE_INACTIVE;
                
                UnityEngine.SceneManagement.SceneManager.LoadScene("hakoniwa_game");

                break;
            case 2://Game end cancel button clicked
                Debug.Log("canceled->" + gameEndCheckerFlg);
                if (!gameEndCheckerFlg) return;
                gameEndCheckerFlg = false;
                controlCanvasActivation(menuCanvas, true);
                controlCanvasActivation(menuOverlayCanvas, false);
                menuOverlayCanvas.planeDistance = GAMEEND_DIALOGUE_INACTIVE;
                break;
            case 3://Menu Cancel button clicked
                if (gameEndCheckerFlg) return;
                Debug.Log("menu close button clicked");
                mainController.setMenuWindowOff();
                break;
            default:

                break;
        }
    }

    //activate front canvas raycaster
    private void controlCanvasActivation(Canvas canvas, bool raycastFlg)
    {
            canvas.GetComponent<GraphicRaycaster>().enabled = raycastFlg;
    }

    public void setGameDialogueText(ArrayList textArr)
    {
        if (isReadingText == true) return;

        currentReadingText = textArr;
        Debug.Log("input text size->" + currentReadingText.Count);
        if (currentReadingText.Count >1)
        {
            isReadingText = true;
            readingCounter = 1;
            //array set
            readNextText();
        }
        else
        {
            foreach(string text in currentReadingText)
            {
                gameDialogueText.text = text;
            }

            if (gameEndTextFlg) gameEndProcess();
        }
    }

    public void readNextText()
    {
        int counter = 1;
        foreach (string text in currentReadingText)
        {
            if (counter == readingCounter)
            {
                gameDialogueText.text = text;
                readingCounter += 1;
                gameDialogueTextSub.text = "touch space key ->";
                if (currentReadingText.Count == counter)
                {
                    isReadingText = false;
                    gameDialogueTextSub.text = "";

                    if (gameEndTextFlg) gameEndProcess();
                }
                return;
            }
            counter++;
        }
    }

    public void setGameEndTextFlg()
    {
        gameEndTextFlg = true;
    }
    private void gameEndProcess()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ADV");
    }
}

//=========================================
//namespace of GUI mode
//=========================================
namespace MenuConsoleModes
{
    enum Mode
    {
        none,
        defaultMode,
        itemMode
    }
}