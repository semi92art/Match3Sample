
namespace Match3SampleModel
{
    public class FigureItemsFactory : IFigureItemsFactory
    {
        public IFigureItem CreateFigureItem(FigureItemType figureItemType)
        {
            return new FigureItem(figureItemType);
        }
    }
}
