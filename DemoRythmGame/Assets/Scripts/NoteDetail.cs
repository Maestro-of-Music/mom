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

    /*
    private void Start()
    {
        if(duration > 1 && gameObject.tag == "Note_0"){
            Vector3 temp = gameObject.transform.localPosition;
            gameObject.transform.localPosition = new Vector3(temp.x, temp.y - 0.2f, temp.z);
        }
    }
    */ 

}
