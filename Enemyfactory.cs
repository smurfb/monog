using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace yur;

public static class EnemyFactory
{
    public static Enemy CreateChaser(Vector2 position, Texture2D texture, Player player)
    {
        var enemy = new Enemy(position, texture);
        enemy.Components.Add(new ChaseComponent(100f));
        return enemy;
    }
    
    public static Enemy CreatePatroller(Vector2 position, Texture2D texture, float leftBound, float rightBound)
    {
        var enemy = new Enemy(position, texture);
        enemy.Components.Add(new PatrolComponent(leftBound, rightBound, 80f));
        enemy.Components.Add(new GravityComponent());
        return enemy;
    }
    
    public static Enemy CreateFlyer(Vector2 position, Texture2D texture, Player player)
    {
        var enemy = new Enemy(position, texture);
        enemy.Components.Add(new ChaseComponent(80f));
        // Ingen GravityComponent = kan flyga!
        return enemy;
    }
}