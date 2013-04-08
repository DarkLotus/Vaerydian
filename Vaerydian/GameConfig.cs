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

		public static Dictionary<string, DamageType> DamageTypes = new Dictionary<string, DamageType>();
		public static Dictionary<string, DamageDef> DamageDefs = new Dictionary<string, DamageDef>();

		private static bool loadDamage(){
			DamageTypes.Add ("NONE", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("SLASHING", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("CRUSHING", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("PIERCING", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("ICE", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("FIRE", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("EARTH", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("WIND", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("WATER", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("LIGHT", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("DARK", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("CHAOS", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("ORDER", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("POISON", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("DISEASE", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("ARCANE", new DamageType{Name = "", ID = 0});			
			DamageTypes.Add ("MENTAL", new DamageType{Name = "", ID = 0});
			DamageTypes.Add ("SONIC", new DamageType{Name = "", ID = 0});

			return true;
		}

		public static Dictionary<string, ActionType> ActionTypes = new Dictionary<string, ActionType>();
		public static Dictionary<string, ImpactType> ImpactTypes = new Dictionary<string, ImpactType>();
		public static Dictionary<string, ModifyType> ModifyTypes = new Dictionary<string, ModifyType>();
		public static Dictionary<string, ModifyDuration> ModifyDurations = new Dictionary<string, ModifyDuration>();
		public static Dictionary<string, CreateType> CreateTypes = new Dictionary<string, CreateType>();
		public static Dictionary<string, DestoryType> DestroyTypes = new Dictionary<string, DestoryType>();

		private static bool loadActions(){
			ActionTypes.Add ("DAMAGE", new ActionType{Name = "", ID = 0});
			ActionTypes.Add ("MODIFY", new ActionType{Name = "", ID = 0});
			ActionTypes.Add ("CREATE", new ActionType{Name = "", ID = 0});
			ActionTypes.Add ("DESTROY", new ActionType{Name = "", ID = 0});

			ImpactTypes.Add ("", new ImpactType{Name = "", ID = 0});
			ImpactTypes.Add ("", new ImpactType{Name = "", ID = 0});
			ImpactTypes.Add ("", new ImpactType{Name = "", ID = 0});
			ImpactTypes.Add ("", new ImpactType{Name = "", ID = 0});
			ImpactTypes.Add ("", new ImpactType{Name = "", ID = 0});





			return true;
		}

		public static Dictionary<string, ActionDef> ActionDefs = new Dictionary<string, ActionDef>();

		private static bool loadActionDefs(){
			try{
				string json = g_JM.loadJSON("./Content/json/actions.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> aDefs = jo ["actiop_defs"].asList<Dictionary<string,object>> ();

				foreach(Dictionary<string,object> dict in aDefs){
					jo = new JsonObject(dict);

					ActionDef aDef;
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

