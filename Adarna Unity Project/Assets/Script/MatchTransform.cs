using UnityEngine;
using System.Collections;

public class MatchTransform : MonoBehaviour {

	public Transform asBasis;
	public bool isAbsoluteScale;
	public bool matchOnStart;
	public bool matchPlayer;

	void Start(){
		if(matchPlayer)
			asBasis = FindObjectOfType<PlayerController>().transform;
	}

	public void scale(){
		Debug.Log("Attempt to scale '" + transform.name + "'.");
		if(isAbsoluteScale)
			scaleAbsolute();
		else
			scaleCompletely();
	}

	void scaleCompletely(){
		transform.localScale = asBasis.localScale;
	}

	void scaleAbsolute(){
		transform.localScale = new Vector3(Mathf.Abs(asBasis.localScale.x), Mathf.Abs(asBasis.localScale.y), Mathf.Abs(asBasis.localScale.z));
	}

	void positionCompletely(){
		transform.position = asBasis.position;
	}

	void positionWithOffset(){
		//Add codes for future use
	}
}
