using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public float speed = 5f;
	public bool grounded = true;
	public float distanceLimit;
	private float distanceFromLimit;
	public bool isFollowing = true;

	private float defaultScaleX;
	private float tempScale;
	private bool allowFlip;
	private float reachBeforeFlip;

	public Animator anim;
	private NPCInteraction npc;

	void Start () {
		target = FindObjectOfType<PlayerController>().transform;
		anim = transform.GetComponentInChildren<Animator>();
		npc = GetComponent<NPCInteraction>();
		defaultScaleX = Mathf.Abs(target.localScale.x);
		tempScale = defaultScaleX;
	}
		
	void FixedUpdate () {
		float tempDistanceLimit = 0f;
		distanceFromLimit = 0f;
		float currentDistance = 0f;

		distanceFromLimit = Mathf.Abs(target.position.x - transform.position.x) - distanceLimit;
		currentDistance = Mathf.Abs(target.position.x - transform.position.x);

		if(!isFollowing){
			return;
		}

		if(currentDistance >= distanceLimit && tempScale != transform.localScale.x){
			transform.localScale = new Vector3(target.localScale.x, transform.localScale.y, transform.localScale.z);
		}

		if (distanceFromLimit < 0.1f){
			//allowFlip = true;
		}
		else if(distanceFromLimit >= 0.1f){
			//allowFlip = false;
			if(target.localScale.x > 0){
				tempDistanceLimit = -distanceLimit;
				tempScale = defaultScaleX;
				//npc.facingRight = true;
			}
			else if(target.localScale.x < 0){
				tempDistanceLimit = distanceLimit;
				tempScale = -defaultScaleX;
				//npc.facingRight = false;
			}
			Follow(tempDistanceLimit);
		}

		anim.SetFloat("Speed", distanceFromLimit);
		anim.SetBool("Ground", grounded);



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

	public void setIsFollowing(bool boolean){
		isFollowing = boolean;
	}
}
