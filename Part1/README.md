# Basic building blocks of C++

For these questions you will code using some of the basic building blocks of C++: vectors, functions, and classes.

Where you are to write functions, ensure you use constness and references where appropriate, to avoid needlessly copying objects, and to help ensure you have written correct code.

Make sure you don't commit any compiled code to your GitHub repository; or if you choose to use an IDE, any large project directories created by your IDE.  You can make these on your machine, but don't `commit` or `add` them to your repository -- this isn't what git is designed for.

----

# COVID case data [3 marks]

For this part of the coursework, you will write code to work with data about individuals that have a positive COVID test result.

Covid cases are described by four properties:

- The person's latitude (a number in degrees, e.g. 51.75087595155126)
- The person's longitude (a number in degrees, e.g. -0.33483137537806396)
- Their name (a string)
- Their age (to the nearest whole number)
- When they tested positive (to keep things simple -- an integral number of hours since 1st January 2020)

In `CovidCase.h`, make a class `CovidCase` that contains these as private member variables.

Give the class an appropriate constructor that initialises these five variables to the arguments passed to the constructor (passed in the above order).

To compile your code, `BasicCovidTest.cpp` provides a main function that makes two `CovidCase` objects.  You can compile this as follows:

`g++ -std=c++11 -o BasicCovidTest BasicCovidTest.cpp`  

...and if it compiles then run:

`./BasicCovidTest`

The code will not produce any output, as it's just constructing objects but doing nothing with them.  Let's work on that.

In `CovidCase.h`, implement an `operator<<` function for printing out CovidCase objects, so that the following code will work:

`CovidCase patientA(51.5115, -0.1160, "PatientA", 48, 49);`  
`cout << patientA << endl;`  

...and will produce output of the form:

`{latitude, longitude, "name", age, time}`

...in this case:

`{51.5115, -0.116, "PatientA", 48, 49}`  

To compile your code, `CovidPrintTest.cpp` provides a main function that makes two `CovidCase` objects, prints them out, and checks the output is correct.  You can compile this as follows:

`g++ -std=c++11 -o CovidPrintTest CovidPrintTest.cpp`  

...and if it compiles then run:

`./CovidPrintTest`

Finally: the distance between two Covid cases can be useful in tracking the spread of the virus.  Create a class function `distanceTo` in `CovidCase`, that will take another `CovidCase`, and return the distance to it in miles.  To compute this distance, use the [Haversine Formula](http://andrew.hedges.name/experiments/haversine/), which can be implemented using the following pseudo-code:

`dlon = lon2 - lon1`  
`dlat = lat2 - lat1`  
`a = pow((sin(dlat/2)), 2) + cos(lat1) * cos(lat2) * pow((sin(dlon/2)), 2)`  
`c = 2 * atan2( sqrt(a), sqrt(1-a) )`  
`distance = R * c (where R is the radius of the Earth)`  

Note that this pseudo-code assumes the latitude and longitude are in *radians*, whereas your class stores them in degrees, so you will need to convert them to radians first.  `cos`, `sin` and the other trignometric functions can be obtained by putting `#include <cmath>` at the top of `CovidCase.h`.  You should assume `R`, the radius of the earth in miles, is 3960.

To test your code, you can use CovidDistanceTest.cpp.  To compile to an executable `CovidDistanceTest`, run:

`g++ -std=c++11 -o CovidDistanceTest CovidDistanceTest.cpp`  

Note that this will only work once you have implemented the constructor and functions discussed above.


----

# Solving the Countdown Numbers Game

The Countdown numbers game is specified as follows:

- Players are given six integer numbers and a target integer number
- The six numbers should be combined using the mathematical operators + - * and / in a way that gets as close as possible to the target number.  Each number can only be used at most once, but it is okay not to use a number
- Getting the target number is worth 10 points, getting within 5 of the target is worth 7 points, getting within 10 is worth 5 points.

For instance if the numbers are (100,4,17,9,3,2) and the target is 37, then one way to do this is `(100 / (3 + 2)) + 17`, which would get the full 10 points.

## Evaluating numeric expressions [2 marks]

In `Countdown.h`, where it says `TODO: write code here`, implement a function `evaluateCountdown` that takes a `string` containing a mathematical expression written in [Reverse Polish Notation](https://en.wikipedia.org/wiki/Reverse_Polish_notation), and returns the result of evaluating the expression, as a `double`.

In reverse polish notation (RPN), operators are written *after* rather than between what they act upon.  For instance:

`3.0 4.0 +`

...is the same as the usual (infix) notation `(3.0 + 4.0)`.  Or:

`3.0 4.0 + 2.0 *`

...is the same as `(3.0 + 4.0) * 2`.  Or, coming back to the example countdown problem above, we can write `(100 / (3 + 2)) + 17` as:

`100 3 2 + / 17 +`

The advantage of RPN is that there is no need to worry about brackets and order of precedence.

RPN expressions can be evaluated by making a stack of numbers (using e.g. a `vector<double>`), splitting the input string by spaces into tokens, and working through it one token at a time:
 
 * If the token is a `+`, pop two numbers a and b off the stack, and push (b + a) onto the stack
 * If the token is a `-`, pop two numbers a and b off the stack, and push (b - a) onto the stack
 * If the token is a `*`, pop two numbers a and b off the stack, and push (b * a) onto the stack
 * If the token is a `/`, pop two numbers a and b off the stack, and push (b / a) onto the stack
 * Otherwise, the token is a number: push it onto the stack
 
After going through all the tokens, the answer is then on the top of the stack: return it.  Because you only need to be able to evaluate numeric expressions for 'countdown' problems, you can safely assume your stack does not ever need to be bigger than six elements -- the worst case is an expression with six numbers (all put on the stack) followed by five arithmetic operators, e.g. `100 4 17 9 3 2 + + + + +`.
 
To test your code, compile and run `TestEval.cpp`.  This will evaluate four expressions, and check they give the right answer.  As a reminder, to compile using g++ at the command line, with debugging turned on (`-g`):
 
 `g++ -std=c++11 -g -o TestEval TestEval.cpp`  
 
 ...then run `./TestEval`
 
Notes:

- `std::stod(s)` will convert the string `s` into a double, and return its value
- Your code must be in C++.  Do not use any of the 'C' language functions for working with strings (the functions defined on the following page <https://en.cppreference.com/w/cpp/header/cstring>, or those on the following page in the 'Numeric string conversion' section -- <https://en.cppreference.com/w/cpp/header/cstdlib>) 

## Solving countdown problems [4 marks]

The file `Countdown.h` contains a small class `CountdownSolution` for storing solutions to Countdown numbers problems: a string containg an RPN numeric expression, and an int containing the value that expression evaluates to.

In `Countdown.h` implement a function `solveCountdownProblem` that takes a `vector<int>` containing 6 numbers, and a target number; and returns a `CountdownSolution` object containing the solution to the problem that gets as close as possible to the target.

To test your code, compile and run `TestCountdown.cpp`.  This will evaluate some Countdown numbers games problems, and check they give the right answer.

Notes:
- The suggested solution approach is to write code that constructs different RPN expressions (using each number at most once, and using the available mathematical operators), evaluates them (using your `evaluateCountdown` function), and returns the best solution found (closest to target number)
- The given code `intToString` may be helpful for you as part of this, when building RPN expressions: it takes an int, and returns a string.
