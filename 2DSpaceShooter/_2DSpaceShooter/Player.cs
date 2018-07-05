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
    public class Player
    {
        
        public Texture2D texture;
        public Vector2 position;
        public int speed;

        // Collision Variables
        public Rectangle boundingBox;
        public bool isColliding;

        // Constructor
        public Player()
        {
            texture = null;
            position = new Vector2(300, 300);
            speed = 10;
            isColliding = false;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship");
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw ship
            spriteBatch.Draw(texture, position, Color.White);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Getting Keyboard State
            KeyboardState keyState = Keyboard.GetState();

            // Ship Controls
            // Up
            if (keyState.IsKeyDown(Keys.W))
                position.Y = position.Y - speed;
            // Down
            if (keyState.IsKeyDown(Keys.A))
                position.X = position.X - speed;
            // Left
            if (keyState.IsKeyDown(Keys.S))
                position.Y = position.Y + speed;
            // Right
            if (keyState.IsKeyDown(Keys.D))
                position.X = position.X + speed;

            // keep Player Ship In Screen Bounds
            // minX = 0
            if (position.X <= 0)
                position.X = 0;
            // minX = maxX.ScreenBound - ship.Width
            if (position.X >= 800 - texture.Width)
                position.X = 800 - texture.Width;


            // minY = 0
            if (position.Y <= 0)
                position.Y = 0;
            // maxY = maxY.ScreenBound - ship.Height
            if (position.Y >= 950 - texture.Height)
                position.Y = 950 - texture.Height;
        }
    }
}
