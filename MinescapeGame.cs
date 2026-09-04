using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minescape.Collisions;
namespace Minescape
{
public class MinescapeGame : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private bool gameWon = false;
    private bool gameLost = false;
    
    private MineSprite[] mines;
    private SubSprite sub;
    private SpriteFont spriteFont;
    private FlagSprite flagSprite;
    private HammerSprite hammerSprite;

    /// <summary>
    /// A game demonstrating collision detection
    /// </summary>
    public MinescapeGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    /// <summary>
    /// Initializes the game 
    /// </summary>
    protected override void Initialize()
    {
       
        System.Random rand = new System.Random();
        //makes mines and adds them randomly to the map
        mines = new MineSprite[]
        {
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height)),
            new MineSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height))
        };
        sub = new SubSprite();
        flagSprite = new FlagSprite(new Vector2((float)rand.NextDouble() * (GraphicsDevice.Viewport.Width - 100), (float)rand.NextDouble() * (GraphicsDevice.Viewport.Height - 100)));
        hammerSprite = new HammerSprite(new Vector2((float)rand.NextDouble() * (GraphicsDevice.Viewport.Width - 100), (float)rand.NextDouble() * (GraphicsDevice.Viewport.Height - 100)));
        base.Initialize();
    }

    /// <summary>
    /// Loads content for the game
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        foreach (var coin in mines) coin.LoadContent(Content);
        sub.LoadContent(Content);
        flagSprite.LoadContent(Content);
        hammerSprite.LoadContent(Content);
        spriteFont = Content.Load<SpriteFont>("arial");
    }

    /// <summary>
    /// Updates the game world
    /// </summary>
    /// <param name="gameTime">The game time</param>
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        sub.Update(gameTime);
        sub.Color = Color.White;
        //Detect and process collisions
        foreach(var mine in mines)
        {
            //get rid of mines that are at the location of the sub spawn
            if(!sub.DoneWithBound && mine.Bounds.CollidesWith(sub.DespawnBounds))
            {
                mine.Collected = true;
            }
            //Checks to see if the mine has hit the sub
            if(!mine.Collected &&  mine.Bounds.CollidesWith(sub.Bounds))
            {
                mine.Collected = true;
                sub.Health--;
                if(sub.Health == 0)
                {
                    sub.Explode();
                    foreach(var minee in mines) minee.Collected = true;
                    gameLost = true;
                }
            }
            //Checks to see if the Sub is within the sight bounds of the mine
            if (mine.SightBounds.CollidesWith(sub.Bounds))
            {
                mine.Position += (sub.Posision - mine.Position)/50;

            }
            
        }
        sub.DoneWithBound = true;
        //Checks to see if the flag has been hit
        if(flagSprite.Bounds.CollidesWith(sub.Bounds))
        {
            foreach(var mine in mines) mine.Collected = true;
            gameWon = true;
        }
        //Checks to see if the hammer has been hit
        if (hammerSprite.Bounds.CollidesWith(sub.Bounds) && !hammerSprite.Collected)
        {
            hammerSprite.Collected = true;
            sub.Health++;
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// Draws the game world
    /// </summary>
    /// <param name="gameTime">The game time</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        foreach (var mine in mines) mine.Draw(gameTime, spriteBatch);
        sub.Draw(gameTime, spriteBatch);
        flagSprite.Draw(gameTime, spriteBatch);
        hammerSprite.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(spriteFont, $"Ship Health: {sub.Health}", new Vector2(2,2), Color.Gold);
        
        spriteBatch.DrawString(spriteFont, "Reach The Flag to Win", new Vector2(450,2), Color.Gold);
        if(gameLost) spriteBatch.DrawString(spriteFont, "You Lost.", new Vector2(325,200), Color.Gold);
        if(gameWon) spriteBatch.DrawString(spriteFont, "You Win!!!", new Vector2(320, 200), Color.Gold);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
}

