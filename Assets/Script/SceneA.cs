using UnityEngine;
using System.Collections;

public class SceneA : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI () { 
		GUI.Label(new Rect(20, 40, 80, 20), "Scene A");
	}
}
