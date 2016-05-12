using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;

public class Location : MonoBehaviour {

	[System.NonSerialized]
	public Vector3 playerSpawn;
	[System.NonSerialized]
	public Vector3 cameraSpawn;

	public float playerSpawnRightX;
	public float playerSpawnLeftX;

	public float playerSpawnRightY;
	[FormerlySerializedAs("playerSpawnY")]
	[SerializeField]
	public float playerSpawnLeftY;
	public float playerSpawnZ;
	public float playerScale;

	public float cameraSpawnRightX;
	public float cameraSpawnLeftX;
	public float cameraSpawnRightY;
	[FormerlySerializedAs("cameraSpawnY")]
	[SerializeField]
	public float cameraSpawnLeftY;
	public float cameraSpawnZ;

	//public Transform[] parallaxLayers;

	public bool isInterior;
	public char size;//s - small, m - medium, l - large

}
