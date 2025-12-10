using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace yur;

// Komponent: Jaga spelaren
public class ChaseComponent : IEnemyComponent
{
    public float Speed { get; set; }
    
    public ChaseComponent(float speed = 100f)
    {
        Speed = speed;
    }
    
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        // Jaga endast på X-axeln
        if (player.Position.X < enemy.Position.X)
            enemy.Velocity = new Vector2(-Speed, enemy.Velocity.Y);
        else if (player.Position.X > enemy.Position.X)
            enemy.Velocity = new Vector2(Speed, enemy.Velocity.Y);
    }
}

// Komponent: Patrullera mellan två punkter
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

// Komponent: Hoppa om spelaren är nära
public class JumpWhenCloseComponent : IEnemyComponent
{
    public float JumpForce { get; set; }
    public float DetectionRange { get; set; }
    public float Cooldown { get; set; }
    private float _timer = 0f;
    private bool _isOnGround = true;
    
    public JumpWhenCloseComponent(float jumpForce = -400f, float detectionRange = 200f, float cooldown = 2f)
    {
        JumpForce = jumpForce;
        DetectionRange = detectionRange;
        Cooldown = cooldown;
    }
    
    public void Update(Enemy enemy, float dt, List<Rectangle> platforms, float gravity, Player player)
    {
        _timer -= dt;
        
        float distance = Vector2.Distance(enemy.Position, player.Position);

        if (distance < DetectionRange && _timer <= 0 && _isOnGround)
        {
            enemy.Velocity = new Vector2(enemy.Velocity.X, JumpForce);
            _timer = Cooldown;
            _isOnGround = false;
        }
        
        if (enemy.Velocity.Y >= 0)
            _isOnGround = true;
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