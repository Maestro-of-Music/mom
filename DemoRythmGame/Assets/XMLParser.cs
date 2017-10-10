using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Text;
using System.Xml.Linq;

public class XMLParser : MonoBehaviour {

	public int tempo;
	public string filename = "two";
	private string path;

	// Use this for initialization
	void Start () {
		loadXML ();
	}

	void loadXML(){
	}

	IEnumerator XMLRead(string content){

		XmlDocument doc = new XmlDocument();
		doc.LoadXml (content);


		//Display all the book titles.
		XmlNodeList elemList = doc.GetElementsByTagName("sound");
		for (int i=0; i < elemList.Count; i++)
		{   
			print(elemList[i].InnerXml);
		}  

		yield return null;  
	}
}
