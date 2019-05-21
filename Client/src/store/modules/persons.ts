import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'

@Module({ name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public persons: Person[] = []

  public get myPersons(): Person[] {
    return this.persons
  }

  @MutationAction({ mutate: ['persons'] })
  public async fetchPersons() {
    // const response = await axios.get('/persons')
    const response = {
      data: [{
        id: 'asdf',
        firstname: 'Marvin',
        lastname: 'Zeising',
        photoUrl: ''
      }]
    }
    return { persons: response.data }
  }

}
