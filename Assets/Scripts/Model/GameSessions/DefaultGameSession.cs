
using System;
using System.Collections.Generic;

namespace Match3SampleModel
{
    public class DefaultGameSession : IGameSession
    {
        public event EventHandler OnBoardChanged;
        public IBoard Board { get; private set; }
        public IFigureItemsInstancer FigureItemsInstancer { get; private set; }
        public Queue<string> Moves { get; private set; }
        

        public DefaultGameSession(IBoard board, IFigureItemsInstancer figureItemsInstancer)
        {
            Board = board;
            FigureItemsInstancer = figureItemsInstancer;
            Moves = new Queue<string>();
        }

        public void TryMoveFigure(Vec2 from, Vec2 to)
        {
            FigureMoveType figureMoveType;
            if (to.x - from.x == -1 && to.y == from.y)
                figureMoveType = FigureMoveType.Left;
            else if (to.x - from.x == 1 && to.y == from.y)
                figureMoveType = FigureMoveType.Right;
            else if (to.x == from.x && to.y - from.y == -1)
                figureMoveType = FigureMoveType.Bottom;
            else if (to.x == from.x && to.y - from.y == 1)
                figureMoveType = FigureMoveType.Top;
            else
            {
                return;
            }

            if (Board.TryMoveFigure(from, figureMoveType))
            {
                FigureItemsInstancer.Moves.Enqueue("h" + from.ToString() + "_" + "b" + to.ToString());
                CheckForMathces();
                PrepareNewMoves();
                OnBoardChanged(null, null);
            }
        }

        private void CheckForMathces()
        {
            if (FigureItemsInstancer.CheckForNewMatches(Board.FigureItemsTable, FigureAction.SetEmpty))
            {
                FigureItemsInstancer.FillEmptyItemsFromQueues(Board);
                CheckForMathces();
            }
        }

        private void PrepareNewMoves()
        {
            Moves.Clear();
            while (FigureItemsInstancer.Moves.Count > 0)
                Moves.Enqueue(FigureItemsInstancer.Moves.Dequeue());
        }
    }
}
