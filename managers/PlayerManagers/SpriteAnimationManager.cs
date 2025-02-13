using Godot;

namespace TESTCS.managers.PlayerManagers;

public partial class SpriteAnimationManager : GodotObject
{
    private AnimatedSprite2D _sprite;
    public AnimatedSprite2D Sprite => _sprite;
    private bool _isLocked = false;
    private string _currentAnimation = "";
    
    [Signal] public delegate void AnimationInterruptedEventHandler();
    [Signal] public delegate void AnimationFinishedEventHandler();
    
    public SpriteAnimationManager(AnimatedSprite2D sprite)
    {
        _sprite = sprite;
        _sprite.AnimationFinished += OnAnimationFinished;
    }
    
    public void PlayAnimation(string animationName, bool lockAnimation = false)
    {
        if (_isLocked) return;
        if (!_sprite.SpriteFrames.HasAnimation(animationName)) return;
        
        _currentAnimation = animationName;
        _sprite.Play(animationName);

        if (lockAnimation)
        {
            _isLocked = true;
        }
    }
    
    public void UnlockAnimation()
    {
        _isLocked = false;
    }

    public void InterruptAnimation()
    {
        UnlockAnimation();
        EmitSignal(nameof(AnimationInterrupted));
    }

    private void OnAnimationFinished()
    {
        UnlockAnimation();
        EmitSignal(nameof(AnimationFinished));
    }
}