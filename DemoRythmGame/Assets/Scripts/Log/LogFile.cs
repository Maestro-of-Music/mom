using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class LogFile{
    public LogData[] logdata;
    public int score;
    public int index; 
}

[System.Serializable]
public class LogData
{
    public int dist; //float multify
    public string result;
    public string left;
    public string right;
    //public int totalScore;
}
