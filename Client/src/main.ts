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
import PersonModule from './store/modules/persons';
import ProjectModule from './store/modules/projects';

async function init() {
  const userModule = getModule(UserModule, store)
  const personModule = getModule(PersonModule, store)
  const projectModule = getModule(ProjectModule, store)

  Vue.config.productionTip = false

  axios.defaults.baseURL = 'http://localhost:5000';
  axios.interceptors.response.use((response) => {
    return response;
  }, (error) => {
    // * automatically sign out if token is expired
    if (error.response.status === 401 && localStorage.getItem('user')) {
      userModule.signOut()
      location.reload(true)
      // TODO: communicate signOut via an alert
    }
    return Promise.reject(error);
  });

  const userItem = localStorage.getItem('user')
  if (userItem) {
    const user = JSON.parse(userItem)
    axios.defaults.headers.common.Authorization = `Bearer ${user.token}`

    await userModule.initUserModule()
    await personModule.initPersonModule()
    await projectModule.initProjectModule()
  }

  new Vue({
    store,
    router,
    render: (h) => h(App)
  }).$mount('#app')
}

init()
