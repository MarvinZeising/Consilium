import { Topic, Participation, Role, Category, Task } from '.'

class Project {
  public static create(data: any) {
    const project = new Project(
      data.id,
      data.name,
      data.email,
      data.allowRequests,
      data.createdTime,
      data.lastUpdatedTime)
    if (data.topics) {
      project.topics = data.topics.map((x: any) => Topic.create(x))
    }
    return project
  }

  public id: string
  public name: string
  public email: string
  public allowRequests: boolean
  public participations: Participation[] = []
  public invitations: Participation[] = []
  public requests: Participation[] = []
  public categories: Category[] = []
  public roles: Role[] = []
  public tasks: Task[] = []
  public topics: Topic[] = []
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    name: string,
    email: string,
    allowRequests: boolean,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.name = name
    this.email = email
    this.allowRequests = allowRequests
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public get getRoles() {
    return [...this.roles].sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  public get getInvitations() {
    return [...this.invitations].sort((a, b) => {
      if (a.project && b.project) {
        if (a.project.name < b.project.name) {
          return 1
        } else if (a.project.name > b.project.name) {
          return -1
        } else {
          return 0
        }
      } else {
        return 0
      }
    })
  }

  public get getRequests() {
    return [...this.requests].sort((a, b) => {
      if (a.project && b.project) {
        if (a.project.name < b.project.name) {
          return 1
        } else if (a.project.name > b.project.name) {
          return -1
        } else {
          return 0
        }
      } else {
        return 0
      }
    })
  }

  public get getParticipations() {
    return [...this.participations].sort((a, b) => {
      if (a.project && b.project) {
        if (a.project.name < b.project.name) {
          return 1
        } else if (a.project.name > b.project.name) {
          return -1
        } else {
          return 0
        }
      } else {
        return 0
      }
    })
  }

  public get getCategories() {
    return [...this.categories].sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  public get getTasks() {
    return [...this.tasks].sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  public get getTopics() {
    return [...this.topics].sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  public copyFrom(project: Project) {
    this.name = project.name
    this.email = project.email
    this.allowRequests = project.allowRequests
    this.createdTime = project.createdTime
    this.lastUpdatedTime = project.lastUpdatedTime
  }

}

export {
  Project,
}
