import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Shift, Category, ShiftStatus } from '../models'

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
  public async getShift(shiftId: string) {
    const project = this.context.getters.getActiveProject
    if (project) {
      for (const category of project.categories) {
        const shift = category.shifts.find((x: Shift) => x.id === shiftId)
        if (shift) {
          return shift
        }
      }
    }
    return undefined
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
  public async updateShift(shift: any) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/shifts/${shift.id}`, shift)
    const updatedShift = Shift.create(response.data)

    await this.context.dispatch('upsertShift', {
      oldCategoryId: shift.oldCategoryId,
      newCategoryId: shift.categoryId,
      shift: updatedShift,
    })
  }

  @Action
  public async updateShiftStatus({
    shiftId,
    status
  }: {
    shiftId: string,
    status: ShiftStatus
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/shifts/${shiftId}/status/${status}`)
    const shift = Shift.create(response.data)

    await this.context.dispatch('upsertShift', { shift })
  }

  @Action
  public async makeAssignment({ shiftId, assignments}: {
    shiftId: string,
    assignments: Array<{
      personId: string,
      teamId: string,
      isCaptain: boolean
    }>,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(
      `/persons/${personId}/projects/${projectId}/shifts/${shiftId}/assignments`, {
        attendees: assignments
      })
    const shift = Shift.create(response.data)

    await this.context.dispatch('upsertShift', { shift })
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

  @Action
  protected async upsertShift(data: {
    shift: Shift,
    oldCategoryId: string,
    newCategoryId: string,
  }) {
    if (data.oldCategoryId && data.newCategoryId) {
      const oldCategory: Category = await this.context.dispatch('getCategory', data.oldCategoryId)
      if (oldCategory) {
        oldCategory.shifts = oldCategory.shifts.filter((x) => x.id !== data.shift.id)
      }
      const newCategory: Category = await this.context.dispatch('getCategory', data.newCategoryId)
      if (newCategory) {
        newCategory.shifts.push(data.shift)
      }
    } else {
      const category: Category = await this.context.dispatch('getCategory', data.shift.categoryId)
      if (category) {
        category.shifts = category.shifts.map((x) => {
          if (x.id === data.shift.id) {
            x.copyFrom(data.shift)
          }
          return x
        })
      }
    }
  }

}
