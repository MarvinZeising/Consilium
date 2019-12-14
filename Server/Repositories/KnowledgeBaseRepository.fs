namespace Consilium

module KnowledgeBaseRepository =

    open KnowledgeBaseDatabase
    open Repository

    let getAllTopics<'a> =
        getAllTopics |> tryCatchError

    let getTopicsByProjectId =
        getTopicsByProjectId |> tryCatchError

    let updateTopic<'a> =
        updateTopic |> tryCatchError

    let insertTopic<'a> =
        insertTopic |> tryCatchError

    let deleteTopic<'a> =
        deleteTopic |> tryCatchError
