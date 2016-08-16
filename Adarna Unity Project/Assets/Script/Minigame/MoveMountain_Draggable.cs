using UnityEngine;
using System.Collections;

public class MoveMountain_Draggable : MonoBehaviour {

	public bool allowDrag;
	public bool achievedTargetArea;
	public bool triggeredTargetArea;
	public bool enableGravityOnDrag;
	public GameObject hoverState;
	public GameObject dragFX;

	public bool lockX;
	public bool lockY;

	private float x = 0;
	private float y = 0;
	private Vector3 initialPosition;
	private Vector3 initialMousePos;

	private int backupLayerNo;

	public bool scaleWhenCloser;
	private float fxInitialScale;
	private Vector3 awakePosition;
	private float scale;
	public float maxScale;
	public float minScale;

	public bool isMoving;

	private MoveMountainManager moveMountainManager;

	void Awake(){
		moveMountainManager = FindObjectOfType<MoveMountainManager>();
		backupLayerNo = gameObject.layer;
		awakePosition = transform.position;
		scale = transform.localScale.x;
		fxInitialScale = dragFX.transform.localScale.x;
	}

	void Update () {
		if(allowDrag){
			if(!lockX){
				x = Input.mousePosition.x;
			}
			else{
				x = awakePosition.x;
			}

			if(!lockY){
				y = Input.mousePosition.y;
			}

			else{
				y = awakePosition.y;
			}
		}

		if(scaleWhenCloser){
			float newScale = scale - (transform.position.y - awakePosition.y);
			if(newScale < minScale){
				newScale = minScale;
			}
			else if(newScale > maxScale){
				newScale = maxScale;
			}
			float fxScale = (newScale * fxInitialScale)/scale;
			transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
			dragFX.transform.localScale = new Vector3(fxScale, fxScale, fxScale);
		}

	}

	void OnMouseDown(){

		if(!allowDrag)
			return;

		Debug.Log("Clicked");
		initialPosition = transform.position;
		initialMousePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y ,0f));
		hoverState.SetActive(true);
	}

	void OnMouseUp(){

		if(!allowDrag)
			return;

		hoverState.SetActive(false);
		setupIsMoving(false);

		if(!triggeredTargetArea){
			moveMountainManager.wrong("Wrong drag: " + this.name);
			StartCoroutine(returnToInitialPos());
		}
		else{
			achievedTarget();
		}
	}
		
	void OnMouseEnter(){

		if(!allowDrag)
			return;

		hoverState.SetActive(true);
	}

	void OnMouseExit(){

		if(!allowDrag)
			return;

		hoverState.SetActive(false);
	}

	void OnMouseDrag(){

		if(!allowDrag)
			return;

		setupIsMoving(true);
		Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y ,0f));
		Vector3 newPosition = new Vector3(initialPosition.x + (currentMousePos.x - initialMousePos.x), initialPosition.y + (currentMousePos.y - initialMousePos.y), initialPosition.z);
		hoverState.SetActive(true);
		transform.position = newPosition;
	}

	void achievedTarget(){
		Debug.Log("In the right place.");
		//this.enabled = false;
		if(enableGravityOnDrag){
			this.GetComponent<Rigidbody2D>().isKinematic = false;
			scaleWhenCloser = false;
		}

		setupAllowDrag(false);
		achievedTargetArea = true;
		setupIsMoving(false);
		moveMountainManager.achieved("Correct drag: " + this.name);
	}

	void setupIsMoving(bool isMoving){
		this.isMoving = isMoving;
		dragFX.SetActive(isMoving);
	}

	void setupAllowDrag(bool allowDrag){
		this.allowDrag = allowDrag;

		this.GetComponent<Collider2D>().enabled = allowDrag;
			
		if(allowDrag){
			gameObject.layer = backupLayerNo;
		}
		else{
			gameObject.layer = 2;
		}
	}

	IEnumerator returnToInitialPos(){
		setupIsMoving(true);
		setupAllowDrag(false);
		while(transform.position != awakePosition){
			transform.position = Vector3.MoveTowards(transform.position, awakePosition, Time.deltaTime * 3f);
			yield return null;
		}
		setupIsMoving(false);
		setupAllowDrag(true);
	}
		
}
