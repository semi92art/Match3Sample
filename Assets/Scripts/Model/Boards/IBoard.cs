﻿
using System.Collections.Generic;

namespace Match3SampleModel
{
    public interface IBoard
    {
        IFigureItem[,] FigureItemsTable { get; }
        bool TryMoveFigure(Vec2 from, FigureMoveType figureMoveType);
        Queue<IFigureItem>[] FigureBuffers { get; }
    }
}
