// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class ShootMessage : Schema {
	[Type(0, "string")]
	public string playerId = "";

	[Type(1, "ref", typeof(ItemSchema))]
	public ItemSchema item = new ItemSchema();

	[Type(2, "number")]
	public float aim = 0;

	[Type(3, "number")]
	public float power = 0;

	[Type(4, "ref", typeof(Vec2))]
	public Vec2 firePoint = new Vec2();
}

