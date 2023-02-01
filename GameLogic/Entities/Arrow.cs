using System;
using GameEngine.Rendering;
using GameLogic.Entities.Components.Movement;
using GameLogic.Entities.Components.Movement.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLogic.Entities;

internal class Arrow : Entity, IMovable {
    public IMover<IMovable> Mover { get; set; }

    public Vector2 Target {
        init {
            //10m/s^2 (assuming 1 unit is 1cm)
            var gravity = new Vector2(0, 1000f); // TODO: add to global constant
            
            var euler = new EulerMove(this, GetVelocityToHitTarget(value, gravity));
            euler.AddConstantAcceleration(gravity);

            Mover = euler;
        }
    }

    public float MaxSpeed { get; set; } = 20 * 100f; // 20m/s by default (assuming 1 unit is 1cm)

    public override void Update(GameTime gameTime) {
        Mover.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.DrawFilledRectangle(WorldPosition, 10, 10, Color.Red);
    }

    private Vector2 GetVelocityToHitTarget(Vector2 target, Vector2 gravity) {
        // https://en.wikipedia.org/wiki/Projectile_motion

        var x = target.X - WorldPosition.X;
        var y = WorldPosition.Y - target.Y;
        var s = MaxSpeed;
        var g = gravity.Y;

        // theta = tan^-1(s^2 +- sqrt(s^4 - g(gx^2 + 2yv^2)) / gx)
        var root = Math.Sqrt(Math.Pow(s, 4) - g * (g * Math.Pow(x, 2) + 2 * y * Math.Pow(s, 2)));
        if (double.IsNaN(root)) {
            // target is unreachable with current speed
            return Vector2.Zero;
        }

        // high trajectory
        var theta1 = Math.Atan2(s * s + root, g * x);

        // flat trajectory
        var theta2 = Math.Atan2(s * s - root, g * x);

        var vx = s * Math.Cos(theta1);
        var vy = s * Math.Sin(theta1);

        return new Vector2((float)vx, -(float)vy);
    }
}