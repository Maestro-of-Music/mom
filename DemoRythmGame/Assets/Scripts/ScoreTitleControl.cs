using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTitleControl : MonoBehaviour {

    private bool opacitycheck = false;
    private Image SelectedImage;
    private float opacity;
	
	// Update is called once per frame
	void Update () {
        if (opacitycheck){
            BeTransparent();
        }
    }

   public void ImageInit()
    {
        gameObject.SetActive(true);
        opacity = 1.0f;
        SelectedImage = gameObject.GetComponent<Image>();
        var tempColor = SelectedImage.color;
        tempColor.a = 1f;
        SelectedImage.color = tempColor;

        if (opacitycheck == false)
        {
            opacitycheck = true;
        }
    }


    void BeTransparent()
    {
        if (SelectedImage.color.a > 0)
        {
            var temp = SelectedImage.color;
            temp.a = opacity - 0.05f;
            //good
            opacity = temp.a;
            SelectedImage.color = temp;
        }
        else
        {
            opacitycheck = false;
            gameObject.SetActive(false);
        }
    }
}
