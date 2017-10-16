using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class BluetoothIOManager : MonoBehaviour,IBtObserver {

	private Bluetooth bluetooth;

	[SerializeField]
	private BluetoothModel bluetoothModel;

	[SerializeField]
	public string GetData;


	private void Awake() {
		this.bluetooth = Bluetooth.getInstance();
	}
		
	private void Start () {
		this.bluetoothModel.AddObserver(this);

	}

	public void OnStateChanged(string _State){}

	public void OnSendMessage(string _Message){
		
	}

	public void OnGetMessage(string _Message){
		GetData = null;
		GetData = _Message.Substring(_Message.Length-1, 1);
		Debug.Log ("Pitch Click: " + GetData);

	}

	public void OnFoundNoDevice() {
	}

	public void OnScanFinish() {
	}

	public void OnFoundDevice() {

	}

}
