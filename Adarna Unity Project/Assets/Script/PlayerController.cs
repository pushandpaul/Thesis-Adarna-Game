using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	private float moveVelocity;
	public float jumpHeight;

	public bool canMove;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	private Animator anim;

	private float scaleX;
	private float scaleY;

	public CameraController camera;
	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
	}

	void FixedUpdate (){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
		if(!canMove){
			anim.SetFloat("Speed", 0);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			return;
		}
			
		moveVelocity = 0f;
		if(Input.GetKey(KeyCode.D))
			moveVelocity = moveSpeed;

		if(Input.GetKey(KeyCode.A))
			moveVelocity = -moveSpeed;{
		}

		anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

		GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);

		anim.SetBool("Ground", grounded);

		if(Input.GetKeyDown(KeyCode.Space) && grounded){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
		}
	
		//Flip
		if(GetComponent<Rigidbody2D>().velocity.x > 0){
			transform.localScale = new Vector3(scaleX, scaleY, 1f);
				
		}
			
		else if(GetComponent<Rigidbody2D>().velocity.x < 0){
			transform.localScale = new Vector3(-scaleX, scaleY, 1f);
		}
	}
}
