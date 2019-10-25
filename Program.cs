﻿using System;
using System.Text;
//using System.Array;
using LLVMSharp;

using System.IO;

public class Program
{
  private static void Main(string[] args)
  {
    var parsed = Parser.ParseExpression("1");
    Console.WriteLine(parsed);

    //LLVM.ConstInlineAsm(LLVM.Int32Type(c), "bswap $0", "=r,r", !truu, !truu);

    LLVMModuleRef module = LLVM.ModuleCreateWithName("Main");

    LLVMTypeRef charpointer = LLVM.PointerType(LLVM.Int8Type(), 0);
    LLVMTypeRef[] putsparams = { charpointer };
    LLVMTypeRef putstype = LLVM.FunctionType(LLVM.Int64Type(), putsparams, false);

    LLVMValueRef putsfunc = LLVM.AddFunction(module, "printf", putstype);

    //LLVM.AddAttributeAtIndex(putsfunc, 0, LLVMSharp.LLVM.attribute)

    LLVMTypeRef[] param_types = { };
    LLVMTypeRef ret_type = LLVM.FunctionType(LLVM.Int8Type(), param_types, false);

    LLVMValueRef sum = LLVM.AddFunction(module, "main", ret_type);
    LLVMBasicBlockRef entry = LLVM.AppendBasicBlock(sum, "entry");
    LLVMBuilderRef builder = LLVM.CreateBuilder();
    LLVM.PositionBuilderAtEnd(builder, entry);


    LLVMValueRef ep = LLVM.ConstInt(LLVM.Int8Type(), 69, false);
    LLVMValueRef tp = LLVM.ConstInt(LLVM.Int8Type(), 0, false);

    LLVMValueRef arr = LLVM.ConstArray(LLVM.Int8Type(), new LLVMValueRef[] { ep, tp });
    LLVMValueRef pointer = LLVM.BuildArrayAlloca(builder, LLVM.Int8Type(), arr, "tjiehf");
    
    //LLVMValueRef pointer = LLVM.BuildAlloca(builder, LLVM.ArrayType(LLVM.Int8Type(), 2), "charp");
    LLVM.BuildStore(builder, arr, pointer);
    LLVMValueRef solved = LLVM.ConstInt(LLVM.Int64Type(), 69, false);//parsed.Solve(builder);
    LLVMValueRef printOp = LLVM.ConstInt(LLVM.Int64Type(), 1, false);

    LLVMValueRef holder = LLVM.BuildAlloca(builder, LLVM.Int64Type(), "holder");
    LLVM.SetAlignment(holder, 4);
    LLVMValueRef store = LLVM.BuildStore(builder, solved, holder);
    LLVM.SetAlignment(store, 4);

    LLVMTypeRef[] asmparams = { LLVM.Int64Type(), LLVM.Int64Type(), LLVM.TypeOf(holder), LLVM.Int64Type() };
    LLVMTypeRef asmfuncthing = LLVM.FunctionType(LLVM.Int64Type(), asmparams, false);
    LLVMValueRef asmthing = LLVM.ConstInlineAsm(asmfuncthing, "syscall", "=r,{rax},{rdi},{rsi},{rdx},~{dirflag},~{fpsr},~{flags}", true, false);

    LLVMValueRef[] argss = { printOp, printOp, holder, printOp };
    LLVMValueRef called = LLVM.BuildCall(builder, asmthing, argss, "thingy");

    LLVM.BuildRet(builder, called);

    LLVM.BuildCall(builder, putsfunc, new LLVMValueRef[] { pointer }, "result");

    LLVMValueRef ret = LLVM.ConstInt(LLVM.Int8Type(), 0, false);
    LLVM.BuildRet(builder, ret);

    if (LLVM.VerifyModule(module, LLVMVerifierFailureAction.LLVMPrintMessageAction, out var error) != falseb)
    {
      Console.WriteLine($"Error: {error}");
    }


    LLVM.DumpModule(module);
    
    LLVM.WriteBitcodeToFile(module, "./out/bitcode.bc");
    


  }

}

