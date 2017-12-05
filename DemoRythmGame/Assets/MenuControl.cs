using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuControl : MonoBehaviour {

    public float speed = 2f;

    public GameObject top;
    public GameObject middle;
    public GameObject bottom;

    public GameObject top_1;
    public GameObject bottom_1;

    public GameObject Description_background;

    public GameObject RadiaMenuManager;

    public bool swap = false;

    public int index;
    private SceneChange scenechange;

    void Awake(){
        this.scenechange = SceneChange.getInstance();
    }

    // Use this for initialization
	void Start () {

        this.gameObject.GetComponent<Button>().onClick.AddListener(()=>{
            OnClicked();
        });
	}

   
    void OnClicked(){
        Debug.Log("This Button's name : " + this.gameObject.name);

        if(this.gameObject.name.Contains("Practice")){
            Debug.Log("Practice open!");

            this.scenechange.mode = 2;
            //  this.bluetooth.Send("@");
            SceneManager.LoadScene("8practice");


        }else if(this.gameObject.name.Contains("Play")){
            Debug.Log("Play open!");

            this.scenechange.mode = 1;
            //  this.bluetooth.Send("!");
            SceneManager.LoadScene("5play");

        }else if(this.gameObject.name.Contains("MyMusic")){
            Debug.Log("MyMusic open!");

            SceneManager.LoadScene("10MyMusic");
        }
        
        Description_background.SetActive(false);

    }

}
