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

  @MutationAction({ mutate: ['projects'] })
  public async clearProjects() {
    return { projects: [] }
  }

  @Action({ commit: 'upsertProject' })
  public async loadProject(data: {
    projectId: string,
    personId: string
  }) {
    const response = await axios.get(`/persons/${data.personId}/projects/${data.projectId}`)
    return Project.create(response.data)
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

    await this.context.dispatch('loadNavbar')

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

    await this.context.dispatch('loadNavbar')

    return Project.create(response.data)
  }

  @Action({ commit: 'removeProject' })
  public async deleteProject(projectId: string) {
    const { personId } = this.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}`)

    await this.context.dispatch('loadNavbar')

    return projectId
  }

  // *******************************************************************************************
  // *
  // *                         Mutations
  // *
  // *******************************************************************************************

  @Mutation
  public upsertProject(project: Project) {
    if (this.projects.find((x) => x.id === project.id)) {
      this.projects = this.projects.map((p) => {
        if (p.id === project.id) {
          p.copyFrom(project)
        }
        return p
      })
    } else {
      this.projects.push(project)
    }
  }

  @Mutation
  public upsertProjectParticipations({ projectId, participations, clearFirst }: {
    projectId: string,
    participations: Participation[],
    clearFirst: boolean,
  }) {
    this.projects = this.projects.map((project) => {
      if (project.id === projectId) {
        if (clearFirst) {
          project.participations = participations
          return project
        }
        project.participations = project.participations.map((participation) => {
          const updatedParticipation = participations.find((y) => y.id === participation.id)
          if (updatedParticipation) {
            participation.copyFrom(updatedParticipation)
          }
          return participation
        })
        for (const updatedParticipation of participations) {
          const alreadyExists = project.participations.find((x) => x.id === updatedParticipation.id)
          if (!alreadyExists) {
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
  public upsertProjectInvitations({ projectId, invitations, clearFirst }: {
    projectId: string,
    invitations: Participation[],
    clearFirst: boolean,
  }) {
    this.projects = this.projects.map((project) => {
      if (project.id === projectId) {
        if (clearFirst) {
          project.invitations = invitations
          return project
        }
        project.invitations = project.invitations.map((invitation) => {
          const updatedInvitation = invitations.find((y) => y.id === invitation.id)
          if (updatedInvitation) {
            invitation.copyFrom(updatedInvitation)
          }
          return invitation
        })
        for (const updatedInvitation of invitations) {
          const alreadyExists = project.invitations.find((x) => x.id === updatedInvitation.id)
          if (!alreadyExists) {
            project.invitations.push(updatedInvitation)
          }
        }
      }
      return project
    })
  }

  @Mutation
  public removeProjectInvitation({ projectId, invitationId }: {
    projectId: string,
    invitationId: string,
  }) {
    this.projects = this.projects.map((p) => {
      if (p.id === projectId) {
        p.invitations = p.invitations.filter((x) => x.id !== invitationId)
      }
      return p
    })
  }

  @Mutation
  public upsertProjectRequests({ projectId, requests, clearFirst }: {
    projectId: string,
    requests: Participation[],
    clearFirst: boolean,
  }) {
    this.projects = this.projects.map((project) => {
      if (project.id === projectId) {
        if (clearFirst) {
          project.requests = requests
          return project
        }
        project.requests = project.requests.map((request) => {
          const updatedRequest = requests.find((y) => y.id === request.id)
          if (updatedRequest) {
            request.copyFrom(updatedRequest)
          }
          return request
        })
        for (const updatedRequest of requests) {
          const alreadyExists = project.requests.find((x) => x.id === updatedRequest.id)
          if (!alreadyExists) {
            project.requests.push(updatedRequest)
          }
        }
      }
      return project
    })
  }

  @Mutation
  public removeProjectRequest({ projectId, requestId }: {
    projectId: string,
    requestId: string,
  }) {
    this.projects = this.projects.map((p) => {
      if (p.id === projectId) {
        p.requests = p.requests.filter((x) => x.id !== requestId)
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
            role.copyFrom(updatedRole)
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
  protected removeProject(projectId: string) {
    this.projects = this.projects.filter((x) => x.id !== projectId)
  }

}
