using PROG7311_POE_.Models;

namespace PROG7311_POE_.Factories
{
    public class StandardFactory : IContractFactory
    {
        public Contract Create()
        {
            return new Contract
            {
                ServiceLevel = "Standard",
            };
        }
    }
}
