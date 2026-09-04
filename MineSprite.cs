using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Minescape.Collisions;

namespace Minescape
{

    public class MineSprite
    {
        private const float ANIMATION_SPEED = 0.1f;

        private double animationTimer;

        private int animationFrame; 
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                bounds.Center = position;
                sightBounds.Center = position;
            }
        }

        private Texture2D texture;
        private BoundingCircle sightBounds;
        /// <summary>
        /// The bounds of the light of sight for the mine
        /// </summary>
        public BoundingCircle SightBounds => sightBounds;

        private BoundingCircle bounds;
        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingCircle Bounds => bounds;

        public bool Collected {get; set;} = false;


        /// <summary>
        /// Creates a new coin sprite
        /// </summary>
        /// <param name="position">The position of the sprite in the game</param>
        public MineSprite(Vector2 position)
        {
            this.bounds = new BoundingCircle(position, 9);
            this.sightBounds = new BoundingCircle(position, 16 * 5);
            this.Position = position;
        }

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Mine");
        }

        /// <summary>
        /// Draws the animated sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Collected) return;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if(animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }

            var source = new Rectangle(animationFrame * 32, 0, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White, 0f, new Vector2(16, 16), 1f, SpriteEffects.None, 0f);
        }
    }
}
