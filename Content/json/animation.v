{
	"character_defs":[
		{
			"name": "BAT",
			"skeletons": [
				"BAT_NORMAL"
			],
			"current_skeleton":"NORMAL",
			"current_animation":"FLY"
		},
		{
			"name" : "PLAYER",
			"skeletons":[
				"PLAYER_FRONT"
			],
			"current_skeleton":"FRONT"
			"current_animation":"IDLE"
		}
	],
	"skeleton_defs":[
		{
			"name": "BAT_NORMAL",
			"bones":[
				{
					"name": "BAT_HEAD_NORMAL",
					"texture":"characters\\bat_head",
					"origin_x":12,
					"origin_y":12,
					"rotation":0.0,
					"rotation_x":4,
					"rotation_y":4,
					"time":500,
					"animations":[
						{
							"name": "FLY",
							"animation_def":"BAT_HEAD_NORMAL_FLY"
						}
					]
				},
				{
					"name":"BAT_LWING_NORMAL",
					"texture":"characters\\bat_wing",
					"origin_x":4,
					"origin_y":12,
					"rotation":0.0,
					"rotation_x":8,
					"rotation_y":4,
					"time":500,
					"animations":[
						{
							"name": "FLY",
							"animation_def":"BAT_LWING_NORMAL_FLY"
						}
					]
				},
				{
					"name":"BAT_RWING_NORMAL",
					"texture":"characters\\bat_wing",
					"origin_x":20,
					"origin_y":12,
					"rotation":0.0,
					"rotation_x":0,
					"rotation_y":4,
					"time":500,
					"animations":[
						{
							"name": "FLY",
							"animation_def":"BAT_RWING_NORMAL_FLY"
						}
					]
				}
			] 
		},
		{
			"name":"PLAYER_FRONT",
			"bones":[
				{
					"name": "PLAYER_HEAD_FRONT",
					"texture":"characters\\face",
					"origin_x":7,
					"origin_y":0,
					"rotation":0.0,
					"rotation_x":0,
					"rotation_y":0,
					"time":500,
					"animations":[
						{
							"name": "IDLE",
							"animation_def":"PLAYER_HEAD_FRONT_IDLE"
						},
						{
							"name": "MOVING",
							"animation_def":"PLAYER_HEAD_FRONT_IDLE"
						}
					]
				}
			]
		}
	],
	"animation_defs":[
		{
			"name": "BAT_HEAD_NORMAL_FLY"
			"key_frames":[
				{"percent":0.0, "x": 0,"y":0,"rotation":0.0},
				{"percent":1.0, "x": 0,"y":0,"rotation":0.0}
			]
		},
		{
			"name": "BAT_LWING_NORMAL_FLY"
			"key_frames":[
				{"percent":0.0, "x": 0,"y":0,"rotation":0.0},
				{"percent":0.33, "x": 0,"y":0,"rotation":-0.5},
				{"percent":0.66, "x": 0,"y":0,"rotation":0.5},
				{"percent":1.0, "x": 0,"y":0,"rotation":0.0}
			]
		},
		{
			"name": "BAT_RWING_NORMAL_FLY"
			"key_frames":[
				{"percent":0.0, "x": 0,"y":0,"rotation":0.0},
				{"percent":0.33, "x": 0,"y":0,"rotation":0.5},
				{"percent":0.66, "x": 0,"y":0,"rotation":-0.5},
				{"percent":1.0, "x": 0,"y":0,"rotation":0.0}
			]

		},
		{
			"name":"PLAYER_HEAD_FRONT_IDLE",
			"key_frames":[
				{"percent":0.0,"x":0,"y":0,"rotation":0.0},
				{"percent":0.4,"x":0,"y":1,"rotation":0.0},
				{"percent":0.5,"x":0,"y":2,"rotation":0.0},
				{"percent":1.0,"x":0,"y":0,"rotation":0.0}
			]
		}
	]
}
