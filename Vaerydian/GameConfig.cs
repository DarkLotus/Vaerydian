/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Vaerydian.Utils;
using Vaerydian.Screens;
using Vaerydian.Components.Actions;
using Vaerydian.Characters;
using Vaerydian.Components.Characters;
using Vaerydian.Components.Items;

namespace Vaerydian
{

	public static class GameConfig
	{
		private static JsonManager g_JM = new JsonManager();

		public static bool loadConfig(){

			if (!GameConfig.loadEffectDefs ())
				return false;

			if (!GameConfig.LoadDamageDefs ())
				return false;

			if (!GameConfig.loadActionDefs ())
				return false;

			if (!GameConfig.loadTerrainDefs ())
				return false;

			if (!GameConfig.loadMapDefs ())
				return false;

			if (!GameConfig.loadStartDefs ())
				return false;

			if (!GameConfig.loadCharacterAnimation ())
				return false;

			if (!GameConfig.loadCreatures ())
				return false;

			return true;
		}

		/// <summary>
		/// The effects.
		/// </summary>
		public static Dictionary<string, short> Effects = new Dictionary<string, short>();

		/// <summary>
		/// Loads the effect defs.
		/// </summary>
		/// <returns><c>true</c>, if effect defs was loaded, <c>false</c> otherwise.</returns>
		private static bool loadEffectDefs(){

			Effects.Add ("NONE", 0);

			return true;
		}

		/// <summary>
		/// The damage defs.
		/// </summary>
		public static Dictionary<string, DamageDef> DamageDefs = new Dictionary<string, DamageDef>();

		/// <summary>
		/// Loads the damage defs.
		/// </summary>
		/// <returns><c>true</c>, if damage defs was loaded, <c>false</c> otherwise.</returns>
		private static bool LoadDamageDefs(){
			try{
				string json = g_JM.loadJSON("./Content/json/damage.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);
				
				List<Dictionary<string,object>> dDefs = jo ["damage_defs"].asList<Dictionary<string,object>> ();
				
				foreach(Dictionary<string,object> dict in dDefs){
					jo = new JsonObject(dict);
					
					DamageDef dDef = default(DamageDef);
					dDef.Name = jo["name"].asString();
					dDef.DamageType = jo["damage_type"].asEnum<DamageType>();
					dDef.DamageBasis = jo["damage_basis"].asEnum<DamageBasis>();
					dDef.Min = jo["min"].asInt();
					dDef.Max = jo["max"].asInt();
					dDef.SkillName = jo["skill_name"].asEnum<SkillName>();
					dDef.StatType = jo["stat_type"].asEnum<StatType>();
					DamageDefs.Add(dDef.Name,dDef);
				}
				
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: failed to load damage defs:\n" + e.ToString());
				return false;
			}
			
			return true;
		}

		/// <summary>
		/// The action defs.
		/// </summary>
		public static Dictionary<string, ActionDef> ActionDefs = new Dictionary<string, ActionDef>();

		/// <summary>
		/// Loads the action defs.
		/// </summary>
		/// <returns><c>true</c>, if action defs was loaded, <c>false</c> otherwise.</returns>
		private static bool loadActionDefs(){
			try{
				string json = g_JM.loadJSON("./Content/json/actions.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> aDefs = jo ["action_defs"].asList<Dictionary<string,object>> ();

				foreach(Dictionary<string,object> dict in aDefs){
					jo = new JsonObject(dict);

					ActionDef aDef = default(ActionDef);
					aDef.Name = jo["name"].asString();
					aDef.ActionType = jo["action_type"].asEnum<ActionType>();
					aDef.DamageDef = DamageDefs[jo["damage_def"].asString()];

					ActionDefs.Add(aDef.Name, aDef);
				}

			}catch(Exception e){
				Console.Error.WriteLine("ERROR: failed to load actions:" + e.ToString());
				return false;
			}


			return true;
		}

		/// <summary>
		/// The terrain defs.
		/// </summary>
		public static Dictionary<string,TerrainDef> TerrainDefs = new Dictionary<string, TerrainDef> ();

		/// <summary>
		/// Loads the terrain defs.
		/// </summary>
		/// <returns><c>true</c>, if terrain defs was loaded, <c>false</c> otherwise.</returns>
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
					tDef.Color = new Color((int) tColor[0],(int)tColor[1],(int)tColor[2]);
					tDef.Passible = jo["passible"].asBool();

					//TODO: define effect - after effect is defined
					tDef.Effect = Effects[jo["effect"].asString()];

					tDef.TerrainType = jo["type"].asEnum<TerrainType>();

					TerrainDefs.Add(tDef.Name,tDef);
				}
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load terrain:\n" + e.ToString());
				return false;
			}

			return true;
		}

		/// <summary>
		/// The map defs.
		/// </summary>
		public static Dictionary<string,MapDef> MapDefs = new Dictionary<string, MapDef>();

		/// <summary>
		/// Loads the map defs.
		/// </summary>
		/// <returns><c>true</c>, if map defs was loaded, <c>false</c> otherwise.</returns>
		private static bool loadMapDefs(){

			try{
				string json = g_JM.loadJSON("./Content/json/maps.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				List<Dictionary<string,object>> mDefs = jo["map_defs"].asList<Dictionary<string,object>>();

				//define map def
				foreach(Dictionary<string,object> dict in mDefs){
					jo = new JsonObject(dict);
					MapDef mDef = default(MapDef);
					mDef.Tiles = new Dictionary<string, List<TileDef>>();

					mDef.Name = jo["name"].asString();
					mDef.MapType = jo["map_type"].asEnum<MapType>();

					List<Dictionary<string,object>> tDefs = jo["tile_maps"].asList<Dictionary<string,object>>();

					//define terrain maps
					foreach(Dictionary<string,object> tDict in tDefs){
						jo = new JsonObject(tDict);

						string mapTo = jo["map_to"].asString();

						List<Dictionary<string,object>> tiles = jo["tiles"].asList<Dictionary<string,object>>();

						List<TileDef> tileDefs = new List<TileDef>();

						//define tiles
						foreach(Dictionary<string,object> tileDict in tiles){
							jo = new JsonObject(tileDict);

							TileDef tileDef = default(TileDef);

							tileDef.TerrainDef = TerrainDefs[jo["name"].asString()];
							tileDef.Probability = jo["prob"].asInt();

							tileDefs.Add(tileDef);
						}

						//store tile reference
						mDef.Tiles.Add(mapTo,tileDefs);
					}
						
					//set the map def
					MapDefs.Add(mDef.Name,mDef);
				}
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load map defs:\n" + e.ToString());
				return false;
			}

			return true;
		}

		/// <summary>
		/// The start defs.
		/// </summary>
		public static StartDefs StartDefs;

		/// <summary>
		/// Loads the start defs.
		/// </summary>
		/// <returns><c>true</c>, if start defs was loaded, <c>false</c> otherwise.</returns>
		private static bool loadStartDefs(){
			try{
				string json = g_JM.loadJSON ("./Content/json/start_screen.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				StartDefs.Seed = jo ["start_level", "seed"].asInt ();  
				StartDefs.SkillLevel = jo ["start_level", "skill_level"].asInt ();
				StartDefs.Returning = jo ["start_level", "returning"].asBool ();
				StartDefs.MapType = jo ["start_level", "map_type"].asEnum<MapType>();

			}catch(Exception e){
				Console.Error.WriteLine("ERORR: could not load starting settings:\n" + e.ToString());
				return false;
			}
			return true;
		}

		/// <summary>
		/// The animation defs.
		/// </summary>
		public static Dictionary<string, AnimationDef> AnimationDefs = new Dictionary<string, AnimationDef> ();
		/// <summary>
		/// The skeletal defs.
		/// </summary>
		public static Dictionary<string, SkeletalDef> SkeletalDefs = new Dictionary<string, SkeletalDef> ();
		/// <summary>
		/// The character defs.
		/// </summary>
		public static Dictionary<string, AvatarDef> AvatarDefs = new Dictionary<string, AvatarDef>();

		/// <summary>
		/// Loads the character animation.
		/// </summary>
		/// <returns><c>true</c>, if character animation was loaded, <c>false</c> otherwise.</returns>
		private static bool loadCharacterAnimation(){
			try{
				string json = g_JM.loadJSON("./Content/json/animation.v");
				JsonObject jo = g_JM.jsonToJsonObject(json);

				//construct all animation defs
				List<Dictionary<string,object>> aDefs = jo["animation_defs"].asList<Dictionary<string,object>>();

				foreach(Dictionary<string,object> dict in aDefs){
					jo = new JsonObject(dict);


					AnimationDef aDef = default(AnimationDef);
					aDef.KeyFrameDefs = new List<KeyFrameDef>();
					aDef.Name = jo["name"].asString();

					List<Dictionary<string,object>> kDefs = jo["key_frames"].asList<Dictionary<string,object>>();

					foreach(Dictionary<string,object> kDict in kDefs){
						jo = new JsonObject(kDict);

						KeyFrameDef kDef = default(KeyFrameDef);
						kDef.Percent = jo["percent"].asFloat();
						kDef.Position = new Vector2(jo["x"].asFloat(),jo["y"].asFloat());
						kDef.Rotation = jo["rotation"].asFloat();

						aDef.KeyFrameDefs.Add(kDef);
					}

					//add to dict
					AnimationDefs.Add(aDef.Name,aDef);
				}

				//reset
				jo = g_JM.jsonToJsonObject(json);

				//add skeletons
				List<Dictionary<string,object>> sDefs = jo["skeleton_defs"].asList<Dictionary<string,object>>();

				foreach(Dictionary<string, object> dict in sDefs){
					jo = new JsonObject(dict);

					SkeletalDef sDef = default(SkeletalDef);
					sDef.BoneDefs = new List<BoneDef>();
					sDef.Name = jo["name"].asString();

					List<Dictionary<string,object>> bDefs = jo["bones"].asList<Dictionary<string,object>>();

					foreach(Dictionary<string, object> bDict in bDefs){
						jo = new JsonObject(bDict);

						BoneDef bDef = default(BoneDef);
						bDef.Animations = new Dictionary<string, AnimationDef>();

						bDef.Name = jo["name"].asString();
						bDef.Texture = jo["texture"].asString();
						bDef.Origin = new Vector2(jo["origin_x"].asInt(),jo["origin_y"].asInt());
						bDef.Rotation = jo["rotation"].asFloat();
						bDef.RotationOrigin = new Vector2(jo["rotation_x"].asInt(), jo["rotation_y"].asInt());
						bDef.Time = jo["time"].asInt();

						List<Dictionary<string,object>> animDefs = jo["animations"].asList<Dictionary<string,object>>();

						foreach(Dictionary<string,object> aDict in animDefs){
							jo = new JsonObject(aDict);

							bDef.Animations.Add(jo["name"].asString(), AnimationDefs[jo["animation_def"].asString()]);
						}

						sDef.BoneDefs.Add(bDef);
					}

					SkeletalDefs.Add(sDef.Name, sDef);
				}

				//reset
				jo = g_JM.jsonToJsonObject(json);
				
				//add character defs
				List<Dictionary<string,object>> cDefs = jo["character_defs"].asList<Dictionary<string,object>>();

				foreach(Dictionary<string,object> dict in cDefs){
					jo = new JsonObject(dict);

					AvatarDef cDef = default(AvatarDef);
					cDef.SkeletalDefs = new List<SkeletalDef>();
					cDef.Name = jo["name"].asString();
					cDef.CurrentSkeleton = jo["current_skeleton"].asString();
					cDef.CurrentAnimation = jo["current_animation"].asString();

					List<string> skels = jo["skeletons"].asList<string>();

					foreach(string skel in skels){

						cDef.SkeletalDefs.Add (SkeletalDefs[skel]);
					}

					AvatarDefs.Add(cDef.Name,cDef);
				}


			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load character animations:\n" + e.ToString());
				return false;
			}

			return true;
		}

		/// <summary>
		/// The creature defs.
		/// </summary>
		public static Dictionary<string,CharacterDef> CharacterDefs = new Dictionary<string, CharacterDef> ();

		/// <summary>
		/// Loads the creatures.
		/// </summary>
		/// <returns><c>true</c>, if creatures was loaded, <c>false</c> otherwise.</returns>
		private static bool loadCreatures(){
			try{
				string json = g_JM.loadJSON ("./Content/json/creatures.v");
				JsonObject jo = g_JM.jsonToJsonObject (json);

				List<Dictionary<string,object>> cDefs = jo ["creature_defs"].asList<Dictionary<string,object>> ();

				foreach (Dictionary<string,object> dict in cDefs) {
					jo = new JsonObject(dict);

					CharacterDef cDef = default(CharacterDef);
					cDef.Name = jo["name"].asString();
					cDef.AvatarDef = AvatarDefs[jo["character_def"].asString()];
					cDef.SkillLevel = jo["skill_level"].asInt();

					CharacterDefs.Add(cDef.Name,cDef);
				}
			}catch(Exception e){
				Console.Error.WriteLine("ERROR: could not load creatures:\n" + e.ToString());
				return false;
			}
			return true;
		}
	}

	/// <summary>
	/// Creature def.
	/// </summary>
	public struct CharacterDef{
		public string Name;
		public AvatarDef AvatarDef;
		public int SkillLevel;
		public InfoDef InfoDef;
		public LifeDef LifeDef;
		public SupportedInteractions SupportedInteractions;
		public EquipmentDef EquipmentDef;
		public KnowledgesDef KnowledgesDef;
		public StatisticsDef StatisticsDef;
		public SkillsDef SkillsDef;
		public FactionsDef FactionsDef;
	}

	/// <summary>
	/// Character def.
	/// </summary>
	public struct AvatarDef{
		public string Name;
		public List<SkeletalDef> SkeletalDefs;
		public string CurrentSkeleton;
		public string CurrentAnimation;
	}

	/// <summary>
	/// Skeletal def.
	/// </summary>
	public struct SkeletalDef{
		public string Name;
		public List<BoneDef> BoneDefs;
	}

	/// <summary>
	/// Bone def.
	/// </summary>
	public struct BoneDef{
		public string Name;
		public string Texture;
		public Vector2 Origin;
		public float Rotation;
		public Vector2 RotationOrigin;
		public int Time;
		public Dictionary<string,AnimationDef> Animations;
	}

	/// <summary>
	/// Animation def.
	/// </summary>
	public struct AnimationDef{
		public string Name;
		public List<KeyFrameDef> KeyFrameDefs;
	}

	/// <summary>
	/// Key frame def.
	/// </summary>
	public struct KeyFrameDef{
		public float Percent;
		public Vector2 Position;
		public float Rotation;
	}


}

