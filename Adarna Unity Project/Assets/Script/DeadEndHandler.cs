using UnityEngine;
using System.Collections;

public class DeadEndHandler : MonoBehaviour {
	private Vector2 savedVelocity;

	void OnCollisionEnter2D(Collision2D coll){
		
		Animator playerAnim = new Animator();
		if(coll.gameObject.tag == "Player"){
			StartCoroutine(waitForReverse(coll.transform));
		}

		Debug.Log("collided");
	}

	IEnumerator waitForReverse(Transform player){

		bool inDeadEnd = true;
		PlayerController playerController = player.GetComponent<PlayerController>();

		if(playerController != null)
			playerController.canMove = false;

		while(inDeadEnd){
			if(player.localScale.x > 0){
				if(Input.GetKeyDown(KeyCode.A)){
					if(playerController != null  && !DialogueController.inDialogue)
						playerController.canMove = true;
					inDeadEnd = false;
				}
			}
			else if(player.localScale.x < 0){
				if(Input.GetKeyDown(KeyCode.D)){
					if(playerController != null && !DialogueController.inDialogue)
						playerController.canMove = true;
					inDeadEnd = false;
				}
			}
			yield return null;
		}
	}
}
