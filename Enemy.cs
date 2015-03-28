using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Rigidbody2D myRigidbody;
	private float move;
	private bool ballInPlay;
	GameObject ball;
	void Start () {
		myRigidbody = gameObject.GetComponent<Rigidbody2D> ();
		myRigidbody.velocity = new Vector2 (0, 0);
	}
	void Update () {
		FindBall ();
		if (Choice.TwoPlayer) {
			move = Input.GetAxisRaw("PlayerTwo");
			// gets raw input for player two
		}else{
		  // simulates raw input based on whether the ball is above or below the paddle
			if (Mathf.Abs ( ball.transform.position.y - this.transform.position.y) < 0.1f)
				move = 0;
			else if (ball.transform.position.y < this.transform.position.y)
				move = -1;
			else if (ball.transform.position.y > this.transform.position.y)
				move = 1;
		}
		if((move > 0 && transform.position.y < 4.5) || (move < 0 && transform.position.y > -4.5))
			myRigidbody.velocity = new Vector2 (0, move * 5);
		else
			myRigidbody.velocity = new Vector2 (0, 0);
			//Stops the paddles from going off the screen
	}
	void FindBall() {
	  // sets the variable "ball" to the closest ball moving right
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Ball");
		Vector3 position = transform.position;
		float distOne = (gos [0].transform.position - position).sqrMagnitude;
		float velOne = gos[0].GetComponent<Rigidbody2D>().velocity.x;
		float distTwo = (gos [1].transform.position - position).sqrMagnitude;
		float velTwo = gos[1].GetComponent<Rigidbody2D>().velocity.x;
		if ((velOne < 0 && velTwo < 0) || (velOne > 0 && velTwo > 0)) {
			if (distOne <= distTwo)
				ball = gos [0];
			else
				ball = gos [1];
		} else if (velOne > 0)
			ball = gos [0];
		else if (velTwo > 0)
			ball = gos [1];
	}
}
