


using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.InterestAccountsServiceContainer;
using BusinessLogicLayer.JournalEntrysServiceContainer;
using BusinessLogicLayer.Logging;
using BusinessLogicLayer.Services.GroupDetailServiceContainer;
using BusinessLogicLayer.Services.InterestAccountServiceContainer;
using BusinessLogicLayer.Services.JournalEntryServiceContainer;
using BusinessLogicLayer.Services.LoanAccountServiceContainer;
using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
using BusinessLogicLayer.Services.MainAccountServiceContainer;
using BusinessLogicLayer.Services.MainAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using BusinessLogicLayer.Services.TransactionTypeServiceContainer;
using BusinessLogicLayer.Services.TransactionTypesServiceContainer;
using DataAccessLayer.Models;
using RDIAccountsAPI;

namespace RDIAccountsAPI
{
	public static class DependecyInjectionRegistration
	{
		public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<GenericRepository<InterestAccount>>();
			serviceCollection.AddScoped<GenericRepository<GroupDetail>>();
			serviceCollection.AddScoped<GenericRepository<JournalEntry>>();
			serviceCollection.AddScoped<GenericRepository<LoanAccount>>();
			serviceCollection.AddScoped<GenericRepository<LoanConfiguration>>();
			serviceCollection.AddScoped<GenericRepository<MainAccount>>();
			serviceCollection.AddScoped<GenericRepository<MemberDetail>>();
			return serviceCollection.AddScoped<GenericRepository<TransactionType>>();

		}

		public static IServiceCollection AddServices(this IServiceCollection service)
		{
			service.AddScoped<IGroupDetailService, GroupDetailService>();
			service.AddScoped<ILoanAccountService, LoanAccountService>();
			service.AddScoped<IMainAccountService, MainAccountService>();
			service.AddScoped<IJournalEntryService, JournalEntryService>();
			service.AddScoped<IMemberDetailService, MemberDetailService>();
			service.AddScoped<ITransactionTypeService, TransactionTypeService>();
			//service.AddScoped<ILogger, RoundTheCodeFileLogger>();
			//service.AddScoped<ILoggerProvider, RoundTheCodeFileLoggerProvider>();
			service.AddScoped<ILoanConfigurationService, LoanConfigurationService>();
			return service.AddScoped<IInterestAccountService, InterestAccountService>();
		}

	}
}
