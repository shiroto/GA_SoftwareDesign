using UnityEngine;

namespace MVT {

    public static class MapViewFactory {

        public static IMapView CreateMapView() {
            MapViewConfig config = MapViewConfig.GetInstance();
            MapView instance = GameObject.Instantiate(config.Prefab);
            return instance;
        }
    }
}