/* ---------------------------------------------------------------------    
 * Copyright:
 * IXinWu Technology Co., Ltd. All rights reserved. 
 * 
 * DynamicTypeBuilder Description:
 * 动态创建类型
 *
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2019/1/23 14:11:04 hulei@ixinwu.com
 *
 * ------------------------------------------------------------------------------*/

using NGP.Framework.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace NGP.Foundation.Service.Analysis
{
    /// <summary>
    /// 动态创建类型
    /// </summary>
    public static class DynamicTypeBuilder
    {
        /// <summary>
        /// 模型构造器
        /// </summary>
        private static readonly ModuleBuilder _moduleBuilder;

        /// <summary>
        /// ctor
        /// </summary>
        static DynamicTypeBuilder()
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(GuidExtend.NewGuid()),
            AssemblyBuilderAccess.Run);
            _moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicFormModule");
        }

        /// <summary>
        /// 创建类型
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static Type CompileType(this IEnumerable<DynamicGenerateFieldInfo> fields)
        {
            string className = "D_" + GuidExtend.NewGuid();
            var typeBuilder = CreateTypeBuilder(className);

            foreach (var fieldItem in fields)
            {
                CreateProperty(typeBuilder, fieldItem);
            }
            var objectType = typeBuilder.CreateType();
            return objectType;
        }

        /// <summary>
        /// 获取类型构造器
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static TypeBuilder CreateTypeBuilder(string className)
        {
            var typeBuilder = _moduleBuilder.DefineType(className
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            //// 添加DataContract特性
            //var dataContractBuilder = new CustomAttributeBuilder(typeof(DataContractAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
            //typeBuilder.SetCustomAttribute(dataContractBuilder);
            return typeBuilder;
        }

        /// <summary>
        /// 创建动态属性
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyItem"></param>
        /// <returns></returns>
        private static void CreateProperty(TypeBuilder typeBuilder, DynamicGenerateFieldInfo propertyItem)
        {
            var propertyName = propertyItem.FieldKey;
            var propertyType = propertyItem.CodeType;

            // 创建字段
            var backingField = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            // 创建属性
            var property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, Type.EmptyTypes);
            var serializedLabel = propertyItem.SerializedLabel;
            if (string.IsNullOrWhiteSpace(serializedLabel))
            {
                serializedLabel = propertyItem.FieldKey;
            }

            // 添加序列化标记
            //var dataMemberAttribute = new CustomAttributeBuilder(
            //    typeof(DataMemberAttribute).GetConstructor(Type.EmptyTypes),
            //    new object[0],
            //    new PropertyInfo[] { typeof(DataMemberAttribute).GetProperty("Name") },
            //    new object[] { serializedLabel });
            //property.SetCustomAttribute(dataMemberAttribute);

            var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            var getMethod = typeBuilder.DefineMethod("get_" + propertyName, getSetAttr, propertyType, Type.EmptyTypes);
            var setMethod = typeBuilder.DefineMethod("set_" + propertyName, getSetAttr, null, new Type[] { propertyType });

            var getIL = getMethod.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, backingField);
            getIL.Emit(OpCodes.Ret);

            var setIL = setMethod.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, backingField);
            setIL.Emit(OpCodes.Ret);

            property.SetSetMethod(setMethod);
            property.SetGetMethod(getMethod);
        }
    }
}
