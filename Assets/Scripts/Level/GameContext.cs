using Between.Player;

namespace Between.Level
{
    public class GameContext
    {
        public FirstPersonController Player { get; set; }
        public LevelRoot CurrentLevelRoot { get; set; }
    }
}
