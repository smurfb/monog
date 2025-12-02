using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json;

namespace yur;
public class RectangleData
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
public class RectanglesFile
{
    public List<RectangleData> Rectangles { get; set; }
}


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D picture;

    private List<Rectangle> platforms = new();
    private Texture2D pixel;
    private float gravity = 1200f;
    public int groundy;

    Player player = new();

    private float worldwidth = 3000f;
    private Vector2 cameraPosition;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1000;
        _graphics.PreferredBackBufferHeight = 700;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        var viewport = GraphicsDevice.Viewport;
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.Azure });
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        picture = Content.Load<Texture2D>("mushroom sprite (2)");
  


        player.Position = new Vector2(
            100f,
            viewport.Height /2f - picture.Height/2f
            
        );

        groundy = viewport.Height - picture.Height;

        string json = System.IO.File.ReadAllText("data.json");
        RectanglesFile data = JsonSerializer.Deserialize<RectanglesFile>(json);

        if(data != null)
        {
            for(int i = 0; i < data.Rectangles.Count; i++)
            {
                RectangleData rect = data.Rectangles[i];
                platforms.Add(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height));
            }
        }
       /* 
        platforms.Add(new Rectangle(550,450,200,20));
        platforms.Add(new Rectangle(300,350,200,20));
        platforms.Add(new Rectangle(100,550,200,20));
        platforms.Add(new Rectangle(750,250,200,20)); */

        cameraPosition = Vector2.Zero;
        
        // TODO: use this.Content to load your game content here
    }
    

    protected override void Update(GameTime gameTime)
    {

        var viewport = GraphicsDevice.Viewport;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(keyboard.IsKeyDown(Keys.A))
        {
            player.Position.X -= player.Speed * dt;
        }
        if(keyboard.IsKeyDown(Keys.D))
        {
            player.Position.X += player.Speed * dt;
        }

        player.Velocity.Y += gravity * dt;

        if(keyboard.IsKeyDown(Keys.Space) && player.IsOnGround)
        {
            player.Velocity.Y = player.JumpSpeed;
            player.IsOnGround = false;
        }

        float targetCamX = player.Position.X - viewport.Width / 2f;



        player.Position += player.Velocity * dt;

        Rectangle playerHitbox = new Rectangle(
        (int)player.Position.X,
        (int)player.Position.Y,
        picture.Width,
        picture.Height
        );

        foreach (var platform in platforms)
        {
            if (playerHitbox.Intersects(platform))
            {
                // Check if falling onto platform
                if (player.Velocity.Y > 0 &&
                    playerHitbox
                    .Bottom - player.Velocity.Y * dt <= platform.Top)
                {
                    player.Position.Y = platform.Top - picture.Height;
                    player.Velocity.Y = 0;
                    player.IsOnGround = true;
                }
            }
        }

        if(player.Position.Y >= groundy)
        {
            player.Position.Y = groundy;
            player.Velocity.Y = 0;
            player.IsOnGround = true;
        } 

        // TODO: Add your update logic here

        player.Position.X = MathHelper.Clamp(player.Position.X, -worldwidth - picture.Width, worldwidth - picture.Width);
        
        cameraPosition.X = targetCamX;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        foreach(var platform in platforms)
        {
            var screenRect = new Rectangle((int)(platform.X - cameraPosition.X), platform.Y, platform.Width, platform.Height);
            _spriteBatch.Draw(pixel, screenRect, Color.Azure); 
        }

        var playerScreenPosition = new Vector2(
        player.Position.X - cameraPosition.X,
        player.Position.Y - cameraPosition.Y
        );

        _spriteBatch.Draw(picture, playerScreenPosition, null, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0f);


        _spriteBatch.End();
        // TODO: Add your drawing code here



        base.Draw(gameTime);
    }
}
