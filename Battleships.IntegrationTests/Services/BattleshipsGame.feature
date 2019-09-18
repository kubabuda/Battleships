Feature: BattleshipsGame
	In order to play Battleships game
	As a user in command line
	I want to have grid displayed every round until all ships are hit


Scenario: New game without ships on grid
	Given New Battleships game 
	And No ships on grid
	When Game play starts
	Then Game is finished
	And Empty grid was displayed 1 times


Scenario: Playing single round and missing
	Given New Battleships game
	And Ships in folowing grid points
	| line | column |
	| 1    | 1      |
	And I play round with 'A4' guess
	Then Grid was displayed 1 times
	And Miss mark was displayed 1 times
	And Console was displaying
	"""
	  1 2 3 4 5 6 7 8 9 10
	A       x             |
	B                     |
	C                     |
	D                     |
	E                     |
	F                     |
	G                     |
	H                     |
	I                     |
	J                     |
	  - - - - - - - - - - 
	"""
	And Game is not finished


Scenario: Playing single round and hiting
	Given New Battleships game
	And Ships in folowing grid points
	| line | column |
	| 1    | 4      |
	And I play round with 'A4' guess
	Then Grid was displayed 1 times
	And Hit mark was displayed 1 times
	And Console was displaying
	"""
	  1 2 3 4 5 6 7 8 9 10
	A       *             |
	B                     |
	C                     |
	D                     |
	E                     |
	F                     |
	G                     |
	H                     |
	I                     |
	J                     |
	  - - - - - - - - - - 
	"""
	And Game is finished

Scenario: Playing entire game with ships on grid
	Given New Battleships game
	And Ships in folowing grid points
	| line | column |
	| 1    | 1      |
	| 1    | 2      |
	| 1    | 3      |
	| 3    | 4      |
	| 4    | 4      |
	And I type grid coordinates
	| value |
	| A1    |
	| A2    |
	| A3    |
	| C4    |
	| D4    |
	When Game play starts
	Then Game is finished
	And Grid was displayed 6 times


Scenario: Using the same guess twice
	Given New Battleships game 
	And Ships in folowing grid points
	| line | column |
	| 1    | 1      |
	| 1    | 2      |
	And I type grid coordinates
	| value |
	| A1    |
	| A1    |
	| A2    |
	When Game play starts
	Then Game displayed shot twice warning 1 times


Scenario: Shooting at invalid coordinates
	Given New Battleships game 
	And Ships in folowing grid points
	| line | column |
	| 1    | 1      |
	| 1    | 2      |
	And I type grid coordinates
	| value |
	| A11   |
	| K1    |
	| K11   |
	| A1    |
	| A2    |
	When Game play starts
	Then Game displayed invalid input warning 3 times


Scenario Outline: Checking if game is unfinised
	Given New Battleships game 
	And '<UnfinishedCellState>' in 1, 1
	Then Game is finished

	Examples: 
		| UnfinishedCellState |
		| Hit				  |
		| Miss				  |
		| Empty				  |


Scenario Outline: Checking if game is finised
	Given New Battleships game 
	And '<FinishedCellState>' in 1, 1
	Then Game is not finished

	Examples: 
		| FinishedCellState |
		| Ship				|