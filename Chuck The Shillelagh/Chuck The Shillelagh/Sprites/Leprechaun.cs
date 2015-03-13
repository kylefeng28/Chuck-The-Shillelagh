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
        public AnimatedTexture2D anim;
        public Texture2D healthbar;
        public Texture2D LepFaint;

        public bool KOd = false;
        public int health;

        public int direction = 1;

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
            LepFaint = Content.Load<Texture2D>("LepFainted");

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {

            if (!KOd) {
                MoveAround(Globals.ScreenWidth / 2 - 100, Globals.ScreenWidth / 2 + 100);
            }
            else { }

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
                texture = LepFaint;
            }

            Rectangle health_rect = new Rectangle(rect.X, rect.Y, 10 * health, 10);
            spriteBatch.Draw(healthbar, health_rect, Color.Green);
            
            base.Draw(spriteBatch);
        }

        public void MoveAround(int a, int b) {
            position.X += velocity_max * direction;
            if (position.X < a || position.X > b) {
                direction *= -1;
            }
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
