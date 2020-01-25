import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Application, Shift, Person } from '../models'

@Module({ dynamic: true, store, name: 'ApplicationModule' })
export default class ApplicationModule extends VuexModule {

  @Action
  public async loadMyApplications() {
    const personId = this.context.getters.getActivePersonId

    const response = await axios.get(`/persons/${personId}/applications`)
    const applications = response.data.map((x: Application) => Application.create(x))

    const person: Person = this.context.getters.getActivePerson
    person.applications = applications
  }

  @Action
  public async createApplication(application: Application) {
    const personId = this.context.getters.getActivePersonId

    const response = await axios.post(`/persons/${personId}/applications`, application)
    const createdApplication = Application.create(response.data)

    const shift: Shift = await this.context.dispatch('getShift', application.shiftId)
    if (shift) {
      shift.applications.push(createdApplication)
    }
  }

  @Action
  public async deleteApplication(application: Application) {
    const personId = this.context.getters.getActivePersonId

    await axios.delete(`/persons/${personId}/applications/${application.id}`)

    const shift: Shift = await this.context.dispatch('getShift', application.shiftId)
    if (shift) {
      shift.applications = shift.applications.filter((x) => x.id !== application.id)
    }
  }

}
