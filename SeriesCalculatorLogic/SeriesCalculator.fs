namespace SeriesCalculatorLogic


module SeriesCalculator = 
    let limitStarterNumber = 1000000000m
    let firstSpecialPosition = 3
        
    type Result=
    | Series of List<decimal>
    | Value of decimal
    | Error of string

    let isPassLimit number = 
        number > limitStarterNumber || number < -limitStarterNumber

    let getFirstNumber starterNumber = 
        match starterNumber with
        | number when isPassLimit number -> Error "Number is too big for the system"
        | number -> Value(((0.5m * pown number 2 ) + (30.0m * number) + 10.0m) / 25.0m)

    let getGrowthRate firstNumber growthModifier =
        match firstNumber with
        | Value number when number = 0m -> Error "The initial number is 0, which would create an invalid growth rate"
        | Value number -> Value(2.0m * growthModifier / 100.0m / 25.0m / number)
        | _ -> Error "Unexpected input for the getGrowthRate function"

    let getSeries firstNumber growthRate length =
        match (firstNumber, growthRate) with
        | (Error x, _) -> Error x
        | (_, Error x) -> Error x
        | (Series x, _) -> Error "A series is not expected here"
        | (_, Series x) -> Error "A series is not expected here"
        | (Value number, Value growth) ->
            let calculateValue element =
                if element = 1 then number
                else growth * (pown number (element - 1))

            let round (number: decimal) =
                let truncated = System.Decimal.Truncate number
                let decimals = number - truncated

                match decimals with
                | x when x <= 0.125m ->  truncated
                | x when x > 0.125m && x <= 0.375m -> truncated + 0.25m
                | x when x > 0.375m && x <= 0.625m -> truncated + 0.5m
                | x when x > 0.625m && x <= 0.875m -> truncated + 0.75m
                | x when x > 0.875m -> truncated + 1m
                | _ -> failwith "How we have come here? It should never reach due to the use decimals"
            
            Series(
                [1..length]
                |> List.map calculateValue 
                |> List.map round
            )

    let getFirstSpecialNumber inputSeries =
        match inputSeries with
        | Error x -> Error x
        | Value x -> Error "A value is not expected here"
        | Series series when series.Length < firstSpecialPosition -> Error "The series is too small to produce the first special number"
        | Series series ->
            let converted = series |> List.toArray
            Value converted.[series.Length - firstSpecialPosition]

    let getSecondSpecialNumber z inputSeries =
        match (z, inputSeries) with
        | (_, Error x) -> Error x
        | (_, Value x) -> Error "A value is not expected here"
        | (number, _) when number = 0m -> Error "The passed number will create an invalid"
        | (_, Series series) when series.Length < 1 -> Error "The series is too small to produce the second special number"
        | (number, Series series) ->
            
            let approximateNumber = 1000m / number

            let getDistance number =
                System.Math.Abs(number - approximateNumber)

            let rec compare remaining current =
                match remaining with
                | head :: tail when getDistance head < getDistance current -> compare tail head
                | head :: tail when getDistance head = getDistance current && head > current -> compare tail head
                | _ :: tail -> compare tail current
                | [] -> Value current
                    
            compare series.Tail series.Head