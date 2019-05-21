namespace Consilium

module ProjectActions =

    open Giraffe
    open System
    open ProjectTypes
    open ProjectRepository
    open Actions

    let findAllProjects =
        getAllProjects ()

    let findProjectById =
        getProjectById

    let updateGeneral (input: UpdateGeneralRequest) =
        result {
            let! validatedName = input.Name |> ProjectValidation.validateName
            let! validatedEmail = input.Email |> EmailValidation.validate
            let request = { input with
                                  Name = validatedName
                                  Email = validatedEmail
            }
            return! updateGeneral request
        }

    let createProject (input: CreateProjectRequest) =
        result {
            let projectId = ShortGuid.fromGuid(Guid.NewGuid())
            let! validatedName = input.Name |> ProjectValidation.validateName
            let! validatedEmail = input.Email |> EmailValidation.validate
            let project = { Id = projectId
                            Name = validatedName
                            Email = validatedEmail}
            return! insertProject project 
        }


    let deleteProject projectId =
        result {
            return! deleteProject projectId
        }
