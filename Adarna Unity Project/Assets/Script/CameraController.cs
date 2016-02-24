using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public PlayerController player;

	public bool isFollowing;

	public float xOffset;
	public float yOffset;

	private Vector3 position1;
	private Vector3 position2;
	public float flipSpeed = 1.0f;

	private float startPoint;
	private float endPoint;
	public bool flipped;
	// Use this for initialization
	void Start () {
	
		player = FindObjectOfType<PlayerController>();
		isFollowing = true;
		//position1 = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, -10f);
		//position2 = new Vector3(-(player.transform.position.x + xOffset), player.transform.position.y + yOffset, -10f);

	}
	
	// Update is called once per frame
	void Update () {
		if(isFollowing){
			transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, -10f);
		}
		//position1 = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, -10f);
		//position2 = new Vector3(-(player.transform.position.x + xOffset), player.transform.position.y + yOffset, -10f);
		//startPoint = player.transform.position.x + xOffset;
		//endPoint = player.transform.position.x + xOffset;
	}

	/*void LateUpdate(){
		if(!flipped)
			Flip();
	}
	public void Flip(){
		transform.position = Vector3.Lerp(position1, position2, (Mathf.Sin(flipSpeed * Time.time)));
		Debug.Log("Current position: " + transform.position.x + " , Target position: " + endPoint);
		if(transform.position.x == endPoint){
			flipped = true;
			//startPoint = player.transform.position.x + xOffset;
			endPoint = player.transform.position.x + xOffset;
		}
		
	}*/
}
