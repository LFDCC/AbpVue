import request from '@/utils/request'
import qs from 'querystring'

/**
 * 登录
 * @param {JSON} data 用户凭据
 */
export function login(data) {
  return request({
    url: '/connect/token',
    method: 'post',
    headers: { 'content-type': 'application/x-www-form-urlencoded' },
    data: qs.stringify(data)
  })
}


/**
 * 退出
 */
export function logout() {
  return request({
    url: '/api/account/logout',
    method: 'get'
  })
}
/**
 * 刷新Token
 * @param {JSON} data 请求参数
 */
export function refreshToken(data) {
  return request({
    url: '/connect/token',
    method: 'post',
    headers: {
      'refresh_token': true,
      'content-type': 'application/x-www-form-urlencoded'
    },
    data: qs.stringify(data)
  })
}
