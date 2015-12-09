namespace SeriesCalculatorLogicTests

module SeriesCalculatorTests =
    open NUnit.Framework
    open FsUnit
    open SeriesCalculatorLogic

    [<Test>]
    let ``When x is 1 then first number should be 1.62``() =
        let result = SeriesCalculator.getFirstNumber 1m
        result |> should equal (SeriesCalculator.Value(1.62m))

    [<Test>]
    let ``When first Number is 1.62 and y is 5062.5 then grow rate should be 2.5`` () =
        let result = SeriesCalculator.getGrowthRate (SeriesCalculator.Value 1.62m) 5062.5m
        result |> should equal (SeriesCalculator.Value(2.5m))

    [<Test>]
    let ``Calculate 5 elements of series`` () =
        let expected = (SeriesCalculator.Series [1.5m; 4m; 6.5m; 10.75m; 17.25m])

        let result = SeriesCalculator.getSeries (SeriesCalculator.Value 1.62m) (SeriesCalculator.Value 2.5m) 5
        result |> should equal expected

    [<Test>]
    let ``For FirstNumber 1.62 and growth rate 2.5 and length 5 getFirstSpecialNumber is 6.5`` () =
        let series = SeriesCalculator.getSeries (SeriesCalculator.Value 1.62m) (SeriesCalculator.Value 2.5m) 5
        let result = SeriesCalculator.getFirstSpecialNumber series
        result |> should equal (SeriesCalculator.Value 6.5m)

    [<Test>]
    let ``For firstNumber 1.62, growth rate 2.5, length 5 and input 160 getSecondSpecialNumber is 6.5`` () =
        let series = SeriesCalculator.getSeries (SeriesCalculator.Value 1.62m) (SeriesCalculator.Value 2.5m) 5
        let result = SeriesCalculator.getSecondSpecialNumber 160m series
        result |> should equal (SeriesCalculator.Value 6.5m)