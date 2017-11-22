using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;


public class LogManager : MonoBehaviour {

    public GameObject LogObject;
    private FileControl filecontrol;

    void Awake(){
        this.filecontrol = FileControl.getInstance();
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
            Debug.Log("result : " + log[i].GetComponent<LogControl>().result);
            temp.result = log[i].GetComponent<LogControl>().result;
            temp.dist = (int)(log[i].GetComponent<LogControl>().dist * 100);
            Debug.Log("temp : " + temp.result);
            logtempFile[i] = temp;
        }

        LogFile a = new LogFile();
        a.score = score;
        a.logdata = logtempFile;

        SaveLog(a,music_name);
    }

    public void SaveLog(LogFile log, string music_name){
        Debug.Log("Save!");

        //DirectoryInfo dataDir = new DirectoryInfo(Application.dataPath + "/Resources/");

        int a =  filecontrol.SearchDirectoryFile(music_name);
        Debug.Log(a);

        string url = Application.dataPath + "/Resources/" + "(" +  music_name + ")" +"log" + a.ToString();

        //Find data URL 

        if (File.Exists(url) == true)
        {
            //rewrite
            JsonData data = JsonMapper.ToJson(log);
            a++;
            url = Application.dataPath +"/Resources/" + "(" + music_name + ")" + "log" + a.ToString();
            File.WriteAllText(url,data.ToString());
        }
        else
        {
            log.index = 1;
            JsonData data = JsonMapper.ToJson(log);
            File.WriteAllText(url,data.ToString());
        }

    }

    public void LoadLog(string music_name){
        Debug.Log("Log Load!");

        string[] saveFile;

        string strFilePath = Application.dataPath;
        DirectoryInfo dataDir = new DirectoryInfo(strFilePath + "/Resources/");

        try{
            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length;i++){
                
            }
        }catch(Exception e){
            Debug.Log(e);
        }



    }

}
