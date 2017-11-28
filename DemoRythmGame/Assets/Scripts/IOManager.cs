using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOManager : MonoBehaviour {

	public GameObject PianoManager;
	public GameObject BluetoothController;
	public int Keyboard_Index;

	//private Bluetooth bluetooth;

	public bool send = false;

	public string GetData;

	void Awake() {
		//this.bluetooth = Bluetooth.getInstance ();
	}

	// Update is called once per frame
	void Update () {
		
		KeyInput (Keyboard_Index);

	}
		
	public void KeyInput(int keyboard_index){
		//get data from arduino 

			/* White Key Input */ 
			if (Input.GetKeyDown (KeyCode.A)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index  + "C" + "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Do 
			else if (Input.GetKeyDown (KeyCode.S)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "D"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Re
			else if (Input.GetKeyDown (KeyCode.D)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index  + "E"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Me 
			else if (Input.GetKeyDown (KeyCode.F)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "F"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Fa 
			else if (Input.GetKeyDown (KeyCode.G)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch +=  keyboard_index + "G"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Sol 
			else if (Input.GetKeyDown (KeyCode.H)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "A"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Ra
			else if (Input.GetKeyDown (KeyCode.J)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch +=  keyboard_index + "B"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Si

			/* Black Key Input */ 
			else if (Input.GetKeyDown (KeyCode.Q)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch +=  keyboard_index + "C#"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Do# 
			else if (Input.GetKeyDown (KeyCode.W)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "D#"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Re#
			else if (Input.GetKeyDown (KeyCode.E)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "F#"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Fa# 
			else if (Input.GetKeyDown (KeyCode.R)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "G#"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Sol# 
			else if (Input.GetKeyDown (KeyCode.T)) {
			PianoManager.gameObject.GetComponent<PianoControl>().pitch += keyboard_index + "A#"+ "/" ;
			Debug.Log ("------" + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

			} // Ra#

			//HardwareInput ();
	}


	public void HardwareInput2(string GetData)
	{
		Debug.Log ("GETDATA : " + GetData);

		if (GetData == "A") {
			Debug.Log ("C Clicked");
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4C";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
		} 
		else if (GetData == "B") {
			Debug.Log ("C# Clicked");
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4C#";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
		
		} 
		else if (GetData == "C") {
			Debug.Log ("D Clicked");
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4D";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
		}
		else if (GetData == "D") {
			Debug.Log ("D# Clicked");
			PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4D#";
			Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

		} 
		else if (GetData == "E") {
			Debug.Log ("E Clicked");
			PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4E";
			Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
		}
			
	}



    //Do Re mi Fa sol Ra Si --select LED color Upload in Observer script 
    public void KeyOutput(string data)
    {
        //send data to arduino 

		Debug.Log ("Result : " + data);

		//if (send == false) {

        if (Application.platform == RuntimePlatform.Android)
        {
         //   this.bluetooth.Send(data);
        }
        //send = true;
    }		
}
