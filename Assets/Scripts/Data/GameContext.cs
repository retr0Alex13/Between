using Between.Player;

namespace Between.Data
{
    public class GameContext
    {
        public FirstPersonController Player { get; set; }

        public LevelRoot CurrentLevel { get; set; }
    }
}
