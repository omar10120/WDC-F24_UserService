using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDC_F24.Application.DTOs.Requests;
using WDC_F24.Application.DTOs.Responses;

namespace WDC_F24.Application.Interfaces
{
    public interface IUserService
    {
        Task<GeneralResponse> GetAllAsync();
        Task<GeneralResponse> GetByIdAsync(Guid id);
        Task<GeneralResponse> DeleteAsync(Guid id);
        Task<GeneralResponse> RegisterAsync(RegisterRequestDto user);
        Task<GeneralResponse> LoginAsync(LoginRequestDto user);
        Task<GeneralResponse> UpdateAsync(UpdateRequestDto user , Guid id);
    }
}
