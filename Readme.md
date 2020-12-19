# Ffo

**F**iles'n **F**olders as **O**bjects

Sometimes you want to have the path to a folder and at the same time know it's path relative to a base folder. Ffo provides this. Ffo is a wrapper around the .NET Folder/File/Path api. It should prevent you from doing File/Folder/Path manipulation and instead work with object references and properties.

# Usage
See the [ExampleTest](./Ffo.UnitTest/ExampleTest.cs) for a clear using example.

# Name
I tried just naming the objects `Folder` and `File`. However, this creates headachess when also using the `System.IO` namespace. I therefore used the prefix `Ff` for `File` and `Folder`: `Ffile` and `Ffolder` (`IFfolder`). This has the added benefit that typing the prefix `Ff` or `ff` will have your editor resolve often to these types.

The library name is an abbreviation of **F**ile **F**older **O**bjects: **Ffo**.

# Todo
- string casing

# Contributions
Feel free to open a PR or issue and provide any feedback.

# License
© Ruud Poutsma, [MIT](./LICENSE).