using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String1 : MonoBehaviour {

	public KeyCode activateString;
	public string lockInput = "n";
	public string releaseKey = "y";



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (activateString) && (lockInput =="n")) 
		{
			lockInput = "y";
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 2);
			StartCoroutine (retractCollider ());
			releaseKey = "n";

		}

		if((Input.GetKeyUp(activateString))){
		
			releaseKey = "y";
		
		}
	}

	IEnumerator retractCollider()
	{
		yield return new WaitForSeconds (.75f);
		GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);

		if (releaseKey == "n") {
			yield return new WaitForSeconds (1);
			//StartCoroutine (releaseNote ());
		
		} else if (releaseKey == "y") {
			
		}


		yield return new WaitForSeconds (.75f);
		GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
		lockInput = "n";
	}
}
