import axios from 'axios'
import { Module, VuexModule, Action, Mutation } from 'vuex-module-decorators'
import router from '../router'
import { Person, Gender, Project, Participation } from '../models/definitions'
import store from '../plugins/vuex'

@Module({ dynamic: true, store, name: 'PersonModule' })
export default class PersonModule extends VuexModule {
  public activePersonId: string | null = null
  public persons: Person[] = []

  public get getPersons() {
    return this.persons
  }

  public get getActivePerson() {
    return this.persons.find((x) => x.id === this.activePersonId)
  }

  public get getActivePersonId() {
    return this.getActivePerson?.id
  }

  public get getActiveRole() {
    return this.getActivePerson
      ?.participations
      ?.find((x) => x.projectId === router.currentRoute.params.projectId)
      ?.role
  }

  @Action
  public async loadPersons(rawPersons: Person[]) {
    const persons = rawPersons.map((rawPerson: Person) => {
      const person = Person.create(rawPerson)
      person.gender = rawPerson.gender
      return person
    })

    const currentlyActivePerson = this.persons.find((x) => x.id === this.activePersonId)

    this.context.commit('setPersons', persons)

    if (currentlyActivePerson && persons.includes(currentlyActivePerson)) {
      await this.context.dispatch('activatePerson', currentlyActivePerson.id)
    } else {
      await this.context.dispatch('activatePerson', persons[0].id)
    }
  }

  @Action
  public async clearPersons() {
    this.context.commit('setPersons', [])
    await this.context.dispatch('activatePerson', null)
  }

  @Action
  public async createPerson(person: { firstname: string, lastname: string }) {
    const response = await axios.post('/persons', {
      firstname: person.firstname,
      lastname: person.lastname,
      gender: Gender.Male,
    })
    const newPerson = Person.create(response.data)

    this.context.commit('insertPerson', newPerson)
    await this.context.dispatch('activatePerson', newPerson.id)
  }

  @Action({ commit: 'setGeneral' })
  public async updatePersonGeneral(person: {
    id: string,
    firstname: string,
    lastname: string,
    gender: string,
  }) {
    await axios.put(`/persons/${person.id}`, {
      firstname: person.firstname,
      lastname: person.lastname,
      gender: person.gender,
    })
    return person
  }

  @Action({ commit: 'removePerson' })
  public async deletePerson(personId: string) {
    await axios.delete(`/persons/${personId}`)

    const otherPerson = this.persons.find((x: Person) => x.id !== personId)
    if (otherPerson) {
      await this.context.dispatch('activatePerson', otherPerson.id)
    }

    return personId
  }

  @Action
  public async activatePerson(personId: string | null) {
    this.context.commit('setActivePersonId', personId)

    await this.context.dispatch('clearProjects')

    if (this.getActivePerson?.participations) {
      for (const participation of this.getActivePerson?.participations) {
        await this.context.dispatch('loadProject', participation.projectId)
      }
    }
  }

  @Mutation
  public setActivePersonId(personId: string | null) {
    this.activePersonId = personId
  }

  @Mutation
  protected setPersons(persons: Person[]) {
    this.persons = persons
  }

  @Mutation
  protected setGeneral(person: Person) {
    this.persons = this.persons.map((x: Person) => {
      if (x.id === person.id) {
        x.firstname = person.firstname
        x.lastname = person.lastname
        x.gender = person.gender
      }
      return x
    })
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
