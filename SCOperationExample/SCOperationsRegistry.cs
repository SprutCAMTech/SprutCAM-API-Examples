// Do not change this namespace.
namespace SprutCAMExternalOperations; 

using System;
using System.Reflection;
using SprutTechnology.STLibraryTypes;

/// <summary>
/// SprutCAM operation class attribute.
/// The OperationID attribute must be unique. 
/// The OperationType attribute must match the interface GUID, implemented in the operation type class
/// (the class to which these attributes are applied). 
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class SCOperationAttribute: System.Attribute
{
    public string ClassID { get; set; } = Guid.Empty.ToString();
    public string InterfaceID { get; set; } = Guid.Empty.ToString();
}

struct OpDescriptorStruct
{
    public Guid InterfaceID;
    public Guid ClassID;
    public Type ClassType;

    public OpDescriptorStruct(Guid interfaceID, Guid classID, Type classType)
    {
        InterfaceID = interfaceID;
        ClassID = classID;
        ClassType = classType;
    }
}

public class SCOperationsRegistry : IST_ClassLibrary
{
    private List<OpDescriptorStruct> opList;

    public SCOperationsRegistry()
    {
        opList = new List<OpDescriptorStruct>();
        RegAllOperations();
    }

    private void RegAllOperations()
    {
        var a = Assembly.GetExecutingAssembly();
        foreach (TypeInfo c in a.DefinedTypes.Where(t => t.IsClass && t.IsPublic)) {
            var atr = c.GetCustomAttribute(typeof(SCOperationAttribute));
            if (atr != null) {
                var dsc = (SCOperationAttribute)atr;
                opList.Add(new OpDescriptorStruct(
                    new Guid(dsc.InterfaceID),
                    new Guid(dsc.ClassID),
                    c.AsType()
                ));
            }
        }
    }

    // IST_ClassLibrary
    public void RegisterClasses(IST_ClassRegistrator Reg)
    {
        if (Reg != null)
            foreach (var op in opList)
                Reg.RegisterCoClass(op.InterfaceID, op.ClassID);
    }

    public object CreateClassInstance(Guid ClassID)
    {
        if (!ClassID.Equals(Guid.Empty))
            foreach (var op in opList) 
                if (ClassID.Equals(op.ClassID))
                    return Activator.CreateInstance(op.ClassType)!;
        return null!;
    }
}

