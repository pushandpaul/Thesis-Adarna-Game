using UnityEngine;
using System.Collections;

public class SpriteController : MonoBehaviour {


	public void punchColors(GameObject spriteSource, Color color){
		SpriteRenderer mySpriteRenderer = spriteSource.GetComponent<SpriteRenderer>();
		SpriteRenderer[] mySpriteRenderers = spriteSource.GetComponentsInChildren<SpriteRenderer>(true);
		if(mySpriteRenderer != null){
			punchColor(mySpriteRenderer, color);
		}
		foreach(SpriteRenderer myRenderer in mySpriteRenderers){
			punchColor(myRenderer, color);
		}
	}

	public void punchColor(SpriteRenderer mySprite, Color color){
		mySprite.color = color;
	}

	public void changeColors(GameObject spriteSource, Color color, float duration){
		SpriteRenderer mySpriteRenderer = spriteSource.GetComponent<SpriteRenderer>();
		SpriteRenderer[] mySpriteRenderers = spriteSource.GetComponentsInChildren<SpriteRenderer>(true);
		if( mySpriteRenderer != null){
			changeColor(mySpriteRenderer, color, duration);
		}
		foreach(SpriteRenderer myRenderer in mySpriteRenderers){
			changeColor(myRenderer, color, duration);
		}
	}

	public void changeColor(SpriteRenderer mySprite, Color color, float duration){
		color = new Color(color.r, color.g, color.b, color.a = 255f);
		StartCoroutine(startChangeColor(mySprite, mySprite.color, color, duration));
	}

	IEnumerator startChangeColor(SpriteRenderer mySprite, Color currentColor, Color targetColor, float duration){
		float startTime = Time.time;
		float endTime = startTime + duration;

		while(Time.time <= endTime){
			float t = (Time.time - startTime)/duration;
			mySprite.color = Color.Lerp(currentColor,  targetColor, t);
			yield return null;
		}
	}
}
