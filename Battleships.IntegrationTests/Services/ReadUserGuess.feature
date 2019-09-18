Feature: ReadUserGuess
	In order to read user input
	As a someone playing Battleships
	I want to get user inpud translated to grid coordinates, or exception


Scenario Outline: Reading valid guess
	Given New reader instance
	When I read '<Guess>'
	Then '<Line>' '<Column>' are read

	Examples: 
		| Guess | Line | Column |
	    | B2  | 1 | 1 |
	    | A1  | 0 | 0 |
	    | A10 | 0 | 9 |
	    | J1  | 9 | 0 |
	    | J10 | 9 | 9 |

  
Scenario Outline: Reading invalid guess
	Given New reader instance
	When I read '<Guess>'
	Then InvalidInputException is thrown by reader

	Examples: 
		| Guess  |
	    | B22    |
		| AA1    |
	    | ASAP10 |
	    | K1     |
		| FOO    |
	    | A0     |
		| J0     |
	    | A      |
		| J      |