using LLVMSharp;
using E_Lang.llvm;

namespace E_Lang.variables
{
  public class EVBoolean : EVariable
  {
    private LLVMValueRef value; // INT1

    public EVBoolean(LLVMHolder holder) : base(holder) { }

    public override LLVMTypeRef GetTypeRef()
    {
      return LLVM.Int1Type();
    }

    public override EVariable Assign(EVariable assign)
    {
      EVBoolean converted = (EVBoolean)assign.Convert(GetEType());
      value = converted.Get();
      return this;
    }

    public override LLVMValueRef Get()
    {
      if (value.IsUndef()) IsUndefined();
      return value;
    }

    public override EVariable Set(dynamic setTo)
    {

      if (setTo.GetType() == typeof(bool))
      {
        bool parsedValue = setTo;
        value = LLVM.ConstInt(GetTypeRef(), (ulong)(parsedValue ? 1 : 0), false);
        return this;

      }
      else if (setTo.GetType() == typeof(LLVMValueRef))
      {
        LLVMValueRef parsedValue = setTo;
        if (LLVM.TypeOf(parsedValue).Equals(GetTypeRef()))
        {
          value = parsedValue;
          return this;
        }
      }

      throw new ELangException("EVBoolean did not receive a bool");
    }
  }
}