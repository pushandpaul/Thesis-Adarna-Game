using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour {
	public PlayerController player;
	public Camera camera;

	public Vector2 margin;
	public Vector2 smoothing;
	public Vector2 lerpSpeed;

	public BoxCollider2D bounds;

	private Vector3 _min;
	private Vector3 _max;

	public float defaultXOffset;
	private float flippedXOffset;
	public float defaultYOffset;
	//public float changedYOffset;
	private float xOffset;
	private float yOffset;

	[FormerlySerializedAs("zoomSize")]
	[SerializeField]
	public float defaultZoomSize;
	public float zoomSize;
	private float defaultCamSize = 5f;
	public float initialCamSize;
	public float defaultZoomDuration = 2f;

	public bool isFollowing;
	public bool flipped;
	public bool isCenter;
	public bool isZoomed;

	void Awake(){
		if(lerpSpeed.x == 0)
			lerpSpeed.x = 1f;
		if(lerpSpeed.y == 0)
			lerpSpeed.y = 4f;
		
	}

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		camera = this.GetComponent<Camera>();

		_min = bounds.bounds.min;
		_max = bounds.bounds.max;

		zoomSize = defaultZoomSize;
		initialCamSize = camera.orthographicSize;

		flippedXOffset = -defaultXOffset;
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

		yOffset = ((defaultYOffset * camera.orthographicSize)/defaultCamSize);

		if(isFollowing){
			if(Mathf.Abs(x - (player.transform.position.x + xOffset)) > margin.x){
				x = Mathf.Lerp(x, player.transform.position.x + xOffset, smoothing.x = Time.deltaTime * lerpSpeed.x);
			}
			if(Mathf.Abs(y - (player.transform.position.y + yOffset)) > margin.y){
				y = Mathf.Lerp(y, player.transform.position.y + yOffset, smoothing.y = Time.deltaTime * lerpSpeed.y);
			}
		}
			
		var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);﻿
		x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

		transform.position = new Vector3(x,y, transform.position.z);
	}

	public void setZoomSize(float zoomSize){
		this.zoomSize = zoomSize;
	}

	public void resetZoomSize(){
		this.zoomSize = defaultZoomSize;
	}

	public void Zoom(){
		StartCoroutine(startZoom(camera.orthographicSize, zoomSize, defaultZoomDuration));
	}

	public void Zoom(float targetZoom){
		StartCoroutine(startZoom(camera.orthographicSize, targetZoom, defaultZoomDuration));
	}

	public void Zoom(float targetZoom, float duration){
		StartCoroutine(startZoom(camera.orthographicSize, targetZoom, duration));
	}
		
	public void centerCam(bool isCenter){
		this.isCenter = isCenter;
	}

	public void zoomInZoomOut(float zoomSize, int delay, bool isCenter){
		this.zoomSize = zoomSize;
		StartCoroutine(startZoomUnzoom(delay, isCenter));
	}

	IEnumerator startZoom(float currentZoom, float targetZoom, float duration){
		float startTime = Time.time ;
		float endTime = startTime + duration;

		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			camera.orthographicSize = Mathf.Lerp(currentZoom, targetZoom, t);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator startZoomUnzoom(int delay, bool isCenter){
		if(isCenter)
			centerCam(true);
		
		Zoom(zoomSize, defaultZoomDuration);
		yield return new WaitForSeconds(delay);
		Zoom(zoomSize, defaultZoomDuration);
		centerCam(false);
	}

}
