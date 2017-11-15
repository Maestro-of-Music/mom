using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDirectoryControl : MonoBehaviour {

    void Start()
    {
        OnFileLoad();        
    }

    public void OnFileLoad(){
        /* 
            if android platform must add Application dataPath
        */

        string strFilePath = Application.dataPath;
 
        DirectoryInfo dataDir = new DirectoryInfo(strFilePath + "/Resources/");
        try{
            FileInfo[] fileinfo = dataDir.GetFiles();
            for (int i = 0; i < fileinfo.Length; i++)
            {
                if (fileinfo[i].Name.Contains(".txt") && fileinfo[i].Name.Contains(".meta") == false){ 
                    string name = fileinfo[i].Name;
                    Debug.Log("Name : " + name);
                }
            }
        }catch(Exception e){
            Debug.Log(e);
        } 
    }

    public void SetTextFile(){
        //setting xml to text file


    }
}
