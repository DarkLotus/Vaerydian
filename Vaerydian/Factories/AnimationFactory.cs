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
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components.Utils;
using Vaerydian.Utils;
using Microsoft.Xna.Framework;

namespace Vaerydian.Factories
{
    static class AnimationFactory
    {

        public static ECSInstance ECSInstance;

		public static Character createAvatar(string avatarName){

			AvatarDef aDef = GameConfig.AvatarDefs[avatarName];
			Character c = new Character ();
			c.CurrentAnimtaion = aDef.CurrentAnimation;
			c.CurrentSkeleton = aDef.CurrentSkeleton;

			foreach (SkeletalDef sDef in aDef.SkeletalDefs) {
				Skeleton skel = new Skeleton();

				foreach(BoneDef bDef in sDef.BoneDefs){

					Bone bone = new Bone();
					bone.TextureName = bDef.Texture;
					bone.Origin = bDef.Origin;
					bone.Rotation = bDef.Rotation;
					bone.RotationOrigin = bDef.RotationOrigin;
					bone.AnimationTime = bDef.Time;

					foreach(string key in bDef.Animations.Keys){
						List<KeyFrame> list = new List<KeyFrame>();

						foreach(KeyFrameDef kDef in bDef.Animations[key].KeyFrameDefs){
							KeyFrame kf = new KeyFrame(kDef.Percent,
							                           kDef.Position,
							                           kDef.Rotation);
							list.Add (kf);
						}

						bone.Animations.Add(key,list);
					}

					skel.Bones.Add(bone);
				}
				c.Skeletons.Add(sDef.Name,skel);

			}

			return c;
		}

        public static Character createBatAnimation()
        {
			return createAvatar ("BAT");
			/*
			Character bat = new Character();

            Skeleton skeleton = new Skeleton();

            Bone wingL = new Bone();
            Bone wingR = new Bone();
            Bone head = new Bone();

            head.TextureName = "characters\\bat_head";
            head.Origin = new Vector2(12,12);
            head.Rotation = 0f;
            head.RotationOrigin = new Vector2(4, 4);
            head.AnimationTime = 500;
            List<KeyFrame> headFly = new List<KeyFrame>();
            headFly.Add(new KeyFrame(0, Vector2.Zero, 0f));
            headFly.Add(new KeyFrame(1, Vector2.Zero, 0f));
            head.Animations.Add("fly", headFly);

            wingL.TextureName = "characters\\bat_wing";
            wingL.Origin = new Vector2(4,12);
            wingL.Rotation = 0f;
            wingL.RotationOrigin = new Vector2(8, 4);
            wingL.AnimationTime = 500;
            List<KeyFrame> wingLFly = new List<KeyFrame>();
            wingLFly.Add(new KeyFrame(0, Vector2.Zero, 0f));
            wingLFly.Add(new KeyFrame(0.33f, Vector2.Zero, -.5f));
            wingLFly.Add(new KeyFrame(0.66f, Vector2.Zero, .5f));
            wingLFly.Add(new KeyFrame(1, Vector2.Zero, 0f));
            wingL.Animations.Add("fly", wingLFly);

            wingR.TextureName = "characters\\bat_wing";
            wingR.Origin = new Vector2(20,12);
            wingR.Rotation = 0f;
            wingR.RotationOrigin = new Vector2(0, 4);
            wingR.AnimationTime = 500;
            List<KeyFrame> wingRFly = new List<KeyFrame>();
            wingRFly.Add(new KeyFrame(0, Vector2.Zero, 0f));
            wingRFly.Add(new KeyFrame(0.33f, Vector2.Zero, .5f));
            wingRFly.Add(new KeyFrame(0.66f, Vector2.Zero, -.5f));
            wingRFly.Add(new KeyFrame(1, Vector2.Zero, 0f));
            wingR.Animations.Add("fly", wingRFly);


            skeleton.Bones.Add(head);
            skeleton.Bones.Add(wingL);
            skeleton.Bones.Add(wingR);

            bat.Skeletons.Add("normal", skeleton);
            bat.CurrentAnimtaion = "fly";
            bat.CurrentSkeleton = "normal";

            return bat;
			*/
        }

        public static Character createPlayerAnimation()
        {
			/*
            Character player = new Character();

            player.Skeletons.Add("front", createStandingSkeleton());


            player.CurrentSkeleton = "front";
            player.CurrentAnimtaion = "idle";

            return player;
            */
			return createAvatar ("PLAYER");
        }

        public static Skeleton createStandingSkeleton()
        {
            Skeleton standing = new Skeleton();

            Bone head = new Bone();
            Bone torso = new Bone();
            Bone lArm = new Bone();
            Bone rArm = new Bone();
            Bone lLeg = new Bone();
            Bone rLeg = new Bone();

            head.TextureName = "characters\\face";
            head.Origin = new Vector2(7, 0);
            head.Rotation = 0f;
            head.RotationOrigin = new Vector2(0, 0);
            head.AnimationTime = 500;
            List<KeyFrame> headIdle = new List<KeyFrame>();
            headIdle.Add(new KeyFrame(0, Vector2.Zero, 0f));
            headIdle.Add(new KeyFrame(.4f, new Vector2(0, 1f), 0f));
            headIdle.Add(new KeyFrame(.5f, new Vector2(0, 2f), 0f));
            headIdle.Add(new KeyFrame(1, Vector2.Zero, 0f));
            head.Animations.Add("idle", headIdle);
            head.Animations.Add("moving", headIdle);

            torso.TextureName = "characters\\ubody";
            torso.Origin = new Vector2(7, 16);
            torso.Rotation = 0f;
            torso.RotationOrigin = new Vector2(0, 0);
            torso.AnimationTime = 500;
            List<KeyFrame> torsoIdle = new List<KeyFrame>();
            torsoIdle.Add(new KeyFrame(0, Vector2.Zero, 0f));
            torsoIdle.Add(new KeyFrame(.4f, new Vector2(0, .75f), 0f));
            torsoIdle.Add(new KeyFrame(.5f, new Vector2(0, 1f), 0f));
            torsoIdle.Add(new KeyFrame(1, Vector2.Zero, 0f));
            torso.Animations.Add("idle", torsoIdle);
            torso.Animations.Add("moving", torsoIdle);

            lArm.TextureName = "characters\\hand";
            lArm.Origin = new Vector2(1, 16);
            lArm.Rotation = 0f;
            lArm.RotationOrigin = new Vector2(0, 0);
            lArm.AnimationTime = 500;
            List<KeyFrame> lArmIdle = new List<KeyFrame>();
            lArmIdle.Add(new KeyFrame(0, Vector2.Zero, 0f));
            lArmIdle.Add(new KeyFrame(.4f, new Vector2(0, 1f), 0f));
            lArmIdle.Add(new KeyFrame(.5f, new Vector2(0, 2f), 0f));
            lArmIdle.Add(new KeyFrame(1, Vector2.Zero, 0f));
            lArm.Animations.Add("idle", lArmIdle);
            List<KeyFrame> lArmMoving = new List<KeyFrame>();
            lArmMoving.Add(new KeyFrame(0, Vector2.Zero, 0f));
            lArmMoving.Add(new KeyFrame(.2f, new Vector2(0,1f), 0f));
            lArmMoving.Add(new KeyFrame(.5f, Vector2.Zero, 0f));
            lArmMoving.Add(new KeyFrame(.8f, new Vector2(0, -1f), 0f));
            lArmMoving.Add(new KeyFrame(1, Vector2.Zero, 0f));
            lArm.Animations.Add("moving", lArmMoving);

            rArm.TextureName = "characters\\hand";
            rArm.Origin = new Vector2(23, 16);
            rArm.Rotation = 0f;
            rArm.RotationOrigin = new Vector2(0, 0);
            rArm.AnimationTime = 500;
            List<KeyFrame> rArmIdle = new List<KeyFrame>();
            rArmIdle.Add(new KeyFrame(0, Vector2.Zero, 0f));
            rArmIdle.Add(new KeyFrame(.4f, new Vector2(0, 1f), 0f));
            rArmIdle.Add(new KeyFrame(.5f, new Vector2(0, 2f), 0f));
            rArmIdle.Add(new KeyFrame(1, Vector2.Zero, 0f));
            rArm.Animations.Add("idle", rArmIdle);
            List<KeyFrame> rArmMoving = new List<KeyFrame>();
            rArmMoving.Add(new KeyFrame(0, Vector2.Zero, 0f));
            rArmMoving.Add(new KeyFrame(.2f, new Vector2(0, -1f), 0f));
            rArmMoving.Add(new KeyFrame(.5f, Vector2.Zero, 0f));
            rArmMoving.Add(new KeyFrame(.8f, new Vector2(0, 1f), 0f));
            rArmMoving.Add(new KeyFrame(1, Vector2.Zero, 0f));
            rArm.Animations.Add("moving", rArmMoving);

            lLeg.TextureName = "characters\\foot";
            lLeg.Origin = new Vector2(7, 26);
            lLeg.Rotation = 0f;
            lLeg.RotationOrigin = new Vector2(0, 0);
            lLeg.AnimationTime = 500;
            List<KeyFrame> lLegIdle = new List<KeyFrame>();
            lLegIdle.Add(new KeyFrame(0, Vector2.Zero, 0f));
            lLegIdle.Add(new KeyFrame(1, Vector2.Zero, 0f));
            lLeg.Animations.Add("idle", lLegIdle);
            List<KeyFrame> lLegMoving = new List<KeyFrame>();
            lLegMoving.Add(new KeyFrame(0, Vector2.Zero, 0f));
            lLegMoving.Add(new KeyFrame(.2f, new Vector2(0, -1f), 0f));
            lLegMoving.Add(new KeyFrame(.5f, Vector2.Zero, 0f));
            lLegMoving.Add(new KeyFrame(.8f, new Vector2(0, 1f), 0f));
            lLegMoving.Add(new KeyFrame(1, Vector2.Zero, 0f));
            lLeg.Animations.Add("moving", lLegMoving);

            rLeg.TextureName = "characters\\foot";
            rLeg.Origin = new Vector2(17, 26);
            rLeg.Rotation = 0f;
            rLeg.RotationOrigin = new Vector2(0, 0);
            rLeg.AnimationTime = 500;
            List<KeyFrame> rLegIdle = new List<KeyFrame>();
            rLegIdle.Add(new KeyFrame(500, Vector2.Zero, 0f));
            rLeg.Animations.Add("idle", rLegIdle);
            List<KeyFrame> rLegMoving = new List<KeyFrame>();
            rLegMoving.Add(new KeyFrame(0, Vector2.Zero, 0f));
            rLegMoving.Add(new KeyFrame(.2f, new Vector2(0, 1f), 0f));
            rLegMoving.Add(new KeyFrame(.5f, Vector2.Zero, 0f));
            rLegMoving.Add(new KeyFrame(.8f, new Vector2(0, -1f), 0f));
            rLegMoving.Add(new KeyFrame(1, Vector2.Zero, 0f));
            rLeg.Animations.Add("moving", rLegMoving);


            standing.Bones.Add(head);
            standing.Bones.Add(torso);
            standing.Bones.Add(lArm);
            standing.Bones.Add(rArm);
            standing.Bones.Add(lLeg);
            standing.Bones.Add(rLeg);


            return standing;
        }

    }
}
