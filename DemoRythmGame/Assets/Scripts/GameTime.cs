using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour {

	public GameObject Metronume;
	public GameObject MusicJson;
	public GameObject PianoManager;
	public Text tempo_data;

	private Metronume MetroData;
	private LoadData JsonData; //Loading NoteInfo 

	public int currentStep;
	public int currentMeasure; 
	public int EndMeasure;
	public int Tempo; //can change tempo

	public float interval;
	public float Velocity;

	void Start(){
		SettingMetronume ();
	}

	// Update is called once per frame
	void Update () {
		LoadingMetronume ();
	}

	void SettingMetronume(){
		//setting Metronume data

		MetroData = Metronume.GetComponent<Metronume> ();
		JsonData = MusicJson.GetComponent<LoadData> ();

		MetroData.Base = JsonData.noteinfo.Beat_type;
		MetroData.Step = JsonData.noteinfo.Beats;
		MetroData.BPM = JsonData.noteinfo.Tempo;
		tempo_data.text = MetroData.BPM.ToString ();

		EndMeasure = JsonData.notedatas [JsonData.notedatas.Length-1].measureIndex;

		StartCoroutine ("Init");
	}
		
	void LoadingMetronume(){
		//loading Metronume data real-time 

		currentStep = MetroData.CurrentStep; 	// need duration checking 
		currentMeasure = MetroData.CurrentMeasure; 

		if (currentMeasure >= EndMeasure + 1) {
			currentMeasure = 0; 
			currentStep = 0;
		}
	}


	//setting indiviual speed setting on button event 

 	public void Tempo_Up(int num){

		MetroData.BPM += num;
		tempo_data.text = MetroData.BPM.ToString ();

		StartCoroutine ("Init");
	}

	public void Tempo_Down(int num){
		
		MetroData.BPM -= num;
		tempo_data.text = MetroData.BPM.ToString ();

		StartCoroutine ("Init");


	}

	public void CalVelocity(float interval){
		this.Velocity = (1 - interval) * 4;  //make velocity
		PianoManager.gameObject.GetComponent<PianoControl> ().LoadVelocity ();

		Debug.Log ("Velocity : " +  Velocity);
	}


	IEnumerator Init(){

		yield return new WaitForSeconds (1);
		//load json data until 3 seconds 

		var multiplier = MetroData.Base / 4f;
		var tmpInterval = 60f / MetroData.BPM;
		Debug.Log (tmpInterval + " " + multiplier);
		interval = tmpInterval / multiplier;

		CalVelocity (interval);
		Debug.Log ("Velocity Setting!");

		yield return null;
	}


}
