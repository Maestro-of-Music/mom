using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class BluetoothIOManager : MonoBehaviour,IBtObserver {

	private Bluetooth bluetooth;

	[SerializeField]
	private BluetoothModel bluetoothModel;

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

		//this.GetData = null;
		//Debug.Log ("GET MESSAGE :" + _Message);
		//this.GetData = _Message.Substring(_Message.Length-1, 1);
	}

	public void OnFoundNoDevice() {
	}

	public void OnScanFinish() {
	}

	public void OnFoundDevice() {

	}

}
