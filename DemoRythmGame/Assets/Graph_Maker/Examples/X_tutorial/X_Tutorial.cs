using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class X_Tutorial : MonoBehaviour {

    public GameObject emptyGraphPrefab;

    public WMG_Axis_Graph graph;

    public WMG_Series series1;

    public List<Vector2> series1Data;

    public bool useData2;

    public List<string> series1Data2;


	// Use this for initialization
	void Start () {
        GameObject graphGo = (GameObject)Instantiate(emptyGraphPrefab);
        graphGo.transform.SetParent(this.transform, false);
        graph = graphGo.GetComponent<WMG_Axis_Graph>();

        series1 = graph.addSeries();
        graph.xAxis.AxisMaxValue = 5;

        if(useData2){
            List<string> groups = new List<string>();
            List<Vector2> data = new List<Vector2>();

            for (int i = 0; i < series1Data2.Count;i++){
                string[] row = series1Data2[i].Split(',');

                Debug.Log(row[0]);
                groups.Add(row[0]);

                float y = float.Parse(row[1]);
                data.Add(new Vector2(i+1,y));
            }

            graph.groups.SetList(groups);
            graph.useGroups = true;

            graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
            series1.pointValues.SetList(data);
            series1.UseXDistBetweenToSpace = true;
            series1.AutoUpdateXDistBetween = true;

        }else{
            series1.pointValues.SetList(series1Data);   
        }

	}
	


}
