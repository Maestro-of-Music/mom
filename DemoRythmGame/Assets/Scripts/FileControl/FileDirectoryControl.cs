using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class FileDirectoryControl : MonoBehaviour {

    public GameObject ItemObject;
    public GameObject HistoryObject;
    public Transform Content;
    public List<Item> ItemList;
    private FileControl filecontrol;
    private SceneChange scenechange;

    public int PlayMode;
    public bool History; //History or PlayPractice 

    public string selected = "#ffe3de";
    public string unselected = "#b57e75";


    /* Play Mode
     * 1 - Play
     * 2 - Practice
     * 3 - History
    */
    private void Awake()
    {
        this.filecontrol = FileControl.getInstance();
        this.scenechange = SceneChange.getInstance();
    }

    void Start()
    {
        if(History){
            OnHistoryLoad();
        }else{
            OnFileLoad();
        }
    }

    public void OnBack(){
        this.scenechange.NextScene("Main");
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
                    string name = arr.name;
                    Binding(name);
                }
            }

            Debug.Log("Resource Files Load!");

            Debug.Log("Now Load in Persistent Data path");

            strFilePath = Application.persistentDataPath + "/" ;  

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
//                         Debug.Log(first);
                         string name = fileinfo[i].Name.Substring(0,first);
                        // Debug.Log("Name : " + name);
                         Binding(name);
                    }
                }
            }
            catch(Exception e){
                Debug.Log(e);
            } 
        }
    }


    public void OnHistoryLoad() //PlayMode
    {
        List<History> temp = this.filecontrol.LoadHistory(PlayMode);
        for (int i = 0; i < temp.Count;i++){
            HistoryBinding(temp[i]);
        }
    }

    public void HistoryBinding(History temp){
        
        GameObject btnItemTemp = Instantiate(this.ItemObject) as GameObject;
        btnItemTemp.GetComponent<ItemObject>().Title.text = temp.title;
        btnItemTemp.GetComponent<ItemObject>().Detail.text = temp.score.ToString();
        btnItemTemp.GetComponent<ItemObject>().Btn.name = temp.result_Alpha;

        btnItemTemp.transform.SetParent(this.Content);
        btnItemTemp.transform.localScale = new Vector3(1, 1, 1);
    }

    public void Binding(string file){
    
        GameObject btnItemTemp = Instantiate(this.ItemObject) as GameObject;
        btnItemTemp.GetComponent<ItemObject>().Title.text = file;
        btnItemTemp.GetComponent<ItemObject>().Detail.text = "";
        btnItemTemp.GetComponent<ItemObject>().Btn.name = file;

        if (file.Contains("a"))
        {
            btnItemTemp.GetComponent<ItemObject>().Icon.sprite = btnItemTemp.GetComponent<ItemObject>().After;
        }
        btnItemTemp.transform.SetParent(this.Content);
        btnItemTemp.transform.localScale = new Vector3(1, 1, 1);
    }

}
