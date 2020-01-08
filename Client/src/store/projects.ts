import axios from 'axios'
import { Module, VuexModule, Mutation, Action, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import { Project, Participation, ParticipationStatus, Role } from '../models/definitions'

@Module({ dynamic: true, store, name: 'ProjectModule' })
export default class ProjectModule extends VuexModule {
  public projects: Project[] = []

  private get resolvePersonAndProject() {
    const personId = this.context.getters.getActivePersonId
    const person = this.context.getters.getActivePerson
    const projectId = router.currentRoute.params.projectId
    const project = this.getActiveProject
    return { personId, person, projectId, project }
  }

  public get getActiveProject() {
    return this.projects.find((x: Project) => x.id === router.currentRoute.params.projectId)
  }

  public get getProjects() {
    return [...this.projects].sort((a, b) => {
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

  @Action({ commit: 'upsertProject' })
  public async loadProject(data: {
    projectId: string,
    personId: string
  }) {
    const response = await axios.get(`/persons/${data.personId}/projects/${data.projectId}`)
    return Project.create(response.data)
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
    const { personId } = this.resolvePersonAndProject

    await axios.post('/project-participations/request', { personId, projectId })

    await this.context.dispatch('loadProjects') // TODO: do manually
  }

  @Action
  public async cancelJoinRequest(participationId: string) {
    await axios.delete(`/project-participations/${participationId}`)
    await this.context.dispatch('loadProjects') // TODO: do manually
  }



  @Action
  public async updateProjectGeneral(project: {
    name: string,
    email: string
  }) {
    const { personId, projectId } = this.resolvePersonAndProject
    const response = await axios.put(`/persons/${personId}/projects/${projectId}`, {
      name: project.name,
      email: project.email
    })
    const updatedProject = Project.create(response.data)

    // ? make the navbar show the updated project name
    await this.context.dispatch('initStore')

    this.context.commit('upsertProject', updatedProject)
  }

  @Action({ commit: 'upsertProject' })
  public async createProject(project: {
    name: string,
    email: string,
  }): Promise<Project> {
    const { personId } = this.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects`, {
      name: project.name,
      email: project.email,
    })

    await this.context.dispatch('initStore')

    return Project.create(response.data)
  }

  @Action({ commit: 'removeProject' })
  public async deleteProject(projectId: string) {
    const { personId } = this.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}`)

    await this.context.dispatch('initStore')

    return projectId
  }

  @Mutation
  public setProjects(projects: Project[]) {
    this.projects = projects
  }

  @Mutation
  public upsertProject(project: Project) {
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
  public upsertProjectParticipations({ projectId, participations, clearPermanentsFirst, clearFirst }: {
    projectId: string,
    participations: Participation[],
    clearPermanentsFirst: boolean,
    clearFirst: boolean,
  }) {
    this.projects = this.projects.map((project) => {
      if (project.id === projectId) {
        if (clearFirst) {
          project.participations = participations
          return project
        } else if (clearPermanentsFirst) {
          project.participations = participations.filter((x) =>
            x.status === ParticipationStatus.Active ||
            x.status === ParticipationStatus.Inactive)
        }
        project.participations = project.participations.map((x) => {
          const updatedParticipation = participations.find((y) => y.id === x.id)
          if (updatedParticipation) {
            x.personId = updatedParticipation.personId
            x.person = updatedParticipation.person
            x.projectId = updatedParticipation.projectId
            x.project = updatedParticipation.project
            x.roleId = updatedParticipation.roleId
            x.role = updatedParticipation.role
            x.status = updatedParticipation.status
            x.createdTime = updatedParticipation.createdTime
            x.lastUpdatedTime = updatedParticipation.lastUpdatedTime
          }
          return x
        })
        for (const updatedParticipation of participations) {
          const participationAlreadyExists = project.participations.find((x) => x.id === updatedParticipation.id)
          if (!participationAlreadyExists) {
            project.participations.push(updatedParticipation)
          }
        }
      }
      return project
    })
  }

  @Mutation
  public removeProjectParticipation({ projectId, participationId }: {
    projectId: string,
    participationId: string,
  }) {
    this.projects = this.projects.map((p) => {
      if (p.id === projectId) {
        p.participations = p.participations.filter((x) => x.id !== participationId)
      }
      return p
    })
  }

  @Mutation
  public upsertProjectRoles({ projectId, roles, clearFirst }: {
    projectId: string,
    roles: Role[],
    clearFirst: boolean,
  }) {
    this.projects = this.projects.map((project) => {
      if (project.id === projectId) {
        if (clearFirst) {
          project.roles = roles
          return project
        }
        project.roles = project.roles.map((role) => {
          const updatedRole = roles.find((x) => x.id === role.id)
          if (updatedRole) {
            role.name = updatedRole.name
            role.knowledgeBaseRead = updatedRole.knowledgeBaseRead
            role.knowledgeBaseWrite = updatedRole.knowledgeBaseWrite
            role.participantsRead = updatedRole.participantsRead
            role.participantsWrite = updatedRole.participantsWrite
            role.rolesRead = updatedRole.rolesRead
            role.rolesWrite = updatedRole.rolesWrite
            role.settingsRead = updatedRole.settingsRead
            role.settingsWrite = updatedRole.settingsWrite
          }
          return role
        })
        for (const updatedRole of roles) {
          const roleAlreadyExists = project.roles.find((x) => x.id === updatedRole.id)
          if (!roleAlreadyExists) {
            project.roles.push(updatedRole)
          }
        }
      }
      return project
    })
  }

  @Mutation
  public removeProjectRole({ projectId, roleId }: {
    projectId: string,
    roleId: string,
  }) {
    this.projects = this.projects.map((p) => {
      if (p.id === projectId) {
        p.roles = p.roles.filter((x) => x.id !== roleId)
      }
      return p
    })
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
  protected removeProject(projectId: string) {
    this.projects = this.projects.filter((x) => x.id !== projectId)
  }

}
