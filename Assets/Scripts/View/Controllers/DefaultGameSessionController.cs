using UnityEngine;
using Match3SampleModel;
using System;

namespace Match3SampleView
{
    public class DefaultGameSessionController : IGameSessionController
    {
        public IGameSession GameSession { get; private set; }
        public IGameSessionView GameSessionView { get; private set; }

        public DefaultGameSessionController(IGameSession gameSession, IGameSessionView gameSessionView)
        {
            GameSession = gameSession;
            GameSessionView = gameSessionView;
        }

        public void TryMove(Vector2Int from, Vector2Int to)
        {
            GameSession.TryMoveFigure(from.ToVec2(), to.ToVec2());
        }

        public void UpdateBoard(object sender, EventArgs e)
        {
            GameSessionView.UpdateBoard();
        }
    }
}
