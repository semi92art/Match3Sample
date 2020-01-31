
namespace Match3SampleModel
{
    public class DefaultFigureItemsInstancerFactory : IFigureItemsInstancerFactory
    {
        public IFigureItemsInstancer Create()
        {
            return new DefaultFigureItemsInstancer();
        }
    }
}
