using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LocationMarker : MonoBehaviour {

	public UIFader myUIFader;
	private string currentSection;

	public Text sectionUIText;
	public Text sectionUIShadow;

	void Awake(){
		//myUIFader = GetComponent<UIFader>();
		myUIFader.setCanvasGroup(this.GetComponent<CanvasGroup>());
	}

	void Start(){
		
	}

	void OnLevelWasLoaded(){
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		string section = "";
		string sceneName = "";
		myUIFader.canvasGroup.alpha = 0f;
		if(levelManager != null && this.gameObject.activeInHierarchy){
			sceneName = levelManager.sceneName.ToLower();
			if(sceneName.Contains("armenya")){
				section = "Armenya";	
			}
			else if(sceneName.Contains("reyno")){
				if(!sceneName.Contains("bahay ni ermitanyo") && !sceneName.Contains("generic forest")){
					section = "Reino de los Crystales";
				}
			}
			else if(sceneName.Contains("mountain")){
				section = "Bundok Tabor";
			}
			else{

				if(sceneName.Contains("castle") || sceneName.Contains("kwarto")){
					section = "Kaharian ng Berbanya";
				}

				else if(sceneName.Contains("forest")){
					if(sceneName.Contains("pedras platas")){
						section = "Piedras Platas";
					}
					else
						section = "Kagubatan";
				}

				else if(sceneName.Contains("bahay ni ermitanyo")){
					section = "Kagubatan";
				}

				else{
					section = "Berbanya";
				} 	
			}
				
			if(currentSection != section && section != ""){
				StopAllCoroutines();
				myUIFader.StopAllCoroutines();
				myUIFader.canvasGroup.alpha = 0f;
				StartCoroutine(show(section));
			}
			currentSection = section;
		}
		else{
			Debug.Log ("Level Manager is null");
		}
	}

	IEnumerator show(string section){
		sectionUIText.text = section;
		sectionUIShadow.text = section;

		while(FindObjectOfType<ScreenFader>().alpha > 0f){
			yield return null;
		}
			
		Debug.Log("trying to show");

		myUIFader.FadeIn(5, 1f, true);

	}


}
