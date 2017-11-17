using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour {

    public GameObject LogObject;

    public void MakeLogObject(int count){
        GameObject note = (GameObject)Instantiate(LogObject, new Vector3(0, gameObject.GetComponent<Transform>().position.y + 1.6f, 2.85f), Quaternion.identity);
        note.GetComponent<LogControl>().count_Keyboard = count;        
    }
	
    //setting calculation of log data
}
