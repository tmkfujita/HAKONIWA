using UnityEngine;
using System.Collections;

public class GameItem : MonoBehaviour {

    public string itemId;
    public string itemName;
    public string itemInfo;
    public string eventExecuteCode="";
    public bool gettableObjectFlg;
    public bool isTouchOnceObjectFlg;

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

    public string getEventExecuteCode()
    {
        return eventExecuteCode;
    }

    public void touchIt()
    {
        touchFlg = true;
        checkDestory();
    }
    

    public bool isAlreadyTouched()
    {
        return touchFlg;
    }

    private void checkDestory()
    {
        if (isTouchOnceObjectFlg == true)
        {
            Destroy(this.gameObject);
        }
    }

}
