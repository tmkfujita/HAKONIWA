using UnityEngine;
using System.Collections;

public class GameItemEventExecutor : MonoBehaviour {

    private GuiController guiController;
    private GameSoundManager gameSoundManager;
    private GameItemManager gameItemManager;

    public AudioClip dark2;

    private bool wallMoveFlg = false;
    private GameObject wall;
    private GameObject firewall;
    private float wallPosLimitX = 25;

    public void initialize(GuiController gContorller, GameSoundManager gsManager,GameItemManager giManager)
    {

        wall = GameObject.Find("moveWall");
        firewall = GameObject.Find("firewall");

        wallPosLimitX = wall.transform.position.x + wallPosLimitX;

        guiController = gContorller;
        gameSoundManager = gsManager;
        gameItemManager = giManager;

        firewall.SetActive(false);
    }

    void Update()
    {
        if (wallMoveFlg==true)
        {
            moveWall();
        }
    }

    public void setEventCode(string eventCode)
    {
        ArrayList itemTextArr;
        switch (eventCode)
        {
            case "end":
                itemTextArr = new ArrayList();
                itemTextArr.Add("「ミッションコンプリート」");
                itemTextArr.Add("報告：ナギナミ機の異常ステータスが解消されました");
                itemTextArr.Add("ふう、やれやれ・・・。");
                guiController.setGameDialogueText(itemTextArr);
                guiController.setGameEndTextFlg();

                break;

            case "setConsole":
                itemTextArr = new ArrayList();
                itemTextArr.Add("ロックされてて先に進めないな");
                itemTextArr.Add("少し戻った通路から入れる小部屋のコンソールで解除しないと");
                itemTextArr.Add("たしか赤い球体に触れれば良かったんだったか…");
                guiController.setGameDialogueText(itemTextArr);
                GameObject.Find("consoleSphere").tag = "clickableObject";
                break;
            case "openDoor":

                itemTextArr = new ArrayList();
                itemTextArr.Add("「ロック解除成功」\n---セキュリティレベルが低下したためファイアーウォールを起動します");
                itemTextArr.Add("ロックが解除されて壁が動く音がする。");
                itemTextArr.Add("ファイアーウォールなんてただの迷路さっさと抜けて早くメンテに戻ろう。");
                guiController.setGameDialogueText(itemTextArr);
                changeMusic();
                wallMoveFlg = true;
                GameObject.Find("naminagi").tag = "clickableObject";
                firewall.SetActive(true);
                break;
            default:

                break;
        }
    }

    private void changeMusic()
    {
        AudioSource audioSource = GameObject.Find("MainCamera").GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = dark2;
        audioSource.Play();
    }

    private void moveWall()
    {
        Debug.Log("movewall");
        Vector3 currentWallPos = wall.transform.position;

        wall.transform.position = new Vector3(currentWallPos.x+0.1f, currentWallPos.y, currentWallPos.z);
        if ((currentWallPos.x + 0.1f) > wallPosLimitX)
        {
            Debug.Log("ok"+currentWallPos.x);
            wallMoveFlg = false;
        }
    }
}
