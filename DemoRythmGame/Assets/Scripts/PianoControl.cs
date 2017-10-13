using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoControl : MonoBehaviour {

	public int Sequence;
	private float Target_Sequence; //height 

	public GameObject Metronume;
	public GameObject CurrentObject;
	public GameTime currentSquence;
	public string pitch; 
	public string Target_pitch;
	public int duration; 
	public float speed; 

	public bool move; //move when practice mode
	public bool Play; //play mode
	public bool Practice; 
	public bool Reset; //reset piano

	//seperate two kind of mode and partial repeat move (using sequence) 

	private Vector3 _location;

	// Use this for initialization
	void Start () {
		InitDevice ();
	}

	//restart

	void InitDevice(){
		
		Sequence = 0; 
		speed = 0;
	
		currentSquence = CurrentObject.GetComponent<GameTime> ();
		transform.position = new Vector3 (0, 0, 0);
		pitch = "";

		Reset = false;
		InitSetMode (2);

		move = false;
		androidSetting ();
		StartCoroutine ("PlayPiano");
	}
	
	// Update is called once per frame
	void Update () {
		KeyInput ();
		SelectMode ();
		EndCheck (); 
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
			move = true;
		}

		else {
			move = false;
			Debug.Log ("Wrong Key!");
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
		
		if (Sequence >= CurrentObject.gameObject.GetComponent<LoadData> ().notedatas.Length) {
			speed = 0.0f;
			move = false;
		} 
	
	}


	void MoveCheck (){

		if (move) {
			//restore now location and compare future location
			//MovePiano (_location);
			StartCoroutine("MovePiano");
		} else {
			PositionCheck ();
		}
	}

	void KeyInput(){
		//Key press input

		/* White Key Input */ 
		if (Input.GetKeyDown (KeyCode.A)) {
			pitch = "4C";

		} // Do 
		else if (Input.GetKeyDown (KeyCode.S)) {
			pitch = "4D";

		} // Re
		else if (Input.GetKeyDown (KeyCode.D)) {
			pitch = "4E";

		} // Me 
		else if (Input.GetKeyDown (KeyCode.F)) {
			pitch = "4F";

		} // Fa 
		else if (Input.GetKeyDown (KeyCode.G)) {
			pitch = "4G";

		} // Sol 
		else if (Input.GetKeyDown (KeyCode.H)) {
			pitch = "4A";

		} // Ra
		else if (Input.GetKeyDown (KeyCode.J)) {
			pitch = "4B";

		} // Si

		/* Black Key Input */ 
		else if (Input.GetKeyDown (KeyCode.Q)) {
			pitch = "4C#";

		} // Do# 
		else if (Input.GetKeyDown (KeyCode.W)) {
			pitch = "4D#";

		} // Re#
		else if (Input.GetKeyDown (KeyCode.E)) {
			pitch = "4F#";

		} // Fa# 
		else if (Input.GetKeyDown (KeyCode.R)) {
			pitch = "4G#";

		} // Sol# 
		else if (Input.GetKeyDown (KeyCode.T)) {
			pitch = "4A#";

		} // Ra# 


		NodeCheck ();
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
			break;

		case 2:
			Debug.Log ("Practice Mode");
			Play = false;
			Practice = true;
			break;
		}

	}

	void SelectMode(){

		PlayMode ();
		ResetMode ();
	
	}

	void LoadVelocity(){
		speed = Metronume.gameObject.GetComponent<Metronume> ().Velocity;

	}

	void PlayMode(){
		
		if (Practice) {
			Play = false;

			MoveCheck (); 
			NodeRest ();

		} else if (Play) {
			Practice = false;
			movePiano ();
		} 

	}

	void ResetMode(){
		if (Reset) {
			StartCoroutine ("ResetPiano");
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
	*/

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
			move = false;
		} else {
			transform.Translate (Vector3.up * Time.deltaTime * speed);
		}

		yield return new WaitForSeconds (1);
	}

	IEnumerator SearchSequence(int num){

		string tag = "Note";

		GameObject[] find = GameObject.FindGameObjectsWithTag (tag);

		if (!NoteTagObjectFind (find, num)) {
			tag = "Rest";

			find = GameObject.FindGameObjectsWithTag (tag);
			NoteTagObjectFind (find, num);
		} 

		Sequence = (num - 1) ; //modifiy now location sequence
		Debug.Log(Sequence);
		transform.position = new Vector3 (transform.position.x, (transform.position.y + (Target_Sequence - 1.8f)), transform.position.z);

		yield return null;
	}
	IEnumerator PlayPiano(){
		yield return new WaitForSeconds (1);
		LoadVelocity (); 
	}
}
