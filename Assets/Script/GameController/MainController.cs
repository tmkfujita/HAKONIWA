using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

//===================================================
//This Class is the main controller for HAKONIWA game
//what is checking every interaction of game objects.
//===================================================

public class MainController : MonoBehaviour {

    private RigidbodyFirstPersonController rigidBodyFPSController;
    private GuiController guiController;//rendering controller of game items
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
    private GameItemEventExecutor gameItemEventExecutor;

	// initialize all controllers
	void Start () {

        if (playerCamera == null)
        {
            Debug.Log("ERROR: player camera is not set");
            return;
        }

        //init gui
        rigidBodyFPSController = GameObject.Find("RigidBodyFPSController").GetComponent<RigidbodyFirstPersonController>();
        guiController = new GuiController();
        guiController.initialize(playerCamera,this);

        //init object manager
        if (gameObjectManager == null)
        {
            gameObjectManager = GameObject.Find("GameObjectManager");
        }
        gameItemManager = gameObjectManager.GetComponent<GameItemManager>();
        gameSoundManager = gameObjectManager.GetComponent<GameSoundManager>();
        gameItemEventExecutor = gameObjectManager.GetComponent<GameItemEventExecutor>();
        gameItemEventExecutor.initialize(guiController, gameSoundManager, gameItemManager);


        musicSoundSource = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        gameSoundManager.addMusicAudioSource(musicSoundSource);
        walkSoundSource = rigidBodyFPSController.GetComponent<AudioSource>();
        gameSoundManager.addSoundEffectAudioSource(walkSoundSource);


        //初回テキスト
        ArrayList itemTextArr = new ArrayList();
        itemTextArr.Add("さて、今日も居眠りこいてる問題児をたたき起こしてやりましょうか。");
        itemTextArr.Add("てか、ずいぶんと今回は暑いなぁ、ふう・・・。");
        itemTextArr.Add("毎度毎度、わけのわからない夢ばかり見やがって、\nたまにはプールサイドで美女と戯れる夢でも見てくりゃいいのに・・・・。");
        itemTextArr.Add("「HAKONIWA」\nマウスの視点移動とキーボードの「W,A,S,D」入力でプレイヤーを操作できます\n画面中央に手のアイコンが表示されている状態で左クリックすると対象に触れることができます");
        guiController.setGameDialogueText(itemTextArr);
        
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
        //Debug.Log("isreading text->" + guiController.isReadingText);
        if (guiController.isReadingText == true && keyboardSpaceKey==true)
        {
            guiController.readNextText();
        }

        //====================================
        //*** GAME OBJECT CHECK START ***
        //====================================

        GameObject detectedGameObject = null;
        string eventExecuteCode = "";

        //check game object is clicked in front of the player
        detectedGameObject = checkFrontObject(mouseLeftButton);
        if(detectedGameObject != null)
        {
            //Debug.Log("check routine activated ->" + detectedGameObject.tag);

            //check object type
            if (detectedGameObject.tag == "gameItemObject")
            {
                //player have to finish reading
                if (guiController.isReadingText == true) return;

                GameItem item = detectedGameObject.GetComponent<GameItem>();
                ArrayList itemTextArr = new ArrayList();
                if (item.getItemId() != null && item.getItemInfo() != null)
                {
                    if (!item.isAlreadyTouched())
                    {
                        gameItemManager.addItemId(item.getItemId());
                        item.touchIt();
                        eventExecuteCode = item.getEventExecuteCode();
                    }
                    itemTextArr.Add(item.getItemInfo());
                    guiController.setGameDialogueText(itemTextArr);

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
                        eventExecuteCode = item.getEventExecuteCode();
                    }
                    itemTextArr.Add(item.getItemInfo());
                    guiController.setGameDialogueText(itemTextArr);

                }
            }

            //if it have an event execute message
            if(eventExecuteCode != "" || eventExecuteCode != null)
            {
                gameItemEventExecutor.setEventCode(eventExecuteCode);
            }
        }
    }

    //gui controller
    void OnGUI()
    {
        guiController.controlGui();
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
        guiController.setMenuConsoleMode(1);
        //reset current view center image
        guiController.setViewCenterImage("");
    }
    public void setMenuWindowOff()
    {
        menuModeFlg = false;
        guiController.setMenuConsoleMode(0);
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
                guiController.setViewCenterImage(imagePath);
                if (mouseLeftButton) return objectHit.gameObject;
            }
            else
            {
                //guiController.setViewCenterImage("Images/hand_b");
                guiController.setViewCenterImage("");
            }
        }
        else
        {
            guiController.setViewCenterImage("");
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
        return guiController;
    }
    public bool getPlayerControlableFlg()
    {
        if (menuModeFlg == true || guiController.isReadingText ==true)
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
