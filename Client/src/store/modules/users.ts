import axios from 'axios'
import { Module, VuexModule, Action, Mutation } from 'vuex-module-decorators'
import SHA512 from 'crypto-js/sha512'
import { User } from '@/models/definitions'
import i18n from '@/i18n'

@Module({ name: 'UserModule' })
export default class UserModule extends VuexModule {
  public user: User | null = null

  public get myUser(): User | null {
    return this.user
  }

  public get isSignedIn(): boolean {
    return this.user !== null
  }

  @Action
  public async initUserModule() {
    const response = await axios.get(`/user`)

    this.context.commit('setUser', response.data)
    this.context.dispatch('updateLocale')
  }

  @Action
  public async updateLocale() {
    if (this.user) {
      i18n.locale = this.user.language
    }
  }

  @Action
  public async isEmailAvailable(email: string) {
    const response = await axios.get(`/users/email-available/${email}`)
    return response.data
  }

  @Action
  public async signIn(credentials: { email: string, password: string }) {
    await this.context.dispatch('signOut')

    const response: any = await axios.post('/authenticate', {
      email: credentials.email,
      password: hashPassword(credentials.password),
    }).catch(() => false)

    if (response.data !== null && typeof(response.data) === 'string') {
      const jwtToken = response.data
      localStorage.setItem('user', JSON.stringify({
        token: jwtToken,
        email: credentials.email,
      }))
      axios.defaults.headers.common.Authorization = `Bearer ${jwtToken}`

      // keep in sync with main.ts
      await this.context.dispatch('initUserModule', credentials.email)
      await this.context.dispatch('initPersonModule')
      await this.context.dispatch('initProjectModule')
      await this.context.dispatch('initKnowledgeBaseModule')

      return true
    }

    return false
  }

  @Action
  public async signUp(credentials: { email: string, password: string }) {
    await axios.post('/users', {
      email: credentials.email,
      password: hashPassword(credentials.password),
    })
    return true
  }

  @Action
  public async updateEmail(email: string) {
    if (this.user) {
      await axios.put('/user/email', {
        email,
      })

      await this.context.dispatch('signOut')
    }
  }

  @Action
  public async updateLanguage(language: string) {
    const user = this.user
    if (user) {
      await axios.put('/user/language', {
        language,
      })
      user.language = language
    }
    this.context.commit('setUser', user)
    this.context.dispatch('updateLocale')
  }

  @Action
  public async updatePassword(passwords: { old: string, new: string }) {
    if (this.user) {
      await axios.put('/user/password', {
        oldPassword: hashPassword(passwords.old),
        newPassword: hashPassword(passwords.new),
      })

      await this.context.dispatch('signOut')
    }
  }

  @Action
  public async deleteAccount() {
    if (this.isSignedIn) {
      await axios.delete(`/user`)

      this.context.dispatch('signOut', null)
    }
  }

  @Action({ commit: 'setUser'})
  public async signOut() {
    localStorage.removeItem('user')
    this.context.dispatch('clearPersons', [])
    this.context.dispatch('clearProjects', [])
    this.context.dispatch('clearKnowledgeBases', [])
    return null
  }

  @Mutation
  protected setUser(user: User | null) {
    this.user = user
  }

}

function hashPassword(password: string) {
  return SHA512(password + 'consilium').toString()
}
