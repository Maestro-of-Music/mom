using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class PianoControl : MonoBehaviour {

	public int Sequence; //measure change 
   
    public GameObject Metronume;
	public GameObject GameManager;
	public Text Score;
	public Text ScoreDisplay;

	public int Mode;
	public string pitch; 
	public string Target_pitch;

    public string [] temp_answer;
    public string [] answer; 


	public int duration; 
	public float speed; 

	public bool move; //move when practice mode
	public bool Play; //play mode
	public bool Practice; 
	public bool Reset; //reset piano
	public bool Repeat;
    public bool end;
	public bool Scorechange;

	public int Repeat_Count; 
  
    public int Tempnote = 0;
    public int CurrentNote = 0;

	public int Target_noteCount = 0;
	public int Player_noteCount = 0;
    public int Player_Life = 100;


	//seperate two kind of mode and partial repeat move (using sequence) 
    private Vector3 _location;

	void Awake(){

		transform.position = new Vector3 (0, -4, 0);
		speed = 0;
		Repeat_Count = 0;
		Sequence = 0; 

	}

	//restart
 	public void InitDevice(){
        //AudioListener.pause = false;
		transform.position = new Vector3 (0, -4, 0); //TURN  
  		//InitPosition ();
		pitch = "";

		InitSetMode (Mode); 
		gameObject.GetComponent<ScoreManager> ().InitScore ();
		Score.text = gameObject.GetComponent<ScoreManager> ().score.ToString();


		androidSetting ();
		StartCoroutine ("PlayPiano");
	}
	
	void Update () {
		SelectMode ();
		NodeCheck ();  //IOManager send Data 
	}

	public void initStartMeasure(int measure_index){
		Sequence = measure_index;
	}

	public void InitPosition(){
		transform.position = new Vector3 (0, -4, 0);
	}

	public void LoadVelocity(){
		speed = GameManager.gameObject.GetComponent<GameTime> ().Velocity;
        GameManager.GetComponent<GameTime>().Setting_Tempo_data();
	}


	void PositionCheck(){
		float temp = transform.position.y;
		temp = Mathf.CeilToInt(temp);
		transform.position = new Vector3 (transform.position.x, temp, transform.position.z);
	}

	IEnumerator SetPitchLength(){
	
		if (Target_pitch.Length >= 3) {
			Target_noteCount = Target_pitch.Length / 3;
			Debug.Log ("Target Note Count : " + Target_noteCount);
		} else {
			Target_noteCount = 1; //counting keyboard Target pitch
		}
        try
        {
            if (pitch.Length >= 3)
            {
                Player_noteCount = pitch.Length  / 3;
                Debug.Log("My Note Count : " + Player_noteCount);
            }
            else
            {
                Player_noteCount = 1;
            }
        }catch(Exception){
            pitch = "";
        }


        temp_answer = pitch.Split('/');
        try
        {
            answer = Target_pitch.Split('/');
        }catch(Exception){
            
        }
//        Debug.Log("Answer : " + answer[0]);

		yield return null;
	}

	void PitchCheck(){
        
        if (pitch != "")
        {
            StartCoroutine("SetPitchLength");
            try
            {
                if (pitch.Contains("/")) //check there is no pitch inputs
                {
                    //StartCoroutine("SetPitchLength");

                    if (Target_noteCount == Player_noteCount)
                    {
                        // two objects crash 
                        Debug.Log("!!!!!!!Correct!!!!");

                        //Add arraylist -> duration and pitch

                        int count = 0;
                        for (int i = 0; i < answer.Length; i++)
                        {

                            if (temp_answer.Contains(answer[i]))
                            {
                                count++;
                            }
                        }

//                      Debug.Log(Tempnote);

                      //  if (count == answer.Length)
                       // {
                            Debug.Log("Really Correct!");
                            _location = transform.position;

                            //check point which calculate score
                            if (Practice)
                              StartCoroutine("ScoreManager",Target_noteCount);
                            else if (Play || Repeat)
                            {
                                //checking note 
                                if (Tempnote != CurrentNote)
                                {
                                    ScoreDisplay.text = "";
                                    Scorechange = true;
                                    StartCoroutine("ScoreManager",Target_noteCount);
                                    Tempnote = CurrentNote;
                                }
                            }
                            move = true;
                       // }
                    }
                    else
                    {
                        StartCoroutine("ResetPitch");
                    }
                }
            }
            catch (Exception e)
            {
                print("no input" + e);
            }
            StartCoroutine("ResetPitch");
        }
       
	}

	IEnumerator ResetPitch(){
		yield return new WaitForSeconds (0.3f);
//		Debug.Log ("Delete pitch data");
		pitch = "";
		StopCoroutine ("ResetPitch");
	}

	bool RestCheck(){
		if (Target_pitch == "-1") {
			return true;
		} else {
			return false;
		}
	}

	public void EndCheck(){
		//end check needed
		if (Play == true || Practice == true) {
			speed = 0.0f;
			move = false;
			GameManager.gameObject.GetComponent<MenuManager> ().GameEnd ();
		}
		else if (Repeat) {
			if (Repeat_Count < gameObject.GetComponent<RepeatControl> ().count) {
				if (transform.position.y >= gameObject.GetComponent<RepeatControl> ().Last_position.y) {
					Repeat_Count++;
					gameObject.GetComponent<RepeatControl> ().Reset_Sequence ();
					Score.text = "";
				}
			}
			else if(gameObject.GetComponent<RepeatControl> ().count!= 0 && Repeat_Count == gameObject.GetComponent<RepeatControl> ().count) {
				Debug.Log ("Repeat End!");
				end = true;
                //AudioListener.pause = true;
				GameManager.gameObject.GetComponent<MenuManager> ().GameEnd ();
			}
		}
	
	}

	void MoveCheck (){

		if (move) {
			//restore now location and compare future location
			if(Practice) {
				StartCoroutine("MovePiano");
			}

		} else {
			PositionCheck ();
			if (Mode == 2) { //when practice mode is opened
				Scorechange = true;
			}
		}
	}
		
	void NodeCheck(){


		if (pitch != "") {
			//divider needed 
			PitchCheck (); //check
        }
	}

	void NodeRest(){
		if (RestCheck()) {
			Debug.Log ("Rest!");
			StartCoroutine ("SetRestPosition");
			pitch = "-1";
			move = true;
		}
	}

	void InitSetMode(int num){

		switch (num) {

		case 1: 
			Debug.Log ("Play Mode");
			Scorechange = false;
			break;

		case 2:
			Debug.Log ("Practice Mode");
			Scorechange = true;

			break;
		case 3:
			Debug.Log ("Repeat Mode");
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
                EndCheck();
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

		if (!Repeat) {	 
			InitDevice (); //if play mode or practice mode init device
			this.Reset = false;
		}
		else {
			//if Repeat mode 
			InitDevice ();
			this.Repeat_Count = 0 ;
			this.Reset = false;
		}
			

		yield return new WaitForSeconds (3);
	}

	IEnumerator MovePiano(){
//        Debug.Log("location Y:!!!!!!!" + _location.y);
 //       Debug.Log("Usual : " + _location.y + duration);
        float a = (float)(_location.y + duration);
   //     Debug.Log("UnUsual : " + a);

		if (transform.position.y > a) {
            
            yield return new WaitForSeconds(0.1f);
			pitch = null;
			move = false;
		} else {
			transform.Translate (Vector3.up * Time.deltaTime * speed);
		}
        //yield return null;

	}

	IEnumerator SearchSequence(){
		transform.position = gameObject.GetComponent<RepeatControl> ().Start_position;
		yield return new WaitForSeconds(4);
	}

	IEnumerator PlayPiano(){
		yield return new WaitForSeconds (1);
		LoadVelocity (); 
	}

	IEnumerator ScoreManager(int count){

		int temp = 0;

		if (Scorechange) {
			temp = this.duration;
			gameObject.GetComponent<ScoreManager> ().score += temp * 100;
            Debug.Log("Count :" + count);
            gameObject.GetComponent<LogManager>().logcount++;
            gameObject.GetComponent<LogManager>().MakeLogObject(count); //make Log

			Scorechange = false;
		}

		ScoreDisplay.text ="+ : " + temp * 100;
		Score.text = gameObject.GetComponent<ScoreManager> ().GetScore ().ToString (); //upload score

		yield return new WaitForSeconds(1);
	}

	IEnumerator WaitTime(int seconds){
		yield return new WaitForSeconds (seconds);
	}
		
}