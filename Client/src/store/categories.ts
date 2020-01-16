import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Category } from '../models'

@Module({ dynamic: true, store, name: 'CategoryModule' })
export default class CategoryModule extends VuexModule {

  @Action
  public async loadCategories() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/categories`)
      const categories = response.data.map((x: any) => Category.create(x))

      this.context.commit('upsertProjectCategories', {
        projectId,
        categories,
        clearFirst: true,
      })
    }
  }

  @Action
  public async createCategory(name: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/categories`, { name })
    const category = Category.create(response.data)

    this.context.commit('upsertProjectCategories', {
      projectId,
      categories: [category],
    })
  }

  @Action
  public async updateCategory(data: {
    categoryId: string,
    name: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/categories/${data.categoryId}`, {
      name: data.name,
    })
    const category = Category.create(response.data)

    this.context.commit('upsertProjectCategories', {
      projectId,
      categories: [category],
    })
  }

  @Action
  public async deleteCategory(categoryId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/categories/${categoryId}`)

    this.context.commit('removeProjectCategory', {
      projectId,
      categoryId,
    })
  }

}
