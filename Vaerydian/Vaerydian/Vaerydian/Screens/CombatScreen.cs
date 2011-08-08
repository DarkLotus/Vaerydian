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
    #region Enums
    
    public enum PlayerAction
    {
        Move,
        Attack,
        Ability,
        Defend,
        Item,
        Flee,
        None
    }

    public enum PlayerMoveDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    #endregion

    /// <summary>
    /// handles combat sessions
    /// </summary>
    class CombatScreen : Screen
    {
        #region Variables

        private SpriteBatch cs_SpriteBatch;
        
        /// <summary>
        /// combat engine reference
        /// </summary>
        private CombatEngine cs_CombatEngine = CombatEngine.Instance;
        
        /// <summary>
        /// menu to display actions in combat
        /// </summary>
        private TextMenuWindow cs_ActionWindow;

        /// <summary>
        /// contains list of current ActionWindow menu items
        /// </summary>
        private List<String> cs_ActionWindowItems = new List<String>();

        private DialogWindow cs_BattleLog;

        private List<Texture2D> textures = new List<Texture2D>();

        private PlayerAction cs_PlayerAction = PlayerAction.None;

        private PlayerMoveDirection cs_PlayerMoveDirection = PlayerMoveDirection.None;

        private Vector2 cs_PlayerIntendedPosition;

        private PlayerCharacter cs_Player;

        private EnemyCharacter[] cs_Enemies;

        #endregion

        #region Initialization & Loading

        /// <summary>
        /// performs any needed screen initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            //cs_Player = GameSession.Instance.PlayerCharacter;
            cs_Player = getTestPlayer();
            cs_Enemies = getTestEnemies();

            //update player intented positon
            cs_PlayerIntendedPosition = cs_Player.BattlePosition;

            //create menu items
            cs_ActionWindowItems.Add("Move");//0
            cs_ActionWindowItems.Add("Attack");//1
            cs_ActionWindowItems.Add("Abilities");//2
            cs_ActionWindowItems.Add("Defend");//3
            cs_ActionWindowItems.Add("Items");//4
            cs_ActionWindowItems.Add("Flee");//5
            
            //create the new window
            cs_ActionWindow = new TextMenuWindow(new Point(0, 400), cs_ActionWindowItems, "General");
            
            //register the window
            this.ScreenManager.WindowManager.addWindow(cs_ActionWindow);

            cs_BattleLog = new DialogWindow("You Enter Combat!", 70, 9,
                new Point(this.ScreenManager.GraphicsDevice.Viewport.Width / 2 - 300, this.ScreenManager.GraphicsDevice.Viewport.Height - 210),
                new Point(600, 200));

            this.ScreenManager.WindowManager.addWindow(cs_BattleLog);

            //initiate combat event
            //
            //THIS IS WITH TEST VALUES
            //
            cs_CombatEngine.newCombatEvent(getTestTerrainArray(), cs_Player, cs_Enemies);
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
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("characters\\player"));
            textures.Add(this.ScreenManager.Game.Content.Load<Texture2D>("characters\\enemy"));
        }

        /// <summary>
        /// unloads all screen related combat
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            //free all textures
            textures.Clear();

            //remove all windows
            this.ScreenManager.WindowManager.removeWindow(cs_ActionWindow);
            this.ScreenManager.WindowManager.removeWindow(cs_BattleLog);
        }

        #endregion

        #region INPUT

        /// <summary>
        /// handles all screen related input
        /// </summary>
        /// <param name="gameTime">current game time</param>
        public override void handleInput(GameTime gameTime)
        {
            base.handleInput(gameTime);

            //see if it is the players turn
            if (cs_CombatEngine.CombatState == CombatState.PlayerChooseAction)
            {
                //handle any action choices
                handleChooseAction(gameTime);
            }

            //if player has chosen an action, and it is to move
            if (cs_CombatEngine.CombatState == CombatState.PlayerActing &&
                cs_PlayerAction == PlayerAction.Move)
            {
                //handle any player movement decisions
                handlePlayerMovement();
            }

            //player wants to exit
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                //remove yourself
                this.ScreenManager.removeScreen(this);
                
                //load the start screen
                LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
            }
        }

        /// <summary>
        /// allows the player to choose an action
        /// </summary>
        /// <param name="gameTime">current game time</param>
        private void handleChooseAction(GameTime gameTime)
        {
            //move selection up
            if (InputManager.isKeyToggled(Keys.Up))
            {
                if (cs_ActionWindow.MenuIndex > 0)
                {
                    cs_ActionWindow.MenuIndex -= 1;
                }
            }//move selection down
            else if (InputManager.isKeyToggled(Keys.Down))
            {
                if (cs_ActionWindow.MenuIndex < cs_ActionWindowItems.Count - 1)
                {
                    cs_ActionWindow.MenuIndex += 1;
                }
            }

            //did player push enter
            if (InputManager.isKeyToggled(Keys.Enter))
            {
                //player chose to move
                if (cs_ActionWindow.MenuIndex == 0)
                {
                    cs_BattleLog.addDialog("Choose a direction to move.");
                    cs_PlayerAction = PlayerAction.Move;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 1)
                {
                    cs_PlayerAction = PlayerAction.Attack;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 2)
                {
                    cs_PlayerAction = PlayerAction.Ability;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 3)
                {
                    cs_PlayerAction = PlayerAction.Defend;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 4)
                {
                    cs_PlayerAction = PlayerAction.Item;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 4)
                {
                    cs_PlayerAction = PlayerAction.Flee;
                    cs_CombatEngine.CombatState = CombatState.PlayerActing;
                }
            }

        }

        /// <summary>
        /// handles moving the player
        /// </summary>
        private void handlePlayerMovement()
        {
            //select square up
            if (InputManager.isKeyToggled(Keys.Up))
            {
                cs_PlayerMoveDirection = PlayerMoveDirection.Up;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(0, -1);

            }//select square right
            else if (InputManager.isKeyToggled(Keys.Right))
            {
                cs_PlayerMoveDirection = PlayerMoveDirection.Right;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(1, 0);
            }//select square left
            else if (InputManager.isKeyToggled(Keys.Left))
            {
                cs_PlayerMoveDirection = PlayerMoveDirection.Left;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(-1, 0);
            }//select square down
            else if (InputManager.isKeyToggled(Keys.Down))
            {
                cs_PlayerMoveDirection = PlayerMoveDirection.Down;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(0, +1);
            }//accept movement
            else if (InputManager.isKeyToggled(Keys.Enter))
            {
                if (cs_PlayerMoveDirection == PlayerMoveDirection.Up)
                {

                    if (isDirectionMovable())
                    {
                        cs_BattleLog.addDialog("You move up.");

                        //reset move direction
                        cs_PlayerMoveDirection = PlayerMoveDirection.None;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move up.");
                    }
                }
                else if (cs_PlayerMoveDirection == PlayerMoveDirection.Down)
                {
                    if (isDirectionMovable())
                    {
                        cs_BattleLog.addDialog("You move down.");

                        //reset move direction
                        cs_PlayerMoveDirection = PlayerMoveDirection.None;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move down.");
                    }
                }
                else if (cs_PlayerMoveDirection == PlayerMoveDirection.Left)
                {
                    if (isDirectionMovable())
                    {
                        cs_BattleLog.addDialog("You move left.");

                        //reset move direction
                        cs_PlayerMoveDirection = PlayerMoveDirection.None;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move left");
                    }
                }
                else if (cs_PlayerMoveDirection == PlayerMoveDirection.Right)
                {
                    if (isDirectionMovable())
                    {
                        cs_BattleLog.addDialog("You move right.");

                        //reset move direction
                        cs_PlayerMoveDirection = PlayerMoveDirection.None;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move right");
                    }
                }
            }
        }

        /// <summary>
        /// change the current player location
        /// </summary>
        private void changePlayerLocation()
        {

        }

        /// <summary>
        /// is it possible to move in the chosen direction
        /// </summary>
        private bool isDirectionMovable()
        {
            return false;
        }

        #endregion

        #region UPDATE

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
                
                //anounce who's turn it is
                cs_BattleLog.addDialog(cs_CombatEngine.TurnList[0].Name + "'s Turn!");
            }//has a turn concluded?
            else if (cs_CombatEngine.CombatState == CombatState.CombatAssessTurn)
            {
                //assess the turn
                cs_CombatEngine.assessCombatTurn();

                //check to see if the character is alive
                if (cs_CombatEngine.IsPlayerDead)
                {
                    cs_CombatEngine.CombatState = CombatState.CombatFinished;
                }
                else
                {
                    //update the turn and continue
                    cs_CombatEngine.updateTurnState();
                    //anounce who's turn it is
                    cs_BattleLog.addDialog(cs_CombatEngine.TurnList[cs_CombatEngine.TurnIndex].Name + "'s Turn!");
                }

            }//is it time for the player to choose an action?
            else if (cs_CombatEngine.CombatState == CombatState.PlayerChooseAction)
            {
            }//has the player chosen an action?
            else if (cs_CombatEngine.CombatState == CombatState.PlayerActing)
            {
                //figure out what the player picked to do, and do it
                handlePlayerAction(gameTime);
            }// is it time for an NPC to choose an action?
            else if (cs_CombatEngine.CombatState == CombatState.NpcChooseAction)
            {
                //have the next NPC plan their action
                cs_CombatEngine.npcPlanAction();
            }//has the NPC chosen an action and ready to act on it?
            else if (cs_CombatEngine.CombatState == CombatState.NpcActing)
            {
                //NPC acts
                cs_CombatEngine.npcPerformAction();
            }
        }

        private void handlePlayerAction(GameTime gameTime)
        {

        }


        #endregion

        #region DRAWING

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
            
            //draw each combat square
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //check to see if you should color the square for player movement choices
                    if (i == cs_PlayerIntendedPosition.X && j == cs_PlayerIntendedPosition.Y && cs_PlayerAction == PlayerAction.Move)
                    {
                        cs_SpriteBatch.Draw(textures[0], new Vector2(i * 100 + 362, j * 100 + 100), null, Color.Red, 0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        cs_SpriteBatch.Draw(textures[0], new Vector2(i * 100 + 362, j * 100 + 100), null, Color.White, 0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);
                    }
                }
            }
            
        }

        /// <summary>
        /// draws all the combatants
        /// </summary>
        private void drawCombatants()
        {
            //draw player
            cs_SpriteBatch.Draw(textures[1], new Vector2(cs_Player.BattlePosition.X * 100 + 362, cs_Player.BattlePosition.Y * 100 + 100),
                null, Color.White, 0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);
            
            //draw enemies
            foreach (EnemyCharacter enemy in cs_CombatEngine.Enemies)
            {
                cs_SpriteBatch.Draw(textures[2], new Vector2(enemy.BattlePosition.X * 100 + 362, enemy.BattlePosition.Y * 100 + 100 ),
                    null, Color.White, 0f, Vector2.Zero, 4.0f, SpriteEffects.None, 0f);
            }
        }

        #endregion


        #region Testing 

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
            player.Name = "Alberez";
            player.Quickness = 50;
            player.Agility = 25;
            player.Perception = 30;
            player.Health = 50;

            player.BattlePosition = new Vector2(1, 1);

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
            enemy.Name = "Rawrizor The Dread Beast";
            enemy.Quickness = 50;
            enemy.Agility = 25;
            enemy.Perception = 30;
            enemy.Health = 50;
            enemy.BattlePosition = new Vector2(1, 0);

            enemies[0] = enemy;

            return enemies;
        }

        #endregion
    }
}
