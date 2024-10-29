using VideogamesWebApp.Models;

namespace VideogamesWebApp.BusinessModels
{
    public class GameTransactionDetails
    {
        public Stores Store { get; set; }
        public Platforms Platform { get; set; }
        public Game Game { get; set; }
        public Launcher Launcher { get; set; }

        public GameTransactionDetails(Stores store, Platforms platform, Game game, Launcher launcher)
        {
            Store = store;
            Platform = platform;
            Game = game;
            Launcher = launcher;
        }

    }
}
