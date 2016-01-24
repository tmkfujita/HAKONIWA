using UnityEngine;
using System.Collections;

public class GameItemManager : MonoBehaviour {

    private ArrayList itemIdList = new ArrayList();

    public void addItemId(string id)
    {
        if (itemDuplicationCheck(id) == false)
        {
            itemIdList.Add(id);
            Debug.Log("item added->" + id);
        }
    }
    private bool itemDuplicationCheck(string inputId)
    {
        foreach (string id in itemIdList)
        {
            if (id == inputId) return true;
        }
        return false;
    }

    public void removeItemID(string id)
    {
        itemIdList.Remove(id);
    }

    public ArrayList getItemIdList()
    {
        return itemIdList;
    }

    //itemId handling functions
    public string getItemImagePath(string id)
    {
        return "gameItems/gameItemImage/" + id;
    }
    public string getItemTextPath(string id)
    {
        return "gameItems/gameItemText/" + id;
    }

    public ArrayList getItemTextArr(string id)
    {
        string path = "gameItems/gameItemText/" + id; 
        ArrayList textArr = new ArrayList();

        return textArr;
    }
}
