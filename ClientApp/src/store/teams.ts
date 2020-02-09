import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Team } from '../models'

@Module({ dynamic: true, store, name: 'TeamModule' })
export default class TeamModule extends VuexModule {

  @Action
  public async loadTeams() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/teams`)
      const teams = response.data.map((x: any) => Team.create(x))

      this.context.commit('upsertProjectTeams', {
        projectId,
        teams,
        clearFirst: true,
      })
    }
  }

  @Action
  public async createTeam(team: Team) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/teams`, team)
    const createdTeam = Team.create(response.data)

    this.context.commit('upsertProjectTeams', {
      projectId,
      teams: [createdTeam],
    })
  }

  @Action
  public async updateTeam(team: Team) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/teams/${team.id}`, team)
    const updatedTeam = Team.create(response.data)

    this.context.commit('upsertProjectTeams', {
      projectId,
      teams: [updatedTeam],
    })
  }

  @Action
  public async deleteTeam(teamId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/teams/${teamId}`)

    this.context.commit('removeProjectTeam', {
      projectId,
      teamId,
    })
  }

}
