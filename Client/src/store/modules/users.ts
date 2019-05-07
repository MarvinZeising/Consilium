import axios from '@/tools/axios'
import { Module, VuexModule, MutationAction } from 'vuex-module-decorators'
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
  public async signIn(credentials: { username: string, password: string }) {
    const hash: WordArray = SHA512(credentials.password + 'consilium')

    const response = await axios.post  ('/authenticate', {
      username: credentials.username,
      password: hash,
    })

    if (response.data !== null) {
      return { user: response.data.fields[0] }
    }

    return { user: null }
  }

  @MutationAction({ mutate: ['user'] })
  public async signOut() {
    // const response = await axios.get('/authenticate')
    return { user: null }
  }

}
