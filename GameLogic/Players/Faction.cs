using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameLogic.Players;

public enum RelationStatus {
    Neutral,
    Allied,
    AtWar
}

public sealed class Faction {
    public string Name { get; init; } = string.Empty;
    public Color Color { get; init; } = Color.Gray;

    private Dictionary<Faction, RelationStatus> Relations { get; }

    public Faction() {
        Relations = new Dictionary<Faction, RelationStatus>();
    }

    public void SetRelation(Faction faction, RelationStatus status) {
        if (!Relations.ContainsKey(faction)) {
            Relations.Add(faction, status);
            return;
        }

        Relations[faction] = status;
    }

    public RelationStatus GetRelation(Faction faction) {
        return Relations.ContainsKey(faction)
            ? Relations[faction]
            : RelationStatus.Neutral;
    }
}