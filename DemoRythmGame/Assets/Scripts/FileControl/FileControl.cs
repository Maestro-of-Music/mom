using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class FileControl : MonoBehaviour {

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
        string strFilePath = Application.dataPath;
        int last_index = 0;
        int count = 0;

        DirectoryInfo dataDir = new DirectoryInfo(strFilePath + "/Resources/");
        try{
            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i].Name.Contains(".txt") && fileinfo[i].Name.Contains(".meta") == false)
                {
                    if (fileinfo[i].Name.Contains("log") && fileinfo[i].Name.Contains(music_name)){
                        int first = fileinfo[i].Name.IndexOf(".txt");
                        string name = fileinfo[i].Name.Substring(0, first);

                        Debug.Log("Found!");
                        last_index = int.Parse(name.Substring(name.Length - 1, name.Length));
                        Debug.Log("last index :" + last_index);
                    }else{
                        count++;
                    }
                }
            }

            if (count == fileinfo.Length -1){
                Debug.Log("no file");
                last_index = 0;
            }

        }catch(Exception e){
            Debug.Log(e);
        }


        return last_index;
    }


}
