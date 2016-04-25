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
	private FollowTarget[] followers;

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
		followers = FindObjectsOfType<FollowTarget>();

		if(followers != null || followers.Length > 0){
			foreach(FollowTarget follower in followers){
				follower.isFollowing = false;
				follower.anim.SetFloat("Speed", 0f);
			}
		}
			
		player.disablePlayerMovement();
		if(centerCam)
			camera.centerCam(centerCam);

		if(zoomCam)
			camera.controlZoom(zoomCam);

		objectivePanelFader.canvasGroup.alpha = 0;
		//objectiveTextBox.disableTextBox();
		//objectivePanelFader.FadeOut(0);

	}

	public void endDialogue(){
		player.enablePlayerMovement();

		followers = FindObjectsOfType<FollowTarget>();

		if(followers != null || followers.Length > 0){
			foreach(FollowTarget follower in followers){
				follower.isFollowing = true;
			}
		}

		if(centerCam)
			camera.centerCam(false);

		if(zoomCam)
			camera.controlZoom(false);
		
		if(objectiveManager.currentObjective.OnReach.Contains(Objective.ActionOnReach.DisplayToTextBox) && !objectiveManager.currentObjective.textBoxDisplayed){
			Debug.Log("Allowed fade in");
			//objectiveTextBox.enableTextBox();
			objectivePanelFader.FadeIn(objectiveManager.fadeDelay, objectiveManager.panelFaderSpeed, true);
			objectiveManager.currentObjective.textBoxDisplayed = true;
		}
	}
}
