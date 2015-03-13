using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chuck_The_Shillelagh {
    public enum GameState {
        TitleScreen,
        Level1,
        Level2,
        Level3,
        Paused,
        GameLost,
        GameWon,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game state machine
        // GameState gameState = GameState.TitleScreen;
        public GameState state = GameState.Level1;

        // Game objects
        public Leprechaun lep1, lep2, lep3;
        public Weapon weapon;
        public MoodLight mood;
        public Song Irish;

        // I/O states
        public GamePadState pad1, pad1_old;
        public KeyboardState kb, kb_old;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            Globals.ScreenWidth = GraphicsDevice.Viewport.Width;
            Globals.ScreenHeight = GraphicsDevice.Viewport.Height;

            lep1 = new LeprechaunLevel1();
            lep2 = new LeprechaunLevel2();
            lep3 = new LeprechaunLevel3();
            weapon = new Weapon();
            mood = new MoodLight(77, 126, 249);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Fonts.LoadContent(Content);
            lep1.LoadContent(Content);
            weapon.LoadContent(Content);
            Irish = Content.Load<Song>("Irish dancing music Reel");

            MediaPlayer.Play(Irish);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Get current states
            pad1 = GamePad.GetState(PlayerIndex.One);
            kb = Keyboard.GetState();
            
            // Allows the game to exit
            if (pad1.Buttons.Back == ButtonState.Pressed) {
                this.Exit();
            }

            lep1.Update(this);
            weapon.Update(this);

            // Store states for next frame
            pad1_old = pad1;
            kb_old = kb;

            if (weapon.rect.Intersects(lep1.rect))
            {
                lep1.health -= 1;
                weapon.state = WeaponState.Aiming;
            }
                

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            Color color = mood.color;
            // Color color = mood.NextColor();

            GraphicsDevice.Clear(color);

            spriteBatch.Begin();
            lep1.Draw(spriteBatch);
            weapon.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
