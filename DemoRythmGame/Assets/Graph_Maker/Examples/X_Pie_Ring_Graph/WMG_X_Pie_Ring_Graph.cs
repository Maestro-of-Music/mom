using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMG_X_Pie_Ring_Graph : WMG_GUI_Functions {


	public Object ringGraphPrefab;

    /*
    private static WMG_X_Pie_Ring_Graph _instance = null;

    public static WMG_X_Pie_Ring_Graph getInstance(){
        if(_instance == null)
        {
            _instance = new WMG_X_Pie_Ring_Graph();
        }
        return _instance;
    }

     void Awake()
    {
        _instance = this;
    }

    */


    public void CreateRingChart(List<int> arr){
        
        GameObject ringGO = GameObject.Instantiate(ringGraphPrefab) as GameObject;
        changeSpriteParent(ringGO, this.gameObject);
        WMG_Ring_Graph graph = ringGO.GetComponent<WMG_Ring_Graph>();
        graph.Init(); // always initialize first (ensures Start() method on the graph gets called first)
        graph.pieMode = true;
        graph.pieModePaddingDegrees = 1; // the degree spacing between each slice
        graph.pieModeDegreeOffset = 90; // the degree rotational offset of the entire graph
        graph.innerRadiusPercentage = 0.75f; // the percentage of the graph that is empty
        graph.autoUpdateBandAlphaReverse = true; // reverses the order of how the bandcolors are updated 
        graph.labelStartCenteredOnBand = true;
        graph.animateData = false;

        graph.values.Clear();

        for (int i = 0; i < arr.Count; i++)
        {
            graph.values.Add(arr[i]);
        }

        changeSpriteSize(graph.gameObject, 700, 600);

        graph.WMG_Ring_Click += MyCoolRingClickFunction;
        graph.WMG_Ring_MouseEnter += MyCoolRingHoverFunction;

    }

	void MyCoolRingClickFunction (WMG_Ring ring, UnityEngine.EventSystems.PointerEventData pointerEventData) {
		Debug.Log ("Ring: " + ring.ringIndex + " value: " + ring.graph.values[ring.ringIndex] + " label: " + ring.graph.labels[ring.ringIndex]);
	}

	void MyCoolRingHoverFunction (WMG_Ring ring, bool hover) {
//		Debug.Log ("Hover: " + hover + " Ring: " + ring.ringIndex + " value: " + ring.graph.values[ring.ringIndex] + " label: " + ring.graph.labels[ring.ringIndex]);
	}
}
