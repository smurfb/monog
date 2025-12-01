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
    public Vector2 DropVelocity { get; set; }
    static void Droppings (Texture2D texture, List<Vector2> drop, Rectangle hitbox, Vector2 position)
    {
        
        for (int i = 0; i < drop.Count; i++)
        {
            drop[i] = new Vector2(drop[i].X, drop[i].Y + 5); //gravity effect
            Rectangle dropHitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            // Here you would typically draw the dropping using the texture and dropHitbox
        }
    }
    
    public Bird(Vector2 position, Rectangle hitbox, Texture2D texture) : base(position, hitbox, texture)
    {
        
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
