using UnityEngine;
using System.Collections;

public class GameClickableObject : MonoBehaviour {
    
    public string itemInfo;
    private bool touchFlg = false;

    public string getItemInfo()
    {
        return itemInfo;
    }

    public void touchIt()
    {
        touchFlg = true;
    }
    public bool isAlreadyTouched()
    {
        return touchFlg;
    }
}
