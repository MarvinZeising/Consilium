import NewTabItemDialog from '@/components/dialogs/NewTabItemDialog.vue'

class User {
  public id: string
  public email: string
  public language: string

  constructor(id: string, email: string, language: string | null) {
    this.id = id
    this.email = email
    this.language = language || 'en-US'
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
  public photoUrl: string
  public gender: Gender

  constructor(id: string, firstname: string, lastname: string, photoUrl: string) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
    this.gender = Gender.Male
    this.photoUrl = photoUrl
  }

  public fullName() {
    return this.firstname + ' ' + this.lastname
  }
}

class Project {
  public id?: string // TODO: remove optional
  public name: string
  public email: string

  constructor(name: string, email: string) {
    this.name = name
    this.email = email
  }
}

class Topic {
  public id: string
  public projectId: string
  public name: string

  constructor(projectId: string, name: string) {
    this.id = ''
    this.projectId = projectId
    this.name = name
  }
}

export {
  User,
  Person,
  Project,
  Topic,
}
