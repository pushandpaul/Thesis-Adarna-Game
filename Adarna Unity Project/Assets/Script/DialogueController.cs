using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

	private PlayerController player;
	private CameraController camera;
	private ObjectiveManager objectiveManager;
	private TextBoxManager objectiveTextBox;
	private UIFader objectivePanelFader;

	public bool zoomCam;
	public bool centerCam;
	void Start () {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		objectiveTextBox = objectiveManager.GetComponent<TextBoxManager>();
		this.objectivePanelFader = objectiveManager.objectivePanelFader;

	}

	public void startDialogue(){
		player.disablePlayerMovement();
		if(centerCam)
			camera.centerCam();

		if(zoomCam)
			camera.zoomCam();

		objectivePanelFader.canvasGroup.alpha = 0;
		//objectiveTextBox.disableTextBox();
		//objectivePanelFader.FadeOut(0);

	}

	public void endDialogue(){
		player.enablePlayerMovement();
		if(centerCam)
			camera.uncenterCam();

		if(zoomCam)
			camera.unzoomCam();
		
		if(objectiveManager.currentObjective.OnReach.Contains(Objective.ActionOnReach.DisplayToTextBox) && !objectiveManager.currentObjective.textBoxDisplayed){
			Debug.Log("Allowed fade in");
			//objectiveTextBox.enableTextBox();
			objectivePanelFader.FadeIn(objectiveManager.fadeDelay, objectiveManager.panelFaderSpeed, true);
			objectiveManager.currentObjective.textBoxDisplayed = true;
		}
	}
}
