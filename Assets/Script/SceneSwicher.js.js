#pragma strict

function Start () {

}

function Update () {

}

function OnMouseUpAsButton() {
    Debug.Log('Now scene is ' + Application.loadedLevelName);
    Application.LoadLevel('scene2');
}