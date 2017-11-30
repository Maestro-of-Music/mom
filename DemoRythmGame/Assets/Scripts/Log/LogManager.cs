using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {

    public GameObject LogObject;
    private FileControl filecontrol;
    public GameObject GameManager;
    public List<String> loadData;
    public int logcount = 0;

    public int Perfect_Count;
    public int Miss_Count;

    public GameObject perfect;
    public GameObject good;
    public GameObject cool;
    public GameObject miss;

    public GameObject temp;
    public Image HealthBar;

    void Awake(){
        this.filecontrol = FileControl.getInstance();
        Perfect_Count = 0;
        Miss_Count = 0;
    }

    public void MakeScoreTitle(string result){
        if(result == "Perfect"){
            temp = perfect;
        }else if (result == "Good"){
            temp = good;
        }else if (result == "Cool"){
            temp = cool;
        }else if(result == "Miss"){ //miss
            temp = miss;
        }
        PlayerLifeControl(result);

        //call 
        temp.SetActive(true);
        temp.GetComponent<ScoreTitleControl>().ImageInit();
    }

    public void PlayerLifeControl(string result){

        if(result == "Perfect"){
            if (gameObject.GetComponent<PianoControl>().Player_Life <= 100){
                gameObject.GetComponent<PianoControl>().Player_Life += 5;
            }

        }else if(result == "Good"){

            if (gameObject.GetComponent<PianoControl>().Player_Life < 100)
            {
                gameObject.GetComponent<PianoControl>().Player_Life += 2;
            }

        }else if(result == "Cool"){

            if (gameObject.GetComponent<PianoControl>().Player_Life < 100)
            {
                gameObject.GetComponent<PianoControl>().Player_Life += 0;
            }
        }else if(result == "Miss"){
            if (gameObject.GetComponent<PianoControl>().Player_Life > 0)
            {
                gameObject.GetComponent<PianoControl>().Player_Life -= 30;

            }else{
                Debug.Log("Game End!");
                GameManager.GetComponent<MenuManager>().GameEnd();
            }
        }

        if(gameObject.GetComponent<PianoControl>().Player_Life > 100){
            gameObject.GetComponent<PianoControl>().Player_Life = 100;   
        }

        Debug.Log(gameObject.GetComponent<PianoControl>().Player_Life);
        UpdateLife(gameObject.GetComponent<PianoControl>().Player_Life);
    }

    void UpdateLife(int life){
        float temp = ((float)life * 0.01f);
        HealthBar.transform.localScale = new Vector3(temp, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
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

            if(temp.result == "perfect" || temp.result == "Perfect"){
                Perfect_Count++;
            }else if(temp.result == "miss" || temp.result == "Miss"){
                Miss_Count++;
            }
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
        string url = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            //persistence 

        }
        else
        {
            url = Application.streamingAssetsPath + "/" + "(" + music_name + ")" + "log" + a.ToString() + ".txt";

            //Find data URL 

            if (File.Exists(url) == true)
            {
                //rewrite
                JsonData data = JsonMapper.ToJson(log);
                a++;

                url = Application.streamingAssetsPath + "/" + "(" + music_name + ")" + "log" + a.ToString() + ".txt";
                File.WriteAllText(url, data.ToString());
            }
            else
            {
                log.index = 1;
                JsonData data = JsonMapper.ToJson(log);
                File.WriteAllText(url, data.ToString());
            }
        }
    }
}
