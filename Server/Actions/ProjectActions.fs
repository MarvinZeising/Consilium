namespace Consilium

module ProjectActions =

    open Giraffe
    open System
    open CommonValidation
    open ProjectTypes
    open ProjectRepository
    open Actions

    let private validateProjectName = validateName 40

    let updateGeneral (input: UpdateGeneralRequest) =
        result {
            let! validatedName = input.Name |> validateProjectName
            let! validatedEmail = input.Email |> validateEmail
            let request = { input with
                                  Name = validatedName
                                  Email = validatedEmail
            }
            return! updateGeneral request
        }

    let createProject (input: CreateProjectRequest) =
        result {
            let projectId = ShortGuid.fromGuid(Guid.NewGuid())
            let! validatedName = input.Name |> validateProjectName
            let! validatedEmail = input.Email |> validateEmail
            let project = { Id = projectId
                            Name = validatedName
                            Email = validatedEmail}
            return! insertProject project 
        }

    let deleteProject projectId =
        result {
            return! deleteProject projectId
        }
