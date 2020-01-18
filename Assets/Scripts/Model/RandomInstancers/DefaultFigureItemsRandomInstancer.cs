
namespace Match3SampleModel
{
    public class DefaultFigureItemsRandomInstancer : IFigureItemsRandomInstancer
    {
        public IFigureItem InstantiateItem()
        {
            int possibleFigureTypesCount = System.Enum.GetNames(typeof(IFigureItem)).Length - 2; //minus empty and no_access
            var rand = new System.Random();
            FigureItemType figureItemType = (FigureItemType)rand.Next(0, possibleFigureTypesCount);

            IFigureItemsFactory figureItemsFactory = new FigureItemsFactory();
            return figureItemsFactory.CreateFigureItem(figureItemType);
        }
    }
}
