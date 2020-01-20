import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Task } from '../models'

@Module({ dynamic: true, store, name: 'TaskModule' })
export default class TaskModule extends VuexModule {

  @Action
  public async loadTasks() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/tasks`)
      const tasks = response.data.map((x: any) => Task.create(x))

      this.context.commit('upsertProjectTasks', {
        projectId,
        tasks,
        clearFirst: true,
      })
    }
  }

  @Action
  public async createTask(task: Task) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/tasks`, task)
    const createdTask = Task.create(response.data)

    this.context.commit('upsertProjectTasks', {
      projectId,
      tasks: [createdTask],
    })
  }

  // @Action
  // public async updateCategory(category: Category) {
  //   const { personId, projectId } = this.context.getters.resolvePersonAndProject

  //   const response = await axios.put(`/persons/${personId}/projects/${projectId}/categories/${category.id}`, category)
  //   const updatedCategory = Category.create(response.data)

  //   this.context.commit('upsertProjectCategories', {
  //     projectId,
  //     categories: [updatedCategory],
  //   })
  // }

  // @Action
  // public async deleteCategory(categoryId: string) {
  //   const { personId, projectId } = this.context.getters.resolvePersonAndProject

  //   await axios.delete(`/persons/${personId}/projects/${projectId}/categories/${categoryId}`)

  //   this.context.commit('removeProjectCategory', {
  //     projectId,
  //     categoryId,
  //   })
  // }

}
