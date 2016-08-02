using UnityEngine;
using System.Collections;
using Fungus;

public class CastleBuilding : MonoBehaviour {

	[System.Serializable]
	public class BinaryStates{
		public Sprite correct;
		public Sprite wrong;
	}

	public BinaryStates[] castleStates;
	//public Animator transitionAnimator;
	//public AnimationClip transitionAnim;

	private int currentStateIndex = 0;
	private UIFader backUIFader;
	public SpriteRenderer castleRenderer;
	//public float timeBeforeChange;

	public Flowchart flowchart;

	void Start(){
		GameObject backUIFaderHolder = GameObject.FindGameObjectWithTag("Back UI Fader");
		backUIFader = backUIFaderHolder.GetComponent<UIFader> ();
		flowchart.ExecuteBlock ("Question 0");
	}

	public void correct(){
		currentStateIndex++;
		StartCoroutine (transitioning (castleStates [currentStateIndex].correct, false));
		Debug.Log ("Answer Correct.");
	}

	public void wrong(){
		StartCoroutine (transitioning (castleStates [currentStateIndex].wrong, true));
		Debug.Log("Answer Wrong.");
	}

	public void end(){
		
	}

	IEnumerator transitioning(Sprite newStateSprite, bool isWrong){

		backUIFader.FadeIn (0, 1f, true);

		while(backUIFader.canvasGroup.alpha < 1){
			yield return null;
		}

		castleRenderer.sprite = newStateSprite;

		while(backUIFader.canvasGroup.alpha > 0){
			yield return null;
		}

		/*float remainingTime = transitionAnim.length -  timeBeforeChange;

		transitionAnimator.Play ("blah");

		if(remainingTime < 0){
			remainingTime = 0;
		}
		
		yield return new WaitForSeconds (timeBeforeChange);
		castleRenderer.sprite = newStateSprite;
		yield return new WaitForSeconds (remainingTime);*/

		if(isWrong){
			flowchart.ExecuteBlock ("Wrong");
		}
		else{
			flowchart.ExecuteBlock ("Question " + currentStateIndex);
		}
		
	}
}
