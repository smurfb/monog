using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace yur;

public class Player
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Rectangle Hitbox { get; set; }

    public float Speed = 350f;
    public float JumpSpeed = -800f;
    
    private float _baseSpeed = 350f;
    private float _baseJumpSpeed = -800f;

    public bool IsOnGround { get; set; }
    public bool HasDoubleJump { get; set; } = false;
    public bool IsInvincible { get; set; } = false;
    
    private bool _hasUsedDoubleJump = false;
    
    // Powerups aktivierade
    private Dictionary<PowerUpType, float> _activePowerUps = new();

    public Player()
    {
        Position = Vector2.Zero;
        Velocity = Vector2.Zero;
        Hitbox = Rectangle.Empty;
        IsOnGround = false;
    }
    
    // Lägger till powerup som har en duration
    public void AddPowerUp(PowerUpType type, float duration)
    {
        _activePowerUps[type] = duration;
    }
    
    // updated som köra i våran update
    public void Update(float dt)
    {
        // Uppdatera power-up timers
        var expiredPowerUps = new List<PowerUpType>();
        
        foreach (var powerUp in _activePowerUps)
        {
            _activePowerUps[powerUp.Key] -= dt;
            
            if (_activePowerUps[powerUp.Key] <= 0)
            {
                expiredPowerUps.Add(powerUp.Key);
            }
        }
        
        // Ta bort där duration expired
        foreach (var expired in expiredPowerUps)
        {
            RemovePowerUp(expired);
        }
        
        // ta bort 
        if (IsOnGround)
        {
            _hasUsedDoubleJump = false;
        }
    }
    
    //ta bort powerups
    private void RemovePowerUp(PowerUpType type)
    {
        _activePowerUps.Remove(type);
        
        switch (type)
        {
            case PowerUpType.JumpBoost:
                JumpSpeed = _baseJumpSpeed;
                break;
                
            case PowerUpType.SpeedBoost:
                Speed = _baseSpeed;
                break;
                
            case PowerUpType.DoubleJump:
                HasDoubleJump = false;
                break;
                
            case PowerUpType.Invincibility:
                IsInvincible = false;
                break;
        }
    }
    
    // är dubbeljump aktiverat?
    public bool CanDoubleJump()
    {
        return HasDoubleJump && !IsOnGround && !_hasUsedDoubleJump;
    }
    
    // dubbel
    public void UseDoubleJump()
    {
        _hasUsedDoubleJump = true;
    }
    
    // Kolla om viss är aktiv
    public bool HasActivePowerUp(PowerUpType type)
    {
        return _activePowerUps.ContainsKey(type);
    }
    
    // Få kvarvarande duration
    public float GetPowerUpTimeRemaining(PowerUpType type)
    {
        return _activePowerUps.ContainsKey(type) ? _activePowerUps[type] : 0f;
    }
}