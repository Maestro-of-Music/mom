using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Observer : MonoBehaviour {

	public GameObject PianoManager;
	//public GameObject Metronume;
	public GameObject GameManager;
	public GameObject KeyBoard;
	public GameTime Current;
	private string Key = "Key_";
	//private int step =0;

	//public bool MetronumePlay;

	// Use this for initialization
	void Start () {
		InitObserver ();
	}

	void Update(){
		/*
		if (MetronumePlay) {
			MoveObserver (); 
		}
		*/
	}
	/*
	public void OnMetronume(){
	
		if (this.MetronumePlay == false) {
			Metronume.gameObject.GetComponent<Metronume> ().StartMetronome (); //piano player move
			this.MetronumePlay = true; 
		} else {
			this.MetronumePlay = false;
		}
	}
	*/

	void InitObserver(){
		//MetronumePlay = false;
		Current = GameManager.GetComponent<GameTime> ();
	}

	/*
	void MoveObserver(){
		int Move = 0;   

		if (step != Current.currentStep) {
			Move = 1; 
		} else if (Current.currentStep == 0) {
		    Move = 0;
			Debug.Log ("Music End! ");
		}

		transform.position = new Vector3 (transform.position.x, transform.position.y + Move, transform.position.z);

		step = Current.currentStep;
	}
	*/

	void OnPianoPlay(Collider col){
		PianoManager.gameObject.GetComponent<PianoControl>().duration = col.gameObject.GetComponent<NoteDetail> ().duration;

		if (col.gameObject.tag == "Rest") {
			PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch = "-1";
		} else {
			PianoManager.gameObject.GetComponent<PianoControl>().Target_pitch = col.gameObject.GetComponent<NoteDetail> ().pitch;
		}
		PianoManager.gameObject.GetComponent<PianoControl> ().Sequence++;
		PianoManager.gameObject.GetComponent<PianoControl> ().move = false;

	}


	void OnTagChange(GameObject col){ //to keyboard
		if (col.gameObject.tag == "NonClicked") {
			col.gameObject.tag = "Clicked";
		} else {
			col.gameObject.tag = "NonClicked";
		}
	}


	void OnTriggerEnter(Collider col){
	
		if (col.gameObject.tag == "Note") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, true); //pressed
			OnPianoPlay (col);
	
		} else if (col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false); //released
			OnPianoPlay (col);
		}
	}

	void OnTriggerExit(Collider col){

		if (col.gameObject.tag == "Note" || col.gameObject.tag == "Rest") {
			KeySequence (col.gameObject.GetComponent<NoteDetail> ().pitch, false ); //release
		}
	}

	public void OnRelease(){
		//release every Keyboard right now 
		
		GameObject [] ClickedObjs = GameObject.FindGameObjectsWithTag ("Clicked");

		for (int i = 0; i < ClickedObjs.Length; i++) {
			//Debug.Log ("Clicked Objects : " + i); 
			ClickedObjs [i].gameObject.GetComponent<Renderer> ().
			material.color = Color.white;
		}

	}

	void KeySequence(string pitch, bool Clicked){

		try //selected objects
		{
			KeyBoard = GameObject.Find (Key + pitch);
			OnTagChange(KeyBoard);
		}
		catch(Exception){ //non selected 
			OnRelease();
		}
		finally{

			if (pitch != "") {
				ColoringKeyBoard (Clicked);
			}
		
		}
	}

	void ColoringKeyBoard(bool Clicked){
		if (Clicked == true) {
			KeyBoard.GetComponent<Renderer> ().material.color = Color.yellow;
		} else {
			KeyBoard.GetComponent<Renderer> ().material.color = Color.white;
		}
	}

}
