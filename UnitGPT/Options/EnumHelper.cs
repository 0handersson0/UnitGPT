using System.Reflection.Emit;
using System.Reflection;

namespace UnitGPT.Services.CodeGeneration
{
    internal class EnumHelper
    {
        internal static Type CreateEnumType(string enumName, params string[] enumValues)
        {
            // Create a dynamic assembly
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            // Create a dynamic module in the assembly
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // Create a dynamic enum type
            EnumBuilder enumBuilder = moduleBuilder.DefineEnum(enumName, TypeAttributes.Public, typeof(int));

            // Add enum values
            for (int i = 0; i < enumValues.Length; i++)
            {
                enumBuilder.DefineLiteral(enumValues[i], i);
            }

            // Create the enum type
            Type enumType = enumBuilder.CreateType();

            return enumType;
        }
    }
}
