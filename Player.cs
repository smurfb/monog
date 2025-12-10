using Microsoft.Xna.Framework;

public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Rectangle Hitbox { get; set; }
    public float Speed = 350f;
    public float JumpSpeed = -800f;
    public bool IsOnGround { get; set; }

    public Player()
    {
        Position = Vector2.Zero;
        Velocity = Vector2.Zero;
        Hitbox = Rectangle.Empty;
        IsOnGround = false;
    }
}
