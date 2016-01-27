using UnityEngine;
using System.Collections;

public class MenuButtonClick : MonoBehaviour {


	public int count;

	public void MenuClick () {
		Debug.Log ("MenuClicked."+count.ToString());
		count++;

	}

	public void MoveToNovel () {
		Debug.Log ("moveToNovel."+count.ToString());
		count++;
		Application.LoadLevel("ADV");
	}


	void Start()
	{
		count = 0;
	}

}
