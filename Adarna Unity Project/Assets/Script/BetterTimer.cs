using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BetterTimer : MonoBehaviour {

	public GameObject timeDisplayUI;
	public Text timeDisplayText;
	public bool onGoing;
	public bool isPaused;

	void Awake(){
		
		timeDisplayUI.SetActive(false);
		timeDisplayText = timeDisplayUI.GetComponentInChildren<Text>();
	}

	public void startTimer(float timeLimit){
		timeDisplayUI.SetActive(true);
		StartCoroutine(startingTimer(timeLimit));
	}
		
	IEnumerator startingTimer(float currentTime){
		string minutes = "";
		string seconds = "";
		onGoing = true;

		while(currentTime > 0 && onGoing){
			if(!isPaused){
				currentTime -= Time.deltaTime;
				minutes = Mathf.Floor(currentTime/60).ToString("00");
				seconds = (currentTime % 60).ToString("00");
				if(timeDisplayText != null){
					timeDisplayText.text = minutes + ":" + seconds;
				}
				Debug.Log("Current time: " + minutes + ":" + seconds);
			}
			yield return null;
		}
		onGoing = false;
	}
}
