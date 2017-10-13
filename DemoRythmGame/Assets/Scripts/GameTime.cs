using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour {

	public GameObject Metronume;
	public GameObject MusicJson;
	public GameObject PianoManager;

	private Metronume MetroData;
	private LoadData JsonData; //Loading NoteInfo 

	public int currentStep;
	public int currentMeasure; 
	public int EndMeasure;
	public int Tempo; //can change tempo

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

		EndMeasure = JsonData.notedatas [JsonData.notedatas.Length-1].measureIndex;
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

	IEnumerator ChangeTempo(bool change, int num){
		Debug.Log ("Change :" + change );

		if (change) {
			Tempo_Up (num);
		} else {
			Tempo_Down (num);
		}

		yield return null;
	}

	void Tempo_Up(int num){
		
	}

	void Tempo_Down(int num){
		
	}

}
