namespace SeriesCalculatorLogicTests

module AdditionalTests =
    open SeriesCalculatorLogic.SeriesCalculator

    open FsCheck
    open FsCheck.NUnit

    let isValue result =
        match result with 
        | Value _ -> true
        | Error _ -> false
        | Series _ -> false

    let isError result =
        match result with 
        | Value _ -> false
        | Error _ -> true
        | Series _ -> false

    let isSeries result =
        match result with 
        | Value _ -> false
        | Error _ -> false
        | Series _ -> true

    type justValue =
        static member Result () = Arb.Default.Derive () |> Arb.filter (isValue) 

    type justSeries =
        static member Result () = Arb.Default.Derive () |> Arb.filter (isSeries)

    [<Property>]
    let ``We get a firstNumber`` number =
        match number with
        | number when isPassLimit number -> getFirstNumber number |> isError
        | _ -> getFirstNumber number |> isValue


    [<Property(Arbitrary = [|typeof<justValue>|])>]
    let ``We get a growthRate`` firstNumber =
        match firstNumber with
        | x when x = 0m -> getGrowthRate (Value firstNumber) 5m |> isError
        | _ -> getGrowthRate (Value firstNumber) 5m |> isValue


    [<Property(Arbitrary = [|typeof<justSeries>|])>]
    let ``We get the first special number`` series =
        match series with 
        | Series s when s.Length > 2 -> getFirstSpecialNumber series |> isValue 
        | _ -> getFirstSpecialNumber series |> isError

    [<Property>]
    let ``We get the second special number`` number series =
        match series with 
        | Series s when s.Length > 0 ->
            getSecondSpecialNumber number series |> isValue
        | _ -> getSecondSpecialNumber number series |> isError


    Check.Quick(``We get a firstNumber``)
    Check.Quick(``We get a growthRate``)
    Check.Quick(``We get the first special number``)
    Check.Quick(``We get the second special number``)