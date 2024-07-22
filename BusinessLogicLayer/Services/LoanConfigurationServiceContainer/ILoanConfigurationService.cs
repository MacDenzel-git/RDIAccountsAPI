

using AllinOne.DataHandlers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.LoanConfigurationServiceContainer
{
    public interface ILoanConfigurationService
    {
        Task<OutputHandler> Create(LoanConfigurationDTO loanConfiguration);
        Task<OutputHandler> Update(LoanConfigurationDTO loanConfiguration);
        Task<OutputHandler> Delete(int loanConfigurationId);
        Task<IEnumerable<LoanConfigurationDTO>> GetAllLoanConfigurations();
        Task<LoanConfigurationDTO> GetLoanConfiguration(int loanConfigurationId);
    }
}

