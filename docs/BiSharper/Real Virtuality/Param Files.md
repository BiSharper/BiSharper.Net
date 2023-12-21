## Syntactic Overview
The structure is setup to be relatively modular, there are two types of elements both adhering to the `IParamElement` marker interface being `IParamStatement` and `IParamValue`. Keep in mind that all though the API is modular, adding custom statements/values is not supported; That being said, custom more descriptive types can be delegated from the provided set of types.
### Value Types
All value types are slim `readonly struct` implementations holding memory for the type they hold and nothing else and are thus treated as value types by C#. This need for memory conservation is the main reason for `IParamElement` being a slim marker interface.
### Statement Types
Statements are split into two separate types, `IParamComputableStatement` and `IParamContextualStatement` both adhering to their own marker `IParamStatement`. At this time there are no statements which extend both statement types.

`IParamContext` is the type used to signify a class (or scope if you will), This type is a `IParamStatement` and contains a name, a list of parameters, a list of statements, and finally a list of other `IParamContext` contained within itself. This interface is implemented by the `ParamClassStatement` implementation aswell as the root of the structure `ParamDocument`

`IParamContextualStatement` statements are the normal type of statement; They just store a reference to the context their located within. 

//TODO Computable Statements
## Serialization Overview
The Source Generator tool automates the generation of C# code for types that adhere to user-defined interfaces marked with the `[ParamSerializable]` attribute. These interfaces signify a node in the param structure, and the generator processes them to create getters and setters. The generated code facilitates easy interaction with complex configuration files, promoting interchangeability.
### Code Generation
To participate in code generation, you must implement a user-defined interfaces marked with the `[ParamSerializable]` attribute, these interfaces are referred to as stubs. Properties within these stubs are annotated with the `[ParamProperty]` attribute, providing additional metadata for the generator. 

The `[ParamSerializable]` also allows you to provide some configuration such as:
- Whether the interface represents a Context or Value delegation. Default is Context; Values will be touched upon at a later time.
- //TODO

#### Notes
 - Although the bulk of the code is added to the implementations of the stub, the stub is still altered to inherit from `IParamSerializableContext` and is therefore partial
 - Notice the lace of setters in the example stub, currently mutability is not implemented and will be thought out at a later date
#### Example

```csharp
//This is our stub, in this example it simply represents a CfgPatches class from an RV Game
[ParamSerializable]
public partial interface ICfgPatch 
{
    [ParamProperty("units")]
    public IParamArray<ParamString> Units { get; }

    [ParamProperty("weapons")]
    public IParamArray<ParamString> Weapons { get; }

    [ParamProperty("requiredVersion")]
    public ParamFloat RequiredVersion { get; }

    [ParamProperty("requiredAddons")]
    public IParamArray<ParamString> RequiredAddons { get; }
}


//This is are implementation e.g where the stub is used and where code will be generated
public partial readonly record CfgPatchs {

	//Use properties and context info here :)

}

//This is the generated portion of the implementation
public readonly partial record struct CfgPatch  
{  
    public IParamArray<ParamString> Units { get =>  /*Getter Code Generated Here*/ }
    public IParamArray<ParamString> Weapons { get =>  /*Getter Code Generated Here*/ }
    public ParamFloat RequiredVersion { get =>  /*Getter Code Generated Here*/ } 
    public IParamArray<ParamString> RequiredAddons { get =>  /*Getter Code Generated Here*/ } 
}

```
