using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour {
	public PlayerController player;
	public Camera camera;
	public Transform followThis;
	private Transform initialFollowThis;

	private List<SpriteRenderer> foregrounds;
	private SpriteController spriteController;
	private Location location;

	private float foregroundAlphaOrig;
	private float foregroundAlphaZoomed;

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
	[Tooltip("In seconds.")]
	public float defaultZoomDuration = 2f;

	public bool isFollowing;
	public bool flipped;
	public bool isCenter;
	public bool isZoomed;

	void Awake(){
		player = FindObjectOfType<PlayerController>();
		camera = this.GetComponent<Camera>();
		foregrounds = new List<SpriteRenderer>();

		initialFollowThis = followThis;

		spriteController = FindObjectOfType<SpriteController>();
		if(spriteController == null)
			spriteController = gameObject.AddComponent<SpriteController>();
		location = FindObjectOfType<Location>();

		if(location != null){
			foreach(SpriteRenderer layer in location.GetComponentsInChildren<SpriteRenderer>()){
				if(layer.transform.position.z < -2 && layer.sortingLayerName == "Foreground"){
					foregrounds.Add(layer);
				}
			}
		}
			
		foregroundAlphaZoomed = 100f;
		foregroundAlphaOrig = 255f;

		if(lerpSpeed.x == 0)
			lerpSpeed.x = 1f;
		if(lerpSpeed.y == 0)
			lerpSpeed.y = 4f;
		
		
	}

	// Use this for initialization
	void Start () {

		if(followThis == null){
			setPlayerAsFollowing();
		}

		_min = bounds.bounds.min;
		_max = bounds.bounds.max;

		zoomSize = defaultZoomSize;
		initialCamSize = camera.orthographicSize;

		flippedXOffset = -defaultXOffset;
	}
	void FixedUpdate () {
		var x = transform.position.x;
		var y = transform.position.y;

		if(followThis.localScale.x > 0)
			flipped = false;
		else if(followThis.localScale.x < 0)
			flipped = true;

		if(!flipped && !isCenter)
			xOffset = defaultXOffset;
		else if(flipped && !isCenter)
			xOffset = flippedXOffset;
		else if(isCenter){
			xOffset = 0;
		}

		//yOffset = ((defaultYOffset * camera.orthographicSize)/defaultCamSize);
		yOffset = (camera.orthographicSize - defaultCamSize) + defaultYOffset;

		if(isFollowing){
			if(Mathf.Abs(x - (followThis.position.x + xOffset)) > margin.x){
				x = Mathf.Lerp(x, followThis.position.x + xOffset, smoothing.x = Time.deltaTime * lerpSpeed.x);
			}
			if(Mathf.Abs(y - (followThis.position.y + yOffset)) > margin.y){
				y = Mathf.Lerp(y, followThis.position.y + yOffset, smoothing.y = Time.deltaTime * lerpSpeed.y);
			}
		}
			
		var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height);﻿
		x = Mathf.Clamp(x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

		transform.position = new Vector3(x,y, transform.position.z);
	}

	public void setFollowing(Transform followThis){
		this.followThis = followThis;
	}

	public void setPlayerAsFollowing(){
		this.followThis = player.transform;
	}

	public void revertFollowing(){
		this.followThis = initialFollowThis;
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

	public void Init(BoxCollider2D bounds){
		this.bounds = bounds;
		_min = bounds.bounds.min;
		_max = bounds.bounds.max;
	}

	void fadeForegrounds(float currentZoom, float targetZoom, float duration){
		Color tempColor;
		if(targetZoom < currentZoom){
			foreach(SpriteRenderer foreground in foregrounds){
				tempColor = new Color(foreground.color.r, foreground.color.g, foreground.color.b, .5f);
				spriteController.changeColor(foreground, tempColor, duration);
			}
		}
		else if(targetZoom >= currentZoom){
			foreach(SpriteRenderer foreground in foregrounds){
				tempColor = new Color(foreground.color.r, foreground.color.g, foreground.color.b, 1f);
				spriteController.changeColor(foreground, tempColor, duration);
			}
		}
	}

	IEnumerator startZoom(float currentZoom, float targetZoom, float duration){
		float startTime = Time.time ;
		float endTime = startTime + duration;

		//fadeForegrounds(currentZoom, targetZoom, duration);
			
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
		Zoom(initialCamSize, defaultZoomDuration);
		centerCam(false);
	}

}
