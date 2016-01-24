using UnityEngine;

public class TestButtonClick : MonoBehaviour {

	public int count;

	public void ClickTest () {
		Debug.Log ("Clicked."+count.ToString());
		count++;
		Application.LoadLevel("ADV");
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