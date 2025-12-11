using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace yur;

public class Teleporter : IGameObject
{
    public Vector2 Position { get; set; }
    public Vector2 Destination { get; set; }
    public Rectangle Hitbox { get; set; }
    public bool IsActive { get; set; } = true;
    
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
    
    public void Update(float dt, Rectangle playerHitbox, ref Player player)
    {
        if (!IsActive) return;
        
        if (_cooldown > 0)
            _cooldown -= dt;
        
        if (playerHitbox.Intersects(Hitbox) && _cooldown <= 0)
        {
            player.Position = Destination;
            _cooldown = 0.5f;
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Texture2D pixel)
    {
        if (!IsActive) return;
        
        var screenRect = new Rectangle(
            (int)(Position.X - cameraPosition.X),
            (int)(Position.Y - cameraPosition.Y),
            Hitbox.Width,
            Hitbox.Height
        );
        
        spriteBatch.Draw(pixel, screenRect, Color.Purple);
    }
}
