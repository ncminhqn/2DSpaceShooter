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

        // Asteroid List
        List<Asteroid> asteroidList = new List<Asteroid>();

        // Instantiating our Player and Starfield objects
        Player player = new Player();
        Starfield starfield = new Starfield();
        //Asteroid asteroid = new Asteroid();

        
        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            this.Window.Title = "2D Space Shooter";
            Content.RootDirectory = "Content";

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

            //asteroid.LoadContent(Content);
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

            // TODO: Add your update logic here
            
            // foreach asteroid in our asteroidList, Update it
            foreach(Asteroid a in asteroidList)
            {
                a.Update(gameTime);
            }

            

            player.Update(gameTime);
            starfield.Update(gameTime);
            LoadAsteroids();

            base.Update(gameTime);
        }
        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            
            starfield.Draw(spriteBatch);
            player.Draw(spriteBatch);

            foreach (Asteroid a in asteroidList)
            {
                a.Draw(spriteBatch);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        // Load Asteroids
        public void LoadAsteroids()
        {
            // Creating random variable for our X and Y axis of our asteroids
            int randX = random.Next(0, 750);
            int randY = random.Next(-600, -50);

            // if there are less than 5 asteroids on the screen, then create more until there is 5 again
            if (asteroidList.Count() < 6)
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
    }
}
