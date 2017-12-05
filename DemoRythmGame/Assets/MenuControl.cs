using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuControl : MonoBehaviour {

    public float speed = 2f;

    public GameObject top;
    public GameObject middle;
    public GameObject bottom;

    public GameObject top_1;
    public GameObject bottom_1;

    public GameObject RadiaMenuManager;

    public bool swap = false;

    public int index;

    // Use this for initialization
	void Start () {
       // RadiaMenuManager.GetComponent<RadialMenuController>().SettingDescription("Play");

        this.gameObject.GetComponent<Button>().onClick.AddListener(()=>{
            OnClicked();
        });
	}

   
    void OnClicked(){
        Debug.Log("This Button's name : " + this.gameObject.name);

        if(this.gameObject.name.Contains("Practice")){
            Debug.Log("Practice open!");
            RadiaMenuManager.GetComponent<RadialMenuController>().SettingDescription("Practice");


        }else if(this.gameObject.name.Contains("Play")){
            Debug.Log("Play open!");
            RadiaMenuManager.GetComponent<RadialMenuController>().SettingDescription("Play");


        }else if(this.gameObject.name.Contains("MyMusic")){
            Debug.Log("MyMusic open!");
            RadiaMenuManager.GetComponent<RadialMenuController>().SettingDescription("MyMusic");

           
        }
        swap = true;
    }

    void MoveOn(){



    }

    private void Update()
    {
        if (swap)
        {
            MoveOn();
        }
    }

}
