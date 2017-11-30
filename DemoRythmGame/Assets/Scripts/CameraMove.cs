using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public Transform lookAt;
	public GameObject Piano;

	private bool smooth = true;
    public float duration = 1.0F;
    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

	private void LateUpdate()
	{
		Vector3 desiredPosition = new Vector3 (Piano.GetComponent<Transform>().transform.localPosition.x - 1.4f, Piano.GetComponent<Transform>().transform.localPosition.y + 5, Piano.GetComponent<Transform> ().transform.localPosition.z - 8);
        float t = (Time.time - startTime) / duration;

		if (smooth) {
            transform.position = Vector3.Lerp (transform.position, desiredPosition, Mathf.SmoothStep(0.0f, 1.0f, t));
		} 
		else{
			transform.position = desiredPosition;
		}
	}

 
}
