import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)
Vue.config.devtools = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({})
