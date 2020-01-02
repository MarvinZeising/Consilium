import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import { Project, ProjectParticipation, ProjectParticipationStatus, Person } from '../models/definitions'
import store from '../plugins/vuex'
import router from '../router'

@Module({ dynamic: true, store, name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []

  // * for the active project
  public participations: ProjectParticipation[] = []
  public participants: Person[] = []

  public get getActiveProject() {
    const projectId = router.currentRoute.params.projectId
    return this.projects.find((x: Project) => x.id === projectId)
  }

  public get getProjects() {
    return this.projects.sort((a, b) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  public get getParticipations() {
    return this.participations
  }

  public get getParticipants() {
    return this.participants
  }
  }

  @Action
  public getProject(projectId: string) {
    return this.projects.find((x: Project) => x.id === projectId)
  }

  @Action
  public getPerson(personId: string) {
    return this.participants.find((x: Person) => x.id === personId)
  }

  @Action
  public async loadProjects() {
    const personId = this.context.getters.getActivePersonId

    const response = await axios.get(`/persons/${personId}/participations`)
    const projectParticipations: ProjectParticipation[] = response.data

    const projects: Project[] = []
    for (const projectParticipation of projectParticipations) {
      if (projectParticipation.project) {
        const project: Project = new Project(
          projectParticipation.project.id,
          projectParticipation.project.name,
          projectParticipation.project.email)
        projects.push(project)
      }

      // const personResponse = await axios.get(`/persons/${projectParticipation.personId}`)
      // const person = new Person(
      //   personResponse.data.id,
      //   personResponse.data.firstname,
      //   personResponse.data.lastname,
      //   personResponse.data.gender,
      // )
      // persons.push(person)
    }

    this.context.commit('setParticipations', projectParticipations)
    this.context.commit('setProjects', projects)

    // TODO: load participants on persons (participants) route
    // this.context.commit('setParticipants', persons)
  }

  @Action
  public async joinProject(projectId: string) {
    const personId = this.context.getters.getActivePersonId

    await axios.post('/project-participations/request', { personId, projectId })

    await this.context.dispatch('loadProjects')
  }

  @Action
  public async cancelJoinRequest(projectParticipationId: string) {
    await axios.delete(`/project-participations/${projectParticipationId}`)
    await this.context.dispatch('loadProjects')
  }

  @Action
  public async declineInvitation(projectParticipationId: string) {
    await axios.delete(`/project-participations/${projectParticipationId}`)
    await this.context.dispatch('loadProjects')
  }

  @Action
  public async acceptInvitation(projectParticipationId: string) {
    await axios.put('/project-participations', {
      id: projectParticipationId,
      status: ProjectParticipationStatus.Active,
    })
    await this.context.dispatch('loadProjects')
  }

  @MutationAction({ mutate: ['projects'] })
  public async clearProjects() {
    return { projects: [] }
  }

  @Action({ commit: 'setGeneral' })
  public async updateProjectGeneral(project: Project) {
    await axios.put(`/projects/${project.id}`, {
      name: project.name,
      email: project.email
    })
    return project
  }

  @Action({ commit: 'insertProject' })
  public async createProject(project: {
    name: string,
    email: string,
    personId: string
  }): Promise<Project> {
    const response = await axios.post('/projects', {
      name: project.name,
      email: project.email,
      personId: this.context.getters.getActivePersonId,
    })

    const newProject = new Project(
      response.data.id,
      response.data.name,
      response.data.email)

    return newProject
  }

  @Action({ commit: 'removeProject' })
  public async deleteProject(projectId: string) {
    await axios.delete(`/projects/${projectId}`)
    return projectId
  }

  @Mutation
  public async setProjects(projects: Project[]) {
    this.projects = projects
  }

  @Mutation
  public async setParticipants(persons: Person[]) {
    this.participants = persons
  }

  @Mutation
  public async setParticipations(participations: ProjectParticipation[]) {
    this.participations = participations
  }

  @Mutation
  protected setGeneral(project: Project) {
    this.projects = this.projects.map((x: Project) => {
      if (x.id === project.id) {
        x.name = project.name
        x.email = project.email
      }
      return x
    })
  }

  @Mutation
  protected insertProject(project: Project) {
    this.projects.push(project)
  }

  @Mutation
  protected removeProject(projectId: string) {
    this.projects = this.projects.filter((x: Project) => x.id !== projectId)
  }

}
