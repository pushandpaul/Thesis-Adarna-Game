using System;
using System.Collections.Generic;

[Serializable]
public class Save_LocationSetup{
	public string sceneToLoad;
	public char timeOfDay;
	public bool isRight;
	public bool isDoor;
	public int doorIndex;
	public List<Save_ExitDoorData> exitDoorData;


	public Save_LocationSetup(string sceneToLoad, char timeOfDay,bool isRight, bool isDoor, int doorIndex, List <Save_ExitDoorData> exitDoorData){
		this.sceneToLoad = sceneToLoad;
		this.timeOfDay = timeOfDay;
		this.isRight = isRight;
		this.isDoor = isDoor;
		this.doorIndex = doorIndex;
		this.exitDoorData = exitDoorData;
	}
}
