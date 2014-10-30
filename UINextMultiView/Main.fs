namespace UINextMultiView

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

type Action =
    | Home

[<JavaScript>]
module Client =
    open IntelliFactory.WebSharper.UI.Next

    type Pages =
    | Home
    | Game of string

    let Main () =
        let routeMap =
            RouteMap.Create
                (function
                    | Home -> []
                    | Game id -> ["game";id])
                (function
                    | "game"::id -> Game (id |> List.head)
                    | _ -> Home)

        let transitionView v =
            let fade = Anim.Simple Interpolation.Double Easing.CubicInOut 300.
            let fadeTrans = 
                Trans.Create fade
                |> Trans.Enter (fun _ -> fade 0. 1.)
                |> Trans.Exit (fun _ -> fade 1. 0.)
            Doc.Element "div" [Attr.AnimatedStyle "opacity" fadeTrans (View.Const 1.) string] [v]

        let p = RouteMap.Install routeMap

//        let p = Var.Create Home
        View.FromVar p
            |> View.Map(fun pg -> match pg with
                                    | Home -> MainView.Main (fun id -> Var.Set p (Game id))
                                    | Game id -> GameView.Main id (fun _ -> Var.Set p Home)
                                  |> transitionView)
            |> Doc.EmbedView
            |> Doc.AsPagelet

module Controls =

    [<Sealed>]
    type EntryPoint() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            Client.Main()

module Skin =
    open System.Web

    type Page =
        {
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Action> =
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Body = body context
            }

module Site =
    open IntelliFactory.Html
    let HomePage =
        Skin.WithTemplate "HomePage" <| fun ctx ->
            [
                Div [new Controls.EntryPoint()]
            ]

    let Main =
        Sitelet.Sum [
            Sitelet.Content "/" Home HomePage
        ]

[<Sealed>]
type Website() =
    interface IWebsite<Action> with
        member this.Sitelet = Site.Main
        member this.Actions = [Home]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
