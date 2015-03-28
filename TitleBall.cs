using UnityEngine;
using System.Collections;

public class TitleBall : MonoBehaviour {
  //The script for the decorative balls on the titel screen. They behave slightly different than the play ones
  //For comments, see TwoBall.cs
	public GameObject trailFab;
	private Rigidbody2D myRigidbody;
	private SpriteRenderer myRender;
	public Material playerMat;
	public Material enemyMat;
	public Material neutralMat;
	private Trail trail;
	private GameObject blur;
	private float bounceAngle;
	private float foundAngle;
	private float currentAngle;
	private float magnitude;
	public string color;
	public void Start () {
		myRender = gameObject.GetComponent<SpriteRenderer> ();

		if (color == "green")
			transform.position = new Vector3 (-2, 2, 0);
		else
			transform.position = new Vector3 (2, 0, 0);
		myRigidbody = gameObject.GetComponent<Rigidbody2D> ();
		if (color == "green")
			myRigidbody.velocity = new Vector2 (5.0f, -0.1f);
		else
			myRigidbody.velocity = new Vector2 (-2.0f, 5.0f);
		ClearLines ();
	}
	void Update () {
		if(trail != null)
			trail.hitTwo = myRigidbody.position;
	}
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.CompareTag ("PlayerPaddle") || collider.gameObject.CompareTag ("EnemyPaddle") || collider.gameObject.CompareTag ("Wall") || collider.gameObject.CompareTag ("GreenTrail") || collider.gameObject.CompareTag ("BlueTrail")){
			if (collider.gameObject.CompareTag ("PlayerPaddle")) {
				myRigidbody.velocity = new Vector2 ((-1.1f * myRigidbody.velocity.x), myRigidbody.velocity.y);
				if (myRigidbody.velocity.x > 10)
					myRigidbody.velocity = new Vector2 (10, myRigidbody.velocity.y);
			} else if (collider.gameObject.CompareTag ("EnemyPaddle")) {
				myRigidbody.velocity = new Vector2 ((-1.1f * myRigidbody.velocity.x), myRigidbody.velocity.y);
				if (myRigidbody.velocity.x < -10)
					myRigidbody.velocity = new Vector2 (-10, myRigidbody.velocity.y);
			} else if (collider.gameObject.CompareTag ("Wall")) {
				if(myRigidbody.velocity.x >= 3 || myRigidbody.velocity.x <= -3)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, -1 * myRigidbody.velocity.y);
				else if(myRigidbody.velocity.x < 3 && myRigidbody.velocity.x > 0)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x + 0.5f, -1 * myRigidbody.velocity.y);
				else if(myRigidbody.velocity.x > -3 && myRigidbody.velocity.x < 0)
					myRigidbody.velocity = new Vector2(myRigidbody.velocity.x - 0.5f, -1 * myRigidbody.velocity.y);
			}else if (collider.gameObject.CompareTag ("GreenTrail") || collider.gameObject.CompareTag ("BlueTrail")) {
				magnitude = Mathf.Sqrt((myRigidbody.velocity.x * myRigidbody.velocity.x) + (myRigidbody.velocity.y * myRigidbody.velocity.y));
				foundAngle = collider.GetComponent<Trail>().angle;
				if(myRigidbody.velocity.y > 0){
					currentAngle = Vector2.Angle(myRigidbody.velocity, new Vector2(1,0));
				}else{
					currentAngle = 360 - Vector2.Angle(myRigidbody.velocity, new Vector2(1,0));
				}
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
				float rad = bounceAngle * (Mathf.PI / 180);
				myRigidbody.velocity = new Vector2(magnitude * Mathf.Cos(rad), magnitude * Mathf.Sin(rad));
				ClearLines();
			}
			if(trail != null){
				trail.hitTwo = myRigidbody.position;
			}
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
		}
	}
	void ClearLines() {
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
