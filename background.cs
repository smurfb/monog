using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Background
{
    public Texture2D Texture;
    public Vector2 Position;
    public float Speed;

    public Background(Texture2D texture, float speed)
    {
        Texture = texture;
        Speed = speed;
    }
}