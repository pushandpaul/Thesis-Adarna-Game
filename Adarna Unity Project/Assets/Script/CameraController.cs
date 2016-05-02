using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public PlayerController player;
	public Camera camera;

	public Vector2 margin;
	public Vector2 smoothing;

	public BoxCollider2D bounds;

	private Vector3 _min;
	private Vector3 _max;

	public float defaultXOffset;
	private float flippedXOffset;
	public float defaultYOffset;
	public float changedYOffset;
	private float xOffset;
	private float yOffset;

	public float zoomSize;
	private float defaultCamSize;

	public bool isFollowing;
	public bool flipped;
	public bool isCenter;
	public bool isZoomed;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		camera = this.GetComponent<Camera>();

		_min = bounds.bounds.min;
		_max = bounds.bounds.max;
		defaultCamSize = camera.orthographicSize;

		flippedXOffset = -defaultXOffset;
	
		Debug.Log("This is the camera's size: " + defaultCamSize);

	}
	void FixedUpdate () {
		var x = transform.position.x;
		var y = transform.position.y;

		if(player.transform.localScale.x > 0)
			flipped = false;
		else if(player.transform.localScale.x < 0)
			flipped = true;

		if(!flipped && !isCenter)
			xOffset = defaultXOffset;
		else if(flipped && !isCenter)
			xOffset = flippedXOffset;
		else if(isCenter){
			xOffset = 0;
		}

		if(isZoomed){
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoomSize, Time.deltaTime);
			yOffset = changedYOffset;
			}
		else{
			camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, defaultCamSize, Time.deltaTime);
			yOffset = defaultYOffset;
		}
			
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

	public void controlZoom(bool isZoomed){
		this.isZoomed = isZoomed;
	}

	public void controlZoom(float zoomSize){
		this.zoomSize = zoomSize;
	}
	public void centerCam(bool isCenter){
		this.isCenter = isCenter;
	}

	public void zoomInZoomOut(float zoomSize, int delay, bool isCenter){
		this.zoomSize = zoomSize;
		StartCoroutine(startZoomUnzoom(delay, isCenter));
	}

	IEnumerator startZoomUnzoom(int delay, bool isCenter){
		controlZoom(true);
		centerCam(isCenter);
		yield return new WaitForSeconds(delay);
		controlZoom(false);
		centerCam(!isCenter);
	}
}
