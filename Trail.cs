using UnityEngine;
using System.Collections;

public class Trail : MonoBehaviour {
  //The script for the trails
  //Materials are used to change the colors
	public Material fade;
	public Vector3 hitOne;
	//Starting point
	public Vector3 hitTwo;
	//Ending point
	public float angle;
	private int number;
	private float scale;
	private float decayTimer = 0;
	private float startTime;
	private float endTime = 0;
	public string color;
	void Start () {
		angle = transform.eulerAngles.z;
		//The trail's angle is set during instantiation. This stores it for easy access
		if (color == "green") {
			number = Player.greenTrails;
			DelGreen ();
			//Make sure there aren't too many lines on the field
		} else {
			number = Player.blueTrails;
			DelBlue ();
			//Make sure there aren't too many lines on the field
		}
	}
	void Update () {
		transform.localScale = new Vector3 (100 * Vector2.Distance (hitOne, hitTwo), transform.localScale.y, transform.localScale.z);	
		transform.position = (hitOne - hitTwo) / 2 + hitTwo;
		//centers the trail image between the two endpoints and then stretches it out to them
		if ((Time.time - decayTimer > Choice.lingerTime) && (decayTimer != 0))
			Destroy (this.gameObject);
			//Deletes old faded trails
	}	
	public void DelGreen() {
		GameObject[] lines;
		lines = GameObject.FindGameObjectsWithTag("GreenTrail");
		if (lines.Length > 2) {
			foreach(GameObject line in lines ){
				if(line.GetComponent<Trail>().number == number - 2)
					line.GetComponent<Trail>().Hide ();
			}
		}
	}
	public void DelBlue() {
		GameObject[] lines;
		lines = GameObject.FindGameObjectsWithTag("BlueTrail");
		if (lines.Length > 2) {
			foreach(GameObject line in lines ){
				if(line.GetComponent<Trail>().number == number - 2)
					line.GetComponent<Trail>().Hide ();
			}
		}
	}
	public void Hide(){
	  //fades trails if the option is on. Otherwise deletes them
		if (Choice.fadeTrails) {
			this.gameObject.GetComponent<SpriteRenderer> ().material = fade;
			this.gameObject.GetComponent<SpriteRenderer> ().sortingOrder = 0;
			decayTimer = Time.time;
			this.gameObject.layer = 11;
      //sets a timer to delete the faded trail
		} else
			Destroy (this.gameObject);
	}
}
