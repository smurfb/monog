using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace yur;

public class Teleporter
{
    public Vector2 Position { get; set; }
    public Vector2 Destination { get; set; }
    public Rectangle Hitbox { get; set; }
    
    private float _cooldown = 0f;
    
    public Teleporter(Vector2 position, Vector2 destination, int width, int height)
    {
        Position = position;
        Destination = destination;
        Hitbox = new Rectangle(
            (int)position.X,
            (int)position.Y,
            width,
            height
        );
    }
    
    public void Update(float dt, Rectangle playerHitbox, ref Vector2 playerPosition)
    {
        if (_cooldown > 0)
            _cooldown -= dt;
        
        if (playerHitbox.Intersects(Hitbox) && _cooldown <= 0)
        {
            playerPosition = Destination;
            _cooldown = 0.5f; 
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Texture2D pixel)
    {
        var screenRect = new Rectangle(
            (int)(Position.X - cameraPosition.X),
            (int)(Position.Y - cameraPosition.Y),
            Hitbox.Width,
            Hitbox.Height
        );
        
        spriteBatch.Draw(pixel, screenRect, Color.Purple);
    }
}