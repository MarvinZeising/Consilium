import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'

@Module({ name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public activePersonId: string | null = null
  public persons: Person[] = []

  public get getPersons(): Person[] {
    return this.persons
  }

  public get getActivePerson(): Person | undefined {
    return this.persons.find((x) => x.id === this.activePersonId)
 }

  @Action
  public async initPersonModule() {
    const response = await axios.get('/persons')

    if (response.data && response.data.length > 0) {
      const currentlyActivePerson = this.persons.find((x) => x.id === this.activePersonId)

      const persons = response.data.map((data: Person) => {
        return new Person(
          data.id,
          data.firstname,
          data.lastname,
        )
      })

      this.context.commit('setPersons', persons)

      if (currentlyActivePerson && persons.includes(currentlyActivePerson)) {
        this.context.commit('activatePerson', currentlyActivePerson.id)
      } else {
        this.context.commit('activatePerson', persons[0].id)
      }
    }
  }

  @Action
  public async clearPersons() {
    this.context.commit('setPersons', [])
    this.context.commit('activatePerson', null)
  }

  @Action
  public async createPerson(person: { firstname: string, lastname: string }) {
    const response = await axios.post('/persons', {
      firstname: person.firstname,
      lastname: person.lastname,
    })

    const newPerson = new Person(
      response.data.id,
      response.data.firstname,
      response.data.lastname)

    this.context.commit('insertPerson', newPerson)
    this.context.commit('activatePerson', newPerson.id)

    await this.context.dispatch('initProjectModule')
    await this.context.dispatch('initKnowledgeBaseModule')
  }

  @Action({ commit: 'removePerson' })
  public async deletePerson(personId: string) {
    await axios.delete(`/persons/${personId}`)

    const otherPerson = this.persons.find((x: Person) => x.id !== personId)
    if (otherPerson) {
      this.context.commit('activatePerson', otherPerson.id)
    }

    return personId
  }

  @Mutation
  public activatePerson(personId: string | null) {
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

  @Mutation
  protected removePerson(personId: string) {
    const person = this.persons.find((x: Person) => x.id === personId)
    if (person) {
      this.persons.splice(this.persons.indexOf(person), 1)
    }
  }

}
