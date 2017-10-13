using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Xml;

public class JsonTest : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		/*
		//string filePath = Application.streamingAssetsPath+ "/school.json";

		string filePath = Path.Combine(Application.streamingAssetsPath, "school.xml");
		//string filePath2 = Path.Combine(Application.streamingAssetsPath+"/Resoureces/", "school.txt");
		string filePath3 = Path.Combine(Application.streamingAssetsPath+"/StreamingAssets/", "school.txt");

		StartCoroutine ("Test",filePath);
		//StartCoroutine ("Test",filePath2);
		//StartCoroutine ("Test",filePath3);

		/*
		string folderPath = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath);
		string filePath1 = folderPath + "school" + ".json";
		string filePath2 = folderPath + "/Resoureces/school" + ".json";
		string filePath3 = folderPath + "/StreamingAssets/school" + ".json";
		*/

		//string filePath = Path.Combine(Application.streamingAssetsPath, "school.txt"); //

		//StartCoroutine ( "Test" , filePath ); //text 
		//json test

		//queueTest ();
	}

	void XMLread(){
		string strFile = "school.xml"; 
		string strFilePath = Application.persistentDataPath + "/" + strFile; 
		if (!File.Exists(strFilePath)) 
		{ 
			WWW wwwUrl = new WWW("jar:file://" + Application.dataPath + "!/assets/" + strFile); 
			File.WriteAllBytes(strFilePath, wwwUrl.bytes); 
		} 
		XmlDocument Xmldoc = new XmlDocument(); 
		Xmldoc.Load(strFilePath);

		Debug.Log (Xmldoc.InnerText);
	}

	IEnumerator JSONread(){
		string strFile = "school.json"; 
		string strFilePath = Application.persistentDataPath + "/" + strFile;
		/*
		if (!File.Exists(strFilePath)) 
		{ 
			WWW wwwUrl = new WWW("jar:file://" + Application.dataPath + "!/assets/" + strFile); 
			File.WriteAllBytes(strFilePath, wwwUrl.bytes); 

		} 
		string dataAsJson = File.ReadAllText (strFilePath);
		Debug.Log ("JSON : " + dataAsJson);
		*/

		WWW www = new WWW(strFilePath); 
		yield return www;

		if (www.text != null) {
			string dataAsJson = www.text;
			Debug.Log (dataAsJson);
		} else {
			Debug.Log ("Can't read Json");
		}
			
	}


	IEnumerator Test(string path){
		
		string dataAsJson;
		if (path.Contains ("://")) {
			WWW www = new WWW (path);
			yield return www;
			dataAsJson = www.text;
		} else {
			dataAsJson = File.ReadAllText (path); 
		}
		Debug.Log (dataAsJson);
	}



	void queueTest(){
		List <string> arr = new List<string>();
		arr.Add ("0");
		arr.Add ("1");
		arr.Add ("2");
		arr.Add ("3");
		arr.Add ("4");
		arr.Add ("5");
		arr.Add ("6");

		if (arr.Contains("0")) {
			Debug.Log ("Exist!!");
		}
	}

	void FileExist(string filename){

		string path= Application.persistentDataPath + "/" ;
		DirectoryInfo dataDir = new DirectoryInfo (path);
		try {
			FileInfo[] fileinfo = dataDir.GetFiles ();
			for (int i=0; i<fileinfo.Length; i++) {
				string name=fileinfo [i].Name;
				Debug.Log("name  "+name);
			}
		} catch (System.Exception e) {
			Debug.Log (e);
		}
	}


}
