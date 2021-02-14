import request from '@/utils/request'

/**
 * 获取服务端应用程序配置
 */
export function applicationConfiguration() {
    return request({
        url: '/api/abp/application-configuration',
        method: 'get'
    })
}

/**
 * 获取租户
 * @param {string} id 租户ID
 */
export function tenantsById(id) {
    return request({
        url: `/api/abp/multi-tenancy/tenants/by-id/${id}`,
        method: 'get'
    })
}

/**
 * 获取租户
 * @param {string} name 租户名称
 */
export function tenantsByName(name) {
    return request({
        url: `/api/abp/multi-tenancy/tenants/by-name/${name}`,
        method: 'get'
    })
}