
using BusinessLogicLayer.ServiceContainer.InterestAccountServiceContainer;
using BusinessLogicLayer.ServiceContainer.LoanAccountServiceContainer;
using BusinessLogicLayer.ServiceContainer.MemberAccountServiceContainer;
 using BusinessLogicLayer.ServiceContainer.TransactionTypeServiceContainer;
 using BusinessLogicLayer.Services.InterestAccountsServiceContainer;
using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
using BusinessLogicLayer.Services.MemberAccountsServiceContainer;
using BusinessLogicLayer.Services.TransactionTypesServiceContainer;
using DataAccessLayer.Models;
 using RDIAccountsAPI;

namespace HospitalityManagementSystemWebApp
{
    public static class DependecyInjectionRegistration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<GenericRepository<InterestAccount>>();
            serviceCollection.AddScoped<GenericRepository<JournalEntry>>();
            serviceCollection.AddScoped<GenericRepository<LoanAccount>>();
            serviceCollection.AddScoped<GenericRepository<LoanConfiguration>>();
            serviceCollection.AddScoped<GenericRepository<MainAccount>>();
            serviceCollection.AddScoped<GenericRepository<MemberAccount>>();
			return serviceCollection.AddScoped<GenericRepository<TransactionType>>();

		}

		public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<ILoanAccountService, LoanAccountService>();
            service.AddScoped<IMemberAccountService, MemberAccountService>();
            service.AddScoped<ITransactionTypeService, TransactionTypeService>();
            service.AddScoped<ILoanConfigurationService, LoanConfigurationService>();
            service.AddScoped<ILoanAccountService, LoanAccountService>();
            return service.AddScoped<IInterestAccountService, InterestAccountService>();
        }
    }
}
