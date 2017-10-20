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
}