using System;

[Serializable]
public class Save_CharData{
	public string Name;
	public int stateHashID;
	public string heldSpriteName;

	public Save_CharData(string Name, int stateHashID, string heldSpriteName){
		this.Name = Name;
		this.stateHashID = stateHashID;
		this.heldSpriteName = heldSpriteName;
	}
}
