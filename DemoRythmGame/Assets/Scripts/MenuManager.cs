using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour {

	public Button back_Btn;
	public Button Pause_btn; //start, repeat button pause

	public Text Music_Title;
	public Text Title;
	
    public Text Score;
    public Text ExcellentScore;
    public Text MissScore;

	public Text CurrentTime;
	public int Mode;

    public GameObject Grade_S;
    public GameObject Grade_A;
    public GameObject Grade_B;
    public GameObject Grade_C;

    public Button More;
    public Button Share;
    public Button Replay;
    public Button Check;

	public GameObject PianoManager;
	public GameObject GameManager;

	public GameObject UIPanel;
	public GameObject TimerPanel;
	public GameObject RepeatPanel;
    public GameObject ScoreTitlePanel;

    private FileControl filecontrol;
    private SceneChange scenechange;
    private Bluetooth bluetooth;

    public string display_ScoreTitle;

	public float TimeSize;
	public bool TurnTimer;

	// Use this for initialization
	void Awake(){
        this.bluetooth = Bluetooth.getInstance();
        this.filecontrol = FileControl.getInstance();
        this.scenechange = SceneChange.getInstance();
		GameMenu ();
        TurnTimer = false;

	}

	void Start () {
        Debug.Log(this.scenechange.Music_title);
        if (Application.platform == RuntimePlatform.Android)
        {
            //Mode = this.scenechange.mode;
            Debug.Log("Selected Mode! : " + this.scenechange.mode);
        }

        if(Mode == 3){
            Debug.Log("End Mode!");
            GameEnd();
        }else{
            GameStart(this.scenechange.mode);        
        }
    }

	void Update(){
        
		Timer ();
	}

	public void Pause(){
		//when play mode or repeat mode ,Can pause 
		if (PianoManager.gameObject.GetComponent<PianoControl> ().Play || PianoManager.gameObject.GetComponent<PianoControl> ().Repeat) {
			Debug.Log ("Pause ! Excuvated ");

		}
	}
		
	public void GameMenu(){

        this.back_Btn.onClick.AddListener (()=>{
            Debug.Log("Go to the PlayModeList");
            if (this.scenechange.mode == 1)
            {
                SceneManager.LoadScene("5play");
            }
            else if(this.scenechange.mode == 2){
                SceneManager.LoadScene("8practice");    
            }
		});

        this.More.onClick.AddListener(() =>
        {
            Debug.Log("Go to the chart");
            SceneManager.LoadScene("12chart");
        });

        this.Share.onClick.AddListener(()=>{
            // Nothing Happen
            Debug.Log("Button Clicked Share");
        });

        this.Replay.onClick.AddListener(()=>{
            Debug.Log("Go to the replay");
            SceneManager.LoadScene("PlayMode");
        });

        this.Check.onClick.AddListener(()=>{
            Debug.Log("Go to the PlayModeList");

            if(scenechange.mode == 1){
                //play mode
                SceneManager.LoadScene("5play");
            }else if(scenechange.mode == 2){
                SceneManager.LoadScene("8practice");
            }
        });
	}

	public void GameStart(int num){
		UIPanel.SetActive (false);
		Music_Title.text = GameManager.gameObject.GetComponent<CreateNote> ().title;

		if (num == 1) {
			Debug.Log ("Play Mode Started");
			TurnTimer = true; //Timer Object Turn On

		} else if (num == 2) {
			Debug.Log ("Practice Mode Started");
			ModeStart ();

		} else if (num == 3) {
			Debug.Log ("Repeat Mode Started");
			ModeStart();

		}

	}

    public void PlayEnd(){
        //save user's data
        PianoManager.GetComponent<LogManager>().CollectLogObject(int.Parse(PianoManager.GetComponent<PianoControl>().Score.text), GameManager.GetComponent<LoadData>().noteinfo.Title);
        CalculateScore(); // at first calculate score

        if (Mode == 2 || Mode == 3)
        {
            PianoManager.gameObject.GetComponent<PianoControl>().Repeat = false;
        }

        SceneManager.LoadScene("7result");
    }

	public void GameEnd(){
		UIPanel.SetActive (true);
        Title.text = this.scenechange.Music_title;

		Music_Title.gameObject.SetActive (false);
		//Score.text = PianoManager.gameObject.GetComponent<ScoreManager> ().score.ToString();

        ScoreTitlePanel.SetActive(false);
		Pause_btn.gameObject.SetActive (false);
        back_Btn.gameObject.SetActive (false);

        LoadCalculateScore(this.scenechange.temp);
	}

    void LoadCalculateScore(History temp){
        Debug.Log("Load Calculate");

        try
        {
            if (temp.result_Alpha == "S" || temp.result_Alpha == "s")
            {
                Grade_S.SetActive(true);

            }
            else if (temp.result_Alpha == "A" || temp.result_Alpha == "a")
            {
                Grade_A.SetActive(true);

            }
            else if (temp.result_Alpha == "B" || temp.result_Alpha == "b")
            {
                Grade_B.SetActive(true);

            }
            else if (temp.result_Alpha == "C" || temp.result_Alpha == "c")
            {
                Grade_C.SetActive(true);

            }

            ExcellentScore.text = this.scenechange.Excellent_count.ToString();
            MissScore.text = this.scenechange.Miss_count.ToString();
            Score.text = temp.score.ToString();
        }catch(Exception e){
            Debug.Log("There is no History data!");
        }
    }

    void CalculateScore(){

        double propotion = ((double)PianoManager.gameObject.GetComponent<ScoreManager>().score / (double)gameObject.GetComponent<CreateNote>().Total_Score) * 100.0;

        if (propotion >= 0.5f){
            display_ScoreTitle = "S";

            Debug.Log("Player result : " + " S ");
        }else if (propotion >= 0.3f && propotion < 0.5f){
            Debug.Log("Player result : " + " A ");
            display_ScoreTitle = "A";

        }else if (propotion >= 0.1f && propotion < 0.3f){
            Debug.Log("Player result : " + " B ");
            display_ScoreTitle = "B";


        }else {
            Debug.Log("Player result : " + " C ");
            display_ScoreTitle = "C";

        }

        this.scenechange.Excellent_count = PianoManager.GetComponent<LogManager>().Perfect_Count;
        this.scenechange.Miss_count = PianoManager.GetComponent<LogManager>().Miss_Count;

        History temp = new History();
        temp.result_Alpha = display_ScoreTitle;
        temp.score = PianoManager.gameObject.GetComponent<ScoreManager>().score;
        temp.title = gameObject.GetComponent<CreateNote>().title;
        temp.mode = Mode;

        //save history
        this.filecontrol.SaveHistory(temp); //save adaption after

        this.scenechange.temp = temp; //send history data to next scene

    }

	public void ModePause(){

		if (Mode == 1) {
			Debug.Log ("Play Paused");

			if (PianoManager.gameObject.GetComponent<PianoControl> ().Play) {
				Debug.Log ("Play Paused");
				PianoManager.gameObject.GetComponent<PianoControl> ().Play = false;
			} else {
				Debug.Log ("Play Started");
				PianoManager.gameObject.GetComponent<PianoControl> ().Play = true;
			}
		}
		else if (Mode == 3) {

			if (PianoManager.gameObject.GetComponent<PianoControl> ().Repeat) {
				Debug.Log ("Repeat Paused");
				PianoManager.gameObject.GetComponent<PianoControl> ().Repeat = false;
			}else {
				Debug.Log ("Play Started");
				PianoManager.gameObject.GetComponent<PianoControl> ().Repeat = true;
			}

		}

	}

	public void ModeStart(){


        if (this.scenechange.mode == 1) {
			Debug.Log ("Play Start");
			StartCoroutine ("StartInitMode");

			PianoManager.gameObject.GetComponent<PianoControl> ().Play = true;
            back_Btn.gameObject.SetActive (true);
			Pause_btn.gameObject.SetActive (true);

        } else if (this.scenechange.mode == 2) {

			Debug.Log ("Practice Start");
			StartCoroutine ("StartInitMode");

			PianoManager.gameObject.GetComponent<PianoControl> ().Practice = true;
            back_Btn.gameObject.SetActive (true); //reset button Activated
		
		}

		else if (Mode == 3) {
			Debug.Log ("Repeat Start");

			PianoManager.gameObject.GetComponent<PianoControl>().Repeat = true;
            back_Btn.gameObject.SetActive (true);
			Pause_btn.gameObject.SetActive (true);

			//Tempo Change
			RepeatPanel.SetActive (true);

		}

	}

	public void GoHome(){
		Debug.Log ("Go Home Menu");
		SceneManager.LoadScene ("TableListScene");//check!
	}

	public void OnTimer(){
		TurnTimer = true;
		Debug.Log ("Timer On!");
	}

	public void Timer(){

		if (TurnTimer) {
			TimerPanel.SetActive (true);

			TimeSize -= Time.deltaTime;
			CurrentTime.text = "Time Left:" + Mathf.Round (TimeSize);

			if (TimeSize < 0) {
				TurnTimer = false;

				if (Mode == 3) {
					Debug.Log ("Start Repeat!");
	
					PianoManager.gameObject.GetComponent<RepeatControl> ().Get_Sequence ();

				} else if (Mode == 1) {
					ModeStart ();
				}

			}

		} else {
			TimerPanel.SetActive (false);
		}
	}

	IEnumerator StartInitMode()
	{
		PianoManager.gameObject.GetComponent<PianoControl> ().InitDevice ();

		yield return new WaitForSeconds(1); //wait for init Device
	}

}
