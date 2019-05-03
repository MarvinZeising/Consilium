import Vue from 'vue'
import Router from 'vue-router'
import Home from '@/views/Home.vue'

Vue.use(Router)

export default new Router({
  mode: 'hash',
  base: process.env.BASE_URL,
  routes: [{
    path: '/home',
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
    component: () => import('./views/project/Calendar.vue')
  }, {
    path: '/project/:projectId/knowledge-base',
    name: 'knowledgeBase',
    component: () => import('./views/project/KnowledgeBase.vue')
  }, {
    path: '/project/:projectId/settings',
    name: 'settings',
    component: () => import('./views/project/Settings.vue')
  }, {
    path: '/project/:projectId/settings/update-general',
    name: 'updateGeneral',
    component: () => import('./views/project/UpdateGeneral.vue')
  }, {
    path: '/profile/configure-projects',
    name: 'configureProjects',
    component: () => import('./views/profile/ConfigureProjects.vue')
  }, {
    path: '/profile/create-project',
    name: 'createProject',
    component: () => import('./views/profile/CreateProject.vue')
  }, {
    path: '/sign-in',
    name: 'signIn',
    component: () => import('./views/SignIn.vue')
  }, {
    path: '/sign-up',
    name: 'signUp',
    component: () => import('./views/SignUp.vue')
  }, {
    path: '*',
    redirect: '/home'
  }],
})
