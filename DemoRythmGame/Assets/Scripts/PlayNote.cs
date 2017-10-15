using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayNote : MonoBehaviour {

	private AudioSource audioOne;
	public float semitone_offest = 0;
	public GameObject PianoObject;
	public GameObject Observer;

	// Use this for initialization
	void Start () {
		audioOne = GetComponent<AudioSource>();
		audioOne.playOnAwake = false;
	}

	void OnMouseDown(){
		OnPlay ();
	}


	void OnPlay(){
		audioOne.pitch = Mathf.Pow (2f, semitone_offest/12.0f);
		audioOne.Play ();
		PianoObject.gameObject.GetComponent<PianoControl> ().pitch = "";
		OnRelease ();
	}

	void OnRelease(){
		//release keyboard 
		Observer.gameObject.GetComponent<Observer> ().OnRelease ();
	}

	void OnTriggerEnter (Collider col){

		if (col.gameObject.tag == "Note") {
			/*
			if (PianoObject.gameObject.GetComponent<PianoControl> ().move) {
				OnPlay (); 
			} */

			OnPlay ();
		}
			
	}
	/*
	void OnTriggerExit(Collider col){
		Destroy (col.gameObject);
	}
	*/
}
