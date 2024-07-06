// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class MovementMessage : Schema {
	[Type(0, "number")]
	public float aim = 0;

	[Type(1, "number")]
	public float power = 0;

	[Type(2, "ref", typeof(Vec2))]
	public Vec2 tankPosition = new Vec2();

	[Type(3, "ref", typeof(Vec4))]
	public Vec4 tankRotaion = new Vec4();
}

