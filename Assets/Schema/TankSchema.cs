// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class TankSchema : Schema {
	[Type(0, "string")]
	public string name = "";

	[Type(1, "number")]
	public float level = 0;

	[Type(2, "number")]
	public float health = 0;

	[Type(3, "number")]
	public float aim = 0;

	[Type(4, "number")]
	public float power = 0;

	[Type(5, "ref", typeof(Vec2))]
	public Vec2 position = new Vec2();

	[Type(6, "ref", typeof(Vec4))]
	public Vec4 rotaion = new Vec4();

	[Type(7, "array", typeof(ArraySchema<ItemSchema>))]
	public ArraySchema<ItemSchema> items = new ArraySchema<ItemSchema>();
}

