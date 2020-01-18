using UnityEngine;
using Model = Match3SampleModel;

namespace Match3SampleView
{
    public class DefaultGameSessionView : IGameSessionView
    {
        public Model.IGameSession GameSession { get; private set; }
        public IGameSessionController GameSessionController { get; private set; }

        public BoardItem[,] BoardItems { get; private set; }

        public FigureItem[,] FigureItems { get; private set; }

        
        public DefaultGameSessionView(Model.IGameSession gameSession, IGameSessionController gameSessionController)
        {
            GameSession = gameSession;
            GameSessionController = gameSessionController;
            InstantiateBoardTextures();
            InstantiateFigureTextures();
        }


        public void MoveFigure(Vector2Int from, Vector2Int to)
        {
            GameSessionController.TryMove(from, to);
        }

        public void UpdateBoard()
        {
            throw new System.NotImplementedException();
        }

        private void InstantiateBoardTextures()
        {
            throw new System.NotImplementedException();
        }

        private void InstantiateFigureTextures()
        {
            throw new System.NotImplementedException();
        }
    }
}
