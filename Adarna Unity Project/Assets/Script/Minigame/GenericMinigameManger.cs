using UnityEngine;
using System.Collections;

public class GenericMinigameManger : MonoBehaviour {

	public string minigameName;
	public bool allowMinigameStart = false;
	public string title;
	public string description;
	private LayuninUIManager layuninUI;
	public AudioClip minigameAudio;

	void Awake () {
		layuninUI = FindObjectOfType<LayuninUIManager>();
	}

	void Start () {
		if(LayuninUIManager.lastTriggered != minigameName){
			layuninUI.Launch(title, description, LayuninUIManager.CloseControl.PressAnywhere, true);
			LayuninUIManager.lastTriggered = minigameName;
			if(minigameAudio != null){
				layuninUI.songAfter = minigameAudio;
			}

		}
		else
			allowMinigameStart = true;
	}

	void Update () {
		if(layuninUI.layuninUI.canvasGroup.alpha > 0){
			return;
		}
		else{
			allowMinigameStart = true;
			this.enabled = false;
		}
	}

	public bool checkCanStart(){
		return allowMinigameStart;
	}
}
