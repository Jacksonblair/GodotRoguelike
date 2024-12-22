using Godot;

public partial class PlayerCharacter : CharacterBody2D
{
    float speed = 200;
    string current_terrain;

    CollisionShape2D collision;
    AnimatedSprite2D sprite;
    ClosestEnemyGetter closestEnemyGetter;
    Timer getEnemyTimer;
    Area2D NPCArea2D;

    bool aimPressed;

    // Nearby NPC
    IInteractable nearbyNPC;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GlobalVariables.Instance.Character = this;
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        sprite.Play();
        closestEnemyGetter = GetNode<ClosestEnemyGetter>("ClosestEnemyGetter");
        getEnemyTimer = GetNode<Timer>("GetEnemyTimer");
        getEnemyTimer.Timeout += FireProjectile;

        NPCArea2D = GetNode<Area2D>("NPCArea2D");
        NPCArea2D.AreaEntered += onNPCAreaEntered;
        NPCArea2D.AreaExited += onNPCAreaExited;
    }

    private void onNPCAreaExited(Area2D area)
    {
        nearbyNPC = null;
    }

    private void onNPCAreaEntered(Node2D body)
    {
        nearbyNPC = (IInteractable)body;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        // Get input and adjust velocity.
        // TODO: Move to handler
        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }
        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionJustPressed("interact"))
        {
            nearbyNPC?.Interact();
        }

        aimPressed = Input.IsActionPressed("aim");

        /**
            If a character's input or other factors affect velocity, normalizing it and applying speed means the character’s movement remains consistent regardless of input strength.
            So, even if velocity varies, the actual speed won’t change—only the direction will.
        */
        if (velocity.Length() > 0)
        {
            sprite.Animation = "run";

            // Update sprite based on velocity
            if (velocity.X != 0)
            {
                sprite.FlipH = velocity.X < 0;
            }

            velocity = velocity.Normalized() * speed;
        }
        else
        {
            sprite.Animation = "idle";
        }

        var label = GetNode<Label>("Label");
        label.Text = Position.ToString();

        Velocity = velocity;
        MoveAndSlide();
    }

    public void FireProjectile()
    {
        if (aimPressed)
        {
            FireProjectileAtMouse();
        }
        else
        {
            FireProjectileAtClosestEnemy();
        }
    }

    public void FireProjectileAtClosestEnemy()
    {
        // If aiming, fire at mouse, otherwise, fire at closest enemy
        var closestEnemy = closestEnemyGetter.GetClosestEnemy();
        if (closestEnemy != null)
        {
            // GD.Print("FIRE AT CLOSEST ENEMY");
            FireProjectile(closestEnemy.Position);
        }
        else
        {
            // GD.Print("NO CLOSEST ENEMY");
        }
    }

    public void FireProjectileAtMouse()
    {
        // GD.Print("FIRE AT MOUSE");
        var level = GlobalVariables.Instance.ActiveMainScene;
        var mousePos = level.GetLocalMousePosition();
        FireProjectile(mousePos);
    }

    public void FireProjectile(Vector2 target)
    {
        var level = GlobalVariables.Instance.ActiveMainScene;
        var projectile = (PackedScene)GD.Load("res://scenes/projectiles/Projectile.tscn");
        var inst2 = projectile.Instantiate<BasicProjectile>();

        Vector2 direction = (target - Position).Normalized();
        inst2.Position = Position;

        // GD.Print(inst2.Position);

        inst2.Init(new TESTCS.scenes.projectiles.LinearProjectileMover(), direction);
        level?.CallDeferred("add_child", inst2);

        // GD.Print(inst2.Position.ToString());
        // var mousePos = level.GetLocalMousePosition();
        // Vector2 direction = (mousePos - Position).Normalized();
        // ((Node2D)inst).Position += mousePos;
    }

    public void GetInput() { }
}
