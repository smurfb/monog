using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace yur;

// Interface för alla komponenter
public interface IEnemyComponent
{
    void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player);
}

// Huvudklassen för alla fiender
public class Enemy
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Rectangle Hitbox { get; set; }
    public Texture2D Texture { get; set; }
    public int Health { get; set; }
    
    // Lista med komponenter som fienden har
    public List<IEnemyComponent> Components { get; set; } = new();
    
    public Enemy(Vector2 position, Texture2D texture)
    {
        Position = position;
        Texture = texture;
        Velocity = Vector2.Zero;
        Health = 100;
        UpdateHitbox();
    }
    
    public void Update(float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        // Kör alla komponenter
        foreach (var component in Components)
        {
            component.Update(this, dt, platforms, gravity, player);
        }
        
        // Applicera velocity
        Position += Velocity * dt;
        
        UpdateHitbox();
    }
    
    private void UpdateHitbox()
    {
        Hitbox = new Rectangle(
            (int)Position.X,
            (int)Position.Y,
            Texture.Width,
            Texture.Height
        );
    }
    
    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition)
    {
        var screenPos = new Vector2(
            Position.X - cameraPosition.X,
            Position.Y - cameraPosition.Y
        );
        
        spriteBatch.Draw(Texture, screenPos, Color.White);
    }
}