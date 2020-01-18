
namespace Match3SampleModel
{
    public static class FigureMover
    {
        public static Vec2 MoveFigure(Vec2 from, FigureMoveType figureMoveType)
        {
            var new_position = from;
            switch (figureMoveType)
            {
                case FigureMoveType.Left:
                    new_position.x--;
                    break;
                case FigureMoveType.Top:
                    new_position.y++;
                    break;
                case FigureMoveType.Right:
                    new_position.x++;
                    break;
                case FigureMoveType.Bottom:
                    new_position.y--;
                    break;
                default:
                    throw new System.NotImplementedException("Move function not implemented completely!");
            }

            return new_position;
        }
    }
}
