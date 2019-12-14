import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'
import router from './router'
import 'roboto-fontface/css/roboto/roboto-fontface.css'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import store from './store'
import axios from 'axios'
import { getModule } from 'vuex-module-decorators'
import UserModule from './store/modules/users'
import PersonModule from './store/modules/persons'
import ProjectModule from './store/modules/projects'
import i18n from './i18n'
import KnowledgeBaseModule from './store/modules/knowledgeBase'

async function init() {
  const userModule = getModule(UserModule, store)
  const personModule = getModule(PersonModule, store)
  const projectModule = getModule(ProjectModule, store)
  const knowledgeBaseModule = getModule(KnowledgeBaseModule, store)

  Vue.config.productionTip = false

  axios.defaults.baseURL = process.env.VUE_APP_SERVER_URL || 'http://localhost:5000'
  axios.defaults.timeout = 3000
  axios.interceptors.response.use((response) => {
    return response
  }, (error) => {
    // * automatically sign out if token is expired or invalid
    if ((error.response.status === 401 || error.response.status === 404)
      && localStorage.getItem('user')) {
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

    // keep in sync with modules/users.ts
    await userModule.initUserModule()
    await personModule.initPersonModule()
    await projectModule.initProjectModule()
    await knowledgeBaseModule.initKnowledgeBaseModule()
  }

  new Vue({
    store,
    router,
    i18n,
    render: (h) => h(App)
  }).$mount('#app')
}

init()
