namespace SeriesCalculatorLogic


module SeriesCalculator = 
    
    type Result=
    | Series of List<decimal>
    | Value of decimal
    | Error of string

    let getFirstNumber number = 
        Value(((0.5m * pown number 2 ) + (30.0m * number) + 10.0m) / 25.0m)

    let getGrowthRate firstNumber y =
        match firstNumber with
        | Value x when x = 0m -> Error "The initial number is 0, which would create an invalid growth rate"
        | Value x -> Value(2.0m * y / 100.0m / 25.0m / x)
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
                | _ -> failwith "How we have come here?"
            
            Series(
                [1..length]
                |> List.map calculateValue 
                |> List.map round
            )

    let getFirstSpecialNumber inputSeries =
        match inputSeries with
        | Error x -> Error x
        | Value x -> Error "A value is not expected here"
        | Series series ->
            if series.Length > 2 then
                let converted = series |> List.toArray
                Value converted.[series.Length - 3]
            else
                Error "The series is too small to produce the first special number"

    let getSecondSpecialNumber z inputSeries =
        match inputSeries with
        | Error x -> Error x
        | Value x -> Error "A value is not expected here"
        | Series series ->
            let approximateNumber = 1000m /z

            let getDistance number =
                System.Math.Abs(number - approximateNumber)

            let rec compare remaining current =
                match remaining with
                | head :: tail when getDistance head < getDistance current -> compare tail head
                | head :: tail when getDistance head = getDistance current && head > current -> compare tail head
                | _ :: tail -> compare tail current
                | [] -> Value current
                    
            compare series.Tail series.Head