namespace Wiki

type Item =
    {
        Id: string
        Title: string
        Text: string
    }

type ItemSave = Item -> Item

type ItemDelete = string -> bool

type ItemCriteria =
    | Id of string
    | Title of string
    | Text of string
    | All

type Tab =
    {
        Id: string
        Name: string
        Items: Item[]
    }

type TabSave = Tab -> Tab

type TabDelete = string -> bool

type TabCriteria =
    | Id of string
    | Name of string
    | All

type TabFind = TabCriteria -> Tab[]
