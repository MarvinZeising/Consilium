import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation } from '../models/definitions'

@Module({ dynamic: true, store, name: 'InvitationModule' })
export default class InvitationModule extends VuexModule {

  @Action
  public async loadInvitations() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/invitations`)
      const invitations = response.data.map((x: any) => Participation.create(x))

      this.context.commit('upsertProjectInvitations', {
        projectId,
        invitations,
      })
    }
  }

  @Action
  public async createInvitation(data: {
    personId: string,
    roleId: string
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/invitations`, {
      personId: data.personId,
      roleId: data.roleId,
    })
    const invitation = Participation.create(response.data)

    this.context.commit('upsertProjectInvitations', {
      projectId,
      invitations: [invitation],
    })
  }

  @Action
  public async updateInvitation(data: {
    invitationId: string,
    roleId: string
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/invitations/${data.invitationId}`, {
      roleId: data.roleId,
    })
    const invitation = Participation.create(response.data)

    this.context.commit('upsertProjectInvitations', {
      projectId,
      invitations: [invitation],
    })
  }

  @Action
  public async acceptInvitation(invitation: Participation) {
    const { personId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${invitation.projectId}/invitations/${invitation.id}/accept`)
    const updatedParticipation = Participation.create(response.data)

    this.context.commit('upsertProjectInvitations', {
      projectId: invitation.projectId,
      invitations: [updatedParticipation],
    })
  }

  @Action
  public async declineInvitation(invitation: Participation) {
    const { personId } = this.context.getters.resolvePersonAndProject

    await axios.put(`/persons/${personId}/projects/${invitation.projectId}/invitations/${invitation.id}/decline`)

    this.context.commit('removeProjectInvitation', {
      projectId: invitation.projectId,
      invitationId: invitation.id,
    })
  }

  @Action
  public async cancelInvitation(invitationId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/invitations/${invitationId}`)

    this.context.commit('removeProjectInvitation', {
      projectId,
      invitationId,
    })
  }

}
