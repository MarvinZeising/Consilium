import axios from 'axios'
import { Module, VuexModule, Action, Mutation } from 'vuex-module-decorators'
import SHA512 from 'crypto-js/sha512'
import { User } from '@/models/definitions'

@Module({ name: 'UserModule' })
export default class UserModule extends VuexModule {
  public user: User | null = null

  public get myUser(): User | null {
    return this.user
  }

  public get isSignedIn(): boolean {
    return this.user !== null
  }

  @Action({ commit: 'setUser' })
  public async fetchUser() {
    const response = await axios.get(`/user`)
    return response.data
  }

  @Action
  public async isEmailAvailable(email: string) {
    const response = await axios.get(`/users/email-available/${email}`)
    return response.data
  }

  @Action
  public async signIn(credentials: { email: string, password: string }) {
    const response = await axios.post('/authenticate', {
      email: credentials.email,
      password: hashPassword(credentials.password),
    })

    if (response.data !== null) {
      const jwtToken = response.data.fields[0]
      localStorage.setItem('user', JSON.stringify({
        token: jwtToken,
        email: credentials.email,
      }));
      axios.defaults.headers.common.Authorization = `Bearer ${jwtToken}`;

      await this.context.dispatch('fetchUser', credentials.email)
      await this.context.dispatch('fetchProjects')

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
  public async deleteAccount() {
    if (this.isSignedIn) {
      await axios.delete(`/user`)

      this.context.dispatch('signOut', null)
    }
  }

  @Action({ commit: 'setUser'})
  public async signOut() {
    localStorage.removeItem('user')
    await this.context.dispatch('clearProjects')
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
