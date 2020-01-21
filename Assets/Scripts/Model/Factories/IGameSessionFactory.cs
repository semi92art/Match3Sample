
namespace Match3SampleModel
{
    public interface IGameSessionFactory
    {
        IGameSession Create(IBoard board, IFigureItemsInstancer figureItemsInstancer);
    }
}
