using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ECSFramework;

using Vaerydian.Components.Characters;
using Vaerydian.Utils;
using Microsoft.Xna.Framework;

namespace Vaerydian.Factories
{
    class AnimationFactory
    {

        private ECSInstance a_EcsInstance;

        public AnimationFactory(ECSInstance ecsInstance) 
        {
            a_EcsInstance = ecsInstance;
        }

        public Character createBatAnimation()
        {
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
            headFly.Add(new KeyFrame(500, Vector2.Zero, 0f));
            head.Animations.Add("fly", headFly);

            wingL.TextureName = "characters\\bat_wing";
            wingL.Origin = new Vector2(4,12);
            wingL.Rotation = 0f;
            wingL.RotationOrigin = new Vector2(8, 4);
            wingL.AnimationTime = 500;
            List<KeyFrame> wingLFly = new List<KeyFrame>();
            wingLFly.Add(new KeyFrame(0, Vector2.Zero, 0f));
            wingLFly.Add(new KeyFrame(150, Vector2.Zero, -.5f));
            wingLFly.Add(new KeyFrame(350, Vector2.Zero, .5f));
            wingLFly.Add(new KeyFrame(500, Vector2.Zero, 0f));
            wingL.Animations.Add("fly", wingLFly);

            wingR.TextureName = "characters\\bat_wing";
            wingR.Origin = new Vector2(20,12);
            wingR.Rotation = 0f;
            wingR.RotationOrigin = new Vector2(0, 4);
            wingR.AnimationTime = 500;
            List<KeyFrame> wingRFly = new List<KeyFrame>();
            wingRFly.Add(new KeyFrame(0, Vector2.Zero, 0f));
            wingRFly.Add(new KeyFrame(150, Vector2.Zero, .5f));
            wingRFly.Add(new KeyFrame(350, Vector2.Zero, -.5f));
            wingRFly.Add(new KeyFrame(500, Vector2.Zero, 0f));
            wingR.Animations.Add("fly", wingRFly);


            skeleton.Bones.Add(head);
            skeleton.Bones.Add(wingL);
            skeleton.Bones.Add(wingR);

            bat.Skeletons.Add("normal", skeleton);
            bat.CurrentAnimtaion = "fly";
            bat.CurrentSkeleton = "normal";

            return bat;
        }

    }
}
