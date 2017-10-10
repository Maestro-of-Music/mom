using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "failCollider") {
			
			Destroy (gameObject);
			Debug.Log (" Failed! ");
		}

		if (other.gameObject.name == "Success") {
			Destroy (gameObject);
			Debug.Log (" Successed! ");

		}
	}
}
