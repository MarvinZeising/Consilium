namespace Consilium

module KnowledgeBaseTypes =

    type CreateTopicRequest =
         { ProjectId: string
           Name: string }

    type UpdateTopicRequest =
         { Id: string
           Name: string }

    type Topic =
         { Id: string
           ProjectId: string
           Name: string }
