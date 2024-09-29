


using BusinessLogicLayer.AuditTrailServiceContainer;
using BusinessLogicLayer.GroupDetailsServiceContainer;
using BusinessLogicLayer.InterestAccountsServiceContainer;
using BusinessLogicLayer.JournalEntrysServiceContainer;
using BusinessLogicLayer.Services.GroupAccountServiceContainer;
using BusinessLogicLayer.Services.GroupAccountsServiceContainer;
using BusinessLogicLayer.Services.GroupDetailServiceContainer;
using BusinessLogicLayer.Services.InterestAccountServiceContainer;
using BusinessLogicLayer.Services.JournalEntryServiceContainer;
using BusinessLogicLayer.Services.LoanAccountServiceContainer;
using BusinessLogicLayer.Services.LoanAccountsServiceContainer;
using BusinessLogicLayer.Services.LoanConfigurationServiceContainer;
using BusinessLogicLayer.Services.MailingListServiceContainer;
using BusinessLogicLayer.Services.MemberAccountServiceContainer;
using BusinessLogicLayer.Services.MemberAccountsServiceContainer;
using BusinessLogicLayer.Services.MemberDetailServiceContainer;
using BusinessLogicLayer.Services.MemberDetailsServiceContainer;
using BusinessLogicLayer.Services.TransactionTypeServiceContainer;
using BusinessLogicLayer.Services.TransactionTypesServiceContainer;
using DataAccessLayer.Models;

namespace RDIAccountsAPI
{
    public static class DependecyInjectionRegistration
	{
		public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<GenericRepository<InterestAccount>>();
			serviceCollection.AddScoped<GenericRepository<GroupDetail>>();
			serviceCollection.AddScoped<GenericRepository<AuditTrail>>();
			serviceCollection.AddScoped<GenericRepository<JournalEntry>>();
			serviceCollection.AddScoped<GenericRepository<LoanAccount>>();
			serviceCollection.AddScoped<GenericRepository<LoanConfiguration>>();
			serviceCollection.AddScoped<GenericRepository<MemberAccount>>();
			serviceCollection.AddScoped<GenericRepository<GroupAccount>>();
			serviceCollection.AddScoped<GenericRepository<MailingList>>();
			serviceCollection.AddScoped<GenericRepository<MemberDetail>>();
			return serviceCollection.AddScoped<GenericRepository<TransactionType>>();

		}

		public static IServiceCollection AddServices(this IServiceCollection service)
		{
			service.AddScoped<IGroupDetailService, GroupDetailService>();
			service.AddScoped<IAuditTrailService, AuditTrailService>();
			service.AddScoped<ILoanAccountService, LoanAccountService>();
			service.AddScoped<IMemberAccountService, MemberAccountService>();
			service.AddScoped<IGroupAccountService, GroupAccountService>();
			service.AddScoped<IJournalEntryService, JournalEntryService>();
			service.AddScoped<IMemberDetailService, MemberDetailService>();
			service.AddScoped<ITransactionTypeService, TransactionTypeService>();
			service.AddScoped<IMailingListService, MailingListService>();
			//service.AddScoped<ILogger, RoundTheCodeFileLogger>();
			//service.AddScoped<ILoggerProvider, RoundTheCodeFileLoggerProvider>();
			service.AddScoped<ILoanConfigurationService, LoanConfigurationService>();
			//service.AddScoped<IUnitOfWork, UnitOfWork>();
			return service.AddScoped<IInterestAccountService, InterestAccountService>();
		}

	}
}
