﻿using UnityEngine;
using System.Collections;

public class ScreenFaderv2 : MonoBehaviour {
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDirection = -1;

	void OnGUI(){

		//Debug.Log("Fading");
		alpha += fadeDirection * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}
		
	public float BeginFade(int direction){
		Debug.Log("Begin Fade");
		this.fadeDirection = direction;
		//drawGUI();
		return(fadeSpeed);
	}

	void OnLevelWasLoaded(){
		this.BeginFade(-1);
	}

}
