import Layout from '@/layout'

const settingRouter = {
  path: '/settings',
  component: Layout,
  children: [
    {
      path: 'setting',
      component: () => import('@/views/settings/setting'),
      name: 'Setting',
      meta: {
        title: 'AbpSettingManagement["Settings"]',
        policy: 'SettingUi.ShowSettingPage',
        icon: 'el-icon-setting'
      }
    }
  ]
}

export default settingRouter
