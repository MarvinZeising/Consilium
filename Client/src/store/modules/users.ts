import axios from '@/tools/axios'
import { Module, VuexModule, MutationAction } from 'vuex-module-decorators'
import { User } from '@/models/definitions'

@Module({ name: 'UserModule' })
export default class UserModule extends VuexModule {
  public user: User | null = null

  public get myUser(): User | null {
    return this.user
  }

  @MutationAction({ mutate: ['user'] })
  public async signIn(username: string, password: string) {
    // const response = await axios.post('/authenticate', {
    //   username: username,
    //   password: password,
    // })
    // return { user: response.data }
    return {
      user: {
        username: 'asdf',
        password: 'pw'
      }
    }
  }

  @MutationAction({ mutate: ['user'] })
  public async signOut() {
    // const response = await axios.get('/authenticate')
    return { user: null }
  }

}
