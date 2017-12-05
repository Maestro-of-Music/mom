using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    private SceneChange scenechange;
   // private Bluetooth bluetooth;
    public Button Setting;

    private void Awake()
    {
        this.scenechange = SceneChange.getInstance();
      
    }

    // Use this for initialization
    void Start () {
        /*
        this.Practice.onClick.AddListener(()=>{
            Debug.Log("Practice Clicked");
            this.scenechange.mode = 2;
          //  this.bluetooth.Send("@");
            SceneManager.LoadScene("8practice");
        });
        this.Play.onClick.AddListener(()=>{
            Debug.Log("Play Clicked");
            this.scenechange.mode = 1;
          //  this.bluetooth.Send("!");
            SceneManager.LoadScene("5play");
        });

        this.MyMusic.onClick.AddListener(()=>{
            SceneManager.LoadScene("10MyMusic");
        });
        */

        this.Setting.onClick.AddListener(()=>{
            SceneManager.LoadScene("Setting"); 
        });


	}

}
