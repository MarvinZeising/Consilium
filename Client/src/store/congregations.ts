import axios from 'axios'
import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Congregation } from '../models'

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

  @Action
  public async createCongregation(data: {
    name: string,
    number: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    return await axios
      .post(`/persons/${personId}/projects/${projectId}/congregations`, {
        name: data.name,
        number: data.number,
      })
      .then(async (response) => {
        const congregation = Congregation.create(response.data)
        this.context.commit('upsertCongregation', congregation)
        return congregation
      })
      .catch((error: any) => error.response?.data)
  }

  @Action
  public async updateCongregation(data: {
    congregationId: string,
    name: string,
    number: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    return await axios
      .put(`/persons/${personId}/projects/${projectId}/congregations/${data.congregationId}`, {
        name: data.name,
        number: data.number,
      })
      .then(async (response) => {
        const congregation = Congregation.create(response.data)
        this.context.commit('upsertCongregation', congregation)

        const activePerson = this.context.getters.getActivePerson
        if (activePerson?.congregationId === congregation.id) {
          activePerson.congregation.name = congregation.name
        }
        return congregation
      })
      .catch((error: any) => error.response?.data)
  }

  @Action
  public async deleteCongregation(congregationId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/congregations/${congregationId}`)

    this.context.commit('removeCongregation', congregationId)
  }

  @Mutation
  protected upsertCongregation(congregation: Congregation) {
    if (this.congregations.find((x) => x.id === congregation.id)) {
      this.congregations = this.congregations.map((x) => {
        if (x.id === congregation.id) {
          x.name = congregation.name
          x.number = congregation.number
        }
        return x
      })
    } else {
      this.congregations.push(congregation)
    }
  }

  @Mutation
  protected removeCongregation(congregationId: string) {
    this.congregations = this.congregations.filter((x) => x.id !== congregationId)
  }

  @Mutation
  protected setCongregations(congregations: Congregation[]) {
    this.congregations = congregations
  }

}
