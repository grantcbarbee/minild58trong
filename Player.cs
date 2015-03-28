using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Rigidbody2D myRigidbody;
	public static int playerScore = 0;
	public static int enemyScore = 0;
	public GameObject scoreboardPly;
	public GameObject scoreboardEny;
	public static int greenTrails = 0;
	public static int blueTrails = 0;
	void Start () {
		myRigidbody = gameObject.GetComponent<Rigidbody2D> ();
		myRigidbody.velocity = new Vector2 (0, 0);
	}
	void Update () {
		float move;
		// move stores raw input. For two players, it takes W&S only, for single it takes that or arrow keys
		if (Choice.TwoPlayer) {
			move = Input.GetAxisRaw ("PlayerOne");
		} else {
			move = Input.GetAxisRaw ("Vertical");
		}
		if((move > 0 && transform.position.y < 4.5) || (move < 0 && transform.position.y > -4.5))
			myRigidbody.velocity = new Vector2 (0, move * 5);
		else
			myRigidbody.velocity = new Vector2 (0, 0);
			//Stops paddle from moving off the screen
		scoreboardEny.GetComponent<GUIText> ().text = enemyScore.ToString ();
		scoreboardPly.GetComponent<GUIText> ().text = playerScore.ToString ();
		//Updates scoreboard
	}
}
