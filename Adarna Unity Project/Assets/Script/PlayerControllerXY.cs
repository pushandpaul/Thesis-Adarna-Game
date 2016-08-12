using UnityEngine;
using System.Collections;

public class PlayerControllerXY : MonoBehaviour {

	public bool canMoveX;
	public bool canMoveY;

	public float moveSpeed;
	private float moveXValue;
	private float moveYValue;

	public bool absXAxisFeedBack;
	public bool absYAxisFeedBack;

	private bool playerMoving;
	private Vector2 lastMove;

	private Animator anim;
	public AnimationClip idleState;

	public bool animationDiffY = true;
	public bool animationDiffX = true;

	public bool moveDiagonally = false;

	void Awake () {
		anim = GetComponentInChildren<Animator>();
		if(anim != null && idleState != null)
			anim.Play(idleState.name);
	}

	void FixedUpdate () {
		if(!canMoveX && !canMoveY){
			return;
		}

		playerMoving = false;

		if(canMoveX && (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < - 0.5f)){
			moveX();
		}

		else if(!moveDiagonally && canMoveY && (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < - 0.5f)){
			moveY();
		}

		if(moveDiagonally && canMoveY && (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < - 0.5f)){
			moveY();
		}

		if(absXAxisFeedBack){
			moveXValue = Mathf.Abs(Input.GetAxisRaw("Horizontal"));
		}
		else
			moveXValue = Input.GetAxisRaw("Horizontal");

		if(absYAxisFeedBack){
			moveYValue = Mathf.Abs(Input.GetAxisRaw("Vertical"));
		}
		else
			moveYValue = Input.GetAxisRaw("Vertical");

		if(anim != null){
			anim.SetFloat("MoveX", moveXValue);
			if(animationDiffY){
				anim.SetFloat("MoveY", moveYValue);
			}
			else{
				anim.SetFloat("MoveX", lastMove.x);
			}
			anim.SetBool ("PlayerMoving", playerMoving);
			anim.SetFloat ("LastMoveX", lastMove.x);
			anim.SetFloat ("LastMoveY", lastMove.y);
		}
	}

	void moveY(){
		transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
		playerMoving = true;
		if(animationDiffY){
			lastMove = new Vector2 (0f, Input.GetAxisRaw("Vertical"));
		}
	}

	void moveX(){
		transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
		playerMoving = true;
		lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
	}
}
