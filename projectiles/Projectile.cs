    using Godot;
using TESTCS.scenes.projectiles;

namespace TESTCS.projectiles;

public partial class Projectile : Area2D
{
    public SpriteFrames ProjectileAnimation;
    public SpriteFrames ProjectileCollisionAnimation;
    protected IProjectileMover Mover = new LinearProjectileMover();
    public float Speed { get; set; }
    public float TimeToLive { get; set; }
    public float Lifetime { get; set; }
    public int Damage { get; set; }
    public int Weight { get; set; }
    public Vector2 InitialDirection { get; set; }
    public AnimatedSprite2D ProjectileSprite { get; set; }
    public AnimatedSprite2D CollisionSprite { get; set; }
    public bool ExplodeOnCollision { get; set; } = true;
    public bool HasCollided = false;
    
    public override void _Process(double delta)
    {
        // Despawn expired projectiles
        TimeToLive -= (float)delta;
        if (TimeToLive <= 0)
        {
            DespawnProjectile();
            return;
        }

        if (!HasCollided)
        {
            Mover?.Move(this, delta);
        }
    }

    public override void _Ready()
    {
        TimeToLive = Lifetime;
        
        ProjectileSprite = GetNode<AnimatedSprite2D>("%ProjectileSprite");
        CollisionSprite = GetNode<AnimatedSprite2D>("%CollisionSprite");
                
        BodyEntered += OnHitboxBodyEntered;
        
        if (ProjectileAnimation != null)
        {
            ProjectileSprite.SpriteFrames = ProjectileAnimation;
            ProjectileSprite.Play();
        }
    }

    private void OnHitboxBodyEntered(Node2D body)
    {
        if (HasCollided) return;
        HasCollided = true;
        
        GetNode<CollisionShape2D>("%ProjectileHitbox").CallDeferred(CollisionShape2D.MethodName.SetDisabled, true);
        ProjectileSprite.Hide();
        
        if (body is IHittable hittable)
        {
            hittable.ReceiveHit(new HitInformation(Damage, Weight, Position));
        }
        
        if (ExplodeOnCollision && ProjectileCollisionAnimation != null)
        {
            CollisionSprite.SpriteFrames = ProjectileCollisionAnimation;
            CollisionSprite.Play();
            CollisionSprite.AnimationFinished += () =>
            {
                CollisionSprite.Hide();
                DespawnProjectile();
            };
        }
        else
        {
            DespawnProjectile();
        }
    }

    public void DespawnProjectile()
    {
        QueueFree();
    }

    /**
     * When we fire one of these, we inherit from the skill
     * - Damage Value
     * - Speed
     * - Size
     */
    /**
     * So the flow is:
     * - Press1
     * - Trigger fireball skill
     * - Skill spawns fireball, gives it damage and speed
     * - Fireball hitbox hits enemy hurtbox
     * - Fireball/skill applies Hit to hurtbox
     *
     * Execute()
     *      var proj = FireballProjectilePackedScene.Instantiate()
     *      proj.Damage = Damage
     *      proj.Speed = Speed
     *      proj.Direction = Direction
     *      AddChild(proj)
     *
     * PROJECTILE:
     * - Explode on hit?
     * - Explosion scene?
     */
    
    /**
     * Now if i have 5 projectile based skills, and i change this process, i have to update all of them.
     * - Projectiles are just a sprite and a hurtbox moving around
     * - Maybe i should add listeners to them instead, and then apply the damage
     */
}