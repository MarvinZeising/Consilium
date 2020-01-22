import { Project, Role } from '.'

class Shift {
  public static create(data: any) {
    const shift = new Shift(
      data.id,
      data.categoryId,
      data.date,
      data.time,
      data.duration,
      data.createdTime,
      data.lastUpdatedTime)
    shift.category = data.category ? Category.create(data.category) : undefined
    return shift
  }

  public id: string
  public categoryId: string
  public category?: Category
  public date: number
  public time: number
  public duration: number
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    categoryId: string,
    date: number,
    time: number,
    duration: number,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.categoryId = categoryId
    this.date = date
    this.time = time
    this.duration = duration
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(shift: Shift) {
    this.id = shift.id
    this.categoryId = shift.categoryId
    this.category = shift.category
    this.date = shift.date
    this.time = shift.time
    this.duration = shift.duration
    this.createdTime = shift.createdTime
    this.lastUpdatedTime = shift.lastUpdatedTime
  }

}

class Task {
  public static create(data: any) {
    const task = new Task(
      data.id,
      data.projectId,
      data.name,
      data.description,
      data.helpLink,
      data.createdTime,
      data.lastUpdatedTime)

    task.project = data.project ? Project.create(data.project) : undefined
    return task
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

  public copyFrom(task: Task) {
    this.id = task.id
    this.projectId = task.projectId
    this.project = task.project
    this.name = task.name
    this.description = task.description
    this.helpLink = task.helpLink
    this.createdTime = task.createdTime
    this.lastUpdatedTime = task.lastUpdatedTime
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
  Task,
  Category,
  Eligibility,
  Shift,
}
