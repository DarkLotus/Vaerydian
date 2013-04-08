{
  "action_types": {
    "DAMAGE" : 0,
    "MODIFY" : 1,
    "CREATE" : 2,
    "DESTROY" : 3,
  },
  "impacts_type" : {
    "NONE": 0,
    "DIRECT" : 1,
    "AREA" : 2,
    "CONE" : 3,
    "OVERTIME" : 4
  },
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
    "MENTAL": 13
  },
  "modify_types" : {
    "NONE": 0,
    "SKILL" : 1,
    "ABILITY" : 2,
    "MECHANIC" : 3,
    "ATTRIBUTE" : 4,
    "KNOWLEDGE" : 5
  },
  "modify_duration" : {
    "NONE" : 0,
    "TEMPORARY" : 1,
    "PERMANENT" : 2,
    "LOCATION" : 3,
  }
  "create_types" : {
    "NONE" : 0,
    "OBJECT" : 1,
    "CHARACTER" : 2,
    "ITEM" : 3,
    "FEATURE" : 4
  },
  "destroy_types" : {
    "NONE" : 0,
    "OBJECT" : 1,
    "CHARACTER" : 2,
    "ITEM" : 3,
    "FEATURE" : 4
  },
  "action_defs":[
    {"name":"NONE","id":0, "action_type:":"", "impact_type":"", "damage_type":"", "modify_type":"", "modify_duration": "", "creation_type":"", "destroy_type":""},
    {"name":"PLAYER_MELEE","id":0, "action_type:":"DAMAGE", "impact_type":"CONE", "damage_type":"SLASHING", "modify_type":"NONE", "modify_duration": "NONE", "creation_type":"NONE", "destroy_type":"NONE"},
    {"name":"PLAYER_RANGED","id":0, "action_type:":"DAMAGE", "impact_type":"DIRECT", "damage_type":"", "modify_type":"", "modify_duration": "", "creation_type":"", "destroy_type":""},
    {"name":"BAT_SONIC","id":0, "action_type:":"DAMAGE", "impact_type":"DIRECT", "damage_type":"", "modify_type":"", "modify_duration": "", "creation_type":"", "destroy_type":""},
  ]
}
