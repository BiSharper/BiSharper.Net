
## Syntactic Overview



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
