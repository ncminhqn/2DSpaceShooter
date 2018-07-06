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

namespace _2DSpaceShooter
{
    // Main 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;

        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();

        // Instantiating our Player and Starfield objects
        Player player = new Player();
        Starfield starfield = new Starfield();
        HUD hud = new HUD();

        
        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = "2D Space Shooter";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 1;
        }
        
        // Init
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        
        // Load Content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            hud.LoadContent(Content);
            player.LoadContent(Content);
            starfield.LoadContent(Content);

            
        }
        
        // Unload Content
        protected override void UnloadContent()
        {
            
        }
        

        // Update
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Updating Enemy's and checking collision of enemyShip to playerShip
            foreach(Enemy e in enemyList)
            {
                // Check if enemyShip is colliding with player
                if (e.boundingBox.Intersects(player.boundingBox))
                {
                    player.life -= 1;
                    e.isVisible = false;
                }

                // Check enemy bullet collision with player ship
                for(int i = 0; i < e.bulletList.Count; i++)
                {
                    if(player.boundingBox.Intersects(e.bulletList[i].boundingBox))
                    {
                        player.life -= enemyBulletDamage;
                        e.bulletList[i].isVisible = false;
                    }
                }

                // Check player bullet collision to enemy ship
                for (int i = 0; i < player.BulletList.Count; i++)
                {
                    if(player.BulletList[i].boundingBox.Intersects(e.boundingBox))
                    {
                        hud.playerScore += 10;
                        player.BulletList[i].isVisible = false;
                        e.isVisible = false;
                    }
                }

                e.Update(gameTime);
            }
            
            // foreach asteroid in our asteroidList, update and check for collision
            foreach(Asteroid a in asteroidList)
            {
                // Check to see if any of the asteroids are colliding with our playership, if they are.. set isVisible to False(remove them from the asteroidList)
                if(a.boundingBox.Intersects(player.boundingBox))
                {
                    player.life -= 1;
                    a.isVisible = false;
                }

                // Interate through our bulletList if any asteroids come in contacts with these bullets, destroy bullet and asteroid
                for (int i = 0; i < player.BulletList.Count; i++ )
                {
                    if(a.boundingBox.Intersects(player.BulletList[i].boundingBox))
                    {
                        hud.playerScore += 5;
                        a.isVisible = false;
                        player.BulletList.ElementAt(i).isVisible = false;
                    }
                }

                a.Update(gameTime);
            }


            //hud.Update(gameTime);
            player.Update(gameTime);
            starfield.Update(gameTime);
            LoadAsteroids();
            LoadEnemies();

            base.Update(gameTime);
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            // background
            starfield.Draw(spriteBatch);
            
            // Asteroids
            foreach (Asteroid a in asteroidList)
            {
                a.Draw(spriteBatch);
            }

            // PlayerShip, Life
            player.Draw(spriteBatch);

            // EnemiesShip
            foreach(Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }

            // HUD: score
            hud.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // Load Asteroids
        public void LoadAsteroids()
        {
            // Creating random variable for our X and Y axis of our asteroids
            int randX = random.Next(0, 750);
            int randY = random.Next(-600, -50);

            // if there are less than 5 asteroids on the screen, then create more until there is 5 again
            if (asteroidList.Count() < 5)
            {
                asteroidList.Add(new Asteroid(Content.Load<Texture2D>("asteroids"), new Vector2(randX, randY)));
            }

            // if any of the asteroids in the list were destroyed(or invisible), then remove them from the list
            for (int i = 0; i < asteroidList.Count;i++)
            {
                if(!asteroidList[i].isVisible)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }
        }

        // Load Enemy Function
        public void LoadEnemies()
        {
            // Creating random variable for our X and Y axis of our asteroids
            int randX = random.Next(0, 750);
            int randY = random.Next(-600, -50);

            // if there are less than 5 enemies on the screen, then create more until there is 5 again
            if (enemyList.Count() < 5)
            {
                enemyList.Add(new Enemy(Content.Load<Texture2D>("enemy"), new Vector2(randX, randY), Content.Load<Texture2D>("enemybullet")));
            }

            // if any of the enemies in the list were destroyed(or invisible), then remove them from the list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisible)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
