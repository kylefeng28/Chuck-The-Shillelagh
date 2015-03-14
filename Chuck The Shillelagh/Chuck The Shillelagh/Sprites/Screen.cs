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
        public Texture2D bg;
        public Rectangle rect;

        public Screen() {
            rect = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public virtual void LoadContent(ContentManager Content) {
        }

        public virtual void Update(Game1 game) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(bg, rect, Color.White);
        }
    }

    public class TitleScreen : Screen {
        public TitleScreen() {
        }

        public override void LoadContent(ContentManager Content) {
            bg = Content.Load<Texture2D>("TitleText");
            
            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            if (game.kb.IsKeyDown(Keys.Enter) || game.pad1.IsButtonDown(Buttons.Start)) {
                game.state = GameState.Playing;
            }
            
            base.Update(game);
        }
    }
}
