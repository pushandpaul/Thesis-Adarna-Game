using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource ambientSource;

	[System.Serializable]
	public class SectionMusic{
		public string sectionName;
		public AudioClip music;
	}
	[System.Serializable]
	public class EnvironmentMusic{
		public string environmentName;
		public AudioClip ambience;
	}

	public List<SectionMusic> sectionMusics;
	public List<EnvironmentMusic> environmentMusics;


	private float origMusicVolume;
	private float origAmbientVolume;

	private bool overrideThis;
	void Awake(){
		origMusicVolume = musicSource.volume;
		origAmbientVolume = ambientSource.volume;
		DontDestroyOnLoad (this);
	}

	void OnLevelWasLoaded(){

		bool ambientFound = false;

		if(overrideThis){
			overrideThis = false;
			return;
		}


		LevelManager levelManager = FindObjectOfType<LevelManager> ();
		if(levelManager != null){
			foreach(SectionMusic sectionMusic in sectionMusics){
				if (determineSection (levelManager.sceneName) == sectionMusic.sectionName){

					if(musicSource.clip != sectionMusic.music){
						musicSource.time = 0f;
						musicSource.clip = sectionMusic.music;
						musicSource.Play ();
					}

					break;
				}
			}
			foreach(EnvironmentMusic environmentMusic in environmentMusics){
				if (determineEnvironment (levelManager.sceneName) == environmentMusic.environmentName) {
					if (ambientSource.clip != environmentMusic.ambience) {

						if (ambientSource.clip != environmentMusic.ambience) {
							ambientSource.time = 0f;
							ambientSource.clip = environmentMusic.ambience;
							ambientSource.Play ();
						}
						ambientFound = true;
						break;
					}
				} else
					ambientSource.clip = null;

			}
		}
	}

	string determineSection(string toDetermine){
		string section = "";
		toDetermine = toDetermine.ToLower ();

		if(toDetermine.Contains("armenya")){
			if(toDetermine.Contains("forest") || toDetermine.Contains("ilalim")){
				section = "armenya forest";
			}
			else
				section = "armenya castle";
		}
		else if(toDetermine.Contains("reyno")){
			if(!toDetermine.Contains("forest"))
				section = "reyno";
		}
		else if(toDetermine.Contains("castle") || toDetermine.Contains("kwarto")){
			section = "berbanya castle";
		}
		else if(toDetermine.Contains("forest")){
			section = "berbanya forest";
		}
		else if(toDetermine.Contains("bahay ni ermitanyo")){
			section = "berbanya forest";
		}
		else {
			section = "berbanya";
		}

		return section;
	}

	string determineEnvironment(string toDetermine){
		string environment = "";
		toDetermine = toDetermine.ToLower ();
		if(toDetermine.Contains("forest")){
			Debug.Log ("this is forest");
			if(FindObjectOfType<GameManager>().timeOfDay == 'n'){
				environment = "forest night";
			}
			else{

				if(toDetermine.Contains("armenya")){
					environment = "armenya forest";
				}
				else
					environment = "forest";

			}
		}
		else if(toDetermine.Contains("ilalim")){
			environment = "ilalim";
		}

		return environment;
	}

	public void Mute(bool isMuted){
		ambientSource.mute = isMuted;
		musicSource.mute = isMuted;
	}

	public void setAmbientVolume(float volume){
		ambientSource.volume = volume;
	}

	public void setMusicVolume(float volume){
		musicSource.volume = volume;
	}

	public void revertOriginalVol(){
		musicSource.volume = origMusicVolume;
		ambientSource.volume = origAmbientVolume;
	}

	public void overridePlay(AudioClip toPlay){
		overrideThis = true;
		musicSource.time = 0;
		musicSource.clip = toPlay;
		musicSource.Play ();
	}
}
