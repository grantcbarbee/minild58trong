using UnityEngine;
using System.Collections;

public class myGUI : MonoBehaviour {
  //This is the GUI script for the play level camera
	void Start(){
	  //Force aspect ratio to match playfield dimensions
		float xFactor = Screen.width / 1920f;
		float yFactor = Screen.height  / 1080f;
		Camera.main.rect=new Rect(0,0,1,xFactor/yFactor);
	}
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) {
		  // Reset scores and quit to menu if esc is pressed
			Player.playerScore = 0;
			Player.enemyScore = 0;
			Application.LoadLevel(0);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
		  //Just in case one of the balls escapes somehow
			Fix();
		}
	}

	void Fix(){
	  //Resets each ball
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("Ball");
		foreach (GameObject go in gos) {
			go.GetComponent<TwoBall> ().playerServe = false;
			go.GetComponent<TwoBall> ().Start ();
		}
	}
}
