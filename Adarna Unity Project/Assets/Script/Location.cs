using UnityEngine;
using System.Collections;

public class Location : MonoBehaviour {

	public float playerSpawnRightX;
	public float playerSpawnLeftX;
	public float playerSpawnY;
	public float playerSpawnZ;
	public float playerScale;

	public float cameraSpawnRightX;
	public float cameraSpawnLeftX;
	public float cameraSpawnY;
	public float cameraSpawnZ;

	public Transform[] parallaxLayers;

	public bool isInterior;
	public char size; //s - small, m - medium, l - large
}
