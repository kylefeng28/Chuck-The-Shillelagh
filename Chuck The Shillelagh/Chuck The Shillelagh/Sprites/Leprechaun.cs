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

            base.LoadContent(Content);
        }

        public override void Update(Game1 game) {
            MoveAround(Globals.ScreenWidth / 2 - 100, Globals.ScreenWidth / 2 + 100);



            base.Update(game);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!KOd) {
                texture = anim.NextFrame();
            }

            else {
                // TODO
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


    /*
    switch (state) {
    case LeprechaunState.Level1:
        health = 6;
        velocity_max = 5;
        break;
    case LeprechaunState.Level2:
        health = 5;
        velocity_max = 8;
        break;
    case LeprechaunState.Level3:
        health = 4;
        velocity_max = 10;
        break;
    }
     */

    /* Child classes */
    class LeprechaunLevel1 : Leprechaun
    {
        public LeprechaunLevel1()
        {
            health = 6;
            velocity_max = 5;  
        }
    }
    class LeprechaunLevel2 : Leprechaun
    {
        public LeprechaunLevel2()
        {
            health = 5;
            velocity_max = 8;
        }
    }
    class LeprechaunLevel3 : Leprechaun
    {
        public LeprechaunLevel3()
        {
            health = 4;
            velocity_max = 10;
        }
    }

}
