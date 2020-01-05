enum Theme {
  Light = 'light',
  Dark = 'dark',
}

class User {
  public static create(data: any) {
    return new User(
      data.id,
      data.email,
      data.language,
      data.theme)
  }

  public id: string
  public email: string
  public language: string
  public theme: Theme

  constructor(id: string, email: string, language?: string, theme?: string) {
    this.id = id
    this.email = email
    this.language = language || 'en-US'
    this.theme = theme === 'dark' ? Theme.Dark : Theme.Light
  }
}

enum Gender {
  Male = 'male',
  Female = 'female',
}

class Person {
  public static create(data: any) {
    const person = new Person(
      data.id,
      data.firstname,
      data.lastname)
    if (data.participations) {
      person.participations = data.participations.map((x: any) => Participation.create(x))
    }
    return person
  }

  public id: string
  public firstname: string
  public lastname: string
  public gender: Gender = Gender.Male
  public participations: Participation[] = []

  constructor(
    id: string,
    firstname: string,
    lastname: string,
  ) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
  }

  public fullName() {
    return this.firstname + ' ' + this.lastname
  }

}

class Project {
  public static create(data: any) {
    return new Project(
      data.id,
      data.name,
      data.email,
      data.createdTime,
      data.lastUpdatedTime)
  }

  public id: string
  public name: string
  public email: string
  public participations: Participation[] = []
  public roles: Role[] = []
  public createdTime: string
  public lastUpdatedTime: string

  constructor(id: string, name: string, email: string, createdTime: string, lastUpdatedTime: string) {
    this.id = id
    this.name = name
    this.email = email
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }
}

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
      data.roleId,
      data.status.toLowerCase(),
      data.createdTime,
      data.lastUpdatedTime)
    participation.role = data.role ? Role.create(data.role) : undefined
    participation.person = data.person ? Person.create(data.person) : undefined
    participation.project = data.project ? Project.create(data.project) : undefined
    return participation
  }

  public id: string
  public personId: string
  public projectId: string
  public roleId: string
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
    roleId: string,
    status: ParticipationStatus,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.personId = personId
    this.projectId = projectId
    this.roleId = roleId
    this.status = status
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }
}

class Role {
  public static create(data: any) {
    return new Role(
      data.id,
      data.projectId,
      data.name,
      data.knowledgeBaseRead,
      data.knowledgeBaseWrite,
      data.participantsRead,
      data.participantsWrite,
      data.rolesRead,
      data.rolesWrite,
      data.settingsRead,
      data.settingsWrite,
      data.editable)
  }

  public id: string
  public projectId: string
  public name: string
  public knowledgeBaseRead: boolean
  public knowledgeBaseWrite: boolean
  public participantsRead: boolean
  public participantsWrite: boolean
  public rolesRead: boolean
  public rolesWrite: boolean
  public settingsRead: boolean
  public settingsWrite: boolean
  public editable: boolean

  constructor(
    id: string,
    projectId: string,
    name: string,
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
}

class Topic {
  public id: string
  public projectId: string
  public name: string
  public order: number

  constructor(projectId: string, name: string) {
    this.id = ''
    this.projectId = projectId
    this.name = name
    this.order = 0
  }
}

class Article {
  public id: string
  public topicId: string
  public title: string
  public content: string

  constructor(topicId: string, title: string, content: string) {
    this.id = ''
    this.topicId = topicId
    this.title = title
    this.content = content
  }
}

export {
  User,
  Theme,
  Person,
  Gender,
  Project,
  Participation,
  ParticipationStatus,
  Role,
  Topic,
  Article,
}
