import { Participation } from '.'

enum Gender {
  Male = 'male',
  Female = 'female',
}

enum Privilege {
  Publisher = 'publisher',
  AuxiliaryPioneer = 'auxiliary',
  RegularPioneer = 'regular',
  SpecialPioneer = 'special',
  Circuit = 'circuit',
  Bethelite = 'bethelite',
  ConstructionServant = 'construction',
}

enum Assignment {
  Publisher = 'publisher',
  MinisterialServant = 'ministerial',
  Elder = 'elder',
  COBE = 'cobe',
  Secretary = 'secretary',
  ServiceOverseer = 'serviceOverseer',
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
      data.gender || Gender.Male,
      data.email || '',
      data.phone || '',
      data.languages || '',
      data.privilege || Privilege.Publisher,
      data.assignment || Assignment.Publisher,
      data.notes || '',
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
  public gender: Gender
  public email: string
  public phone: string
  public languages: string
  public privilege: Privilege
  public assignment: Assignment
  public notes: string
  public participations: Participation[] = []
  public createdTime: string
  public lastUpdatedTime: string

  constructor(
    id: string,
    firstname: string,
    lastname: string,
    gender: Gender,
    email: string,
    phone: string,
    languages: string,
    privilege: Privilege,
    assignment: Assignment,
    notes: string,
    createdTime: string,
    lastUpdatedTime: string,
  ) {
    this.id = id
    this.firstname = firstname
    this.lastname = lastname
    this.gender = gender
    this.email = email
    this.phone = phone
    this.languages = languages
    this.privilege = privilege
    this.assignment = assignment
    this.notes = notes
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
    this.email = person.email
    this.phone = person.phone
    this.languages = person.languages
    this.privilege = person.privilege
    this.assignment = person.assignment
    this.notes = person.notes
  }

}

export {
  Person,
  Gender,
  Congregation,
  Assignment,
  Privilege,
}
