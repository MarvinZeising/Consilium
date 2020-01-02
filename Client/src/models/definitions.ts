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

  constructor(id: string, firstname: string, lastname: string, gender: Gender) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
    this.gender = gender
  }

  public fullName() {
    return this.firstname + ' ' + this.lastname
  }

}

class Project {
  public id: string
  public name: string
  public email: string

  constructor(id: string, name: string, email: string) {
    this.id = id
    this.name = name
    this.email = email
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
  public createdDate: string
  public status: ProjectParticipationStatus

  constructor(
    id: string,
    personId: string,
    projectId: string,
    createdDate: string,
    status: ProjectParticipationStatus
  ) {
    this.id = id
    this.personId = personId
    this.projectId = projectId
    this.createdDate = createdDate
    this.status = status
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
