import request from "@/utils/request";
import { transformAbpListQuery } from '@/utils/abp'
/**
 * 获取用户详情
 */
export function getInfo() {
  return request({
    url: "/api/identity/my-profile",
    method: "get"
  });
}
/**
 * 获取头像
 * @param {String} blobName 图片名称
 */
export function getAvatar(blobName) {
  return `${process.env.VUE_APP_BASE_API}/api/identity/my-profile/picture/${blobName}`;
}
/**
 * 保存头像
 * @param {FormData} data 文件
 */
export function saveAvatar(data) {
  return request({
    url: "/api/identity/my-profile/picture",
    method: "post",
    data: data
  });
}
/**
 * 修改个人资料
 * @param {JSON} data 参数
 */
export function setUserInfo(data) {
  return request({
    url: "/api/identity/my-profile",
    method: "put",
    data: data
  });
}
/**
 * 修改密码
 * @param {JSON} data 参数
 */
export function changePassword(data) {
  return request({
    url: "/api/identity/my-profile/change-password",
    method: "post",
    data: data
  });
}
/**
 * 获取我的安全日志
 * @param {JSON} query 参数
 */
export function getSecurityLogs(query) {
  return request({
    url: "/api/identity/my-profile/security-logs",
    method: 'get',
    params: transformAbpListQuery(query)
  })
}