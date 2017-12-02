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
    public float start_Height;
    public float end_Height;

	public Vector3 Last_position;
	public Vector3 Start_position;

	void Start(){
		Last_position = new Vector3(0, 0, 0);

		//ScorePanel.SetActive (false);
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
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		if (CheckSetting (start_sequence.text, end_sequence.text, count_repeat.text)) {
			start_index = int.Parse (start_sequence.text);
			end_index = int.Parse (end_sequence.text);
			count = int.Parse (count_repeat.text);

			//repeat mode try catch needed!!

			int[] arr = new int[] { start_index, end_index }; 
            StartCoroutine("checkSequencePosition", arr);

            Start_position = new Vector3 (0, start_Height, 0);
            Last_position = new Vector3(0, end_Height, 0);

            Debug.Log("Start & Last_position : " + Start_position + " " + Last_position);

			gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence");

			//move piano where start_index started;

			//ScorePanel.SetActive(true);

			Repeat_start = true;
		} else {
			Repeat_start = false;
		}
	}

	public void Reset_Sequence(){
		gameObject.GetComponent<ScoreManager> ().score = 0;
		gameObject.GetComponent<PianoControl> ().InitPosition ();

		int [] arr = new int[] {start_index, end_index}; 
		gameObject.GetComponent<PianoControl> ().StartCoroutine ("SearchSequence", arr);

	}

    IEnumerator checkSequencePosition(int [] arr){
        SearchStartPosition(arr[0], arr[1]);
        yield return null;
    }

    public void SearchStartPosition(int start_index, int end_index){
        Debug.Log("Starting Start Position of repeat start");
		
        GameObject[] find = GameObject.FindGameObjectsWithTag("Note");

        for (int i = 0; i < find.Length; i++)
        {
            if (find[i].gameObject.GetComponent<NoteDetail>().sequence.Equals(start_index))
            {
                Debug.Log("Find out start index");
                start_Height = find[i].gameObject.GetComponent<Transform>().position.y;
            }
            else if (find[i].gameObject.GetComponent<NoteDetail>().sequence+1 == end_index +1)
            {
                Debug.Log("Find out end index");
                end_Height = find[i].gameObject.GetComponent<Transform>().position.y;
            }
        }

	}
}
