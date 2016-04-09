using UnityEngine;
using System.Collections;

public class ItemToGive : MonoBehaviour {

	//public SpriteRenderer myRenderer;

	public void setItem(Sprite item){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		gameManager.currentHeldItem = item;
		SpriteRenderer myRenderer = this.GetComponent<SpriteRenderer>();
		myRenderer.enabled = true;
		myRenderer.sprite = item;
	}

	public void disableItem(){
		SpriteRenderer myRenderer = this.GetComponent<SpriteRenderer>();
		myRenderer.enabled = false;
		//myRenderer.sprite = item;
	}
}
