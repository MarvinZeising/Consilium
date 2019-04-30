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

  @Action
  public async updateProjectGeneral(project: ProjectEntity) {
    await axios.put(`/projects/${project.id}`, {
      name: project.name,
      email: project.email
    })

    this.context.commit('setName', {
      id: project.id,
      name: project.name
    })
  }

  @Mutation
  public setName(project: ProjectEntity) {
    this.projects = this.projects.map((x: ProjectEntity) => {
      if (x.id === project.id) {
        x.name = project.name
      }
      return x
    })
  }

  @Mutation
  public setEmail(project: ProjectEntity) {
    this.projects = this.projects.map((x: ProjectEntity) => {
      if (x.id === project.id) {
        x.email = project.email
      }
      return x
    })
  }

}
