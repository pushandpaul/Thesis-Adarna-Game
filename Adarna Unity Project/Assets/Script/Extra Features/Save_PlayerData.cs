using System;

[Serializable]
public class Save_PlayerData{
	public string Name;
	public int stateHashID;
	public string heldItemName;
	public float positionX;
	public float positionY;
	public float positionZ;

	public Save_PlayerData(string Name, int stateHashID, string heldItemName){
		this.Name = Name;
		this.stateHashID = stateHashID;
		this.heldItemName = heldItemName;
	}
}
