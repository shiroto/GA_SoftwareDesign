using Map;
using MapGen;
using MVT;
using UnityEngine;

public class Startup : MonoBehaviour {

    [SerializeField]
    private int seed, width, height, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts;

    [ContextMenu("Randomize Seed")]
    private void RandomizeSeed() {
        seed = Random.Range(int.MinValue, int.MaxValue);
    }

    private void Start() {
        MapGenConfig config = new(height, width, minRoomSize, maxRoomSize, maxRoomCount, maxRoomGenerationAttempts, new System.Random(seed));
        IMap map = MapGenerator.GenMap(config);
        IMapView view = MapViewFactory.CreateMapView();
        view.SetMap(map);
    }
}