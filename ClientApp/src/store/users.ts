import axios from 'axios'
import moment from 'moment'
import 'moment/locale/de'
import { Module, VuexModule, Action, Mutation } from 'vuex-module-decorators'
import i18n from '../i18n'
import vuetify from '../plugins/vuetify'
import store from '../plugins/vuex'
import { User, Theme, Person, Language } from '../models'
import { getCookie, hashPassword } from './_helpers'

@Module({ dynamic: true, store, name: 'UserModule' })
export default class UserModule extends VuexModule {
  public user: User | null = null
  public version?: string = undefined

  public get getUser() {
    return this.user
  }

  public get getVersion() {
    return this.version || 'development'
  }

  @Action
  public async loadNavbar() {
    this.context.dispatch('loadVersion')

    const response = await axios.get(`/users/current`)

    this.context.commit('setUser', User.create(response.data))
    this.context.commit('applyLocale')
    this.context.commit('applyTheme')

    const persons = response.data.persons.map((rawPerson: Person) => Person.create(rawPerson))

    const currentlyActivePerson = this.context.getters.getPersons.find(
      (x: Person) => x.id === this.context.getters.getActivePersonId
    )

    this.context.commit('setPersons', persons)

    if (currentlyActivePerson && persons.includes(currentlyActivePerson)) {
      await this.context.dispatch('activatePerson', currentlyActivePerson.id)
    } else {
      const activePersonIdByCookie = getCookie('activePersonId')
      const personExists = this.context.getters.getPersons.find((x: Person) => x.id === activePersonIdByCookie)

      if (activePersonIdByCookie && personExists) {
        await this.context.dispatch('activatePerson', activePersonIdByCookie)
      } else if (persons.length > 0) {
        await this.context.dispatch('activatePerson', persons[0].id)
      }
    }
  }

  @Action
  public async loadVersion() {
    if (process.env.NODE_ENV !== 'development') {
      const response = await axios.get('/version')
      this.context.commit('setVersion', response.data)
    }
  }

  @Action
  public async isEmailAvailable(email: string) {
    const response = await axios.get(`/users/email-available/${email}`)
    return response.data
  }

  @Action
  public async signIn(credentials: { email: string; password: string }) {
    await this.context.dispatch('signOut')

    const response: any = await axios
      .post('/users/authenticate', {
        email: credentials.email,
        password: hashPassword(credentials.password),
      })
      .catch(() => false)

    if (response.data !== null && typeof response.data === 'string') {
      const jwtToken = response.data
      localStorage.setItem(
        'user',
        JSON.stringify({
          token: jwtToken,
          email: credentials.email,
        })
      )
      axios.defaults.headers.common.Authorization = `Bearer ${jwtToken}`

      await this.context.dispatch('loadNavbar', credentials.email)

      return true
    }

    return false
  }

  @Action
  public async signUp(credentials: { email: string; password: string; language: string; theme: string }) {
    await axios.post('/users', {
      email: credentials.email,
      password: hashPassword(credentials.password),
      language: credentials.language,
      theme: credentials.theme,
    })
    return true
  }

  @Action
  public async updateGeneral(email: string) {
    if (this.user) {
      await axios.put('/users', { email })
      await this.context.dispatch('signOut')
    }
  }

  @Action
  public async updateInterface(accountInterface: {
    language: Language
    theme: string
    dateFormat: string
    timeFormat: string
  }) {
    const user = this.user
    if (user) {
      await axios.put('/users/interface', {
        language: accountInterface.language,
        theme: accountInterface.theme,
        dateFormat: accountInterface.dateFormat,
        timeFormat: accountInterface.timeFormat,
      })
      user.language = accountInterface.language
      user.theme = accountInterface.theme === 'light' ? Theme.Light : Theme.Dark
      user.dateFormat = accountInterface.dateFormat
      user.timeFormat = accountInterface.timeFormat
    }
    this.context.commit('setUser', user)
    this.context.commit('applyLocale')
    this.context.commit('applyTheme')
  }

  @Action
  public async updatePassword(passwords: { old: string; new: string }) {
    if (this.user) {
      await axios.put('/users/password', {
        oldPassword: hashPassword(passwords.old),
        newPassword: hashPassword(passwords.new),
      })

      await this.context.dispatch('signOut')
    }
  }

  @Action
  public async deleteAccount() {
    if (this.getUser) {
      await axios.delete(`/users`)

      await this.context.dispatch('signOut', null)
      this.context.commit('applyTheme')
    }
  }

  @Action
  public async signOut() {
    localStorage.removeItem('user')
    await this.context.dispatch('clearPersons')
    await this.context.dispatch('clearProjects')
    this.context.commit('setUser', null)
  }

  @Mutation
  protected setUser(user: User | null) {
    this.user = user
  }

  @Mutation
  protected setVersion(version: string) {
    this.version = version
  }

  @Mutation
  protected applyLocale() {
    if (this.user) {
      i18n.locale = this.user.language
      moment.locale(this.user.language.substr(0, 2))
    }
  }

  @Mutation
  protected applyTheme() {
    if (this.user?.theme === Theme.Dark) {
      vuetify.framework.theme.dark = true
      document.getElementsByTagName('body')[0].style.backgroundColor = '#202020'
    } else {
      vuetify.framework.theme.dark = false
      document.getElementsByTagName('body')[0].style.backgroundColor = ''
    }
  }
}
