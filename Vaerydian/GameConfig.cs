using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Vaerydian.Utils;
using Vaerydian.Screens;
using Vaerydian.Components.Actions;

namespace Vaerydian
{

	public static class GameConfig
	{
		private static JsonManager g_JM = new JsonManager();

		public static bool loadConfig(){

			if (!GameConfig.loadEffects ())
				return false;

			if (!GameConfig.LoadDamageDefs ())
				return false;

			if (!GameConfig.loadTerrainTypes ())
				return false;

			if (!GameConfig.loadTerrainDefs ())
				return false;

			if (!GameConfig.loadMapTypes ())
				return false;

			if (!GameConfig.loadMapDefs ())
				return false;

			if (!GameConfig.loadStartDefs ())
				return false;

			return true;
		}

		public static Dictionary<string, short> Effects = new Dictionary<string, short>();

		private static bool loadEffects(){

			Effects.Add ("NONE", 0);

			return true;
		}

		public static Dictionary<string, DamageDef> DamageDefs = new Dictionary<string, DamageDef>();

		private static bool LoadDamageDefs(){
			try{
				string json = g_JM.loadJSON("./Content/json/damage.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);
				
				List<Dictionary<string,object>> dDefs = jo ["damage_defs"].asList<Dictionary<string,object>> ();
				
				foreach(Dictionary<string,object> dict in dDefs){
					jo = new JsonObject(dict);
					
					DamageDef dDef = default(DamageDef);
					dDef.Name = jo["name"].asString();
					dDef.ID = jo["id"].asShort();
					dDef.DamageType = jo["damage_type"].asEnum<DamageType>();
					
					DamageDefs.Add(dDef.Name,dDef);
				}
				
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: failed to load damage defs:\n" + e.ToString());
				return false;
			}
			
			return true;
		}


		public static Dictionary<string, ActionDef> ActionDefs = new Dictionary<string, ActionDef>();

		private static bool loadActionDefs(){
			try{
				string json = g_JM.loadJSON("./Content/json/actions.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> aDefs = jo ["action_defs"].asList<Dictionary<string,object>> ();

				foreach(Dictionary<string,object> dict in aDefs){
					jo = new JsonObject(dict);

					ActionDef aDef;
					aDef.Name = jo["name"].asString();
					aDef.ID = jo["id"].asShort();
					aDef.ActionType = jo["action_type"].asEnum<ActionType>();
				}

			}catch(Exception e){
				Console.Error.WriteLine("ERROR: failed to load actions:" + e.ToString());
				return false;
			}


			return true;
		}

		public static Dictionary<string,TerrainType> TerrainTypes = new Dictionary<string, TerrainType> ();

		/// <summary>
		/// Loads the terrain types.
		/// </summary>
		/// <returns><c>true</c>, if terrain types was loaded, <c>false</c> otherwise.</returns>
		private static bool loadTerrainTypes(){
			TerrainTypes.Add ("NOTHING", new TerrainType{Name = "NOTHING", ID = 0});
			TerrainTypes.Add ("BOUNDARY", new TerrainType{Name = "BOUNDARY", ID = 1});
			TerrainTypes.Add ("FLOOR", new TerrainType{Name = "FLOOR", ID = 2});
			TerrainTypes.Add ("WALL", new TerrainType{Name = "WALL", ID = 3});
			TerrainTypes.Add ("DECORATION", new TerrainType{Name = "DECORATION", ID = 4});
			TerrainTypes.Add ("TRANSITION", new TerrainType{Name = "TRANSITION", ID = 5});
			TerrainTypes.Add ("TRIGGER", new TerrainType{Name = "TRIGGER", ID = 6});
			return true;
		}

		public static Dictionary<string,TerrainDef> TerrainDefs = new Dictionary<string, TerrainDef> ();

		private static bool loadTerrainDefs(){
			try{
				string json = g_JM.loadJSON ("./Content/json/terrain.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> tDefs = jo ["terrain_defs"].asList<Dictionary<string,object>> ();

				foreach (Dictionary<string,object> dict in tDefs) {
					jo = new JsonObject(dict);
					TerrainDef tDef;

					tDef.Name = jo["name"].asString();
					tDef.ID = jo["id"].asShort();
					tDef.Texture = jo["texture"].asString();

					List<long> tOff = jo["texture_offset"].asList<long>();
							tDef.TextureOffset = new Point((int)tOff[0], (int)tOff[1]);

					List<long> tColor = jo["color"].asList<long>();
					tDef.Color = new Color(tColor[0],tColor[1],tColor[2]);
					tDef.IsPassible = jo["ispassible"].asBool();

					//TODO: define effect - after effect is defined
					tDef.Effect = Effects[jo["effect"].asString()];

					tDef.Type = TerrainTypes[jo["type"].asString()];

					TerrainDefs.Add(tDef.Name,tDef);
				}
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load terrain:\n" + e.ToString());
				return false;
			}

			return true;
		}

		public static Dictionary<string,MapType> MapTypes = new Dictionary<string, MapType> ();

		private static bool loadMapTypes(){

			MapTypes.Add ("CAVE", new MapType{Name = "CAVE", ID = 0});
			MapTypes.Add ("DUNGEON", new MapType{Name = "DUNGEON", ID = 1});
			MapTypes.Add ("TOWN", new MapType{Name = "TOWN", ID = 2});
			MapTypes.Add ("CTY", new MapType{Name = "CITY", ID = 3});
			MapTypes.Add ("TOWER", new MapType{Name = "TOWER", ID = 4});
			MapTypes.Add ("OUTPOST", new MapType{Name = "OUTPOST", ID = 5});
			MapTypes.Add ("FORT", new MapType{Name = "FORT", ID = 6});
			MapTypes.Add ("NEXUS", new MapType{Name = "NEXUS", ID = 7});
			MapTypes.Add ("WORLD", new MapType{Name = "WORLD", ID = 8});
			MapTypes.Add ("WILDERNESS", new MapType{Name = "WILDERNESS", ID = 9});

			return true;
		}

		private static Dictionary<string,MapDef> MapDefs = new Dictionary<string, MapDef>();

		private static bool loadMapDefs(){

			try{
				string json = g_JM.loadJSON("./Content/json/maps.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> mDefs = jo["map_defs"].asList<Dictionary<string,object>>();

				foreach(Dictionary<string,object> dict in mDefs){
					jo = new JsonObject(dict);
					MapDef mDef;

					mDef.Name = jo["name"].asString();
					mDef.ID = jo["id"].asShort();

					MapDefs.Add(mDef.Name,mDef);
				}
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load map defs:\n" + e.ToString());
				return false;
			}

			return true;
		}

		public static StartDefs StartDefs;
		
		private static bool loadStartDefs(){
			try{
			string json = g_JM.loadJSON ("./Content/json/start_screen.v");
			JsonObject jo = g_JM.jsonToJsonObject(json);

			StartDefs.Seed = jo ["start_level", "seed"].asInt ();  
			StartDefs.SkillLevel = jo ["start_level", "skill_level"].asInt ();
			StartDefs.Returning = jo ["start_level", "returning"].asBool ();
			StartDefs.MapType = MapTypes [jo ["start_level", "map_type"].asString ()];

			}catch(Exception e){
				Console.Error.WriteLine("ERORR: could not load starting settings:\n" + e.ToString());
				return false;
			}
			return true;
		}
	}
}

