namespace UINextList

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next

/// Some shortcut functions for working with the RDOM library.
[<AutoOpen>]
[<JavaScript>]
module internal Utilities =

    /// Class attribute
    let cls n = Attr.Class n

    /// Style attribute
    let sty n v = Attr.Style n v

    /// Static attribute
    let ( ==> ) k v = Attr.Create k v

    /// Div with single class
    let divc c docs = Doc.Element "div" [cls c] docs

    /// Text node
    let txt t = Doc.TextNode t

    /// Button with Bootstrap attributes
    let btn caption act = Doc.Button caption [cls "btn"; cls "btn-default"] act

    /// Link with click callback
    let link cap attr act = Doc.Link cap attr act

    /// Link with click callback
    let href text url = Doc.Element "a" ["href" ==> url] [txt text]

    let listGroup docs = divc "list-group" docs

    let listGroupItem cap act = Doc.Link cap [cls "list-group-item"] act

    let image src = Doc.Element "img" ["src" ==> src] []