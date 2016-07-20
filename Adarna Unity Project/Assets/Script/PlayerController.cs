using UnityEngine;
using System.Collections;
using LitJson;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	private float moveVelocity;
	public float jumpHeight;

	public bool canMove;
	public bool canJump = true;
	public bool facingRight;
	public bool allowFlip = true;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;

	public Animator anim;
	public ItemToGive item;

	private float scaleX;
	private float scaleY;
	private float scaleZ;

	private bool waitForPress;

	public CameraController camera;
	private float defaultCamOffset;

	private GameManager gameManager;
	private bool movementInControl = false;
	// Use this for initialization
	void Awake() {
		camera = FindObjectOfType<CameraController>();
		gameManager = FindObjectOfType<GameManager> ();
		anim = GetComponentInChildren<Animator>();
		//item = GetComponentInChildren<ItemToGive>(true);

		scaleX = Mathf.Abs(transform.localScale.x);
		scaleY = transform.localScale.y;
		scaleZ = transform.localScale.z;

		if(gameManager != null)
			moveSpeed = gameManager.playerSpeed;
		moveSpeed += (Mathf.Abs(transform.localScale.x) % 0.5f) * 10;
		Debug.Log("This is player speed: " + moveSpeed);
		canMove = false;
		canJump = false;
	

		initState();
	}

	void Start(){
		StartCoroutine(waitTilScreenFaded());
	}

	void FixedUpdate (){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
	}
	
	// Update is called once per frame
	void Update () {
		moveVelocity = 0f;

		if(transform.localScale.x < 0)
			facingRight = false;
		else if(transform.localScale.x > 0)
			facingRight = true;

		if(!canMove){
			anim.SetFloat("Speed", 0);
			anim.SetBool("Ground", true);
			GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
			return;
		}
			
		if(Input.GetKeyDown(KeyCode.Space) && grounded && canJump){
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
		}
			
		if(Input.GetKey(KeyCode.D)){
			moveVelocity = moveSpeed;
		}

		else if(Input.GetKey(KeyCode.A))
			moveVelocity = -moveSpeed;{
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2(moveVelocity, GetComponent<Rigidbody2D>().velocity.y);

		if(anim != null){
			anim.SetBool("Ground", grounded);
			anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
		}
		else
			anim = GetComponentInChildren<Animator>();

		//Flip
		if(allowFlip){
			if(GetComponent<Rigidbody2D>().velocity.x > 0){
				transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
				//facingRight = true;
				//flipPlayer();
			}

			else if(GetComponent<Rigidbody2D>().velocity.x < 0){
				transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
				//facingRight = false;
				//flipPlayer();

			}
		}

	}
	public void enablePlayerMovement(){
		canMove = true;
	}
	public void disablePlayerMovement(){
		//GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
		canMove = false;
	}

	public void setCanJump(bool canJump){
		this.canJump = canJump;
	}
	public void flipPlayer(){
		Debug.Log("Initial Scale" + transform.localScale.x);
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y, 1f);
		Debug.Log("Changed Scale" + transform.localScale.x);
		facingRight = !facingRight;
	}

	public void setPlayerSate(string state){
		gameManager.playerIdleState = Animator.StringToHash(state);
		//Debug.Log(gameManager.playerIdleState);
		anim.Play (state);
	}

	public void setPlayerState(int stateHash){
		gameManager.playerIdleState = stateHash;
		anim.Play (stateHash);
	}

	public void initState(){
		if(gameManager != null){
			Debug.Log ("Attempt to play animation: " + gameManager.playerIdleState + " player state.");

			anim.Play (gameManager.playerIdleState);
			if(gameManager.currentHeldItem != null && item != null)
				item.setItem(gameManager.currentHeldItem);
			/*if (gameManager.currentHeldItem == null) {
				setPlayerSate ("Idle");
			}*/
		}
	}

	public void setSpeed(float speed){
		moveSpeed = speed;
		moveSpeed += (Mathf.Abs(transform.localScale.x) % 0.5f) * 10;
		gameManager.playerSpeed = speed;
	}

	public void revertSpeed(){
		moveSpeed = 5f;
		moveSpeed += (Mathf.Abs(transform.localScale.x) % 0.5f) * 10;
		gameManager.playerSpeed = 5f;
	}

	IEnumerator waitTilScreenFaded(){
		LoadingScreenManager loadingScreen = FindObjectOfType<LoadingScreenManager>();

		if(loadingScreen != null){
			while(!loadingScreen.officiallyLoaded){
				yield return null;
			}
		}
			
		if(!DialogueController.inDialogue && !TutorialManager.inTutorial){
			canMove = true;
			canJump = true;
		}
	}
}

