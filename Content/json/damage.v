{
  "damage_types" : {
    "NONE": 0,
    "SLASHING":1,
    "CRUSHING":2,
    "PIERCING":3,
    "ICE": 4,
    "FIRE": 5,
    "EARTH": 6,
    "WIND": 7,
    "WATER": 8,
    "LIGHT": 9,
    "DARK": 10,
    "CHAOS": 11,
    "ORDER": 12,
    "POISON": 13,
    "DISEASE": 14,
    "ARCANE": 15,
    "MENTAL": 16,
    "SONIC" : 17,
	"ACID" : 18
  }, 
  "damage_defs":[
    {
		"name":"NONE",
		"damage_type": "NONE"
		"damage_basis":"NONE",
		"min":0,
		"max":0,
		"skill_name":"NONE",
		"stat_type":"NONE"
    },
	{
		"name":"TEST_DMG",
		"damage_type":"ACID",
		"damage_basis":"STATIC",
		"min":5,
		"max":10,
		"skill_name":"NONE",
		"stat_type":"NONE"		
	},
	{
		"name":"RANGED_DMG",
		"damage_type":"PIERCING",
		"damage_basis":"WEAPON",
		"min":0,
		"max":0,
		"skill_name":"RANGED",
		"stat_type":"FOCUS"		
	}

  ]
}
