import axios from '@/tools/axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import { ProjectEntity } from '@/models/definitions'

@Module({ name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: ProjectEntity[] = []

  public get myProjects(): ProjectEntity[] {
    return this.projects
  }

  @MutationAction({ mutate: ['projects'] })
  public async fetchProjects() {
    const response = await axios.get('/projects')
    return { projects: response.data }
  }

  @Action({ commit: 'setNameAndEmail' })
  public async updateProjectGeneral(project: ProjectEntity) {
    await axios.put(`/projects/${project.id}`, {
      name: project.name,
      email: project.email
    })
    return project
  }

  @Action({ commit: 'insertProject' })
  public async createProject(project: ProjectEntity): Promise<ProjectEntity> {
    const response = await axios.post(`/projects`, {
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
  public setNameAndEmail(project: ProjectEntity) {
    this.projects = this.projects.map((x: ProjectEntity) => {
      if (x.id === project.id) {
        x.name = project.name
        x.email = project.email
      }
      return x
    })
  }

  @Mutation
  public insertProject(project: ProjectEntity) {
    this.projects.push(project)
  }

  @Mutation
  public removeProject(projectId: string) {
    this.projects = this.projects.filter((x: ProjectEntity) => x.id !== projectId)
  }

}
