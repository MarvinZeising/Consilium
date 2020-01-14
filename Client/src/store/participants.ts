import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation, ParticipationStatus, Gender, Language, Privilege, Assignment } from '../models'

@Module({ dynamic: true, store, name: 'ParticipantModule' })
export default class ParticipantModule extends VuexModule {

  @Action
  public async loadParticipants() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/participations`)
      const participations = response.data.map((x: any) => Participation.create(x))

      this.context.commit('upsertProjectParticipations', {
        projectId,
        participations,
        clearFirst: true,
      })
    }
  }

  @Action
  public async updateParticipant(data: {
    participationId: string,
    firstname: string,
    lastname: string,
    gender: Gender,
    email: string,
    language: Language,
    phone: string,
    privilege: Privilege,
    assignment: Assignment,
    languages: string,
    notes: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(
      `/persons/${personId}/projects/${projectId}/participants/${data.participationId}`, {
      firstname: data.firstname,
      lastname: data.lastname,
      gender: data.gender,
      email: data.email,
      language: data.language,
      phone: data.phone,
      privilege: data.privilege,
      assignment: data.assignment,
      languages: data.languages,
      notes: data.notes,
    })
    const participation = Participation.create(response.data)

    this.context.commit('upsertProjectParticipations', {
      projectId,
      participations: [participation],
    })
  }

  @Action
  public async deleteParticipant(participationId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/participants/${participationId}`)

    this.context.commit('removeProjectParticipation', {
      projectId,
      participationId,
    })
  }

  @Action
  public async deleteParticipation(participation: Participation) {
    await axios.delete(`/persons/${participation.personId}/projects/${participation.projectId}/participations/${participation.id}`)

    this.context.commit('removePersonParticipation', participation.id)
  }

}
