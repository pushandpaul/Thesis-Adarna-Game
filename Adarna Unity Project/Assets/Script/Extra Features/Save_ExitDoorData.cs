using System;
using System.Collections.Generic;

[Serializable]
public class Save_ExitDoorData{
	public string sceneName;
	public List<Save_Exit> exits;

	public Save_ExitDoorData(string sceneName, List<Save_Exit> exits){
		this.sceneName = sceneName;
		this.exits = exits;
	}
}
