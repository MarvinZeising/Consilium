enum Theme {
  Light = 'light',
  Dark = 'dark',
}

class User {
  public id: string
  public email: string
  public language: string
  public theme: Theme

  constructor(id: string, email: string, language: string | null) {
    this.id = id
    this.email = email
    this.language = language || 'en-US'
    this.theme = Theme.Light
  }
}

enum Gender {
  Male = 'male',
  Female = 'female',
}

class Person {
  public id: string
  public firstname: string
  public lastname: string
  public gender: Gender
  public participations: ProjectParticipation[]

  constructor(
    id: string,
    firstname: string,
    lastname: string,
    gender: Gender,
    participations: ProjectParticipation[]
  ) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
    this.gender = gender
    this.participations = participations
  }

  public fullName() {
    return this.firstname + ' ' + this.lastname
  }

}

class Project {
  public id: string
  public name: string
  public email: string
  public participations?: ProjectParticipation[]
  public roles?: Role[]
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

enum ProjectParticipationStatus {
  Invited = 'Invited',
  Requested = 'Requested',
  Active = 'Active',
  Inactive = 'Inactive',
}

class ProjectParticipation {
  public id: string
  public personId: string
  public projectId: string
  public roleId: string
  public status: ProjectParticipationStatus
  public createdTime: string
  public lastUpdatedTime: string
  public project?: Project
  public role?: Role

  constructor(
    id: string,
    personId: string,
    projectId: string,
    roleId: string,
    status: ProjectParticipationStatus,
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
  ProjectParticipation,
  ProjectParticipationStatus,
  Topic,
  Article,
}
