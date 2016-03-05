using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	private float moveVelocity;
	public float jumpHeight;

	public bool canMove;
	public bool flipped;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	private Animator anim;

	private float scaleX;
	private float scaleY;

	private bool waitForPress;

	public CameraController camera;
	private float defaultCamOffset;

	public GameObject shadow;
	private float shadowX;
	private float shadowY;
	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		scaleX = Mathf.Abs(transform.localScale.x);
		scaleY = transform.localScale.y;
		shadowX = shadow.transform.position.x;
		shadowY = shadow.transform.position.y;
	}

	void FixedUpdate (){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

	}
	
	// Update is called once per frame
	void Update () {
		if(!canMove){
			anim.SetFloat("Speed", 0);
			anim.SetBool("Ground", true);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			return;
		}

		if(Input.GetKeyDown(KeyCode.Space) && grounded){
			Debug.Log("This is speed: " + anim.GetFloat("Speed"));
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
		}


		shadow.transform.position = new Vector3(shadow.transform.position.x, shadowY, 0f);
		/*if(moveVelocity > 0)
			Debug.Log("pangit mo paul");
		else if(moveVelocity == 0)
			Debug.Log("ganda ni clark");
		*/
		anim.SetBool("Ground", grounded);
			
		moveVelocity = 0f;
		if(Input.GetKey(KeyCode.D)){
			moveVelocity = moveSpeed;
		}
			

		if(Input.GetKey(KeyCode.A))
			moveVelocity = -moveSpeed;{
		}

		anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
		GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);

		//Flip

		if(GetComponent<Rigidbody2D>().velocity.x > 0){
			transform.localScale = new Vector3(scaleX, scaleY, 0f);
			camera.flipped = false;
		}
			
		else if(GetComponent<Rigidbody2D>().velocity.x < 0){
			transform.localScale = new Vector3(-scaleX, scaleY, 0f);
			camera.flipped = true;
		}
	}

	public void enablePlayerMovement(){
		canMove = true;
	}
	public void disablePlayerMovement(){
		canMove = false;
	}
}
