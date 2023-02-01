using GameLogic.Entities.Components.Movement.Interface;
using Microsoft.Xna.Framework;

namespace GameLogic.Entities.Components.Movement;

internal class EulerMove: IMover<IMovable> {
    private readonly IMovable _owner;
    private Vector2 _velocity;
    private Vector2 _acceleration;

    public EulerMove(IMovable owner, Vector2 initialVelocity = default) {
        _owner = owner;
        _velocity = initialVelocity;
    }

    public void Update(GameTime gameTime) {
        var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var acceleration = GetCurrentAcceleration();

        _velocity += acceleration * deltaT;
        _owner.WorldPosition += _velocity * deltaT;
    }

    private Vector2 GetCurrentAcceleration() {
        return _acceleration;
    }

    public void AddConstantAcceleration(Vector2 acceleration) {
        _acceleration += acceleration;
    }
}