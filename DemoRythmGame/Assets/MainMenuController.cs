using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    private SceneChange scenechange;
    public Button Setting;
    public Button Up;

    public GameObject RadialMenu;

    private void Awake()
    {
        this.scenechange = SceneChange.getInstance();
      
    }

    // Use this for initialization
    void Start () {
        
        this.Setting.onClick.AddListener(()=>{
            SceneManager.LoadScene("Setting"); 
        });

        this.Up.onClick.AddListener(()=>{
            Debug.Log("Up Button Clicked");

            if (RadialMenu.GetComponent<RadialMenuController>().shift_index < 2)
            {
                RadialMenu.GetComponent<RadialMenuController>().shift_index++;
            }
            else
            {
                RadialMenu.GetComponent<RadialMenuController>().shift_index = 0;
            }

            RadialMenu.GetComponent<RadialMenuController>().moveOn = true;

        });

	}

}
