using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemObject : MonoBehaviour {

    public Button Btn;
    public Image Icon;
    public Text Title;
    public Text Detail;
    public string Scene;

    public Sprite Before;
    public Sprite After;

    public bool fill = false; 

    private SceneChange scenechange;

    private void Awake()
    {
        this.scenechange = SceneChange.getInstance();

    }


    public void ItemClick_Result()
    {
        Debug.Log("Clicked : " + Btn.name);
        this.scenechange.NextScene(Scene ,Btn.name);
    }

    public void StarFilling(){
        if (fill){
            Debug.Log("Clicked!");
            Icon.sprite = Before;
            fill = false;
        }else {
            Debug.Log("Clicked!");
            Icon.sprite = After;
            fill = true;
        }
    }
}
