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
    public class Leprechaun : Sprite {
        public static AnimatedTexture2D anim;
        public static Texture2D healthbar;
        public static Texture2D texture_fainted;

        public bool KOd = false;
        public int health;

        public int direction = 1;
        public float angle = 0f;
        public float angle_max = (float) (Math.PI / 4);

        public Leprechaun() {
            position.X = Globals.ScreenWidth / 2;
            position.Y = 100;
            scale = 0.5f;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content) {
            anim = new AnimatedTexture2D(4, 10);
            for (int i = 0; i < anim.textures.Length; i++) {
                anim.textures[i] = Content.Load<Texture2D>("leprechaun" + (i + 1));
            }

            healthbar = Content.Load<Texture2D>("pixel");
            texture_fainted = Content.Load<Texture2D>("LepFainted");

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {

            if (!KOd) {
                // MoveBetween(Globals.ScreenWidth / 2 - 100, Globals.ScreenWidth / 2 + 100);
                ArcBetween(new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight - 100),
                           300, angle_max);
            }

            else {
            }

            if (health <= 0) {
                KOd = true;
            }
            
            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!KOd) {
                texture = anim.NextFrame();
            }

            else {
                texture = texture_fainted;
            }

            Rectangle health_rect = new Rectangle(rect.X, rect.Y, 10 * health, 10);
            spriteBatch.Draw(healthbar, health_rect, Color.Green);
            
            base.Draw(spriteBatch);
        }

        public void MoveBetween(int a, int b) {
            position.X += velocity_max * direction;
            if (position.X <= a || position.X >= b) {
                direction *= -1;
            }
        }

        public void ArcBetween(Vector2 center, float radius, float angle1, float angle2) {
            angle += velocity_max / 200 * direction;

            position.X = (float) (center.X + radius * Math.Sin(angle));
            position.Y = (float) (center.Y - radius * Math.Cos(angle));

            if (angle < angle1 || angle > angle2) {
                direction *= -1;
            }
        }

        public void ArcBetween(Vector2 center, float radius, float angleAbs) {
            ArcBetween(center, radius, -angleAbs, angleAbs);
        }
    }

    /* Child classes */
    public class LeprechaunLevel1 : Leprechaun {
        public LeprechaunLevel1() {
            health = 6;
            velocity_max = 5;
        }
    }
    public class LeprechaunLevel2 : Leprechaun {
        public LeprechaunLevel2() {
            health = 5;
            velocity_max = 8;
        }
    }
    public class LeprechaunLevel3 : Leprechaun {
        public LeprechaunLevel3() {
            health = 4;
            velocity_max = 10;
        }
    }
}
