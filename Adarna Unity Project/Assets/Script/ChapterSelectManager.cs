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

	private ConfirmationBox confirmationBox;
	private GameManager gameManager;
	private TalasalitaanManager.PartDataList partReference;
	private LocationMarker locationMarker;

	public GameObject backButton;
	private int currentPartIndex = 0;

	void Awake(){
		gameManager = FindObjectOfType<GameManager>();
		gameManager.GetComponentInChildren<TutorialManager> (true).gameObject.SetActive (false);
		confirmationBox = gameManager.GetComponentInChildren<ConfirmationBox>(true);
		locationMarker = gameManager.GetComponentInChildren<LocationMarker>(true);
		partReference = FindObjectOfType<TalasalitaanManager>().partDataList;
		currentPartIndex = gameManager.latestPartIndex;
		initPanels();
	}

	void Start(){
		gameManager.HUDs.Add (backButton);
		gameManager.setHUDs(true);
		gameManager.setPauseMenu(false);
		gameManager.pauseButton.SetActive(false);
		locationMarker.gameObject.SetActive (false);
		FindObjectOfType<ObjectiveManager>().objectivePanelFader.StopAllCoroutines();
		FindObjectOfType<ObjectiveManager>().objectivePanelFader.canvasGroup.alpha = 0f;
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
			currentPartIndex++;
		}
		initPanels();
	}

	public void back(){
		if(currentPartIndex > 0){
			currentPartIndex--;
		}
		initPanels();
	}

	void initPanels(){

		if(currentPartIndex == 0){
			selectChapterButton.interactable = true;
			startAssessment.interactable = false;
		}
		else if(currentPartIndex > gameManager.latestPartIndex){
			selectChapterButton.interactable = false;
			startAssessment.interactable = false;
		}

		else if(currentPartIndex <= gameManager.latestPartIndex){

			if (!gameManager.prevAssessmentDone) {

				if (currentPartIndex == gameManager.latestPartIndex) {
					selectChapterButton.interactable = false;
					startAssessment.interactable = false;
				} 
				else{
					selectChapterButton.interactable = true;
					startAssessment.interactable = true;
				}

			} 
			else{
				selectChapterButton.interactable = true;
				startAssessment.interactable = true;
			}
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
	
		if(partReference.partsData[index].partIndex > 0){
			panel.partIndexText.text = "" + partReference.partsData[index].partIndex;
		}
		else
			panel.partIndexText.text ="T";

		if(index > gameManager.latestPartIndex){
			panel.titleText.text = "????";
		}

		else if(index == gameManager.latestPartIndex && !gameManager.prevAssessmentDone){
			panel.titleText.text = "????";
		}
		else
			panel.titleText.text = partReference.partsData[index].title;
	}

	public void selectPart(){
		
		if(currentPartIndex == 0){
			gameManager.initTutorial();
		}
		else
			gameManager.loadPartData(currentPartIndex);

		foreach(GameObject HUD in gameManager.HUDs){
			if(HUD.name == "Back Button"){
				gameManager.HUDs.Remove(HUD);
				break;
			}
		}
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
		FindObjectOfType<LevelLoader> ().launchScene ("() Assessment");
	}
}
