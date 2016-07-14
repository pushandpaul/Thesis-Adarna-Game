using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalasalitaanButton : MonoBehaviour {

	public Text salitaUI;
	public Text kasingKahuluganUI;
	public Button button;
	public Button audioButton;
	private Transform halimbawaUI;
	private TalasalitaanManager talasalitaanManager;
	private Talasalitaan talasalitaaMatch;

	public string halimbawa;
	public bool inHalimbawa = false;

	public bool newlyActivated;
	public GameObject notif;

	void Awake(){
		salitaUI.text = "????";
		kasingKahuluganUI.text = "????";
		talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
		audioButton.interactable = false;
		halimbawaUI = talasalitaanManager.halimbawaUI;
	}

	public void Init(Talasalitaan talasalitaan){
		talasalitaaMatch = talasalitaan;
		salitaUI.text = talasalitaan.salita;
		kasingKahuluganUI.text = talasalitaan.kasingKahulugan;
		halimbawa = talasalitaan.halimbawa;
		newlyActivated = talasalitaan.newlyActivated;
		audioButton.interactable = true;

		notif.SetActive(newlyActivated);
	}

	public void ShowExample(){
		talasalitaaMatch.newlyActivated = false;
		notif.SetActive(false);

		foreach(TalasalitaanButton talasalitaanButton in talasalitaanManager.talasalitaanBtns){
			if(talasalitaanButton != this){
				talasalitaanButton.inHalimbawa = false;
			}
		}

		if(!inHalimbawa){
			if(!halimbawaUI.gameObject.activeSelf)
				halimbawaUI.gameObject.SetActive(true);

			if(this.transform.GetSiblingIndex() < halimbawaUI.GetSiblingIndex()){
				halimbawaUI.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
			}
			else
				halimbawaUI.SetSiblingIndex(this.transform.GetSiblingIndex());

			talasalitaanManager.halimbawaUIText.text = halimbawa;
			inHalimbawa = true;
		}

		else if (inHalimbawa){
			halimbawaUI.gameObject.SetActive(false);
			inHalimbawa = false;
		}
	}

	public void playAudio(){
		Debug.Log("Play Audio: Work in progress");
	}
}
