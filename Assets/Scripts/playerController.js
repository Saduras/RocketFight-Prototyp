#pragma strict
/**
 * Prototype Script, dass simple Bewegungen des Characters via Gamepad ermöglicht.
 * Translation relativ zur Welt und Rotation in Controllerstick Richtung.
 * @author: David Speck
 */

var speed = 0.1;
var controlable = true;
var projectile : Rigidbody;
var lastRocketStart : float = 0;
var rocketCD = 3;

var owner : NetworkPlayer;

function Awake () {
	if(Network.isClient)
		enabled = false;
}

/**
 * Jeden Frame wird die aktuelle Werte der Controller-Sticks ausgelesen.
 * Der Linke Stick wird als Translation (relativ zur Welt) angewendet.
 * Der Recht Stick wird als Blickrichtung verwendet.
 */
function Update () {
	//print("Test: " + controlable + "&&" + Network.isClient + "&&" + (owner != null) +"&&"+ (owner == Network.player));
	if(controlable && Network.isClient && (owner != null) && (owner == Network.player)) {
		var LSx = Input.GetAxis("P1_LS_X");
		var LSy = Input.GetAxis("P1_LS_Y");
		var move : Vector3 = new Vector3(Mathf.Pow(LSx,3),0,Mathf.Pow(LSy,3)) * this.speed;
			
		var RSx = Input.GetAxis("P1_RS_X");
		var RSy = Input.GetAxis("P1_RS_Y");
		var rotation = new Vector3(RSx,0,RSy);
		
		var fire = Input.GetAxis("Fire1");
		
		// RT ist komplett runter gedrückt!
		if(fire == -1) {
			networkView.RPC("ShootRocket",RPCMode.Server);
		}
		
		// Translation & Rotation an Server senden
		this.networkView.RPC("Move",RPCMode.Others, move);
		this.networkView.RPC("Rotate",RPCMode.Others, rotation);
		// Bewegungsvorhersage
		this.Move(move);
		this.Rotate(rotation);
	}
}

@RPC
function setOwner(player : NetworkPlayer) {
	Debug.Log("Setting the owner.");
    owner = player;
    if(player == Network.player){
        //So it just so happens that WE are the player in question,
        //which means we can enable this control again
        enabled=true;
    }
}

@RPC
function Move(vec : Vector3) {
	this.transform.Translate(vec, Space.World);
}

@RPC
function Rotate(rotation : Vector3) {
	if(!rotation.Equals(Vector3.zero))
		this.transform.rotation = Quaternion.LookRotation(rotation);
}

@RPC
function ShootRocket() {
	var rocketStart = Time.time;
	if(rocketStart - lastRocketStart > rocketCD) {
		lastRocketStart = Time.time;
		var clone = Network.Instantiate(projectile, transform.FindChild("Barrel").transform.position, transform.rotation, 0);
	}
}