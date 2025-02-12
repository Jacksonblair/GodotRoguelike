using System.Threading.Tasks;
using Godot;
using TESTCS.actors;

namespace TESTCS.helpers.Commands;

public partial class AnimatedCommand : Command
{
    private AnimatedSprite2D _sprite;
    private string _expectedAnimation;
    private int _triggerFrame;
    private System.Action _onTrigger;
    private bool _triggered = false;

    public AnimatedCommand(AnimatedSprite2D sprite, string animationName, int triggerFrame, System.Action onTrigger)
    {
        _sprite = sprite;
        _expectedAnimation = animationName;
        _triggerFrame = triggerFrame;
        _onTrigger = onTrigger;
    }

    public override async Task Execute()
    {
        GD.Print("EXECUTED COMMANDYPOOS");
        
        _sprite.Play(_expectedAnimation);
        _sprite.FrameChanged += OnFrameChanged;
        _sprite.AnimationChanged += OnAnimationChanged; 
        _sprite.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished()
    {
        Cleanup();
    }

    private void OnFrameChanged()
    {
        // If the animation changed before the desired frame, cancel the action
        if (_sprite.Animation != _expectedAnimation)
        {
            Cleanup();
            return;
        }

        // Apply effect if the correct frame is reached
        if (!_triggered && _sprite.Frame == _triggerFrame)
        {
            _triggered = true;
            _onTrigger?.Invoke();
        }
    }

    private void OnAnimationChanged()
    {
        // If the animation changes unexpectedly, cancel execution
        if (_sprite.Animation != _expectedAnimation)
        {
            Cleanup();
        }
    }

    private void Cleanup()
    {
        _sprite.FrameChanged -= OnFrameChanged;
        _sprite.AnimationChanged -= OnAnimationChanged;
        _sprite.AnimationFinished -= OnAnimationFinished;
        EmitSignal(nameof(CommandFinished));
    }
}