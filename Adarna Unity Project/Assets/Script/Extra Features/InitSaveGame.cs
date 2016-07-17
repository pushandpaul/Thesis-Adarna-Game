using System;

[Serializable]
public class InitSaveGame : SaveGame {
	public int latestPartIndex;
	public bool watchedIntro;
	public bool prevAssessmentDone;

	public InitSaveGame(int latesPartIndex, bool watchedIntro, bool prevAssessmentDone){
		this.latestPartIndex = latesPartIndex;
		this.watchedIntro = watchedIntro;
		this.prevAssessmentDone = prevAssessmentDone;
	}
}
