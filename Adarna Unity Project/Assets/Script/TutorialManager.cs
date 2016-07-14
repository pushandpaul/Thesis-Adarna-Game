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
	void Awake () {
		uiFader = GetComponent<UIFader>();
		gameManager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>();
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
		
		while(FindObjectOfType<ScreenFader>().alpha > 0){
			yield return null;
		}
			
		player.canJump = false;
		player.canMove = false;
			
		//gameManager.blurredCam.gameObject.SetActive(true);
	}

	IEnumerator Closing(){ 
		uiFader.StopAllCoroutines();
		uiFader.FadeOut(0, 5f);
		//gameManager.pause(false);

		while(uiFader.canvasGroup.alpha != 0){
			yield return null;
		}
		if(!DialogueController.inDialogue){
			Debug.Log("Not in dialogue");
			player.canJump = true;
			player.canMove = true;
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
	}
}
