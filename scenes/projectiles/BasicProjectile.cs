// using System;
// using Godot;
// using TESTCS.scenes.projectiles;
//
// public partial class BasicProjectile : BaseProjectile
// {
//     private Godot.Timer _timer;
//     private bool _hasHit;
//     private bool _stopMoving;
//
//     [Export]
//     public int Damage = 50;
//     public int Weight = 50;
//
//     // Called when the node enters the scene tree for the first time.
//     public override void _Ready()
//     {
//         // GD.Print("Projectile ready");
//         _timer = GetNode<Godot.Timer>("Lifetime");
//         _timer.WaitTime = 3f;
//         _timer.Timeout += OnLifetimeEnd;
//         _timer.Start();
//
//         // Add event listener to projectile
//         BodyEntered += OnBodyEntered;
//
//         var explosion = GetNode<Explosion1>("Explosion1");
//         var animation = explosion.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
//         animation.AnimationFinished += new Action(() =>
//         {
//             DespawnProjectile();
//         });
//     }
//
//     private void OnBodyEntered(Node2D body)
//     {
//         GD.Print("SETTING HASHIT TO TRUE");
//         _stopMoving = true;
//         _hasHit = true;
//         Explode();
//     }
//
//     public override void _Process(double delta)
//     {
//         if (!_stopMoving)
//         {
//             Mover?.Move(this, delta);
//         }
//     }
//
//     public void Init(IProjectileMover mover, Vector2 initDirection)
//     {
//         Mover = mover;
//         InitDirection = initDirection;
//     }
//
//     private void OnLifetimeEnd()
//     {
//         if (!_hasHit)
//         {
//             Explode();
//         }
//     }
//
//     private void Explode()
//     {
//         var explosion = GetNode<Explosion1>("Explosion1");
//         var animation = explosion.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
//
//         explosion.Show();
//         animation.Play();
//
//         ApplyExplosionDamage();
//     }
//
//     private void DespawnProjectile()
//     {
//         QueueFree();
//     }
//
//     private void ApplyExplosionDamage()
//     {
//         var explosion = GetNode<Explosion1>("Explosion1");
//         var bodies = explosion.GetOverlappingBodies();
//         foreach (Node2D body in bodies)
//         {
//             if (body is IHittable damageable)
//             {
//                 damageable.ReceiveHit(new HitInformation(
//                     Damage, Weight, explosion.GlobalPosition));
//             }
//         }
//     }
//
//     public int GetDamage()
//     {
//         return Damage;
//     }
// }
