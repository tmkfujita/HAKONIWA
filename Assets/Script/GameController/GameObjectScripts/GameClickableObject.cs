using UnityEngine;
using System.Collections;

public class GameClickableObject : MonoBehaviour {
    
    public string itemInfo;
    private bool touchFlg = false;
    public bool isTouchOnceObjectFlg;
    public string eventExecuteCode;

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
