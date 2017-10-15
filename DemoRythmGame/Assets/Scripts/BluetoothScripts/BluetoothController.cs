using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class BluetoothController : MonoBehaviour, IBtObserver {
    
    private Bluetooth bluetooth;

	public int number;

    [SerializeField]
    private BluetoothModel bluetoothModel;

    [SerializeField]
    private Dropdown deviceDropdown;

    [SerializeField]
    private Button searchButton;

    [SerializeField]
    private Button connectButton;


	[SerializeField]
	private Button sendButton;

    [SerializeField]
    public Text bluetoothMessage;


    private void Awake() {
        this.bluetooth = Bluetooth.getInstance();
    }

    private void Start() {
        this.bluetoothModel.AddObserver(this);
        this.deviceDropdown.ClearOptions();

        this.searchButton.onClick.AddListener(
            () => {
				this.deviceDropdown.ClearOptions();
                this.bluetooth.SearchDevice();
            });

        this.connectButton.onClick.AddListener(
             () => {
				Debug.Log((this.deviceDropdown.value).ToString());
                 this.bluetooth.Connect(this.deviceDropdown.options[this.deviceDropdown.value].text);
             });
		
		this.sendButton.onClick.AddListener(
			() => {
				Debug.Log("Clicked ! " + number.ToString());
				this.bluetooth.Send(number.ToString());
			});
		
	}

    public void OnStateChanged(string _State) {
    }

    public void OnSendMessage(string _Message) {
	}

    public void OnGetMessage(string _Message) {
		
        this.bluetoothMessage.text = _Message;
        Debug.Log(_Message);

	}

    public void OnFoundNoDevice() {
    }

    public void OnScanFinish() {
    }

    public void OnFoundDevice() {
        // Clear and Get new List
        
		deviceDropdown.ClearOptions();
        deviceDropdown.AddOptions(this.bluetoothModel.macAddresses);

		Debug.Log (this.bluetoothModel.macAddresses); //find pianote device 
		//and go next page press A or B click piano keyboard
		//Catch MacAddress 
    }


}
