using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public PlayerController player;

	public Vector2 margin;
	public Vector2 smoothing;

	public BoxCollider2D bounds;

	private Vector3 _min;
	private Vector3 _max;

	public float xOffset;
	public float yOffset;

	public bool isFollowing;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		_min = bounds.bounds.min;
		_max = bounds.bounds.max;

	}
	void FixedUpdate () {
		var x = transform.position.x;
		var y = transform.position.y;

		if(isFollowing){
			if(Mathf.Abs(x - (player.transform.position.x + xOffset)) > margin.x){
				x = Mathf.Lerp(x, player.transform.position.x + xOffset, smoothing.x = Time.deltaTime);
			}

			if(Mathf.Abs(y - (player.transform.position.y + yOffset)) > margin.y){
				y = Mathf.Lerp(y, player.transform.position.y + yOffset, smoothing.y = Time.deltaTime);
			}
		}

		var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);﻿
		x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

		transform.position = new Vector3(x,y, transform.position.z);
	}

		
}
