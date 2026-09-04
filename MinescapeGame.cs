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
    
    private MineSprite[] mines;
    private SubSprite slimeGhost;
    private SpriteFont spriteFont;
    private FlagSprite flagSprite;

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
        slimeGhost = new SubSprite();
        flagSprite = new FlagSprite(new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 100, (float)rand.NextDouble() * GraphicsDevice.Viewport.Height - 100));

        base.Initialize();
    }

    /// <summary>
    /// Loads content for the game
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        foreach (var coin in mines) coin.LoadContent(Content);
        slimeGhost.LoadContent(Content);
        flagSprite.LoadContent(Content);
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
        slimeGhost.Update(gameTime);
        slimeGhost.Color = Color.White;
        //Detect and process collisions
        foreach(var mine in mines)
        {
            if(!mine.Collected &&  mine.Bounds.CollidesWith(slimeGhost.Bounds))
            {
                slimeGhost.Color = Color.Red;
                //mine.Collected = true;
                //slimeGhost.Health--;
                if(slimeGhost.Health == 0)
                {
                    slimeGhost.Explode();
                }
            }
            if (mine.SightBounds.CollidesWith(slimeGhost.Bounds))
            {
                //slimeGhost.Color = Color.Blue;
                //mine.Position += (slimeGhost.Posision - mine.Position)/50;
            }
            
        }
        if(flagSprite.Bounds.CollidesWith(slimeGhost.Bounds))
        {
            slimeGhost.Color = Color.Yellow;
            foreach(var mine in mines) mine.Collected = true;
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
        slimeGhost.Draw(gameTime, spriteBatch);
        flagSprite.Draw(gameTime, spriteBatch);
        spriteBatch.DrawString(spriteFont, $"Ship Health: {slimeGhost.Health}", new Vector2(2,2), Color.Gold);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}}

