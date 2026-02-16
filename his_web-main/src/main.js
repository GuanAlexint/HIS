import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './plugins/element.js'
import axios from 'axios'


import './assets/font/iconfont.css'
import './assets/font/iconfont.js'

// 导入全局样式表
import './assets/css/global.css'


import request from './request'

Vue.prototype.$http = request


Vue.config.productionTip = false




import _ from 'lodash'







// 全局函数----------------------------------------------------------------------
function formatDateTimeToYYMMDDHHMMSS() {
  let currentDateTime = new Date()

  // 获取年、月、日
  let year = currentDateTime.getFullYear()
  let month = (currentDateTime.getMonth() + 1)
      .toString()
      .padStart(2, '0') // 月份从0开始，需要加1；padStart用于补零
  let day = currentDateTime.getDate().toString().padStart(2, '0') // 补零

  // 获取时、分、秒
  let hours = currentDateTime.getHours().toString().padStart(2, '0')
  let minutes = currentDateTime
      .getMinutes()
      .toString()
      .padStart(2, '0')
  let seconds = currentDateTime
      .getSeconds()
      .toString()
      .padStart(2, '0')
  let milliseconds = currentDateTime
      .getMilliseconds()
      .toString()
      .padStart(3,'0')
  // 拼接成yyyyMMddHHmmss形式
  let formattedDateTime = `${year.toString().slice(2)}${month}${day}${hours}${minutes}${seconds}${milliseconds}`

  return  formattedDateTime
}
window.formatDateTimeToYYMMDDHHMMSS = formatDateTimeToYYMMDDHHMMSS







function getCurrentFormattedTime() {
  const now = new Date();

  const year = now.getFullYear();
  const month = String(now.getMonth() + 1).padStart(2, '0');  // 月份从0开始
  const day = String(now.getDate()).padStart(2, '0');

  const hours = String(now.getHours()).padStart(2, '0');
  const minutes = String(now.getMinutes()).padStart(2, '0');
  const seconds = String(now.getSeconds()).padStart(2, '0');

  return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}
window.formatDateTimeToNorm = getCurrentFormattedTime










new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
