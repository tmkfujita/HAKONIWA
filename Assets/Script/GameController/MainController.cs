using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

//===================================================
//This Class is the main controller for HAKONIWA game
//what is checking every interaction of game objects.
//===================================================

public class MainController : MonoBehaviour {

    private RigidbodyFirstPersonController rigidBodyFPSController;
    private GuiController guiContorller;//rendering controller of game items
    private RaycastHit hit;
    private bool menuModeFlg = false;
    private ArrayList gameControllerObjects = new ArrayList();

    public Camera playerCamera;//camera to render player FPS view
    public float cameraDetectDist=10;
    public float clickableDist = 1.0f;

    private GameObject gameObjectManager;
    private AudioSource walkSoundSource;
    private AudioSource musicSoundSource;

    private GameItemManager gameItemManager;
    private GameSoundManager gameSoundManager;

	// initialize all controllers
	void Start () {

        if (playerCamera == null)
        {
            Debug.Log("ERROR: player camera is not set");
            return;
        }

        //init gui
        rigidBodyFPSController = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        guiContorller = new GuiController();
        guiContorller.initialize(playerCamera,this);

        //init object manager
        if (gameObjectManager == null)
        {
            gameObjectManager = GameObject.Find("GameObjectManager");
        }
        gameItemManager = gameObjectManager.GetComponent<GameItemManager>();
        gameSoundManager = gameObjectManager.GetComponent<GameSoundManager>();

        musicSoundSource = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        gameSoundManager.addMusicAudioSource(musicSoundSource);
        walkSoundSource = rigidBodyFPSController.GetComponent<AudioSource>();
        gameSoundManager.addSoundEffectAudioSource(walkSoundSource);

        //初回テキスト
        ArrayList itemTextArr = new ArrayList();
        itemTextArr.Add("さて、今日も居眠りこいてる問題児をたたき起こしてやりましょうか。");
        itemTextArr.Add("てか、ずいぶんと今回は暑いなぁ、ふう・・・。");
        itemTextArr.Add("毎度毎度、わけのわからない夢ばかり見やがって、\nたまにはプールサイドで美女と戯れる夢でも見てくりゃいいのに・・・・。");
        guiContorller.setGameDialogueText(itemTextArr);

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
        bool keyboardSpaceKey = Input.GetKeyUp(KeyCode.Space);

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

        //read next text
        //Debug.Log("isreading text->" + guiContorller.isReadingText);
        if(guiContorller.isReadingText == true && keyboardSpaceKey==true)
        {
            guiContorller.readNextText();
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
            if (detectedGameObject.tag == "gameItemObject")
            {
                //player have to finish reading
                if (guiContorller.isReadingText == true) return;

                GameItem item = detectedGameObject.GetComponent<GameItem>();
                ArrayList itemTextArr = new ArrayList();
                if (item.getItemId() != null && item.getItemInfo() != null)
                {
                    if (!item.isAlreadyTouched())
                    {
                        gameItemManager.addItemId(item.getItemId());
                        item.touchIt();
                    }
                    itemTextArr.Add(item.getItemInfo());
                    guiContorller.setGameDialogueText(itemTextArr);

                }
                //Debug.Log("gameitem->"+ detectedGameObject.GetComponent<GameItem>().getItemId());
            }
            else if(detectedGameObject.tag == "clickableObject")
            {
                //Debug.Log("clickable obj");
                GameClickableObject item = detectedGameObject.GetComponent<GameClickableObject>();
                ArrayList itemTextArr = new ArrayList();
                if (item.getItemInfo() != null)
                {
                    if (!item.isAlreadyTouched())
                    {
                        item.touchIt();
                    }
                    itemTextArr.Add(item.getItemInfo());
                    guiContorller.setGameDialogueText(itemTextArr);

                }
            }

            //if it is a Item, get name and destroy Gameobject
        }
    }

    //gui controller
    void OnGUI()
    {
        guiContorller.controlGui();
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
        //set gui console mode to defaultMenu mode
        guiContorller.setMenuConsoleMode(1);
        //reset current view center image
        guiContorller.setViewCenterImage("");
    }
    public void setMenuWindowOff()
    {
        menuModeFlg = false;
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
            if (hit.distance < clickableDist &&(objectHit.gameObject.tag== "clickableObject" || objectHit.gameObject.tag == "gameItemObject"))
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
        else
        {
            guiContorller.setViewCenterImage("");
        }
        //Debug.DrawRay(ray.origin, ray.direction * 20, Color.yellow);
        return null;
    }

    //===============================================
    //following methods are getter/setter
    //===============================================
    public GuiController getGuiController()
    {
        //Debug.Log("get gui controller");
        return guiContorller;
    }
    public bool getPlayerControlableFlg()
    {
        if (menuModeFlg == true || guiContorller.isReadingText ==true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public GameItemManager getGameItemManager()
    {
        return gameItemManager;
    }
    public GameSoundManager getGameSoundManager()
    {
        return gameSoundManager;
    }
    
}
