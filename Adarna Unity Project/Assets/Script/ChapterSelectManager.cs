using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChapterSelectManager : MonoBehaviour {

	public ChapterPanel selectChapter;
	public ChapterPanel chapterBefore;
	public ChapterPanel chapterAfter;

	public Button selectChapterButton;
	public Button restartGameButton;
	public Button startAssessment;

	public Sprite[] thumbnails;

	private ConfirmationBox confirmationBox;
	private GameManager gameManager;
	private TalasalitaanManager.PartDataList partReference;
	private LocationMarker locationMarker;

	public AudioClip BGM;
	public GameObject backButton;
	private int currentPartIndex = 0;

	public Button backBtn;
	public Button nextBtn;

	void Awake(){
		gameManager = FindObjectOfType<GameManager>();
		gameManager.GetComponentInChildren<TutorialManager> (true).gameObject.SetActive (false);
		confirmationBox = gameManager.GetComponentInChildren<ConfirmationBox>(true);
		locationMarker = gameManager.GetComponentInChildren<LocationMarker>(true);
		partReference = FindObjectOfType<TalasalitaanManager>().partDataList;
		currentPartIndex = gameManager.latestPartIndex;
		if(currentPartIndex > 1){
			if(!partReference.partsData[currentPartIndex - 1].isFinished){
				currentPartIndex = gameManager.latestPartIndex - 1;
			}
		}
		initPanels();
	}

	void Start(){
		gameManager.HUDs.Add (backButton);
		gameManager.setHUDs(true);
		gameManager.pauseButton.SetActive(false);
		gameManager.setPauseMenu(false);
		locationMarker.gameObject.SetActive (false);
		FindObjectOfType<ObjectiveManager>().objectivePanelFader.StopAllCoroutines();
		FindObjectOfType<ObjectiveManager>().objectivePanelFader.canvasGroup.alpha = 0f;
		if(BGM != null){
			FindObjectOfType<BGMManager>().overridePlay(BGM);
		}
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
			next();
		}

		else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
			back();
		}
	}

	public void next(){
		if(currentPartIndex < 12){
			if(currentPartIndex == 11){
				nextBtn.gameObject.SetActive(false);
			}
			else{
				nextBtn.gameObject.SetActive(true);
				backBtn.gameObject.SetActive(true);
			}
			currentPartIndex++;
		}
		initPanels();
	}

	public void back(){
		if(currentPartIndex > 0){
			if(currentPartIndex == 1){
				backBtn.gameObject.SetActive(false);
			}
			else{
				backBtn.gameObject.SetActive(true);
				nextBtn.gameObject.SetActive(true);
			}
			currentPartIndex--;
		}
		initPanels();
	}

	void initPanels(){
		int previousPartIndex = gameManager.latestPartIndex - 1;
		if(currentPartIndex == 0){
			selectChapterButton.interactable = true;
			startAssessment.interactable = false;
		}
		else if(currentPartIndex > gameManager.latestPartIndex){
			selectChapterButton.interactable = false;
			//if(partReference.partsData[i].isFinished)
			startAssessment.interactable = false;
		}

		else if(currentPartIndex <= gameManager.latestPartIndex){
			
			if (currentPartIndex == gameManager.latestPartIndex && partReference.partsData [previousPartIndex].isFinished || currentPartIndex == 1) {
				selectChapterButton.interactable = true;
			} else if (currentPartIndex < gameManager.latestPartIndex)
				selectChapterButton.interactable = true;
			else
				selectChapterButton.interactable = false;

			if(currentPartIndex <= previousPartIndex){
				startAssessment.interactable = true;
			}
			else
				startAssessment.interactable = false;
		}

		else
			selectChapterButton.interactable = true;

		setChapterPanel(selectChapter, currentPartIndex);
		if(currentPartIndex < 12){
			setChapterPanel(chapterAfter, currentPartIndex + 1);
		}
		else
			chapterAfter.gameObject.SetActive(false);
		
		if(currentPartIndex > 0)
			setChapterPanel (chapterBefore, currentPartIndex - 1);

		else 
			chapterBefore.gameObject.SetActive(false);
	}

	void setChapterPanel(ChapterPanel panel, int index){
		panel.gameObject.SetActive(true);
		int previousPartIndex = gameManager.latestPartIndex - 1;

		if(previousPartIndex < 0){
			previousPartIndex = 0;
		}

		if(index <= gameManager.latestPartIndex){
			if(partReference.partsData[previousPartIndex].isFinished || index <= 1){
				panel.image.enabled = true;
				panel.image.sprite = thumbnails [index];
			}
			else
				panel.image.enabled = false;
		}

		else{
			panel.image.enabled = false;
		}

		if(partReference.partsData[index].partIndex > 0){
			panel.partIndexText.text = "" + partReference.partsData[index].partIndex;
		}
		else
			panel.partIndexText.text ="T";

		if(index > gameManager.latestPartIndex || !partReference.partsData[previousPartIndex].isFinished && index > 1){
			panel.titleText.text = "????";

		}
		else{
			panel.titleText.text = partReference.partsData[index].title;
		}
			
	}

	public void selectPart(){
		
		if(currentPartIndex == 0){
			gameManager.initTutorial(false);
		}
		else
			gameManager.loadPartData(currentPartIndex);

		foreach(GameObject HUD in gameManager.HUDs){
			if(HUD.name == "Back Button"){
				gameManager.HUDs.Remove(HUD);
				break;
			}
		}
			
		FindObjectOfType<DoorAndExitController>().exitsInScenes = new List<DoorAndExitController.ExitsInScene>();
		locationMarker.gameObject.SetActive (true);
		gameManager.pauseButton.SetActive(true);
		gameManager.setPauseMenu(true);
	}

	public void restartGameSave(){
		confirmationBox.show("delete save");
	}

	public void returnMainMenu(){

		foreach(GameObject HUD in gameManager.HUDs){
			if(HUD.name == "Back Button"){
				gameManager.HUDs.Remove(HUD);
				break;
			}
		}

		FindObjectOfType<LevelLoader>().launchScene("Main Menu");
	}

	public void launchAssessment(){
		//AssessmentManager.assessmentNumber = currentPartIndex;
		AssessmentManager.assessmentNumber = currentPartIndex;
		gameManager.setPauseMenu(true);
		FindObjectOfType<LevelLoader> ().launchScene ("() Assessment");
	}
}
