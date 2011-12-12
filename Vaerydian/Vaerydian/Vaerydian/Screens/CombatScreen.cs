using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Vaerydian.Combat;
using Vaerydian.Combat.CombatIntelligence;
using Vaerydian.Windows;
using Vaerydian.Sessions;
using WorldGeneration.World;
using Vaerydian.Characters;
using Vaerydian.Characters.Abilities;
using Vaerydian.Characters.Skills;
using Vaerydian.Characters.Stats;
using Vaerydian.Items;
using Vaerydian.Characters.Behaviors;
using System.IO;




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

    public enum PlayerDirectionChoice
    {
        Up,
        Down,
        Left,
        Right,
        Self,
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

        private PlayerDirectionChoice cs_PlayerDirectionChoice = PlayerDirectionChoice.None;

        private Vector2 cs_PlayerIntendedPosition;

        private Vector2 cs_PlayerIntendedTarget;

        private Character cs_Player;

        private List<Character> cs_Combatants;

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
            cs_Combatants = getTestCombatants();
            cs_Combatants.Add(cs_Player);

            //update player intented positon
            cs_PlayerIntendedPosition = cs_Player.BattlePosition;
            cs_PlayerIntendedTarget = cs_Player.BattlePosition;

            //create menu items
            cs_ActionWindowItems.Add("Move");//0
            cs_ActionWindowItems.Add("Attack");//1
            cs_ActionWindowItems.Add("Abilities");//2
            cs_ActionWindowItems.Add("Defend");//3
            cs_ActionWindowItems.Add("Items");//4
            cs_ActionWindowItems.Add("Flee");//5
            
            //create the new Action window
            cs_ActionWindow = new TextMenuWindow(new Point(0, 400), cs_ActionWindowItems, "General");
            
            //register the Action window
            this.ScreenManager.WindowManager.addWindow(cs_ActionWindow);

            //create the Battle Log Window
            cs_BattleLog = new DialogWindow("You Enter Combat!", 70, 9,
                new Point(this.ScreenManager.GraphicsDevice.Viewport.Width / 2 - 300, this.ScreenManager.GraphicsDevice.Viewport.Height - 210),
                new Point(600, 200));

            //register the battle log
            this.ScreenManager.WindowManager.addWindow(cs_BattleLog);

            //initiate combat event
            //
            //THIS IS WITH TEST VALUES
            //
            cs_CombatEngine.newCombatEvent(getTestTerrainArray(10), 10, cs_Player, cs_Combatants);
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
            if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerChooseAction)
            {
                //handle any action choices
                handleChooseAction(gameTime);
            }

            //if player has chosen an action, and it is to move
            if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerActing &&
                cs_PlayerAction == PlayerAction.Move)
            {
                //handle any player movement decisions
                handlePlayerMovement();
            }
            else if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerActing &&
               cs_PlayerAction == PlayerAction.Attack)
            {
                //handle selection of target
                handlePlayerTargetChoice();
            }

            //player wants to exit
            if (InputManager.isKeyToggled(Keys.Escape))
            {
                if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerActing)
                {
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerChooseAction;
                    
                    cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                    cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                    cs_PlayerAction = PlayerAction.None;
                }
                else
                {

                    //remove yourself
                    this.ScreenManager.removeScreen(this);

                    //load the start screen
                    LoadingScreen.Load(this.ScreenManager, false, new StartScreen());
                }
            }

            if (InputManager.isKeyToggled(Keys.PrintScreen))
            {
                InputManager.YesScreenshot = true;
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
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 1)
                {
                    cs_PlayerAction = PlayerAction.Attack;
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 2)
                {
                    cs_PlayerAction = PlayerAction.Ability;
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 3)
                {
                    cs_PlayerAction = PlayerAction.Defend;
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 4)
                {
                    cs_PlayerAction = PlayerAction.Item;
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
                }
                else if (cs_ActionWindow.MenuIndex == 4)
                {
                    cs_PlayerAction = PlayerAction.Flee;
                    cs_CombatEngine.CurrentCombatState = CombatState.PlayerActing;
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
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Up;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(0, -1);

            }//select square right
            else if (InputManager.isKeyToggled(Keys.Right))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Right;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(1, 0);
            }//select square left
            else if (InputManager.isKeyToggled(Keys.Left))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Left;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(-1, 0);
            }//select square down
            else if (InputManager.isKeyToggled(Keys.Down))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Down;
                //set 1 above character
                cs_PlayerIntendedPosition = cs_Player.BattlePosition + new Vector2(0, +1);
            }//accept movement
            else if (InputManager.isKeyToggled(Keys.Enter))
            {
                if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Up)
                {

                    if (cs_CombatEngine.isDirectionMovable(cs_PlayerIntendedPosition))
                    {
                        cs_BattleLog.addDialog("You move up.");

                        cs_Player.BattlePosition = cs_PlayerIntendedPosition;

                        //reset move direction, intended target, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move up.");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Down)
                {
                    if (cs_CombatEngine.isDirectionMovable(cs_PlayerIntendedPosition))
                    {
                        cs_BattleLog.addDialog("You move down.");

                        cs_Player.BattlePosition = cs_PlayerIntendedPosition;

                        //reset move direction, intended target, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move down.");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Left)
                {
                    if (cs_CombatEngine.isDirectionMovable(cs_PlayerIntendedPosition))
                    {
                        cs_BattleLog.addDialog("You move left.");

                        cs_Player.BattlePosition = cs_PlayerIntendedPosition;

                        //reset move direction, intended target, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move left");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Right)
                {
                    if (cs_CombatEngine.isDirectionMovable(cs_PlayerIntendedPosition))
                    {
                        cs_BattleLog.addDialog("You move right.");

                        cs_Player.BattlePosition = cs_PlayerIntendedPosition;

                        //reset move direction, intended target, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You cannot move right");
                    }
                }
            }
        }

        /// <summary>
        /// handle the player's target choice
        /// </summary>
        private void handlePlayerTargetChoice()
        {
            //select square up
            if (InputManager.isKeyToggled(Keys.Up))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Up;
                //set 1 above character
                cs_PlayerIntendedTarget = cs_Player.BattlePosition + new Vector2(0, -1);

            }//select square right
            else if (InputManager.isKeyToggled(Keys.Right))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Right;
                //set 1 above character
                cs_PlayerIntendedTarget = cs_Player.BattlePosition + new Vector2(1, 0);
            }//select square left
            else if (InputManager.isKeyToggled(Keys.Left))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Left;
                //set 1 above character
                cs_PlayerIntendedTarget = cs_Player.BattlePosition + new Vector2(-1, 0);
            }//select square down
            else if (InputManager.isKeyToggled(Keys.Down))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Down;
                //set 1 above character
                cs_PlayerIntendedTarget = cs_Player.BattlePosition + new Vector2(0, +1);
            }//player targets self
            else if (InputManager.isKeyToggled(Keys.End))
            {
                cs_PlayerDirectionChoice = PlayerDirectionChoice.Self;
                //set 1 above character
                cs_PlayerIntendedTarget = cs_Player.BattlePosition;
            }//accept movement
            else if (InputManager.isKeyToggled(Keys.Enter))
            {
                if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Up)
                {

                    if (cs_CombatEngine.isCellAttackable(cs_PlayerIntendedTarget))
                    {
                        //attack the target
                        cs_CombatEngine.attackTarget(cs_Player, cs_CombatEngine.getTarget(cs_PlayerIntendedTarget));
                        
                        //tell the player the outcome
                        if(cs_CombatEngine.BattleText.Length > 0)
                            cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());

                        //reset move direction, intended target, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You Can't Target That");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Down)
                {
                    if (cs_CombatEngine.isCellAttackable(cs_PlayerIntendedTarget))
                    {
                        //attack the target
                        cs_CombatEngine.attackTarget(cs_Player, cs_CombatEngine.getTarget(cs_PlayerIntendedTarget));

                        //tell the player the outcome
                        if (cs_CombatEngine.BattleText.Length > 0)
                            cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());

                        //reset move direction, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You Can't Target That");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Left)
                {
                    if (cs_CombatEngine.isCellAttackable(cs_PlayerIntendedTarget))
                    {
                        //attack the target
                        cs_CombatEngine.attackTarget(cs_Player, cs_CombatEngine.getTarget(cs_PlayerIntendedTarget));

                        //tell the player the outcome
                        if (cs_CombatEngine.BattleText.Length > 0)
                            cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());

                        //reset move direction, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You Can't Target That");
                    }
                }
                else if (cs_PlayerDirectionChoice == PlayerDirectionChoice.Right)
                {
                    if (cs_CombatEngine.isCellAttackable(cs_PlayerIntendedTarget))
                    {
                        //attack the target
                        cs_CombatEngine.attackTarget(cs_Player, cs_CombatEngine.getTarget(cs_PlayerIntendedTarget));

                        //tell the player the outcome
                        if (cs_CombatEngine.BattleText.Length > 0)
                            cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());

                        //reset move direction, player action, and combat state
                        cs_PlayerDirectionChoice = PlayerDirectionChoice.None;
                        cs_PlayerIntendedTarget = cs_Player.BattlePosition;
                        cs_PlayerAction = PlayerAction.None;
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatAssessTurn;
                    }
                    else
                    {
                        cs_BattleLog.addDialog("You Can't Target That");
                    }
                }
            }
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
            if (cs_CombatEngine.CurrentCombatState == CombatState.CombatReady)
            {
                //announced the round
                cs_BattleLog.addDialog("[Round " + cs_CombatEngine.RoundCounter + "]");

                //figure out who goes first
                cs_CombatEngine.determineInitiative();
                
                //anounce who's turn it is
                cs_BattleLog.addDialog(cs_CombatEngine.TurnList[0].Name + "'s Turn!");
            }//has a turn concluded?
            else if (cs_CombatEngine.CurrentCombatState == CombatState.CombatAssessTurn)
            {
                //assess the turn
                cs_CombatEngine.assessCombatTurn();

                //add any assessment dialog
                if(cs_CombatEngine.BattleText.Length > 0)
                    cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());

                //check to see if the character is alive
                if (cs_CombatEngine.IsPlayerDead)
                {

                    cs_CombatEngine.CurrentCombatState = CombatState.CombatLost;
                    return;
                }
                else if(cs_CombatEngine.TurnList.Count == 1)
                {
                    if (cs_CombatEngine.TurnList[0].Name == cs_Player.Name)
                    {
                        //player has won
                        cs_BattleLog.addDialog(cs_Player.Name + " has won the battle!");
                        cs_CombatEngine.CurrentCombatState = CombatState.CombatFinished;
                    }
                }
                else
                {
                    //are there turns left for this round?
                    if (cs_CombatEngine.TurnIndex < cs_CombatEngine.TurnList.Count-1)
                    {
                        //update the turn and continue
                        cs_CombatEngine.updateTurnState();

                        //anounce who's turn it is
                        cs_BattleLog.addDialog(cs_CombatEngine.TurnList[cs_CombatEngine.TurnIndex].Name + "'s Turn!");
                    }
                    else//all turns have gone, update round
                    {
                        cs_CombatEngine.newRound();
                    }
                }

            }//combat has been lost
            else if (cs_CombatEngine.CurrentCombatState == CombatState.CombatLost)
            {
            }//combat has been finished
            else if (cs_CombatEngine.CurrentCombatState == CombatState.CombatFinished)
            {
            }//combat time to exit
            else if (cs_CombatEngine.CurrentCombatState == CombatState.CombatExit)
            {


            }//is it time for the player to choose an action?
            else if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerChooseAction)
            {
            }//has the player chosen an action?
            else if (cs_CombatEngine.CurrentCombatState == CombatState.PlayerActing)
            {
                //figure out what the player picked to do, and do it
                handlePlayerAction(gameTime);
            }// is it time for an NPC to choose an action?
            else if (cs_CombatEngine.CurrentCombatState == CombatState.NpcChooseAction)
            {
                //have the next NPC plan their action
                cs_CombatEngine.npcPlanAction();
                //cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());
            }//has the NPC chosen an action and ready to act on it?
            else if (cs_CombatEngine.CurrentCombatState == CombatState.NpcActing)
            {
                //NPC acts
                if(cs_CombatEngine.npcPerformAction() && cs_CombatEngine.BattleText.Length > 0)
                    cs_BattleLog.addDialog(cs_CombatEngine.BattleText.ToString());
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
            if (cs_CombatEngine.CurrentCombatState != CombatState.CombatInitializing ||
                cs_CombatEngine.CurrentCombatState != CombatState.CombatExit ||
                cs_CombatEngine.CurrentCombatState != CombatState.CombatFinished)
            {
                //draw the field
                drawCombatField();

                //draw the combatants
                drawCombatants();
            }

            //draw the combat finish graphics
            if (cs_CombatEngine.CurrentCombatState == CombatState.CombatFinished)
            {
            }

            cs_SpriteBatch.End();

            if (InputManager.YesScreenshot)
            {
                InputManager.YesScreenshot = false;
                saveScreenShot(cs_SpriteBatch.GraphicsDevice);
            }
        }

        /// <summary>
        /// draws the battle field
        /// </summary>
        private void drawCombatField()
        {
            
            //draw each combat square
            for (int i = 0; i < cs_CombatEngine.Dimensions; i++)
            {
                for (int j = 0; j < cs_CombatEngine.Dimensions; j++)
                {
                    //check to see if you should color the square for player movement choices
                    if ((i == cs_PlayerIntendedPosition.X && j == cs_PlayerIntendedPosition.Y && cs_PlayerAction == PlayerAction.Move) ||
                        (i == cs_PlayerIntendedTarget.X && j == cs_PlayerIntendedTarget.Y && cs_PlayerAction == PlayerAction.Attack))
                    {
                        cs_SpriteBatch.Draw(textures[0], new Vector2(i * 25 + 362, j * 25 + 100), null, Color.Red, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        cs_SpriteBatch.Draw(textures[0], new Vector2(i * 25 + 362, j * 25 + 100), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    }
                }
            }
            
        }

        /// <summary>
        /// 
        /// draws all the combatants
        /// </summary>
        private void drawCombatants()
        {
            //draw combatants
            foreach (Character combatant in cs_CombatEngine.TurnList)
            {
                //draw player different
                if (combatant.CharacterType == CharacterType.Player)
                {
                    cs_SpriteBatch.Draw(textures[1], new Vector2(cs_Player.BattlePosition.X * 25 + 362 + 6.25f, cs_Player.BattlePosition.Y * 25 + 100 + 6.25f),
                        null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                }
                else
                {
                    cs_SpriteBatch.Draw(textures[2], new Vector2(combatant.BattlePosition.X * 25 + 362 + 6.25f, combatant.BattlePosition.Y * 25 + 100 + 6.25f),
                        null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
                }
            }
        }

        /// <summary>
        /// captures and saves the screen of the current graphics device
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void saveScreenShot(GraphicsDevice graphicsDevice)
        {
            //setup a color buffer to get the back Buffer's data
            Color[] colors = new Color[graphicsDevice.PresentationParameters.BackBufferHeight * graphicsDevice.PresentationParameters.BackBufferWidth];

            //place the back bugger data into the color buffer
            graphicsDevice.GetBackBufferData<Color>(colors);

            //setup the filestream for the screenshot
            FileStream fs = new FileStream("screenshot.png", FileMode.Create);

            //setup the texture that will be saved
            Texture2D picTex = new Texture2D(graphicsDevice, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //set the texture's color data to that of the color buffer
            picTex.SetData<Color>(colors);

            //save the texture to a png image file
            picTex.SaveAsPng(fs, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight);

            //close the file stream
            fs.Close();

            GC.Collect();
        }

        #endregion

        #region Testing 

        /// <summary>
        /// generates the 3x3 terrain tiles
        /// </summary>
        /// <returns></returns>
        private Terrain[,] getTestTerrainArray(int size)
        {
            Terrain[,] terrain = new Terrain[size, size];
            Terrain tempTerrain = new Terrain();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
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
        private Character getTestPlayer()
        {
            Character player = new Character();
            player.CharacterType = CharacterType.Player;
            player.Name = "Player";
            player.Health = 100;
            player.Stats.Add("Quickness", new Stat("Quickness", 55));
            player.Stats.Add("Agility", new Stat("Agility", 55));
            player.Stats.Add("Strength", new Stat("Strength", 55));
            player.Stats.Add("Perception", new Stat("Perception", 55));
            player.BattlePosition = new Vector2(0, 9);

            //setup skills
            player.Skills.Add("Dodge", new Skill("Dodge", 55, SkillType.Defensive));
            player.Skills.Add("WeaponSkill", new Skill("WeaponSkill", 55, SkillType.Offensive));

            //setup equipment
            Equipment equipment = new Equipment();
            equipment.ArmorChest = new Armor(10, 4);
            equipment.Weapon = new Weapon(5, 4, 1, 1, DamageType.Common);
            player.Equipment = equipment;


            return player;
        }

        /// <summary>
        /// generate a test enemy
        /// </summary>
        /// <returns></returns>
        private List<Character> getTestCombatants()
        {
            List<Character> combatants = new List<Character>();

            Character enemy = new Character();
            enemy.CharacterType = CharacterType.NPC;
            enemy.Name = "Test Enemy";
            enemy.Health = 100;
            enemy.Stats.Add("Quickness", new Stat("Quickness", 50));
            enemy.Stats.Add("Agility", new Stat("Agility", 50));
            enemy.Stats.Add("Strength", new Stat("Strength", 50));
            enemy.Stats.Add("Perception", new Stat("Perception", 50));
            enemy.BattlePosition = new Vector2(9, 0);
            
            //setup skills
            enemy.Skills.Add("Dodge", new Skill("Dodge",50,SkillType.Defensive));
            enemy.Skills.Add("WeaponSkill", new Skill("WeaponSkill",50,SkillType.Offensive));

            //setup equipment
            Equipment equipment = new Equipment();
            equipment.ArmorChest = new Armor(5,3);
            equipment.Weapon = new Weapon(1,3,1,1f,DamageType.Common);
            enemy.Equipment = equipment;

            //enemy.OldBehavior = new OldBehavior(enemy);
            enemy.Behavior = new SimpleEnemyBehavior(enemy);
            enemy.Behavior.EnemyState = SimpleEnemyState.Combat;
            enemy.Behavior.CombatState = SimpleCombatState.Thinking;

            Character enemy2 = new Character();
            enemy2.CharacterType = CharacterType.NPC;
            enemy2.Name = "Cooperating Enemy";
            enemy2.Health = 50;
            enemy2.Stats.Add("Quickness", new Stat("Quickness", 50));
            enemy2.Stats.Add("Agility", new Stat("Agility", 50));
            enemy2.Stats.Add("Strength", new Stat("Strength", 50));
            enemy2.Stats.Add("Perception", new Stat("Perception", 50));
            enemy2.BattlePosition = new Vector2(0, 0);

            //setup skills
            enemy2.Skills.Add("Dodge", new Skill("Dodge", 50, SkillType.Defensive));
            enemy2.Skills.Add("WeaponSkill", new Skill("WeaponSkill", 50, SkillType.Offensive));

            //setup equipment
            Equipment equipmentEnemy2 = new Equipment();
            equipmentEnemy2.ArmorChest = new Armor(5, 3);
            equipmentEnemy2.Weapon = new Weapon(1, 3, 3f, 5f, DamageType.Common);
            enemy2.Equipment = equipmentEnemy2;

            //enemy.OldBehavior = new OldBehavior(enemy);
            enemy2.Behavior = new SimpleEnemyBehavior(enemy2);
            enemy2.Behavior.EnemyState = SimpleEnemyState.Combat;
            enemy2.Behavior.CombatState = SimpleCombatState.Thinking;
            enemy2.Behavior.HostileList.Add(enemy);

            combatants.Add(enemy);
            combatants.Add(enemy2);

            return combatants;
        }

        #endregion
    }
}
