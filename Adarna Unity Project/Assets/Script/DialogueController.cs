using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

	private PlayerController player;
	private CameraController camera;
	private ObjectiveManager objectiveManager;
	private TextBoxManager objectiveTextBox;

	public bool zoomCam;
	public bool centerCam;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		objectiveTextBox = objectiveManager.GetComponent<TextBoxManager>();
	}

	public void startDialogue(){
		player.disablePlayerMovement();
		if(centerCam)
			camera.centerCam();

		if(zoomCam)
			camera.zoomCam();
		objectiveTextBox.disableTextBox();

	}

	public void endDialogue(){
		player.enablePlayerMovement();
		if(centerCam)
			camera.uncenterCam();

		if(zoomCam)
			camera.unzoomCam();
		objectiveTextBox.enableTextBox();
	}
		
}
