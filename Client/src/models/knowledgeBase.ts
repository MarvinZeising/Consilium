class Topic {
  public static create(data: any) {
    return new Topic(
      data.id,
      data.projectId,
      data.name)
  }

  public id: string
  public projectId: string
  public name: string
  public articles: Article[] = []

  constructor(id: string, projectId: string, name: string) {
    this.id = id
    this.projectId = projectId
    this.name = name
  }

  public copyFrom(topic: Topic) {
    this.name = topic.name
  }

  public get getArticles() {
    return [...this.articles].sort((a, b) => {
      if (a.title < b.title) {
        return -1
      } else if (a.title > b.title) {
        return 1
      } else {
        return 0
      }
    })
  }
}

class Article {
  public static create(data: any) {
    return new Article(
      data.id,
      data.topicId,
      data.title,
      data.content)
  }

  public id: string
  public topicId: string
  public title: string
  public content: string

  constructor(
    id: string,
    topicId: string,
    title: string,
    content: string) {
    this.id = id
    this.topicId = topicId
    this.title = title
    this.content = content
  }
}

export {
  Topic,
  Article,
}
