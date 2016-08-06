using UnityEngine;
using System.Collections;

public class AgilaRespawnController : MonoBehaviour {

	private AgilaFlyingController agila;

	void Awake(){
		agila = FindObjectOfType<AgilaFlyingController> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Triggered Agila Respawn Controller");
		if(other.tag == "Player"){
			StartCoroutine (WaitTilOutOfScreen ());
		}
	}

	IEnumerator WaitTilOutOfScreen(){
		agila.SetObstacleTriggered(true);
		while(agila.transform.position.y > GameObject.FindWithTag("Out of the Screen").transform.position.y){
			yield return null;
		}
		FindObjectOfType<GenericMinigameManger>().lose();
		//GetComponentInChildren<RespawnManager>().FadeAndRespawnf ();
		agila.SetCanMove (false);
		agila.SetObstacleTriggered(false);
	}
}
