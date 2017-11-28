using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMG_X_Pie_Ring_Graph : WMG_GUI_Functions {


	public Object ringGraphPrefab;

    /*
    private void Start()
    {
        CreateRingChart();
    }
    */

    public void CreateRingChart(){
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

        graph.values.Add(100);
        graph.values.Add(150);
        graph.values.Add(50);
        graph.values.Add(10);

        graph.labels.Add("Perfect");
        graph.labels.Add("Good");
        graph.labels.Add("Cool");
        graph.labels.Add("Miss");



        changeSpriteSize(graph.gameObject, 700, 600);

        graph.WMG_Ring_Click += MyCoolRingClickFunction;
        graph.WMG_Ring_MouseEnter += MyCoolRingHoverFunction;

    }

    public void CreateRingChart(List<string> arr){
        
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
            string [] result = arr[i].Split(',');
            graph.values.Add(float.Parse(result[1]));
            graph.labels.Add(result[0]);
        }

        changeSpriteSize(graph.gameObject, 600, 500); //resize

        graph.WMG_Ring_Click += MyCoolRingClickFunction;
        graph.WMG_Ring_MouseEnter += MyCoolRingHoverFunction;

        ringGO.GetComponent<RectTransform>().localPosition = new Vector3(-230, -20 ,0);

    }

	void MyCoolRingClickFunction (WMG_Ring ring, UnityEngine.EventSystems.PointerEventData pointerEventData) {
		Debug.Log ("Ring: " + ring.ringIndex + " value: " + ring.graph.values[ring.ringIndex] + " label: " + ring.graph.labels[ring.ringIndex]);
	}

	void MyCoolRingHoverFunction (WMG_Ring ring, bool hover) {
//		Debug.Log ("Hover: " + hover + " Ring: " + ring.ringIndex + " value: " + ring.graph.values[ring.ringIndex] + " label: " + ring.graph.labels[ring.ringIndex]);
	}
}
