using Entities;
using Map;
using System.Collections.Generic;

namespace Main {

    public interface IGameState {
        IEnumerable<IEntity> Entities { get; }

        IMap Map { get; }
    }
}