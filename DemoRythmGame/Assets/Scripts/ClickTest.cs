using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickTest : MonoBehaviour {

    public GameObject Show_Image;
    public bool check = false;
    private Image temp;
    private float opacity;
    public string result; //check Perfect, Good , Cool ,Miss // log file 기준

    private void Awake()
    {
        temp = Show_Image.GetComponent<Image>();
        Show_Image.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Show_Image.SetActive(true);
            opacity = 1.0f;
           
            var tempColor = temp.color;
            tempColor.a = 1f;
            temp.color = tempColor;

            Debug.Log("Pressed A !");
            if (check == false)
                check = true;
        }

        if(check){
            BeTransparent();
        }
       
    }
    void BeTransparent(){
        if(temp.color.a > 0){
            var tempColor = temp.color;
            tempColor.a = opacity - 0.02f; 
            //good
            opacity = tempColor.a;
            temp.color = tempColor;
        }else{
            check = false;
        }
    }
}
