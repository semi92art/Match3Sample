using UnityEngine;
using Model = Match3SampleModel;

namespace Match3SampleView
{
    public interface IGameSessionView
    {
        Model.IGameSession GameSession { get; }
        IGameSessionController GameSessionController { get; }
        BoardItem[,] BoardItems { get; }
        FigureItem[,] FigureItems { get; }

        void MoveFigure(Vector2Int from, Vector2Int to);
        void UpdateBoard();
    }
}
