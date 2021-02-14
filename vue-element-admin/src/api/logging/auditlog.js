import request from '@/utils/request'
import { transformAbpListQuery } from '@/utils/abp'

export function getAuditLogs(query) {
  return request({
    url: '/api/log-management/audit-logs',
    method: 'get',
    params: transformAbpListQuery(query)
  })
}

export function getAuditLog(id) {
  return request({
    url: `/api/log-management/audit-logs/${id}`,
    method: 'get'
  })
}

export function deleteAuditLog(id) {
  return request({
    url: `/api/log-management/audit-logs/${id}`,
    method: 'delete'
  })
}
