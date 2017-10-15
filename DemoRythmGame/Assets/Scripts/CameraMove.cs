using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public Transform lookAt;
	public GameObject Piano;

	private bool smooth = true;
	private float smoothSpeed = 0.125f;

	private void LateUpdate()
	{
		Vector3 desiredPosition = new Vector3 (Piano.GetComponent<Transform> ().transform.localPosition.x, Piano.GetComponent<Transform> ().transform.localPosition.y+3, Piano.GetComponent<Transform> ().transform.localPosition.z);

		if (smooth) {
			transform.position = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);
		} 
		else{
			transform.position = desiredPosition;
		}
			
	}
}
