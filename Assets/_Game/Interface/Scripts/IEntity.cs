using UnityEngine;

namespace Entities {

    public interface IEntity {

        char Appearance { get; }

        Color Color { get; }

        Vector2Int Position { get; }
    }
}