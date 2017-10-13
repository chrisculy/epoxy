# epoxy

Epoxy is a tool for generating bindings for C++ code to other programming languages.

Support: C# (P/Invoke), Java (JNI), JavaScript (Emscripten) with optional TypeScript definition files.

# Design

## Usage

Epoxy is run using a JSON configuration file like so

```epoxy project.json```

The JSON configuration file has the following options:

* `languages` :
  * an array of the languages to generate bindings for; supported values are `c#`, `java`, `javascript`, and `typescript`
* `explicit` :
  * when `true`, Epoxy will only generate bindings for symbols that are explicited labeled with `@bind` (and not labeled with `@bind{exclude}`) in their Doxygen documentation
  * when `false`, Epoxy will generate bindings for any symbol that does not contain the `@bind{exclude}` Doxygen tag, even if that symbol is not documented.
* `doxygenXmlDirectory`
  * the path to the directory where the Doxygen-generated XML is located

## Specifying Bindings

Bindings can be specified through Doxygen documentation on the APIs that you want exposed to other languages. Alternatively, you can set up your configuration to expose everything by default and only exclude specific APIs.
