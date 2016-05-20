using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {

	public bool useCamDefaults;//uses the default values in the camera controller
	public bool revertOnTriggerExit;
	public float targetSize;
	public float zoomDuration;
	public float revertDuration;

	private CameraController camera;

	void Awake(){
		camera = FindObjectOfType<CameraController>();	
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			if(useCamDefaults){
				camera.Zoom();
			}
			else{
				camera.Zoom(targetSize, zoomDuration);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(revertOnTriggerExit && other.tag == "Player"){
			if(useCamDefaults){
				camera.Zoom(camera.initialCamSize);
			}
			else
				camera.Zoom(camera.initialCamSize, revertDuration);
		}
	}
}
