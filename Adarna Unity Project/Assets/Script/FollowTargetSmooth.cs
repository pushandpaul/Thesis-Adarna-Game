using UnityEngine;
using System.Collections;

public class FollowTargetSmooth : MonoBehaviour {

	public float speed;
	public float maxDistance;
	public Transform target;
	public bool isFollowing = true;

	private float relativeSpeed;
	private Animator anim;
	private float defaultScaleX;
	private float walking;

	void Start(){
		relativeSpeed = speed * .1f + 1f;
		anim = this.GetComponentInChildren<Animator>();
		defaultScaleX = Mathf.Abs(target.localScale.x);
		target = FindObjectOfType<PlayerController>().gameObject.transform;
	}

	void FixedUpdate () {

		if(!isFollowing)
			return;

		float x = transform.position.x;

		if(Vector3.Distance(transform.position, target.position) > maxDistance){
			x = Mathf.Lerp(x, target.position.x, Time.deltaTime/relativeSpeed);
			if(target.localScale.x > 0)
				transform.localScale = new Vector3(defaultScaleX, transform.localScale.y, transform.localScale.z);
			else if(target.localScale.x < 0)
				transform.localScale = new Vector3(-defaultScaleX, transform.localScale.y, transform.localScale.z);
			walking = 1f;
		}
		else
			walking = 0f;
		
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
		anim.SetFloat("Speed", walking);
	}

	public void thisConstructor(float speed, float distance, Transform target, float defaultScale){
		this.speed = speed;
		this.maxDistance = distance;
		this.target = target;
		this.defaultScaleX = Mathf.Abs(defaultScale);
	}
}
