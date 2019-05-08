import axios from '@/tools/axios'
import { Module, VuexModule, MutationAction, Action } from 'vuex-module-decorators'
import { User } from '@/models/definitions'
import SHA512 from 'crypto-js/sha512'
import { WordArray } from 'crypto-js'

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
      // TODO: put jwtToken into authentication header

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
    // const response = await axios.get('/authenticate')
    return { user: null }
  }

}
