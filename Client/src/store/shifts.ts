import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Shift, Category } from '../models'

@Module({ dynamic: true, store, name: 'ShiftModule' })
export default class ShiftModule extends VuexModule {

  @Action
  public async loadShifts() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/shifts/20200101-20200131`)
      const shifts: Shift[] = response.data.map((x: any) => Shift.create(x))

      const categories = shifts.reduce((storage: { [categoryId: string]: Shift[] }, item: Shift) => {
        (storage[item.categoryId] = storage[item.categoryId] || []).push(item)
        return storage
      }, {})

      for (const categoryId of Object.keys(categories)) {
        const category: Category = await this.context.dispatch('getCategory', categoryId)
      if (category) {
          category.shifts = categories[categoryId]
        }
      }
    }
  }

  @Action
  public async createShift(shift: Shift) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/shifts`, shift)
    const createdShift = Shift.create(response.data)

    const category: Category = await this.context.dispatch('getCategory', shift.categoryId)
    if (category) {
      category.shifts.push(createdShift)
    }
  }


  @Action
  public async deleteShift(shift: Shift) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/shifts/${shift.id}`)

    const category: Category = await this.context.dispatch('getCategory', shift.categoryId)
    if (category) {
      category.shifts = category.shifts.filter((x) => x.id !== shift.id)
    }
  }

}
