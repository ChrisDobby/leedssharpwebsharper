namespace Formlet

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet
open IntelliFactory.WebSharper.Formlet.Controls

[<JavaScript>]
module Client =
    let ProductForm =
        let products = Remoting.Products ()
        let product =
            Select 0 products
            |> Enhance.WithTextLabel "Product"
        let quantity =
            Input ""
            |> Validator.IsInt "Quantity must be a number"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Quantity"
        Formlet.Yield (fun p q ->
            {ProductId = p; Quantity = int q})
        <*> product
        <*> quantity

    let CustomerForm : Formlet<Customer> =
        let i label errMsg =
            Input ""
            |> Validator.IsNotEmpty errMsg
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel label
        let n =
            Input ""
            |> Validator.Is (fun s -> s.ToUpper() = "LEEDS SHARP") "Your name must be Leeds Sharp"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Name"
        let e = 
            Input ""
            |> Validator.IsEmail "Must be valid email address"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Email"

        let p = 
            Input ""
            |> Validator.IsNotEmpty "Empty postcode not allowed"
            |> Validator.IsRegexMatch "^(([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z])))) [0-9][A-Za-z]{2}))$" "Must be a valid postcode"
            |> Enhance.WithValidationIcon
            |> Enhance.WithTextLabel "Postcode"

        Formlet.Yield (fun nm em st ct cnt pc ->
            {Name = nm; Email = em; Street = st; City = ct; County = cnt; PostCode = pc})
        <*> n
        <*> e
        <*> i "Street" "Empty street not allowed"
        <*> i "City" "Empty city not allowed"
        <*> i "County" "Empty county not allowed"
        <*> p

    let OrderForm () =
        let items =
            (ProductForm |> Enhance.WithLegend "Product")
            |> Enhance.Many
            |> Enhance.WithLegend "Items"
        let customer =
            CustomerForm
            |> Enhance.WithLegend "Customer"

        Formlet.Yield (fun lines cust ->
            {
                Lines = lines
                Customer = cust
            }
        )
        <*> items
        <*> customer

    let Main() =
        let container = {
            Enhance.FormContainerConfiguration.Default with
                Header = "Order Formlet" |> Enhance.FormPart.Text |> Some
                Description =
                    "Order your halloween stuff!"
                    |> Enhance.FormPart.Text
                    |> Some
            }

        OrderForm()
            |> Enhance.WithSubmitAndResetButtons
            |> Enhance.WithCustomFormContainer container
            |> Formlet.Run (fun o -> Remoting.ProcessOrder o |> ignore)
