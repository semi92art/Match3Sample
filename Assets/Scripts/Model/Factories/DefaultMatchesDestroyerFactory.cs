
namespace Match3SampleModel
{
    public class DefaultMatchesDestroyerFactory : IMatchesDestroyerFactory
    {
        public IMatchesDestroyer Create()
        {
            return new DefaultMatchesDestroyer();
        }
    }
}
