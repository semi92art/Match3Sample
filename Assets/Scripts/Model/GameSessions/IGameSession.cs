using System;

namespace Match3SampleModel
{
    public interface IGameSession
    {
        event EventHandler OnBoardChanged;
        IBoard Board { get; }
        IMatchesDestroyer MatchesDestroyer { get; }
        void TryMoveFigure(Vec2 from, Vec2 to);
    }
}
