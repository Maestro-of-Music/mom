using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemObject : MonoBehaviour {

    public Button Btn;
    public Image Icon;
    public Text Title;
    public Text Detail;

    public void ItemClick_Result(Button button)
    {
        Debug.Log(button.name);
    }
}
