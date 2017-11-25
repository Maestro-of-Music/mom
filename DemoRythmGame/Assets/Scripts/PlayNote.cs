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

	void OnPlay(){
		OnRelease (PianoObject.gameObject.GetComponent<PianoControl> ().pitch);
		int c4Key = (int)(semitone_offest - 72); 

		audioOne.pitch = Mathf.Pow (2f, c4Key/12.0f);

        audioOne.Play ();
		PianoObject.gameObject.GetComponent<PianoControl> ().pitch = null;

	}

	void OnRelease(string pitch){
		//release keyboard 
		Observer.gameObject.GetComponent<Observer> ().OnRelease (pitch);
	}

	void OnTriggerEnter (Collider col){

		if (col.gameObject.tag == "Note") {

           if (PianoObject.gameObject.GetComponent<PianoControl>().end == false) {
				OnPlay (); 
			}
		}
			
	}

	IEnumerator OnReleaseStart(string pitch){
		Observer.gameObject.GetComponent<Observer> ().OnRelease (pitch);

		yield return null;
	}
	/*
	void OnTriggerExit(Collider col){
		Destroy (col.gameObject);
	}
	*/
}
