import Vue from 'vue'
import axios from 'axios'

import { getModule } from 'vuex-module-decorators'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import store from './plugins/vuex'
import './plugins/showdown'
import router from './router'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import i18n from './i18n'

import AlertModule from './store/alerts'
import UserModule from './store/users'

Vue.config.warnHandler = (msg: string | null, vm: Vue | null, trace: string | null) => {
  const ignoreWarnMessage = 'The .native modifier for v-on is only valid on components but it was used on <div>.'
  const route = router.currentRoute
  if (route.name === 'calendar' && msg === ignoreWarnMessage) {
    msg = null
    vm = null
    trace = null
  }
}

async function init() {
  Vue.config.productionTip = false

  new Vue({
    vuetify,
    store,
    router,
    i18n,
    render: (h) => h(App),
  }).$mount('#app')

  console.log(process.env)

  axios.defaults.baseURL = process.env.VUE_APP_SERVER_URL || 'https://localhost:5001/api'
  axios.defaults.timeout = 5000
  axios.interceptors.response.use(
    (response) => {
      return response
    },
    async (error) => {
      if (!error.response) {
        alertModule.showAlert({
          message: error.message,
          color: 'error',
          timeout: 5000,
        })
      } else if ([400, 500].includes(error.response.status)) {
        alertModule.showAlert({
          message: error.message,
          color: 'error',
          timeout: 5000,
        })
      } else if (error.response.status === 401 && localStorage.getItem('user')) {
        // * automatically sign out if token is expired or invalid
        await userModule.signOut()
        router.push({
          name: 'signIn',
          query: { afterSignIn: router.currentRoute.fullPath },
        })

        alertModule.showAlert({
          message: i18n.t('core.unauthorizedAlert').toString(),
          color: 'info',
          timeout: 5000,
        })
      }
      return Promise.reject(error)
    }
  )

  const alertModule = getModule(AlertModule)
  const userModule = getModule(UserModule)

  const userItem = localStorage.getItem('user')
  if (userItem) {
    const user = JSON.parse(userItem)
    axios.defaults.headers.common.Authorization = `Bearer ${user.token}`

    try {
      await userModule.loadNavbar()

      if (router.currentRoute.name === 'serverException') {
        router.push({ name: 'home' })
      }
    } catch (error) {
      if (!error.response) {
        router.push({ name: 'serverException' })
      }
    }
  }

  alertModule.updateLoading(false)
}

init()
