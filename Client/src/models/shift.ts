import { Project, Role } from '.'

class Shift {
  public static create(data: any) {
    const shift = new Shift(
      data.id,
      data.categoryId,
      data.start,
      data.end,
      data.createdTime,
      data.lastUpdatedTime)
    shift.category = data.category ? Category.create(data.category) : undefined
    return shift
  }

  public id: string
  public categoryId: string
  public category?: Category
  public start: string
  public end: string
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    categoryId: string,
    start: string,
    end: string,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.categoryId = categoryId
    this.start = start
    this.end = end
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(shift: Shift) {
    this.id = shift.id
    this.categoryId = shift.categoryId
    this.category = shift.category
    this.start = shift.start
    this.end = shift.end
    this.createdTime = shift.createdTime
    this.lastUpdatedTime = shift.lastUpdatedTime
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
    return category
  }

  public id: string
  public projectId: string
  public project?: Project
  public name: string
  public createdTime: string
  public lastUpdatedTime: string
  public shifts: Shift[] = []

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
      if (a.start < b.start) {
        return -1
      } else if (a.start > b.start) {
        return 1
      } else {
        if (a.end < b.end) {
          return -1
        } else if (a.end > b.end) {
          return 1
        } else {
          return 0
        }
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
      data.isSubstituteTeamCaptain)
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

}

export {
  Category,
  Eligibility,
  Shift,
}