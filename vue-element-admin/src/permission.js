import router from "./router";
import store from "./store";
import { Message } from "element-ui";
import NProgress from "nprogress"; // progress bar
import "nprogress/nprogress.css"; // progress bar style
//import { getToken } from '@/utils/auth' // get token from cookie
import getPageTitle from "@/utils/get-page-title";

NProgress.configure({showSpinner: false })

const whiteList = ["/login", "/auth-redirect"]; // no redirect whitelist

router.beforeEach(async (to, from, next) => {
  // start progress bar
  NProgress.start();

  //拉取系统配置文件
  let abpConfig = store.getters.abpConfig;
  if (!abpConfig) {
    abpConfig = await store.dispatch("app/applicationConfiguration");
  }

  // determine whether the user has logged in
  //const hasToken = getToken()

  //如果已经登录
  if (abpConfig.currentUser.isAuthenticated) {
    if (to.path === "/login") {
      // if is logged in, redirect to the home page
      next({ path: "/" });
      NProgress.done(); // hack: https://github.com/PanJiaChen/vue-element-admin/pull/2939
    } else {
      // determine whether the user has obtained his permission roles through getInfo
      const username = store.getters.username;
      if (username) {
        next();
      } else {
        try {
          // get user info
          await store.dispatch("user/getInfo");

          await store.dispatch("user/setRoles", abpConfig.currentUser.roles);

          //当前用户的授权的策略
          const grantedPolicies = abpConfig.auth.grantedPolicies;

          const accessRoutes = await store.dispatch(
            "permission/generateRoutes",
            grantedPolicies
          );

          router.addRoutes(accessRoutes);

          next({ ...to, replace: true });
        } catch (error) {
          await store.dispatch("user/resetToken");
          Message.error(error || "Has Error");
          next(`/login?redirect=${to.path}`);
          NProgress.done();
        }
      }
    }
  } else {
    /* has no token*/
    //如果没有登录 且在白名单内 正常跳转
    if (whiteList.indexOf(to.path) !== -1) {
      next();
    } else {
      next(`/login?redirect=${to.path}`);
      NProgress.done();
    }
  }
});

router.afterEach((to, from, next) => {
  // finish progress bar
  NProgress.done();
  // set page title
  if (to.meta.displayName) {
    document.title = to.meta.displayName;
  } else {
    document.title = getPageTitle(to.meta.title);
  }
  //setTimeout(() => {
    let loading = document.getElementById("Loading");
    if (loading) {
      document.body.removeChild(loading);
    }
  //}, 50);
});
