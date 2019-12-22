import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import { Project } from '../../models/definitions'

@Module({ name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []

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

  @MutationAction({ mutate: ['projects'] })
  public async initProjectModule() {
    const response = await axios.get('/projects')
    return { projects: response.data }
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
