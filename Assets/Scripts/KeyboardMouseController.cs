using UnityEngine;
using System.Collections;

public class KeyboardMouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 mousePos = Input.mousePosition;
		Vector2 objectPos = Camera.mainCamera.WorldToScreenPoint(this.transform.position);
		
		// vector pointing from object on screen to mouse position on screen
		Vector2 vec = mousePos - objectPos;
		// calculate the smaller angle between vec and the Y-axis in 2D
		float angle = Vector2.Angle(new Vector2(0,1),vec);
		// if vex.x is negativ we invert the angle since we want the bigger one in this case
		if(vec.x < 0)
			angle = 360.0f - angle;
		
		// rotate to calculated angle
		this.transform.rotation = Quaternion.Euler(new Vector3(0,angle,0));
	}
}
