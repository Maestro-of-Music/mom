using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoControl : MonoBehaviour {

	public int Sequence;

	public GameObject Metronume;
	public GameObject GameManager;
	public GameTime currentSquence;
	public Text Score;

	public int Mode;
	public string pitch; 
	public string Target_pitch;
	public int duration; 
	public float speed; 

	public bool move; //move when practice mode
	public bool Play; //play mode
	public bool Practice; 
	public bool Reset; //reset piano
	public bool Repeat;

	public bool Scorechange;

	public int Repeat_Count; 

	//seperate two kind of mode and partial repeat move (using sequence) 

	private Vector3 _location;

	void Start () {
		InitDevice ();
	}

	//restart

	void InitDevice(){
		
		Sequence = 0; 
		speed = 0;
		Repeat_Count = 0;
		gameObject.GetComponent<ScoreManager> ().InitScore ();

		currentSquence = GameManager.GetComponent<GameTime> ();
		InitPosition ();
		pitch = "";

		Reset = false;
		Repeat = false;
		move = false;

		InitSetMode (Mode); 

		androidSetting ();
		StartCoroutine ("PlayPiano");
	}
	
	void Update () {
		SelectMode ();
		NodeCheck ();  //IOManager send Data 
		EndCheck (); 
	}

	public void InitPosition(){
		transform.position = new Vector3 (0, 0, 0);
	}

	public void LoadVelocity(){
		speed = GameManager.gameObject.GetComponent<GameTime> ().Velocity;
	}


	void PositionCheck(){
		float temp = transform.position.y;
		temp = Mathf.CeilToInt(temp);
		transform.position = new Vector3 (transform.position.x, temp, transform.position.z);
	}

	void PitchCheck(){
		
		if (Target_pitch == pitch) {
			_location = transform.position; 

			//check point which calculate score
			if (Practice)
				StartCoroutine ("ScoreManager");
			else if (Play) {
				Scorechange = true;
				StartCoroutine ("ScoreManager");
				Scorechange = false;

			}
			move = true;

		}
		else {
			move = false;
			Scorechange = false;
		}

	}

	bool RestCheck(){
		if (Target_pitch == "-1") {
			return true;
		} else {
			return false;
		}
	}

	void EndCheck(){

		if (Play == true || Practice == true) {
			if (Sequence >= GameManager.gameObject.GetComponent<LoadData> ().notedatas.Length) {
				speed = 0.0f;
				move = false;
			}
		}
		else if (Repeat) {
			if (Repeat_Count < gameObject.GetComponent<RepeatControl> ().count) {
				if (transform.position.y >= gameObject.GetComponent<RepeatControl> ().Last_position.y) {
					Repeat_Count++;
					gameObject.GetComponent<RepeatControl> ().Reset_Sequence ();
				}
			} 
			else {
				Debug.Log ("Repeat End!");
				gameObject.GetComponent<RepeatControl> ().Repeat_start = false;
			}
		}
	
	}

	void MoveCheck (){

		if (move) {
			//restore now location and compare future location
			StartCoroutine("MovePiano");
			
		} else {
			PositionCheck ();
			if (Mode == 2) { //when practice mode is opened
				Scorechange = true;
			}
		}
	}
		
	void NodeCheck(){
		
		if (pitch != "") {
			PitchCheck (); //check
		} 
	}

	void NodeRest(){
		if (RestCheck()) {
			Debug.Log ("Rest! ");
			StartCoroutine ("SetRestPosition");
			move = true;
			StartCoroutine ("MovePiano");
		}
	}

	void InitSetMode(int num){

		switch (num) {

		case 1: 
			Debug.Log ("Play Mode");
			Play = true;
			Practice = false;
			Repeat = false;
			Scorechange = false;

			break;

		case 2:
			Debug.Log ("Practice Mode");
			Play = false;
			Practice = true;
			Repeat = false;
			Scorechange = true;

			break;
		case 3:
			Debug.Log ("Repeat Mode");
			Repeat = true;
			Play = false;
			Practice = false;

			break;

		}

	}

	void SelectMode(){

		PlayMode ();
		ResetMode ();
	
	}
		
	void PlayMode(){
		
		if (Practice) {
			Play = false;
			Repeat = false;

			MoveCheck (); 
			NodeRest ();

		} else if (Play) {
			Repeat = false;
			Practice = false;
		
			movePiano ();

		}  else if (Repeat) {
			Play = false;
			Practice = false;

			if (gameObject.GetComponent<RepeatControl> ().Repeat_start == true) {
				movePiano ();
			}
		} 

	}

	void ResetMode(){
		if (Reset) {
			StartCoroutine ("ResetPiano");
		}
	}

	void movePiano(){
		transform.Translate (Vector3.up * Time.deltaTime * speed);
	
	}

	void androidSetting(){
		if (Application.platform == RuntimePlatform.Android) {
			Screen.orientation = ScreenOrientation.Landscape;
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		} //set android landscape mode
	}
		

	IEnumerator SetRestPosition(){
		_location = transform.position;

		Debug.Log ("Position : " + _location.y);

		yield return null;
	}

	IEnumerator ResetPiano(){
	
		Debug.Log ("Reset Piano position");
		InitDevice ();

		yield return new WaitForSeconds (3);
	}

	IEnumerator MovePiano(){

		if (transform.position.y >= _location.y + duration - 0.2f) {
			pitch = null;
			move = false;
		} else {
			transform.Translate (Vector3.up * Time.deltaTime * speed);
		}

		yield return new WaitForSeconds (1);
	}

	IEnumerator SearchSequence(int [] arr){
		Debug.Log ("Arr [0] : " + arr [0]);
		Debug.Log ("Arr [1] : " + arr [1]);

		Sequence = arr [0];

		transform.position = gameObject.GetComponent<RepeatControl> ().Start_position;
		gameObject.GetComponent<RepeatControl> ().Last_position = new Vector3 (0, (0 + (1.29f * (arr[1]))), 0);

		yield return new WaitForSeconds(4);
	}

	IEnumerator PlayPiano(){
		yield return new WaitForSeconds (1);
		LoadVelocity (); 
	}

	IEnumerator ScoreManager(){
	
		if (Sequence == 0)
			yield return new WaitForSeconds (5);

		int temp = 0;

		if (Scorechange) {
			temp = this.duration;
		} else {
			temp = 0;
		}
			gameObject.GetComponent<ScoreManager> ().SetScore (temp);

		Score.text = gameObject.GetComponent<ScoreManager> ().GetScore ().ToString (); //upload score

		yield return new WaitForSeconds(1);
	}

	IEnumerator WaitTime(int seconds){
		yield return new WaitForSeconds (seconds);
	}
		
}



/*
	IEnumerator DeleteBeforeNote (int num){

		GameObject[] find = GameObject.FindGameObjectsWithTag ("Note");

		for (int i = 0; i < num; i++) {
			Destroy (find [i]);
		}

		Debug.Log ("Deleted!");

		yield return new WaitForSeconds (3);
	}


	bool NoteTagObjectFind(GameObject [] find, int num){
		bool result = false;

		for (int i = 0; i < find.Length; i++) {
			Debug.Log ("Sequence :  " + find [i].gameObject.GetComponent<NoteDetail> ().sequence);	

			if (find [i].gameObject.GetComponent<NoteDetail> ().sequence == num) {
				Debug.Log ("Y position : " + find [i].gameObject.transform.position.y);	
				Target_Sequence = find [i].gameObject.transform.position.y;
				result = true;
			}
		}
		return result;
	}
	*/
/*
int NoteTagObjectFind(GameObject [] find, int num){
	int result = 0;

	for (int i = 0; i < find.Length; i++) {
		Debug.Log ("Sequence :  " + find [i].gameObject.GetComponent<NoteDetail> ().sequence);	

		if (find [i].gameObject.GetComponent<NoteDetail> ().sequence == num) {
			Debug.Log ("Y position : " + find [i].gameObject.transform.position.y);	
			Target_Sequence = find [i].gameObject.transform.position.y;
			result = true;
		}
	}
	return result;
}
*/
