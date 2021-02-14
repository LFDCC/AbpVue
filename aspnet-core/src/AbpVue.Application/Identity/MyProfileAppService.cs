using AbpVue.Files.Container;
using AbpVue.Files.Dtos;
using AbpVue.Localization;
using AbpVue.LogManagement.SecurityLogging.Dtos;
using AbpVue.Settings;
using AbpVue.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Settings;
using Volo.Abp.Auditing;

namespace AbpVue.Identity
{
    [Authorize]
    public class MyProfileAppService : AbpVueAppService, IMyProfileAppService
    {
        private readonly ICustomRepository<AppUser, Guid> _appUserRepository;
        private readonly IStringLocalizer<FileResource> _localizerFile;
        private readonly IBlobContainer<ProfilePictureContainer> _blobContainer;
        private readonly IIdentitySecurityLogRepository _identitySecurityLogRepository;
        public MyProfileAppService(IBlobContainer<ProfilePictureContainer> blobContainer, IIdentitySecurityLogRepository identitySecurityLogRepository, ICustomRepository<AppUser, Guid> appUserRepository, IStringLocalizer<FileResource> localizerFile)
        {
            _localizerFile = localizerFile;
            _blobContainer = blobContainer;
            _appUserRepository = appUserRepository;
            _identitySecurityLogRepository = identitySecurityLogRepository;
        }
        /// <summary>
        /// 获取我的安全日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<SecurityLogDto>> GetMySecurityLogs(GetSecurityLogDto input)
        {
            input.UserId = CurrentUser.Id;
            var securityLogCount = await _identitySecurityLogRepository
                .GetCountAsync(input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId
                );

            var securityLogs = await _identitySecurityLogRepository
                .GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount,
                    input.StartTime, input.EndTime,
                    input.ApplicationName, input.Identity, input.ActionName,
                    input.UserId, input.UserName, input.ClientId, input.CorrelationId,
                    includeDetails: false
                );
                               
            return new PagedResultDto<SecurityLogDto>(securityLogCount,
                ObjectMapper.Map<List<IdentitySecurityLog>, List<SecurityLogDto>>(securityLogs));
        }

        [AllowAnonymous]
        public async Task<GetFileOutput> GetFile(GetFileInput input)
        {
            var bytes = await _blobContainer.GetAllBytesOrNullAsync(input.BlobName);
            return new GetFileOutput
            {
                BlobName = input.BlobName,
                Bytes = bytes
            };
        }
        public async Task<SaveFileOutput> SaveFile(SaveFileInput input)
        {
            await CheckFile(input);

            await _blobContainer.SaveAsync(input.BolbName, input.Bytes);
            await _appUserRepository.UpdateAsync(t => t.Id == Guid.Parse(CurrentUser.Id.ToString()), t => new AppUser
            {
                Avatar = input.BolbName
            });
            return new SaveFileOutput
            {
                BolbName = input.BolbName
            };
        }

        protected virtual async Task CheckFile(SaveFileInput input)
        {
            if (input.Bytes.IsNullOrEmpty())
            {
                throw new UserFriendlyException(_localizerFile["File.Empty"]);
            }

            var allowdUploadSize = await SettingProvider.GetAsync<int>(AbpVueSettings.AllowedUserAvatarSize);
            if (input.Bytes.Length > allowdUploadSize * 1024)
            {
                throw new UserFriendlyException(_localizerFile["File.ErrorSize", allowdUploadSize]);
            }

            var allowedUploadFormats = (await SettingProvider.GetOrNullAsync(AbpVueSettings.AllowedUserAvatarFormats))
                ?.Split(",", StringSplitOptions.RemoveEmptyEntries);


            if (allowedUploadFormats == null || !allowedUploadFormats.Contains(Path.GetExtension(input.BolbName)))
            {
                throw new UserFriendlyException(_localizerFile["File.ErrorFormat"]);
            }
        }
    }
}
