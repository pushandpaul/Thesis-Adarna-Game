using UnityEngine;
using System.Collections;

public class MatchTransform : MonoBehaviour {

	public Transform asBasis;
	public bool isAbsoluteScale;
	public bool isOriginalScale;
	public bool matchOnStart;
	public bool matchPlayer;

	void Awake(){
		if(matchPlayer)
			asBasis = FindObjectOfType<PlayerController>().transform;
	}

	public void scale(){
		Debug.Log("Attempt to scale '" + transform.name + "'.");
		if(isAbsoluteScale && !isOriginalScale)
			scaleAbsolute();
		else if(!isAbsoluteScale && !isOriginalScale)
			scaleCompletely();
		else if(!isAbsoluteScale && isOriginalScale)
			scaleOriginal();
	}

	void scaleCompletely(){
		transform.localScale = asBasis.localScale;
	}

	void scaleAbsolute(){
		transform.localScale = new Vector3(Mathf.Abs(asBasis.localScale.x), Mathf.Abs(asBasis.localScale.y), Mathf.Abs(asBasis.localScale.z));
	}

	void scaleOriginal(){
		if(transform.localScale.x > 0){
			transform.localScale = new Vector3(Mathf.Abs(asBasis.localScale.x), Mathf.Abs(asBasis.localScale.y), Mathf.Abs(asBasis.localScale.z));
		}
		else if(transform.localScale.x < 0){
			transform.localScale = new Vector3(-Mathf.Abs(asBasis.localScale.x), Mathf.Abs(asBasis.localScale.y), Mathf.Abs(asBasis.localScale.z));
		}
	}

	void positionCompletely(){
		transform.position = asBasis.position;
	}

	void positionWithOffset(){
		//Add codes for future use
	}

	public void setIsOriginalScale(bool isOriginalScale){
		this.isOriginalScale = isOriginalScale;
		isAbsoluteScale = false;
	}
	public void setIsAbsoluteScale(bool isAbsoluteScale){
		this.isAbsoluteScale = isAbsoluteScale;
		isOriginalScale = false;
	}
}
