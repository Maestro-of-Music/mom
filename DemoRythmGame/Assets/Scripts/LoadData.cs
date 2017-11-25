using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class LoadData : MonoBehaviour {

	public TextAsset jsonData;
	public NoteInfo noteinfo;
	public NoteData [] notedatas;
	public NoteData [] backupdatas;

	public int End_Measure; //check measure in note info 

	// Use this for initialization
	void Awake () {
		StartCoroutine ("LoadJSON");
	}
		
	IEnumerator LoadJSON (){
		JsonData getData = JsonMapper.ToObject (jsonData.text);

		noteinfo = new NoteInfo ();

		LoadNoteIofo (getData);
		LoadNoteData (getData);

		Debug.Log ("Load Data!");

		yield return null;
	}

	void LoadNoteIofo(JsonData JsonObj){
	//note information	

		NoteInfo temp = new NoteInfo ();

		for (int i = 0; i < JsonObj ["NoteInfo"].Count; i++) {

			//expend the music note 
			temp.Title = JsonObj ["NoteInfo"][i]["Title"].ToString();
			temp.Tempo = int.Parse(JsonObj ["NoteInfo"][i]["Tempo"].ToString());
			temp.Measure = int.Parse(JsonObj ["NoteInfo"][i]["Measure"].ToString());
			temp.Beats = int.Parse(JsonObj ["NoteInfo"][i]["Beats"].ToString());
			temp.Beat_type = int.Parse(JsonObj ["NoteInfo"][i]["Beat_type"].ToString());
		
		}

		noteinfo = temp; //save note basic data  
	}

	void LoadNoteData(JsonData jsonObj){
	//note data 
		LoadForwardData (jsonObj);

		try{
			LoadBackwardData (jsonObj);
		}catch(Exception){
			Debug.Log ("no backup measure");
		}

	}


	void LoadForwardData(JsonData jsonObj){
		notedatas = new NoteData[jsonObj ["noteDatas"] ["Forward"].Count];
		Debug.Log (notedatas.Length);

		for (int i = 0; i < notedatas.Length; i++) {
			NoteData temp = new NoteData ();
			temp.step = jsonObj ["noteDatas"]["Forward"] [i] ["step"].ToString ();
			temp.octave = int.Parse(jsonObj ["noteDatas"]["Forward"] [i] ["octave"].ToString ());
			temp.duration = int.Parse(jsonObj ["noteDatas"]["Forward"] [i] ["duration"].ToString ());
			temp.rest = bool.Parse(jsonObj ["noteDatas"]["Forward"] [i] ["rest"].ToString ());
			temp.measureIndex = int.Parse(jsonObj ["noteDatas"]["Forward"] [i] ["measureIndex"].ToString ());
			temp.alter = bool.Parse(jsonObj ["noteDatas"]["Forward"] [i] ["alter"].ToString ());
			temp.repeat = jsonObj ["noteDatas"]["Forward"] [i] ["repeat"].ToString ();

			notedatas [i] = temp;

			End_Measure = temp.measureIndex;
		}
	}

	void LoadBackwardData(JsonData jsonObj){
		backupdatas = new NoteData[jsonObj ["noteDatas"] ["Backward"].Count];
		Debug.Log (backupdatas.Length);

		for (int i = 0; i < notedatas.Length; i++) {
			NoteData temp = new NoteData ();
			temp.step = jsonObj ["noteDatas"]["Backward"] [i] ["step"].ToString ();
			temp.octave = int.Parse(jsonObj ["noteDatas"]["Backward"] [i] ["octave"].ToString ());
			temp.duration = int.Parse(jsonObj ["noteDatas"]["Backward"] [i] ["duration"].ToString ());
			temp.rest = bool.Parse(jsonObj ["noteDatas"]["Backward"] [i] ["rest"].ToString ());
			temp.measureIndex = int.Parse(jsonObj ["noteDatas"]["Backward"] [i] ["measureIndex"].ToString ());
			temp.alter = bool.Parse(jsonObj ["noteDatas"]["Backward"] [i] ["alter"].ToString ());
			temp.repeat = jsonObj ["noteDatas"]["Backward"] [i] ["repeat"].ToString ();

			backupdatas [i] = temp;

		}

	}

	//make courtine
	void Display(){
	
		Debug.Log (notedatas.Length);

		for (int i = 0; i < notedatas.Length; i++) {

			Debug.Log ("----------------------------");  
			Debug.Log ("Step : " + notedatas [i].step);  
			Debug.Log ("Octave : " + notedatas [i].octave);  
			Debug.Log ("Duration : " + notedatas [i].duration);  
			Debug.Log ("Rest : " + notedatas [i].rest);  
			Debug.Log ("MeasureIndex : " + notedatas [i].measureIndex);  
			Debug.Log ("----------------------------");  
		}


	}

	IEnumerator LoadDisplay(){
		//Display jsonData

		Display ();

		yield return new WaitForSeconds (3f);
	}
}


	