using GamesDataAccess;

class DataPopulator
{
    private GamesDao _gamesDao;

    public DataPopulator(GamesDao gamesDao)
    {
        _gamesDao = gamesDao;
    }

    public int AddSomeGames()
    {
        int affected = 0;
        foreach (var game in GetGames())
        {
            affected += _gamesDao.AddNewGame(game);
        }
        return affected;
    }

    private IEnumerable<GameDbItem> GetGames()
    {
        yield return
            new GameDbItem
            (
                "elden-ring",
                "Elden Ring",
                "GOTY 2022",
                "soulslike;gdr;adventure"
            );

        yield return
            new GameDbItem
            (
                "zelda-botw",
                "Zelda Breath of the Wild",
                "The Legend of Zelda: Breath of the Wild",
                "zelda;gdr;nintendo;adventure"
            );

        yield return
            new GameDbItem
            (
                "super-mario-wonder",
                "Super Mario Wonder",
                "Super Mario Wonder",
                "mario;platform;nintendo"
            );

        yield return
            new GameDbItem
            (
                "palworld",
                "Palworld",
                "Pokemon clone",
                "pokemon;survival"
            );

        yield return
            new GameDbItem
            (
                "alan-wake-2",
                "Alan Wake 2",
                "Alan Wake 2",
                "adventure;survival;horror"
            );
    }
}