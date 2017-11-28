using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

public class ChartControl : MonoBehaviour {

    private FileControl filecontrol;

    private List<string> loaddata;

    public string music_name;

    public List<LogFile> file;
    public List<string> User_result;

    public GameObject Canvas;
    public Text Answer; 

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

        LoadLogChart();
        LoadScoreChart();
    } 

    void LoadScoreChart(){

        List<string> score = new List<string>(); 

        for (int i = 0; i < file.Count;i++){
            score.Add((i+1) +"th,"+ file[i].score);            
        }

        Canvas.GetComponent<X_Tutorial>().series1Data2 = score;
        Canvas.GetComponent<X_Tutorial>().MakeLineGraph();
    } 

    void LoadLogChart(){

        for (int i = file.Count-1; i < file.Count; i++)
        {

            int Perfect = 0;
            int Good = 0;
            int Cool = 0;
            int Miss = 0;

            for (int j = 0; j < file[i].logdata.Length; j++)
            {
                if (file[i].logdata[j].result == "Perfect")
                {
                    Perfect++;
                }
                else if (file[i].logdata[j].result == "Good")
                {
                    Good++;
                }
                else if (file[i].logdata[j].result == "Cool")
                {
                    Cool++;
                }
                else if (file[i].logdata[j].result == "Miss")
                {
                    Miss++;
                }
            }

            User_result.Add("Perfect" + "," + Perfect.ToString());
            User_result.Add("Good" + "," + Good.ToString());
            User_result.Add("Cool" + "," + Cool.ToString());
            User_result.Add("Miss" + "," + Miss.ToString());

        }

        MakeReference(file[file.Count - 1]);
        this.Canvas.gameObject.GetComponent<WMG_X_Pie_Ring_Graph>().CreateRingChart(User_result);

    }

    // Make Reference to User
    void MakeReference(LogFile log){
        //left hand right hand 

        float left = 0;
        float right = 0;
        float both = 0;

        float leftSum = 0;
        float rightSum = 0;
        float bothSum = 0;

        for (int i = 0; i < log.logdata.Length; i++)
        {
            if (log.logdata[i].left != "" && log.logdata[i].right != ""){
                //if left and right hand pressed
                both++;
                if (log.logdata[i].result == "Perfect")
                {
                    bothSum++;
                }
            }
            else if (log.logdata[i].left != "")
            {
                //if left hand pressed
                left++;
                if (log.logdata[i].result == "Perfect")
                {
                    leftSum++;
                }
            }
            else if (log.logdata[i].right != "")
            {
                //if right hand pressed
                right++;
                if (log.logdata[i].result == "Perfect")
                {
                    rightSum++;
                }
            }
        }

        Debug.Log("LeftHand : " + left + " " + "RightHand : " + right + " Both : " + both);

        string result = "";

        string Bothanswer = (bothSum / both).ToString("0.00%");
        Debug.Log("BothSum : " + (Bothanswer));

        string Leftanswer = (leftSum / left).ToString("0.00%");
        Debug.Log("LeftSum : " + (Leftanswer));

        string Rightanswer = (rightSum / right).ToString("0.00%");
        Debug.Log("RightSum : " + (Rightanswer));


        if (Leftanswer != "NaN"){
            //Debug.Log("There is no Left Hand ");
            result += "leftHand Accurancy is " + (Leftanswer) + "\n";
        }else if(Rightanswer != "NaN"){
            result += "rightHand Accurancy is " + (Rightanswer) + "\n";
        }else if (Bothanswer != "NaN"){
            result += "bothHand Accurancy is " + (Bothanswer) + "\n";
        }

        string temp ="";
        if ((leftSum / left) < (rightSum / right)){

            if ((rightSum / right) < (bothSum / both)){
                temp = "Right Hand";
            }else{
                temp = "Both Hand";
            }

        }else{
            
            if ((leftSum / left) < (bothSum / both))
            {
                temp = "Left Hand";
            }
            else
            {
                temp = "Both Hand";
            }
        }
        result = "More Play " + temp;

        Answer.text = result;
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
            temp.left = getData["logdata"][i]["left"].ToString();
            temp.right = getData["logdata"][i]["right"].ToString();
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
