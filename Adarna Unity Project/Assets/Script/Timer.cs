using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {


	public float timeInSecs;
	private int tempTime;
	private bool start;
	private bool isOnGoing;
	void Update () {
		if(start && timeInSecs > 0){
			tempTime = (int)timeInSecs;
			timeInSecs -= Time.deltaTime;
			if((int)timeInSecs != tempTime)
				Debug.Log("" + (int)timeInSecs);
		}
		else
			isOnGoing = false;
	}

	public void startTimer(){
		start = true;
		isOnGoing = true;
		Debug.Log("Timer Started");
	}

	public void stopTimer(){
		start = false;
		isOnGoing = false;
		Debug.Log("Timer Stopped");
	}

	public bool checkIfOnGoing(){
		return isOnGoing;
	}
}
