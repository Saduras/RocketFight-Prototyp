using UnityEngine;
using System.Collections;

public class Client_Rocket : MonoBehaviour {

	[RPC]
	public void SetColor(Vector3 colorVec) {
		Color col = new Color(colorVec.x, colorVec.y, colorVec.z, 1);
		this.renderer.material.SetColor("_Color",col);
	}
}
