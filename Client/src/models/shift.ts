import { Project, Role, Person, User } from '.'
import moment from 'moment'

enum ShiftStatus {
  draft = 'draft',
  planned = 'planned',
  scheduled = 'scheduled',
  suspended = 'suspended',
  calledOff = 'calledOff',
}

enum ShiftMode {
  open = 'open',
  captainsOnly = 'captainsOnly',
  closed = 'closed',
}

class Shift {
  public static create(data: any) {
    const shift = new Shift(
      data.id,
      data.categoryId,
      data.date,
      data.time,
      data.duration,
      data.status,
      data.mode,
      data.calledOffReason,
      data.isApplicant,
      data.isAttendee,
      data.createdTime,
      data.lastUpdatedTime)

    shift.category = data.category ? Category.create(data.category) : undefined
    if (data.applications) {
      shift.applications = data.applications.map((x: Application) => Application.create(x))
    }

    if (data.attendees) {
      shift.attendees = data.attendees.map((x: Attendee) => Attendee.create(x))
    }

    return shift
  }

  public id: string
  public categoryId: string
  public category?: Category
  public date: number
  public time: number
  public duration: number
  public status: ShiftStatus
  public mode: ShiftMode
  public calledOffReason: string
  public isApplicant: boolean
  public isAttendee: boolean
  public createdTime: string
  public lastUpdatedTime: string
  public applications: Application[] = []
  public attendees: Attendee[] = []

  constructor(
    id: string,
    categoryId: string,
    date: number,
    time: number,
    duration: number,
    status: ShiftStatus,
    mode: ShiftMode,
    calledOffReason: string,
    isApplicant: boolean,
    isAttendee: boolean,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.categoryId = categoryId
    this.date = date
    this.time = time
    this.duration = duration
    this.status = status
    this.mode = mode
    this.calledOffReason = calledOffReason
    this.isApplicant = isApplicant
    this.isAttendee = isAttendee
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public get getApplications() {
    return [...this.applications].sort((a, b) => {
      if (a.person && b.person) {
        if (a.person.lastname < b.person.lastname) {
          return -1
        } else if (a.person.lastname > b.person.lastname) {
          return 1
        } else if (a.person.firstname < b.person.firstname) {
          return -1
        } else if (a.person.firstname > b.person.firstname) {
          return 1
        } else {
          return 0
        }
      }
      return 0
    })
  }

  public get getAttendees() {
    return [...this.attendees].sort((a, b) => {
      if (a.isCaptain && !b.isCaptain) {
        return -1
      } else if (!a.isCaptain && b.isCaptain) {
        return 1
      } else if (a.person && b.person) {
        if (a.person.lastname < b.person.lastname) {
          return -1
        } else if (a.person.lastname > b.person.lastname) {
          return 1
        } else if (a.person.firstname < b.person.firstname) {
          return -1
        } else if (a.person.firstname > b.person.firstname) {
          return 1
        } else {
          return 0
        }
      }
      return 0
    })
  }

  public getTimespan(user: User) {
    let startFormat = 'H:mm - '
    let endFormat = user.timeFormat

    if (moment(this.time, 'Hmm').format('mm') === '00') {
      startFormat = 'H - '
    }

    const startTime = moment(this.time, 'Hmm')
    const endTime = moment(this.time, 'Hmm')
    const durationHours = moment(this.duration, 'Hmm').format('H')
    const durationMinutes = moment(this.duration, 'Hmm').format('mm')
    endTime.add(durationHours, 'h')
    endTime.add(durationMinutes, 'm')

    if (endTime.format('mm') === '00') {
      endFormat = endFormat.replace(/:mm/gi, '')
    }

    return startTime.format(startFormat) + endTime.format(endFormat)
  }

  public get isDraft() {
    return this.status === ShiftStatus.draft
  }

  public get isPlanned() {
    return this.status === ShiftStatus.planned
  }

  public get isScheduled() {
    return this.status === ShiftStatus.scheduled
  }

  public get isSuspended() {
    return this.status === ShiftStatus.suspended
  }

  public get isCalledOff() {
    return this.status === ShiftStatus.calledOff
  }

  public copyFrom(shift: Shift) {
    this.id = shift.id
    this.categoryId = shift.categoryId
    this.category = shift.category
    this.date = shift.date
    this.time = shift.time
    this.duration = shift.duration
    this.status = shift.status
    this.mode = shift.mode
    this.calledOffReason = shift.calledOffReason
    this.isApplicant = shift.isApplicant
    this.isAttendee = shift.isAttendee
    this.createdTime = shift.createdTime
    this.lastUpdatedTime = shift.lastUpdatedTime
    this.applications = shift.applications
    this.attendees = shift.attendees
  }

}

class Application {
  public static create(data: any) {
    const application = new Application(
      data.id,
      data.shiftId,
      data.personId,
      data.availableAfter,
      data.notes,
      data.createdTime,
      data.lastUpdatedTime)
    application.shift = data.shift ? Shift.create(data.shift) : undefined
    application.person = data.person ? Person.create(data.person) : undefined
    return application
  }

  public id: string
  public shiftId: string
  public shift?: Shift
  public personId: string
  public person?: Person
  public availableAfter: boolean
  public notes: string
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    shiftId: string,
    personId: string,
    availableAfter: boolean,
    notes: string,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.shiftId = shiftId
    this.personId = personId
    this.availableAfter = availableAfter
    this.notes = notes
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(application: Application) {
    this.id = application.id
    this.shiftId = application.shiftId
    this.personId = application.personId
    this.availableAfter = application.availableAfter
    this.notes = application.notes
    this.createdTime = application.createdTime
    this.lastUpdatedTime = application.lastUpdatedTime
  }

}

class Attendee {
  public static create(data: any) {
    const attendee = new Attendee(
      data.id,
      data.personId,
      data.shiftId,
      data.teamId,
      data.isCaptain,
      data.createdTime,
      data.lastUpdatedTime)
    attendee.shift = data.shift ? Shift.create(data.shift) : undefined
    attendee.team = data.team ? Team.create(data.team) : undefined
    attendee.person = data.person ? Person.create(data.person) : undefined
    return attendee
  }

  public id: string
  public personId: string
  public person?: Person
  public shiftId: string
  public shift?: Shift
  public teamId: string
  public team?: Team
  public isCaptain: boolean
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    personId: string,
    shiftId: string,
    teamId: string,
    isCaptain: boolean,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.personId = personId
    this.shiftId = shiftId
    this.teamId = teamId
    this.isCaptain = isCaptain
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(attendee: Attendee) {
    this.id = attendee.id
    this.personId = attendee.personId
    this.shiftId = attendee.shiftId
    this.teamId = attendee.teamId
    this.isCaptain = attendee.isCaptain
    this.createdTime = attendee.createdTime
    this.lastUpdatedTime = attendee.lastUpdatedTime
  }

}

class Team {
  public static create(data: any) {
    const team = new Team(
      data.id,
      data.projectId,
      data.name,
      data.description,
      data.helpLink,
      data.createdTime,
      data.lastUpdatedTime)

    team.project = data.project ? Project.create(data.project) : undefined
    return team
  }

  public id: string
  public projectId: string
  public project?: Project
  public name: string
  public description: string
  public helpLink: string
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    projectId: string,
    name: string,
    description: string,
    helpLink: string,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.projectId = projectId
    this.name = name
    this.description = description
    this.helpLink = helpLink
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(team: Team) {
    this.id = team.id
    this.projectId = team.projectId
    this.project = team.project
    this.name = team.name
    this.description = team.description
    this.helpLink = team.helpLink
    this.createdTime = team.createdTime
    this.lastUpdatedTime = team.lastUpdatedTime
  }
}

class Category {
  public static create(data: any) {
    const category = new Category(
      data.id,
      data.projectId,
      data.name,
      data.createdTime,
      data.lastUpdatedTime)

    category.project = data.project ? Project.create(data.project) : undefined

    if (data.shifts) {
      category.shifts = data.shifts.map((x: Shift) => Shift.create(x))
    }
    if (data.eligibilities) {
      category.eligibilities = data.eligibilities.map((x: Eligibility) => Eligibility.create(x))
    }
    return category
  }

  public id: string
  public projectId: string
  public project?: Project
  public name: string
  public createdTime: string
  public lastUpdatedTime: string
  public shifts: Shift[] = []
  public eligibilities: Eligibility[] = []

  constructor(
    id: string,
    projectId: string,
    name: string,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.projectId = projectId
    this.name = name
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public get getShifts() {
    return [...this.shifts].sort((a, b) => {
      if (a.date < b.date) {
        return -1
      } else if (a.date > b.date) {
        return 1
      } else if (a.time < b.time) {
        return -1
      } else if (a.time > b.time) {
        return 1
      } else if (a.duration < b.duration) {
        return -1
      } else if (a.duration > b.duration) {
        return 1
      } else {
        return 0
      }
    })
  }

  public copyFrom(category: Category) {
    this.id = category.id
    this.projectId = category.projectId
    this.project = category.project
    this.name = category.name
    this.createdTime = category.createdTime
    this.lastUpdatedTime = category.lastUpdatedTime
    this.shifts = category.shifts
    this.eligibilities = category.eligibilities
  }
}

class Eligibility {
  public static create(data: any) {
    const eligibility = new Eligibility(
      data.id,
      data.roleId,
      data.categoryId,
      data.shiftsRead,
      data.shiftsWrite,
      data.isTeamCaptain,
      data.isSubstituteCaptain)
    eligibility.role = data.role ? Role.create(data.role) : undefined
    eligibility.category = data.category ? Category.create(data.category) : undefined
    return eligibility
  }

  public id: string
  public roleId: string
  public role?: Role
  public categoryId: string
  public category?: Category
  public shiftsRead: boolean
  public shiftsWrite: boolean
  public isTeamCaptain: boolean
  public isSubstituteCaptain: boolean

  constructor(
    id: string,
    roleId: string,
    categoryId: string,
    shiftsRead: boolean,
    shiftsWrite: boolean,
    isTeamCaptain: boolean,
    isSubstituteCaptain: boolean,
  ) {
    this.id = id
    this.roleId = roleId
    this.categoryId = categoryId
    this.shiftsRead = shiftsRead
    this.shiftsWrite = shiftsWrite
    this.isTeamCaptain = isTeamCaptain
    this.isSubstituteCaptain = isSubstituteCaptain
  }

  public copyFrom(data: Eligibility) {
    this.id = data.id
    this.role = data.role
    this.roleId = data.roleId
    this.category = data.category
    this.categoryId = data.categoryId
    this.shiftsRead = data.shiftsRead
    this.shiftsWrite = data.shiftsWrite
    this.isTeamCaptain = data.isTeamCaptain
    this.isSubstituteCaptain = data.isSubstituteCaptain
  }

  public getPermissionModel(area: 'shifts') {
    const role: any = this
    const read = role[area + 'Read']
    const write = role[area + 'Write']
    return { value: write ? 'write' : read ? 'read' : 'none' }
  }

  public setPermissionModel(
    area: 'shifts',
    modelValue: 'none' | 'read' | 'write'
  ) {
    const role: any = this
    role[area + 'Read'] = modelValue !== 'none'
    role[area + 'Write'] = modelValue === 'write'
  }

}

export {
  Team,
  Category,
  Eligibility,
  Shift,
  ShiftStatus,
  ShiftMode,
  Application,
  Attendee,
}
