using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectcontrol : MonoBehaviour {

    //timer setting
    private float timeLeft = 5.0f;
    private bool check = false;

    private void Start()
    {
        if (check == false){
            check = true;
        }
    }

    void Update () {

        if(check){
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                OffEffect();
            }
        }
	}

    void OffEffect(){
        gameObject.SetActive(false);
    }

}
