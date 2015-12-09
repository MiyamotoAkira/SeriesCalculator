namespace SeriesCalculatorLogicTests

module AdditionalTests =
    open NUnit.Framework
    open FsUnit
    open SeriesCalculatorLogic

    open FsCheck
    open FsCheck.NUnit


    let isValue result =
        match result with 
        | SeriesCalculator.Value _ -> true
        | SeriesCalculator.Error _ -> false
        | SeriesCalculator.Series _ -> false

    let isError result =
        match result with 
        | SeriesCalculator.Value _ -> false
        | SeriesCalculator.Error _ -> true
        | SeriesCalculator.Series _ -> false

    let isSeries result =
        match result with 
        | SeriesCalculator.Value _ -> false
        | SeriesCalculator.Error _ -> false
        | SeriesCalculator.Series _ -> true

    [<Property>]
    let ``We get a growthRate`` firstNumber =
        match firstNumber with
        | x when x = 0m -> SeriesCalculator.getGrowthRate (SeriesCalculator.Value firstNumber) 5m |> isError
        | _ -> SeriesCalculator.getGrowthRate (SeriesCalculator.Value firstNumber) 5m |> isValue


    Check.Quick(``We get a growthRate``)


    [<Property>]
    let ``We get the first special number`` series =
        match series with 
        | SeriesCalculator.Series s when s.Length > 2 ->
          SeriesCalculator.getFirstSpecialNumber series |> isValue
        | _ -> SeriesCalculator.getFirstSpecialNumber series |> isError

    Check.Quick(``We get the first special number``)