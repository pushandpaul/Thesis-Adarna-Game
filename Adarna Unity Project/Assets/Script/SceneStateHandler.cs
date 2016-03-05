using UnityEngine;
using System.Collections;

public class SceneStateHandler : MonoBehaviour {

	public Location location;
	public float[] layerX;

	private static bool created;

	void Awake() {
		location = FindObjectOfType<Location>();
		layerX = new float[location.parallaxLayers.Length];
		saveCoordinates();

		if(!created){
			DontDestroyOnLoad(this.gameObject);
			created = true;
		}
		else
			Destroy(this.gameObject);
	}
	public void saveCoordinates(){
		Debug.Log("Entered: setCoordinates function");
		for(int i = 0; i < location.parallaxLayers.Length; i++){
			layerX[i] = location.parallaxLayers[i].position.x ;
		}
	}
	public void setCoordinates(){
		Debug.Log("Entered: setCoordinates function");
		for(int i = 0; i < layerX.Length; i++){
			location.parallaxLayers[i].position = new Vector3(layerX[i], location.parallaxLayers[i].position.y, location.parallaxLayers[i].position.z);
		}
	}
}
