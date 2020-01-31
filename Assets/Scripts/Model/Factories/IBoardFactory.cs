
namespace Match3SampleModel
{
    public interface IBoardFactory
    {
        IBoard Create();
        IBoard Create(int size_x, int size_y);
    }
}
