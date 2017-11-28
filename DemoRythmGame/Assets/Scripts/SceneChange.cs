using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange{

    public string Music_title; //selected Music title


    private static SceneChange _instance = null;

    public static SceneChange getInstance()
    {
        if(_instance == null){
            _instance = new SceneChange();
        }
        return _instance;
    }

	void Awake(){
        _instance = this;
    }

	public void NextScene(string scene, string music_title){
		Debug.Log ("Move Next Scene");
        this.Music_title = music_title;
        SceneManager.LoadScene (scene);
	}
}
