import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'

@Module({ name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public persons: Person[] = []

  public get myPersons(): Person[] {
    return this.persons
  }

  public get getActivePerson(): Person | null {
    const persons = this.persons.filter((x) => x.isActive)
    if (persons.length > 0) {
      return persons[0]
    }
    return null
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
      }, {
        id: 'asdf2',
        firstname: 'Boas',
        lastname: 'Lehrke',
        photoUrl: '',
      }]
    }
    const persons: Person[] = response.data.map((data) => {
      return new Person(
        data.id,
        data.firstname,
        data.lastname,
        data.photoUrl,
      )
    })
    persons[0].isActive = true
    return { persons }
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

  @Action({ commit: 'setIsActive' })
  public async activatePerson(personId: string) {
    // await axios.put(`/projects/${project.id}`, {
    //   name: project.name,
    //   email: project.email
    // })
    return personId
  }

  @Mutation
  protected insertPerson(person: Person) {
    this.persons.push(person)
  }

  @Mutation
  protected setIsActive(personId: string) {
    this.persons = this.persons.map((x: Person) => {
      if (x.id === personId) {
        x.isActive = true
      } else if (x.isActive === true) {
        x.isActive = false
      }
      return x
    })
  }

}
