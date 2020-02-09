import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import { Person, Gender, Participation, Assignment, Privilege, Language } from '../models'
import { setCookie } from './_helpers'

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

  @MutationAction({ mutate: ['persons', 'activePersonId'] })
  public async clearPersons() {
    return { persons: [], activePersonId: null }
  }

  @Action
  public async createPerson(person: { firstname: string, lastname: string }) {
    const response = await axios.post('/persons', {
      firstname: person.firstname,
      lastname: person.lastname,
      gender: Gender.Male,
    })
    const newPerson = Person.create(response.data)

    this.context.commit('upsertPerson', newPerson)
    await this.context.dispatch('activatePerson', newPerson.id)
  }

  @Action
  public async updatePersonGeneral(person: {
    id: string,
    firstname: string,
    lastname: string,
    gender: string,
  }) {
    const response = await axios.put(`/persons/${person.id}/general`, {
      firstname: person.firstname,
      lastname: person.lastname,
      gender: person.gender,
    })
    const updatedPerson = Person.create(response.data)

    this.context.commit('upsertPerson', updatedPerson)
  }

  @Action
  public async updatePersonTheocratic(data: {
    assignment: Assignment,
    privilege: Privilege,
    congregationId?: string,
  }) {
    const response = await axios.put(`/persons/${this.getActivePersonId}/theocratic`, {
      assignment: data.assignment,
      privilege: data.privilege,
      congregationId: data.congregationId,
    })
    const updatedPerson = Person.create(response.data)

    this.context.commit('upsertPerson', updatedPerson)
  }

  @Action
  public async updatePersonMiscellaneous(data: {
    languages: string,
    notes: string,
  }) {
    const response = await axios.put(`/persons/${this.getActivePersonId}/miscellaneous`, {
      languages: data.languages,
      notes: data.notes,
    })
    const updatedPerson = Person.create(response.data)

    this.context.commit('upsertPerson', updatedPerson)
  }

  @Action
  public async updatePersonContact(data: {
    email: string,
    language: Language,
    phone: string,
  }) {
    const response = await axios.put(`/persons/${this.getActivePersonId}/contact`, {
      email: data.email,
      language: data.language,
      phone: data.phone,
    })
    const updatedPerson = Person.create(response.data)

    this.context.commit('upsertPerson', updatedPerson)
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

    const response = await axios.get(`/persons/${personId}/participations`)
    const participations = response.data.map((x: Participation) => Participation.create(x))

    const person = this.persons.find((x) => x.id === personId)
    if (person) {
      person.participations = participations
      this.context.commit('upsertPerson', person)

      for (const participation of participations) {
        this.context.commit('upsertProject', participation.project)
      }
    }
  }

  @Mutation
  public upsertPerson(person: Person) {
    if (this.persons.find((x) => x.id === person.id)) {
      this.persons = this.persons.map((p) => {
        if (p.id === person.id) {
          p.copyFrom(person)
        }
        return p
      })
    } else {
      this.persons.push(person)
    }
  }

  @Mutation
  public upsertPersonParticipations(participations: Participation[]) {
    this.persons = this.persons.map((person) => {
      if (person.id === this.activePersonId) {
        person.participations = person.participations.map((participation) => {
          const updatedParticipation = participations.find((y) => y.id === participation.id)
          if (updatedParticipation) {
            participation.copyFrom(updatedParticipation)
          }
          return participation
        })
        for (const updatedParticipation of participations) {
          const alreadyExists = person.participations.find((x) => x.id === updatedParticipation.id)
          if (!alreadyExists) {
            person.participations.push(updatedParticipation)
          }
        }
      }
      return person
    })
  }

  @Mutation
  public removePersonParticipation(participationId: string) {
    this.persons = this.persons.map((person) => {
      if (person.id === this.activePersonId) {
        person.participations = person.participations.filter((x) => x.id !== participationId)
      }
      return person
    })
  }

  @Mutation
  public setActivePersonId(personId: string | null) {
    setCookie('activePersonId', personId || '')

    this.activePersonId = personId
  }

  @Mutation
  public setPersons(persons: Person[]) {
    this.persons = persons
  }

  @Mutation
  public removePerson(personId: string) {
    const person = this.persons.find((x: Person) => x.id === personId)
    if (person) {
      this.persons.splice(this.persons.indexOf(person), 1)
    }
  }

}
