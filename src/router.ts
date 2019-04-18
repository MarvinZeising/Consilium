import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [{
    path: '/',
    name: 'home',
    component: Home
  }, {
    path: '/terms-of-use',
    name: 'termsOfUse',
    component: () => import('./views/TermsOfUse.vue')
  }, {
    path: '/privacy-policy',
    name: 'privacyPolicy',
    component: () => import('./views/PrivacyPolicy.vue')
  }, {
    path: '/project/:projectId/calendar',
    name: 'calendar',
    component: () => import('./views/Project/Calendar.vue')
  }, {
    path: '/project/:projectId/knowledge-base',
    name: 'knowledgeBase',
    component: () => import('./views/Project/KnowledgeBase.vue')
  }, {
    path: '/project/:projectId/settings',
    name: 'settings',
    component: () => import('./views/Project/Settings.vue')
  }, {
    path: '/profile/configure-projects',
    name: 'configureProjects',
    component: () => import('./views/Profile/ConfigureProjects.vue')
  }, {
    path: '/sign-up',
    name: 'signUp',
    component: () => import('./views/SignUp.vue')
  }, {
    path: '*',
    redirect: '/'
  }],
})
