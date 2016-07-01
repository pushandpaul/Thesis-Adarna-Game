using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour {

	public Vector3 respawnPosition;
	public float storedFaceDirection;

	private PlayerController player;
	public Transform toRespawn;
	private MoveObject mover;

	public enum RespawnRoutine{
		FadeAndRespawn = 0,
	}

	public RespawnRoutine respawnRoutine;

	void Awake(){
		player = FindObjectOfType<PlayerController> ();
		if(toRespawn == null){
			toRespawn = player.transform;
		}
		mover = FindObjectOfType<MoveObject>();
	}

	public void FadeAndRespawnf(){
		StartCoroutine(StartFadeAndSpawn());
	}

	void OnTriggerEnter2D (Collider2D other){
		if(other.tag == "Player"){
			switch(respawnRoutine){
			case (RespawnRoutine.FadeAndRespawn):
				FadeAndRespawnf();
				break;
			}
		}
	}

	IEnumerator StartFadeAndSpawn(){
		GameObject BackUIFaderHolder = GameObject.FindGameObjectWithTag("Back UI Fader");
		UIFader fader = BackUIFaderHolder.GetComponent<UIFader>();
		CanvasGroup faderCanvasGroup = BackUIFaderHolder.GetComponent<CanvasGroup>();
		CameraController camera = FindObjectOfType<CameraController>();

		fader.FadeIn(1, 2f, true);
		if(player != null)
			player.canMove = false;

		while(faderCanvasGroup.alpha != 1){
			yield return null;
		}

		mover.punchMove(respawnPosition, toRespawn);
		toRespawn.localScale = new Vector3 (storedFaceDirection, toRespawn.localScale.y, toRespawn.localScale.z);
		mover.punchMove(new Vector3(respawnPosition.x, respawnPosition.y, camera.transform.position.z), camera.transform);


		while(faderCanvasGroup.alpha != 0){
			yield return null;
		}

		if(player != null)
			player.canMove = true;
	}
}
