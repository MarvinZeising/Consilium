import axios from 'axios'
import { Module, VuexModule, Action, Mutation, MutationAction } from 'vuex-module-decorators'
import store from '../plugins/vuex'
import router from '../router'
import { Topic, Article } from '../models'

@Module({ dynamic: true, store, name: 'KnowledgeBaseModule' })
export default class KnowledgeBaseModule extends VuexModule {

  public get getActiveTopic() {
    return this.context.getters
      .getActiveProject
      ?.getTopics
      .find((x: Topic) => x.id === router.currentRoute.params.topicId)
  }

  public get getActiveArticle() {
    return this.context.getters
      .getActiveProject
      ?.getTopics
      .find((x: Topic) => x.id === router.currentRoute.params.topicId)
      ?.articles
      .find((x: Article) => x.id === router.currentRoute.params.articleId)
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
  public async loadArticles() {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    const topic = this.getActiveTopic
    if (personId && projectId && topic) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/topics/${topic.id}/articles`)
      const articles = response.data.map((x: any) => Article.create(x))
      topic.articles = articles
    }
  }

  @Action
  public async loadArticle(articleId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    const topic: Topic = this.getActiveTopic
    if (personId && projectId && topic) {
      const response = await axios.get(`/persons/${personId}/projects/${projectId}/topics/${topic.id}/articles/${articleId}`)
      const article = Article.create(response.data)

      if (topic.articles.find((x: Article) => x.id === article.id)) {
        topic.articles = topic.articles.map((x) => {
          if (x.id === article.id) {
            x.content = article.content
          }
          return x
        })
      } else {
        topic.articles.push(article)
      }
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
  public async createArticle(data: {
    title: string,
    content: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    const topic: Topic = this.getActiveTopic

    const response = await axios.post(`/persons/${personId}/projects/${projectId}/topics/${topic.id}/articles`, {
      title: data.title,
      content: data.content,
    })
    const article = Article.create(response.data)
    topic.articles.push(article)
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
  public async updateArticle(data: {
    articleId: string,
    title: string,
    content: string,
  }) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    const topic: Topic = this.getActiveTopic

    const response = await axios.put(
      `/persons/${personId}/projects/${projectId}/topics/${topic.id}/articles/${data.articleId}`, {
      title: data.title,
      content: data.content,
    })
    const article = Article.create(response.data)
    topic.articles = topic.articles.map((x) => {
      if (x.id === article.id) {
        x.title = article.title
        x.content = article.content
      }
      return x
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

  @Action
  public async deleteArticle(articleId: string) {
    const { personId, projectId } = this.context.getters.resolvePersonAndProject
    const topic: Topic = this.getActiveTopic

    await axios.delete(`/persons/${personId}/projects/${projectId}/topics/${topic.id}/articles/${articleId}`)

    topic.articles = topic.articles.filter((x) => x.id !== articleId)
  }

}
