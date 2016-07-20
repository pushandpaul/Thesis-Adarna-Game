using UnityEngine;
using System.Collections;

public class MoveCharacter : MonoBehaviour {

	//default character speed 7f/sec
	private float defaultSpeed = 7f;
	private bool grounded = true;

	public void flipCharacter(Transform character, string direction){

		if(direction == "r"){
			character.localScale = new Vector3(Mathf.Abs(character.localScale.x), character.localScale.y, character.localScale.z);
			//Debug.Log("Flipped to the right.");
		}
		else if(direction == "l"){
			character.localScale = new Vector3(-(Mathf.Abs(character.localScale.x)), character.localScale.y, character.localScale.z);

			//Debug.Log("Flipped to the left.");
		}
	}

	public void moveCharacter(Transform character, Vector3 targetPosition){
		//character = GameObject.Find (character.name).transform;
		float duration = Mathf.Abs(targetPosition.x - character.position.x)/defaultSpeed;
		StartCoroutine(startMoving(character, character.position, targetPosition, false, Vector3.zero, duration));
	}

	public float _moveCharacter(Transform character, Vector3 targetPosition, float speed){
		float duration = Mathf.Abs(targetPosition.x - character.position.x)/speed;
		return 1/duration;
	}

	public void moveCharacter(Transform character, Vector3 targetPosition, float speed){
		//character = GameObject.Find (character.name).transform;
		float duration = Mathf.Abs(targetPosition.x - character.position.x)/speed;
		StartCoroutine(startMoving(character, character.position, targetPosition, false, Vector3.zero, duration));
	}


	public void moveCharacter(Transform character, float pointsToMoveX, float targetPositionY, bool moveTowards, bool allowFlip, bool revertOrigScale, float duration){

		Vector3 targetPosition = Vector3.zero;
		Vector3 originalScale = character.localScale;

		if(moveTowards){
			if(character.localScale.x < 0){
				targetPosition = new Vector3(character.position.x - pointsToMoveX, targetPositionY, character.position.z);
			}
			else if(character.localScale.x > 0){
				targetPosition = new Vector3(character.position.x + pointsToMoveX, targetPositionY, character.position.z);
			}
		}
		else{
			if(character.localScale.x < 0){
				targetPosition = new Vector3(character.position.x + pointsToMoveX, targetPositionY, character.position.z);
				if(allowFlip){
					flipCharacter (character, "r");
				}
			}
			else if(character.localScale.x > 0){
				Debug.Log ("Move to left");
				targetPosition = new Vector3(character.position.x - pointsToMoveX, targetPositionY, character.position.z);
				if(allowFlip){
					flipCharacter (character, "l");
				}
			}
		}

		StartCoroutine(startMoving(character, character.position, targetPosition, revertOrigScale, originalScale, duration));

	}

	IEnumerator startMoving(Transform character, Vector3 current, Vector3 target, bool revertOrigScale, Vector3 originalScale, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;
		bool animatorFound = false;
		Animator anim = character.GetComponentInChildren<Animator>();
		PlayerController player = character.GetComponent<PlayerController>();
		Vector3 distance = new Vector3(); 

		if (anim != null){
			animatorFound = true;
			Debug.Log(anim.name);
		}
		else
			animatorFound = false;
		if(player != null){
			player.allowFlip = false;
			player.canJump = false;
		}

		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			character.position = Vector3.Lerp(current, target, t);
			if (animatorFound) {
				anim.SetFloat ("Speed", Mathf.Abs (target.x - character.position.x));
				anim.SetBool("Ground", grounded);
			}
			//yield return new WaitForFixedUpdate();
			yield return null;
		}

		if(revertOrigScale && character.localScale != originalScale){
			character.localScale = originalScale;
		}

		if(player != null){
			player.allowFlip = true;
			if(!DialogueController.inDialogue){
				player.canJump = true;
			}
		}
			
		if(animatorFound)
			anim.SetFloat ("Speed", 0);
	}
}
