using PROG7311_POE_.Models;

namespace PROG7311_POE_.Factories
{
    public class PremiumFactory : IContractFactory
    {
        public Contract Create()
        {
            return new Contract
            {
                ServiceLevel = "Premium",
            };
        }
    }
}
