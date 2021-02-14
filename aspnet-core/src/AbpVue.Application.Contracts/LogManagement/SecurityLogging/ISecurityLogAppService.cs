using AbpVue.LogManagement.SecurityLogging.Dtos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace AbpVue.LogManagement.SecurityLogging
{
    public interface ISecurityLogAppService : IReadOnlyAppService<SecurityLogDto, Guid, GetSecurityLogDto>, IDeleteAppService<Guid>
    {
      
    }
}
