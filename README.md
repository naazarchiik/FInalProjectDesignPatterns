# LAb 6

## Programming Principles

### SOLID

**Single Responsibility Principle (SRP) :** Each class in the program is responsible for only one functionality. For example, the [Block](./ClassLibraryForTetris/Block.cs) class is responsible for the basic 
functionality of blocks in the game Tetris, such as [movement](./ClassLibraryForTetris/Block.cs#L44-L48), [rotation](./ClassLibraryForTetris/Block.cs#L27-L42), etc., while subclasses such as [IBlock](./ClassLibraryForTetris/Blocks/Iblock.cs), [JBlock](./ClassLibraryForTetris/Blocks/JBlock.cs) 
are only responsible for specific types of blocks and their specific parameters.

**Liskov Substitution Principle (LSP) :** Subclasses must be substitutable for their base classes. In our code, any [Block](./ClassLibraryForTetris/Block.cs) subclass, such as 
[IBlock](./ClassLibraryForTetris/Blocks/Iblock.cs), can be used where a Block base class is expected.

**Dependency Inversion Principle (DIP) :** High-level classes do not depend on low-level classes, but both types of classes depend on abstractions. In your code, the 
[Block](./ClassLibraryForTetris/Block.cs) class does not depend on concrete block types, but uses abstract methods and properties to communicate with them.

**Open/Closed Principle (OCP) :** The code is extensible, but closed to modifications. For example, it's easy to add new block types to the game without 
changing existing classes by using the [BlockQueue](./ClassLibraryForTetris/BlockQueue.cs#L23-L31) factory method.

**Interface Segregation Principle (ISP) :** Classes interact with each other through interfaces that describe their shared methods. For example, the [TilePosition
method in the Block](./ClassLibraryForTetris/Block.cs#L19-L25) class is used through the IEnumerable interface, which allows iterating over blocks.

### YAGNI (You Aren’t Gonna Need It)

The code does not contain anything superfluous and implements only the necessary functionality.

### KISS (Keep It Simple, Stupid)

The code is simple and clear. 
It performs its tasks without unnecessary complexity.

### DRY (Don’t Repeat Yourself)

The code does not contain repeated blocks of code. 
Each piece of information has a single, immutable representation in the code.
We can see it in every class in [directory](./ClassLibraryForTetris). 
For exampe: Handling rows in [GameGrid](./ClassLibraryForTetris/GameGrid.cs). The GameGrid class has [methods](./ClassLibraryForTetris/GameGrid.cs#L27-L92) for checking for 
fullness and removing full rows. These methods are used to remove rows after the blocks are placed on the board. This logic is also located in one place and is not repeated in other parts of the program.

### Program to Interfaces, not Implementations

The [GameState](./ClassLibraryForTetris/GameState.cs) class has no direct dependency on specific block implementations. It works with objects of type [Block](./ClassLibraryForTetris/Block.cs), which is 
an abstract class. This makes it easy to replace specific block types with any others that conform to this interface.


## Design Patterns

### Observer

This pattern is used to implement the mechanism of notification of changes in the object. In this code, watching for changes in the game happens from the outside through methods like 
[RotateBlockCW](./ClassLibraryForTetris/GameState.cs#L77-L85), [MoveBlockLeft](./ClassLibraryForTetris/GameState.cs#L97-L105), etc., which change the state of the game and the blocks.

### Strategy 

This template allows you to replace algorithms regardless of the client that uses them. In this case, the methods RotateBlockCW, RotateBlockCCW, MoveBlockLeft, MoveBlockRight, MoveBlockDown, DropBlock in the [GameState](./ClassLibraryForTetris/GameState.cs) class are used to control the movement of blocks in the game. These methods use the current block ([CurrentBlock](./ClassLibraryForTetris/GameState.cs#L3-L25)), which can be of any type that is a descendant of the [Block](./ClassLibraryForTetris/Block.cs) class, because they are implemented in the Block abstract class, and the block movement algorithm itself depends on the specific implementation of this method in the subclass.

### State 

This pattern is used implicitly in the [GameState](./ClassLibraryForTetris/GameState.cs) class. This class stores game state such as current block, score, etc. Each operation, such as 
[moving](./ClassLibraryForTetris/GameState.cs#L97-L105) a block or [rotating](./ClassLibraryForTetris/GameState.cs#L77-L85) it, changes the state of the game.

### Factory Method 

This pattern is used to create subclass objects based on some common interface. In this code, the [BlockQueue](./ClassLibraryForTetris/BlockQueue.cs) class uses a "factory method" in the 
[RandomBlock](./ClassLibraryForTetris/BlockQueue.cs#L23-L31) method to create objects of Block subclasses (such as IBlock, JBlock, etc.) that are added to the block queue.

### Template Method

In this case, the [Block](./ClassLibraryForTetris/Block.cs) class is an abstract base class that defines the general algorithm for how blocks work in the Tetris game. It contains methods such as [RotateCW, RotateCCW, Move, Reset](./ClassLibraryForTetris/Block.cs#L27-L57) and is used to handle rotating, moving and resetting blocks.
Each concrete type of block (such as [IBlock](./ClassLibraryForTetris/Blocks/Iblock.cs)) is a subclass of Block and overrides abstract methods and properties of the base class, such as [Tiles](./ClassLibraryForTetris/Blocks/Iblock.cs#L5-L11) and [StartOffset](./ClassLibraryForTetris/Blocks/Iblock.cs#L14). This allows each block type to have its own starting coordinates and describe its own unique tiles for each rotation.
