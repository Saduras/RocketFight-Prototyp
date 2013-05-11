
// Sea Green For the Win!
private var sceneViewDisplayColor = Color (0.9, 0.0, 0.0, 0.50);

// Whoever enters Deathbox recives a message "OnDeath".
// They don't have to react to it.
function OnTriggerEnter (other : Collider) {
	other.gameObject.SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
	Debug.Log("Dead");
}

function OnDrawGizmos () {
	var bounds = GetComponent(BoxCollider).bounds;
	var transform = GetComponent(Transform);
	//transform.position;
	
	//topFrontRight = transform.TransformPoint(bounds.center + bounds.extents);
	topFrontRight = (bounds.center + bounds.extents);
	topFrontLeft = (bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, 1, 1)));
	topBackRight = (bounds.center + Vector3.Scale(bounds.extents, Vector3(1, 1, -1)));
	topBackLeft = (bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, 1, -1)));
	bottomFrontRight = (bounds.center + Vector3.Scale(bounds.extents, Vector3(1, -1, 1)));
	bottomFrontLeft = (bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, -1, 1)));
	bottomBackRight = (bounds.center + Vector3.Scale(bounds.extents, Vector3(1, -1, -1)));
	bottomBackLeft = (bounds.center + Vector3.Scale(bounds.extents, Vector3(-1, -1, -1)));
	
	Gizmos.color = sceneViewDisplayColor;
	Gizmos.DrawLine(topFrontRight,topFrontLeft);
	Gizmos.DrawLine(topFrontRight,topBackRight);
	Gizmos.DrawLine(topBackLeft,topFrontLeft);
	Gizmos.DrawLine(topBackLeft,topBackRight);
	Gizmos.DrawLine(bottomFrontRight,bottomFrontLeft);
	Gizmos.DrawLine(bottomFrontRight,bottomBackRight);
	Gizmos.DrawLine(bottomBackLeft,bottomFrontLeft);
	Gizmos.DrawLine(bottomBackLeft,bottomBackRight);
	Gizmos.DrawLine(topFrontLeft,bottomFrontLeft);
	Gizmos.DrawLine(topFrontRight,bottomFrontRight);
	Gizmos.DrawLine(topBackLeft,bottomBackLeft);
	Gizmos.DrawLine(topBackRight,bottomBackRight);
	
	Gizmos.DrawIcon(transform.position, "Skull And Crossbones Icon.tif");
}