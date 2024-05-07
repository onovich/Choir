namespace TenonKit.Choir {

    public class SoundIDService {

        int playerID = 0;

        public int PickPlayerID() {
            return playerID++;
        }

    }

}