import { Person, Project } from '.'
import { Eligibility } from './shift'

enum ParticipationStatus {
  Invited = 'invited',
  Requested = 'requested',
  Active = 'active',
  Inactive = 'inactive',
}

class Participation {
  public static create(data: any) {
    const participation = new Participation(
      data.id,
      data.personId,
      data.projectId,
      data.status.toLowerCase(),
      data.createdTime,
      data.lastUpdatedTime)
    participation.roleId = data.roleId ? data.roleId : undefined
    participation.role = data.role ? Role.create(data.role) : undefined
    participation.person = data.person ? Person.create(data.person) : undefined
    participation.project = data.project ? Project.create(data.project) : undefined
    return participation
  }

  public id: string
  public personId: string
  public projectId: string
  public roleId?: string
  public status: ParticipationStatus
  public createdTime: string
  public lastUpdatedTime: string
  public person?: Person
  public project?: Project
  public role?: Role

  constructor(
    id: string,
    personId: string,
    projectId: string,
    status: ParticipationStatus,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.personId = personId
    this.projectId = projectId
    this.status = status
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public copyFrom(participation: Participation) {
    this.personId = participation.personId
    this.person = participation.person
    this.projectId = participation.projectId
    this.project = participation.project
    this.roleId = participation.roleId
    this.role = participation.role
    this.status = participation.status
    this.createdTime = participation.createdTime
    this.lastUpdatedTime = participation.lastUpdatedTime
  }
}

class Role {
  public static create(data: any) {
    const role = new Role(
      data.id,
      data.projectId,
      data.name,
      data.calendarRead,
      data.calendarWrite,
      data.knowledgeBaseRead,
      data.knowledgeBaseWrite,
      data.participantsRead,
      data.participantsWrite,
      data.rolesRead,
      data.rolesWrite,
      data.settingsRead,
      data.settingsWrite,
      data.editable)
    if (data.eligibilities) {
      role.eligibilities = data.eligibilities.map((x: Eligibility) => Eligibility.create(x))
    }
    return role
  }

  public id: string
  public projectId: string
  public name: string
  public calendarRead: boolean
  public calendarWrite: boolean
  public knowledgeBaseRead: boolean
  public knowledgeBaseWrite: boolean
  public participantsRead: boolean
  public participantsWrite: boolean
  public rolesRead: boolean
  public rolesWrite: boolean
  public settingsRead: boolean
  public settingsWrite: boolean
  public editable: boolean
  public eligibilities: Eligibility[] = []

  constructor(
    id: string,
    projectId: string,
    name: string,
    calendarRead: boolean,
    calendarWrite: boolean,
    knowledgeBaseRead: boolean,
    knowledgeBaseWrite: boolean,
    participantsRead: boolean,
    participantsWrite: boolean,
    rolesRead: boolean,
    rolesWrite: boolean,
    settingsRead: boolean,
    settingsWrite: boolean,
    editable: boolean
  ) {
    this.id = id
    this.projectId = projectId
    this.name = name
    this.calendarRead = calendarRead
    this.calendarWrite = calendarWrite
    this.knowledgeBaseRead = knowledgeBaseRead
    this.knowledgeBaseWrite = knowledgeBaseWrite
    this.participantsRead = participantsRead
    this.participantsWrite = participantsWrite
    this.rolesRead = rolesRead
    this.rolesWrite = rolesWrite
    this.settingsRead = settingsRead
    this.settingsWrite = settingsWrite
    this.editable = editable
  }

  public copyFrom(role: Role) {
    this.name = role.name
    this.calendarRead = role.calendarRead
    this.calendarWrite = role.calendarWrite
    this.knowledgeBaseRead = role.knowledgeBaseRead
    this.knowledgeBaseWrite = role.knowledgeBaseWrite
    this.participantsRead = role.participantsRead
    this.participantsWrite = role.participantsWrite
    this.rolesRead = role.rolesRead
    this.rolesWrite = role.rolesWrite
    this.settingsRead = role.settingsRead
    this.settingsWrite = role.settingsWrite
    this.eligibilities = role.eligibilities
  }

  public getPermissionModel(
    area: 'calendar' | 'knowledgeBase' | 'participants' | 'roles' | 'settings'
  ) {
    const role: any = this
    const read = role[area + 'Read']
    const write = role[area + 'Write']
    return { value: write ? 'write' : read ? 'read' : 'none' }
  }

  public setPermissionModel(
    area: 'calendar' | 'knowledgeBase' | 'participants' | 'roles' | 'settings',
    modelValue: 'none' | 'read' | 'write'
  ) {
    const role: any = this
    role[area + 'Read'] = modelValue !== 'none'
    role[area + 'Write'] = modelValue === 'write'
  }

}

export {
  Participation,
  ParticipationStatus,
  Role,
}
