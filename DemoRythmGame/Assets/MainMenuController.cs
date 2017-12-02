using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    private SceneChange scenechange;

    public Button Practice;
    public Button Play;
    public Button Bluetooth;

    private void Awake()
    {
        this.scenechange = SceneChange.getInstance(); 
    }

    // Use this for initialization
    void Start () {
        this.Practice.onClick.AddListener(()=>{
            Debug.Log("Practice Clicked");
            this.scenechange.mode = 2;
            SceneManager.LoadScene("8");
        });
        this.Play.onClick.AddListener(()=>{
            Debug.Log("Play Clicked");
            this.scenechange.mode = 1;
            SceneManager.LoadScene("5");
        });
        this.Bluetooth.onClick.AddListener(()=>{
            SceneManager.LoadScene("BluetoothDemo"); 
        });
	}

}
