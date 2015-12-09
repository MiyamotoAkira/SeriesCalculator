This code is from an exercise done for Gamesys

There are a few decisions to take into account.

First, there is no specification of the context in which this is created.

There was no specification about performance. I am using Decimal that provides precision.
For better performance Double would be used.

List was used as well, to use by default an inmutable collection. If better perfomance were needed the use of
a mutable collection could be investigated.

To provide a good selection of tests FsCheck has been used.

This has raised a few possible errors, with the use of 0 a denominator, the lenght of 
the series and the size of the input as sticking points.

Instead of having to test after the function if errors have been raised, the decision of using a discriminated
union was taken. The idea is to use what Scott Wachlin calls "Railway Oriented Programming" (ROP). That way
the functions can be concatenated without exceptions being raised. It does mean additional checks inside 
each function, but simplifies the client.

As there is no context to the exercise, some of the parameter and variable names are a bit underwhelming.
With a proper context those names could be improved.