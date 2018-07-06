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
        // State Enum
        public enum State
        {
            Menu,
            Playing, 
            Gameover
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        public int enemyBulletDamage;
        public Texture2D menu;
        public Texture2D gameover;

        // Lists
        List<Asteroid> asteroidList = new List<Asteroid>();
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();

        // Instantiating our Player and Starfield objects
        Player player = new Player();
        Starfield starfield = new Starfield();
        HUD hud = new HUD();

        // Set first State
        State gameState = State.Menu;
        
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
            menu = null;
            gameover = null;
        }
        
        // Init
        protected override void Initialize()
        {
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
            menu = Content.Load<Texture2D>("menu");
            gameover = Content.Load<Texture2D>("gameover");
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

            // Updating playing state
            switch (gameState)
            {
                case State.Playing:
                    {
                        // Updating Enemy's and checking collision of enemyShip to playerShip
                        starfield.speed = 5;
                        foreach (Enemy e in enemyList)
                        {
                            // Check if enemyShip is colliding with player
                            if (e.boundingBox.Intersects(player.boundingBox))
                            {
                                player.life -= 1;
                                e.isVisible = false;
                            }

                            // Check enemy bullet collision with player ship
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (player.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    player.life -= enemyBulletDamage;
                                    e.bulletList[i].isVisible = false;
                                }
                            }

                            // Check player bullet collision to enemy ship
                            for (int i = 0; i < player.BulletList.Count; i++)
                            {
                                if (player.BulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(e.position.X, e.position.Y)));
                                    hud.playerScore += 10;
                                    player.BulletList[i].isVisible = false;
                                    e.isVisible = false;
                                }
                            }

                            e.Update(gameTime);
                        }

                        // Update Explosions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }

                        // foreach asteroid in our asteroidList, update and check for collision
                        foreach (Asteroid a in asteroidList)
                        {
                            // Check to see if any of the asteroids are colliding with our playership, if they are.. set isVisible to False(remove them from the asteroidList)
                            if (a.boundingBox.Intersects(player.boundingBox))
                            {
                                player.life -= 1;
                                a.isVisible = false;
                            }

                            // Interate through our bulletList if any asteroids come in contacts with these bullets, destroy bullet and asteroid
                            for (int i = 0; i < player.BulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(player.BulletList[i].boundingBox))
                                {
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(a.position.X, a.position.Y)));
                                    hud.playerScore += 5;
                                    a.isVisible = false;
                                    player.BulletList.ElementAt(i).isVisible = false;
                                }
                            }

                            a.Update(gameTime);
                        }


                        //hud.Update(gameTime);

                        // If playerlife hits 0 then go to gameover state
                        if (player.life <= 0)
                            gameState = State.Gameover;

                        player.Update(gameTime);
                        starfield.Update(gameTime);
                        ManageExplosions();
                        LoadAsteroids();
                        LoadEnemies();
                        break;
                    }
                    // Updating menu state
                case State.Menu:
                    {
                        // Get Keyboard State
                        KeyboardState keyState = Keyboard.GetState();

                        if(keyState.IsKeyDown(Keys.Space))
                        {
                            gameState = State.Playing;
                        }
                        starfield.Update(gameTime);
                        starfield.speed = 1;
                        break;
                    }
                    // Updating gameover state
                case State.Gameover:
                    {
                        // Get Keyboard State
                        KeyboardState keyState = Keyboard.GetState();

                        // If in the gameover screen and user hits "Escape" key, Return to the main menu
                        if (keyState.IsKeyDown(Keys.Escape))
                        {
                            player.position = new Vector2(400, 800);
                            enemyList.Clear();
                            asteroidList.Clear();
                            player.life = 10;
                            hud.playerScore = 0;
                            gameState = State.Menu;
                        }
                        starfield.Update(gameTime);
                        starfield.speed = 1;
                        break;
                    }
            }
            
            base.Update(gameTime);
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            switch(gameState)
            {
                // Drawing playing state
                case State.Playing:
                    {
                        // background
                        starfield.Draw(spriteBatch);

                        // Asteroids
                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);
                        }

                        // PlayerShip, Life
                        player.Draw(spriteBatch);

                        // EnemiesShip
                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }

                        // HUD: score
                        hud.Draw(spriteBatch);

                        break;
                    }

                    // Drawing menu state
                case State.Menu:
                    {
                        starfield.Draw(spriteBatch);
                        spriteBatch.Draw(menu, new Vector2(0, 0), Color.White);
                        break;
                    }

                    // Drawing gameover state
                case State.Gameover:
                    {
                        starfield.Draw(spriteBatch);
                        spriteBatch.Draw(gameover, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Your Final Score: " + hud.playerScore.ToString(), new Vector2(240, 400), Color.Red);
                        break;
                    }
            }
            
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

        // Manage Explosions
        public void ManageExplosions()
        {
            for(int i = 0; i < explosionList.Count; i++)
            {
                if(!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
