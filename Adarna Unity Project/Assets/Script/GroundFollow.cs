using UnityEngine;
using System.Collections;

public class GroundFollow : MonoBehaviour {
	public PlayerController player;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, transform.position.y, -10f);
	}
}
