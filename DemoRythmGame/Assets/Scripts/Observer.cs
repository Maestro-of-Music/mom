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
	public int Keyboard_count;

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

    void InitObserver()
    {
        //MetronumePlay = false;
        Current = GameManager.GetComponent<GameTime>();
    }

	void OnPianoPlay(Collider col){
		PianoManager.gameObject.GetComponent<PianoControl>().duration = col.gameObject.GetComponent<NoteDetail> ().duration;

		if (col.gameObject.tag == "Rest") {
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";
		} else {
			PianoManager.gameObject.GetComponent<PianoControl>().Target_pitch += col.gameObject.GetComponent<NoteDetail> ().pitch + "/";
		}
		PianoManager.gameObject.GetComponent<PianoControl> ().Sequence = col.gameObject.GetComponent<NoteDetail> ().sequence;
        PianoManager.gameObject.GetComponent<PianoControl>().CurrentNote = col.gameObject.GetComponent<NoteDetail>().note_index;

		//measure change check 
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

		//Debug.Log (pitch);

		int result = 0;

		if (pitch.Length > 2) {
			//alter
			char[] _datas = pitch.ToCharArray ();
			int octave = (int)(_datas[0]-'0');

			string _pitch = new string (_datas, 1, 2);

			switch (octave) {
			case 3:
				Debug.Log ("3 Octave");
				break;
			case 4:
				Debug.Log ("4 Octave");

				if (_pitch == "C") {
					result = 1;
				}
				else if(_pitch == "C#"){
					result = 2;
				}
				else if (_pitch == "D") {
					result = 3;
				}
				else if(_pitch == "D#"){
					result = 4;
				}
				else if (_pitch == "E") {
					result = 5;

				} else if (_pitch == "F") {
					result = 6;
				} else if(_pitch == "F#"){
					result = 7;
				} else if (_pitch == "G") {
					result = 8;

				} else if(_pitch == "G#"){
					result = 9;
				} else if (_pitch == "A") {
					result = 10;
				} else if(_pitch == "A#"){
					result = 11;
				}  else if (_pitch == "B") {
					result = 12;
				}

				break;
			case 5:
				Debug.Log ("4 Octave");
				break;
			}

			return result;

		} else {
			//normal

			char[] _datas = pitch.ToCharArray ();
			int octave = (int)(_datas[0]-'0');

			string _pitch = pitch.Substring(1).ToString();			


			Debug.Log ("Octave : " + octave);
			Debug.Log ("Pitch : " + _pitch);

			switch (octave) {
			case 3:
				Debug.Log ("3 Octave");
				break;
			case 4:
				Debug.Log ("4 Octave");
					
				if (_pitch == "C") {
					result = 1;
				}else if(_pitch == "C#"){
					result = 2;
				}
				else if (_pitch == "D") {
					result = 3;
				}
				else if(_pitch == "D#"){
					result = 4;
				}
				else if (_pitch == "E") {
					result = 5;

				} else if (_pitch == "F") {
					result = 6;
				} else if(_pitch == "F#"){
					result = 7;
				} else if (_pitch == "G") {
					result = 8;

				} else if(_pitch == "G#"){
					result = 9;
				} else if (_pitch == "A") {
					result = 10;
				} else if(_pitch == "A#"){
					result = 11;
				}  else if (_pitch == "B") {
					result = 12;
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
            Debug.Log("Note 1 !! Sended!");
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
			OnPianoPlay (col);
			OnLEDChange (col.gameObject.GetComponent<NoteDetail> ().pitch);

		} else if (col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false); //released
			OnPianoPlay (col);
        } else if (col.gameObject.tag =="Note_1" ){
            Debug.Log("Note 2 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch);
        }else if (col.gameObject.tag =="Note_2"){
            Debug.Log("Note 3 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch);
        }

		if (col.gameObject.tag == "End") {
			Debug.Log ("End!");
			if (col.gameObject.GetComponent<NoteDetail> ().pitch == "End") {
				Debug.Log ("End!");
			}
			PianoManager.gameObject.GetComponent<PianoControl> ().EndCheck ();
		}
	}

	void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Note" || col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false ); //release
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";

		}
	}

	public void OnRelease(string pitch){
		//release every Keyboard right now 
		
		GameObject [] ClickedObjs = GameObject.FindGameObjectsWithTag ("Clicked");

		for (int i = 0; i < ClickedObjs.Length; i++) {

			if (ClickedObjs[i].gameObject.name.EndsWith("#")) {
				ClickedObjs [i].gameObject.GetComponent<Renderer> ().material.color = Color.black;
			} else {
				ClickedObjs [i].gameObject.GetComponent<Renderer> ().material.color = Color.white;
			}
		}

		PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";
	}


	void KeySequence(string pitch, bool Clicked){

		try //selected objects
		{
			KeyBoard = GameObject.Find (Key + pitch);
			OnTagChange(KeyBoard);
		}
		catch(Exception){ //non selected 
			OnRelease(pitch);
		}
		finally{

			if (pitch != "") {
				ColoringKeyBoard (Clicked,pitch);
				//string count

			}
		
		}
	}

	void ColoringKeyBoard(bool Clicked,string pitch){
		if (Clicked == true) {
			KeyBoard.GetComponent<Renderer> ().material.color = Color.yellow;
		} else {
			
			if (pitch.EndsWith ("#")) {
				KeyBoard.GetComponent<Renderer> ().material.color = Color.black; //Black keyboard			
			} else {
				KeyBoard.GetComponent<Renderer> ().material.color = Color.white; //White keyboard			
			}
			IOManagerCtrl.gameObject.GetComponent<IOManager> ().send = false;
		}
	}

	IEnumerator SendLED(string pitch){

		int led = OnSetLED (pitch);
		Debug.Log ("led : " + led);

		IOManagerCtrl.gameObject.GetComponent<IOManager>().KeyOutput(led.ToString());

		yield return null;

	}
}
