using System;
using System.Linq;
using System.Collections.Generic;

using E_Lang.types;


namespace E_Lang.variables
{
  public abstract class EVariable
  {
    private static readonly Dictionary<string, Type> types = new Dictionary<string, Type> {
      { "int", typeof(EVInt) },
      { "double", typeof(EVDouble) },
      { "boolean", typeof(EVBoolean) },
      { "void", typeof(EVVoid) }
    };

    public static EVariable New(EType type)
    {
      string typeString = type.ToString();
      return New(typeString);
    }

    public static EVariable New(string typeString)
    {
      if (!types.ContainsKey(typeString)) throw new Exception("Variable type " + typeString + " is unknown");
      Type createType = types[typeString];
      return (EVariable)Activator.CreateInstance(createType);
    }

    public EType GetEType()
    {
      string type = types.Where((pair) => pair.Value == GetType()).First().Key;
      return new EType(type);
    }

    public static EType GetEType(Type t){
      string type = types.Where((pair) => pair.Value == t).First().Key;
      return new EType(type);
    }

    public virtual EVariable Assign(EVariable assign)
    {
      throw new Exception("Cannot assign to abstract class");
    }

    public EVariable Convert(string to){
      return Convert(new EType(to));
    }

    public virtual EVariable Convert(EType to)
    {
      return CannotConvert(to);
    }

    public virtual dynamic Get(){
      return "";
    }

    public virtual EVariable Set(dynamic setto){
     return this;
    }

    protected EVariable CannotConvert(EType type)
    {
      throw new Exception("Cannot convert " + type + " to " + GetEType());
    }
  }
}