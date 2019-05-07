import axios from '@/tools/axios'
import { Module, VuexModule, MutationAction, Action } from 'vuex-module-decorators'
import { User } from '@/models/definitions'
import SHA512 from 'crypto-js/sha512'
import { WordArray } from 'crypto-js';

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
    const response = await axios.get('/users?email=')
    return { user: response.data }
  }

  @MutationAction({ mutate: ['user'] })
  public async signIn(credentials: { email: string, password: string }) {
    const hash: WordArray = SHA512(credentials.password + 'consilium')

    // TODO: response should contain JWT Token
    const response = await axios.post('/authenticate', {
      email: credentials.email,
      password: hash,
    })

    if (response.data !== null) {
      const user = this.fetchUser(credentials.email)
      return { user }
    }

    return { user: null }
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
