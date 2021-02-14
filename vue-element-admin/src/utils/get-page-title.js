import settings from '@/settings'
import i18n from '@/lang'

export default function getPageTitle(key) {
  const hasKey = i18n.te(key)
  if (hasKey) {
    const pageName = i18n.t(key)
    return `${pageName} - ${settings.title}`
  }
  return `${settings.title}`
}
