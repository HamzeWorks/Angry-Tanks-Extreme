// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class PickupItemMessage : Schema {
	[Type(0, "string")]
	public string username = "";

	[Type(1, "ref", typeof(ItemSchema))]
	public ItemSchema item = new ItemSchema();
}

