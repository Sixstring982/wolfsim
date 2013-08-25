using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WolfSim
{
    class Animation
    {
        private int ticks = 0;
        private int maxTicks;
        private IAsset[] assets;
        private int currentAsset;
        private bool flipped;

        public Animation(IAsset[] assets, int changeTicks, bool flipped = false)
        {
            this.assets = assets;
            this.maxTicks = changeTicks;
            this.flipped = flipped;
        }

        public void Render(SpriteBatch sb, Vector2 location)
        {
            ticks++;
            if (ticks > maxTicks)
            {
                currentAsset++;
                if (currentAsset == assets.Length)
                {
                    currentAsset = 0;
                }
                ticks = 0;
            }

            sb.Draw(AssMan.Get(assets[currentAsset]), location, null, Color.White, 0.0f, Vector2.Zero, 1.0f, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
        }
    }
}