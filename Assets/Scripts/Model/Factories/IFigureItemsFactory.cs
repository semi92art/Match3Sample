
namespace Match3SampleModel
{
    public interface IFigureItemsFactory
    {
        IFigureItem CreateFigureItem(FigureItemType figureItemType);
    }
}
