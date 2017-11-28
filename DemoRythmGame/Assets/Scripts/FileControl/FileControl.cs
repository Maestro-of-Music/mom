using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using LitJson;


public class FileControl{

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

    public int SearchDirectoryFile(string music_name){
        
        string strFilePath = Application.streamingAssetsPath;
        int last_index = 0;
        //int count = 0;

        DirectoryInfo dataDir = new DirectoryInfo(strFilePath);
        try{
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

     
        }catch(Exception e){
            Debug.Log(e);
        }

        return last_index;
    }

    public List<string> GetLogData(string music_name){
        List<string> temp = new List<string>();

        string path = Application.streamingAssetsPath;
        Debug.Log(path);
        DirectoryInfo dataDir = new DirectoryInfo(path);

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


}
