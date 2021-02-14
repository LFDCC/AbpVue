
import i18n from '@/lang'
import moment from 'moment'
import 'moment/locale/zh-cn'
moment.locale('zh-cn')
export function pickerRangeWithHotKey() {
  return{
  shortcuts: [{
    text: i18n.t('AbpVue.CurrentDay'),
    onClick(picker) {
      const end = moment().endOf('day').format('YYYY-MM-DD HH:mm:ss')
      const start = moment().startOf('day').format('YYYY-MM-DD HH:mm:ss')
      picker.$emit('pick', [start, end])
    }
  },
  {
    text: i18n.t('AbpVue.OneWeek'),
    onClick(picker) {
      const end = moment().endOf('day').format('YYYY-MM-DD HH:mm:ss')
      const start = moment().endOf('day').subtract(7, 'days').format('YYYY-MM-DD HH:mm:ss')
      picker.$emit('pick', [start, end])
    }
  },
  {
    text: i18n.t('AbpVue.OneMonth'),
    onClick(picker) {
      const end = moment().endOf('day').format('YYYY-MM-DD HH:mm:ss')
      const start = moment().endOf('day').subtract(1, 'months').format('YYYY-MM-DD HH:mm:ss')
      picker.$emit('pick', [start, end])
    }
  },
  {
    text: i18n.t('AbpVue.ThreeMonths'),
    onClick(picker) {
      const end = moment().endOf('day').format('YYYY-MM-DD HH:mm:ss')
      const start = moment().endOf('day').subtract(3, 'months').format('YYYY-MM-DD HH:mm:ss')
      picker.$emit('pick', [start, end])
    }
  }]
}
}
