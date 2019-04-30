import axios from '@/tools/axios'
import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators'
import { ProjectEntity } from '@/models/definitions'

@Module({ name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: ProjectEntity[] = []

  public get myProjects(): any[] {
    return this.projects
  }

  @Mutation
  public setProjects(projects: ProjectEntity[]) {
    this.projects = projects
  }

  @Action({commit: 'setProjects'})
  public async fetchProjects() {
    return (await axios.get('/projects')).data
  }
}
