using UnityEngine;
using System.Collections;

public class ObjectDataReference : MonoBehaviour {

	public Vector3 coordinates;
	public string Name;
	public bool destroyed;
	void Awake () {
		this.Name = this.name;
	}
}
