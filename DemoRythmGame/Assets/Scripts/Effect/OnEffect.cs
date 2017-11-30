using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEffect : MonoBehaviour {

    public GameObject Effect;

    public void OnCollision(){
        Effect.SetActive(true);
    }

    IEnumerator stopEffect(){
        yield return new WaitForSeconds(5f);
        Effect.SetActive(false);
    }

    IEnumerator WaitForNoteDetail(){
        yield return new WaitForSeconds(2f);
    }
}
