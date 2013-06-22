{
    "_comment" : "Terrain Types are static, they are here to show options availablewwww",
    "terrain_types" : {
        "NOTHING" : 0,
        "BOUNDARY" : 1,
        "FLOOR" : 2,
        "WALL" : 3,
        "DECORATION" : 4,
        "TRANSITION" : 5,
        "TRIGGER" : 6,
        
    },
    "terrain_defs" : [
        {
            "name" : "NOTHING",
            "id" : 0,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "NOTHING"
        },
        {
            "name" : "BASE_LAND",
            "id" : 1,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "BASE_OCEAN",
            "id" : 2,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "BASE_MOUNTAIN",
            "id" : 3,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "BASE_RIVER",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "OCEAN_ICE",
            "id" : 100,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                204,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "OCEAN_LITTORAL",
            "id" : 101,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                51,
                153,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "OCEAN_SUBLITTORAL",
            "id" : 102,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                0,
                102,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "OCEAN_ABYSSAL",
            "id" : 103,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                0,
                51,
                204
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "OCEAN_COAST",
            "id" : 104,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                153
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_SCORCED",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                153,
                102,
                51
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_DESERT",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                204,
                204,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_SAVANA",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                204,
                255,
                102
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TEMPERATE_FOREST",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                0,
                153,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TROPICAL_RAIN_FOREST",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                0,
                128,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_SWAMP",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                0,
                51,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TAIGA",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                24,
                72,
                48
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TUNDRA",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                53,
                111,
                53
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TEMPERATE_GRASSLAND",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                153,
                255,
                102
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_SHRUBLAND",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                102,
                153,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TEMPERATE_RAUN_FOREST",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_TROPICAL_FOREST",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                102,
                255,
                51
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_SNOW_PLAINS",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_MARHS",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                33,
                101,
                67
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_BOG",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                51,
                51,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_ARTIC_DESERT",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                204,
                204,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_GLACIER",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                153,
                255,
                204
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "LAND_HYBOREAN_RIMELAND",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                204,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAUN_FOOTHIL",
            "id" : 300,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                57,
                69,
                43
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_LOWLAND",
            "id" : 301,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                79,
                95,
                59
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_HIGHLAND",
            "id" : 302,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                115,
                123,
                105
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_CASCADE",
            "id" : 303,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                150,
                150,
                150
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_DRY_PEAK",
            "id" : 304,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                192,
                192,
                192
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_SNOWY_PEAK",
            "id" : 305,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                221,
                221,
                221
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "MOUNTAIN_VOLCANO",
            "id" : 306,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                255,
                255,
                255
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "CAVE_WALL",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                120,
                54,
                29
            ],
            "passible" : false,
            "effect" : "NONE",
            "type" : "WALL"
        },
        {
            "name" : "CAVE_FLOOR",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                211,
                158,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "DUNGEON_WALL",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                180,
                180,
                180
            ],
            "passible" : false,
            "effect" : "NONE",
            "type" : "WALL"
        },
        {
            "name" : "DUNGEON_FLOOR",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                211,
                158,
                78
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "DUNGEON_DOOR",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                150,
                75,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
        {
            "name" : "DUNGEON_BEDROCK",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                64,
                64,
                64
            ],
            "passible" : false,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
		        {
            "name" : "DUNGEON_EARTH",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                100,
                100,
                100
            ],
            "passible" : false,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
		    {
            "name" : "DUNGEON_CORRIDOR",
            "id" : 4,
            "texture" : "terrain\\default",
            "texture_offset" : [
                0,
                0
            ],
            "color" : [
                153,
                153,
                0
            ],
            "passible" : true,
            "effect" : "NONE",
            "type" : "FLOOR"
        },
    ]
}
