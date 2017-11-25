using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoteData
{
	public int octave;
	public string step;
	public int duration;
	public bool rest;
	public int measureIndex;
	public bool alter;
	public bool backward; //check forward or backward
	public string repeat;
}

[System.Serializable]
public class NoteDatas
{
	public List<NoteData> Forward;
	public List<NoteData> Backward;
}