import { Module, VuexModule, Action, Mutation } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'

@Module({ name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public activePersonId: string | null = 'asdf'
  public persons: Person[] = []

  public get myPersons(): Person[] {
    return this.persons
  }

  public get getActivePerson(): Person {
    const persons = this.persons.filter((x) => x.id === this.activePersonId)
    if (persons.length > 0) {
      return persons[0]
    }
    return new Person('tempGuid', 'Loading...', 'Loading...', '')
 }

  @Action
  public async initPersonModule() {
    // const response = await axios.get('/persons')
    const currentlyActivePerson: Person | undefined = this.persons.find((x) => x.id === this.activePersonId)

    // * fake database
    const response = {
      data: [{
        id: 'asdf',
        firstname: 'Marvin',
        lastname: 'Zeising',
        photoUrl: 'https://randomuser.me/api/portraits/men/21.jpg',
      }, {
        id: 'asdf2',
        firstname: 'Timon',
        lastname: 'Loeffen',
        photoUrl: 'https://randomuser.me/api/portraits/men/0.jpg',
      }]
    }
    if (this.persons.length > 0) {
      response.data = this.persons
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

    this.context.commit('setPersons', persons)

    if (currentlyActivePerson && persons.includes(currentlyActivePerson)) {
      this.context.commit('activatePerson', currentlyActivePerson.id)
    } else {
      this.context.commit('activatePerson', persons[0].id)
    }
  }

  @Action
  public async createPerson(person: { firstname: string, lastname: string }) {
    // const response = await axios.post('/persons', {
    //   firstname: person.firstname,
    //   lastname: person.lastname,
    // })
    const response = {
      data: new Person(
        'k987fhvianfdkahfsgpuh',
        person.firstname,
        person.lastname,
        'https://randomuser.me/api/portraits/men/30.jpg',
      )
    }
    await this.context.commit('insertPerson', response.data)
    await this.context.commit('activatePerson', response.data.id)
  }

  @Mutation
  public activatePerson(personId: string) {
    this.activePersonId = personId
  }

  @Mutation
  protected setPersons(persons: Person[]) {
    this.persons = persons
  }

  @Mutation
  protected insertPerson(person: Person) {
    this.persons.push(person)
  }

}
