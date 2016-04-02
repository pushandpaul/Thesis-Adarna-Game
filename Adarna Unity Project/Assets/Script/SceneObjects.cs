using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneObjects : MonoBehaviour {

	public string Name;
	public List <ObjectDataReference> sceneObjectData;

	void Awake(){
		sceneObjectData = new List<ObjectDataReference>();	
	}
}
