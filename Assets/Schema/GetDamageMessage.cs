// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 0.5.41
// 

using Colyseus.Schema;

public class GetDamageMessage : Schema {
	[Type(0, "number")]
	public float damage = 0;

	[Type(1, "number")]
	public float currentHealth = 0;
}

