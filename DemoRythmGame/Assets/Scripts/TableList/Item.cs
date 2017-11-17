using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour {

    public string Title;
    public string Detail;
    public Sprite Icon;
    public bool Confirm;

    public Button.ButtonClickedEvent OnItemClick;

}
