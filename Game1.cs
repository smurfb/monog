using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace yur;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D picture;

    private List<Rectangle> platforms = new();
    private Texture2D pixel;
    private Vector2 playerPosition; 
    private Vector2 playerVelocity;
    private float movespeed = 200f;
    private float gravity = 1200f;
    private float jumpspeed = -800f;
    private float groundy;
    private bool isonground = false;
    private bool isonplatform = false;

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
        picture = Content.Load<Texture2D>("front-bracer");


        playerPosition = new Vector2(
            100f,
            viewport.Height /2f - picture.Height/2f
            
        );

        groundy = viewport.Height - picture.Height;

        platforms.Add(new Rectangle(550,450,200,20));
        platforms.Add(new Rectangle(300,350,200,20));
        platforms.Add(new Rectangle(100,550,200,20));
        platforms.Add(new Rectangle(750,250,200,20));

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
            playerPosition.X -= movespeed * dt;
        }
        if(keyboard.IsKeyDown(Keys.D))
        {
            playerPosition.X += movespeed * dt;
        }

        playerVelocity.Y += gravity * dt;

        if(keyboard.IsKeyDown(Keys.Space) && isonground)
        {
            playerVelocity.Y = jumpspeed;
            isonground = false;
        }

        float targetCamX = playerPosition.X - viewport.Width / 2f;



        playerPosition += playerVelocity * dt;

        Rectangle playerRect = new Rectangle(
        (int)playerPosition.X,
        (int)playerPosition.Y,
        picture.Width,
        picture.Height
        );

        foreach (var platform in platforms)
        {
            if (playerRect.Intersects(platform))
            {
                // Check if falling onto platform
                if (playerVelocity.Y > 0 &&
                    playerRect.Bottom - playerVelocity.Y * dt <= platform.Top)
                {
                    playerPosition.Y = platform.Top - picture.Height;
                    playerVelocity.Y = 0;
                    isonground = true;
                    isonplatform = true;
                }
            }
        }

        if(playerPosition.Y >= groundy)
        {
            playerPosition.Y = groundy;
            playerVelocity.Y = 0;
            isonground = true;
        }

        // TODO: Add your update logic here

        playerPosition.X = MathHelper.Clamp(playerPosition.X, -worldwidth - picture.Width, worldwidth - picture.Width);
        
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
        playerPosition.X - cameraPosition.X,
        playerPosition.Y - cameraPosition.Y
        );

        _spriteBatch.Draw(picture, playerScreenPosition, Color.White);


        _spriteBatch.End();
        // TODO: Add your drawing code here



        base.Draw(gameTime);
    }
}
