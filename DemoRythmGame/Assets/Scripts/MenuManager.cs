using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public Button Reset_btn;
	public Button Play_btn;
	public Button Repeat_btn;
	public Button Practice_btn;
	public Button Pause_btn; //start, repeat button pause
	public Button Restart_btn;
	public Button Home_btn;

	public Text Music_Title;
	public Text Title;
	public Text Score;

	public Text CurrentTime;
	private int Mode;

	public GameObject PianoManager;
	public GameObject GameManager;

	public GameObject UIPanel;
	public GameObject TempoPanel;
	public GameObject ScorePanel;
	public GameObject TimerPanel;
	public GameObject RepeatPanel;



	public float TimeSize;
	public bool TurnTimer;

	// Use this for initialization
	void Awake(){
		GameMenu ();
	}

	void Start () {
	//	GameMenu ();
		TurnTimer = false;
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
		UIPanel.SetActive (true);
		Reset_btn.gameObject.SetActive (false);
		Restart_btn.gameObject.SetActive (false); //reload scene

		Title.text = "Select Menu";

		this.Reset_btn.onClick.AddListener (()=>{
			PianoManager.gameObject.GetComponent<PianoControl>().Reset = true;
		});

		this.Play_btn.onClick.AddListener (() => {
			Mode = 1;
			GameStart(Mode);
		});

		this.Repeat_btn.onClick.AddListener (() => {
			Mode = 2;
			GameStart(Mode);
		});

		this.Practice_btn.onClick.AddListener (() => {
			Mode = 3;
			GameStart(Mode);
		});

		this.Home_btn.onClick.AddListener (() => {
			GoHome();
		});

		this.Restart_btn.onClick.AddListener (() => {
			SceneManager.LoadScene ("Test");

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
		Title.text = "End";
		Music_Title.gameObject.SetActive (false);
		Score.text = "Total Score : " + PianoManager.gameObject.GetComponent<ScoreManager> ().score.ToString();

		Restart_btn.gameObject.SetActive (true); //reload scene
		Play_btn.gameObject.SetActive (false);
		Practice_btn.gameObject.SetActive (false);
		Repeat_btn.gameObject.SetActive (false);
		Pause_btn.gameObject.SetActive (false);
		Reset_btn.gameObject.SetActive (false);

        //save user's data
        PianoManager.GetComponent<LogManager>().CollectLogObject(int.Parse(PianoManager.GetComponent<PianoControl>().Score.text),GameManager.GetComponent<LoadData>().noteinfo.Title);

		if (Mode == 2 || Mode == 3) {
			Score.gameObject.SetActive (false);
			PianoManager.gameObject.GetComponent<PianoControl> ().Repeat = false;

		}else{
			Score.gameObject.SetActive (true);
		}

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
			Reset_btn.gameObject.SetActive (true);
			Pause_btn.gameObject.SetActive (true);

			ScorePanel.SetActive (true);

		} else if (Mode == 2) {
			Debug.Log ("Practice Start");
			StartCoroutine ("StartInitMode");

			PianoManager.gameObject.GetComponent<PianoControl> ().Practice = true;
			Reset_btn.gameObject.SetActive (true); //reset button Activated
			TempoPanel.SetActive (true);
			TempoPanel.gameObject.transform.localPosition = new Vector3 (305, 90, 0);
			TempoPanel.gameObject.transform.localScale = new Vector3 (1.9f, 1.9f, 1.6f);

		}

		else if (Mode == 3) {
			Debug.Log ("Repeat Start");

			PianoManager.gameObject.GetComponent<PianoControl>().Repeat = true;
			//Reset_btn.gameObject.SetActive (true);
			//Pause_btn.gameObject.SetActive (true);

			//Tempo Change
			TempoPanel.SetActive(true);
			RepeatPanel.SetActive (true);

		}

	}

	public void GoHome(){
		Debug.Log ("Go Home Menu");
		SceneManager.LoadScene ("Test");
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
				//ModeStart (); //start Mode (Play, Repeat)

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
