// src/request.js

import axios from 'axios'

const instance = axios.create({
  baseURL: 'http://localhost:5003',
  withCredentials: true
})

// 请求拦截器
instance.interceptors.request.use(config => {
  const token = window.sessionStorage.getItem('token')
  if (token) {
    config.headers['Authorization'] = 'Bearer ' + token
  }
  config.headers['Content-Type'] = 'application/json'
  return config
})

// 响应拦截器
// TODO 处理Token失效，重新登录
import router from './router'
instance.interceptors.response.use(
  response => {
    // 正常响应直接返回
    return response
  },
  error => {
    // 判断是否是 401
    if (error.response && error.response.status === 401) {
      // 清除本地 token
      localStorage.removeItem('token')

      // 可选提示用户
      alert('登录已失效，请重新登录')

      // 跳转到登录页
      router.push({ path: '/Login' })

      // 或者返回一个 reject 错误
      return Promise.reject(error)
    }

    // 其他错误也要继续抛出
    return Promise.reject(error)
  }
)

export default instance
