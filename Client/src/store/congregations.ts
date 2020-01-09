import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Congregation } from '../models/definitions'

@Module({ dynamic: true, store, name: 'CongregationModule' })
export default class CongregationModule extends VuexModule {
  public congregations: Congregation[] = []

  public get getCongregations() {
    return [...this.congregations].sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  @Action
  public async loadCongregations() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.get(`/persons/${personId}/projects/${projectId}/congregations`)
    const congregations = response.data.map((x: Congregation) => Congregation.create(x))

    this.context.commit('setCongregations', congregations)
  }

  @Mutation
  private setCongregations(congregations: Congregation[]) {
    this.congregations = congregations
  }

}
