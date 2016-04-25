using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour {

	//default character speed 7f/sec
	float defaultSpeed = 7f;

	public void flipCharacter(Transform character, string direction){
		NPCInteraction npc = GetComponent<NPCInteraction>();
		bool npcFacingRight = false;

		if(direction == "r"){
			character.localScale = new Vector3(Mathf.Abs(character.localScale.x), character.localScale.y, character.localScale.z);
			npcFacingRight = true;
			Debug.Log("Fliped to the right.");
		}
		else if(direction == "l"){
			character.localScale = new Vector3(-(Mathf.Abs(character.localScale.x)), character.localScale.y, character.localScale.z);
			npcFacingRight = false;
			Debug.Log("Fliped to the left.");
		}

		if(npc != null){
			npc.facingRight = npcFacingRight;
		}
	}

	public void moveCharacter(Transform character, Vector3 targetPosition){
		//character = GameObject.Find (character.name).transform;
		float duration = Mathf.Abs(targetPosition.x - character.position.x)/defaultSpeed;
		StartCoroutine(startMoving(character, character.position, targetPosition, duration));
	}

	IEnumerator startMoving(Transform character, Vector3 current, Vector3 target, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;
		bool animatorFound = false;
		Animator anim = character.GetComponentInChildren<Animator>();
		Vector3 distance; 

		if (anim != null)
			animatorFound = true;
		else
			animatorFound = false;
		
		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			character.position = Vector3.Lerp(current, target, t);
			if (animatorFound) {
				anim.SetFloat ("Speed", (int)Mathf.Abs (target.x - character.position.x));
			}
			yield return null;
		}
	}
}
