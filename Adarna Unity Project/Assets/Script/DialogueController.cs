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
	private GameManager gameManager;
	private TextBoxManager objectiveTextBox;
	private UIFader objectivePanelFader;
	private FollowerManager followerManager;

	private ObjectInteraction[] objectsInteraction;
	private NPCInteraction[] npcsInteraction;

	public bool zoomCam;
	public bool centerCam;

	public static bool inDialogue;
	void Awake () {
		player = FindObjectOfType<PlayerController>();
		camera = FindObjectOfType<CameraController>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		objectiveTextBox = objectiveManager.GetComponent<TextBoxManager>();
		followerManager = FindObjectOfType<FollowerManager> ();
		gameManager = FindObjectOfType<GameManager>();
		this.objectivePanelFader = objectiveManager.objectivePanelFader;
	}

	public void startDialogue(){
		inDialogue = true;
		enableInteraction(false);
		DoorHandler[] doors = FindObjectsOfType<DoorHandler>();

		StopAllCoroutines();
		StartCoroutine(fadeObjectivePanel());

		foreach(DoorHandler door in doors){
			door.enabled = false;
		}

		foreach(FollowTarget follower in followerManager.activeFollowers){
			follower.isFollowing = false;
			if(follower.anim != null){
				follower.anim.SetFloat ("Speed", 0f);
			}
		}
			
		player.disablePlayerMovement();
		player.setCanJump(false);
		if(centerCam)
			camera.centerCam(centerCam);

		if(zoomCam)
			camera.Zoom();
	}

	public void endDialogue(){
		player.enablePlayerMovement();
		player.setCanJump(true);
		inDialogue = false;
		enableInteraction(true);
		DoorHandler[] doors = FindObjectsOfType<DoorHandler>();

		foreach(DoorHandler door in doors){
			door.enabled = true;
		}
			
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

	IEnumerator fadeObjectivePanel(){
		//gameManager.mainHUD.canvasGroup.interactable = false;
		//gameManager.mainHUD.canvasGroup.alpha = 0f;
		objectivePanelFader.StopAllCoroutines();
		objectivePanelFader.canvasGroup.alpha = 0f;
		while(DialogueController.inDialogue){
			yield return null;
		}

		if(objectiveManager.currentObjective.OnReach.Contains(Objective.ActionOnReach.DisplayToTextBox)){
			objectiveManager.currentObjective.displayToTextBox();
			objectivePanelFader.FadeIn(objectiveManager.fadeDelay, objectiveManager.panelFaderSpeed, true);
		}
		else{
			objectivePanelFader.canvasGroup.alpha = 0f;
		}
			
		//gameManager.mainHUD.canvasGroup.alpha = 1f;
		//gameManager.mainHUD.canvasGroup.interactable = true;
	}
}
