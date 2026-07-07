using Cysharp.Threading.Tasks;

namespace Game
{
    public class GameSession
    {
        private readonly DefeatScenario _defeatScenario;

        public GameSession(DefeatScenario defeatScenario)
        {
            _defeatScenario = defeatScenario;
        }

        public void OnMutantCaughtPlayer()
        {
            _defeatScenario.RunAsync().Forget();
        }
    }
}
