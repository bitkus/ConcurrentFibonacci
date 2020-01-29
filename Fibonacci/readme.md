### Run
Run as per usual, should work out of the box.

### Implementation
First of all, I used Binet's formula to find the nearest Fibonacci number. Otherwise if we go and calculate the Fibonacci numbers in order to check the nearest one, then we will have already found the numbers in the first place and this defeats the purpose.
The solution checks each number until it hits a Fibonacci and then does it to the other side of the number line. For very large numbers this might be slow, so I would look for another implementation here.

Secondly, the concurrent computation. In order to find the next number in sequence I need to use recursion, but that requires to pre-compute all the sequence, so instead I used Binet's formula for that particular number as well. Then I use the regular formula together with a shared state. Instead of computing the previous members I just wait for them to be calculated by other nodes.

The current implementation has limitations due to type precission and size of types. The max number the program can handle is 72723460248141L, since for larger numbers double starts to be not precise enough. Maybe other types could be used, but the Math library operates on doubles, so one could investigate that. Another limitaion - if you make the random seed large enough, then iteration when finding the nearest fibonacci will be long.

Lastly, I hardcoded the pseudo-random seed for node-1 to be a small one, since there is only a small probability to randomly generate a small number, and there are not many Fibonacci numbers between large numbers.

### Tests
I started with tests in mind by trying to create interfaces to be able to mock, but with timeframe in mind and the use of external modules forbidden, I did not write them, however, we can talk about them.