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
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vaerydian.Screens;

using Glimpse.Managers;

namespace Vaerydian
{

	public class NewLoadingScreen : Screen
	{

		private static ScreenManager n_ScreenManager;

		private static Screen n_Screen;

		private static bool n_DoneLoading = false;

		private Thread n_BackgroundThread;

		private static SpriteBatch n_SpriteBatch;

        private Texture2D n_LoadingTexture;

        private Rectangle n_BackgroundRect;

		private NewLoadingScreen(ScreenManager manager, Screen screen)
		{
			NewLoadingScreen.n_ScreenManager = manager;
			NewLoadingScreen.n_Screen = screen;

            n_BackgroundRect = new Rectangle(0, 0, manager.GraphicsDevice.Viewport.Width, manager.GraphicsDevice.Viewport.Height);

			NewLoadingScreen.n_DoneLoading = false;

			n_BackgroundThread = new Thread(handleSlowLoading);
			n_BackgroundThread.IsBackground = true;
			n_BackgroundThread.Start();
		}

		public static void Load(ScreenManager manager, bool isLoadingSlow, Screen screen)
		{
			screen.ScreenManager = manager;
			screen.ScreenManager.Game.Content = manager.Game.Content;
			screen.ScreenState = ScreenState.Inactive;

			if (isLoadingSlow) {
				NewLoadingScreen loadingScreen = new NewLoadingScreen (manager, screen);

				manager.addScreen(loadingScreen);
			} else {
				manager.addScreen(screen);
			}
		}

		private void handleSlowLoading()
		{
			n_Screen.Initialize();
			n_Screen.LoadContent();
			n_DoneLoading = true;
		}

		private void stop ()
		{
			n_ScreenManager.removeScreen(this);
			n_ScreenManager.addLoadedScreen(n_Screen);

			if (n_BackgroundThread != null) {
				n_BackgroundThread.Join();
			}
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void LoadContent()
		{
			base.LoadContent();

			n_SpriteBatch = n_ScreenManager.SpriteBatch;

			n_LoadingTexture = n_ScreenManager.Game.Content.Load<Texture2D>("Title");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (!n_DoneLoading) {
				//do stuff
				Thread.Sleep(16);
			} else {
				stop ();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw (gameTime);

			n_ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            const string message = "Generating World...";  

            // Center the Loading text in the viewport.  
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = FontManager.Fonts["General"].MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            Vector2 StatusMessageSize = FontManager.Fonts["General"].MeasureString(n_Screen.LoadingMessage);
            Vector2 statusPosition = (viewportSize - StatusMessageSize) / 2;
            statusPosition.Y = statusPosition.Y + 40;

			n_SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

            n_SpriteBatch.Draw(n_LoadingTexture, n_BackgroundRect, Color.DimGray);

            n_SpriteBatch.DrawString(FontManager.Fonts["General"], message, textPosition, Color.White);

            n_SpriteBatch.DrawString(FontManager.Fonts["General"], n_Screen.LoadingMessage, statusPosition, Color.White);

			n_SpriteBatch.End();
		}
	}
}

