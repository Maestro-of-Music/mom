using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MetronomeEvent(Metronume metronome);

public class Metronume : MonoBehaviour {

	public int Base;
	public int Step;
	public float BPM;
	public int CurrentStep = 1;
	public int CurrentMeasure;

	private float interval;
	private float nextTime;

	public float Velocity;

	public event MetronomeEvent OnTick;
	public event MetronomeEvent OnNewMeasure;


	// Use this for initialization
	void Awake(){
	//	StartCoroutine("Init");
	}

	void Start () {
		//StartMetronume();
	}


	// Update is called once per frame
	void Update () {
	}

	public void StartMetronome()
	{
		StopCoroutine("DoTick");
		CurrentStep = 1;
		var multiplier = Base / 4f;
		var tmpInterval = 60f / BPM;
		interval = tmpInterval / multiplier;

		CalVelocity (interval);
		nextTime = Time.time;
		StartCoroutine("DoTick");
	}

	public void CalVelocity(float interval){
		this.Velocity = interval * 4;  //make velocity
	}

	public void StopMetornume(){
		StopCoroutine("DoTick");
	}

	/*

	IEnumerator Init(){

		yield return new WaitForSeconds (1);
		//load json data until 3 seconds 

		var multiplier = Base / 4f;
		var tmpInterval = 60f / BPM;
		interval = tmpInterval / multiplier;

		CalVelocity (interval);
		Debug.Log ("Velocity Setting!");

		yield return null;
	}
*/
	IEnumerator DoTick()
	{
		for (; ; )
		{
			if (CurrentStep == 1 && OnNewMeasure != null)
				OnNewMeasure(this);
			if (OnTick != null)
				OnTick(this);
			nextTime += interval;
			yield return new WaitForSeconds(nextTime - Time.time);
			CurrentStep++;
			if (CurrentStep > Step)
			{
				CurrentStep = 1;
				CurrentMeasure++;
			}
		}
	}
}
