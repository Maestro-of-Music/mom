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
        /*
        int a = 64;
        Debug.Log("Char :" + (char)a);*/
    }

	// Use this for initialization
	void Start () {
		InitObserver ();

	}

	void Update(){
		
	}

    void InitObserver()
    {
        //MetronumePlay = false;
        Current = GameManager.GetComponent<GameTime>();
    }

	void OnPianoPlay(Collider col){
		PianoManager.gameObject.GetComponent<PianoControl>().duration = col.gameObject.GetComponent<NoteDetail> ().duration;

		if (col.gameObject.tag == "Rest") {
            PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";
			//PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "/";
           // PianoManager.gameObject.GetComponent<PianoControl> ().move = true;
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


	void OnLEDChange(string pitch, int status){

		if (pitch != "") {
            //Send data 	
            pitch += "," + status.ToString();
  //          Debug.Log("OnLEDChange : " + pitch);

//			Debug.Log ("LED_SEND started! Pitch : "  + pitch);
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

            result = ResultCheck(_pitch, octave);

		} else {
			//normal

			char[] _datas = pitch.ToCharArray ();
			int octave = (int)(_datas[0]-'0');

			string _pitch = pitch.Substring(1).ToString();			

            result = ResultCheck(_pitch, octave);
		}

        Debug.Log("Result : " + result);

        return result;
	}

    int ResultCheck(string _pitch, int octave){
        int result = 0;

        switch (octave)
        {
            case 2:
                Debug.Log("2 Octave");

                if (_pitch == "C")
                {
                    result = 1;
                }
                else if (_pitch == "C#")
                {
                    result = 2;
                }
                else if (_pitch == "D")
                {
                    result = 3;
                }
                else if (_pitch == "D#")
                {
                    result = 4;
                }
                else if (_pitch == "E")
                {
                    result = 5;
                }
                else if (_pitch == "F")
                {
                    result = 6;
                }
                else if (_pitch == "F#")
                {
                    result = 7;
                }
                else if (_pitch == "G")
                {
                    result = 8;
                }
                else if (_pitch == "G#")
                {
                    result = 9;
                }
                else if (_pitch == "A")
                {
                    result = 10;
                }
                else if (_pitch == "A#")
                {
                    result = 11;
                }
                else if (_pitch == "B")
                {
                    result = 12;
                }

                break;
            case 3:
                Debug.Log("3 Octave");

                if (_pitch == "C")
                {
                    result = 13;
                }
                else if (_pitch == "C#")
                {
                    result = 14;
                }
                else if (_pitch == "D")
                {
                    result = 15;
                }
                else if (_pitch == "D#")
                {
                    result = 16;
                }
                else if (_pitch == "E")
                {
                    result = 17;
                }
                else if (_pitch == "F")
                {
                    result = 18;
                }
                else if (_pitch == "F#")
                {
                    result = 19;
                }
                else if (_pitch == "G")
                {
                    result = 20;
                }
                else if (_pitch == "G#")
                {
                    result = 21;
                }
                else if (_pitch == "A")
                {
                    result = 22;
                }
                else if (_pitch == "A#")
                {
                    result = 23;
                }
                else if (_pitch == "B")
                {
                    result = 24;
                }
                break;
            case 4:
                Debug.Log("4 Octave");

                if (_pitch == "C")
                {
                    result = 25;
                }
                else if (_pitch == "C#")
                {
                    result = 26;
                }
                else if (_pitch == "D")
                {
                    result = 27;
                }
                else if (_pitch == "D#")
                {
                    result = 28;
                }
                else if (_pitch == "E")
                {
                    result = 29;
                }
                else if (_pitch == "F")
                {
                    result = 30;
                }
                else if (_pitch == "F#")
                {
                    result = 31;
                }
                else if (_pitch == "G")
                {
                    result = 32;
                }
                else if (_pitch == "G#")
                {
                    result = 33;
                }
                else if (_pitch == "A")
                {
                    result = 34;
                }
                else if (_pitch == "A#")
                {
                    result = 35;
                }
                else if (_pitch == "B")
                {
                    result = 36;
                }

                break;
            case 5:
                Debug.Log("5 Octave");

                if (_pitch == "C")
                {
                    result = 37;
                }
                else if (_pitch == "C#")
                {
                    result = 38;
                }
                else if (_pitch == "D")
                {
                    result = 39;
                }
                else if (_pitch == "D#")
                {
                    result = 40;
                }
                else if (_pitch == "E")
                {
                    result = 41;
                }
                else if (_pitch == "F")
                {
                    result = 42;
                }
                else if (_pitch == "F#")
                {
                    result = 43;
                }
                else if (_pitch == "G")
                {
                    result = 44;
                }
                else if (_pitch == "G#")
                {
                    result = 45;
                }
                else if (_pitch == "A")
                {
                    result = 46;
                }
                else if (_pitch == "A#")
                {
                    result = 47;
                }
                else if (_pitch == "B")
                {
                    result = 48;
                }
                break;
        }

        return result;
    }


	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Note_0") {
            Debug.Log("Note  !! Sended! and Effect On!");

            if (col.gameObject.GetComponent<NoteDetail>().duration < 3)
            {
                col.gameObject.GetComponent<OnEffect>().OnCollision();
            }
            /*
            KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
			OnPianoPlay (col);
			OnLEDChange (col.gameObject.GetComponent<NoteDetail> ().pitch, 1);
            */
        }
        else if (col.gameObject.tag == "Note"){
            KeySequence(col.gameObject.GetComponent<NoteDetail>().pitch, true); //pressed
            OnPianoPlay(col);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch, 1);
        } 
        else if (col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false); //released
			OnPianoPlay (col);
        } else if (col.gameObject.tag =="Note_1" ){
            Debug.Log("Note 1 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch,1);

            if (col.gameObject.GetComponent<NoteDetail>().duration > 3)
            {
                col.gameObject.GetComponent<OnEffect>().OnCollision();
            }

        }else if (col.gameObject.tag =="Note_2"){
            Debug.Log("Note 2 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch,1);

        }else if(col.gameObject.tag =="Note_3"){
            Debug.Log("Note 3 !! Send Release ");
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch, 0);

            if (col.gameObject.GetComponent<NoteDetail>().duration > 3)
            {
                col.gameObject.GetComponent<OnEffect>().OffCollision();
            }
        }

		if (col.gameObject.tag == "End") {
			Debug.Log ("End!");
			if (col.gameObject.GetComponent<NoteDetail> ().pitch == "End") {
				Debug.Log ("End!");
			}
			PianoManager.gameObject.GetComponent<PianoControl> ().EndCheck ();
		}
	}


    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Note_0")
        {
            KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
            //OnPianoPlay (col);
            //OnLEDChange (col.gameObject.GetComponent<NoteDetail> ().pitch, 1);
        }
    }


    void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Note" || col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false ); //release
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";

            if(col.gameObject.GetComponent<NoteDetail>().pitch.Contains("#")){
                Destroy(col.gameObject);
            }
		}

        else if (col.gameObject.tag == "Note_0")
        {
            KeySequence(col.gameObject.GetComponent<NoteDetail>().pitch, false); //pressed
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
           // Debug.Log("Find : " + KeyBoard.gameObject.name);
			OnTagChange(KeyBoard);

            if (pitch != "")
            {
                ColoringKeyBoard(Clicked, pitch);
                //string count
            }

		}
		catch(Exception){ //non selected 
			OnRelease(pitch);
		}/*
		finally{

			if (pitch != "") {
				ColoringKeyBoard (Clicked,pitch);
				//string count
			}
		
		}*/
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

        string [] _pitch = pitch.Split(',');

		int led = OnSetLED (_pitch[0]);

        string result = "";

        if(_pitch[1] == "1"){
            //LED On!
            result = "!" + led.ToString();

        }else{
            //LED Off!
            result = "@" + led.ToString();
        }

        Debug.Log("SendLED : " + result);

		IOManagerCtrl.gameObject.GetComponent<IOManager>().KeyOutput(result);

		yield return null;

	}
}
