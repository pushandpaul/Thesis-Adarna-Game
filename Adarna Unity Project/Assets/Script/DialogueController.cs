using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	private ObjectInteraction[] objectsInteraction;
	private NPCInteraction[] npcsInteraction;

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
		enableInteraction(false);

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
			camera.Zoom();

		objectivePanelFader.canvasGroup.alpha = 0;
		//objectiveTextBox.disableTextBox();
		//objectivePanelFader.FadeOut(0);

	}

	public void endDialogue(){
		player.enablePlayerMovement();
		inDialogue = false;
		enableInteraction(true);

		foreach(FollowTarget follower in followerManager.activeFollowers){
			follower.isFollowing = true;
			if(follower.anim != null){
				follower.anim.SetFloat ("Speed", 0f);
			}
		}
		if(centerCam)
			camera.centerCam(false);

		if(zoomCam)
			camera.Zoom(camera.initialCamSize);
		
		if(objectiveManager.currentObjective.OnReach.Contains(Objective.ActionOnReach.DisplayToTextBox) && !objectiveManager.currentObjective.textBoxDisplayed){
			Debug.Log("Allowed fade in");
			//objectiveTextBox.enableTextBox();
			objectivePanelFader.FadeIn(objectiveManager.fadeDelay, objectiveManager.panelFaderSpeed, true);
			objectiveManager.currentObjective.textBoxDisplayed = true;
		}
	}

	void enableInteraction(bool enable){
		npcsInteraction = FindObjectsOfType<NPCInteraction>();
		objectsInteraction = FindObjectsOfType<ObjectInteraction>();

		foreach(NPCInteraction npcInteraction in npcsInteraction){
			npcInteraction.GetComponent<Collider2D>().enabled = enable;
		}

		foreach(ObjectInteraction objectInteraction in objectsInteraction){
			objectInteraction.GetComponent<Collider2D>().enabled = enable;
		}
	}
}
