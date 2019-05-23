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
        photoUrl: '',
      }]
    }
    return { persons: response.data }
  }

  @Action({ commit: 'insertPerson' })
  public async createPerson(person: { firstname: string, lastname: string }): Promise<Person> {
    // const response = await axios.post('/persons', {
    //   firstname: person.firstname,
    //   lastname: person.lastname,
    // })
    const response = {
      data: new Person(
        'k987fhvianfdkahfsgpuh',
        person.firstname,
        person.lastname,
        '',
      )
    }
    return response.data
  }

  @Mutation
  protected insertPerson(person: Person) {
    this.persons.push(person)
  }

}
