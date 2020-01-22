import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Shift } from '../models'

@Module({ dynamic: true, store, name: 'ShiftModule' })
export default class ShiftModule extends VuexModule {

  @Action
  public async loadShifts() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/shifts`)
      const shifts: Shift[] = response.data.map((x: any) => Shift.create(x))

      const categories = shifts.reduce((storage: { [categoryId: string]: Shift[] }, item: Shift) => {
        (storage[item.categoryId] = storage[item.categoryId] || []).push(item)
        return storage
      }, {})

      for (const categoryId of Object.keys(categories)) {
      const category = await this.context.dispatch('getCategory', categoryId)
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

    const category = await this.context.dispatch('getCategory', shift.categoryId)
    if (category) {
      category.shifts.push(createdShift)
    }
  }


}
