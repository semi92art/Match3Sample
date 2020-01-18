
using System;

namespace Match3SampleModel
{
    public class DefaultGameSession : IGameSession
    {
        public event EventHandler OnBoardChanged;
        public IBoard Board { get; private set; }
        public IMatchesDestroyer MatchesDestroyer { get; private set; }

        public DefaultGameSession(IBoard board, IMatchesDestroyer matchesDestroyer)
        {
            Board = board;
            MatchesDestroyer = matchesDestroyer;
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
                OnBoardChanged(null, null);
        }
    }
}
