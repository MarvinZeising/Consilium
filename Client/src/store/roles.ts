import axios from 'axios'
import { Module, VuexModule, Action } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Role } from '../models/definitions'

@Module({ dynamic: true, store, name: 'RoleModule' })
export default class RoleModule extends VuexModule {

  public get getRoles() {
    return this.context.getters.getActiveProject?.roles?.sort((a: Role, b: Role) => {
      if (a.name < b.name) {
        return -1
      } else if (a.name > b.name) {
        return 1
      } else {
        return 0
      }
    })
  }

  @Action
  public async loadRoles() {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/roles`)
      const roles = response.data.map((x: any) => Role.create(x))

      if (project) {
        project.roles = roles

        this.context.commit('updateProject', project)
      }
    }
  }

  @Action
  public async createRole(data: {
    name: string,
    settings: string,
    roles: string,
    participants: string,
    knowledgeBase: string,
  }) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

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
    const role = Role.create(response.data)

    if (project) {
      project.roles.push(role)

      this.context.commit('updateProject', project)
    }
  }

  @Action
  public async updateRole(data: {
    roleId: string,
    name: string,
    settings: string,
    roles: string,
    participants: string,
    knowledgeBase: string,
  }) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

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
    const role = Role.create(response.data)

    if (project) {
      project.roles = project.roles?.map((x: Role) => {
        return x.id === role.id ? role : x
      })

      this.context.commit('updateProject', project)
    }
  }

  @Action
  public async deleteRole(roleId: string) {
    const { personId, projectId, project } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/roles/${roleId}`)

    if (project) {
      project.roles = project.roles?.filter((x: Role) => x.id !== roleId)

      this.context.commit('updateProject', project)
    }
  }


}
