using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	private float x = 0;
	private float y = 0;
	private Vector3 initialPosition;
	private Vector3 initialMousePos;

	void Update () {
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;
	}

	void OnMouseDown(){
		initialPosition = transform.position;
		initialMousePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y ,0f));
	}


	void OnMouseDrag(){
		Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y ,0f));
		Vector3 newPosition = new Vector3(initialPosition.x + (currentMousePos.x - initialMousePos.x), initialPosition.y + (currentMousePos.y - initialMousePos.y), initialPosition.z);
		transform.position = newPosition;
	}
}
