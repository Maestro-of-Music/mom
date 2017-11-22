using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNote : MonoBehaviour {

	public GameObject LoadObject;
	public GameObject NoteLocation;
	public GameObject StartLocation;
	public GameObject NotePrefab;
	public GameObject RestNotePrefab;
	public GameObject MeasurePrefab;
	public GameObject EndPrefab;

	private LoadData loaddata;

	public int tempo;
	public string title;
	public int measure;
	public int beats;
	public int beats_type;
    private int notenum;

	public NoteData [] notedatas;
	public NoteData[] backupdatas;

	public float noteHeight;
	private int measure_count;
	private float forward;
	private float backward;


	void Awake(){
		StartCoroutine ("WaitForLoadData"); //wait for load data 
		StartCoroutine ("ConnectLoadData"); //actually connect load data
		noteHeight = 1.4f;
        measure_count = 0;
        notenum = 1; // check total number of note
	}

	void Start(){
		InstantiateNote ();
	}

	void LoadNoteInfo(){
		//create beat prefabs 

		tempo = loaddata.noteinfo.Tempo;
		title = loaddata.noteinfo.Title;
		measure = loaddata.noteinfo.Measure;
		beats = loaddata.noteinfo.Beats;
		beats_type = loaddata.noteinfo.Beat_type;
	}

	void LoadNoteData(){

		notedatas = new NoteData[loaddata.notedatas.Length];
		notedatas = loaddata.notedatas;

		backupdatas = new NoteData[loaddata.backupdatas.Length];
		backupdatas = loaddata.backupdatas;
	}
		
	void InstantiateNote(){

		//load notedata and instanstiate each music note 
		for (int i = 0; i < notedatas.Length; i++) 
		{

			string Pitch = SetNotePitch (notedatas [i]);
			Debug.Log (Pitch);

			if (notedatas [i].backward) {
					
			}

			try{
				NoteLocation = GameObject.Find (Pitch); //refer to each pitch's location data 
			}

			catch(NullReferenceException){
				//instantiate rest note using duration note
				Debug.Log("rest note ");
				NoteLocation = null;
				Pitch = "";
			}
			finally{
				//SetMeasureObject (NoteLocation, notedatas [i].measureIndex);
				SetNoteObject(NoteLocation, notedatas[i].duration,Pitch,notedatas[i].measureIndex);
                notenum++;
				forward = noteHeight;
			}
		}

		noteHeight = 1.4f; // initiate noteheight 

		//load notedata and instanstiate each music note 
		for (int i = 0; i < backupdatas.Length; i++) 
		{

			string Pitch = SetNotePitch (backupdatas [i]);
			Debug.Log (Pitch);

			/*
			if (notedatas [i].backward) {
			}
			*/

			try{
				NoteLocation = GameObject.Find (Pitch); //refer to each pitch's location data 
			}

			catch(NullReferenceException){
				//instantiate rest note using duration note
				Debug.Log("rest note ");
				NoteLocation = null;
				Pitch = "";
			}
			finally{
				//SetMeasureObject (NoteLocation, notedatas [i].measureIndex);
				SetNoteObject(NoteLocation, backupdatas[i].duration ,Pitch ,backupdatas[i].measureIndex);
                measure_count = backupdatas[i].measureIndex;
				notenum++;
				backward = noteHeight;
			}
		}

		noteHeight = (forward > backward) ? forward : backward;  

		EndNoteObject (measure_count);

	}
		
	void SetMeasureObject(GameObject noteObject, int index){

		if (index != measure_count) {
			SetNoteObject (noteObject);
		} 
		else {
			index = measure_count;
		}
	}

	string SetNotePitch(NoteData data){

		string result = ""; 

		if (data.octave != -1) {
			if (data.alter == true) { 
				result = data.octave + data.step + "#";
			} else {
				result = data.octave + data.step;	
			}
		} 
		// case note 
		else {
			result = "rest";
		} // case rest
		Debug.Log (result);
		 

		return result;
	}

	//setting NoteMeasure Data

	void SetNoteObject(GameObject noteObject){

		GameObject note = null;
		float Height = noteHeight;

		//instantiate measure index 
		note = (GameObject)Instantiate(MeasurePrefab,new Vector3(noteObject.GetComponent<Transform>().transform.position.x+50, Height, noteObject.GetComponent<Transform>().transform.position.z ),Quaternion.identity);
	}

	void EndNoteObject(int Measure){
		//final end note intantiate 
		GameObject note;
		note = (GameObject)Instantiate (EndPrefab, new Vector3 (0, noteHeight, 2.85f), Quaternion.identity);
        note.GetComponent<NoteDetail>().sequence = Measure + 1; //setting Measure 

	}
		
	//setting NoteObject
	void SetNoteObject(GameObject noteObject , int duration, string pitch, int sequence ){

		GameObject note;
		float Height = noteHeight;

		if (noteObject != null) {

			Debug.Log ("pitch : " + pitch);

			note = (GameObject) Instantiate(NotePrefab, new Vector3 (noteObject.GetComponent<Transform>().transform.position.x, Height, noteObject.GetComponent<Transform>().transform.position.z),Quaternion.identity);
			note.transform.localScale = new Vector3 (1, duration, 1);
			note.GetComponent<NoteDetail> ().duration = duration;
			note.GetComponent<NoteDetail> ().pitch = pitch;
			note.GetComponent<NoteDetail> ().sequence = sequence;
            note.GetComponent<NoteDetail>().note_index = notenum; //setting note index

            NoteDetail [] temp = note.GetComponentsInChildren<NoteDetail>();              foreach(NoteDetail index in temp){                 index.duration = duration;                 index.pitch = pitch;                 index.sequence = sequence;             }

			switch (duration) {
			case 1: //white
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32(255, 0, 0, 0);
				break;
			case 2: 
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32(255, 80, 80,0);
				break;
			case 3: 
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (255, 168, 82,0);
				break;
			case 4: 
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (255, 212, 82,0);
				break; 
			case 5:  
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (212, 255, 82,0);
				break; 
			case 6:  
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (168, 255, 82,0);
				break; 
			case 7:  
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (125, 255, 82,0);
				break; 
			case 8:  
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (82, 255, 82,0);
				break; 
			default:
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32 (82, 255, 125,0);
				break; 

			}
		

			if (pitch.EndsWith ("#")) {
				Debug.Log ("alter note");
				note.gameObject.GetComponentInChildren<Renderer> ().material.color = new Color32(255, 80+88, 80,0);
				note.transform.localScale = new Vector3 (1, duration, 0.5f);
			} else {
				note.transform.localScale = new Vector3 (1, duration, 1);
			}

		} 
		else if(noteObject == null){ 
			note = (GameObject) Instantiate(RestNotePrefab, new Vector3 (-3.8f, Height, 3),Quaternion.identity);
			note.GetComponent<NoteDetail> ().duration = duration;
			note.GetComponent<NoteDetail> ().sequence = sequence;
			//setting rest object 
		}

		noteHeight += (float)duration;

	} 

	IEnumerator WaitForLoadData(){
		yield return new WaitForSeconds (3f);
	}

	IEnumerator ConnectLoadData(){

		loaddata = LoadObject.GetComponent<LoadData> ();
		LoadNoteInfo (); 
		LoadNoteData ();


		yield return null;
	}
}	
