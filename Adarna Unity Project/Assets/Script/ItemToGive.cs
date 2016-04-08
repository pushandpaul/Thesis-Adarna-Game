using UnityEngine;
using System.Collections;

public class ItemToGive : MonoBehaviour {

	private SpriteRenderer myRenderer;

	void Start(){
		myRenderer = this.GetComponent<SpriteRenderer>();
	}

	public void setItem(Sprite item){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		gameManager.currentHeldItem = item;
		myRenderer.enabled = true;
		myRenderer.sprite = item;
	}

	public void disableItem(){
		myRenderer.enabled = false;
		//myRenderer.sprite = item;
	}
}
