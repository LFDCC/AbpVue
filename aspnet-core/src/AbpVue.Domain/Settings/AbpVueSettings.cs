namespace AbpVue.Settings
{
    public static class AbpVueSettings
    {
        private const string Prefix = "AbpVue";

        //Add your own setting names here. Example:
        //public const string MySetting1 = Prefix + ".MySetting1";

        private const string UserAvatarGroupName = Prefix + ".AllowedUserAvatar";

        public const string AllowedUserAvatarFormats = UserAvatarGroupName + ".Formats";
        public const string AllowedUserAvatarSize = UserAvatarGroupName + ".Size";
    }
}