using UnityEngine;
using System.Collections;
using Fungus;

public class CastleBuilding : MonoBehaviour {

	[System.Serializable]
	public class BinaryStates{
		public Sprite correct;
		public Sprite[] wrong;
	}

	private GenericMinigameManger minigameManager;

	public BinaryStates[] castleStates;
	//public Animator transitionAnimator;
	//public AnimationClip transitionAnim;

	private int currentStateIndex = 0;
	private UIFader backUIFader;
	public SpriteRenderer castleRenderer;
	//public float timeBeforeChange;

	public Flowchart flowchart;

	public GameObject magicEffectPrefab;
	public Vector3 magicEffectPos;
	private float magicEffectDuration;


	void Awake(){
		minigameManager = FindObjectOfType<GenericMinigameManger> ();
		magicEffectDuration = magicEffectPrefab.GetComponent<ParticleSystem> ().duration;
	}

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

	public void wrong(int spriteIndex){
		StartCoroutine (transitioning (castleStates [currentStateIndex].wrong[spriteIndex], true));
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

		GameObject tempParticle = (GameObject)Instantiate (magicEffectPrefab);
		ParticleSystem particle = tempParticle.GetComponent<ParticleSystem> ();

		while(particle.IsAlive()){
			yield return null;
		}
		Destroy (tempParticle);

		if(isWrong){
			flowchart.ExecuteBlock ("Wrong");
			//Excecute lose method in minigameManager in this block^
		}
		else{
			if(currentStateIndex < castleStates.Length){
				flowchart.ExecuteBlock ("Question " + currentStateIndex);
			}
			else{
				flowchart.ExecuteBlock ("End");
			}
		}
		
	}
}
