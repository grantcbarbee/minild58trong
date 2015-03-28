using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
  //Script for title screen GUI. Mostly buttons
	public GameObject image;
	private string trail;
	private float linger = 1;
	void Start(){
		float xFactor = Screen.width / 1920f;
		float yFactor = Screen.height  / 1080f;
		Camera.main.rect=new Rect(0,0,1,xFactor/yFactor);
	}
	void Update(){
		if (Choice.fadeTrails)
			trail = "ON";
		else
			trail = "OFF";
	}
	void OnGUI(){
		GUIStyle centerStyle = GUI.skin.GetStyle ("Label");
		centerStyle.alignment = TextAnchor.UpperCenter;
		GUI.Label (new Rect (1 * Screen.width / 4, 11 * Screen.height / 16, Screen.width / 2, Screen.height / 12), "Arrow Keys or W&S to move. R resets balls, Esc for menu.", centerStyle);
		if (GUI.Button (new Rect (3 * Screen.width/8, 12 * Screen.height/16, Screen.width/4, Screen.height/24), "Single Player")) {
			Choice.TwoPlayer = false;
			Application.LoadLevel(1);
		}
		if (GUI.Button (new Rect (3 * Screen.width/8, 13 * Screen.height/16, Screen.width/4, Screen.height/24), "Two Player")) {
			Choice.TwoPlayer = true;
			Application.LoadLevel(1);
		}
		if (GUI.Button (new Rect (3 * Screen.width/8, 14 * Screen.height/16, Screen.width/4, Screen.height/24), "Faded Trails: " + trail)) {
			Choice.fadeTrails = !Choice.fadeTrails;
		}
		if (Choice.fadeTrails) {
			linger = GUI.HorizontalSlider (new Rect (5 * Screen.width/8, (14 * Screen.height/16) + 10, 3 * Screen.width/8, 20), linger, 1, 10);
			GUI.Button (new Rect (5 * Screen.width/8, 14 * Screen.height/16, 3 * Screen.width/8, Screen.height/24), "Seconds: " + ((int)linger).ToString ());
			Choice.lingerTime = (int)linger;
		}
		if (GUI.Button (new Rect (3 * Screen.width/8, 15 * Screen.height/16, Screen.width/4, Screen.height/24), "Quit")) {
			Application.Quit();
		}
	}
}
