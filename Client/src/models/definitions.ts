class Project {
  public id?: string
  public name: string
  public email: string

  constructor(name: string, email: string) {
    this.name = name
    this.email = email
  }
}

class User {
  public id?: string
  public email: string

  constructor(email: string) {
    this.email = email
  }
}

export {
  Project,
  User,
}
