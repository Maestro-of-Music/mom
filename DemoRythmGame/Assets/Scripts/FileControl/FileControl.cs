using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;


public class FileControl
{

    private static FileControl _instance = null;

    public static FileControl getInstance()
    {
        if (_instance == null)
        {
            _instance = new FileControl();
        }
        return _instance;
    }


    void Awake()
    {
        _instance = this;
    }

    public int SearchDirectoryFile(string music_name)
    {

        string strFilePath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android! Search Directory File");
            /*
            strFilePath = "jar:file://" + Application.dataPath + "!/assets";
            */
            strFilePath = Application.persistentDataPath + "/";
        }
        else
        {
            Debug.Log("PC! Search Directory File");
            strFilePath = Application.streamingAssetsPath;
        }

        int last_index = 0;

        DirectoryInfo dataDir = new DirectoryInfo(strFilePath);
        try
        {
            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i].Name.Contains("log") && fileinfo[i].Name.Contains(".meta") == false)
                {
                    if (fileinfo[i].Name.Contains(music_name))
                    {
                        last_index++;
                    }
                }
            }


        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return last_index;
    }

    public List<string> GetLogData(string music_name)
    {
        List<string> temp = new List<string>();

        string strFilePath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android! Search Directory File");
            strFilePath = Application.persistentDataPath + "/";

            //load previous data

            object[] array = Resources.LoadAll("logs", typeof(TextAsset));

            char[] text = music_name.ToCharArray();
            char[] temp_text = new char[3];
            temp_text[0] = text[text.Length - 3];
            temp_text[1] = text[text.Length - 2];
            temp_text[2] = text[text.Length - 1];

            string contain = new string(temp_text); 

            Debug.Log("text : " + contain);

            if (array != null)
            {

                for (int i = 0; i < array.Length; i++)
                {
                    TextAsset arr = (TextAsset)array[i];

                    if(arr.name.Contains("log")){
                        string temp_title = arr.name;
                        Debug.Log(temp_title);
                        string[] title = temp_title.Split(')');
                        Debug.Log(title[0] + " " + title[1]);

                        if(title[0].Contains(contain) == true){
                            temp.Add(arr.text);
                        }
                    }   
                }
            }else{
                Debug.Log("No");
            }

        }
        else
        {
            Debug.Log("PC! Search Directory File");
            strFilePath = Application.streamingAssetsPath;
        }

        DirectoryInfo dataDir = new DirectoryInfo(strFilePath);
        int count = 0;

        try
        {
            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i].Name.Contains("log") && fileinfo[i].Name.Contains(".meta") == false)
                {
                    if (fileinfo[i].Name.Contains(music_name))
                    {
                        string url = fileinfo[i].FullName;
                        string data = File.ReadAllText(url);

                        temp.Add(data);
                        count++;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);

        }
        return temp;
    }

    public List<History> LoadHistory(int mode){
        //load history and display to table view
        List<History> historyList = new List<History>();
        string path = "";
        Debug.Log("!!");

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }
        else
        {
            path = Application.dataPath + "/";
        }

        try
        {
            if (File.Exists(path + "history.txt")){

                string LoadHistory = File.ReadAllText(path + "history.txt");
                JsonData load = JsonMapper.ToObject(LoadHistory);

                for (int i = 0; i < load["historyList"].Count; i++)
                {
                    if (int.Parse(load["historyList"][i]["mode"].ToString()) == mode)
                    {
                        History temp = new History();
                        temp.result_Alpha = load["historyList"][i]["result_Alpha"].ToString();
                        temp.score = int.Parse(load["historyList"][i]["score"].ToString());
                        temp.title = load["historyList"][i]["title"].ToString();
                        temp.mode = int.Parse(load["historyList"][i]["mode"].ToString());

                        historyList.Add(temp);
                    }
                }


            }

        }catch(Exception e){
            Debug.Log(e);
        }

        return historyList;
    }

    public void SaveHistory(History data)
    {
        Debug.Log("Save music history");
        string path = "";
        HistoryList arr = new HistoryList();
        List<History> historyList = new List<History>();

        if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }
        else
        {
            path = Application.dataPath + "/";
        }
        try
        {

            if (File.Exists(path + "history.txt") == false)
            {
                //save first history file
                historyList.Add(data);
                arr.historyList = historyList;
                JsonData save = JsonMapper.ToJson(arr);

                File.WriteAllText(path + "history.txt", save.ToString());
                Debug.Log("Save File Created!");

            }
            else
            {
                //save second history file
                string LoadHistory = File.ReadAllText(path + "history.txt");
                JsonData load = JsonMapper.ToObject(LoadHistory);

                for (int i = 0; i < load["historyList"].Count;i++){
                    History temp = new History();
                    temp.result_Alpha = load["historyList"][i]["result_Alpha"].ToString();
                    temp.score = int.Parse(load["historyList"][i]["score"].ToString());
                    temp.title = load["historyList"][i]["title"].ToString();
                    temp.mode = int.Parse(load["historyList"][i]["mode"].ToString()); //game mode save 
                    historyList.Add(temp);
                }

                historyList.Add(data);
                arr.historyList = historyList;
                JsonData save = JsonMapper.ToJson(arr);

                File.WriteAllText(path + "history.txt", save.ToString());
                Debug.Log("Resaved File Created!");
            }
        }catch(Exception e){
            Debug.Log(e.ToString());
        }
    }
}
