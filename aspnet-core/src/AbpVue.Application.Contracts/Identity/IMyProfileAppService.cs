using AbpVue.Files.Dtos;
using AbpVue.LogManagement.SecurityLogging.Dtos;

using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;

namespace AbpVue.Identity
{
    public interface IMyProfileAppService
    {
        Task<PagedResultDto<SecurityLogDto>> GetMySecurityLogs(GetSecurityLogDto input);

        Task<GetFileOutput> GetFile(GetFileInput input);

        Task<SaveFileOutput> SaveFile(SaveFileInput input);
    }
}
