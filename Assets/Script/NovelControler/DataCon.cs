using UnityEngine;
using System.Collections;

public class DataCon : MonoBehaviour {

	public static int currentLine = 0;

	public static int GetCurrentLine(){
		return currentLine;
	}

	public static void SetCurrentLine(int num){
		currentLine = num;
	}

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void MoveToNovelFunc () {
		Debug.Log ("MoveToNovelFunc");
		UnityEngine.SceneManagement.SceneManager.LoadScene("hakoniwa_game");

	}
}
