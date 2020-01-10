import { Participation } from '.'

enum Gender {
  Male = 'male',
  Female = 'female',
}

class Congregation {
  public static create(data: any) {
    return new Congregation(
      data.id,
      data.name,
      data.number,
      data.numberOfParticipants,
      data.createdBy,
      data.lastUpdatedBy,
      data.createdTime,
      data.lastUpdatedTime)
  }

  public id: string
  public name: string
  public number: string
  public numberOfParticipants: number
  public createdBy: string
  public lastUpdatedBy: string
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    name: string,
    // tslint:disable-next-line: variable-name
    number: string,
    numberOfParticipants: number,
    createdBy: string,
    lastUpdatedBy: string,
    createdTime: string,
    lastUpdatedTime: string
  ) {
    this.id = id
    this.name = name
    this.number = number
    this.numberOfParticipants = numberOfParticipants
    this.createdBy = createdBy
    this.lastUpdatedBy = lastUpdatedBy
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }
}

class Person {
  public static create(data: any) {
    const person = new Person(
      data.id,
      data.firstname,
      data.lastname,
      data.createdTime,
      data.lastUpdatedTime)
    person.congregationId = data.congregationId ? data.congregationId : undefined
    person.congregation = data.congregation ? Congregation.create(data.congregation) : undefined
    if (data.participations) {
      person.participations = data.participations.map((x: any) => Participation.create(x))
    }
    return person
  }

  public id: string
  public firstname: string
  public lastname: string
  public congregationId?: string
  public congregation?: Congregation
  public gender: Gender = Gender.Male
  public participations: Participation[] = []
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    firstname: string,
    lastname: string,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
    this.createdTime = createdTime
    this.lastUpdatedTime = lastUpdatedTime
  }

  public get getFullName() {
    return this.firstname + ' ' + this.lastname
  }

  public get getCongregationName() {
    return this.congregation?.name || ''
  }

  public get getParticipations() {
    return [...this.participations].sort((a, b) => {
      if (a.project && b.project) {
        if (a.project.name < b.project.name) {
          return 1
        } else if (a.project.name > b.project.name) {
          return -1
        } else {
          return 0
        }
      } else {
        return 0
      }
    })
  }

  public copyFrom(person: Person) {
    this.firstname = person.firstname
    this.lastname = person.lastname
    this.gender = person.gender
  }

}

export {
  Person,
  Gender,
  Congregation,
}
