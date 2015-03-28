public class Choice : MonoBehaviour {
  //Stores the variables from the title screen options for when the scene loads.
	public static bool TwoPlayer = false;
	public static bool fadeTrails = false;
	public static int lingerTime = 1;
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}
