using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableListDetailControl : MonoBehaviour {
    
    public string selected = "#ffe3de";
    public string unselected = "#b57e75";

    private void Start()
    {
        init();
    }

    void init(){
        if(gameObject.tag == "Clicked"){
            gameObject.GetComponent<Text>().color = TextColoring(selected); 
        }else{
            gameObject.GetComponent<Text>().color = TextColoring(unselected); 
        }
    }

    public void Select(){

        if(gameObject.tag =="NonClicked"){
            GameObject objs = GameObject.FindGameObjectWithTag("Clicked");
            objs.GetComponent<Text>().color = TextColoring(unselected);
            objs.tag = "NonClicked";

            
            gameObject.tag = "Clicked";
            gameObject.GetComponent<Text>().color = TextColoring(selected); 
        }
    } 


    public Color TextColoring(string hex){
        
        Color mycolor = new Color();
        ColorUtility.TryParseHtmlString(hex, out mycolor);

        return mycolor;
    }
}
