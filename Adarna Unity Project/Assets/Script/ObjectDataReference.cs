using UnityEngine;
using System.Collections;

public class ObjectDataReference : MonoBehaviour {

	public Vector3 coordinates;
	public string Name;
	public bool destroyed;
	public string parentName;
	void Awake () {
		this.Name = this.name;
	}
}
