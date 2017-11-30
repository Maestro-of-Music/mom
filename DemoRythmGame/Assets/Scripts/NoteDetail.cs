using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteDetail : MonoBehaviour {

    public int default_x;
	public string pitch;
	public int duration;
	public int sequence; //measure
    public int note_index; //counting note
    public GameObject NoteCheck;

    private void Start()
    {
        CreatePreChecker();
    }

    void CreatePreChecker()
    {
        if (gameObject.tag == "Note")
        {
            GameObject note = (GameObject)Instantiate(NoteCheck, gameObject.transform.position, Quaternion.identity);
            NoteDetail[] temp = note.GetComponentsInChildren<NoteDetail>();

            foreach (NoteDetail index in temp)
            {
                index.duration = duration;
                index.pitch = pitch;
                index.sequence = sequence;
                index.note_index = note_index;
            }
        }
    }
}
