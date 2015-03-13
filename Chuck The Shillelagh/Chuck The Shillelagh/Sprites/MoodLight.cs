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
    public class MoodLight {
        private bool redIncrease = false;
        private bool greenIncrease = false;
        private bool blueIncrease = false;

        private byte redIntensity = 0;
        private byte greenIntensity = 0;
        private byte blueIntensity = 0;

        public Color color {
            get {
                return NextColor();
            }
        }

        public MoodLight(byte redIntensity, byte greenIntensity, byte blueIntensity) {
            this.redIntensity = redIntensity;
            this.greenIntensity = greenIntensity;
            this.blueIntensity = blueIntensity;
        }

        public MoodLight(Color initColor) {
            this.redIntensity = initColor.R;
            this.greenIntensity = initColor.G;
            this.blueIntensity = initColor.B;
        }
            
        public Color NextColor() {
            if (redIncrease) { redIntensity++; } else { redIntensity--; }
            if (greenIncrease) { greenIntensity++; } else { greenIntensity--; }
            if (blueIncrease) { blueIntensity++; } else { blueIntensity--; }

            if (redIntensity == 255) { redIncrease = false; }
            else if (redIntensity == 0) { redIncrease = true; }

            if (greenIntensity == 255) { greenIncrease = false; }
            else if (greenIntensity == 0) { greenIncrease = true; }

            if (blueIntensity == 255) { blueIncrease = false; }
            else if (blueIntensity == 0) { blueIncrease = true; }

            return new Color(redIntensity, greenIntensity, blueIntensity);
        }
    }
}
