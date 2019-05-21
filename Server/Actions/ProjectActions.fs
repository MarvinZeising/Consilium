namespace Consilium

module ProjectActions =

    open ProjectTypes
    open ProjectRepository
    open Actions

    let findAllProjects =
        getAllProjects ()

    let findProjectById =
        getProjectById

    let updateGeneral (input : UpdateGeneralRequest) =
        result {
            let! validatedName = input.Name |> ProjectValidation.validateName
            let! validatedEmail = input.Email |> EmailValidation.validate
            let request = { input with
                                  Name = validatedName
                                  Email = validatedEmail
            }
            return! updateGeneral request
        }
