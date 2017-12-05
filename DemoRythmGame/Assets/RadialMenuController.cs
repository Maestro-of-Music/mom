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

    public int index;

    private SceneChange scenechange;

    public Button [] btns;
    public Vector3[] position;

    public GameObject Setting_position;

    public bool moveOn = false;
    public bool moveInit = false;
    public float btnspeed = 4.0f;

    public int shift_index = 0;

    public Vector3 [] _ButtonPosition;

    private void Awake()
    {
        this.scenechange = SceneChange.getInstance();
        position = new Vector3[3];

        if(Application.platform == RuntimePlatform.Android){
            buttonDistance = 750;
        }else{
            buttonDistance = 240;
        }
    }

    // Use this for initialization
    void Start () {

        childButtons = this.GetComponentsInChildren<Button>(true).Where(x => x.gameObject.transform.parent !=
                                                                        transform.parent).ToList();
        buttonGoalPos = new Vector2[childButtons.Count];
        
        Background.SetActive(true);

        //OpenMenu();

        gameObject.transform.eulerAngles = new Vector3(
        gameObject.transform.eulerAngles.x,
        gameObject.transform.eulerAngles.y,
        gameObject.transform.eulerAngles.z);

        init();

	}

    void init(){
        for(int i =0 ; i < _ButtonPosition.Length;i++){
            _ButtonPosition[i] = btns[i].GetComponent<Transform>().position;
        }
    }

    void Update(){
       if(moveOn){
          init_position();
            Background.SetActive(false);

       }else if (moveInit){
           //swarping button and description show 
            last_position(shift_index);
            Background.SetActive(false);
       }else if (moveInit == false && moveOn == false){
            Background.SetActive(true);
       }
    }

    void init_position(){
        
        for(int i= 0 ; i< btns.Length;i++){
            btns[i].transform.position = move_Button(btns[i].transform.position , Setting_position.transform.position);
        }
    }

    void last_position(int num){
       
            if(num == 0){
                //play
                 btns[0].transform.position = move_Buttons(btns[0].transform.position , _ButtonPosition[0]);
                 btns[1].transform.position = move_Buttons(btns[1].transform.position , _ButtonPosition[1]);
                 btns[2].transform.position = move_Buttons(btns[2].transform.position , _ButtonPosition[2]);
                

            }else if(num == 1){
                //practice
                 btns[0].transform.position = move_Buttons(btns[0].transform.position , _ButtonPosition[1]);
                 btns[1].transform.position = move_Buttons(btns[1].transform.position , _ButtonPosition[2]);
                 btns[2].transform.position = move_Buttons(btns[2].transform.position , _ButtonPosition[0]);

            }else{
                //mymusic
                  btns[0].transform.position = move_Buttons(btns[0].transform.position , _ButtonPosition[2]);
                 btns[1].transform.position = move_Buttons(btns[1].transform.position , _ButtonPosition[0]);
                 btns[2].transform.position = move_Buttons(btns[2].transform.position , _ButtonPosition[1]);
            }
          
    }
    Vector3 move_Buttons(Vector3 _position, Vector3  _Target){
        
        if((int)_position.y == (int)_Target.y-1){
            Debug.Log("Setting!");
            moveInit = false;
            
            if(shift_index == 0){
                gameObject.GetComponent<RadialMenuController>().SettingDescription("Play");
            }else if (shift_index == 1){
                gameObject.GetComponent<RadialMenuController>().SettingDescription("Practice");
            }else{
                gameObject.GetComponent<RadialMenuController>().SettingDescription("MyMusic");
            }

            Background.SetActive(true);
        }else{
            _position = Vector3.Lerp(_position, _Target, btnspeed * Time.deltaTime);
        }

        return _position;
    }

    Vector3 move_Button(Vector3 _position , Vector3 _Target){
        if((int)_position.x == (int)_Target.x){
            Debug.Log("Setting!");
            moveOn = false;
            moveInit = true;
        }else{
            _position = Vector3.Lerp(_position, _Target, btnspeed * Time.deltaTime);
        }

        return _position;
    }



    public void SettingDescription(string title)
    {
        if(title == "Practice"){
            Title.GetComponent<Image>().sprite = Practice_title;
            Description.GetComponent<Text>().text = "새로운 음악을 연습해 보세요!";

        }else if (title == "Play"){
            Title.GetComponent<Image>().sprite = Play_title;
            Description.GetComponent<Text>().text = "원하는 음악을 연주해 보세요!";

        }else if(title == "MyMusic"){
            Title.GetComponent<Image>().sprite = MyMusic_title;
            Description.GetComponent<Text>().text = "그 동안의 연주 기록을 확인하세요!";

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
