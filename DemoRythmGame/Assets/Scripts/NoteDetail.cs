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
            float result = checkLength(duration); 
            GameObject note = (GameObject)Instantiate(NoteCheck, new Vector3(gameObject.transform.position.x ,gameObject.transform.position.y + result, gameObject.transform.position.z ), Quaternion.identity);
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

    float checkLength(int d){
        float result = 0;
        switch(d){
            case 1:
                result = 0.1f;
                break;
            case 2:
                result = 0.15f; 
                break;
            case 3:
                result = 0.2f;
                break;
            case 4:
                result = 0.25f;
                break;
            default:
                result = 0.0f;
                break;
        }
        return result;
    }
}
