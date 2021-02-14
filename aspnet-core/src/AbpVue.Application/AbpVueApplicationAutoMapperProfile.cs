
using AbpVue.LogManagement.AuditLogging.Dtos;
using AbpVue.LogManagement.SecurityLogging.Dtos;

using AutoMapper;

using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;

namespace AbpVue
{
    public class AbpVueApplicationAutoMapperProfile : Profile
    {
        public AbpVueApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<AuditLog, AuditLogDto>().MapExtraProperties();
            CreateMap<AuditLogAction, AuditLogActionDto>().MapExtraProperties();
            CreateMap<EntityChange, EntityChangeDto>().MapExtraProperties();
            CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            CreateMap<IdentitySecurityLog, SecurityLogDto>().MapExtraProperties();
        }
    }
}
