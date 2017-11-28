using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;


public class LogManager : MonoBehaviour {

    public GameObject LogObject;
    private FileControl filecontrol;
    public GameObject GameManager;
    public List<String> loadData;

    void Awake(){
        this.filecontrol = FileControl.getInstance();
    }

    void Start(){
//        Debug.Log("Title: " + GameManager.GetComponent<CreateNote>().title);
    }

    public void MakeLogObject(int count){
        GameObject note = (GameObject)Instantiate(LogObject, new Vector3(0, gameObject.GetComponent<Transform>().position.y + 1.6f, 2.85f), Quaternion.identity);
        note.GetComponent<LogControl>().count_Keyboard = count;        
    }
	
    //setting calculation of log data

    public void CollectLogObject(int score, string music_name){
        GameObject[] log = GameObject.FindGameObjectsWithTag("Log");

        LogData[] logtempFile = new LogData[log.Length];

        Debug.Log("Log!!!!!!! " + log.Length);

        for (int i = 0; i < log.Length;i++){
            LogData temp = new LogData();
            temp.result = log[i].GetComponent<LogControl>().result;
            temp.dist = (int)(log[i].GetComponent<LogControl>().dist * 100);
            temp.left = log[i].GetComponent<LogControl>().leftHand;
            temp.right = log[i].GetComponent<LogControl>().rightHand;
            logtempFile[i] = temp;
        }

        LogFile a = new LogFile();
        a.score = score;
        a.logdata = logtempFile;

        SaveLog(a,music_name);
    }

    public void SaveLog(LogFile log, string music_name){
        Debug.Log("Save!");

        int a =  filecontrol.SearchDirectoryFile(music_name);
        Debug.Log(a);

        string url = Application.streamingAssetsPath + "/" + "(" +  music_name + ")" +"log" + a.ToString()+".txt";

        //Find data URL 

        if (File.Exists(url) == true)
        {
            //rewrite
            JsonData data = JsonMapper.ToJson(log);
            a++;

            url = Application.streamingAssetsPath + "/" + "(" + music_name + ")" + "log" + a.ToString()+".txt";
            File.WriteAllText(url,data.ToString());
        }
        else
        {
            log.index = 1;
            JsonData data = JsonMapper.ToJson(log);
            File.WriteAllText(url,data.ToString());
        }
    }
}
