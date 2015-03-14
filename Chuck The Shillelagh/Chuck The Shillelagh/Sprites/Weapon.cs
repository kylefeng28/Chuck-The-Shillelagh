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

        public Vector2 position_center;
        public Rectangle rect_center;
        public float angle = 0.0f;

        public Weapon() {
            scale = 1 / 10f;
            velocity_max = 30;
            ResetPosition();
        }

        public override void LoadContent(ContentManager Content) {
            anim = new AnimatedTexture2D(1, 10);
            for (int i = 0; i < anim.textures.Length; i++) {
                anim.textures[i] = Content.Load<Texture2D>("Shillelagh1");
            }

            pixel = Content.Load<Texture2D>("pixel");

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            switch (state) {
            case WeaponState.Aiming:
                MoveWithKeyboard(game.kb);
                MoveWithGamePad(game.pad1);

                // Bounds checking
                if (position_center.X < 0) {
                    position_center.X = 0;
                }
                if (position_center.X > Globals.ScreenWidth) {
                    position_center.X = Globals.ScreenWidth;
                }

                if (Math.Abs(angle) > Math.PI / 2) {
                    angle = (float) (Math.Sign(angle) * Math.PI / 2);
                }

                // Update shillelagh position
                position.X = (float) (rect_center.Center.X + 50 * Math.Sin(angle));
                position.Y = (float) (rect_center.Center.Y - 50 * Math.Cos(angle));

                // Change state
                if ((game.kb.IsKeyDown(Keys.Space)) || (game.pad1.Triggers.Right > .5)) {
                    state = WeaponState.Moving;
                }
                break;

            case WeaponState.Moving:
                position.X += (float) (velocity_max * Math.Sin(angle));
                position.Y -= (float) (velocity_max * Math.Cos(angle));

                if (position.Y < 0 || position.X < 0 || position.X > Globals.ScreenWidth) {
                    state = WeaponState.Aiming;
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

            int x = (int) (position.X - width / 2);
            int y = (int) (position.Y - height / 2);

            rect = new Rectangle(x, y, width, height);

            spriteBatch.Draw(texture, rect, Color.White);

            // Draw center 
            rect_center = new Rectangle((int) position_center.X - 5,
                                        (int) position_center.Y - 5,
                                         10, 10);

            spriteBatch.Draw(pixel, rect_center, Color.Black);
        }

        public override void MoveWithGamePad(GamePadState pad1) {
            if (pad1.ThumbSticks.Left.X > 0) {
                position_center.X += pad1.ThumbSticks.Left.X * velocity_max / 3;
            }
            if (pad1.ThumbSticks.Left.X < 0) {
                position_center.X += pad1.ThumbSticks.Left.X * velocity_max / 3;
            }
            if (pad1.ThumbSticks.Right.X > 0) {
                angle += pad1.ThumbSticks.Right.X * 0.1f;
            }
            if (pad1.ThumbSticks.Right.X < 0) {
                angle += pad1.ThumbSticks.Right.X * 0.1f;
            }
        }

        public override void MoveWithKeyboard(KeyboardState kb) {
            if (kb.IsKeyDown(Keys.D)) {
                angle += 0.1f;
            }
            if (kb.IsKeyDown(Keys.A)) {
                angle -= 0.1f;
            }
            if (kb.IsKeyDown(Keys.Right)) {
                position_center.X += velocity_max / 3;
            }
            if (kb.IsKeyDown(Keys.Left)) {
                position_center.X -= velocity_max / 3;
            }
        }

        public void ResetPosition() {
            position_center = new Vector2(Globals.ScreenWidth / 2,
                                          Globals.ScreenHeight - 20);
            angle = 0;
        }

    }
}
