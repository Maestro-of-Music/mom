using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public string display_ScoreTitle;

	public float TimeSize;
	public bool TurnTimer;

	// Use this for initialization
	void Awake(){
        this.filecontrol = FileControl.getInstance();
        this.scenechange = SceneChange.getInstance();
		GameMenu ();
        TurnTimer = false;

	}

	void Start () {
        Debug.Log(this.scenechange.Music_title);
        if (Application.platform == RuntimePlatform.Android)
        {
            Mode = this.scenechange.mode;
        }else{
            Mode = 1;
        }
        GameStart(Mode);
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
            
            SceneManager.LoadScene("PlayModeList");
		});

        this.More.onClick.AddListener(() =>
        {
            Debug.Log("Go to the chart");
            SceneManager.LoadScene("Chart_Scene");
        });

        this.Share.onClick.AddListener(()=>{
            // Nothing Happen
            Debug.Log("Button Clicked Share");
        });

        this.Replay.onClick.AddListener(()=>{
            SceneManager.LoadScene("PlayMode");

        });

        this.Check.onClick.AddListener(()=>{
            if(scenechange.mode == 1){
                //play mode
                SceneManager.LoadScene("PlayModeList");
            }else if(scenechange.mode == 2){
                SceneManager.LoadScene("PracticeModeList");
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

	public void GameEnd(){
		UIPanel.SetActive (true);
        Title.text = gameObject.GetComponent<CreateNote>().title; //setting music's name

		Music_Title.gameObject.SetActive (false);
		Score.text = PianoManager.gameObject.GetComponent<ScoreManager> ().score.ToString();

        ScoreTitlePanel.SetActive(false);
		Pause_btn.gameObject.SetActive (false);
        back_Btn.gameObject.SetActive (false);


        //save user's data
        PianoManager.GetComponent<LogManager>().CollectLogObject(int.Parse(PianoManager.GetComponent<PianoControl>().Score.text),GameManager.GetComponent<LoadData>().noteinfo.Title);
        CalculateScore(); // at first calculate score


		if (Mode == 2 || Mode == 3) {
			Score.gameObject.SetActive (false);
			PianoManager.gameObject.GetComponent<PianoControl> ().Repeat = false;

		}else{
			Score.gameObject.SetActive (true);
		}
	}

    void CalculateScore(){

        //cal Total_result;
        Debug.Log(PianoManager.gameObject.GetComponent<ScoreManager>().score);
        Debug.Log(gameObject.GetComponent<CreateNote>().Total_Score);

        double propotion = ((double)PianoManager.gameObject.GetComponent<ScoreManager>().score / (double)gameObject.GetComponent<CreateNote>().Total_Score) * 100.0;
        Debug.Log("propotion : " + propotion);

        if (propotion >= 0.5f){
            display_ScoreTitle = "S";
            Grade_S.SetActive(true);

            Debug.Log("Player result : " + " S ");
        }else if (propotion >= 0.3f && propotion < 0.5f){
            Debug.Log("Player result : " + " A ");
            display_ScoreTitle = "A";
            Grade_A.SetActive(true);

        }else if (propotion >= 0.1f && propotion < 0.3f){
            Debug.Log("Player result : " + " B ");
            display_ScoreTitle = "B";
            Grade_B.SetActive(true);


        }else {
            Debug.Log("Player result : " + " C ");
            display_ScoreTitle = "C";
            Grade_C.SetActive(true);

        }

        //show perfect and miss
        Debug.Log("Perfect Result : " + PianoManager.GetComponent<LogManager>().Perfect_Count);
        Debug.Log("Miss Result : " + PianoManager.GetComponent<LogManager>().Miss_Count);

        ExcellentScore.text = PianoManager.GetComponent<LogManager>().Perfect_Count.ToString();
        MissScore.text = PianoManager.GetComponent<LogManager>().Miss_Count.ToString(); 

        History temp = new History();
        temp.result_Alpha = display_ScoreTitle;
        temp.score = PianoManager.gameObject.GetComponent<ScoreManager>().score;
        temp.title = gameObject.GetComponent<CreateNote>().title;
        temp.mode = Mode;

        //save history
        this.filecontrol.SaveHistory(temp); //save adaption after

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


		if (Mode == 1) {
			Debug.Log ("Play Start");
			StartCoroutine ("StartInitMode");

			PianoManager.gameObject.GetComponent<PianoControl> ().Play = true;
            back_Btn.gameObject.SetActive (true);
			Pause_btn.gameObject.SetActive (true);

		} else if (Mode == 2) {
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
