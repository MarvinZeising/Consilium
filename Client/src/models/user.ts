import moment from 'moment'

enum Theme {
  Light = 'light',
  Dark = 'dark',
}

class User {
  public static create(data: any) {
    return new User(
      data.id,
      data.email,
      data.language,
      data.theme,
      data.dateFormat,
      data.timeFormat,
      data.createdTime,
      data.lastUpdatedTime)
  }

  public id: string
  public email: string
  public language: string
  public theme: Theme
  public dateFormat: string
  public timeFormat: string
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    email: string,
    language: string,
    theme: string,
    dateFormat: string,
    timeFormat: string,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.email = email
    this.language = language
    this.theme = theme === 'dark' ? Theme.Dark : Theme.Light
    this.dateFormat = dateFormat
    this.timeFormat = timeFormat
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public formatDate(datetime: string) {
    return moment(datetime).format(this.dateFormat)
  }

  public formatTime(datetime: string) {
    return moment(datetime).format(this.timeFormat)
  }

  public formatDateTime(datetime: string) {
    return moment(datetime).format(`${this.dateFormat}, ${this.timeFormat}`)
  }
}

export {
  User,
  Theme,
}
