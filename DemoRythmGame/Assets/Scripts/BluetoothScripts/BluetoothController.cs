using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class BluetoothController : MonoBehaviour, IBtObserver {

    private string serverUrl = "http://52.78.228.8/restapi/upload_file.php";

    private Bluetooth bluetooth;
    public RawImage m_image;

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

    public void OnGalleryOpen(){
        Debug.Log("Gallery open!");
        this.bluetooth.TakePhotoByCam();
    }

    public void OnReceiveGallery(string UrlPath){
        Debug.Log("Open Photos!");
        Debug.Log(UrlPath);
        //this.bluetooth.OnReceiveGallery(UrlPath);
        string url = Application.persistentDataPath + UrlPath;
       // Debug.Log(url);
        StartCoroutine("LoadImage", UrlPath);
    }

    void SetImage(string url){
        Texture2D tex = null;
        byte[] fileData;

        Debug.Log("URL :" + url);

        if (File.Exists(url))
        {
            fileData = File.ReadAllBytes(url);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        m_image.GetComponent<Renderer>().material.mainTexture = tex;
    }

    IEnumerator LoadImage(string url)
    {
        string path = url;

        if (!File.Exists(path))
        {
            Debug.Log("File Not Exist... So Downloading....");
            WWW www = new WWW(url);
            yield return www;
            Byte[] bytes = www.texture.EncodeToPNG();
            Debug.Log("Writing File.....");
            File.WriteAllBytes(path, bytes);
        }
        else
        {
            Debug.Log("File Exist......");
        }
        // StartCoroutine(ShowImage(path));
        StartCoroutine(UploadFile(path));
    }
    IEnumerator UploadFile(string path)
    {
        WWW localFile = new WWW("file://" + path);
        yield return localFile;
        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break;
        }

        WWWForm postForm = new WWWForm();
        postForm.AddBinaryData("fileData", localFile.bytes, "musicsheet.jpg", "Image/jpg" );

        WWW upload = new WWW(serverUrl, postForm);
        yield return upload;
        if (upload.error == null)
            Debug.Log("upload done");
        else
            Debug.Log("upload error" + upload.error);
    }

    IEnumerator ShowImage(string path){
        Debug.Log("Load Image......");
        yield return new WaitForSeconds(1.0f);
        /*
        for (int i = 0; i < tBytes.Length;i++){
            Debug.Log(tBytes[i] + " ");
        }
        */

        Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        Byte[] tBytes = File.ReadAllBytes(path);
        texture.LoadImage(tBytes);
        m_image.texture = texture;
    }
}
