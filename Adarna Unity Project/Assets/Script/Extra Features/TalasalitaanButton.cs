using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TalasalitaanButton : MonoBehaviour {

	public Text salitaUI;
	public Text kasingKahuluganUI;
	public Button button;
	private Transform halimbawaUI;
	private TalasalitaanManager talasalitaanManager;

	public string halimbawa;
	public int clicks = 0;

	void Awake(){
		salitaUI.text = "????";
		kasingKahuluganUI.text = "????";
		talasalitaanManager = FindObjectOfType<TalasalitaanManager>();
		halimbawaUI = talasalitaanManager.halimbawaUI;
	}

	public void SetTexts(string salita, string kasingKahulugan, string halimbawa){
		salitaUI.text = salita;
		kasingKahuluganUI.text = kasingKahulugan;
		this.halimbawa = halimbawa;
	}

	public void ShowExample(){

		foreach(TalasalitaanButton talasalitaanButton in talasalitaanManager.talasalitaanBtns){
			if(talasalitaanButton != this){
				talasalitaanButton.clicks = 0;
			}
		}

		if(clicks == 0){
			if(!halimbawaUI.gameObject.activeSelf)
				halimbawaUI.gameObject.SetActive(true);

			if(this.transform.GetSiblingIndex() < halimbawaUI.GetSiblingIndex()){
				halimbawaUI.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
			}
			else
				halimbawaUI.SetSiblingIndex(this.transform.GetSiblingIndex());

			talasalitaanManager.halimbawaUIText.text = halimbawa;
		}

		else if (clicks == 1){
			halimbawaUI.gameObject.SetActive(false);
			clicks = 0;
		}

		clicks++;

	}

	public void playAudio(){
		Debug.Log("Play Audio: Work in progress");
	}
}
