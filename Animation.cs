using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UnrivaledPractise
{
    public class Animation
    {
        public int CurrentFrame { get; set; }
        public int FrameCount { get; private set; }
        public int FrameHeight { get { return Texture.Height; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public bool isLooping { get; set; }
        public Texture2D Texture { get; private set; }
        public Animation(Texture2D texture, int frameCount, bool IsLooping)
        {
            Texture = texture;
            FrameCount = frameCount;
            isLooping = IsLooping;
            FrameSpeed = 0.1f;
        }
    }
}
