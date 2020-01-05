import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import { Project, Participation, ParticipationStatus } from '../models/definitions'

@Module({ dynamic: true, store, name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []

  private get resolvePersonAndProject() {
    const personId = this.getPersonId
    const projectId = this.getProjectId
    const project = this.getActiveProject
    return { personId, projectId, project }
  }

  public get getProjectId() {
    return router.currentRoute.params.projectId
  }

  public get getPersonId() {
    return this.context.getters.getActivePersonId
  }

  public get getActiveProject() {
    return this.projects.find((x: Project) => x.id === this.getProjectId)
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

  public get getAllParticipations() {
    return this.getActiveProject?.participations
  }

  public get getRequests() {
    return this.getAllParticipations?.filter((x) => x.status === ParticipationStatus.Requested)
  }

  public get getParticipants() {
    return this.getActiveProject?.participations?.filter((x) =>
      x.status === ParticipationStatus.Active || x.status === ParticipationStatus.Inactive)
  }

  @Action({ commit: 'addProject' })
  public async loadProject(projectId: string) {
    const response = await axios.get(`/persons/${this.getPersonId}/projects/${projectId}`)
    return Project.create(response.data)
  }

  @Action({ commit: 'setParticipants' })
  public async loadParticipants() {
    const { personId, projectId } = this.resolvePersonAndProject
    const response = await axios.get(`/persons/${personId}/projects/${projectId}/participations`)
    return response.data.map((x: any) => Participation.create(x))
  }

  @Action({ commit: 'setRequests' })
  public async loadRequests() {
    const { personId, projectId } = this.resolvePersonAndProject
    const response = await axios.get(`/persons/${personId}/projects/${projectId}/requests`)
    return response.data.map((x: any) => Participation.create(x))
  }

  // ? do we need all these???
  @Action
  public async joinProject(projectId: string) {
    const personId = this.context.getters.getActivePersonId

    await axios.post('/project-participations/request', { personId, projectId })

    await this.context.dispatch('loadProjects') // TODO: do manually
  }

  @Action
  public async cancelJoinRequest(participationId: string) {
    await axios.delete(`/project-participations/${participationId}`)
    await this.context.dispatch('loadProjects') // TODO: do manually
  }

  @Action
  public async declineInvitation(participationId: string) {
    await axios.delete(`/project-participations/${participationId}`)
    await this.context.dispatch('loadProjects') // TODO: do manually
  }

  @Action
  public async acceptInvitation(participationId: string) {
    await axios.put('/project-participations', {
      id: participationId,
      status: ParticipationStatus.Active,
    })
    await this.context.dispatch('loadProjects') // TODO: do manually
  }



  @MutationAction({ mutate: ['projects'] })
  public async clearProjects() {
    return { projects: [] }
  }

  @Action({ commit: 'setGeneral' })
  public async updateProjectGeneral(project: {
    name: string,
    email: string
  }) {
    const { personId, projectId } = this.resolvePersonAndProject
    await axios.put(`/persons/${personId}/projects/${projectId}`, {
      name: project.name,
      email: project.email
    })

    // ? make the navbar show the updated project name
    await this.context.dispatch('initStore')

    return project
  }

  @Action({ commit: 'insertProject' })
  public async createProject(project: {
    name: string,
    email: string,
  }): Promise<Project> {
    const response = await axios.post(`/persons/${this.getPersonId}/projects`, {
      name: project.name,
      email: project.email,
    })

    await this.context.dispatch('initStore')

    return Project.create(response.data)
  }

  @Action({ commit: 'removeProject' })
  public async deleteProject(projectId: string) {
    await axios.delete(`/persons/${this.getPersonId}/projects/${projectId}`)

    await this.context.dispatch('initStore')

    return projectId
  }

  @Mutation
  public async setProjects(projects: Project[]) {
    this.projects = projects
  }

  @Mutation
  public async addProject(project: Project) {
    if (this.projects.filter((x) => x.id === project.id).length > 0) {
      this.projects = this.projects.map((p) => {
        if (p.id === project.id) {
          p.name = project.name
          p.email = project.email
          p.createdTime = project.createdTime
          p.lastUpdatedTime = project.lastUpdatedTime
        }
        return p
      })
    } else {
      this.projects.push(project)
    }
  }

  @Mutation
  protected setRequests(participations: Participation[]) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.participations = project.participations?.filter((x) => x.status !== ParticipationStatus.Requested)
        project.participations?.push(...participations)
      }
      return project
    })
  }

  @Mutation
  protected setParticipants(participations: Participation[]) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.participations = project.participations?.filter((x) =>
          x.status !== ParticipationStatus.Active &&
          x.status !== ParticipationStatus.Inactive)

        project.participations?.push(...participations)
      }
      return project
    })
  }

  @Mutation
  protected setGeneral(project: Project) {
    this.projects = this.projects.map((x) => {
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
  protected updateProject(project: Project) {
    this.projects = this.projects.map((x) => {
      return x.id === project.id ? project : x
    })
  }

  @Mutation
  protected removeProject(projectId: string) {
    this.projects = this.projects.filter((x) => x.id !== projectId)
  }

}
