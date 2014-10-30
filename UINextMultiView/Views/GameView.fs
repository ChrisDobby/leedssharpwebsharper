namespace UINextMultiView

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Html
open IntelliFactory.WebSharper.Html
open Utilities

[<JavaScript>]
module GameView =
    let ReadGame id f =
        async {
            let! game = Remoting.Game (id)
            f game
        } |> Async.Start

    let Main gameId back =
        let game : Var<Match option> = Var.Create None                
        let view g =
            let backButton = btn "Back" (fun _ -> back ())
            let countryImage =
                match g.Country.ToLower() with
                    | "united arab emirates" -> image "./Images/uae.jpg"
                    | "malaysia" -> image "./Images/malaysia.jpg"
                    | "sri lanka" -> image "./Images/sl.jpg"
                    | "new zealand" -> image "./Images/nz.jpg"
                    | "south africa" -> image "./Images/sa.jpg"
                    | _ -> Div0[]
                
            Div0[                
                divc "md12" [backButton]
                divc "well jumbotron" [
                                        divc "well" [countryImage]
                                        Doc.TextNode g.Description
                                      ]
            ]

        let gameDoc = game.View
                            |> View.Map (fun gm -> match gm with
                                                    | None -> Doc.TextNode "No Match Selected"
                                                    | Some g -> view g)
                            |> Doc.EmbedView

        ReadGame gameId (fun g -> Var.Set game (Some g))
        Div0[gameDoc]


