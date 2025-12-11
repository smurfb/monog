using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace yur;

// component, jaga spelaren, chaser
public class ChaseComponent : IEnemyComponent
{
    public float Speed { get; set; }
    
    public ChaseComponent(float speed = 100f)
    {
        Speed = speed;
    }
    
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        // Jagar endast i X-led
        if (player.Position.X < enemy.Position.X)
            enemy.Velocity = new Vector2(-Speed, enemy.Velocity.Y);
        else if (player.Position.X > enemy.Position.X)
            enemy.Velocity = new Vector2(Speed, enemy.Velocity.Y);
    }
}

//  patroller, går fram å tillbaka som en döing
public class PatrolComponent : IEnemyComponent
{
    public float LeftBound { get; set; }
    public float RightBound { get; set; }
    public float Speed { get; set; }
    private bool _movingRight = true;
    
    public PatrolComponent(float leftBound, float rightBound, float speed = 80f)
    {
        LeftBound = leftBound;
        RightBound = rightBound;
        Speed = speed;
    }
    
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        // Vänd vid gränserna
        if (enemy.Position.X >= RightBound)
            _movingRight = false;
        else if (enemy.Position.X <= LeftBound)
            _movingRight = true;
        
        enemy.Velocity = new Vector2(_movingRight ? Speed : -Speed, enemy.Velocity.Y);
    }
}



public class GravityComponent : IEnemyComponent
{
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        enemy.Velocity = new Vector2(enemy.Velocity.X, enemy.Velocity.Y + gravity * dt);
        
        var futureHitbox = new Rectangle(
            (int)(enemy.Position.X + enemy.Velocity.X * dt),
            (int)(enemy.Position.Y + enemy.Velocity.Y * dt),
            enemy.Hitbox.Width,
            enemy.Hitbox.Height
        );
        
        foreach (var platform in platforms)
        {
            if (futureHitbox.Intersects(platform))
            {
                if (enemy.Velocity.Y > 0 && enemy.Hitbox.Bottom <= platform.Top + 5)
                {
                    enemy.Position = new Vector2(
                        enemy.Position.X,
                        platform.Top - enemy.Texture.Height
                    );
                    enemy.Velocity = new Vector2(enemy.Velocity.X, 0);
                }
            }
        }
    }
}

public class StationaryComponent : IEnemyComponent
{
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        enemy.Velocity = Vector2.Zero;
    }
}