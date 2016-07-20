using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Fungus;

public class TutorialManager : MonoBehaviour {
	public Text titleUI;
	public Text descriptionUI;
	private UIFader uiFader;
	private GameManager gameManager;

	[System.Serializable]
	public class Tutorial{
		public string title;
		public string description;
		public bool triggerNext;
		public GameObject display;
	}

	public Tutorial[] tutorials;
	public int currentIndex = 0;

	public int tutorialIndex = 0;
	private DialogInput dialogInput; 
	private PlayerController player;
	private bool tutorialShown;

	public static bool inTutorial = false;

	void Awake () {
		uiFader = GetComponent<UIFader>();
		gameManager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>();

	}

	void OnLevelLoad(){
		foreach(Tutorial tutorial in tutorials){
			tutorial.display.gameObject.SetActive (false);
		}
	}
		
	void Update(){
		dialogInput = FindObjectOfType<DialogInput>();
	
		if(dialogInput != null){
			dialogInput.keyPressMode = DialogInput.KeyPressMode.Disabled;
		}

		if(gameManager.mainHUD.canvasGroup.alpha >  0f){
			gameManager.hideHUDs(false);
		}

	}

	public void Launch(int index){
		inTutorial = true;
		if(player == null){
			player = FindObjectOfType<PlayerController> ();
		}
		this.enabled = true;
		if(index < tutorials.Length){
			this.gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(Launching(index));
		}
		else
			this.gameObject.SetActive(false);
	}

	public void Next(){
		Debug.Log("Clicked");

		gameManager.pause(false);
		if(tutorials[currentIndex].triggerNext){
			if(currentIndex+1 < tutorials.Length){
				Launch(currentIndex+1);
			}
		}
		else{
			StopAllCoroutines();
			StartCoroutine(Closing());
		}
	}

	IEnumerator Launching(int index){
		uiFader.canvasGroup.alpha = 0;
		uiFader.StopAllCoroutines();
		uiFader.FadeIn(0, 5f, false);
		currentIndex =  index;

		/*foreach(NPCInteraction npc in FindObjectsOfType<NPCInteraction>()){
			npc.enabled = false;
		}*/

		if(index > 0){
			tutorials[index-1].display.SetActive(false);
		}

		tutorials[index].display.SetActive(true);
		titleUI.text = tutorials[index].title;
		descriptionUI.text = tutorials[index].description;
		while(uiFader.canvasGroup.alpha != 1f){
			yield return null;
		}
		gameManager.blurredCam.gameObject.SetActive(true);
		if(index == 4 || index == 5){
			gameManager.pause(true);
		}
		else
			gameManager.hideHUDs(false);
		
		while(FindObjectOfType<ScreenFader>().alpha > 0.1f){
			yield return null;
		}

		player.canMove = false;
		player.canJump = false;
		//gameManager.blurredCam.gameObject.SetActive(true);
	}

	IEnumerator Closing(){ 
		uiFader.StopAllCoroutines();
		uiFader.FadeOut(0, 5f);
		//gameManager.pause(false);

		/*foreach(NPCInteraction npc in FindObjectsOfType<NPCInteraction>()){
			npc.enabled = true;
		}*/

		while(uiFader.canvasGroup.alpha != 0){
			yield return null;
		}
		tutorialShown = false;
		if(!DialogueController.inDialogue){
			Debug.Log("Not in dialogue");
			player.canJump = true;
			player.canMove = true;
		}
		else {
			player.canJump = false;
			player.canMove = false;
		}

		if(currentIndex == 5){
			gameManager.pause(false);
		}
		else
			gameManager.hideHUDs(true);

		gameManager.blurredCam.gameObject.SetActive(false);
		tutorials[currentIndex].display.SetActive(false);
		this.gameObject.SetActive(false);

		if(dialogInput != null){
			dialogInput.keyPressMode = DialogInput.KeyPressMode.KeyPressed;
		}

		if(tutorials.Length - 1 == currentIndex){
			this.enabled = false;
		}

		inTutorial = false;	
	}
}
