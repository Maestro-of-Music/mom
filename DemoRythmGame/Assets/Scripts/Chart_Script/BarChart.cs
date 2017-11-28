using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BarChart : MonoBehaviour {

    public Bar barPrefab;

    List<Bar> bars = new List<Bar>(); //Hands Accuracy
    public int[] inputValues;
    float charHeight;

     void Start(){
        charHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        DisplayGraph(inputValues);   
    }

    void DisplayGraph(int [] vals){
        int maxValues = vals.Max();

        for (int i = 0; i < vals.Length; i++){
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform);
            newBar.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();

            switch(i+1){
                case 1:
                    newBar.bar.GetComponent<Image>().color = Color.green;
                    break;
                case 2:
                    newBar.bar.GetComponent<Image>().color = Color.blue;
                    break;
                case 3:
                    newBar.bar.GetComponent<Image>().color = Color.cyan;
                    break;
                case 4:
                    newBar.bar.GetComponent<Image>().color = Color.gray;
                    break;
            }

            float normalizedValue = ((float)vals[i] / (float)maxValues) * 0.95f;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, charHeight * normalizedValue);
            Debug.Log(i);
        }
    }



}
