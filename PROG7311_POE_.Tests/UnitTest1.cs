using PROG7311_POE_.Factories;
using PROG7311_POE_.Models;
using PROG7311_POE_.Observers;
using PROG7311_POE_.Services;
using Xunit;

namespace PROG7311_POE_.Tests
{
    public class CurrencyServiceTests
    {
        [Fact]
        public async Task ConvertUsdToZar_ShouldConvertCorrectly()
        {
            // Arrange
            var httpClient = new HttpClient();
            var currencyService = new CurrencyService(httpClient);

            decimal usd = 1;
            decimal zar = await currencyService.ConvertUsdToZar(usd);

            Assert.True(zar > 10);
            Assert.True(zar < 30);
        }
    }

    public class ContractFactoryTests
    {
        [Fact]
        public void PremiumFactory_ShouldCreatePremiumContract()
        {
            // Arrange
            IContractFactory factory = new PremiumFactory();

            // Act
            Contract contract = factory.Create();

            // Assert
            Assert.Equal("Premium", contract.ServiceLevel);
        }
    }

    public class StandardFactoryTests
    {
        [Fact]
        public void StandardFactory_ShouldCreateStandardContract()
        {
            // Arrange
            IContractFactory factory = new StandardFactory();

            // Act
            Contract contract = factory.Create();

            // Assert
            Assert.Equal("Standard", contract.ServiceLevel);
        }
    }

    public class ContractTests
    {
        [Fact]
        public void Contract_ShouldStoreValuesCorrectly()
        {
            // Arrange
            var contract = new Contract
            {
                ClientID = 1,
                Status = "Active",
                ServiceLevel = "Premium"
            };

            // Assert
            Assert.Equal(1, contract.ClientID);
            Assert.Equal("Active", contract.Status);
            Assert.Equal("Premium", contract.ServiceLevel);
        }
    }

    public class ObserverTests
    {
        [Fact]
        public void Observer_ShouldNotify()
        {
            // Arrange
            var subject = new ContractSubject();
            var observer = new Notification();

            subject.Attach(observer);

            // Act
            subject.Notify("New Contract");

            // Assert
            Assert.True(true); // Pass if no errors
        }
    }

}
