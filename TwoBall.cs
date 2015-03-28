using UnityEngine;
using System.Collections;

public class TwoBall : MonoBehaviour {
  //The script for the balls in the play level
	public AudioClip bounce;
	public AudioClip score;
	public GameObject trailFab;
	private Rigidbody2D myRigidbody;
	private SpriteRenderer myRender;
	public Material playerMat;
	public Material enemyMat;
	public Material neutralMat;
	public bool playerServe;
	private Trail trail;
	//This Trail object represents the trail the ball is currently making
	private GameObject blur;
	private float bounceAngle;
	private float foundAngle;
	private float currentAngle;
	private float magnitude;
	public string color;
	public void Start () {
		myRender = gameObject.GetComponent<SpriteRenderer> ();
		if (color == "green")
			transform.position = new Vector3 (-2, 0, 0);
		else
			transform.position = new Vector3 (2, 0, 0);
		myRigidbody = gameObject.GetComponent<Rigidbody2D> ();
		if(playerServe)
			myRigidbody.velocity = new Vector2 (5.0f, 0);
		else
			myRigidbody.velocity = new Vector2 (-5.0f, 0);
		ClearLines ();
	}
	void Update () {
	  //If the ball is currently making a trail, update the Endpoint to be the ball's position
		if(trail != null)
			trail.hitTwo = myRigidbody.position;
	}
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.CompareTag ("PlayerPaddle") || collider.gameObject.CompareTag ("EnemyPaddle") || collider.gameObject.CompareTag ("Wall") || collider.gameObject.CompareTag ("GreenTrail") || collider.gameObject.CompareTag ("BlueTrail")){	 
		  AudioSource.PlayClipAtPoint(bounce, transform.position);
		  //If it hits anything other than a goal, play bounce audio then...
			if (collider.gameObject.CompareTag ("PlayerPaddle")) {
				myRigidbody.velocity = new Vector2 ((-1.1f * myRigidbody.velocity.x), ((Random.Range (0.4f, 0.6f) * collider.gameObject.GetComponent<Player> ().myRigidbody.velocity.y) + myRigidbody.velocity.y));
				//If it is a player paddle, reverse x velocity, then check to make sure that it is less than ten (the collisions can get pretty buggy at high speeds)
				if (myRigidbody.velocity.x > 10)
					myRigidbody.velocity = new Vector2 (10, myRigidbody.velocity.y);
			} else if (collider.gameObject.CompareTag ("EnemyPaddle")) {
			  //If it is an enemy paddle, reverse x velocity, then check to make sure that it is less than ten (the collisions can get pretty buggy at high speeds). If it is a stationary AI, slightly alter y velocity (serves the ball better)
				if(collider.gameObject.rigidbody2D.velocity.y == 0 && (!Choice.TwoPlayer))
					myRigidbody.velocity = new Vector2 ((-1.1f * myRigidbody.velocity.x), (Random.Range (-1f, 1f) + myRigidbody.velocity.y));
				else
					myRigidbody.velocity = new Vector2 ((-1.1f * myRigidbody.velocity.x), ((Random.Range (0.4f, 0.6f) * collider.gameObject.GetComponent<Enemy> ().myRigidbody.velocity.y) + myRigidbody.velocity.y));
				if (myRigidbody.velocity.x < -10)
					myRigidbody.velocity = new Vector2 (-10, myRigidbody.velocity.y);
			} else if (collider.gameObject.CompareTag ("Wall")) {
			  //If it is a wall (top or bottom of screen), reverse y velocity. If x velocity is less than 3, speed it up some (prevents those agonizingly slow almost vertical bounces)
				if(myRigidbody.velocity.x >= 3 || myRigidbody.velocity.x <= -3)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -1 * myRigidbody.velocity.y);
				else if(myRigidbody.velocity.x < 3 && myRigidbody.velocity.x > 0)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + 0.5f, -1 * myRigidbody.velocity.y);
				else if(myRigidbody.velocity.x > -3 && myRigidbody.velocity.x < 0)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x - 0.5f, -1 * myRigidbody.velocity.y);
			}else if (collider.gameObject.CompareTag ("GreenTrail") || collider.gameObject.CompareTag ("BlueTrail")) {
			  //Here is where it gets complicated. First, it finds the magnitude of the velocity vector using pythagorean theorem
				magnitude = Mathf.Sqrt((myRigidbody.velocity.x * myRigidbody.velocity.x) + (myRigidbody.velocity.y * myRigidbody.velocity.y));
				//Then it pulls in the angle of the trail it collided with
				foundAngle = collider.GetComponent<Trail>().angle;
				//Vector2.Angle returns the smallest angle between the two vectors. This turns it into a number from 0-360 instead of 0-180
				if(myRigidbody.velocity.y > 0){
					currentAngle = Vector2.Angle(myRigidbody.velocity, new Vector2(1,0));
				}else{
					currentAngle = 360 - Vector2.Angle(myRigidbody.velocity, new Vector2(1,0));
				}
				//Convert the trail's angle to first and second quadrant only, then apply some complicated checks to determine the bounce angle
				if(foundAngle > 180)
					foundAngle -= 180;
				float range = currentAngle - foundAngle;
				if(range == 0 || range == 360)
					bounceAngle = currentAngle;
				else if (range == 90)
					bounceAngle = foundAngle + 90;
				else if (range > 0 && range < 180)
					bounceAngle = foundAngle - range;
				else if (range == 180)
					bounceAngle = currentAngle;
				else if (range == 270)
					bounceAngle = foundAngle - 90;
				else if (range > 180 && range < 360)
					bounceAngle = 360 + foundAngle - range;
				//Convert bounce angle into radians (VERY IMPORTANT, wasted hours trying to fix the bouncing system, when it turned out the method wanted radians)
				float rad = bounceAngle * (Mathf.PI / 180);
				//Using the magnitude of velocity and our new angle, set the vertical and horizontal components
				myRigidbody.velocity = new Vector2(magnitude * Mathf.Cos(rad), magnitude * Mathf.Sin(rad));
				ClearLines();
			}
			//If the ball collides with pretty much anything, set its trails second position to the ball's position one last time
			if(trail != null)
				trail.hitTwo = myRigidbody.position;
			//Then add one to the trail count and spawn a new trail, pointing in the direction of the ball's velocity
			if(color == "green")
				Player.greenTrails++;
			else
				Player.blueTrails++;
			if(myRigidbody.velocity.y > 0)
				blur = (GameObject)Instantiate (trailFab, myRigidbody.position, Quaternion.Euler (0, 0, Vector2.Angle(myRigidbody.velocity, new Vector2(1, 0))));
			else
				blur = (GameObject)Instantiate (trailFab, myRigidbody.position, Quaternion.Euler (0, 0, -Vector2.Angle(myRigidbody.velocity, new Vector2(1, 0))));
			blur.GetComponent<SpriteRenderer> ().material = myRender.material;
			if(color == "green")
				blur.layer = 14;
			else
				blur.layer = 15;
			trail = blur.GetComponent<Trail>();
			trail.color = color;
			trail.hitOne = myRigidbody.position;
			//Set the new trail's layers, colors, and update the object "trail" to refer to it. Also set it's start position to be the ball's position
			
		}else if (collider.gameObject.CompareTag ("PlayerScore")) {
		  //Pretty straightforward score stuff. Calls Start to reset the ball
			AudioSource.PlayClipAtPoint(score, transform.position);
			Player.playerScore++;
			playerServe = true;
			if(trail != null)
				trail.hitTwo = myRigidbody.position;
			trail = null;
			Start ();
		}else if (collider.gameObject.CompareTag ("EnemyScore")) {
			AudioSource.PlayClipAtPoint(score, transform.position);
			Player.enemyScore++;
			playerServe = false;
			if(trail != null)
				trail.hitTwo = myRigidbody.position;
			trail = null;
			Start ();
		} 
	}
	void ClearLines() {
	  //Finds all of the trails on the screen and has them run Hide(), which will either fade or delete them, depending on the current game settings
		GameObject[] lines;
		lines = GameObject.FindGameObjectsWithTag("GreenTrail");
		if (lines.Length != 0) {
			foreach(GameObject line in lines ){
				if(line.gameObject.layer != 11)
					line.GetComponent<Trail>().Hide ();
			}
		}
		lines = GameObject.FindGameObjectsWithTag("BlueTrail");
		if (lines.Length != 0) {
			foreach(GameObject line in lines ){
				if(line.gameObject.layer != 11)
					line.GetComponent<Trail>().Hide ();
			}
		}
		Player.greenTrails = 0;
		Player.blueTrails = 0;
	}
}
