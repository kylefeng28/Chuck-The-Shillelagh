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
        protected Rectangle rect;

        public Screen() {
            rect = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public virtual void LoadContent(ContentManager Content) {
        }

        public virtual void Update(Game1 game) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            if (bg != null) {
                spriteBatch.Draw(bg, rect, Color.White);
            }
        }
    }

    public class TitleScreen : Screen {
        protected Texture2D text;
        protected Vector2 textPos;

        public override void LoadContent(ContentManager Content) {
            text = Content.Load<Texture2D>("TitleText");
            textPos = new Vector2(rect.Center.X - text.Width / 2, 20);

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            if (game.kb.IsKeyDown(Keys.Enter) || game.pad1.IsButtonDown(Buttons.Start)) {
                game.state = GameState.Playing;
            }
            
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(text, textPos, Color.White);
            
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
            spriteBatch.Draw(bg, rect, color);
        }
    }

    public class GameWonScreen : Screen {
        protected Texture2D text;
        protected Vector2 textPos;

        protected Texture2D potOfGold;
        protected Vector2 potOfGoldPos;

        public override void LoadContent(ContentManager Content) {
            text = Content.Load<Texture2D>("GameWonText");
            textPos = new Vector2(rect.Center.X - text.Width / 2, 20);

            potOfGold = Content.Load<Texture2D>("Pot O Gold");
            potOfGoldPos = new Vector2(rect.Center.X - potOfGold.Width / 2, 160);

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
            textPos = new Vector2(rect.Center.X - text.Width / 2, 20);

            base.LoadContent(Content);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(text, textPos, Color.White);

            base.Draw(spriteBatch);
        }
    }
}
