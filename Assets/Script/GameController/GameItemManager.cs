using UnityEngine;
using System.Collections;

public class GameItemManager : MonoBehaviour {

    private ArrayList itemIdList = new ArrayList();

    public void addItemId(string id)
    {
        if (itemIdList.Contains(id) == false)
        {
            itemIdList.Add(id);
            Debug.Log("item added->" + id);
        }
    }

    public void removeItemID(string id)
    {
        if(itemIdList.Contains(id)==true) itemIdList.Remove(id);
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
        //read it

        return textArr;
    }
}
