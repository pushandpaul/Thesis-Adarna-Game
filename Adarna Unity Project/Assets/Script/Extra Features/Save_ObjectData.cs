using System;
using System.Collections;

[Serializable]
public class Save_ObjectData{

	public string Name;
	public float positionX;
	public float positionY;
	public float positionZ;
	public string parentName;
	public bool isDestroyed;

	public Save_ObjectData(string Name, float positionX, float positionY, float positionZ, string parentName, bool isDestroyed){
		this.Name = Name;
		this.positionX = positionX;
		this.positionY = positionY;
		this.positionZ = positionZ;
		this.parentName = parentName;
		this.isDestroyed = isDestroyed;
	}
}
