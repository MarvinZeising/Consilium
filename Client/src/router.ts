import Vue from 'vue'
import VueRouter, { Route } from 'vue-router'
import Home from './views/Home.vue'
import ServerException from './views/ServerException.vue'
import SignIn from './views/authentication/SignIn.vue'
import SignOut from './views/authentication/SignOut.vue'

Vue.use(VueRouter)

const router = new VueRouter({
  mode: 'hash',
  base: process.env.BASE_URL,
  routes: [{
    path: '/home',
    name: 'home',
    component: Home,
  }, {
    path: '/server-exception',
    name: 'serverException',
    component: ServerException,
  }, {
    path: '/sign-in',
    name: 'signIn',
    component: SignIn,
  }, {
    path: '/sign-out',
    name: 'signOut',
    component: SignOut,
  }, {
    path: '/sign-up',
    name: 'signUp',
    component: () => import('./views/authentication/SignUp.vue'),
  }, {
    path: '/terms-of-use',
    name: 'termsOfUse',
    component: () => import('./views/TermsOfUse.vue'),
  }, {
    path: '/privacy-policy',
    name: 'privacyPolicy',
    component: () => import('./views/PrivacyPolicy.vue'),
  }, {
    path: '/project/:projectId/calendar',
    name: 'calendar',
    component: () => import('./views/project/Calendar.vue'),
  }, {
    path: '/project/:projectId/topic/:topicId',
    name: 'topic',
    component: () => import('./views/project/Topic.vue'),
  }, {
    path: '/project/:projectId/topic/:topicId/article/:articleId',
    name: 'article',
    component: () => import('./views/project/Article.vue'),
  }, {
    path: '/project/:projectId/settings',
    name: 'settings',
    component: () => import('./views/project/Settings.vue'),
  }, {
    path: '/project/:projectId/participants',
    name: 'participants',
    component: () => import('./views/project/Participants.vue'),
  }, {
    path: '/project/:projectId/congregations',
    name: 'congregations',
    component: () => import('./views/project/Congregations.vue'),
  }, {
    path: '/profile/personal',
    name: 'personal',
    component: () => import('./views/profile/Personal.vue'),
  }, {
    path: '/profile/configure-projects',
    name: 'configureProjects',
    component: () => import('./views/profile/ConfigureProjects.vue'),
  }, {
    path: '/account',
    name: 'account',
    component: () => import('./views/account/Account.vue'),
  }, {
    path: '/account/create-person',
    name: 'createPerson',
    component: () => import('./views/account/CreatePerson.vue'),
  }, {
    path: '/token',
    name: 'token',
    component: () => import('./views/authentication/Token.vue'),
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
  const authRequird = !publicPages.includes(to.name)
  const loggedIn = localStorage.getItem('user')

  if (authRequird && !loggedIn) {
    return next({ name: 'signIn', query: { afterSignIn: to.fullPath } })
  }

  next()
})

export default router
