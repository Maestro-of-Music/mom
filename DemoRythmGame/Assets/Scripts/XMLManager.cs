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
    public XmlDocument _XmlDoc;

    public string _filename = "";

    private static XMLManager _instance = null;

    public static XMLManager getInstance(){
        if(_instance == null){
            _instance = new XMLManager();
        }
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    public void RunningXmlLoad(XmlDocument xmldoc, string file_name){
        Debug.Log(file_name);
        _XmlDoc = xmldoc;

        this._filename = file_name;
        Debug.Log(xmldoc.InnerText);
               
        try

        {
            SetNodeInfo(xmldoc);
            SetNodeData(xmldoc);
            SetNodeJSON();
        }catch(Exception e){
            Debug.Log(e);
        }

        Debug.Log("XML converting Complete!");
	}


    IEnumerator Xml_load(string Filename)
    {
		XmlDocument xmldoc;
        xmldoc = _XmlDoc;

		if (Application.platform == RuntimePlatform.Android) {
			Debug.Log ("Load in Android");
            xmldoc = _XmlDoc;
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

   /*
    XmlDocument XMLread()
    {
        if (File.Exists(StrFilePath))
        {
            WWW wwwUrl = new WWW(StrFilePath);
            yield return wwwUrl;

            File.WriteAllBytes(StrFilePath, wwwUrl.bytes);
        }else{
            Debug.Log("File not Found!");

        }

        Debug.Log("File writing");
        File.WriteAllBytes(StrFilePath, XmlFile.bytes);

        Debug.Log("load!");
        XmlDocument Xmldoc = new XmlDocument();
        Xmldoc.Load(StrFilePath);

        return Xmldoc;
    }

    */

	public void SetNodeJSON(){


		NoteDatas result = new NoteDatas ();
		result.Forward = arr;
		result.Backward = backarr;

		NoteJson notejson = new NoteJson ();
		notejson.NoteInfo.Add(noteinfo);
		notejson.noteDatas = result;

		JsonData data = JsonMapper.ToJson (notejson);

		try{

		if(Application.platform == RuntimePlatform.Android){
                Debug.Log("_filename : " + _filename);
			File.WriteAllText(Application.persistentDataPath + "/" +_filename + ".txt",data.ToString());
		}
		else{
			File.WriteAllText(Application.dataPath + "/Resources/ " +_filename + ".txt",data.ToString());
		}

		Debug.Log ("File Created");
		}catch(Exception e){
			Debug.Log(e);
		}
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

                    if(duration > 120){
                        duration = duration / 120;
                    }

                    int default_x = int.Parse(pitch.Attributes.GetNamedItem("default-x").InnerText);
                    //Debug.Log(default_x + " Default - x");

                    NoteData temp = new NoteData();
					temp.step = step;
					temp.octave = octave;
					temp.duration = duration;
                    temp.default_x = default_x;
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
        try
        {
            tempo = int.Parse(content.GetElementsByTagName("sound").Item(0).Attributes.GetNamedItem("tempo").InnerText);
        }catch(Exception)
        {
            Debug.Log("!!!!!!!!!!" + content.GetElementsByTagName("sound").Count);
        }
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
	