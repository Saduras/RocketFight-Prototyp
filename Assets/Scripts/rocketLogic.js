#pragma strict
var timer : float = 0;
static var rocketSpeed = 2;

function Start () {
	print("Awake");
	timer = Time.time;
	this.networkView.RPC("SetVelocity",RPCMode.Others, Vector3(rocketSpeed,0,0));
	this.SetVelocity(Vector3(rocketSpeed,0,0));
}

function Update () {
	if(Time.time - timer > 3) {
		Network.Destroy(this.gameObject);
	}
}

function OnCollisionEnter(collsion : Collision) {
	if(Network.isServer) {
		print("BOOM!");
		Network.Destroy(this.gameObject);
	}
}

@RPC
function SetVelocity( vec : Vector3 ) {
	print("SetVelocity " + vec.x);
	this.rigidbody.velocity = this.transform.TransformDirection( vec );
}