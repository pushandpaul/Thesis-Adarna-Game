using System;
using System.Collections.Generic;

[Serializable]
public class MySaveGame : SaveGame {

	public int partIndex;
	public Save_PlayerData playerData;
	public List<Save_CharData> charData;
	public List<Save_SceneObjects> sceneObjects;
	public List<string> followers;

	public MySaveGame(int partIndex, Save_PlayerData playerData, List<Save_CharData> charData, List<Save_SceneObjects> sceneObjects, List<string> followers){
		this.partIndex = partIndex;
		this.playerData = playerData;
		this.charData = charData;
		this.sceneObjects = sceneObjects;
		this.followers = followers;
	}
}
