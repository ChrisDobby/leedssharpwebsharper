namespace UINextMultiView

open IntelliFactory.WebSharper
open FSharp.Data

type Match = {
    Id : string
    Source : string
    Description : string;
    Url : string;
    Venue : string;
    Latitude : string;
    Longitude : string;
    Country : string;
    LastUpdate : System.DateTime
}

module Remoting =
    type MatchFeed = JsonProvider<"./matches.json">

    [<Remote>]
    let Games() =
        async {
            let matches = MatchFeed.Load("./matches.json") 
                            |> Seq.map(fun m -> {
                                                    Id = m.Id.ToString()
                                                    Source = m.Source
                                                    Description = m.Description
                                                    Url = m.Url
                                                    Venue = m.Venue
                                                    Latitude = string m.Latitude
                                                    Longitude = string m.Longitude
                                                    Country = m.Country
                                                    LastUpdate = m.LastUpdate
                                                })
                            |> Seq.toList
            return matches
        }

    [<Remote>]
    let Game(id:string) =
        async {
            let m = MatchFeed.Load("./matches.json")
                        |> Seq.filter (fun mt -> mt.Id.ToString() = id)
                        |> Seq.toList
                        |> List.head

            return {
                        Id = m.Id.ToString()
                        Source = m.Source
                        Description = m.Description
                        Url = m.Url
                        Venue = m.Venue
                        Latitude = string m.Latitude
                        Longitude = string m.Longitude
                        Country = m.Country
                        LastUpdate = m.LastUpdate
                   }
        }