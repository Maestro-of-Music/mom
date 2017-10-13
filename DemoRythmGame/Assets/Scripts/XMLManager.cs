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
        //TextAsset textAsset = (TextAsset)Resources.Load(Filename);
		//
		/*
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(textAsset.text);
		*/
		XmlDocument xmldoc = XMLread ();

        SetNodeInfo(xmldoc);
        SetNodeData(xmldoc);

        Debug.Log("End!");

        yield return null;
    }

	public void SetNodeJSON(){
	
		NoteJson notejson = new NoteJson ();
		notejson.NoteInfo.Add(noteinfo);
		notejson.NoteData = arr;

		JsonData data = JsonMapper.ToJson (notejson);

		File.WriteAllText(Application.dataPath + "/Resources/ " +_filename + ".json",data.ToString());

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

		for (int i = 0; i < noteinfo.Measure; i++)
        {

			XmlElement Ele = (XmlElement)node.Item(i);

            for (int j = 0; j < Ele.GetElementsByTagName("note").Count; j++)
            {
				string step = "";
				int octave = 0;
				int duration = 0;
				bool rest = false;

                try
                {
                    step = Ele.GetElementsByTagName("step").Item(j).InnerText;
                    octave = int.Parse(Ele.GetElementsByTagName("octave").Item(j).InnerText);
					rest = false;

                }catch(Exception){
                    step = "";
                    octave = -1;
                    rest = true;
                }
				finally{
					duration = int.Parse(Ele.GetElementsByTagName("duration").Item(j).InnerText);
                    NoteData temp = new NoteData();
					temp.step = step;
					temp.octave = octave;
					temp.duration = duration;
					temp.measureIndex = (i+1) ;
					temp.rest = rest;
                    arr.Add(temp);
                }
            }
        }
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
	public List<NoteData> NoteData;

	public NoteJson(){
		NoteInfo = new List<NoteInfo> ();
		NoteData = new List<NoteData> ();
	}
}
	