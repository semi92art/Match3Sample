
namespace Match3SampleModel
{
    public class DefaultGameSessionFactory : IGameSessionFactory
    {
        public IGameSession Create(IBoard board, IFigureItemsInstancer figureItemsInstancer)
        {
            return new DefaultGameSession(board, figureItemsInstancer);
        }
    }
}
