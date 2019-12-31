import Vue from 'vue'
import axios from 'axios'
import { getModule } from 'vuex-module-decorators'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import store from './plugins/vuex'
import router from './router'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import i18n from './i18n'

import AlertModule from './store/alerts'
import UserModule from './store/users'

async function init() {
  const alertModule = getModule(AlertModule)
  const userModule = getModule(UserModule)

  Vue.config.productionTip = false

  axios.defaults.baseURL = process.env.VUE_APP_SERVER_URL || 'http://localhost:5000/api'
  axios.defaults.timeout = 5000
  axios.interceptors.response.use(
    (response) => {
      return response
    },
    (error) => {
      if (!error.response) {
        alertModule.showAlert({
          message: error.message,
          color: 'error',
          timeout: 5000,
        })
      } else if ((error.response.status === 401) && localStorage.getItem('user')) {
        // * automatically sign out if token is expired or invalid
        userModule.signOut()
        location.reload(true)
        // TODO: communicate signOut via an alert
      }
      return Promise.reject(error)
    })

  const userItem = localStorage.getItem('user')
  if (userItem) {
    const user = JSON.parse(userItem)
    axios.defaults.headers.common.Authorization = `Bearer ${user.token}`

    try {
      await userModule.initStore()

      if (router.currentRoute.name === 'serverException') {
        router.push({ name: 'home' })
      }
    } catch (error) {
      if (!error.response) {
        router.push({ name: 'serverException' })
      }
    }
  }

  new Vue({
    vuetify,
    store,
    router,
    i18n,
    render: (h) => h(App)
  }).$mount('#app')
}

init()
