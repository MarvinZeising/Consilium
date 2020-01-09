import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation } from '../models/definitions'

@Module({ dynamic: true, store, name: 'RequestModule' })
export default class RequestModule extends VuexModule {

  @Action
  public async loadRequests() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/requests`)
      const requests = response.data.map((x: any) => Participation.create(x))

      this.context.commit('upsertProjectRequests', {
        projectId,
        requests,
      })
    }
  }

  @Action
  public async createRequest(projectId: string) {
    const { personId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/requests`, { personId })
    const participation = Participation.create(response.data)

    this.context.commit('upsertPersonParticipations', [participation])
  }

  @Action
  public async updateRequest(data: {
    requestId: string,
    roleId: string
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/requests/${data.requestId}`, {
      roleId: data.roleId,
    })
    const request = Participation.create(response.data)

    this.context.commit('upsertProjectRequests', {
      projectId,
      requests: [request],
    })
  }

  @Action
  public async acceptRequest(request: Participation) {
    const { personId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${request.projectId}/requests/${request.id}/accept`)
    const updatedRequest = Participation.create(response.data)

    this.context.commit('upsertProjectRequests', {
      projectId: request.projectId,
      requests: [updatedRequest],
    })
  }

  @Action
  public async declineRequest(request: Participation) {
    const { personId } = this.context.getters.resolvePersonAndProject

    await axios.put(`/persons/${personId}/projects/${request.projectId}/requests/${request.id}/decline`)

    this.context.commit('removeProjectRequest', {
      projectId: request.projectId,
      requestId: request.id,
    })
  }

  @Action
  public async cancelRequest(participation: Participation) {
    await axios.put(`/persons/${participation.personId}/projects/${participation.projectId}/requests/${participation.id}/cancel`)

    this.context.commit('removePersonParticipation', participation.id)
  }

}
