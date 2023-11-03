using System;

namespace Main {

    internal class LevelChangedEventArgs : EventArgs {
        public int LevelID { get; }

        public LevelChangedEventArgs(int levelID) {
            this.LevelID = levelID;
        }
    }
}