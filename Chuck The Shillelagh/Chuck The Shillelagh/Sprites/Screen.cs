using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chuck_The_Shillelagh {
    public abstract class Screen {
        protected Texture2D bg;
        protected Rectangle bgRect;

        public Screen() {
            bgRect = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public virtual void LoadContent(ContentManager Content) {
        }

        public virtual void Update(Game1 game) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            if (bg != null) {
                spriteBatch.Draw(bg, bgRect, Color.White);
            }
        }
    }

    public class TitleScreen : Screen {
        protected Texture2D titleText;
        protected Vector2 titlePos;
        protected string text;
        protected Vector2 textPos;

        public override void LoadContent(ContentManager Content) {
            titleText = Content.Load<Texture2D>("TitleText");
            titlePos = new Vector2(bgRect.Center.X - titleText.Width / 2, 20);

            text = "Press start to begin";
            textPos = new Vector2(bgRect.Center.X - Fonts.Dialog.MeasureString(text).X,
                                  bgRect.Center.Y - Fonts.Dialog.MeasureString(text).Y);

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            if (game.kb.IsKeyDown(Keys.Enter) || game.pad1.IsButtonDown(Buttons.Start) && game.pad1_old.IsButtonUp(Buttons.Start)) {
                game.state = GameState.Playing;
            }
            
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(titleText, titlePos, Color.White);

            spriteBatch.DrawString(Fonts.Dialog, text, textPos, Color.White);
            
            base.Draw(spriteBatch);
        }
    }

    public class PlayingScreen : Screen {
        public Color color = Color.White;

        public override void LoadContent(ContentManager Content) {
            bg = Content.Load<Texture2D>("Background");
            
            base.LoadContent(Content);
        }

        public new void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(bg, bgRect, color);
        }
    }

    public class GameWonScreen : Screen {
        protected Texture2D text;
        protected Vector2 textPos;

        protected Texture2D potOfGold;
        protected Vector2 potOfGoldPos;

        public override void LoadContent(ContentManager Content) {
            text = Content.Load<Texture2D>("GameWonText");
            textPos = new Vector2(bgRect.Center.X - text.Width / 2, 20);

            potOfGold = Content.Load<Texture2D>("Pot O Gold");
            potOfGoldPos = new Vector2(bgRect.Center.X - potOfGold.Width / 2, 160);

            base.LoadContent(Content);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(text, textPos, Color.White);
            spriteBatch.Draw(potOfGold, potOfGoldPos, Color.White);

            base.Draw(spriteBatch);
        }
    }

    public class GameLostScreen : Screen {
        protected Texture2D text;
        protected Vector2 textPos;

        public override void LoadContent(ContentManager Content) {
            text = Content.Load<Texture2D>("GameLostText");
            textPos = new Vector2(bgRect.Center.X - text.Width / 2, 20);

            base.LoadContent(Content);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(text, textPos, Color.White);

            base.Draw(spriteBatch);
        }
    }
}
