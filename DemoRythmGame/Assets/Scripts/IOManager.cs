using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOManager : MonoBehaviour {

	public GameObject PianoManager;
	public GameObject BluetoothController;
//	public Text UIText; 
	//여기서 인풋 개수 만큼 배열을 늘림 

	private Bluetooth bluetooth;

	bool click = true;
	public bool send = false;

	public string GetData;

	void Awake() {
		this.bluetooth = Bluetooth.getInstance ();
	}

	// Update is called once per frame
	void Update () {
		
		//KeyInput ();
	}
		
	public void KeyInput(){
		//get data from arduino 

			/* White Key Input */ 
			if (Input.GetKeyDown (KeyCode.A)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4C";

			} // Do 
			else if (Input.GetKeyDown (KeyCode.S)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4D";

			} // Re
			else if (Input.GetKeyDown (KeyCode.D)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4E";

			} // Me 
			else if (Input.GetKeyDown (KeyCode.F)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4F";

			} // Fa 
			else if (Input.GetKeyDown (KeyCode.G)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4G";

			} // Sol 
			else if (Input.GetKeyDown (KeyCode.H)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4A";

			} // Ra
			else if (Input.GetKeyDown (KeyCode.J)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4B";

			} // Si

			/* Black Key Input */ 
			else if (Input.GetKeyDown (KeyCode.Q)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4C#";

			} // Do# 
			else if (Input.GetKeyDown (KeyCode.W)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4D#";

			} // Re#
			else if (Input.GetKeyDown (KeyCode.E)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4F#";

			} // Fa# 
			else if (Input.GetKeyDown (KeyCode.R)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4G#";

			} // Sol# 
			else if (Input.GetKeyDown (KeyCode.T)) {
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4A#";

			} // Ra#

			//HardwareInput ();
	}

	void ConvertGetData(){
		//parsing part 

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
	/*
	public void HardwareInput(string GetData){
		//if(BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData != null)
		//Debug.Log ("Bluetooth GetData : " + BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData);
		Debug.Log("GETDATA : " + GetData);

		if (GetData == "A") {
			Debug.Log ("C Clicked");
			if (click) {

				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4C";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = null;
				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		} 
		if (GetData == "C") {

			if (click) {

				//UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4D";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = null;

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		} 
		if (BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData == "E") {

			if (click) {

				//UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4E";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = null;

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}
		}


		 if (GetData == "B"){
			
			if (click) {

				//UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4C#";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = null;

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		} 

		if (BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData == "D") {

			if (click) {

				//UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = "4D#";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = null;

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		}

	}
*/
	//Do Re mi Fa sol Ra Si --select LED color Upload in Observer script 
	public void KeyOutput(string data){
		//send data to arduino 
		string start = "#";
		string end = "/";

		string result = start + data + end; 

		Debug.Log ("Result : " + result);

		//if (send == false) {
		Debug.Log("Data : " + result);
		if(Application.platform == RuntimePlatform.Android){
			this.bluetooth.Send (result);
		}

		//	send = true;
	//	}

	
	}
		
}
