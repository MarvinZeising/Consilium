import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Application, Shift } from '../models'

@Module({ dynamic: true, store, name: 'ApplicationModule' })
export default class ApplicationModule extends VuexModule {

  @Action
  public async createApplication(application: Application) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(
      `/persons/${personId}/projects/${projectId}/shifts/${application.shiftId}/applications`,
      application)
    const createdApplication = Application.create(response.data)

    const shift: Shift = await this.context.dispatch('getShift', application.shiftId)
    if (shift) {
      shift.applications.push(createdApplication)
    }
  }

  @Action
  public async deleteApplication(application: Application) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/shifts/${application.shiftId}/applications/${application.id}`)

    const shift: Shift = await this.context.dispatch('getShift', application.shiftId)
    if (shift) {
      shift.applications = shift.applications.filter((x) => x.id !== application.id)
    }
  }

}
