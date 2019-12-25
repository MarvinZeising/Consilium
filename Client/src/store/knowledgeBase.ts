import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import { Topic } from '../models/definitions'

@Module({ dynamic: true, store, name: 'KnowledgeBaseModule' })
export default class KnowledgeBaseModule extends VuexModule {
  public topics: Topic[] = []

  public get allTopics(): Topic[] {
    return this.topics.sort((a: Topic, b: Topic) => a.order - b.order)
  }

  @Action
  public async initKnowledgeBaseModule() {
    const response = await axios.get('/knowledge-base/topics')
    this.context.commit('setTopics', response.data)
  }

  @MutationAction({ mutate: ['topics'] })
  public async clearKnowledgeBases() {
    return { topics: [] }
  }

  @Action
  public async createTopic(topic: Topic) {
    const response = await axios.post('/knowledge-base/topics', {
      projectId: topic.projectId,
      name: topic.name,
    })
    this.context.commit('insertTopic', response.data)
  }

  @Action
  public async changeTopic(topic: Topic) {
    await axios.put('/knowledge-base/topics', {
      id: topic.id,
      name: topic.name,
      order: topic.order,
    })
    this.context.commit('updateTopic', topic)
  }

  @Action
  public async deleteTopic(topicId: string) {
    const topic = this.allTopics.filter((x: Topic) => x.id === topicId)[0]
    await axios.delete(`/knowledge-base/topics/${topicId}`)
    this.context.commit('removeTopic', topicId)

    for (const t of this.allTopics.filter((x: Topic) => x.order > topic.order)) {
      await this.context.dispatch('moveTopicDown', t.id)
    }
  }

  @Action
  public async moveTopicUp(currentOrder: number) {
    for (const topic of this.allTopics) {
      if (topic.order === currentOrder - 1) {
        topic.order++
        await this.context.dispatch('changeTopic', topic)
      } else if (topic.order === currentOrder) {
        topic.order--
        await this.context.dispatch('changeTopic', topic)
        break
      }
    }
  }

  @Action
  public async moveTopicDown(currentOrder: number) {
    for (const topic of this.allTopics) {
      if (topic.order === currentOrder) {
        topic.order++
        await this.context.dispatch('changeTopic', topic)
      } else if (topic.order === currentOrder + 1) {
        topic.order--
        await this.context.dispatch('changeTopic', topic)
        break
      }
    }
  }

  @Mutation
  protected setTopics(topics: Topic[]) {
    this.topics = topics
  }

  @Mutation
  protected insertTopic(topic: Topic) {
    this.topics.push(topic)
  }

  @Mutation
  protected updateTopic(updatedTopic: Topic) {
    for (const topic of this.topics) {
      if (topic.id === updatedTopic.id) {
        topic.name = updatedTopic.name
        topic.order = updatedTopic.order
      }
    }
  }

  @Mutation
  protected removeTopic(topicId: string) {
    // TODO: re-calculate order
    this.topics = this.topics.filter((x: Topic) => x.id !== topicId)
  }

}
