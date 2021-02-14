import axios from 'axios'
import { Message } from 'element-ui'
import store from '@/store'
import { param as encodeParam } from '@/utils'
import { getToken } from '@/utils/auth'
import i18n from '@/lang'
import NProgress from "nprogress"; // progress bar
import "nprogress/nprogress.css"; // progress bar style

NProgress.configure({showSpinner: false })

let isRefreshing = false;
// 重试队列，每一项将是一个待执行的函数形式
let requests = [];

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  //timeout: 30000 // request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    NProgress.start();

    config.headers['X-Requested-With'] = 'XMLHttpRequest'
    config.headers['accept-language'] = store.getters.language

    if (store.getters.token) {
      config.headers['authorization'] = 'Bearer ' + store.getters.token
    }

    if (store.getters.tenant) {
      config.headers['__tenant'] = store.getters.tenant
    }

    config.paramsSerializer = function (params) {
      return encodeParam(params)
    }
    return config
  },
  error => {
    NProgress.done();
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  response => {
    NProgress.done();
    const res = response.data
    return res
  },
  async error => {
    NProgress.done();
    let response = error.response
    if (response) {
      if (response.config.headers['refresh_token']) {
        //如果refresh_token报错 直接跳转到登录页面
        return new Promise(async (resolve) => {
          Message({
            message: i18n.t('AbpVue["DefaultErrorMessage401Detail"]'),
            type: 'error',
            duration: 5 * 1000
          })
          await store.dispatch('user/resetToken')
          location.reload()
          resolve()
        })
      }

      if (response.status === 401) {
        if (!isRefreshing) {
          isRefreshing = true
          const token = await store.dispatch('user/refreshToken')
          response.config.headers['authorization'] = 'Bearer ' + token
          requests.forEach(fun => fun(token))//refreshToken后 执行其他的的请求
          requests = []
          isRefreshing = false
          return await service(response.config)
        } else {
          //将其他401状态的请求放入队列
          return new Promise((resolve) => {
            requests.push((token) => {
              response.config.headers['authorization'] = 'Bearer ' + token
              resolve(service(response.config))
            })
          })
        }
      } else {
        Message({
          message: (response.data.error.message || response.data.error_description),
          type: 'error',
          duration: 5 * 1000
        })
      }
    } else {
      Message({
        message: error.message,
        type: 'error',
        duration: 5 * 1000
      })
    }
    return Promise.reject(error)
  }
)

export default service
