# FolderAttribute

And NOW you can reference folders into Unity's Inspector.
That's what I've been waiting for years, until I decided to code it myself!
Enjoy :D

## Getting Started

For a quick import into an existing project, just get the UnityPackage.

The FolderAttribute folder is an empty project with only the plugin imported and some examples! :)

See the [Code Usage](#code-usage) for details on how to use it in your project.

## Prerequisites

There are absolutely no prerequisites to this plugin.

Everything comes into a few files (and most of them are used for demo).

## Code Usage

```csharp
[Folder]
public string materialsFolder; // A variable with [Folder] must be a string.

void Start()
{
	Material[] materials = materialsFolder.LoadFolder<Material>(); // Get the content of the folder!
}
```

## Screenshots

![Example 1](Screenshots/Example_1.PNG)

## Notes

* Last tested with [Unity 2018.2.1f1](https://unity3d.com/unity/whatsnew/unity-2018.2.1).

## Authors

* Special thanks to **[Jérémy Chopin](https://www.linkedin.com/in/jeremy-chopin/)**
* **[Arthur Cousseau](https://www.linkedin.com/in/arthurcousseau)**

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Going further...

We could do many things on top of this.

For example:

- Use the GUID of the folder in addition to its path, to keep track of the folder even if it's renamed or moved elsewhere.