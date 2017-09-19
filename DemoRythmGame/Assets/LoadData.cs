using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LoadData : MonoBehaviour {

	public TextAsset jsonData;
	public NoteInfo noteinfo;
	public NoteData [] notedatas;

	// Use this for initialization
	void Awake () {

		StartCoroutine ("LoadJSON");
	
	}

	void Update(){
		Display ();
	}
		
		
	IEnumerator LoadJSON (){
		JsonData getData = JsonMapper.ToObject (jsonData.text);

		noteinfo = new NoteInfo ();

		LoadNoteIofo (getData);
		LoadNoteData (getData);

		yield return null;
	}

	void LoadNoteIofo(JsonData JsonObj){
	//note information	

		NoteInfo temp = new NoteInfo ();

		for (int i = 0; i < JsonObj ["NoteInfo"].Count; i++) {

			//expend the music note 
			temp.title = JsonObj ["NoteInfo"][i]["Title"].ToString();
			temp.tempo = int.Parse(JsonObj ["NoteInfo"][i]["Tempo"].ToString());
			temp.measure = int.Parse(JsonObj ["NoteInfo"][i]["Measure"].ToString());
			temp.beats = int.Parse(JsonObj ["NoteInfo"][i]["Beats"].ToString());
			temp.beats_type = int.Parse(JsonObj ["NoteInfo"][i]["Beats_type"].ToString());

		}

		noteinfo = temp; //save note basic data  
	}

	void LoadNoteData(JsonData jsonObj){
	//note data 
		notedatas = new NoteData[jsonObj ["NoteData"].Count];

		for (int i = 0; i < notedatas.Length; i++) {

			NoteData temp = new NoteData ();
		
			temp.step = jsonObj ["NoteData"] [i] ["step"].ToString ();
			temp.octave = int.Parse(jsonObj ["NoteData"] [i] ["octave"].ToString());
			temp.duration = int.Parse(jsonObj ["NoteData"] [i] ["duration"].ToString());
			temp.rest =  bool.Parse(jsonObj["NoteData"] [i] ["rest"].ToString());
			temp.measureIndex = int.Parse(jsonObj ["NoteData"] [i] ["meausreIndex"].ToString());
		
			notedatas [i] = temp;
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
}


	