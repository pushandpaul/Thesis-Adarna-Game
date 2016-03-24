using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bar : MonoBehaviour {

	public float lowPercent = 25f;
	public float medPercent = 75f;

	public Color lowColor;
	public Color medColor;
	public Color highColor;

	private Color targetColor;

	private Image image;
	public Image stateImage;

	public Sprite lowState;
	public Sprite medState;
	public Sprite highState;

	// Use this for initialization
	void Start () {
		image = this.GetComponent<Image>();
		lowPercent *= .01f;
		medPercent *= .01f;
	}
	
	// Update is called once per frame
	void Update () {
		if(image.fillAmount <= lowPercent){
			targetColor = lowColor;
			this.changeState(lowState);
		}
			
		else if(image.fillAmount <= medPercent){
			this.changeState(medState);
			targetColor = medColor;
		}
		else if(image.fillAmount > medPercent){
			this.changeState(highState);
			targetColor = highColor;
		}
		if(image.color != targetColor)
			image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * 15f);
	}

	public void changeColor(Color color){
		image.color = color;
	}

	/*IEnumerator changeColor(Color targetColor, float duration){
		float time = 0f;
		Color originColor = image.color;

		while(time < duration){
			time += Time.deltaTime;
			image.color = Color.Lerp(originColor, targetColor, time/duration);

			//simage.color = targetColor;
			yield return null;
		}
	}*/

	public void changeState(Sprite currentState){
		stateImage.sprite = currentState;
	}
		
}
