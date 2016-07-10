using System;
using System.Collections.Generic;

[Serializable]
public class Save_SceneObjects{
	public string sceneName;
	public List<Save_ObjectData> objectsData;

	public Save_SceneObjects(string sceneName, List<Save_ObjectData> objectsData){
		this.sceneName = sceneName;
		this.objectsData = objectsData;
	}
}
