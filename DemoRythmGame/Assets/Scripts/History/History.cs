using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class History{

    public string result_Alpha;
    public int score;
    public string title;

}

[System.Serializable]
public class HistoryList{
    public List<History> historyList;
}
