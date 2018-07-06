﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2DSpaceShooter
{
    class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficiltyLevel;
        public bool isVisible;
        public List<Bullet> bulletList;

        // Constructor
        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexture;
            bulletTexture = newBulletTexture;
            health = 5;
            position = newPosition;
            currentDifficiltyLevel = 1;
            bulletDelay = 60;
            speed = 5;
            isVisible = true;
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Update Collision Rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            // Update Enemy Movement
            position.Y += speed;

            // Move enemy to top of the screen if he fly's off buttom
            if (position.Y >= 950)
                position.Y = -75;

            EnemyShoot();
            UpdateBullets();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Enemy Ship
            spriteBatch.Draw(texture, position, Color.White);

            // Draw ememy bullets
            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }
        }

        // Update bullet function
        public void UpdateBullets()
        {
            // for each bullet in our bulletList: update the movement and if the bullet hits the top of the screen remove it from the list
            foreach (Bullet b in bulletList)
            {
                // BoundingBox for our every bullet in our bulletList
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);

                // set movement for bullet
                b.position.Y = b.position.Y + b.speed;

                // if bullet hits the top of the screen, then make visible flase
                if (b.position.Y >= 950)
                    b.isVisible = false;
            }

            // Iterate through bulletList and see if any of the bullets are not visible, if they arent then remove that bullet from our bullet list
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisible)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Enemy Shoot Function
        public void EnemyShoot()
        {
            // Shoot inly if bulletdelay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            if(bulletDelay <= 0)
            {
                // Create new bullet and position it front and center of enemy ship
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y + 30);

                newBullet.isVisible = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            }

            // reset bullet delay
            if (bulletDelay == 0)
                bulletDelay = 40;
        }
    }
}
