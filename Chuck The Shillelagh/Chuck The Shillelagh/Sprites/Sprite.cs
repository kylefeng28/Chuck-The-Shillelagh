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
    public abstract class Sprite {
        public Texture2D texture;
        public Rectangle rect;
        public float scale = 1;

        public Vector2 position = Vector2.Zero;
        public float velocity_max = 1;
        
        public virtual void LoadContent(ContentManager Content) {
        }

        public virtual void Update(Game1 game) {
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            int width = (int) (texture.Width * scale);
            int height = (int) (texture.Height * scale);

            rect = new Rectangle((int) position.X - width / 2, (int) position.Y - height / 2,
                                 width, height);

            spriteBatch.Draw(texture, rect, Color.White);
        }

        public virtual void MoveWithKeyboard(KeyboardState kb) {
            if (kb.IsKeyDown(Keys.D)) {
                position.X += velocity_max;
            }

            if (kb.IsKeyDown(Keys.A)) {
                position.X -= velocity_max;
            }

            if (kb.IsKeyDown(Keys.W)) {
                position.Y -= velocity_max;
            }

            if (kb.IsKeyDown(Keys.S)) {
                position.Y += velocity_max;
            }
        }

        public virtual void MoveWithGamePad(GamePadState pad1) {
            position.X += pad1.ThumbSticks.Left.X;
            position.Y += pad1.ThumbSticks.Left.Y;
        }
    }
}
