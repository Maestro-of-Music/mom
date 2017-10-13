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
		pitch = "";

		move = false;

		androidSetting ();
		StartCoroutine ("PlayPiano");
	}
	
	// Update is called once per frame
	void Update () {
		KeyInput ();
		SelectMode ();
		//movePiano();
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
			_location = transform.position;
			move = true;
		}
	}

	void SelectMode(){

		if (Practice) {
			Play = false;

			MoveCheck (); 
			NodeRest ();

		} else if (Play) {
			Practice = false;

			movePiano ();
		} 

	}

	void LoadVelocity(){
		speed = Metronume.gameObject.GetComponent<Metronume> ().Velocity;

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

	IEnumerator PlayPiano(){
		yield return new WaitForSeconds (1);
		LoadVelocity (); 
	}
}
