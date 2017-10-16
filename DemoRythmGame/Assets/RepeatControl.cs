using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RepeatControl : MonoBehaviour {

	public GameObject GameManager;
	public GameObject TempoPanel;
	public GameObject RepeatPanel;

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
		Repeat_start = false;

		this._Repeat.onClick.AddListener (() => {
			GameManager.gameObject.GetComponent<MenuManager>().OnTimer();
		});

		//delay 3 seconds
	}

	public void Get_Sequence(){
		TempoPanel.SetActive (false);


		gameObject.GetComponent<PianoControl> ().Repeat_Count = 0;
		Last_position = new Vector3 (0, 0, 0);
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		start_index = int.Parse (start_sequence.text);
		end_index = int.Parse (end_sequence.text);
		count = int.Parse (count_repeat.text);

		int [] arr = new int[] {start_index, end_index}; 
		Start_position = new Vector3 (0, (0 + (1.29f * (arr[0]))), 0);
		gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence", arr);
		//move piano where start_index started;

		Repeat_start = true;
	}

	public void Reset_Sequence(){
		gameObject.GetComponent<ScoreManager> ().score = 0;
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		int [] arr = new int[] {start_index, end_index}; 
		Last_position = new Vector3 (0, 0, 0);

		gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence", arr);

	}
}
