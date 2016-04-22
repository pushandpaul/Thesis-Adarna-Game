using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour {

	//default character speed 7f/sec
	float defaultSpeed = 7f;

	public void moveCharacter(Transform character, Vector3 targetPosition){
		float duration = Mathf.Abs(targetPosition.x - character.position.x)/defaultSpeed;
		StartCoroutine(startMoving(character, character.position, targetPosition, duration));
	}

	IEnumerator startMoving(Transform character, Vector3 current, Vector3 target, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;
		bool animatorFound = false;
		Animator anim = character.GetComponentInChildren<Animator>();

		if(anim != null)
			animatorFound = true;
		else
			animatorFound = false;
		
		while(Time.time <= endTime){
			if(animatorFound)
				anim.SetFloat("Speed", Mathf.Abs(target.x - character.localPosition.x));
			float t = (Time.time - startTime)/duration;
			character.position = Vector3.Lerp(current, target, t);
			yield return null;
		}
	}
}
