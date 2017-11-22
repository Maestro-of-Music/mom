using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogControl : MonoBehaviour {

    private bool check = false;
    public float dist;
    public string result;
    public int count_Keyboard;
    public List<string> pitch;
    public string leftHand;
    public string rightHand;

    IEnumerator ChangeBox (){
        
        if (count_Keyboard > 1)
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(50, 0.4f, 1);
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(9, 0.4f, 1);
        }

        yield return null;

    }

    void OnTriggerStay(Collider other)
    {
        if (check == false)
        {
            StartCoroutine("ChangeBox");
            LogObjBuild(other);
        }
    }

    void LogObjBuild(Collider other){
        
            if (other.gameObject.tag == "Note")
            {
                Debug.Log("Check! Log!!");               
                if(pitch.Count != count_Keyboard){
                //Debug.Log("AAAAAAAAAAAA: " + other.gameObject.GetComponent<NoteDetail>().pitch);
               
                if (pitch.Contains(other.gameObject.GetComponent<NoteDetail>().pitch) == false)
                    pitch.Add(other.gameObject.GetComponent<NoteDetail>().pitch);
                
                LRSetting(other.gameObject.GetComponent<NoteDetail>().pitch);
                distCalculate(other);

            }else{
                check = true;
            }

        }
    }

    void distCalculate(Collider col){
        dist = gameObject.GetComponent<Transform>().position.y - col.gameObject.GetComponent<Transform>().position.y;
        dist = Mathf.Round(dist * 100.0f);
        dist = dist / 100;

        if (dist < 0.2f){
            result = "Perfect";
        }else if (0.2f <= dist && dist < 0.5f){
            result = "Good";
        }else if (0.5f <= dist && dist < 0.8f){
            result = "Cool";
        }else {
            result = "Miss";
        }

    }


    void LRSetting(string tempPitch)
    {
        int index = int.Parse(tempPitch.Substring(0, 1));

        if (index < 4)
        {
            leftHand = tempPitch;
        }
        else
        {
            rightHand = tempPitch;
        }
    }

}
