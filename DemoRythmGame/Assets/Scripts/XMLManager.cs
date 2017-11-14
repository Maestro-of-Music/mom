using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.IO;
using LitJson;

public class XMLManager : MonoBehaviour {

    public NoteInfo noteinfo;
    public List<NoteData> arr;
	public List<NoteData> backarr;

    public string _filename; 

     void Awake()
    {
        StartCoroutine("Xml_load", _filename);

		SetNodeJSON ();
    }

	XmlDocument XMLread(){
		
		string strFile = _filename + ".xml"; 
		string strFilePath = Application.persistentDataPath + "/" + strFile; 
		if (!File.Exists(strFilePath)) 
		{ 
			WWW wwwUrl = new WWW("jar:file://" + Application.dataPath + "!/assets/" + strFile); 
			File.WriteAllBytes(strFilePath, wwwUrl.bytes); 
		} 
		XmlDocument Xmldoc = new XmlDocument(); 
		Xmldoc.Load(strFilePath);

		return Xmldoc;
	}


    IEnumerator Xml_load(string Filename)
    {
		XmlDocument xmldoc;

		if (Application.platform == RuntimePlatform.Android) {
			Debug.Log ("Load in Android");
			xmldoc = XMLread ();
		} 

		else {
			Debug.Log ("Load in PC");
			TextAsset textAsset = (TextAsset)Resources.Load(Filename);
			xmldoc = new XmlDocument();
			xmldoc.LoadXml(textAsset.text);		
		}
 

        SetNodeInfo(xmldoc);
        SetNodeData(xmldoc);

        Debug.Log("End!");

        yield return null;
    }

	public void SetNodeJSON(){


		NoteDatas result = new NoteDatas ();
		result.Forward = arr;
		result.Backward = backarr;

		NoteJson notejson = new NoteJson ();
		notejson.NoteInfo.Add(noteinfo);
		notejson.noteDatas = result;

		JsonData data = JsonMapper.ToJson (notejson);

		File.WriteAllText(Application.dataPath + "/Resources/ " +_filename + ".txt",data.ToString());
		Debug.Log ("File Created");
	}

    public void SetNodeInfo (XmlDocument content){

		noteinfo = new NoteInfo ();

		noteinfo.Beats =  (SetBeats (content));
		noteinfo.Beat_type = (SetBeatType (content));
		noteinfo.Measure =  (SetMeasure (content));
		noteinfo.Title = (SetTitle (content));
		noteinfo.Tempo = (SetTempo (content));

	}

    public void SetNodeData(XmlDocument content){
        arr = new List<NoteData>();

		XmlNodeList node = content.GetElementsByTagName("measure");

		for (int i = 0; i < node.Count; i++)
        {
			Debug.Log("Meausure num : " + i);

			XmlElement Ele = (XmlElement)node.Item(i);

			string repeat = RepeatCheck (Ele);
			bool backward = false;

			if (repeat == "forward") { //repeat mode start
				Debug.Log ("!!!");
			}				

			for (int j = 0; j < Ele.GetElementsByTagName("note").Count; j++)
            {
				string step = "";
				int octave = 0;
				int duration = 0;
				bool rest = false;
				bool alter = false;

				XmlElement pitch = (XmlElement)Ele.GetElementsByTagName ("note").Item(j);
				alter = SetAlter (pitch); //check alter 

				Debug.Log (int.Parse (pitch.GetElementsByTagName ("staff").Item (0).InnerText));

				if (int.Parse (pitch.GetElementsByTagName ("staff").Item (0).InnerText) == 1) {
					//forward 
					backward = false;
				} else if (int.Parse (pitch.GetElementsByTagName ("staff").Item (0).InnerText) == 2) {
					//backward
					backward = true;
				}

                try
                {
					step = pitch.GetElementsByTagName("step").Item(0).InnerText;
					octave = int.Parse(pitch.GetElementsByTagName("octave").Item(0).InnerText);
					rest = false;

                }catch(Exception){
                    step = "";
                    octave = -1;
                    rest = true;
                }
				finally{
					duration = 	int.Parse(pitch.GetElementsByTagName("duration").Item(0).InnerText);
					Debug.Log (duration);

                    NoteData temp = new NoteData();
					temp.step = step;
					temp.octave = octave;
					temp.duration = duration;
					temp.measureIndex = (i+1) ;
					temp.rest = rest;
					temp.alter = alter;
					temp.repeat = repeat; //repeat mode 
					temp.backward = backward; //forward or backward 

					if (temp.backward == true) {
						backarr.Add (temp);
					} else {
						arr.Add (temp);
					}
                }
            }

			/*
			if (repeat == "backward") {
				Debug.Log ("!!!");
			}	
			*/

			Debug.Log ("--------------------------");
        }
    }

	public bool backwardChange(bool content){

		if (content) {
			return false;
		} else {
			return true;
		}
	}

	public string RepeatCheck(XmlElement content){

		string repeat = "";
		try{
			repeat = content.GetElementsByTagName("repeat").Item(0).Attributes.GetNamedItem("direction").InnerText;
		}catch(Exception){
			Debug.Log ("no repeat");
		}finally{
			if (repeat == "forward")
				Debug.Log ("Repeat start!");
			else if(repeat == "backward") {
				Debug.Log ("Repeat end");
			}
		}
		return repeat;
	}

	bool SetAlter(XmlElement content){
		bool alter = false;
		int num = 0;

		try{
			num = int.Parse(content.GetElementsByTagName("alter").Item(0).InnerText);
		}catch(Exception){
			num = 0;
		}finally{
			if (num != 0)
				alter = true;
		}
	
		return alter;
	}

	public string SetTitle(XmlDocument content)
	{
		string title = "";

		try{
			title = content.GetElementsByTagName("work-title").Item(0).InnerText;
		}
		catch(Exception){
			title = "Untitled";
		}
		Debug.Log (title);
		return title;
	}

	public int SetMeasure(XmlDocument content){
		int measure;
		measure = content.GetElementsByTagName ("measure").Count;
		Debug.Log (measure);
		return measure;
	}
		
	public int SetBeats(XmlDocument content){
		int beat = 0; 

		beat = int.Parse (content.GetElementsByTagName ("beats").Item (0).InnerText);
		Debug.Log (beat);

		return beat;
	}

	public int SetBeatType(XmlDocument content){
		int beat_type = 0;
		beat_type = int.Parse (content.GetElementsByTagName ("beat-type").Item (0).InnerText);
		Debug.Log (beat_type);

		return beat_type;
	}

	public int SetTempo(XmlDocument content){
		int tempo = 0;

		tempo = int.Parse(content.GetElementsByTagName ("sound").Item(0).Attributes.GetNamedItem("tempo").InnerText);
		Debug.Log (tempo);

		return tempo;
	}



}




public class NoteJson
{
	public List<NoteInfo> NoteInfo;
	public NoteDatas noteDatas;

	public NoteJson(){
		NoteInfo = new List<NoteInfo> ();
		noteDatas = new NoteDatas ();
	}
}
	