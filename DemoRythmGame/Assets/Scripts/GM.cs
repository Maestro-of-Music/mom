using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

	public List<float> whichNote = new List<float>() {1,6,3,4,2,5,1,2,3,5,6,4,6,5,5,1,2,4,1,1,4,5,5};

	public int noteMark = 0;

	public Transform noteObj;

	public string timerReset = "y";

	public float xPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (timerReset == "y") 
		{
			StartCoroutine (spawnNote ());
			timerReset = "n";
		}

	}


	IEnumerator spawnNote(){
		yield return new WaitForSeconds (1);

		if (whichNote [noteMark] == 1) {
			xPos = -1.34f;
		}

		if (whichNote [noteMark] == 2) {
			xPos = -.7f;
		}

		if (whichNote [noteMark] == 3) {
			xPos = -.1f;
		}
		if (whichNote [noteMark] == 4) {
			xPos = .4f;
		}
		if (whichNote [noteMark] == 5) {
			xPos = .95f;
		}
		if (whichNote [noteMark] == 6) {
			xPos = 1.45f;
		}

		noteMark += 1;
		timerReset = "y";
		Instantiate (noteObj, new Vector3 (xPos, 1.2f, -2.18f), noteObj.rotation);

	}
}
