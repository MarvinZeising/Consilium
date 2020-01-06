import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Participation, ParticipationStatus, Person } from '../models/definitions'

@Module({ dynamic: true, store, name: 'ParticipantModule' })
export default class ParticipantModule extends VuexModule {

  public get getParticipations(): Participation[] {
    return this.context.getters.getActiveProject?.participations?.filter((x: Participation) =>
      x.status === ParticipationStatus.Active ||
      x.status === ParticipationStatus.Inactive)
  }

  @Action
  public async loadParticipants() {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/participations`)
      const participations = response.data.map((x: any) => Participation.create(x))

      if (project) {
        project.participations = project.participations?.filter((x: Participation) =>
          x.status !== ParticipationStatus.Active &&
          x.status !== ParticipationStatus.Inactive)
        project.participations?.push(...participations)

        this.context.commit('updateProject', project)
      }
    }
  }

  @Action
  public async deleteParticipant(participationId: string) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/participants/${participationId}`)

    if (project) {
      project.participations = project.participations?.filter((x: Participation) => x.id !== participationId)

      this.context.commit('updateProject', project)
    }
  }

}
