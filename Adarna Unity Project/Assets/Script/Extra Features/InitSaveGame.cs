using System;

[Serializable]
public class InitSaveGame : SaveGame {
	public int latestPartIndex;
	public bool watchedIntro;

	public InitSaveGame(int latesPartIndex, bool watchedIntro){
		this.latestPartIndex = latesPartIndex;
		this.watchedIntro = watchedIntro;
	}
}
