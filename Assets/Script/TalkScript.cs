using UnityEngine;
using System.Collections;
using UnityEngine.UI;  ////ここを追加////

public class TalkScript : MonoBehaviour {

	//点数を格納する変数
	public int score = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	public void Update () {
		this.GetComponent<Text>().text = "点数" + score.ToString() + "点";
	}
}