using UnityEngine;
using System.Collections;

public class PunchMoveObject : MonoBehaviour {

	public void punchMove(Vector3 moveTo){
		transform.localPosition = moveTo;
	}
}
