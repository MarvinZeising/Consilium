class Project {
  public id?: string // TODO: remove optional
  public name: string
  public email: string

  constructor(name: string, email: string) {
    this.name = name
    this.email = email
  }
}

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

export {
  Project,
  User,
}
