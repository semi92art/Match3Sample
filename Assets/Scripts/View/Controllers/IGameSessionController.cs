using UnityEngine;
using Match3SampleModel;

namespace Match3SampleView
{
    public interface IGameSessionController
    {
        IGameSession GameSession { get; }
        void TryMove(Vector2Int from, Vector2Int to);
        void UpdateBoard(object sender, System.EventArgs e);

        

    }
}
