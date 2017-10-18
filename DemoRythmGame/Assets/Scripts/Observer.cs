using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Observer : MonoBehaviour {


	public GameObject PianoManager;
	//public GameObject Metronume;
	public GameObject IOManagerCtrl;
	public GameObject GameManager;
	public GameObject KeyBoard;
	public GameTime Current;
	private string Key = "Key_";


	//public bool MetronumePlay;

	void Awake(){
	}

	// Use this for initialization
	void Start () {
		InitObserver ();
	}

	void Update(){
		/*
		if (MetronumePlay) {
			MoveObserver (); 
		}
		*/
	}
	/*
	public void OnMetronume(){
	
		if (this.MetronumePlay == false) {
			Metronume.gameObject.GetComponent<Metronume> ().StartMetronome (); //piano player move
			this.MetronumePlay = true; 
		} else {
			this.MetronumePlay = false;
		}
	}
	*/

	void InitObserver(){
		//MetronumePlay = false;
		Current = GameManager.GetComponent<GameTime> ();
	}

	/*
	void MoveObserver(){
		int Move = 0;   

		if (step != Current.currentStep) {
			Move = 1; 
		} else if (Current.currentStep == 0) {
		    Move = 0;
			Debug.Log ("Music End! ");
		}

		transform.position = new Vector3 (transform.position.x, transform.position.y + Move, transform.position.z);

		step = Current.currentStep;
	}
	*/

	void OnPianoPlay(Collider col){
		PianoManager.gameObject.GetComponent<PianoControl>().duration = col.gameObject.GetComponent<NoteDetail> ().duration;

		if (col.gameObject.tag == "Rest") {
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "-1";
		} else {
			PianoManager.gameObject.GetComponent<PianoControl>().Target_pitch = col.gameObject.GetComponent<NoteDetail> ().pitch;
		}
		PianoManager.gameObject.GetComponent<PianoControl> ().Sequence++;
		PianoManager.gameObject.GetComponent<PianoControl> ().move = false;

	}


	void OnTagChange(GameObject col){ //to keyboard
		if (col.gameObject.tag == "NonClicked") {
			col.gameObject.tag = "Clicked";
		} else {
			col.gameObject.tag = "NonClicked";
		}
	}


	void OnLEDChange(string pitch){

		if (pitch != "") {
			//Send data 	
			Debug.Log ("LED_SEND started! Pitch : "  + pitch);
			StartCoroutine ("SendLED", pitch);
		}
	}

	int OnSetLED(string pitch){

		int result = 0;

		if (pitch.Length > 2) {
			//alter
			return result;

		} else {
			//normal

			char[] _datas = pitch.ToCharArray ();
			int octave = (int)(_datas[0]-'0');
			char _pitch = _datas [1];

			Debug.Log ("Octave : " + octave);
			Debug.Log ("Pitch : " + _pitch);

			switch (octave) {
			case 3:
				Debug.Log ("3 Octave");
				break;
			case 4:
				Debug.Log ("4 Octave");
					
				if (_pitch == 'C') {
					result = 1;
				} else if (_pitch == 'D') {
					result = 2;

				} else if (_pitch == 'E') {
					result = 3;

				} else if (_pitch == 'F') {
					result = 4;

				} else if (_pitch == 'G') {
					result = 5;

				} else if (_pitch == 'A') {
					result = 6;

				} else if (_pitch == 'B') {
					result = 7;
				}

				break;
			case 5:
				Debug.Log ("4 Octave");
				break;
			}

			return result;
		}
	}


	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Note") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
			OnPianoPlay (col);

			OnLEDChange (col.gameObject.GetComponent<NoteDetail> ().pitch);
			 

		} else if (col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false); //released
			OnPianoPlay (col);
		}
	}

	void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Note" || col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false ); //release
		}
	}

	public void OnRelease(){
		//release every Keyboard right now 
		
		GameObject [] ClickedObjs = GameObject.FindGameObjectsWithTag ("Clicked");

		for (int i = 0; i < ClickedObjs.Length; i++) {
			//Debug.Log ("Clicked Objects : " + i); 
			ClickedObjs [i].gameObject.GetComponent<Renderer> ().
			material.color = Color.white;
		}

	}

	void KeySequence(string pitch, bool Clicked){

		try //selected objects
		{
			KeyBoard = GameObject.Find (Key + pitch);
			OnTagChange(KeyBoard);
		}
		catch(Exception){ //non selected 
			OnRelease();
		}
		finally{

			if (pitch != "") {
				ColoringKeyBoard (Clicked);
			}
		
		}
	}

	void ColoringKeyBoard(bool Clicked){
		if (Clicked == true) {
			KeyBoard.GetComponent<Renderer> ().material.color = Color.yellow;
		} else {
			KeyBoard.GetComponent<Renderer> ().material.color = Color.white;
			IOManagerCtrl.gameObject.GetComponent<IOManager> ().send = false;
		}
	}

	IEnumerator SendLED(string pitch){

		int led = OnSetLED (pitch);

		/*
		string result = "";
		if (led == 5) {
			result = "1";
		} else {
			result = "0";
		}
		*/

		IOManagerCtrl.gameObject.GetComponent<IOManager>().KeyOutput(led.ToString());

		yield return null;

	}
}
