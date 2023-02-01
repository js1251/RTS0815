using Microsoft.Xna.Framework;

namespace GameLogic.Entities.Components.Movement.Interface;

internal interface IMovable {
    public Vector2 WorldPosition { get; set; }
    public IMover<IMovable> Mover { get; set; }
}