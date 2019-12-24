import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction, getModule } from 'vuex-module-decorators'
import { Project, ProjectParticipation } from '../../models/definitions'
import PersonModule from './persons'

@Module({ name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []
  public participations: ProjectParticipation[] = []

  public get getProjects(): Project[] {
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

  public get getParticipations(): ProjectParticipation[] {
    return this.participations
  }

  @Action
  public async loadProjects() {
    const personId = this.context.getters.getActivePerson.id

    const response = await axios.get(`/project-participations/${personId}`)
    const projectParticipations: ProjectParticipation[] = response.data

    const projects: Project[] = []
    for (const projectParticipation of projectParticipations) {
      const projectResponse = await axios.get(`/projects/${projectParticipation.projectId}`)
      const project: Project = projectResponse.data
      projects.push(project)
    }

    this.context.commit('setParticipations', projectParticipations)
    this.context.commit('setProjects', projects)
  }

  @Action
  public async joinProject(projectId: string) {
    const personId = this.context.getters.getActivePerson.id

    await axios.post('/project-participations/request', { personId, projectId })

    await this.context.dispatch('loadProjects')
  }

  @Action
  public async cancelJoinRequest(projectParticipationId: string) {
    await axios.delete(`/project-participations/${projectParticipationId}`)

    await this.context.dispatch('loadProjects')
  }

  @MutationAction({ mutate: ['projects'] })
  public async clearProjects() {
    return { projects: [] }
  }

  @Action({ commit: 'setNameAndEmail' })
  public async updateProjectGeneral(project: Project) {
    await axios.put(`/projects/${project.id}`, {
      name: project.name,
      email: project.email
    })
    return project
  }

  @Action({ commit: 'insertProject' })
  public async createProject(project: Project): Promise<Project> {
    const response = await axios.post('/projects', {
      name: project.name,
      email: project.email
    })
    return response.data
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
  public async setParticipations(participations: ProjectParticipation[]) {
    this.participations = participations
  }

  @Mutation
  protected setNameAndEmail(project: Project) {
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
