/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/layout'

const loggingRouter = {
  path: '/logging',
  component: Layout,
  redirect: 'noRedirect',
  alwaysShow: true,
  name: 'Logging',
  meta: {
    title: 'LogManagement["Menu:LogManagement"]',
    icon: 'el-icon-document',
    policy:''
  },
  children: [
    {
      path: 'auditlogging',
      component: () => import('@/views/logging/auditlogging'),
      name: 'Auditlogging',
      meta: { 
        title: 'LogManagement.AuditLogging', 
        policy: 'LogManagement.AuditLogs' 
      }
    },
    {
      path: 'securitylogging',
      component: () => import('@/views/logging/securitylogging'),
      name: 'Securitylogging',
      meta: { 
        title: 'LogManagement.SecurityLogging', 
        policy: 'LogManagement.SecurityLogs' }
    }
  ]
}

export default loggingRouter
