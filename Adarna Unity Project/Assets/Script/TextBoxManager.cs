using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text theText;

	public void setText(string text){
		this.theText.text = text;
	}

	public void enableTextBox(){
		this.textBox.SetActive(true);
	}

	public void disableTextBox(){
		this.textBox.SetActive(false);
	}
}
