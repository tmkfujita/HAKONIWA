using UnityEngine;
using System.Collections;

public class MoveToNovel : MonoBehaviour
{

	public void MoveToNovelFunc () {
		Debug.Log ("MoveToNovelFunc");
//		Application.LoadLevel("ADV");
		UnityEngine.SceneManagement.SceneManager.LoadScene("hakoniwa_game");

	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

