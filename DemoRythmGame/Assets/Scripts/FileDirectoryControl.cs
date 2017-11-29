using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class FileDirectoryControl : MonoBehaviour {

    public GameObject ItemObject;
    public Transform Content;
    public List<Item> ItemList;

    void Start()
    {
        OnFileLoad();
    }

    public void OnFileLoad(){
        /* 
            if android platform must add Application dataPath
        */
        string strFilePath = "";

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android!!");

            TextAsset a = Resources.Load<TextAsset>("march");
            object [] array = Resources.LoadAll("files",typeof(TextAsset));


            Debug.Log(a.text);

            if(array != null){
                Debug.Log(array.Length);

                for (int i = 0; i < array.Length;i++){
                    TextAsset arr = (TextAsset)array[i];
                    Debug.Log(arr.name);
                    //int first = arr.name.IndexOf(".txt");
                    //Debug.Log(first);
                    string name = arr.name;
                    //Debug.Log("Name : " + name);
                    Binding(name);
                }
            }
            Debug.Log("Load!");

        }
        else
        {
             strFilePath = Application.dataPath + "/Resources/";
       
              DirectoryInfo dataDir = new DirectoryInfo(strFilePath);
             try{
                 FileInfo[] fileinfo = dataDir.GetFiles();
             for (int i = 0; i < fileinfo.Length; i++)
             {
                    if (fileinfo[i].Name.Contains(".txt") && fileinfo[i].Name.Contains(".meta") == false){
                         int first = fileinfo[i].Name.IndexOf(".txt");
                         Debug.Log(first);
                         string name = fileinfo[i].Name.Substring(0,first);
                         Debug.Log("Name : " + name);
                         Binding(name);
                    }
                }
            }
            catch(Exception e){
                Debug.Log(e);
            } 
        }

    

    }

    void Binding(string file){
    
        GameObject btnItemTemp = Instantiate(this.ItemObject) as GameObject;
        btnItemTemp.GetComponent<ItemObject>().Title.text = file;
        btnItemTemp.GetComponent<ItemObject>().Detail.text = "";
        btnItemTemp.GetComponent<ItemObject>().Btn.name = file;

        btnItemTemp.transform.SetParent(this.Content);
    }

}
