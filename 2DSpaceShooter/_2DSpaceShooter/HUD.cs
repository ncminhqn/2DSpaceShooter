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
    public class HUD
    {
        public int playerScore, screenWidth, screenHeight;
        public SpriteFont playerScoreFont;
        public Vector2 playerScorePos;
        public bool showHud;

        // Constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            screenHeight = 950;
            screenWidth = 800;
            playerScoreFont = null;
            playerScorePos = new Vector2(screenWidth - 200, 7);
        }

        // Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("georgia");
        }

        // Update
        public void Update(GameTime gameTime)
        {
            // Get Keyboard state
            KeyboardState keyState = Keyboard.GetState();
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // If we are showing our HUD (if showHUD == true) then display our HUD
            if (showHud)
                spriteBatch.DrawString(playerScoreFont, "Score : " + playerScore, playerScorePos, Color.Red);

        }
    }
}
