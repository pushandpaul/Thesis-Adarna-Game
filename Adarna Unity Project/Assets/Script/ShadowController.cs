using UnityEngine;
using System.Collections;

public class ShadowController : MonoBehaviour {

	public Transform owner;
	public float shadowMaxDistance = 20f;
	private Vector3 initialPosition;
	private Vector3 calculatedMaxDistance;
	private float newShadowY;
	public float slopeOffset = 0.3f;

	private RaycastHit2D myLineCast;
	private bool hitGround = false;

	void Update(){
		DrawRaycast();
		UpdateY();
	}

	void DrawRaycast(){
		//calculatedMaxDistance = transform.position - (new Vector3(0, shadowMaxDistance, 0));
		calculatedMaxDistance = owner.position - (new Vector3(0, shadowMaxDistance, 0));
		Debug.DrawLine(owner.position, calculatedMaxDistance, Color.green);
		myLineCast = Physics2D.Linecast(owner.position, calculatedMaxDistance, 1 << LayerMask.NameToLayer("Ground"));
		hitGround = myLineCast;
	}

	void UpdateY(){
		newShadowY = myLineCast.point.y;
		//add an offset if there is a slope


		if(hitGround){
			if(myLineCast.collider.tag == ("Ground Slope"))
				newShadowY += slopeOffset;
		}

		//check if raycast hits the ground, if not the position will be at max
		else if(!hitGround)
			newShadowY = calculatedMaxDistance.y;

		transform.position = new Vector3(transform.position.x, newShadowY, 0f);
	}
}
