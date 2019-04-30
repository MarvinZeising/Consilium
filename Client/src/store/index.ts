import Vue from 'vue'
import Vuex from 'vuex'
import ProjectModule from './modules/projects'

Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    ProjectModule
  }
})

export default store
