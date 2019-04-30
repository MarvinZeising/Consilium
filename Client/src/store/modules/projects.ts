import axios from '@/tools/axios'
import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators'
import { ProjectEntity } from '@/models/definitions'

@Module
export default class Projects extends VuexModule {
  projects: Array<ProjectEntity> = []

  get myProjects() {
    return this.projects
  }

  @Mutation
  setProjects(projects: Array<ProjectEntity>) {
    this.projects = projects
  }

  @Action({commit: 'setProjects'})
  async fetchProjects() {
    return await axios.get('/projects')
  }
}
