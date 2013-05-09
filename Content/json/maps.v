{
  "map_types":{
    "CAVE": 0,
    "DUNGEON": 1,
    "TOWN": 2,
    "CITY": 3,
    "TOWER": 4,
    "OUTPOST": 5,
    "FORT": 6,
    "NEXUS": 7,
    "WORLD": 8,
    "WILDERNESS": 9
  },
	"map_defs":[
		{
			"name":"CAVE_DEFAULT",
			"type":"CAVE",
			"tile_maps":[
				{
					"mapto":"WALL",
					"tiles":[
						{"name":"CAVE_WALL","prob":100}
					]
				},
				{
					"mapto":"FLOOR",
					"tiles":[
						{"name":"CAVE_FLOOR","prob":100}
					]
				}
			]
	 	},
		{
			"name":"DUNGEON_DEFAULT",
			"type":"DUNGEON",
			"tile_maps":[
				{
					"mapto":"WALL",
					"tiles":[
						{"name":"DUNGEON_WALL","prob":100}
					]
				},
				{
					"mapto":"FLOOR",
					"tiles":[
						{"name":"DUNGEON_FLOOR","prob":100}
					]
				},
				{
					"mapto":"DOOR",
					"tiles":[
						{"name":"DUNGEON_DOOR","prob":100}
					]
				}
			]
	 	}
  ],
  "map_params":{
    "WORLD" :{
      "world_params_x" : 0,
      "world_params_y" : 0,
      "world_params_dx" : 854,
      "world_params_dy" : 480,
      "world_params_z" : 5.0,
      "world_params_xsize" : 854,
      "world_params_ysize" : 480,
      "world_params_seed" : null
    },
    "CAVE" :{
      "cave_params_x" : 100,
      "cave_params_y" : 100,
      "cave_params_prob" : 45,
      "cave_params_cell_op_spec" : true,
      "cave_params_iter" : 50000,
      "cave_params_neighbors" : 4,
      "cave_params_seed" : null
    }
  }
}
