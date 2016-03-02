using UnityEngine;
using System.Collections;

public class XMatcher: MonoBehaviour {
	public PlayerController player;
	private float defaultY;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		defaultY = transform.position.y;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x, defaultY, 0f);
	}
}
