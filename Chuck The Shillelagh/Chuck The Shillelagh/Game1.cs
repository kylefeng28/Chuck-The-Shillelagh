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
        Playing,
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
        public int level = 1;

        // Game objects
        public List<Leprechaun> leps;
        public Weapon weapon;
        public MoodLight mood;
        public Song irishMusic;

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

            // Create list of leprechauns
            leps = new List<Leprechaun>(0);
            SetLevel(level);

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

            foreach (Leprechaun lep in leps) {
                lep.LoadContent(Content);
            }

            weapon.LoadContent(Content);
            irishMusic = Content.Load<Song>("Irish dancing music Reel");

            MediaPlayer.Play(irishMusic);
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

            foreach (Leprechaun lep in leps) {
                lep.Update(this);
            }

            // Find how many leprechauns are knocked out
            int KOcounter = leps.Sum(x => x.KOd ? 1 : 0);

            // All leprechauns are knocked out
            if (KOcounter == leps.Count) {
                level++; // Increment level
                SetLevel(level);
            }

            weapon.Update(this);

            // Collision detection
            foreach (Leprechaun lep in leps) {
                if (weapon.rect.Intersects(lep.rect)) {
                    lep.health -= 1;
                    weapon.state = WeaponState.Aiming;
                }
            }

            // Store states for next frame
            pad1_old = pad1;
            kb_old = kb;

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
            
            foreach (Leprechaun lep in leps) {
                lep.Draw(spriteBatch);
            }

            weapon.Draw(spriteBatch);

            spriteBatch.DrawString(Fonts.Dialog, "Level " + level.ToString(), Vector2.Zero, InvertColor(color));

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected Color InvertColor(Color color) {
            Color invColor = color;
            invColor.R = (byte) (~color.R);
            invColor.G = (byte) (~color.G);
            invColor.B = (byte) (~color.B);
            return invColor;
        }

        protected void AddLeprechaun(Leprechaun lep) {
            leps.Add(lep);
        }

        protected void SetLevel(int level) {
            leps.Clear();
            switch(level) {
            case 1:
                AddLeprechaun(new LeprechaunLevel1());
                break;
            case 2:
                AddLeprechaun(new LeprechaunLevel2());
                AddLeprechaun(new LeprechaunLevel2());
                break;
            case 3:
                AddLeprechaun(new LeprechaunLevel3());
                AddLeprechaun(new LeprechaunLevel3());
                AddLeprechaun(new LeprechaunLevel3());
                break;
            }
            foreach (Leprechaun lep in leps) {
                // lep.position.X = Globals.rnd.Next(Globals.ScreenWidth / 2 - 100, Globals.ScreenWidth / 2 + 100);
                lep.angle = (float) (lep.angle_max * Globals.rnd.NextDouble());
            }
        }
    }
}
