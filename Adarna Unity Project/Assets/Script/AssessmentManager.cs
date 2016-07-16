using UnityEngine;
using System.Collections;
using Fungus;

public class AssessmentManager : MonoBehaviour {

	public Flowchart flowchart;
	private ObjectiveManager objectiveManager;
	private GameManager gameManager;

	void Start () {
		gameManager = FindObjectOfType<GameManager>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		flowchart = this.GetComponent<Flowchart>();
		flowchart.SendFungusMessage("Part " + objectiveManager.currentPartIndex + " Assessment");
		//ExitAssessment("Kwarto ni Haring Fernando");
	}

	public void ExitAssessment(string sceneToLaunch){
		ObjectiveMapper objectiveMapper = GetComponent<ObjectiveMapper>();
		TalasalitaanManager talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
		LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

		if(levelLoader == null){
			this.gameObject.AddComponent<LevelLoader>();
		}
		if(objectiveMapper.checkIfCurrent()){
			FindObjectOfType<LevelLoader>().launchScene(sceneToLaunch);
			Debug.Log("This is the scene to load:" + LevelLoader.sceneToLoad);
			this.GetComponent<ObjectiveMapper>().checkIfCurrent_misc();
			//gameManager.feedDataAndSave();
			Debug.Log("This is the part to save:" + objectiveManager.currentPartIndex);
		}
	}
}
