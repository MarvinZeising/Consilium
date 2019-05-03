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
  public username: string
  public password: string

  constructor(username: string, password: string) {
    this.username = username
    this.password = password
  }
}

export {
  Project,
  User,
}
