namespace UINextTextbox

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.UI.Next
open IntelliFactory.WebSharper.UI.Next.Html
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Google.Visualization
open IntelliFactory.WebSharper.Google.Visualization.Base
open Utilities

[<JavaScript>]
module MainView =
    let Main =
        let t = Var.Create ""
        let input = Doc.Input[] t
        let label = Doc.TextView t.View
        let labelCaps = Doc.TextView (t.View |> View.Map (fun s -> s.ToUpper()))
        let vowelChart = 
            t.View
            |> View.Map (fun s -> let vowels = s.ToLower()
                                                    |> Seq.filter (fun c -> ['a';'e';'i';'o';'u'] |> List.tryFind (fun n -> n = c) <> None)
                                                    |> Seq.length
                                  let numbers = s.ToLower() 
                                                    |> Seq.filter (fun c -> ['0'..'9'] |> List.tryFind (fun n -> n = c) <> None)
                                                    |> Seq.length
                                  let chartDiv = Div[]
                                  let chart = new ColumnChart(chartDiv.Dom)
                                  let chartData = IntelliFactory.WebSharper.Google.Visualization.Base.DataTable()
                                  chartData.addColumn(IntelliFactory.WebSharper.Google.Visualization.Base.ColumnType.StringType, "Count") |> ignore
                                  chartData.addColumn(IntelliFactory.WebSharper.Google.Visualization.Base.ColumnType.NumberType, "Vowels") |> ignore
                                  chartData.addColumn(IntelliFactory.WebSharper.Google.Visualization.Base.ColumnType.NumberType, "Numbers") |> ignore
                                  chartData.addRows(1) |> ignore
                                  chartData.setCell(0, 1, vowels)
                                  chartData.setCell(0, 2, numbers)
                                  chart.draw(chartData, ColumnChartOptions(height = 300, 
                                                                           width = 450,
                                                                           vAxis = Axis(minValue = 0., maxValue = 15.)))
                                  chartDiv.Dom |> Doc.Static)
            |> Doc.EmbedView

        Div0[
            divc "row well" [
                divc "md12" [input]
                divc "md12" [label]
                divc "md12" [labelCaps]
                divc "md12" [vowelChart]
            ]
        ]
