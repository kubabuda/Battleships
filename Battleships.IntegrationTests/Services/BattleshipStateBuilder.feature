Feature: BattleshipStateBuilder
	In order to get Battleship game state
	As a someone playing Battleships
	I want to get proper game states


Scenario Outline: Place all ship on new game board
	Given New state builder instance
	And I have entered '<Ships>' into configuration
	When I generate new game state
	Then All '<Ships>' are placed on board

	Examples: 
		| Ships |
		| 2,3,4 |
		| 4,2,3 |


Scenario Outline: Next state with invalid guess
	Given New state builder instance
	When I generate next state with '<InvalidGuess>'
	Then InvalidInputException is thrown
	
	Examples: 
		| InvalidGuess |
		| A11          |
		| AA1          |
		| M1	       |
		| MAAAA1       |
		| FooBar       |
		|   |


Scenario Outline: Shooting new spot
	Given New state builder instance
	And I have previous game state
	And '<State>' in prev state '<Line>' '<Column>'
	When I generate next state with '<Guess>'
	Then '<Line>' '<Column>' is in '<NextState>'
	
	Examples: 
		| State | Line | Column | Guess | NextState |
		| Empty | 1    | 1      | A1    | Miss      |
		| Ship  | 1    | 1      | A1    | Hit      |


Scenario Outline: Shooting the same spot twice
	Given New state builder instance
	And I have previous game state
	And '<State>' in prev state '<Line>' '<Column>'
	When I generate next state with '<InvalidGuess>'
	Then CellRepetitionException is thrown
	
	Examples: 
		| State | Line | Column | InvalidGuess |
		| Hit   | 1    | 1      |	A1         |
		| Miss  | 1    | 1      |	A1         |
