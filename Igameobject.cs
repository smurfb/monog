using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace yur;

// intereface för gameobjekct


public interface IGameObject
{
    Vector2 Position { get; set; }
    Rectangle Hitbox { get; set; }
    bool IsActive { get; set; }  
    
    void Update(float dt, Rectangle playerHitbox, ref Player player);
    void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, Texture2D pixel);
}
