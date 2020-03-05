import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Application, Shift, Person, Attendee } from '../models'

@Module({ dynamic: true, store, name: 'ApplicationModule' })
export default class ApplicationModule extends VuexModule {
  @Action
  public async loadMyApplications() {
    const personId = this.context.getters.getActivePersonId
    if (personId) {
      const response = await axios.get(`/persons/${personId}/applications`)
      const applications = response.data.map((x: Application) => Application.create(x))

      const person: Person = this.context.getters.getActivePerson
      person.applications = applications
    }
  }

  @Action
  public async loadMyAttendances() {
    const personId = this.context.getters.getActivePersonId
    if (personId) {
      const response = await axios.get(`/persons/${personId}/attendances`)
      const attendances = response.data.map((x: Attendee) => Attendee.create(x))

      const person: Person = this.context.getters.getActivePerson
      person.attendances = attendances
    }
  }

  @Action
  public async createApplication(application: Application) {
    const personId = this.context.getters.getActivePersonId

    const response = await axios.post(`/persons/${personId}/applications`, application)
    const createdApplication = Application.create(response.data)

    const shift: Shift = await this.context.dispatch('getShift', application.shiftId)
    if (shift) {
      shift.isApplicant = true
      shift.applications.push(createdApplication)
    }
  }

  @Action
  public async cancelAttendance(shiftId: string) {
    const personId = this.context.getters.getActivePersonId

    await axios.put(`/persons/${personId}/shifts/${shiftId}/cancelAttendance`)

    // TODO: also delete application once applicationId exists (also do it serverside, then)
    const shift: Shift = await this.context.dispatch('getShift', shiftId)
    if (shift) {
      shift.isAttendee = false
      shift.attendees = shift.attendees.filter((x) => x.personId !== personId)
    }
  }

  @Action
  public async cancelApplication(shiftId: string) {
    const personId = this.context.getters.getActivePersonId

    await axios.put(`/persons/${personId}/shifts/${shiftId}/cancelApplication`)

    const shift: Shift = await this.context.dispatch('getShift', shiftId)
    if (shift) {
      shift.isApplicant = false
      shift.applications = shift.applications.filter((x) => x.personId !== personId)
    }
  }
}
