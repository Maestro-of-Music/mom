using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEffect : MonoBehaviour {

    public GameObject Effect;

    public void OnCollision(){
        /*
        if (gameObject.GetComponent<NoteDetail>().duration >= 4)
        {
            Effect.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            Effect.GetComponent<Transform>().localPosition = new Vector3(0, 2, 0);
        }
        */
        Effect.SetActive(true);
    }

    public void OffCollision(){
        Effect.SetActive(false);
    }

    IEnumerator stopEffet(){
        yield return new WaitForSeconds(1f);
        Effect.SetActive(false);
    }

    IEnumerator WaitForNoteDetail(){
        yield return new WaitForSeconds(2f);
    }
}
