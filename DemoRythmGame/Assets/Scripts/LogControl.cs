using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogControl : MonoBehaviour {

    private bool check = false;
    public float dist;
    public string result;

    private void OnTriggerEnter(Collider other)
    {
        if (check == false){
            if(other.gameObject.tag == "Note"){
                Debug.Log("Check! Log!!");
                distCalculate(other);
                check = true;
            }
        }
    }

    void distCalculate(Collider col){
        dist = gameObject.GetComponent<Transform>().position.y - col.gameObject.GetComponent<Transform>().position.y;
        dist = Mathf.Round(dist * 100.0f);
        dist = dist / 100;
    }
}
