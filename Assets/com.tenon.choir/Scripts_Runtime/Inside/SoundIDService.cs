namespace TenonKit.Choir {

    internal class SoundIDService {

        int playerID = 0;

        internal int PickPlayerID() {
            return playerID++;
        }

    }

}