// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class BattleState : Schema {
	[Type(0, "map", typeof(MapSchema<PlayerSchema>))]
	public MapSchema<PlayerSchema> players = new MapSchema<PlayerSchema>();

	[Type(1, "string")]
	public string activePlayerId = "";

	[Type(2, "number")]
	public float seed = 0;

	[Type(3, "number")]
	public float roundIndex = 0;

	[Type(4, "number")]
	public float roundDuration = 0;

	[Type(5, "ref", typeof(ShootMessage))]
	public ShootMessage lastShoot = new ShootMessage();

	[Type(6, "boolean")]
	public bool ready = false;

	[Type(7, "boolean")]
	public bool started = false;

	[Type(8, "boolean")]
	public bool finished = false;

	[Type(9, "string")]
	public string winnderPlayerId = "";
}

