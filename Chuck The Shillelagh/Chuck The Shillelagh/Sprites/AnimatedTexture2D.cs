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
    public class AnimatedTexture2D {        
        public Texture2D[] textures;
        private int numSprites;
        private int interval;

        private int animationCounter = -1;
        private int frameCounter = -1;

        public AnimatedTexture2D(int numSprites, int interval) {
            this.numSprites = numSprites;
            textures = new Texture2D[numSprites];
            this.interval = interval;
        }

        public Texture2D NextFrame() {
            frameCounter = (frameCounter + 1) % interval;
            
            if (frameCounter == 0) {
                animationCounter = (animationCounter + 1) % numSprites;
            }

            return textures[animationCounter];
        }
    }
}
