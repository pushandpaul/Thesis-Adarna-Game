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

		player.GetComponent<PlayerController>().canMove = false;

		while(inDeadEnd){
			if(player.localScale.x > 0){
				if(Input.GetKeyDown(KeyCode.A)){
					player.GetComponent<PlayerController>().canMove = true;
					inDeadEnd = false;
				}
			}
			else if(player.localScale.x < 0){
				if(Input.GetKeyDown(KeyCode.D)){
					player.GetComponent<PlayerController>().canMove = true;
					inDeadEnd = false;
				}
			}
			yield return null;
		}
	}
}
