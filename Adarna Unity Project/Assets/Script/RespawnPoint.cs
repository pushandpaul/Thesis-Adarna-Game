using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {

	public RespawnManager respawnManager;

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			respawnManager.respawnPosition = this.transform.position;
		}
	}
}
