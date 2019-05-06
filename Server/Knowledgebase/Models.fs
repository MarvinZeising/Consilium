namespace Knowledgebase

type Item =
    {
        Id: string
        Title: string
        Text: string
    }

type ItemSave = Item -> Item

type ItemCriteria =
    | All

type Tab =
    {
        Id: string
        Name: string
        Items: Item[]
    }
