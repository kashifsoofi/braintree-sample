using Braintree;
using BraintreeSample.Configuration;

namespace BraintreeSample
{
    public static class ServiceCollectionSetup
    {
        public static IServiceCollection AddBraintree(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var braintreeConfiguration = new BraintreeConfiguration();
            configuration.Bind(nameof(BraintreeConfiguration), braintreeConfiguration);
            serviceCollection.AddSingleton(braintreeConfiguration);

            serviceCollection.AddTransient<IBraintreeGateway>(provider =>
            {
                var braintreeConfiguration = provider.GetRequiredService<BraintreeConfiguration>();
                return new BraintreeGateway(
                    braintreeConfiguration.Environment,
                    braintreeConfiguration.MerchantId,
                    braintreeConfiguration.PublicKey,
                    braintreeConfiguration.PrivateKey);
            });

            return serviceCollection;
        }
    }
}
