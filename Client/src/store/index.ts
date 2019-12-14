import Vue from 'vue'
import Vuex from 'vuex'
import UserModule from './modules/users'
import PersonModule from './modules/persons'
import ProjectModule from './modules/projects'
import KnowledgeBaseModule from './modules/knowledgeBase'
import AlertModule from './modules/alerts'

Vue.use(Vuex)
Vue.config.devtools = process.env.NODE_ENV !== 'production'

const store = new Vuex.Store({
  modules: {
    UserModule,
    PersonModule,
    ProjectModule,
    KnowledgeBaseModule,
    AlertModule,
  }
})

export default store
