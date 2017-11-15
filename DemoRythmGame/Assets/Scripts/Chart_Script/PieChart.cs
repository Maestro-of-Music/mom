using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieChart : MonoBehaviour {

    public float[] values;
    public Image [] Nodes;
    public Text [] labels;

	// Use this for initialization
	void Start () {
        MakeGraph();
	}
	
    void MakeGraph(){
        float total = 0f;
        float zRotation = 0f;
        for (int i = 0; i < values.Length-1;i++){
            total += values[i];
        }

        for (int i = 0; i < values.Length-1; i++){
            Nodes[i].transform.SetParent(transform, false);
            Nodes[i].fillAmount = values[i] / total;
            Nodes[i].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= Nodes[i].fillAmount * 360f;
            labels[i].text = ((Mathf.Round(Nodes[i].fillAmount / .01f) * .01f)*100).ToString()+"%";
        }
    }
}
