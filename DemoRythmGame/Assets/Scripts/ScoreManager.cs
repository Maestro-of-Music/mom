﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	//public Text ScoreDisplay;
	public int score; //after change it  

	ScoreManager(){
		score = 0;
	}

	public void SetScore(int duration){
		//propotional duration 
		if(gameObject.GetComponent<PianoControl>().Scorechange){
			//ScoreDisplay.text = (duration * 100).ToString(); 
			this.score += duration * 100; //if correct , 1 duration 100 score
			gameObject.GetComponent<PianoControl>().Scorechange = false;
		}
 	}

	public int GetScore(){
		return this.score;
	}

	public void InitScore(){
		this.score = 0;
	}

}
