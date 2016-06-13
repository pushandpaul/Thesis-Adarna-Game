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

	private Animator anim;
	public AnimationClip idleState;

	void Awake () {
		anim = GetComponentInChildren<Animator>();
		anim.Play(idleState.name);
	}

	void FixedUpdate () {
		if(!canMoveX && !canMoveY){
			return;
		}

		if(canMoveX && (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < - 0.5f)){
			transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
		}

		if(canMoveY && (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < - 0.5f)){
			transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
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

		anim.SetFloat("MoveX", moveXValue);
		anim.SetFloat("MoveY", moveYValue);
	}
}
