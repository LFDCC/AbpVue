import request from '@/utils/request'
import { transformAbpListQuery } from '@/utils/abp'

export function getSecurityLogs(query) {
  return request({
    url: '/api/log-management/security-logs',
    method: 'get',
    params: transformAbpListQuery(query)
  })
}

export function getSecurityLog(id) {
  return request({
    url: `/api/log-management/security-logs/${id}`,
    method: 'get'
  })
}

export function deleteSecurityLog(id) {
  return request({
    url: `/api/log-management/security-logs/${id}`,
    method: 'delete'
  })
}
