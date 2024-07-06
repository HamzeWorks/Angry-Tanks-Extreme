// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class PlayerSchema : Schema {
	[Type(0, "string")]
	public string id = "";

	[Type(1, "string")]
	public string username = "";

	[Type(2, "boolean")]
	public bool connected = false;

	[Type(3, "boolean")]
	public bool ready = false;

	[Type(4, "boolean")]
	public bool active = false;

	[Type(5, "ref", typeof(TankSchema))]
	public TankSchema tank = new TankSchema();
}

