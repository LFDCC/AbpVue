import { login, logout, refreshToken } from "@/api/account";
import { getInfo, setUserInfo } from "@/api/identity/profile";
import {
  getToken,
  setToken,
  removeToken,
  getRefreshToken,
  setRefreshToken,
  removeRefreshToken
} from "@/utils/auth";
import { resetRouter } from "@/router";

const clientSetting = {
  grant_type: "password",
  scope: "AbpVue offline_access", //offline_access获取refreshToken
  username: "",
  password: "",
  client_id: "AbpVue_App",
  client_secret: "1q2w3e*"
};

const state = {
  token: getToken(), //token 有默认5分钟的时间偏移量（ClockSkew ）
  refreshToken: getRefreshToken(),
  username: "",
  name: "",
  avatar: "",
  introduction: "",
  phoneNumber: "",
  email: "",
  roles: []
};

const mutations = {
  SET_TOKEN: (state, token) => {
    state.token = token;
    setToken(token);
  },
  SET_REFRESHTOKEN: (state, refreshToken) => {
    state.refreshToken = refreshToken;
    setRefreshToken(refreshToken);
  },
  SET_INTRODUCTION: (state, introduction) => {
    state.introduction = introduction;
  },
  SET_NAME: (state, name) => {
    state.name = name;
  },
  SET_USERNAME: (state, userName) => {
    state.username = userName;
  },
  SET_TEL: (state, phoneNumber) => {
    state.phoneNumber = phoneNumber;
  },
  SET_AVATAR: (state, avatar) => {    
    state.avatar = avatar;
  },
  SET_EMAIL: (state, email) => {
    state.email = email;
  },
  SET_ROLES: (state, roles) => {
    state.roles = roles;
  },
  CLEAN: state => {
    state.token = "";
    state.refreshToken = "";
    state.name = "";
    state.username = "";
    state.roles = [];
    state.avatar = "";
    state.email = "";
    state.introduction = "";
    state.phoneNumber = "";
  }
};

const actions = {
  // user login
  login({ commit, dispatch }, userInfo) {
    const { username, password } = userInfo;
    return new Promise((resolve, reject) => {
      clientSetting.username = username.trim();
      clientSetting.password = password;
      login(clientSetting)
        .then(async response => {
          commit("SET_TOKEN", response.access_token);
          commit("SET_REFRESHTOKEN", response.refresh_token);
          await dispatch("app/applicationConfiguration", null, { root: true });
          resolve();
        })
        .catch(error => {
          reject(error);
        });
    });
  },
  //刷新token
  refreshToken({ commit, dispatch }) {
    return new Promise((resolve, reject) => {
      const refreshParam = {
        grant_type: "refresh_token",
        refresh_token: state.refreshToken,
        client_id: clientSetting.client_id,
        client_secret: clientSetting.client_secret
      };
      refreshToken(refreshParam)
        .then(async response => {
          commit("SET_TOKEN", response.access_token);
          commit("SET_REFRESHTOKEN", response.refresh_token);
          await dispatch("app/applicationConfiguration", null, { root: true });
          resolve(response.access_token);
        })
        .catch(error => {
          reject(error);
        });
    });
  },
  // get user info
  getInfo({ commit }) {
    return new Promise((resolve, reject) => {
      getInfo()
        .then(response => {
          if (!response) {
            reject("Verification failed, please Login again.");
          }
          const {
            userName,
            name,
            phoneNumber,
            email,
            extraProperties
          } = response;
          commit("SET_NAME", name);
          commit("SET_USERNAME", userName);
          commit("SET_TEL", phoneNumber);
          commit("SET_AVATAR", extraProperties.Avatar);
          commit("SET_EMAIL", email);
          commit("SET_INTRODUCTION", extraProperties.Introduction);
          resolve(response);
        })
        .catch(error => {
          reject(error);
        });
    });
  },
  setRoles({ commit }, roles) {
    commit("SET_ROLES", roles);
  },
  // user logout
  logout({ commit, dispatch }) {
    return new Promise((resolve, reject) => {
      logout()
        .then(async () => {
          commit("CLEAN");
          removeToken();
          removeRefreshToken();
          resetRouter();
          await dispatch("app/applicationConfiguration", null, { root: true });
          await dispatch("tagsView/delAllViews", null, { root: true });
          resolve();
        })
        .catch(error => {
          reject(error);
        });
    });
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit("CLEAN");
      removeToken();
      removeRefreshToken();
      resolve();
    }).catch(error => {
      reject(error);
    });
  },
  setUserInfo({ commit }, userInfo) {
    return new Promise((resolve, reject) => {
      setUserInfo(userInfo)
        .then(response => {
          const {
            userName,
            name,
            phoneNumber,
            email,
            extraProperties
          } = userInfo;
          commit("SET_NAME", name);
          commit("SET_USERNAME", userName);
          commit("SET_TEL", phoneNumber);
          commit("SET_AVATAR", extraProperties.Avatar);
          commit("SET_EMAIL", email);
          commit("SET_INTRODUCTION", extraProperties.Introduction);
          resolve(response);
        })
        .catch(error => {
          reject(error);
        });
    });
  }
};

export default {
  namespaced: true,
  state,
  mutations,
  actions
};
