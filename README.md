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
