using UnityEngine;
using System.Collections;
using Fungus;

public class AssessmentManager : MonoBehaviour {

	public Flowchart flowchart;
	private ObjectiveManager objectiveManager;
	private GameManager gameManager;

	public int score = 0;
	public int totalItems = 5;

	//public static int assessmentNumber = 0;

	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		flowchart = this.GetComponent<Flowchart>();

		flowchart.SendFungusMessage ("Part " + objectiveManager.currentPartIndex + " Assessment");

		gameManager.bookHUDbtn.SetActive (false);
		//ExitAssessment("Kwarto ni Haring Fernando");
	}

	public void addScore(){
		score++;
		if(score > totalItems){
			score = totalItems;
		}
	}

	public void ExitAssessment(string sceneToLaunch){
		ObjectiveMapper objectiveMapper = GetComponent<ObjectiveMapper>();
		TalasalitaanManager talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

		if(levelLoader == null){
			this.gameObject.AddComponent<LevelLoader>();
		}
			
		LevelLoader.sceneToLoad = sceneToLaunch;
		Debug.Log("This is the scene to load:" + LevelLoader.sceneToLoad);
		//gameManager.feedDataAndSave();
		Debug.Log("This is the part to save:" + objectiveManager.currentPartIndex);

		if(objectiveMapper.checkIfCurrent()){
			this.GetComponent<ObjectiveMapper> ().checkIfCurrent_misc ();
			FindObjectOfType<LevelLoader> ().launchScene (sceneToLaunch);
		}

		/*
		if (score >= 3) {
			FindObjectOfType<LevelLoader>().launchScene(sceneToLaunch);
			gameManager.prevAssessmentDone = true;
		} 
		else{
			FindObjectOfType<LevelLoader>().discreteLaunchScene("Chapter Selection");
			gameManager.prevAssessmentDone = false;
		}

		if(objectiveMapper.checkIfCurrent()){
			this.GetComponent<ObjectiveMapper>().checkIfCurrent_misc();
		}

		else{
			gameManager.initSaveUpdate ();		
		}*/

		gameManager.bookHUDbtn.SetActive (true);
	}
}
