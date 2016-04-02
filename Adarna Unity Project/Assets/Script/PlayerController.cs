using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	private float moveVelocity;
	public float jumpHeight;

	public bool canMove;
	public bool facingRight;

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
		camera = FindObjectOfType<CameraController>();

		scaleX = Mathf.Abs(transform.localScale.x);
		scaleY = transform.localScale.y;
		shadowX = shadow.transform.position.x;
		shadowY = shadow.transform.position.y;

		moveSpeed += (Mathf.Abs(transform.localScale.x) % 0.5f) * 10;
		Debug.Log("This is player speed: " + moveSpeed);

		if(transform.localScale.x < 0)
			facingRight = false;
		else if(transform.localScale.x > 0)
			facingRight = true;
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
			facingRight = true;
			//flipPlayer();
		}
			
		else if(GetComponent<Rigidbody2D>().velocity.x < 0){
			transform.localScale = new Vector3(-scaleX, scaleY, 0f);
			camera.flipped = true;
			facingRight = false;
			//flipPlayer();

		}

	}

	public void enablePlayerMovement(){
		canMove = true;
	}
	public void disablePlayerMovement(){
		//GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		canMove = false;
	}
	public void flipPlayer(){
		Debug.Log("Initial Scale" + transform.localScale.x);
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y, 0f);
		Debug.Log("Changed Scale" + transform.localScale.x);
		camera.flipped = !camera.flipped;
		facingRight = !facingRight;
	}
}
