using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	public Text text;

	public void OnClick (int number){

		switch (number) {

		case 0:
			Debug.Log ("Test.0");
			break;
		case 1:
			Debug.Log ("Test.1");
			break;
		default:
			break;
		}

	}

}
