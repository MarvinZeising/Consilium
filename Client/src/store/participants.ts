import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation, ParticipationStatus } from '../models/definitions'

@Module({ dynamic: true, store, name: 'ParticipantModule' })
export default class ParticipantModule extends VuexModule {

  public get getParticipations(): Participation[] {
    return this.context.getters.getActiveProject?.participations?.filter((x: Participation) =>
      x.status === ParticipationStatus.Active ||
      x.status === ParticipationStatus.Inactive)
  }

  @Action
  public async loadParticipants() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/participations`)
      const participations = response.data.map((x: any) => Participation.create(x))

      this.context.commit('upsertProjectParticipations', {
        projectId,
        participations,
        clearPermanentsFirst: true,
      })
    }
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

}
