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
        
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int speed;
        public float bulletDelay;
        public Rectangle boundingBox;
        public bool isColliding;
        private List<Bullet> bulletList;


        // Constructor
        public Player()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(300, 300);
            bulletDelay = 5;
            speed = 10;
            isColliding = false;
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("ship");
            bulletTexture = Content.Load<Texture2D>("bullet");
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw ship
            spriteBatch.Draw(texture, position, Color.White);
            foreach (Bullet b in bulletList)
                b.Draw(spriteBatch);
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Getting Keyboard State
            KeyboardState keyState = Keyboard.GetState();

            // Fire Bullet
            if(keyState.IsKeyDown(Keys.Space))
            {
                Shoot();
            }

            UpdateBullets();

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

        // Shoot Method: used to set starting position of out bullets
        public void Shoot()
        {
            // Shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            // If bulletDelay is at 0: create new bullet at player position, make it visible on the screen, then add that bullet to the List
            if (bulletDelay <= 0)
            {
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 32 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            // reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 5;
        }

        // Update bullet function
        public void UpdateBullets()
        {
            // for each bullet in our bulletList: update the movement and if the bullet hits the top of the screen remove it from the list
            foreach (Bullet b in bulletList)
            {
                // set movement for bullet
                b.position.Y = b.position.Y - b.speed;

                // if bullet hits the top of the screen, then make visible flase
                if (b.position.Y <= 0)
                    b.isVisible = false;
            }

            // Iterate through bulletList and see if any of the bullets are not visible, if they arent then remove that bullet from our bullet list
            for(int i = 0; i < bulletList.Count; i++)
            {
                if(!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
