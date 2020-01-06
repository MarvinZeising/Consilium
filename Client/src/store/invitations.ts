import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation, ParticipationStatus } from '../models/definitions'

@Module({ dynamic: true, store, name: 'InvitationModule' })
export default class InvitationModule extends VuexModule {

  public get getInvitations() {
    return this.context.getters.getAllParticipations
      ?.filter((x: Participation) => x.status === ParticipationStatus.Invited)
  }

  @Action
  public async loadInvitations() {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    const response = await axios.get(`/persons/${personId}/projects/${projectId}/invitations`)
    const participations = response.data.map((x: any) => Participation.create(x))

    if (project) {
      project.participations = project.participations
        ?.filter((x: Participation) => x.status !== ParticipationStatus.Invited)
      project.participations?.push(...participations)

      this.context.commit('updateProject', project)
    }
  }

  @Action
  public async createInvitation(data: {
    personId: string,
    roleId: string
  }) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/invitations`, {
      personId: data.personId,
      roleId: data.roleId,
    })
    const participation = Participation.create(response.data)

    if (project) {
      project.participations.push(participation)

      this.context.commit('updateProject', project)
    }
  }

  @Action
  public async updateInvitation(data: {
    participationId: string,
    roleId: string
  }) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/invitations/${data.participationId}`, {
      roleId: data.roleId,
    })
    const participation = Participation.create(response.data)

    if (project) {
      project.participations = project.participations?.map((x: Participation) => {
        return x.id === participation.id ? participation : x
      })

      this.context.commit('updateProject', project)
    }
  }

  @Action
  public async acceptInvitation(participation: Participation) {
    const { personId, person } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${participation.projectId}/invitations/${participation.id}/accept`)
    const updatedParticipation = Participation.create(response.data)

    if (person) {
      person.participations = person.participations?.map((x: Participation) =>
        x.id === participation.id ? updatedParticipation : x)

      this.context.commit('updatePerson', person)
    }
  }

  @Action
  public async declineInvitation(participation: Participation) {
    const { personId, person } = this.context.getters.resolvePersonAndProject

    await axios.put(`/persons/${personId}/projects/${participation.projectId}/invitations/${participation.id}/decline`)

    if (person) {
      person.participations = person.participations?.filter((x: Participation) => x.id !== participation.id)

      this.context.commit('updatePerson', person)
    }
  }

  @Action
  public async cancelInvitation(participationId: string) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/invitations/${participationId}`)

    if (project) {
      project.participations = project.participations
        ?.filter((x: Participation) => x.id !== participationId)

      this.context.commit('updateProject', project)
    }
  }

}
