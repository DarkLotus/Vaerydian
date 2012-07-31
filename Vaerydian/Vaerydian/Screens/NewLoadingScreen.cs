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

		//private static bool n_IsLoadingSlow;

		private static bool n_DoneLoading = false;

		private Thread n_BackgroundThread;

		private static SpriteBatch n_SpriteBatch;

		private NewLoadingScreen (ScreenManager manager, Screen screen)
		{
			NewLoadingScreen.n_ScreenManager = manager;
			NewLoadingScreen.n_Screen = screen;

			NewLoadingScreen.n_DoneLoading = false;

			Thread n_BackgroundThread = new Thread(handleSlowLoading);
			n_BackgroundThread.IsBackground = true;
			n_BackgroundThread.Start();

		}

		public static void Load (ScreenManager manager, bool isLoadingSlow, Screen screen)
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
			n_ScreenManager.removeScreen (this);
			n_ScreenManager.addLoadedScreen (n_Screen);

			if (n_BackgroundThread != null) {
				n_BackgroundThread.Join();
			}
		}


		public override void Initialize ()
		{
			base.Initialize ();
		}

		public override void LoadContent ()
		{
			base.LoadContent ();

			n_SpriteBatch = n_ScreenManager.SpriteBatch;
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (!n_DoneLoading) {
				//do stuff
				Thread.Sleep(16);
			} else {
				stop ();
			}
		}

		public override void Draw (GameTime gameTime)
		{
			base.Draw (gameTime);

			n_ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

			n_SpriteBatch.Begin();

			n_SpriteBatch.DrawString(FontManager.Fonts["General"], n_Screen.LoadingMessage,new Vector2(384,240),Color.White);

			n_SpriteBatch.End();
		}
	}
}

