using Godot;

// TODO: Derive SkillCooldownManager from a base CooldownManager class
public partial class SkillCooldownManager : Node
{
    [Signal] public delegate void StartedCooldownEventHandler(float timeLeft);
    [Signal] public delegate void NumberChargesChangedEventHandler(int numCharges);

    private Skill _skill;
    private int _currentCharges;
    private float _cooldownTimer;

    public override void _Process(double delta)
    {
        if (_currentCharges < _skill.NumCharges)
        {
            _cooldownTimer -= (float)delta;
            if (_cooldownTimer <= 0f)
            {
                _currentCharges++;
                _cooldownTimer = _skill.CooldownTime;
                EmitSignal(nameof(NumberChargesChangedEventHandler), _currentCharges);
                GD.Print($"Charge restored. Current charges: {_currentCharges}");
            }
        }
    }
    
    public override void _Ready()
    {
        this._skill = GetParent<Skill>();
        this._currentCharges = _skill.NumCharges;
        _cooldownTimer = 0f;
    }

    public bool TryUseCharge()
    {
        GD.Print("Charges available: ", this._currentCharges, "/", this._skill.NumCharges);
        
        if (_currentCharges > 0)
        {
            GD.Print("Used charge");
            _currentCharges--;
            EmitSignal(nameof(NumberChargesChangedEventHandler), _currentCharges);

            if (_currentCharges < _skill.NumCharges && _cooldownTimer <= 0f)
            {
                _cooldownTimer = _skill.CooldownTime;
                GD.Print("Charge restoring in: ", _cooldownTimer);
                EmitSignal(nameof(StartedCooldownEventHandler), _cooldownTimer);
            }
            return true;
        }
        GD.Print("Could not use charge");

        return false;
    }

    public void ResetCharges()
    {
        _currentCharges = _skill.NumCharges;
        _cooldownTimer = 0f;
    }
}