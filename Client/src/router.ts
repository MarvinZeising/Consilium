import Vue from 'vue'
import Router, { Route } from 'vue-router'
import Home from '@/views/Home.vue'

Vue.use(Router)

const router = new Router({
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
    path: '/account',
    name: 'account',
    component: () => import('./views/account/Account.vue')
  }, {
    path: '/account/update-password',
    name: 'updateAccountPassword',
    component: () => import('./views/account/UpdateAccountPassword.vue')
  }, {
    path: '/account/update-general',
    name: 'updateAccountGeneral',
    component: () => import('./views/account/UpdateAccountGeneral.vue')
  }, {
    path: '/account/update-language',
    name: 'updateAccountLanguage',
    component: () => import('./views/account/UpdateAccountLanguage.vue')
  }, {
    path: '/sign-in',
    name: 'signIn',
    component: () => import('./views/SignIn.vue')
  }, {
    path: '/sign-out',
    name: 'signOut',
    component: () => import('./views/SignOut.vue')
  }, {
    path: '/sign-up',
    name: 'signUp',
    component: () => import('./views/SignUp.vue')
  }, {
    path: '*',
    redirect: '/home'
  }],
})

router.beforeEach((to: Route, from: Route, next) => {
  const publicPages: any[] = [
    'signIn',
    'signUp'
  ]
  const authRequird: boolean = !publicPages.includes(to.name)
  const loggedIn = localStorage.getItem('user')

  if (authRequird && !loggedIn) {
    return next({ name: 'signIn', query: { afterSignIn: to.fullPath } })
  }

  next()
})

export default router
