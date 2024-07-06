// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class SetupMessage : Schema {
	[Type(0, "string")]
	public string tankName = "";

	[Type(1, "ref", typeof(TankSchema))]
	public TankSchema tank = new TankSchema();
}

