using UnityEngine;
using System.Collections;

public class AgilaFlyingController : MonoBehaviour {


	public Vector3 jumpForce = new Vector2 (0, 400);
	private Rigidbody2D myRigidBody2D;
	private Animator anim; 
	private GenericMinigameManger minigameManager;

	private bool canMove = false;
	private bool obstacleTriggered = false;
	public float moveSpeed = 5f;

	private Vector3 originalPosition;

	void Awake(){
		minigameManager = FindObjectOfType<GenericMinigameManger>();
		myRigidBody2D = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();
		anim.Play("(Agila) Idle with Nakasakay");
		anim.SetFloat ("Speed", 1f);

		originalPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}

	void Update () {

		if(!canMove){
			myRigidBody2D.gravityScale = 0f;
			myRigidBody2D.velocity = Vector2.zero;

			if(!minigameManager.checkCanStart()){
				return;
			}

			else if(Input.GetKeyDown(KeyCode.E)){
				canMove = true;
				myRigidBody2D.gravityScale = 1f;
				Jump ();
			}
			return;
		}
		if (!obstacleTriggered){
			myRigidBody2D.velocity = new Vector2 (moveSpeed, myRigidBody2D.velocity.y);
			if(Input.GetKeyDown(KeyCode.E)){
				Jump ();
			}
		}
			
		else
			myRigidBody2D.velocity = new Vector2 (0, myRigidBody2D.velocity.y);
	}

	void Jump(){
		myRigidBody2D.velocity = Vector2.zero;
		myRigidBody2D.AddForce (jumpForce);
	}

	public void SetCanMove(bool canMove){
		this.canMove = canMove;
	}

	public void SetObstacleTriggered(bool obstacleTriggered){
		this.obstacleTriggered = obstacleTriggered;
	}
}
