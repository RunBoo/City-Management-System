# Further C++ practice

These questions give you more practice working with the basic building blocks of C++: vectors, functions, and classes; and additionally, pointers.

Where you are to write functions, ensure you use constness and references where appropriate, to avoid needlessly copying objects, and to help ensure you have written correct code.

Make sure you don't commit any compiled code to your GitHub repository; or if you choose to use an IDE, any large project directories created by your IDE.  You can make these on your machine, but don't `commit` or `add` them to your repository -- this isn't what git is designed for.

----

# Covid cases class extensions [2 marks]

(Copy your `CovidCase.h` file across from Part1 into the Part2 directory before attempting this part of the work.)

In Part1, you wrote the CovidCase class.  Let's add some more functionality to it.

First, implement an equality operator -- `operator==` -- that allows you to determine whether two `CovidCase` objects are the same -- i.e. have the same values for all member variables.

Second, implement 'getter' functions for accessing the values stored in the object.  These getters should be named `getLatitude`, `getLongitude`, `getName`, `getAge`, `getTime` for the respective member variables.

Third, add another constructor to `CovidCase` that takes a string, and uses this to initialise the member variables of the object.  You can assume the input string is of the form:

`{latitude, longitude, "name", age, time}`

so e.g. making a CovidCase object as follows:

`CovidCase patientA1("{51.5115, -0.116, \"PatientA\", 48, 49}");`  

...should yield a CovidCase object that is identical to:

`CovidCase patientA2(51.5115, -0.1160, "PatientA", 48, 49);`  

 To test your code, compile and run `TestCovidExt.cpp`.  This will run some tests that use the functionality above.  As a reminder, to compile using g++ at the command line, with debugging turned on (`-g`):
 
 `g++ -std=c++11 -g -o TestCovidExt TestCovidExt.cpp`  
 
 ...then run `./TestCovidExt`
 
Notes:

- When extracting the name from the string passed to the constructor, you can assume it does not contain the double-quotes " character, but don't make any other assumptions, e.g. that names only contain letters of the alphabet -- in any realistic system, the name would be some sort of anonymous patient identifier, so could be pretty much anything.
- `std::stoi(s)` and `std::stod(s)` will convert the string `s` into a double or int (respectively), and returning its value
- Your code must be in C++.  Do not use any of the 'C' language functions for working with strings (the functions defined on the following page <https://en.cppreference.com/w/cpp/header/cstring>, or those on the following page in the 'Numeric string conversion' section -- <https://en.cppreference.com/w/cpp/header/cstdlib>) 

----

# Covid Case Analysis


## Covid case map

In `CovidCaseMap.h` -- not in `CovidCase.h` -- make a class `CovidCaseMap`. To begin with, it needs to have:

- a private member variable that stores a vector of `CovidCase`s
- a public function `addCase` for adding a CovidCase to the end of this vector

## Active cases over time [2 marks]

So far, `CovidCaseMap` contains a vector of confirmed covid cases -- all positive tests recorded since time 0.  If we assume patients are only 'active cases' for a fixed self-isolation period (e.g. 14 days) we can work out how the number of active changes over time.

First implement a small class `TimeAndCaseData` that can be used to record the number of active cases at some point in time.   It needs to have two getters: `getTime` and `getNumberOfCases`.

Next, write a function `getCasesOverTime` in the `CovidCaseMap` class that returns a vector of these `TimeAndCaseData` objects, as determined from the cases added to the Covid case map using `addCase`.  It should take as its parameter an `int` of how many time units (hours) we are assuming a case is active for, e.g. 336 if we are assuming 14 days, 120 for 5 days, etc.

As an example:

`CovidCaseMap m;`  
`m.addCase(CovidCase(51.75186263338416,-0.3356807902,"Alice",21,5));`  
`m.addCase(CovidCase(51.75293227875113,-0.3363451467,"Bob",19,73));`  
`m.addCase(CovidCase(51.750621522352574,-0.3373409116,"Charlie",42,142));`  
`vector<TimeAndCaseData> active = m.getCasesOverTime(120);`  

...should return a vector where the elements are:
- time = 0, number of cases = 0 (no cases start this soon)
- time = 5, number of cases = 1 (Alice has a positive test)
- time = 73, number of cases = 2 (Bob has a positive test)
- time = 125, number of cases = 1 (Alice's positive test was 120 hours ago, so no longer an active case)
- time = 142, number of cases = 2 (Charlie has a positive test)
- time = 193, number of cases = 1 (Bob's positive test was 120 hours ago, so no longer an active case)
- time = 262, number of cases = 0 (Charlie's positive test was 120 hours ago, so no longer an active case)


Notes:
- The active cases should always begin with time 0
- You can assume that no time value is less than 0, but don't assume any upper bound.

To test your implementation, use `CovidCaseMapTest1.cpp`.

## Covid support visit route planning [2 marks]

In many countries, people who are self-isolating due to positive COVID tests have daily support visits to check how they are, and to drop off food or other supplies.  For this question, assume the person visiting covid patients starts at some location, visits all the active cases, then returns to their start location.

In general, this is a Travelling Salesman Problem (TSP) -- so finding the best possible (shortest) route to follow is hard.  But we can find an 'okay' route using a greedy algorithm:

- Start at the start location
- Then go to the nearest patient who hasn't been visited yet
- Then go to the nearest patient who hasn't been visited yet
- etc. etc. until all patients have been visited
- Then return to the start location

In `CovidCaseMap`, write a function `supportVisitGreedyTSP` that takes four parameters:
- A latitude, and longitude -- this is the start position
- A time, and length of the self-isolation period -- this is used to determine who are the active cases that need to be visited

...and returns the length in miles of a route that starts at the given latitude,longitude; visits all active cases at the given time (using the greedy TSP algorithm); then returns back to the given latitude,longitude.

To test your implementation, use `CovidCaseMapTest2.cpp`.

# String construction [3 marks]

This part of the assignment considers a string construction problem defined as follows:

- You are given as input a target string
- Starting with an empty string, you add characters to it, until your new string is same as the target.  You have two options to add characters to a string:
  - You can append an arbitrary character to your new string, with cost x
  - You can clone any substring of your new string so far, and append it to the end of your new string, with cost y
- For a given target, append cost x, and clone cost y, we want to know what the *cheapest cost* is of building the target string

For some simple examples:

- Target "aa", append cost 1, clone cost 2: the cheapest cost is 2:
  - Start with an empty string, ""
  - Append 'a' (cost 1), giving the string "a"
  - Append 'a' (cost 1), giving the string "aa"

- Target "aaaa", append cost 2, clone cost 3: the cheapest cost is 7:
  - Start with an empty string, ""
  - Append 'a' (cost 2), giving the string "a"
  - Append 'a' (cost 2), giving the string "aa"
  - Clone "aa" (cost 3), giving the string "aaaa"

- Target "xzxpzxzxpq", append cost 10, clone cost 11: the cheapest cost is 71:
  - Start with an empty string, ""
  - Append 'x' (cost 10): "x"
  - Append 'z' (cost 10): "xz" 
  - Append 'x' (cost 10): "xzx"
  - Append 'p' (cost 10): "xzxp"
  - Append 'z' (cost 10): "xzxpz"
  - Clone "xzxp" (cost 11): "xzxpzxzxp"
  - Append 'q' (cost 10) : "xzxpzxzxpq"

In the file `StringConstruction.h` write a function `stringConstruction` that takes the target string, the clone cost, and the append cost (in that order), and returns the cheapest way of making the target string.  It doesn't need to return *how* to do it, just the cost.

To test your code, TestStringCons.cpp contains some test cases.  To compile and run at the command line:

`g++ -std=c++11 -o TestSC TestStringCons.cpp`  
`./TestSC`

Note: when your work is marked, it will be tested with target strings of a range of sizes.  To get full marks, it will need to work for the largest of these target strings, which is 30,000 characters.  Your code will be ran on a modest desktop PC, and allowed 2 seconds.

