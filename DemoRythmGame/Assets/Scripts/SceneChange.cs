using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

	public string Scene;

	void Awake(){
		Scene = "Test";
	}

	public void NextScene(){
		Debug.Log ("Move Next Scene");
		SceneManager.LoadScene (Scene);
	}
}
