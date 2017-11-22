using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public class ChartControl : MonoBehaviour {

    private FileControl filecontrol;

    private List<string> loaddata;

    public string music_name;

    public List<LogFile> file;

    private void Awake()
    {
        this.filecontrol = FileControl.getInstance();
    }

    // Use this for initialization
    void Start () {

        StartCoroutine("LoadGetLogData",music_name);
        loadLogData();
    }


    void loadLogData(){

        for (int i = 0; i < loaddata.Count; i++)
        {
            StartCoroutine("LoadJsonBuilder", loaddata[i]);

        }
    } 

    IEnumerator LoadJsonBuilder(string log){

        JsonData getData = JsonMapper.ToObject(log);
        LogFile logFile = new LogFile();
        LogData[] logdata = new LogData[getData["logdata"].Count];

        for (int i = 0; i < getData["logdata"].Count; i++)
        {
            LogData temp = new LogData();
            temp.dist = int.Parse(getData["logdata"][i]["dist"].ToString());
            temp.result = getData["logdata"][i]["result"].ToString();
            logdata[i] = temp;
        }

        logFile.logdata = logdata;
        logFile.score = int.Parse(getData["score"].ToString());
        logFile.index = int.Parse(getData["index"].ToString());

        file.Add(logFile);

        yield return null;
    }

    IEnumerator LoadGetLogData(string music_name){
        loaddata = this.filecontrol.GetLogData(music_name); //send music data before scene
        yield return new WaitForSeconds(3);
    }

	
}
