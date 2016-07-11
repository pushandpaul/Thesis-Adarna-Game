﻿using UnityEngine;
using System.Collections;

public class ItemToGive : MonoBehaviour {

	public void setItem(Sprite item){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		if(transform.root.GetComponent<PlayerController>() != null){
			gameManager.currentHeldItem = item;
		}
		SpriteRenderer myRenderer = this.GetComponent<SpriteRenderer>();
		myRenderer.enabled = true;
		myRenderer.sprite = item;
		Debug.Log("This is my item: " + myRenderer.sprite.name);
	}

	public void disableItem(){
		GameManager gameManager = FindObjectOfType<GameManager> ();
		gameManager.currentHeldItem = null;
		Debug.Log ("Not holding any item now.");
		SpriteRenderer myRenderer = this.GetComponent<SpriteRenderer>();
		myRenderer.enabled = false;
		//myRenderer.sprite = item;
	}

	public void clearItem(){
		SpriteRenderer myRender = this.GetComponent<SpriteRenderer> ();	
		myRender.sprite = null;
	}

	public Sprite getItem(){
		return this.GetComponent<SpriteRenderer>().sprite;
	}
}
