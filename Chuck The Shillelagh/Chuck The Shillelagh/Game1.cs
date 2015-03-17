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
        GameWon,
        GameLost,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game state machine
        public GameState state = GameState.TitleScreen;
        public int level = 1;
        public const int level_max = 3;

        // Screens
        public TitleScreen titleScreen;
        public PlayingScreen playingScreen;
        public GameLostScreen gameLostScreen;
        public GameWonScreen gameWonScreen;

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

            // Initialize screens
            titleScreen = new TitleScreen();
            playingScreen = new PlayingScreen();
            gameLostScreen = new GameLostScreen();
            gameWonScreen = new GameWonScreen();
            mood = new MoodLight(77, 126, 249);

            // Create list of leprechauns
            leps = new List<Leprechaun>(0);
            weapon = new Shillelagh();

            SetLevel(level);

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

            titleScreen.LoadContent(Content);
            playingScreen.LoadContent(Content);
            gameLostScreen.LoadContent(Content);
            gameWonScreen.LoadContent(Content);

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
            if (pad1.Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape)) {
                this.Exit();
            }

            switch (state) {
            case GameState.TitleScreen:
                titleScreen.Update(this);
                break;
                

            case GameState.Playing:
                foreach (Leprechaun lep in leps) {
                    lep.Update(this);
                }

                // Player ran out of ammo
                if (weapon.ammo <= 0) {
                    state = GameState.GameLost;
                }

                // Find how many leprechauns are knocked out
                int KOcounter = leps.Sum(x => x.KOd ? 1 : 0);

                // All leprechauns are knocked out
                if (KOcounter == leps.Count) {
                    IncreaseLevel();
                }

                weapon.Update(this);

                // Collision detection
                foreach (Leprechaun lep in leps) {
                    if (weapon.rect.Intersects(lep.rect)) {
                        lep.health -= 1;
                        weapon.state = WeaponState.Aiming;
                    }
                }
                break;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (state) {
            case GameState.TitleScreen:
                titleScreen.Draw(spriteBatch);
                break;

            case GameState.Playing:
                playingScreen.Draw(spriteBatch);

                // Level indicator
                spriteBatch.DrawString(Fonts.Dialog, "Level " + level.ToString(), Vector2.Zero, Color.White);

                // Rainbow mode
                if (level >= 3) {
                    playingScreen.color = mood.color; // Color color = mood.NextColor();
                }

                foreach (Leprechaun lep in leps) {
                    lep.Draw(spriteBatch);
                }

                weapon.Draw(spriteBatch);

                break;

            case GameState.GameLost:
                gameLostScreen.Draw(spriteBatch);
                break;

            case GameState.GameWon:
                gameWonScreen.Draw(spriteBatch);
                break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void AddLeprechaun(Leprechaun lep) {
            leps.Add(lep);
        }

        protected void SetLevel(int level) {
            leps.Clear();
            switch(level) {
            case 1:
                AddLeprechaun(new LeprechaunLevel1());
                leps[0].angle = 0f;
                weapon.ammo = 8;
                break;
            case 2:
                AddLeprechaun(new LeprechaunLevel2());
                AddLeprechaun(new LeprechaunLevel2());
                leps[0].angle = -leps[0].angle_max;
                leps[1].angle = leps[0].angle_max;
                weapon.ammo = 12;
                break;
            case 3:
                AddLeprechaun(new LeprechaunLevel3());
                AddLeprechaun(new LeprechaunLevel3());
                AddLeprechaun(new LeprechaunLevel3());
                leps[0].angle = -0.5f * leps[0].angle_max;
                leps[1].angle = 0f;
                leps[2].angle = 0.5f * leps[0].angle_max;
                weapon.ammo = 15;
                break;
            }
            /*
            foreach (Leprechaun lep in leps) {
                // lep.position.X = Globals.rnd.Next(Globals.ScreenWidth / 2 - 100, Globals.ScreenWidth / 2 + 100);
            }
            */
        }

        protected void IncreaseLevel() {
            if (level < level_max) {
                level++;
                SetLevel(level);
            }
            else {
                state = GameState.GameWon;
            }
        }

        // TODO
        int dialogTimer = 0;
        protected void DisplayDialog(string msg, int t) {
            
        }
    }
}
