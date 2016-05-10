using UnityEngine;
using System.Collections;

public class ObjectDataReference : MonoBehaviour {

	public Vector3 coordinates;
	public Vector3 scale;
	public string Name;
	public bool destroyed;
	public string parentName;
	void Awake () {
		this.Name = this.name;
	}

	public void Init(Vector3 coordinates, bool destroyed){
		this.coordinates = coordinates;
		this.destroyed = destroyed;
	}
	public void Init(Vector3 coordinates, bool destroyed, string parentName){
		this.coordinates = coordinates;
		this.destroyed = destroyed;
		this.parentName = parentName;
	}

	public void Init(Vector3 coordinates, string Name, bool destroyed, string parentName){
		this.coordinates = coordinates;
		this.Name = Name;
		this.destroyed = destroyed;
		this.parentName = parentName;
	}
}
