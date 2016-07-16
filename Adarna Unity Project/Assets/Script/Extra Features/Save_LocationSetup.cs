using System;

[Serializable]
public class Save_LocationSetup{
	public string sceneToLoad;
	public bool isRight;
	public bool isDoor;
	public int doorIndex;

	public Save_LocationSetup(string sceneToLoad, bool isRight, bool isDoor, int doorIndex){
		this.sceneToLoad = sceneToLoad;
		this.isRight = isRight;
		this.isDoor = isDoor;
		this.doorIndex = doorIndex;
	}
}
