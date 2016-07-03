using UnityEngine;
using System.Collections;

public class BalonMinigame : MonoBehaviour {

	public float dragSpeed;
	public float moveSpeed;

	public float movePoint;
	public bool goingDown;
	private bool isGoalReached;

	public bool isDragged = true;
	public bool allowMove = true;

	public Transform point1;
	public Transform point2;

	private float startPosition;
	private float endPosition;
	public float targetPosition;

	public Transform character;
	private Animator characterAnim;
	public Transform cameraTransform;

	private float _y;


	void Awake () {
		Init();
	}

	void FixedUpdate () {
		_y = character.position.y;

		if(Input.GetKeyDown(KeyCode.E)){
			moveToExit();
		}
		if(allowMove){
			if(isDragged){
				_y = Mathf.MoveTowards(_y, targetPosition, Time.deltaTime * dragSpeed);
			}
			else{
				_y = Mathf.MoveTowards(_y, targetPosition, Time.deltaTime * moveSpeed);
				if(_y == targetPosition){
					dragAway();
				}
			}
		}
	
		if(_y <= endPosition && goingDown){
			goalReached();
		}
		else if(_y >= endPosition && !goingDown){
			goalReached();
		}
			
		characterAnim.SetFloat("Speed",Mathf.Abs(_y - targetPosition));
		character.position = new Vector3(character.position.x, _y, character.position.z);
	}

	void moveToExit(){
		targetPosition = character.position.y + movePoint;
		isDragged = false;
	}

	void dragAway(){
		targetPosition = startPosition;
		isDragged = true;
	}

	void goalReached(){
		//character.GetComponent<Animator>().Play("Climbing Idle");
		Debug.Log("Goal Reached!");
		targetPosition = endPosition;
		isGoalReached = true;
		isDragged = false;
		allowMove = false;
	}


	void Init(){
		cameraTransform = FindObjectOfType<CameraController>().transform;
		//goingDown = gameManager.balonGoingDown;
		if(goingDown){
			startPosition = Mathf.Max(point1.position.y, point2.position.y);
			endPosition = Mathf.Min(point1.position.y, point2.position.y);
			movePoint = -Mathf.Abs(movePoint);
		}
		else{
			startPosition = Mathf.Min(point1.position.y, point2.position.y);
			endPosition= Mathf.Max(point1.position.y, point2.position.y);
			movePoint = Mathf.Abs(movePoint);
		}

		character.position = new Vector3(character.position.x, startPosition, character.position.z);
		characterAnim = character.GetComponentInChildren<Animator>();
		characterAnim.Play("Gapang sa Balon Idle");
		cameraTransform.position = new Vector3(cameraTransform.position.x, startPosition, cameraTransform.position.z);
		targetPosition = startPosition;
	}

}
