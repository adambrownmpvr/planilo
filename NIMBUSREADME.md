This file outlines the changes made to the BASE planilo asset.

### [12/23/2021] ADAM BROWN ###

# Changes
- BTGraph: 
	- Added override to Copy() method to support Planilo behaviors in xNode
	- Added renaming of nodes in Copy() method
	- Set Root only sets node.isRoot to false if it is a node inside the graph
	- Set Root only sets node to root if node is in graph


### [12/17/2021] ADAM BROWN ###

# Changes
- BTConnection: Added null check around empty connected node in Init()
- BTDecoratorNodeEditor: Added a pre-processor #if to draw the child node port, to avoid a double draw

### [12/08/2021] ADAM BROWN ###

# Changes
- BTConnection:
	- Made Run() and Init() virtual methods
	- portName and isConnected properties updated for clarity
	- Made several fields protected for use from derived classes
- BTCompositeNode: Removed obselete method calls and replaced with new ones.

### [12/06/2021] ADAM BROWN ###

# Additions 
- Added assembly definitions in Editor folders