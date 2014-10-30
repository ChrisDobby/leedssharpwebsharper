namespace UINextMultiView

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Html
open IntelliFactory.WebSharper.Html
open Utilities

[<JavaScript>]
module MainView =
    type ViewModel = 
        {
            Matches : ListModel<string,Match>
            Selected : Var<Match option>
        }

    let ReadGames f =
        async {
            let! games = Remoting.Games()
            f games
        } |> Async.Start

    let CreateModel() = 
        { Matches = ListModel.Create(fun m -> m.Id )[]
          Selected = Var.Create None }

    let RenderMatchItem (model : ViewModel) (m : Match) : Doc =
        listGroupItem m.Description (fun _ -> Var.Set model.Selected (Some m))

    let MatchList model =
        ListModel.View model.Matches
            |> Doc.ConvertBy (fun m -> m.Id) (RenderMatchItem model)

    let Main showMatch =
        let m = CreateModel()

        let selectedMatch = 
            let hideButton sm = 
                btn "Hide game" (fun _ -> m.Matches.Remove sm
                                          Var.Set m.Selected None)

            let showButton sm =
                btn "Show game" (fun _ -> showMatch sm.Id)

            m.Selected.View
            |> View.Map (fun g -> match g with
                                    | None -> Doc.TextNode "No Match Selected"
                                    | Some(m) -> divc "" [
                                                            Doc.TextNode m.Description
                                                            divc "md12" [
                                                                            hideButton m
                                                                            showButton m
                                                                        ]
                                                         ])
            |> Doc.EmbedView

        ReadGames (fun games -> games |> Seq.iter (fun g -> m.Matches.Add g))

        Div0[
            divc "row well" [
                divc "jumbotron"[selectedMatch]
            ]

            divc "row well" [
                listGroup[MatchList m] 
            ]
        ]


