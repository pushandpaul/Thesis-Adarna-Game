using UnityEngine;
using System.Collections;

public class FollowPlayerRigid : MonoBehaviour {

	public Transform target;
	public int maxRange;
	public int minRange;
	public bool isFollowing;
	public float moveSpeed;

	private bool playerFound = true;
	private Animator anim;
	private float moveVelocity = 0f;
	private float defaultScale;
	// Use this for initialization
	void Start () {
		anim = this.GetComponentInChildren<Animator>();
		this.defaultScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isFollowing)
			return;

		if(Vector3.Distance(transform.position, target.position) >= minRange){
			if(playerFound){
				if(target.localScale.x > 0){
					moveVelocity = moveSpeed;
					transform.localScale = new Vector3(defaultScale, transform.localScale.y, transform.localScale.z);
				}
					
				else if(target.localScale.x < 0){
					moveVelocity = -moveSpeed;
					transform.localScale = new Vector3(-defaultScale, transform.localScale.y, transform.localScale.z);
				}
				this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, this.GetComponent<Rigidbody2D>().velocity.y);
			}
			//this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(target.position.x, this.GetComponent<Rigidbody2D>().velocity.y));
		}

		else{
			moveVelocity = 0f;
		}

		anim.SetFloat("Speed" , Mathf.Abs(moveVelocity));
	}
}
