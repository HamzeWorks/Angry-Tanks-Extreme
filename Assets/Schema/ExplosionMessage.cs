// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class ExplosionMessage : Schema {
	[Type(0, "ref", typeof(ItemSchema))]
	public ItemSchema item = new ItemSchema();

	[Type(1, "ref", typeof(Vec2))]
	public Vec2 point = new Vec2();
}

