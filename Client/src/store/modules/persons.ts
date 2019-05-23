import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'

@Module({ name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public activePersonId: string | null = 'asdf'
  public persons: Person[] = []

  public get myPersons(): Person[] {
    return this.persons
  }

  public get getActivePerson(): Person {
    return this.persons.filter((x) => x.id === this.activePersonId)[0]
 }

  @Action({ commit: 'setPersons' })
  public async fetchPersons() {
    // const response = await axios.get('/persons')
    const response = {
      data: [{
        id: 'asdf',
        firstname: 'Marvin',
        lastname: 'Zeising',
        photoUrl: 'https://randomuser.me/api/portraits/men/85.jpg',
      }, {
        id: 'asdf2',
        firstname: 'Boas',
        lastname: 'Lehrke',
        photoUrl: 'https://randomuser.me/api/portraits/men/43.jpg',
      }]
    }
    const persons: Person[] = response.data.map((data) => {
      const person = new Person(
        data.id,
        data.firstname,
        data.lastname,
        data.photoUrl,
      )
      if (person.id === this.activePersonId) {
        person.isActive = true
      }
      return person
    })
    if (!this.activePersonId && persons.length > 0) {
      this.context.dispatch('activatePerson', persons[0].id)
    }
    return persons
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
  protected setPersons(persons: Person[]) {
    this.persons = persons
  }

  @Mutation
  protected insertPerson(person: Person) {
    this.persons.push(person)
  }

  @Mutation
  protected setIsActive(personId: string) {
    this.activePersonId = personId
  }

}
