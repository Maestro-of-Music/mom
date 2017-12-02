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
            PianoManager.gameObject.GetComponent<PianoControl> ().move = true;
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

//			Debug.Log ("LED_SEND started! Pitch : "  + pitch);
			StartCoroutine ("SendLED", pitch);
		}
	}

	int OnSetLED(string pitch){

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

//        Debug.Log("Result : " + result);

        return result;
	}

    public int ResultCheck(string _pitch, int octave){
        int result = 0;

        switch (octave)
        {
            case 2:
                Debug.Log("2 Octave");

                if (_pitch == "C")
                {
                    result = 33;
                }
                else if (_pitch == "C#")
                {
                    result = 34;
                }
                else if (_pitch == "D")
                {
                    result = 35;
                }
                else if (_pitch == "D#")
                {
                    result = 36;
                }
                else if (_pitch == "E")
                {
                    result = 37;
                }
                else if (_pitch == "F")
                {
                    result = 38;
                }
                else if (_pitch == "F#")
                {
                    result = 39;
                }
                else if (_pitch == "G")
                {
                    result = 40;
                }
                else if (_pitch == "G#")
                {
                    result = 41;
                }
                else if (_pitch == "A")
                {
                    result = 42;
                }
                else if (_pitch == "A#")
                {
                    result = 43;
                }
                else if (_pitch == "B")
                {
                    result = 44;
                }

                break;
            case 3:
//                Debug.Log("3 Octave");
                /*
                if (_pitch == "C")
                {
                    result = 45;
                }
                else if (_pitch == "C#")
                {
                    result = 46;
                }
                else if (_pitch == "D")
                {
                    result = 47;
                }
                else if (_pitch == "D#")
                {
                    result = 48;
                }
                else if (_pitch == "E")
                {
                    result = 49;
                }
                else if (_pitch == "F")
                {
                    result = 50;
                }
                else if (_pitch == "F#")
                {
                    result = 51;
                }
                else if (_pitch == "G")
                {
                    result = 52;
                }
                else if (_pitch == "G#")
                {
                    result = 53;
                }
                else if (_pitch == "A")
                {
                    result = 54;
                }
                else if (_pitch == "A#")
                {
                    result = 55;
                }
                else if (_pitch == "B")
                {
                    result = 56;
                }*/

                if (_pitch == "C")
                {
                    result = 33;
                }
                else if (_pitch == "C#")
                {
                    result = 34;
                }
                else if (_pitch == "D")
                {
                    result = 35;
                }
                else if (_pitch == "D#")
                {
                    result = 36;
                }
                else if (_pitch == "E")
                {
                    result = 37;
                }
                else if (_pitch == "F")
                {
                    result = 38;
                }
                else if (_pitch == "F#")
                {
                    result = 39;
                }
                else if (_pitch == "G")
                {
                    result = 40;
                }
                else if (_pitch == "G#")
                {
                    result = 41;
                }
                else if (_pitch == "A")
                {
                    result = 42;
                }
                else if (_pitch == "A#")
                {
                    result = 43;
                }
                else if (_pitch == "B")
                {
                    result = 44;
                }
                break;
            case 4:
                Debug.Log("4 Octave");

                /*
                if (_pitch == "C")
                {
                    result = 57;
                }
                else if (_pitch == "C#")
                {
                    result = 58;
                }
                else if (_pitch == "D")
                {
                    result = 59;
                }
                else if (_pitch == "D#")
                {
                    result = 60;
                }
                else if (_pitch == "E")
                {
                    result = 61;
                }
                else if (_pitch == "F")
                {
                    result = 62;
                }
                else if (_pitch == "F#")
                {
                    result = 63;
                }
                else if (_pitch == "G")
                {
                    result = 64;
                }
                else if (_pitch == "G#")
                {
                    result = 65;
                }
                else if (_pitch == "A")
                {
                    result = 66;
                }
                else if (_pitch == "A#")
                {
                    result = 67;
                }
                else if (_pitch == "B")
                {
                    result = 68;
                }*/


                if (_pitch == "C")
                {
                    result = 45;
                }
                else if (_pitch == "C#")
                {
                    result = 46;
                }
                else if (_pitch == "D")
                {
                    result = 47;
                }
                else if (_pitch == "D#")
                {
                    result = 48;
                }
                else if (_pitch == "E")
                {
                    result = 49;
                }
                else if (_pitch == "F")
                {
                    result = 50;
                }
                else if (_pitch == "F#")
                {
                    result = 51;
                }
                else if (_pitch == "G")
                {
                    result = 52;
                }
                else if (_pitch == "G#")
                {
                    result = 53;
                }
                else if (_pitch == "A")
                {
                    result = 54;
                }
                else if (_pitch == "A#")
                {
                    result = 55;
                }
                else if (_pitch == "B")
                {
                    result = 56;
                }


                break;
            case 5:
//                Debug.Log("5 Octave");

                if (_pitch == "C")
                {
                    result = 69;
                }
                else if (_pitch == "C#")
                {
                    result = 70;
                }
                else if (_pitch == "D")
                {
                    result = 71;
                }
                else if (_pitch == "D#")
                {
                    result = 72;
                }
                else if (_pitch == "E")
                {
                    result = 73;
                }
                else if (_pitch == "F")
                {
                    result = 74;
                }
                else if (_pitch == "F#")
                {
                    result = 75;
                }
                else if (_pitch == "G")
                {
                    result = 76;
                }
                else if (_pitch == "G#")
                {
                    result = 77;
                }
                else if (_pitch == "A")
                {
                    result = 78;
                }
                else if (_pitch == "A#")
                {
                    result = 79;
                }
                else if (_pitch == "B")
                {
                    result = 80;
                }
                break;
        }

        return result;
    }


	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Note_0") {
         //   Debug.Log("Note  !! Sended! and Effect On!");
         
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch, 1);

        }
        else if (col.gameObject.tag == "Note"){
            
            col.gameObject.GetComponent<OnEffect>().OnCollision(); 
            //OnCollision setting
            KeySequence(col.gameObject.GetComponent<NoteDetail>().pitch, true); //pressed
            OnPianoPlay(col);

        } 
        else if (col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false); //released
			OnPianoPlay (col);
            Debug.Log("Rest!!");

        } else if (col.gameObject.tag =="Note_1" ){
//            Debug.Log("Note 1 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch,1);
            /*
            if (col.gameObject.GetComponent<NoteDetail>().duration > 3)
            {
                col.gameObject.GetComponent<OnEffect>().OnCollision();
            }
            */

        }else if (col.gameObject.tag =="Note_2"){
           // Debug.Log("Note 2 !! Sended! : " + col.gameObject.GetComponent<NoteDetail>().pitch);
            OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch,1);

        }else if(col.gameObject.tag =="Note_3"){
            //  Debug.Log("Note 3 !! Send Release ");
          //  OnLEDChange(col.gameObject.GetComponent<NoteDetail>().pitch, 0);
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
//            Debug.Log("!!!!! Note_0 stayed");
            KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
            //OnPianoPlay (col);
            //OnLEDChange (col.gameObject.GetComponent<NoteDetail> ().pitch, 1);
        }
    }


    void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Note" || col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false ); //release
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "";
          //  Destroy(col.gameObject);
            /*
            if(col.gameObject.GetComponent<NoteDetail>().pitch.Contains("#")){
                Destroy(col.gameObject);
            }*/
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
        /*
        if(_pitch[1] == "1"){
            //LED On!
            result = "!" + led.ToString();

        }else{
            //LED Off!
            result = "@" + led.ToString();
        }
        */

        result = ((char)led).ToString();
        Debug.Log("Send LED : " + result);
		IOManagerCtrl.gameObject.GetComponent<IOManager>().KeyOutput(result);

		yield return null;

	}
}
