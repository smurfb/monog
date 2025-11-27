using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Danger
{
    public Vector2 Position { get; set; }
    public Texture2D Texture {get; set; }
    public Rectangle Hitbox { get; set; }

    public Danger(Vector2 position, Rectangle hitbox, Texture2D texture)
    {
        Position = position;
        Texture = texture;
        Hitbox = hitbox;
    }

}

public class Bird : Danger
{
    public List<Vector2> PatrolPoints { get; set; } //velocity for droppings
    public Vector2 Velocity { get; set; }
    public Bird(Vector2 position, Rectangle hitbox, Texture2D texture) : base(position, hitbox, texture)
    {
        PatrolPoints = new List<Vector2>();
    }
}

public class Spike : Danger
{
    public Spike(Vector2 position, Rectangle hitbox, Texture2D texture) : base(position, hitbox, texture)
    {
    }
}

public class Chaser : Danger
{
    public float Speed { get; set; }

    public Chaser(Vector2 position, Rectangle hitbox, Texture2D texture, float speed) : base(position, hitbox, texture)
    {
        Speed = speed;
    }
}
