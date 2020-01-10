import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import { Topic } from '../models'

@Module({ dynamic: true, store, name: 'KnowledgeBaseModule' })
export default class KnowledgeBaseModule extends VuexModule {

  public get getActiveTopic() {
    return this.context.getters
      .getActiveProject
      ?.getTopics
      .find((x: Topic) => x.id === router.currentRoute.params.topicId)
  }

  @Action
  public async loadTopics() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    if (personId && projectId) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/topics`)
      const topics = response.data.map((x: any) => Topic.create(x))

      this.context.commit('upsertProjectTopics', {
        projectId,
        topics,
        clearFirst: true,
      })
    }
  }

  @Action
  public async createTopic(name: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/topics`, { name })
    const topic = Topic.create(response.data)

    this.context.commit('upsertProjectTopics', {
      projectId,
      topics: [topic],
    })
  }

  @Action
  public async updateTopic(data: {
    topicId: string,
    name: string
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    const response = await axios.put(`/persons/${personId}/projects/${projectId}/topics/${data.topicId}`, {
      name: data.name
    })
    const topic = Topic.create(response.data)

    this.context.commit('upsertProjectTopics', {
      projectId,
      topics: [topic],
    })
  }

  @Action
  public async deleteTopic(topicId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject

    await axios.delete(`/persons/${personId}/projects/${projectId}/topics/${topicId}`)

    this.context.commit('removeProjectTopic', {
      projectId,
      topicId,
    })
  }

}
