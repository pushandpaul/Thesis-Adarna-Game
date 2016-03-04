using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour {

	public bool isRight;
	public string nextLocation;

	void OnTriggerEnter2D (Collider2D other){
		LevelManager.exitInRight = isRight;
		SceneManager.LoadScene(nextLocation);
	}
}
