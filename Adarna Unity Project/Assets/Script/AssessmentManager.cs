using UnityEngine;
using System.Collections;
using Fungus;

public class AssessmentManager : MonoBehaviour {

	public Flowchart flowchart;
	private ObjectiveManager objectiveManager;

	void Start () {
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		flowchart = this.GetComponent<Flowchart>();
		flowchart.SendFungusMessage("Part " + objectiveManager.currentPartIndex + " Assessment");
	}
}
