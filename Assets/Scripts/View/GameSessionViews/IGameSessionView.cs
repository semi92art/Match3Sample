using UnityEngine;
using Model = Match3SampleModel;

namespace Match3SampleView
{
    public interface IGameSessionView
    {
        void InitView(IGameSessionController gameSessionController, IBoardWorldPosition boardWorldPosition, ICommandsConverter commandsConverter);
        IBoardItem[,] BoardItems { get; }
        IFigureItem[,] FigureItems { get; }
        int BoardSize_X { get; }
        int BoardSize_Y { get; }

        void MakeTurn(Vector2Int position);
        void UpdateBoard();
        void InitBoard();
        Vector3 GetBoardWorldPosition(int x, int y);
        bool ArePreviousMovesFinished();
    }
}
