using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
public class Player
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Rectangle Hitbox { get; set; }
    public float Speed = 750f;
    public float JumpSpeed = -900f;
    public bool IsOnGround;

    public Player()
    {
    }
}