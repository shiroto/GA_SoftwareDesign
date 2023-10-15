using Random = System.Random;

namespace MapGen {

    public class MapGenConfig {

        public MapGenConfig(
            int mapHeight,
            int mapWidth,
            int minRoomSize,
            int maxRoomSize,
            int maxRoomCount,
            int maxRoomGenerationAttempts,
            Random random) {
            MapHeight = mapHeight;
            MapWidth = mapWidth;
            MinRoomSize = minRoomSize;
            MaxRoomSize = maxRoomSize;
            MaxRoomCount = maxRoomCount;
            MaxRoomGenerationAttempts = maxRoomGenerationAttempts;
            Random = random;
        }

        public int MapHeight { get; }

        public int MapWidth { get; }

        public int MaxRoomCount { get; }

        public int MaxRoomGenerationAttempts { get; }

        public int MaxRoomSize { get; }

        public int MinRoomSize { get; }

        public Random Random { get; }
    }
}