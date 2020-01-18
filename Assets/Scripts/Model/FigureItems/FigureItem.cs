
namespace Match3SampleModel
{
    public class FigureItem : IFigureItem
    {
        public FigureItemType FigureType { get; private set; }

        public FigureItem(FigureItemType figureItemType)
        {
            FigureType = figureItemType;
        }

        public bool IsEmpty()
        {
            return FigureType == FigureItemType.empty;
        }

        public void SetEmpty()
        {
            FigureType = FigureItemType.empty;
        }
    }
}
