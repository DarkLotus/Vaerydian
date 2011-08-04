using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Combat;
using Vaerydian.Windows;
using Vaerydian.Sessions;
using WorldGeneration.Terrain;
using Vaerydian.Characters;


namespace Vaerydian.Screens
{
    /// <summary>
    /// handles combat sessions
    /// </summary>
    class CombatScreen : Screen
    {
        private SpriteBatch cs_SpriteBatch;
        
        /// <summary>
        /// combat engine reference
        /// </summary>
        private CombatEngine cs_CombatEngine = CombatEngine.Instance;
        
        /// <summary>
        /// menu to display actions in combat
        /// </summary>
        private TextMenuWindow cc_ActionWindow;

        /// <summary>
        /// contains list of current ActionWindow menu items
        /// </summary>
        private List<String> cc_ActionWindowItems = new List<String>();

        private List<Texture2D> textures = new List<Texture2D>();

        /// <summary>
        /// performs any needed screen initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //create menu items
            cc_ActionWindowItems.Add("Attack");//0
            cc_ActionWindowItems.Add("Abilities");//1
            cc_ActionWindowItems.Add("Items");//2
            cc_ActionWindowItems.Add("Defend");//3
            cc_ActionWindowItems.Add("Flee");//4
            
            //create the new window
            cc_ActionWindow = new TextMenuWindow(new Point(0, 400), cc_ActionWindowItems, "General");
            
            //register the window
            this.ScreenManager.WindowManager.addWindow(cc_ActionWindow);

            //initiate combat event
            //
            //THIS IS WITH TEST VALUES
            //
            cs_CombatEngine.newCombatEvent(getTestTerrainArray(), getTestPlayer(), getTestEnemies());
            //
            //THIS IS WITH TEST VALUES
            //


        }

        /// <summary>
        /// loads all screen related content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("terrain\\forest"));
        }

        /// <summary>
        /// unloads all screen related combat
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// handles all screen related input
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //see if it is the players turn
            if (cs_CombatEngine.CombatState == CombatState.PlayerTurn)
            {
                if(InputManager.isKeyToggled(Keys.Up))//move selection up
                {
                    if (cc_ActionWindow.MenuIndex > 0)
                    {
                        cc_ActionWindow.MenuIndex -= 1;
                    }
                }
                else if(InputManager.isKeyToggled(Keys.Down))//move selection down
                {
                    if (cc_ActionWindow.MenuIndex < cc_ActionWindowItems.Count - 1)
                    {
                        cc_ActionWindow.MenuIndex += 1;
                    }
                }
            }


            //player wants to exit
            if (InputManager.isKeyToggled(Keys.Escape))
                InputManager.yesExit = true;
        }

        /// <summary>
        /// performs all screen related updates
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            //Is the engine ready?
            if (cs_CombatEngine.CombatState == CombatState.CombatReady)
            {
                //figure out who goes first
                cs_CombatEngine.determineInitiative();
            }

        }

        /// <summary>
        /// performs all screen related drawing
        /// </summary>
        /// <param name="gameTime">curent game time</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //set spritebatch
            cs_SpriteBatch = ScreenManager.SpriteBatch;

            cs_SpriteBatch.Begin();

            //draw the field as long as combat is going on
            if (cs_CombatEngine.CombatState != CombatState.CombatInitializing ||
                cs_CombatEngine.CombatState != CombatState.CombatExit ||
                cs_CombatEngine.CombatState != CombatState.CombatFinished)
            {
                //draw the field
                drawCombatField();

                //draw the combatants
                drawCombatants();
            }

            //draw the combat finish graphics
            if (cs_CombatEngine.CombatState == CombatState.CombatFinished)
            {
            }

            cs_SpriteBatch.End();
        }

        /// <summary>
        /// draws the battle field
        /// </summary>
        private void drawCombatField()
        {
            

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    cs_SpriteBatch.Draw(textures[0], new Vector2(i * 200+240, j * 200 + 37), null, Color.White, 0f, Vector2.Zero, 8.0f, SpriteEffects.None, 0f);
                }
            }
            
        }

        /// <summary>
        /// draws all the combatants
        /// </summary>
        private void drawCombatants()
        {
            //draw player

            //draw enemies

        }


        /// <summary>
        /// generates the 3x3 terrain tiles
        /// </summary>
        /// <returns></returns>
        private Terrain[,] getTestTerrainArray()
        {
            Terrain[,] terrain = new Terrain[3, 3];
            Terrain tempTerrain = new Terrain();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tempTerrain.BaseTerrainType = BaseTerrainType.Land;
                    tempTerrain.LandTerrainType = LandTerrainType.Grassland;

                    terrain[i, j] = tempTerrain;
                }
            }

            return terrain;
        }

        /// <summary>
        /// generates a test player
        /// </summary>
        /// <returns></returns>
        private PlayerCharacter getTestPlayer()
        {
            PlayerCharacter player = new PlayerCharacter();
            player.Quickness = 50;


            return player;
        }

        /// <summary>
        /// generate a test enemy
        /// </summary>
        /// <returns></returns>
        private EnemyCharacter[] getTestEnemies()
        {
            EnemyCharacter[] enemies = new EnemyCharacter[1];
            EnemyCharacter enemy = new EnemyCharacter();
            enemy.Quickness = 50;


            enemies[0] = enemy;

            return enemies;
        }
    }
}
