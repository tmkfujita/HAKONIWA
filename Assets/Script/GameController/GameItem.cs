using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {

    public string itemId;
    public string itemName;
    public string itemInfo;
    public bool gettableObjectFlg;

    private bool touchFlg = false;
    
    public string getItemId()
    {
        return itemId;
    }

    public string getItemName()
    {
        return itemName;
    }
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
