using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveData : MonoBehaviour {

	private static SaveData instance;

	public static SaveData Instance{

		get{
			if (instance == null) {
				instance = new SaveData ();
			} else {
				
			}
			return instance;
		}
	}

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/PlayerInfo.dat", FileMode.Open);

		PlayerDatas data = new PlayerDatas ();

	}
}

[System.Serializable]
class PlayerDatas
{
	public List<PlayerDataInfo> data;
}

[System.Serializable]
class PlayerDataInfo{
	public float Dist;
	public string Opinion;
}