import Vue from 'vue'
import Vuex from 'vuex'
import ProjectModule from './modules/projects'
import UserModule from './modules/users'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    ProjectModule,
    UserModule,
  }
})

export default store
