using System;


[Serializable]
public class Save_Exit {
	public string Name;
	public bool isOpen = true;
	public bool isDoor;

	public Save_Exit(string Name, bool isOpen, bool isDoor){
		this.Name = Name;
		this.isOpen = isOpen;
		this.isDoor = isDoor;
	}

}
