using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	//public Vector3 Current;
	public Vector3 Target;

	//duration is in seconds

	public void moveToPositionDefault(float duration){ //uses value from the public variable Target (for animation event)
		Debug.Log("Move to position: " + this.transform.localPosition);
		StartCoroutine(startMoving(this.transform.localPosition, Target, duration));
	}
		
	public void moveToPosition(Vector3 target, float duration){//can be called in other functions with custom parameters
		StartCoroutine(startMoving(this.transform.localPosition, target, duration));
	}

	IEnumerator startMoving(Vector3 current, Vector3 target, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;
		//Debug.Log("Move position starting.");
		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			this.transform.localPosition = Vector3.Lerp(current, target, t);
			//Debug.Log("Current position: " + this.transform.localPosition);
			yield return null;

		}

	}
}
