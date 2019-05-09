import axios from 'axios'
import { Module, VuexModule, MutationAction, Action } from 'vuex-module-decorators'
import { WordArray } from 'crypto-js'
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

  @MutationAction({ mutate: ['user'] })
  public async fetchUser(email: string) {
    const response = await axios.get(`/users?email=${email}`)
    return { user: response.data }
  }

  @Action
  public async signIn(credentials: { email: string, password: string }) {
    const hash: WordArray = SHA512(credentials.password + 'consilium')

    const response = await axios.post('/authenticate', {
      email: credentials.email,
      password: hash.toString(),
    })

    if (response.data !== null) {
      const jwtToken = response.data.fields[0]
      localStorage.setItem('user', JSON.stringify({
        token: jwtToken,
        email: credentials.email
      }));
      axios.defaults.headers.common.Authorization = `Bearer ${jwtToken}`;

      await this.context.dispatch('fetchUser', credentials.email)
      await this.context.dispatch('fetchProjects')

      return true
    }

    return false
  }

  @Action
  public async isEmailAvailable(email: string) {
    const response = await axios.get(`/users/email-available/${email}`)
    return response.data
  }

  @Action
  public async signUp(credentials: { email: string, password: string }) {
    const hash: WordArray = SHA512(credentials.password + 'consilium')

    await axios.post('/users', {
      email: credentials.email,
      password: hash.toString(),
    })
    return true
  }

  @MutationAction({ mutate: ['user'] })
  public async signOut() {
    localStorage.removeItem('user')
    await this.context.dispatch('clearProjects')
    return { user: null }
  }

}
