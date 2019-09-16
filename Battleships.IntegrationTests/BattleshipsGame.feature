Feature: BattleshipsGame
	In order to play Battleships game
	As a user in command line
	I want to have grid displayed every round until all ships are hit


Scenario: New game without ships on grid
	Given New Battleships game 
	And No ships on grid
	When Game play starts
	Then Game is finished
	And Empty grid was displayed
	And Grid was displayed 1 times


Scenario: New game with ships on grid
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
	| A1     |
	| A2     |
	| A3     |
	| C4     |
	| D4     |
	When Game play starts
	Then Game is finished
	And Grid was displayed 6 times
