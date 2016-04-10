using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public int maxRange;
	public int minRange;
	public bool isFollowing;
	public float speed;

	private float moveVelocity;
	private float temp = 0f;
	private Vector3 targetCoordinates;

	private Animator anim;
	private float defaultScaleX;

	void Awake(){
		target = FindObjectOfType<PlayerController>().transform;
		anim = this.GetComponentInChildren<Animator>();
		transform.position = new Vector3(target.position.x + minRange, target.position.y, target.position.z);
		defaultScaleX = Mathf.Abs(target.localScale.x);
	}
	void Update () {
		
		if(!isFollowing || target == null)
			return;
		if(Vector3.Distance(transform.position, target.position) >= minRange){
			targetCoordinates = new Vector3(target.position.x, transform.position.y, transform.position.z);
			transform.Translate((targetCoordinates - transform.position).normalized * speed * Time.deltaTime);
			temp = 1f;
			//this.GetComponent<Rigidbody2D>().velocity = target.GetComponent<Rigidbody2D>().velocity;
			if(target.localScale.x < 0)
				transform.localScale = new Vector3(-(defaultScaleX), transform.localScale.y, transform.localScale.z);
			else if(target.localScale.x > 0)
				transform.localScale =  new Vector3(defaultScaleX, transform.localScale.y, transform.localScale.z);
		}
		else
			temp = 0f;
		anim.SetFloat("Speed" , temp);
		//Debug.Log("Current follow speed" + temp);
			
	}

	public void setParameters(Transform target, int maxRange, int minRange, float speed, Vector3 scale){
		this.target = target;
		this.maxRange = maxRange;
		this.minRange = minRange;
		this.speed = speed;
		this.transform.localScale = scale;
		this.defaultScaleX = Mathf.Abs(scale.x);
	}
}
