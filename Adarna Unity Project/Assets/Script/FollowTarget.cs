using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public float speed = 5f;
	public float distanceLimit;
	public bool isFollowing = true;

	private float defaultScaleX;
	private float tempScale;
	private bool allowFlip;
	private float reachBeforeFlip;

	private Animator anim;

	void Awake () {
		target = FindObjectOfType<PlayerController>().transform;
		anim = transform.GetComponentInChildren<Animator>();
		defaultScaleX = Mathf.Abs(target.localScale.x);
		tempScale = defaultScaleX;
	}
		
	void FixedUpdate () {
		float tempDistanceLimit = 0f;
		float distanceFromLimit = 0f;
		float currentDistance = 0f;

		distanceFromLimit = Mathf.Abs(target.position.x - transform.position.x) - distanceLimit;
		currentDistance = Mathf.Abs(target.position.x - transform.position.x);

		if(!isFollowing)
			return;

		if (distanceFromLimit < 0.1f){
			//allowFlip = true;
		}
		else if(distanceFromLimit >= 0.1f){
			//allowFlip = false;
			if(target.localScale.x > 0){
				tempDistanceLimit = -distanceLimit;
				tempScale = defaultScaleX;
			}
			else if(target.localScale.x < 0){
				tempDistanceLimit = distanceLimit;
				tempScale = -defaultScaleX;
			}
			Follow(tempDistanceLimit);
		}

		anim.SetFloat("Speed", distanceFromLimit);

		if(currentDistance >= distanceLimit && tempScale != transform.localScale.x){
			Debug.Log("Position before flip: " + transform.position.ToString());
			transform.localScale = new Vector3(tempScale, transform.localScale.y, transform.localScale.z);
			Debug.Log("Position after flip: " + transform.position.ToString());
		}

	}

	public void Follow(float distance){
		float targetDirectionX = target.position.x - transform.position.x + distance;
		float xPosition = transform.position.x;
		xPosition += targetDirectionX * speed * Time.deltaTime;
		transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
	}
	public void thisConstructor(float speed, float distance, Transform target, Vector3 targetScale){
		transform.localScale = targetScale;
		this.defaultScaleX = Mathf.Abs(targetScale.x);
		this.tempScale = defaultScaleX;
		this.speed = speed;
		this.distanceLimit = distance;
		this.target = target;
	}
}
