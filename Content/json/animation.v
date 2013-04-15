{
	"character_defs":[
		{
			"name": "BAT",
			"skeletons": [
				{
					"name":"NORMAL",
					"skeleton_def":"BAT_NORMAL"
				}
			],
			"current_skeleton":"BAT_NORMAL",
			"current_animation":"FLY"
		},
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

		}
	]
}
