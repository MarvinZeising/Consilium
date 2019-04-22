namespace Projects

type Project =
    {
        Id: string
        Name: string
        Email: string
    }

type ProjectSave = Project -> Project

type ProjectCriteria =
    | All

type ProjectFind = ProjectCriteria -> Project[]

type ProjectFindById = string -> Project option

type ProjectDelete = string -> bool
