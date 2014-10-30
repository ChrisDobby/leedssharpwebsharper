namespace Formlet

open IntelliFactory.WebSharper

type OrderLine = {
    ProductId : int
    Quantity : int
}

type Customer = {
    Name : string
    Email : string
    Street : string
    City : string
    County : string
    PostCode : string
}

type Order = {
    Customer : Customer
    Lines : List<OrderLine>
}

module Remoting =

    [<Remote>]
    let Products () =
        [
            "Pumpkins", 1
            "Broomsticks", 2
            "Black Cats", 3
            "Cauldrons", 4
        ]

    [<Remote>]
    let ProcessOrder (o:Order) =
        o
