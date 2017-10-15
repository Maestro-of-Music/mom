using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {


	public int score; //after change it  

	ScoreManager(){
		score = 0;
	}

	public void SetScore(int duration){
		//propotional duration 
		this.score += duration * 100; //if correct , 1 duration 100 score
 	}

	public int GetScore(){
		return this.score;
	}

	public void InitScore(){
		this.score = 0;
	}

	public void SaveScore(){
	
	}

	public void LoadScore(){
		//previous data load 
	}

}
