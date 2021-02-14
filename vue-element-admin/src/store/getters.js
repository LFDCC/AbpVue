import { getAvatar} from "@/api/identity/profile";
const getters = {
  sidebar: state => state.app.sidebar,
  language: state => state.app.language,
  size: state => state.app.size,
  device: state => state.app.device,
  abpConfig: state => state.app.abpConfig,
  tenant: state => state.app.tenant,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  avatarFullPath: state => {
    if (!state.user.avatar) {
      return "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif";
    }
   return getAvatar(state.user.avatar);
  },
  name: state => state.user.name,
  username: state => state.user.username,
  logout: state => state.user.logout,
  email: state => state.user.email,
  phoneNumber: state => state.user.phoneNumber,
  introduction: state => state.user.introduction,
  roles: state => state.user.roles,
  permission_routes: state => state.permission.routes,
  errorLogs: state => state.errorLog.logs
};
export default getters;
