using UnityEngine;
using System.Collections;

public class DragabbleTargetArea : MonoBehaviour {

	public Collider2D dragabbleTarget;
	public bool checkerInParent;
	private MoveMountain_Draggable draggable;
	public bool triggered;

	void Awake(){
		if(!checkerInParent){
			draggable = dragabbleTarget.GetComponent<MoveMountain_Draggable>();
		}
		else
			draggable = dragabbleTarget.GetComponentInParent<MoveMountain_Draggable>();
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other == dragabbleTarget){
			draggable.triggeredTargetArea = true;
			triggered = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		
		if(other == dragabbleTarget){
			draggable.triggeredTargetArea = false;
			triggered = false;
		}
	}
}
