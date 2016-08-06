using UnityEngine;
using System.Collections;

public class MoveMountain_HideOnExit : MonoBehaviour {

	public Collider2D target;
	public SpriteRenderer[] toHide;
	public bool stopPhysicsOnExit;
	public bool targetRigidInParent;

	void OnTriggerEnter2D(Collider2D other){
		if(other == target){
			foreach(SpriteRenderer _toHide in toHide){
				_toHide.enabled = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if(other == target){
			foreach(SpriteRenderer _toHide in toHide){
				_toHide.enabled = false;
			}

			if(stopPhysicsOnExit){
				if(targetRigidInParent){
					target.GetComponentInParent<Rigidbody2D>().isKinematic = true;
				}
				else{
					target.GetComponent<Rigidbody2D>().isKinematic = true;
				}
			}

		}
	}
}
