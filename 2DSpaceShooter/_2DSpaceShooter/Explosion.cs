using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DSpaceShooter
{
    class Explosion
    {
        public Texture2D texture;
        public Vector2 position;
        public float timer;
        public float interval;
        public Vector2 origin;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRectangle;
        public bool isVisible;

        // Constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 15f;
            currentFrame = 1;
            spriteWidth = 64;
            spriteHeight = 64;
            isVisible = true;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Increase the timer by the number of milliseconds since update was last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check the timer is more than the chosen interval
            if (timer > interval)
            {
                // Show next frame
                currentFrame++;
                // Reser Timer
                timer = 0f;
            }

            // If were on the last frame, make the explosion invisible and reset currentFrame to beginning of spritesheet
            if (currentFrame == 10)
            {
                isVisible = false;
                currentFrame = 0;
            }

            sourceRectangle = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // if visible then draw
            if(isVisible == true)
            {
                spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}
