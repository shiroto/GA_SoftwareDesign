using Entities;
using Map;
using System.Collections.Generic;

namespace MVT {

    public interface IMapView {

        void Init(IMap map);

        /// <summary>
        /// Interface segregation!
        /// </summary>
        void UpdateView(IEnumerable<IEntity> entities);
    }
}