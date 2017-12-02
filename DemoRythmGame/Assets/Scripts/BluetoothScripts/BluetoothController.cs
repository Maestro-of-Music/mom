using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class BluetoothController : MonoBehaviour, IBtObserver {

    private string serverUrl = "http://52.78.228.8/restapi/upload_file.php";

    private Bluetooth bluetooth;
    private SceneChange scenechange;
    public RawImage m_image;
    public string sceneName;

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
        this.scenechange = SceneChange.getInstance();
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

    public void nextPage(){
        Debug.Log(sceneName);
        SceneManager.LoadScene(sceneName);
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
        //StartCoroutine(ShowImage(path));
        StartCoroutine(UploadFile(path));
    }
    IEnumerator UploadFile(string path)
    {
        Debug.Log("Path : " + path);

        WWW localFile = new WWW("file://" + path); //"file://"
        yield return localFile;
        /*
        for (int i = 0; i < localFile.bytes.Length; i++)
        {
            Debug.Log("LocalFile bytes : " + localFile.bytes[i]);
        }
        */
        /*
        if (localFile.error == null)
            Debug.Log("Loaded file successfully");
        else
        {
            Debug.Log("Open file error: " + localFile.error);
            yield break;
        }
        */

        WWWForm postForm = new WWWForm();
        postForm.AddBinaryData("fileData", localFile.bytes, "musicsheet.jpg", "Image/jpg" );

        WWW upload = new WWW(serverUrl, postForm);
        yield return upload;
        if (upload.error == null)
            Debug.Log("upload done");
        else
            Debug.Log("upload error" + upload.error);

        if(upload.isDone)
        {
            Debug.Log("complete!");
            WriteXML(upload);
        }
    }

    void WriteXML(WWW XmlFile)
    {
        Debug.Log("in WriteXML");
        string strFile = "test_audiveris"; // test_audiveris(1).xml
        string strFilePath = Application.persistentDataPath + "/" + strFile + ".xml";
      //  string strFilePath = "Assets/" + strFile;

        while(File.Exists(strFilePath))
        {
            Regex rg = new Regex(@".*\((?<Num>\d*)\)");
            Match mt = rg.Match(strFilePath);

            if (mt.Success)
            {
                string numberOfCopy = mt.Groups["Num"].Value;
                int nextNumberOfCopy = int.Parse(numberOfCopy) + 1;
                int posStart = strFilePath.LastIndexOf("(" + numberOfCopy + ")");
                strFilePath = string.Format("{0}({1}){2}", strFilePath.Substring(0, posStart), nextNumberOfCopy, ".xml");
            }
            else
            {
                strFilePath = Application.persistentDataPath + "/" + strFile + "(2)" + ".xml";
            }
        }
        Debug.Log("File writing");
        File.WriteAllBytes(strFilePath, XmlFile.bytes);

        XmlDocument Xmldoc = new XmlDocument();
        Xmldoc.Load(strFilePath);
        Debug.Log("filename: " + strFilePath);
        Debug.Log("xmldoc : " + Xmldoc.InnerText);
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
