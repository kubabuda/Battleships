Feature: Play
	In order play Battleships game
	As a commandline player
	I want to have grid displayed every round until all ships are hit


Scenario: New game with ships on grid
	Given New Battleships game 
	And Ships in folowing grid points	
	When Game plays single round
	Then Game is finished



#@battleshipsTag1
#Scenario: New game with ships on grid
#	Given New Battleships game 
#	And Ships in folowing grid points
#	| Line | Column |
#	| 1    | 1      |
#	| 1    | 2      |
#	| 1    | 3      |
#	| 3    | 4      |
#	| 4    | 4      |
#	When I type grid coordinates
#	| values |
#	| A1     |
#	| A2     |
#	| A3     |
#	| C4     |
#	| D4     |
#	Then the game is finished
