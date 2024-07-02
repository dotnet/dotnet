// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the License.txt file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
#if NET9_0_OR_GREATER
using System.Runtime.InteropServices.Marshalling;
#endif

namespace Microsoft.DiaSymReader
{
    [ComVisible(false)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
#if NET9_0_OR_GREATER
    [GeneratedComInterface(StringMarshalling = StringMarshalling.Utf16)]
#else
    [ComImport]
#endif
    internal unsafe partial interface IMetadataImport
    {
        [PreserveSig]
        void CloseEnum(void* enumHandle);

        [PreserveSig]
        int CountEnum(void* enumHandle, out int count);

        [PreserveSig]
        int ResetEnum(void* enumHandle, int position);

        [PreserveSig]
        int EnumTypeDefs(
            ref void* enumHandle,
            int* typeDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumInterfaceImpls(
            ref void* enumHandle,
            int typeDef,
            int* interfaceImpls,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumTypeRefs(
            ref void* enumHandle,
            int* typeRefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int FindTypeDefByName(
            string name,
            int enclosingClass,
            out int typeDef); // must be specified

        [PreserveSig]
        int GetScopeProps(
            char* name,
            int bufferLength,
            int* nameLength,
            Guid* mvid);

        [PreserveSig]
        int GetModuleFromScope(
            out int moduleDef); // must be specified

        [PreserveSig]
        int GetTypeDefProps(
            int typeDef,
            char* qualifiedName,
            int qualifiedNameBufferLength,
            int* qualifiedNameLength,
            TypeAttributes* attributes,
            int* baseType);

        [PreserveSig]
        int GetInterfaceImplProps(
            int interfaceImpl,
            int* typeDef,
            int* interfaceDefRefSpec);

        [PreserveSig]
        int GetTypeRefProps(
            int typeRef,
            int* resolutionScope, // ModuleRef or AssemblyRef
            char* qualifiedName,
            int qualifiedNameBufferLength,
            int* qualifiedNameLength);

        /// <summary>
        /// Resolves type reference.
        /// </summary>
        /// <param name="typeRef">The TypeRef metadata token to return the referenced type information for.</param>
        /// <param name="scopeInterfaceId">The IID of the interface to return in scope. Typically, this would be IID_IMetaDataImport.</param>
        /// <param name="scope">An interface to the module scope in which the referenced type is defined.</param>
        /// <param name="typeDef">A pointer to a TypeDef token that represents the referenced type.</param>
        /// <remarks>
        /// TypeDefs define a type within a scope. TypeRefs refer to type-defs in other scopes
        /// and allow you to import a type from another scope. This function attempts to determine
        /// which type-def a type-ref points to.
        ///
        /// This resolve (type-ref, this cope) --> (type-def=*ptd, other scope=*ppIScope)
        ///
        /// However, this resolution requires knowing what modules have been loaded, which is not decided
        /// until runtime via loader / fusion policy. Thus this interface can't possibly be correct since
        /// it doesn't have that knowledge. Furthermore, when inspecting metadata from another process
        /// (such as a debugger inspecting the debuggee's metadata), this API can be truly misleading.
        ///
        /// This API usage should be avoided.
        /// </remarks>
        [PreserveSig]
        int ResolveTypeRef(
            int typeRef,
            ref Guid scopeInterfaceId,
            [MarshalAs(UnmanagedType.Interface)] out object scope, // must be specified
            out int typeDef); // must be specified

        [PreserveSig]
        int EnumMembers(
            ref void* enumHandle,
            int typeDef,
            int* memberDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumMembersWithName(
            ref void* enumHandle,
            int typeDef,
            string name,
            int* memberDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumMethods(
            ref void* enumHandle,
            int typeDef,
            int* methodDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumMethodsWithName(
            ref void* enumHandle,
            int typeDef,
            string name,
            int* methodDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumFields(
            ref void* enumHandle,
            int typeDef,
            int* fieldDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumFieldsWithName(
            ref void* enumHandle,
            int typeDef,
            string name,
            int* fieldDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumParams(
            ref void* enumHandle,
            int methodDef,
            int* paramDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumMemberRefs(
            ref void* enumHandle,
            int parentToken,
            int* memberRefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumMethodImpls(
            ref void* enumHandle,
            int typeDef,
            int* implementationTokens,
            int* declarationTokens,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumPermissionSets(
            ref void* enumHandle,
            int token, // TypeDef, MethodDef or Assembly
            uint action, // DeclarativeSecurityAction
            int* declSecurityTokens,
            int bufferLength,
            int* count);

        [PreserveSig]
        int FindMember(
            int typeDef,
            string name,
            byte* signature,
            int signatureLength,
            out int memberDef);

        [PreserveSig]
        int FindMethod(
            int typeDef,
            string name,
            byte* signature,
            int signatureLength,
            out int methodDef);

        [PreserveSig]
        int FindField(
            int typeDef,
            string name,
            byte* signature,
            int signatureLength,
            out int fieldDef);

        [PreserveSig]
        int FindMemberRef(
            int typeDef,
            string name,
            byte* signature,
            int signatureLength,
            out int memberRef);

        [PreserveSig]
        int GetMethodProps(
           int methodDef,
           int* declaringTypeDef,
           char* name,
           int nameBufferLength,
           int* nameLength,
           MethodAttributes* attributes,
           byte** signature, // returns pointer to signature blob
           int* signatureLength,
           int* relativeVirtualAddress,
           MethodImplAttributes* implAttributes);

        [PreserveSig]
        int GetMemberRefProps(
            int memberRef,
            int* declaringType, // TypeDef or TypeRef
            char* name,
            int nameBufferLength,
            int* nameLength,
            byte** signature, // returns pointer to signature blob
            int* signatureLength);

        [PreserveSig]
        int EnumProperties(
           ref void* enumHandle,
           int typeDef,
           int* properties,
           int bufferLength,
           int* count);

        [PreserveSig]
        uint EnumEvents(
           ref void* enumHandle,
           int typeDef,
           int* events,
           int bufferLength,
           int* count);

        [PreserveSig]
        int GetEventProps(
            int @event,
            int* declaringTypeDef,
            char* name,
            int nameBufferLength,
            int* nameLength,
            int* attributes,
            int* eventType,
            int* adderMethodDef,
            int* removerMethodDef,
            int* raiserMethodDef,
            int* otherMethodDefs,
            int otherMethodDefBufferLength,
            int* methodMethodDefsLength);

        [PreserveSig]
        int EnumMethodSemantics(
            ref void* enumHandle,
            int methodDef,
            int* eventsAndProperties,
            int bufferLength,
            int* count);

        [PreserveSig]
        int GetMethodSemantics(
            int methodDef,
            int eventOrProperty,
            int* semantics);

        [PreserveSig]
        int GetClassLayout(
            int typeDef,
            int* packSize,  // 1, 2, 4, 8, or 16
            MetadataImportFieldOffset* fieldOffsets,
            int bufferLength,
            int* count,
            int* typeSize);

        [PreserveSig]
        int GetFieldMarshal(
            int fieldDef,
            byte** nativeTypeSignature, // returns pointer to signature blob
            int* nativeTypeSignatureLength);

        [PreserveSig]
        int GetRVA(
            int methodDef,
            int* relativeVirtualAddress,
            int* implAttributes);

        [PreserveSig]
        int GetPermissionSetProps(
            int declSecurity,
            uint* action,
            byte** permissionBlob, // returns pointer to permission blob
            int* permissionBlobLength);

        [PreserveSig]
        int GetSigFromToken(
            int standaloneSignature,
            byte** signature, // returns pointer to signature blob
            int* signatureLength);

        [PreserveSig]
        int GetModuleRefProps(
            int moduleRef,
            char* name,
            int nameBufferLength,
            int* nameLength);

        [PreserveSig]
        int EnumModuleRefs(
            ref void* enumHandle,
            int* moduleRefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int GetTypeSpecFromToken(
            int typeSpec,
            byte** signature, // returns pointer to signature blob
            int* signatureLength);

        [PreserveSig]
        int GetNameFromToken(
            int token,
            byte* nameUTF8); // name on the #String heap

        [PreserveSig]
        int EnumUnresolvedMethods(
            ref void* enumHandle,
            int* methodDefs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int GetUserString(
            int userStringToken,
            char* buffer,
            int bufferLength,
            int* length);

        [PreserveSig]
        int GetPinvokeMap(
            int memberDef,  // FieldDef, MethodDef
            int* attributes,
            char* importName,
            int importNameBufferLength,
            int* importNameLength,
            int* moduleRef);

        [PreserveSig]
        int EnumSignatures(
            ref void* enumHandle,
            int* signatureTokens,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumTypeSpecs(
            ref void* enumHandle,
            int* typeSpecs,
            int bufferLength,
            int* count);

        [PreserveSig]
        int EnumUserStrings(
            ref void* enumHandle,
            int* userStrings,
            int bufferLength,
            int* count);

        [PreserveSig]
        int GetParamForMethodIndex(
            int methodDef,
            int sequenceNumber,
            out int parameterToken); // must be specified

        [PreserveSig]
        int EnumCustomAttributes(
            ref void* enumHandle,
            int parent,
            int attributeType,
            int* customAttributes,
            int bufferLength,
            int* count);

        [PreserveSig]
        int GetCustomAttributeProps(
            int customAttribute,
            int* parent,
            int* constructor,  // MethodDef, MethodRef
            byte** value, // returns pointer to a value blob
            int* valueLength);

        [PreserveSig]
        int FindTypeRef(
            int resolutionScope,
            string name,
            out int typeRef); // must be specified

        [PreserveSig]
        int GetMemberProps(
            int member, // Field or Property
            int* declaringTypeDef,
            char* name,
            int nameBufferLength,
            int* nameLength,
            int* attributes,
            byte** signature, // returns pointer to signature blob
            int* signatureLength,
            int* relativeVirtualAddress,
            int* implAttributes,
            int* constantType,
            byte** constantValue, // returns pointer to constant value blob
            int* constantValueLength);

        [PreserveSig]
        int GetFieldProps(
            int fieldDef,
            int* declaringTypeDef,
            char* name,
            int nameBufferLength,
            int* nameLength,
            int* attributes,
            byte** signature, // returns pointer to signature blob
            int* signatureLength,
            int* constantType,
            byte** constantValue, // returns pointer to constant value blob
            int* constantValueLength);

        [PreserveSig]
        int GetPropertyProps(
            int propertyDef,
            int* declaringTypeDef,
            char* name,
            int nameBufferLength,
            int* nameLength,
            int* attributes,
            byte** signature, // returns pointer to signature blob
            int* signatureLength,
            int* constantType,
            byte** constantValue, // returns pointer to constant value blob
            int* constantValueLength,
            int* setterMethodDef,
            int* getterMethodDef,
            int* outerMethodDefs,
            int outerMethodDefsBufferLength,
            int* otherMethodDefCount);

        [PreserveSig]
        int GetParamProps(
            int parameter,
            int* declaringMethodDef,
            int* sequenceNumber,
            char* name,
            int nameBufferLength,
            int* nameLength,
            int* attributes,
            int* constantType,
            byte** constantValue, // returns pointer to constant value blob
            int* constantValueLength);

        [PreserveSig]
        int GetCustomAttributeByName(
            int parent,
            string name,
            byte** value, // returns pointer to a value blob
            int* valueLength);

        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Bool)]
        bool IsValidToken(int token);

        [PreserveSig]
        int GetNestedClassProps(
            int nestedClass,
            out int enclosingClass);

        [PreserveSig]
        int GetNativeCallConvFromSig(
            byte* signature,
            int signatureLength,
            int* callingConvention);

        [PreserveSig]
        int IsGlobal(
            int token,
            [MarshalAs(UnmanagedType.Bool)] bool value);
    }
}
