using UnityEngine;

using Match3SampleModel;

namespace Match3SampleView
{
    public interface IFigureItem
    {
        Vector2Int Position { get; set; }
        FigureItemType FigureType { get; }

        void InitItem(int x_position, int y_position, FigureItemType type, FigureLocation location);
        void MoveFigureToPosition(int x, int y);
        FigureLocation Location { get; set; }

        void MoveFromQueueToBoard();
        void KillIfOnBoard();
        bool IsInAction { get; }
    }
}
