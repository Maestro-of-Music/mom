using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class RadialMenuController : MonoBehaviour {

    List<Button> childButtons = new List<Button>();
    bool open = false;
    public int buttonDistance = 150;
    Vector2[] buttonGoalPos;
    private float speed = 50.0f;
    public bool roundCheck = false;

    public bool OpacityOn = false;

    public GameObject Background;
    public GameObject Title;
    public GameObject Description;

    public Sprite Practice_title;
    public Sprite Play_title;
    public Sprite MyMusic_title;

    private float opacity;

    public Vector3 top;
    public Vector3 middle;
    public Vector3 bottom;

    private SceneChange scenechange;


    private void Awake()
    {
        this.scenechange = SceneChange.getInstance();

        if(Application.platform == RuntimePlatform.Android){
            buttonDistance = 750;
        }else{
            buttonDistance = 150;
        }
    }

    // Use this for initialization
    void Start () {

        childButtons = this.GetComponentsInChildren<Button>(true).Where(x => x.gameObject.transform.parent !=
                                                                        transform.parent).ToList();
        buttonGoalPos = new Vector2[childButtons.Count];

        OpenMenu();

        gameObject.transform.eulerAngles = new Vector3(
        gameObject.transform.eulerAngles.x,
        gameObject.transform.eulerAngles.y,
        gameObject.transform.eulerAngles.z);
	}


    void Update(){
       
    }

    public void initPosition(){
      
        top = childButtons[2].GetComponent<Transform>().position;
        middle = childButtons[3].GetComponent<Transform>().position;
        bottom = childButtons[4].GetComponent<Transform>().position;

    }

    void SwarpButtons(int num){
        for (int i = 0; i < childButtons.Count;i++){
            if(childButtons[i].GetComponent<MenuControl>().index == num){
                Debug.Log(num - 1);

                Debug.Log(num );
                Debug.Log(num + 1);
            }
        }
    }


    public void SettingDescription(string title)
    {
        if(title == "Practice"){
            Title.GetComponent<Image>().sprite = Practice_title;
            Description.GetComponent<Text>().text = "새로운 음악을 연습해 보세요!";

            this.scenechange.mode = 2;
            //  this.bluetooth.Send("@");
            SceneManager.LoadScene("8practice");

        }else if (title == "Play"){
            Title.GetComponent<Image>().sprite = Play_title;
            Description.GetComponent<Text>().text = "원하는 음악을 연주해 보세요!";

            this.scenechange.mode = 1;
            //  this.bluetooth.Send("!");
            SceneManager.LoadScene("5play");

        }else if(title == "MyMusic"){
            Title.GetComponent<Image>().sprite = MyMusic_title;
            Description.GetComponent<Text>().text = "그 동안의 연주 기록을 확인하세요!";

            SceneManager.LoadScene("10MyMusic");

        }
        Description.SetActive(true);
    
    }


    IEnumerator WaitTimeing(){
        yield return new WaitForSeconds(5.0f);

    }


    public void OpenMenu(){

        open = !open;
        //90/ (childButtons.Count - 1) * Mathf.Deg2Rad;
        float angle =  360 / (childButtons.Count - 1) * Mathf.Deg2Rad;
        for (int i = 0; i < childButtons.Count; i++){
            if(open){
                float xpos = Mathf.Cos(angle * i) * buttonDistance;
                float ypos = Mathf.Sin(angle * i) * buttonDistance;

                childButtons[i].GetComponent<MenuControl>().index = i;
                childButtons[i].transform.position = new Vector2(this.transform.position.x + xpos,
                                                                 this.transform.position.y + ypos);
                childButtons[i].transform.SetParent(transform, false);
                Debug.Log("Child buttons");
            }else{
                childButtons[i].transform.position  = this.transform.position;
            }
        }

        initPosition();
    }


    private void RotateButtons(){
        //        yield return new WaitForSeconds(0.01f);
        if (speed * Time.deltaTime < 0)
        {
            roundCheck = false;
        }
        else
        {
            gameObject.GetComponent<Transform>().Rotate(0, 0, speed * Time.deltaTime);
        }
    }


    private IEnumerator MoveButtons(){
        foreach(Button b in  childButtons){
            b.gameObject.SetActive(true);
        }
        int loops = 0;
        while(loops <= buttonDistance/speed){
            yield return new WaitForSeconds(0.01f);

            for (int i = 0; i < childButtons.Count;i++){
                
                childButtons[i].gameObject.transform.position = Vector2.Lerp(childButtons[i].gameObject.transform.position,
                                                                             buttonGoalPos[i], speed * Time.deltaTime);
                childButtons[i].gameObject.transform.Rotate(0, 0, 90);
            }
            loops++;
        }

    }
}
