using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RepeatControl : MonoBehaviour {

	public GameObject GameManager;
	public GameObject TempoPanel;
	public GameObject RepeatPanel;
	public GameObject ScorePanel;

	public InputField start_sequence;
	public InputField end_sequence;
	public InputField count_repeat;

	public Button _Repeat;

	public int start_index;
	public int end_index;

	public int count; 
	public bool Repeat_start;

	public Vector3 Last_position;
	public Vector3 Start_position;

	void Start(){
		ScorePanel.SetActive (false);
		Repeat_start = false;

		this._Repeat.onClick.AddListener (() => {
			if (CheckSetting (start_sequence.text, end_sequence.text, count_repeat.text)){
				TempoPanel.SetActive (false);
				RepeatPanel.SetActive(false);
				GameManager.gameObject.GetComponent<MenuManager>().OnTimer();
			}
		});

		//delay 3 seconds
	}

	public bool CheckSetting(string _start, string _end , string _count){
		if (_start != "" && _end != "" && _count != "") {
			return true;
		} else {
			return false;
		}
	}

	public void Get_Sequence(){

		gameObject.GetComponent<PianoControl> ().Repeat_Count = 0;
		Last_position = new Vector3 (0, 0, 0);
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		if (CheckSetting (start_sequence.text, end_sequence.text, count_repeat.text)) {
			start_index = int.Parse (start_sequence.text);
			end_index = int.Parse (end_sequence.text);
			count = int.Parse (count_repeat.text);

			//repeat mode try catch needed!!

			int[] arr = new int[] { start_index, end_index }; 
			Start_position = new Vector3 (0, (0 + (1.29f * (arr [0]))), 0);
			gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence", arr);
			//move piano where start_index started;

			ScorePanel.SetActive(true);

			Repeat_start = true;
		} else {
			Repeat_start = false;
		}
	}



	public void Reset_Sequence(){
		gameObject.GetComponent<ScoreManager> ().score = 0;
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		int [] arr = new int[] {start_index, end_index}; 
		Last_position = new Vector3 (0, 0, 0);

		gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence", arr);

	}

	public void CallSetInitMode(){
		GameManager.gameObject.GetComponent<MenuManager> ().StartCoroutine ("StartInitMode");
	}
}
