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
    public enum WeaponState {
        Aiming,
        Moving,
    }

    public class Weapon : Sprite {
        public AnimatedTexture2D anim;
        public Texture2D pixel;
        public WeaponState state = WeaponState.Aiming;

        public Vector2 fulcrum;
        public float angle = 0.0f;

        public Weapon() {
            scale = 1 / 20f;
            velocity_max = 10;

            fulcrum = new Vector2(Globals.ScreenWidth / 2,
                                  Globals.ScreenHeight - 20);
            ResetPosition();
        }

        public override void LoadContent(ContentManager Content) {
            anim = new AnimatedTexture2D(1, 10);
            for (int i = 0; i < anim.textures.Length; i++) {
                anim.textures[i] = Content.Load<Texture2D>("circle");
            }

            pixel = Content.Load<Texture2D>("pixel");

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            switch (state) {
            case WeaponState.Aiming:
                MoveWithKeyboard(game.kb);
                MoveWithGamePad(game.pad1);

                position.X = (float) (fulcrum.X + 50 * Math.Sin(angle));
                position.Y = (float) (fulcrum.Y - 50 * Math.Cos(angle));

                if (game.kb.IsKeyDown(Keys.Space)) {
                    state = WeaponState.Moving;
                }
                break;
            case WeaponState.Moving:

                position.X += (float) (3 * velocity_max * Math.Sin(angle));
                position.Y -= (float) (3 * velocity_max * Math.Cos(angle));

                if (position.Y < 0 || position.X < 0 || position.X > Globals.ScreenWidth) {
                    state = WeaponState.Aiming;
                    ResetPosition();
                }
                break;
            }
            
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            texture = anim.NextFrame();
            
            // Draw weapon
            int width = (int) (texture.Width * scale);
            int height = (int) (texture.Height * scale);

            int x = (int) (fulcrum.X - width / 2);
            int y = (int) (fulcrum.Y - height / 2);

            rect = new Rectangle(x, y, width, height);

            spriteBatch.Draw(texture, rect, Color.White);

            // Draw target point
            Rectangle rect_target = new Rectangle((int) position.X - 5, (int) position.Y - 5,
                                                   10, 10);
            spriteBatch.Draw(pixel, rect_target, Color.Black);
        }

        public override void MoveWithGamePad(GamePadState pad1) {
            // TODO
            base.MoveWithGamePad(pad1);
        }

        public override void MoveWithKeyboard(KeyboardState kb) {
            if (kb.IsKeyDown(Keys.D)) {
                angle += 0.1f;
            }
            if (kb.IsKeyDown(Keys.A)) {
                angle -= 0.1f;
            }
        }

        public void ResetPosition() {
            angle = 0;
        }

    }
}
