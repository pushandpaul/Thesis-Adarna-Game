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
	private FollowerManager followerManager;

	public bool zoomCam;
	public bool centerCam;

	public static bool inDialogue;
	void Start () {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		objectiveTextBox = objectiveManager.GetComponent<TextBoxManager>();
		followerManager = FindObjectOfType<FollowerManager> ();
		this.objectivePanelFader = objectiveManager.objectivePanelFader;
	}

	public void startDialogue(){
		inDialogue = true;

		foreach(FollowTarget follower in followerManager.activeFollowers){
			follower.isFollowing = false;
			if(follower.anim != null){
				follower.anim.SetFloat ("Speed", 0f);
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
		inDialogue = false;

		foreach(FollowTarget follower in followerManager.activeFollowers){
			follower.isFollowing = true;
			if(follower.anim != null){
				follower.anim.SetFloat ("Speed", 0f);
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
