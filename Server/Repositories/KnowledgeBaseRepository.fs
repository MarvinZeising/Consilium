namespace Consilium

module KnowledgeBaseRepository =

    open KnowledgeBaseDatabase
    open Repository

    let updateTopic<'a> =
        updateTopic |> tryCatchError

    let insertTopic<'a> =
        insertTopic |> tryCatchError

    let deleteTopic<'a> =
        deleteTopic |> tryCatchError
