using System;
using System.Collections.Generic;

namespace Match3SampleModel
{
    public interface IGameSession
    {
        event EventHandler OnBoardChanged;
        IBoard Board { get; }
        IFigureItemsInstancer FigureItemsInstancer { get; }
        void TryMoveFigure(Vec2 from, Vec2 to);
        Queue<string> Moves { get; }
    }
}
