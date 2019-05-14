module Wiki.WikiCollection

open Wiki
open MongoDB.Driver
open Microsoft.Extensions.DependencyInjection

let find (collection : IMongoCollection<Tab>) (criteria : TabCriteria) : Tab[] =
    [|
       {
            Id = "1"
            Name = "FAQs"
            Items = [|
                {
                    Id = "1"
                    Title = "Et mollit quis duis velit"
                    Text = "Cupidatat dolor ex cupidatat minim do amet ex ullamco."
                }
            |]
       };
       {
            Id = "2"
            Name = "About the Team"
            Items = [|
                {
                    Id = "2"
                    Title = "Est minim ad cupidatat anim"
                    Text = "Esse irure culpa incididunt cupidatat consequat id."
                }
            |]
       };
       {
            Id = "3"
            Name = "Meeting-Point Information"
            Items = [|
                {
                    Id = "3"
                    Title = "Non ipsum velit consequat exercitation ut duis"
                    Text = "Ex ex ullamco sint do ullamco ipsum ex ex."
            }
            |]
        };
        {
            Id = "4"
            Name = "Videos"
            Items = [|
                {
                    Id = "4"
                    Title = "Lorem proident culpa cillum ad"
                    Text = "Aliqua ullamco pariatur proident labore incididunt."
                }
            |]
        };
        {
            Id = "5"
            Name = "Miscellaneous"
            Items = [|
                {
                    Id = "5"
                    Title = "Laborum irure do exercitation et quis"
                    Text = "Aute velit irure aliqua fugiat ut dolore elit officia in exercitation dolor."
                };
                {
                    Id = "6"
                    Title = "Lorem non anim proident aliquip in dolore"
                    Text = "Laboris cillum sunt nisi irure commodo sunt exercitation reprehenderit."
                };
                {
                    Id = "7"
                    Title = "Id nostrud aliquip irure aliquip voluptate"
                    Text = "Cupidatat pariatur laboris do occaecat veniam officia qui sunt laboris dolor amet ut non."
                }
            |]
      }
    |]

let save (collection : IMongoCollection<Tab>) (project : Tab) : Tab =
    project

let delete (collection : IMongoCollection<Tab>) (id : string) : bool =
    true

type IServiceCollection with
    member this.AddTabCollection (collection : IMongoCollection<Tab>) =
        this.AddSingleton<TabFind>(find collection) |> ignore
        this.AddSingleton<TabSave>(save collection) |> ignore
        this.AddSingleton<TabDelete>(delete collection) |> ignore
