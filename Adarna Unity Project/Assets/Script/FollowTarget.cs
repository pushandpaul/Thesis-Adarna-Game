using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public float speed;
	public float maxDistance;
	public Transform target;
	public bool isFollowing = true;

	private float relativeSpeed;
	private Animator anim;
	private float defaultScaleX;
	private float walking = 0f;

	void Awake(){

		relativeSpeed = speed * maxDistance * 0.1f;
		anim = this.GetComponentInChildren<Animator>();
		target = FindObjectOfType<PlayerController>().transform;
		defaultScaleX = Mathf.Abs(target.localScale.x);

	}

	void FixedUpdate () {
		Vector3 targetCoordinates = new Vector3(target.position.x, transform.position.y, transform.position.z);
		float temp = 0f;


		if(!isFollowing)
			return;

		float x = transform.position.x;
		float currentDistance = Vector3.Distance(transform.position, target.position);
		//Debug.Log("Current Distance to the player: " + currentDistance);

		if((int) currentDistance > (int) maxDistance){
			//x = Mathf.Lerp(x, target.position.x, Time.deltaTime*relativeSpeed);

			//transform.Translate((targetCoordinates - transform.position).normalized * speed * Time.deltaTime);
			if(target.localScale.x > 0){
				x = Mathf.Lerp(x, target.position.x - maxDistance, Time.deltaTime*speed);
				transform.localScale = new Vector3(defaultScaleX, transform.localScale.y, transform.localScale.z);
			}
				
			else if(target.localScale.x < 0){
				x = Mathf.Lerp(x, target.position.x + maxDistance, Time.deltaTime*speed);
				transform.localScale = new Vector3(-defaultScaleX, transform.localScale.y, transform.localScale.z);
			}
				
			walking = 1f;
		}
		else{
			//transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			walking = 0f;
		} 
			
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
		anim.SetFloat("Speed", walking);
	}


	void LateUpdate(){
		//anim.SetFloat("Speed", walking);
	}
	public void thisConstructor(float speed, float distance, Transform target, Vector3 defaultScale){
		this.speed = speed;
		this.maxDistance = distance;
		this.target = target;
		transform.localScale = defaultScale;
		this.defaultScaleX = Mathf.Abs(defaultScale.x);
	}
}
