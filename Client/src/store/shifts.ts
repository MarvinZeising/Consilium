import axios from 'axios'
import moment from 'moment'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Shift } from '../models'

@Module({ dynamic: true, store, name: 'ShiftModule' })
export default class ShiftModule extends VuexModule {

  @Action
  public async loadShifts(categoryId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/categories/${categoryId}/shifts`)
      const shifts = response.data.map((x: any) => Shift.create(x))

      shifts.push({
        name: 'Shift',
        start: moment().format('YYYY-MM-DD HH:mm'),
        end: moment().add(2, 'hours').format('YYYY-MM-DD HH:mm'),
      })

      const category = await this.context.dispatch('getCategory', categoryId)
      if (category) {
        category.shifts = shifts
      }
    }
  }

  @Action
  public async createShift(shift: Shift) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(
      `/persons/${personId}/projects/${projectId}/categories/${shift.categoryId}/shifts`,
      shift)
    const createdShift = Shift.create(response.data)

    const category = await this.context.dispatch('getCategory', shift.categoryId)
    if (category) {
      category.shifts.push(createdShift)
    }
  }


}
