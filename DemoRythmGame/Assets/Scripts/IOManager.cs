using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IOManager : MonoBehaviour {

	public GameObject PianoManager;
	public GameObject BluetoothController;
	public Text UIText;

	//private Bluetooth bluetooth;

	bool click = true;
	public bool send = false;

	void Awake() {
		//this.bluetooth = Bluetooth.getInstance ();
	}

	// Update is called once per frame
	void Update () {
		KeyInput ();
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

			HardwareInput ();
	}

	public void HardwareInput(){
		
		if (BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData == "A") {

			if (click) {

				UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl> ().pitch = PianoManager.gameObject.GetComponent<PianoControl> ().Target_pitch;
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		} else if (BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData == "B") {

			if (click) {

				UIText.text = BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData;
				PianoManager.gameObject.GetComponent<PianoControl>().pitch = "4A";
				Debug.Log ("Pitch : " + PianoManager.gameObject.GetComponent<PianoControl> ().pitch);

				click = false;
			} else {
				BluetoothController.gameObject.GetComponent<BluetoothIOManager> ().GetData = "";
				click = true;
			}

		}

	}
	//Do Re mi Fa sol Ra Si --select LED color Upload in Observer script 
	public void KeyOutput(string data){
		//send data to arduino 
		if (send == false) {
			Debug.Log("Data : " + data);
			if(Application.platform == RuntimePlatform.Android){
			//	this.bluetooth.Send (data);
			}

			send = true;
		}

	
	}
		
}
