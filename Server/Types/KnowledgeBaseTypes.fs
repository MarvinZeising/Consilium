namespace Consilium

module KnowledgeBaseTypes =

    [<CLIMutable>]
    type CreateTopicRequest =
         { ProjectId: string
           Name: string
           Order: int }

    [<CLIMutable>]
    type UpdateTopicRequest =
         { Id: string
           Name: string
           Order: int }

    type Topic =
         { Id: string
           ProjectId: string
           Name: string
           Order: int }
