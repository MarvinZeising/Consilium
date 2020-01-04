import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import {
  Project,
  Participation,
  ParticipationStatus,
  Person,
  Role
} from '../models/definitions'

@Module({ dynamic: true, store, name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []

  // * for the active project
  public participations: Participation[] = []
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

  public get getRoles() {
    return this.getActiveProject?.roles
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
  public async loadProject(projectId: string) {
    const personId = this.context.getters.getActivePersonId
    const response = await axios.get(`/projects/${projectId}/${personId}`)
    const project: Project = response.data

    this.context.commit('addProject', project)
  }

  @Action({ commit: 'setRoles' })
  public async loadRoles(projectId: string) {
    const personId = this.context.getters.getActivePersonId
    const response = await axios.get(`/persons/${personId}/projects/${projectId}/roles`)
    return response.data
  }

  @Action({ commit: 'setParticipations' })
  public async loadParticipations(projectId: string) {
    const personId = this.context.getters.getActivePersonId
    const response = await axios.get(`/projects/${projectId}/${personId}/participations`)
    return response.data
  }

  @Action({ commit: 'addRole' })
  public async createRole(data: {
    name: string,
    settings: string,
    roles: string,
    participants: string,
    knowledgeBase: string,
  }) {
    const personId = this.context.getters.getActivePersonId
    const projectId = router.currentRoute.params.projectId

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/roles`, {
      name: data.name,
      settingsRead: data.settings !== 'none',
      settingsWrite: data.settings === 'write',
      rolesRead: data.roles !== 'none',
      rolesWrite: data.roles === 'write',
      participantsRead: data.participants !== 'none',
      participantsWrite: data.participants === 'write',
      knowledgeBaseRead: data.knowledgeBase !== 'none',
      knowledgeBaseWrite: data.knowledgeBase === 'write',
    })
    return response.data
  }

  @Action({ commit: 'changeRole' })
  public async updateRole(data: {
    roleId: string,
    name: string,
    settings: string,
    roles: string,
    participants: string,
    knowledgeBase: string,
  }) {
    const personId = this.context.getters.getActivePersonId
    const projectId = router.currentRoute.params.projectId

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/roles/${data.roleId}`, {
      name: data.name,
      settingsRead: data.settings !== 'none',
      settingsWrite: data.settings === 'write',
      rolesRead: data.roles !== 'none',
      rolesWrite: data.roles === 'write',
      participantsRead: data.participants !== 'none',
      participantsWrite: data.participants === 'write',
      knowledgeBaseRead: data.knowledgeBase !== 'none',
      knowledgeBaseWrite: data.knowledgeBase === 'write',
    })
    return response.data
  }

  @Action({ commit: 'removeRole' })
  public async deleteRole(roleId: string) {
    const personId = this.context.getters.getActivePersonId
    const projectId = router.currentRoute.params.projectId

    await axios.delete(`/persons/${personId}/projects/${projectId}/roles/${roleId}`)
    return roleId
  }

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
    id: string,
    name: string,
    email: string
  }) {
    await axios.put(`/projects/${project.id}`, {
      personId: this.context.getters.getActivePersonId,
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
      response.data.email,
      '',
      '')

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
  protected setParticipants(persons: Person[]) {
    this.participants = persons
  }

  @Mutation
  protected setParticipations(participations: Participation[]) {
    this.participations = participations
  }

  @Mutation
  protected setRoles(roles: Role[]) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.roles = roles
      }
      return project
    })
  }

  @Mutation
  protected addRole(role: Role) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.roles?.push(role)
      }
      return project
    })
  }

  @Mutation
  protected changeRole(role: Role) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.roles = project.roles?.map((x) => {
          if (x.id === role.id) {
            x.name = role.name
            x.settingsRead = role.settingsRead
            x.settingsWrite = role.settingsWrite
            x.rolesRead = role.rolesRead
            x.rolesWrite = role.rolesWrite
            x.participantsRead = role.participantsRead
            x.participantsWrite = role.participantsWrite
            x.knowledgeBaseRead = role.knowledgeBaseRead
            x.knowledgeBaseWrite = role.knowledgeBaseWrite
          }
          return x
        })
      }
      return project
    })
  }

  @Mutation
  protected removeRole(roleId: string) {
    this.projects = this.projects.map((project) => {
      if (project.id === router.currentRoute.params.projectId) {
        project.roles = project.roles?.filter((x) => x.id !== roleId)
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
  protected removeProject(projectId: string) {
    this.projects = this.projects.filter((x) => x.id !== projectId)
  }

}
