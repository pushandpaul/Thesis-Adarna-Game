using UnityEngine;
using System.Collections;
using Fungus;

public class AssessmentManager : MonoBehaviour {

	public Flowchart flowchart;
	private ObjectiveManager objectiveManager;
	private GameManager gameManager;

	public int score = 0;
	public int totalItems = 5;
	private TalasalitaanManager.PartDataList partReference;

	public AudioClip BGMusic;

	public static int assessmentNumber = 0;

	void Awake () {
		gameManager = FindObjectOfType<GameManager>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		flowchart = this.GetComponent<Flowchart>();
		partReference = FindObjectOfType<TalasalitaanManager>().partDataList;

		gameManager.bookHUDbtn.SetActive (false);
		//ExitAssessment("Kwarto ni Haring Fernando");
	}

	void Start(){
		flowchart.SendFungusMessage ("Part " + assessmentNumber + " Assessment");
		Debug.Log("Part " + assessmentNumber + " Assessment");
		FindObjectOfType<BGMManager>().overridePlay(BGMusic);
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

		Debug.Log("This is the part to save:" + objectiveManager.currentPartIndex);


		if(score >= 3 && !partReference.partsData[assessmentNumber].isFinished){
			partReference.partsData[assessmentNumber].isFinished = true;
			if(this.GetComponent<ObjectiveMapper> ().checkIfCurrent()){
				FindObjectOfType<LevelLoader> ().launchScene (sceneToLaunch);
			}
			else{
				levelLoader.discreteLaunchScene("Chapter Selection");
				FindObjectOfType<TalasalitaanManager>().RewriteJson();
			}
		}
		else{
			levelLoader.discreteLaunchScene("Chapter Selection");
		}

		this.GetComponent<ObjectiveMapper> ().checkIfCurrent_misc ();
		//gameManager.feedDataAndSave();

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
