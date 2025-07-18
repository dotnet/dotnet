// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// ==++==
//

//
// ==--==

// ===========================================================================
// STRIKE.CPP
// ===========================================================================
//
// History:
//   09/07/99  Microsoft  Created
//
//************************************************************************************************
// SOS is the native debugging extension designed to support investigations into CLR (mis-)
// behavior by both users of the runtime as well as the code owners. It allows inspection of
// internal structures, of user visible entities, as well as execution control.
//
// This is the main SOS file hosting the implementation of all the exposed commands. A good
// starting point for understanding the semantics of these commands is the sosdocs.txt file.
//
// #CrossPlatformSOS
// SOS currently supports cross platform debugging from x86 to ARM. It takes a different approach
// from the DAC: whereas for the DAC we produce one binary for each supported host-target
// architecture pair, for SOS we produce only one binary for each host architecture; this one
// binary contains code for all supported target architectures. In doing this SOS depends on two
// assumptions:
//   . that the debugger will load the appropriate DAC, and
//   . that the host and target word size is identical.
// The second assumption is identical to the DAC assumption, and there will be considerable effort
// required (in the EE, the DAC, and SOS) if we ever need to remove it.
//
// In an ideal world SOS would be able to retrieve all platform specific information it needs
// either from the debugger or from DAC. However, SOS has taken some subtle and not so subtle
// dependencies on the CLR and the target platform.
// To resolve this problem, SOS now abstracts the target behind the IMachine interface, and uses
// calls on IMachine to take target-specific actions. It implements X86Machine, ARMMachine, and
// AMD64Machine. An instance of these exists in each appropriate host (e.g. the X86 version of SOS
// contains instances of X86Machine and ARMMachine, the ARM version contains an instance of
// ARMMachine, and the AMD64 version contains an instance of AMD64Machine). The code included in
// each version if determined by the SosTarget*** MSBuild symbols, and SOS_TARGET_*** conditional
// compilation symbols (as specified in sos.targets).
//
// Most of the target specific code is hosted in disasm.h/.cpp, and disasmX86.cpp, disasmARM.cpp.
// Some code currently under _TARGET_*** ifdefs may need to be reviewed/revisited.
//
// Issues:
// The one-binary-per-host decision does have some drawbacks:
//   . Currently including system headers or even CLR headers will only account for the host
//     target, IOW, when building the X86 version of SOS, CONTEXT will refer to the X86 CONTEXT
//     structure, so we need to be careful when debugging ARM targets. The CONTEXT issue is
//     partially resolved by CROSS_PLATFORM_CONTEXT (there is still a need to be very careful
//     when handling arrays of CONTEXTs - see _EFN_StackTrace for details on this).
//   . For larger includes (e.g. GC info), we will need to include files in specific namespaces,
//     with specific _TARGET_*** macros defined in order to avoid name clashes and ensure correct
//     system types are used.
// -----------------------------------------------------------------------------------------------

#define DO_NOT_DISABLE_RAND //this is a standalone tool, and can use rand()

#include <windows.h>
#include <winver.h>
#include <winternl.h>
#include <psapi.h>
#include <inttypes.h>
#ifndef FEATURE_PAL
#include <list>
#endif // !FEATURE_PAL
#include <wchar.h>
#include "platformspecific.h"

#define NOEXTAPI
#define KDEXT_64BIT
#include <wdbgexts.h>
#undef DECLARE_API
#undef StackTrace

#include <dbghelp.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stddef.h>
#include <stdexcept>
#include <deque>
#include <set>
#include <vector>
#include <map>
#include <tuple>
#include <memory>
#include <functional>
#include <algorithm>
#include <iostream>
#include <sstream>
#ifdef HOST_UNIX
#include <dlfcn.h>
#endif

#include "strike.h"
#include "sos.h"

#ifndef STRESS_LOG
#define STRESS_LOG
#endif // STRESS_LOG
#define STRESS_LOG_READONLY
#include "stresslog.h"

#include "util.h"

#include "corhdr.h"
#include "cor.h"
#include "cordebug.h"
#include "dacprivate.h"
#include "corexcep.h"
#include <dumpcommon.h>

#define  CORHANDLE_MASK 0x1
#define SWITCHED_OUT_FIBER_OSID 0xbaadf00d;

#define DEFINE_EXT_GLOBALS

#include "data.h"
#include "disasm.h"

#include "predeftlsslot.h"
#include "hillclimbing.h"
#include "sos_md.h"

#ifndef FEATURE_PAL

#include "ExpressionNode.h"
#include "WatchCmd.h"
#include "tls.h"
#include "clrma/managedanalysis.h"

typedef struct _VM_COUNTERS {
    SIZE_T PeakVirtualSize;
    SIZE_T VirtualSize;
    ULONG PageFaultCount;
    SIZE_T PeakWorkingSetSize;
    SIZE_T WorkingSetSize;
    SIZE_T QuotaPeakPagedPoolUsage;
    SIZE_T QuotaPagedPoolUsage;
    SIZE_T QuotaPeakNonPagedPoolUsage;
    SIZE_T QuotaNonPagedPoolUsage;
    SIZE_T PagefileUsage;
    SIZE_T PeakPagefileUsage;
} VM_COUNTERS;
typedef VM_COUNTERS *PVM_COUNTERS;

const PROCESSINFOCLASS ProcessVmCounters = static_cast<PROCESSINFOCLASS>(3);

#endif // !FEATURE_PAL

// Max number of methods that !dumpmodule -prof will print
const UINT kcMaxMethodDescsForProfiler = 100;

BOOL ControlC = FALSE;
WCHAR g_mdName[mdNameLen];

#ifndef FEATURE_PAL
HMODULE g_hInstance = NULL;
#endif // !FEATURE_PAL

#ifdef _MSC_VER
#pragma warning(disable:4244)   // conversion from 'unsigned int' to 'unsigned short', possible loss of data
#pragma warning(disable:4189)   // local variable is initialized but not referenced
#endif

#ifdef FEATURE_PAL
#define SOSPrefix ""
#else
extern const char* g_sosPrefix;
#define SOSPrefix g_sosPrefix
#endif

#if defined _X86_ && !defined FEATURE_PAL
// disable FPO for X86 builds
#pragma optimize("y", off)
#endif

#undef assert

#ifdef _MSC_VER
#pragma warning(default:4244)
#pragma warning(default:4189)
#endif

#ifndef FEATURE_PAL
#include "ntinfo.h"
#endif // FEATURE_PAL

#undef IfFailRet
#define IfFailRet(EXPR) do { Status = (EXPR); if(FAILED(Status)) { return (Status); } } while (0)

#ifdef FEATURE_PAL

#define MINIDUMP_NOT_SUPPORTED()
#define ONLY_SUPPORTED_ON_WINDOWS_TARGET()

#else // !FEATURE_PAL

#define MINIDUMP_NOT_SUPPORTED()   \
    if (IsMiniDumpFile())      \
    {                          \
        ExtOut("This command is not supported in a minidump without full memory\n"); \
        ExtOut("To try the command anyway, run !MinidumpMode 0\n"); \
        return Status;         \
    }

#define ONLY_SUPPORTED_ON_WINDOWS_TARGET()                                    \
    if (!IsWindowsTarget())                                                   \
    {                                                                         \
        ExtOut("This command is only supported for Windows targets\n");       \
        return Status;                                                        \
    }

#include "safemath.h"

DECLARE_API (MinidumpMode)
{
    INIT_API();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();
    DWORD_PTR Value=0;

    CMDValue arg[] =
    {   // vptr, type;
        {&Value, COHEX}
    };

    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg == 0)
    {
        // Print status of current mode
       ExtOut("Current mode: %s - unsafe minidump commands are %s.\n",
               g_InMinidumpSafeMode ? "1" : "0",
               g_InMinidumpSafeMode ? "disabled" : "enabled");
    }
    else
    {
        if (Value != 0 && Value != 1)
        {
            ExtOut("Mode must be 0 or 1\n");
            return Status;
        }

        g_InMinidumpSafeMode = (BOOL) Value;
        ExtOut("Unsafe minidump commands are %s.\n",
                g_InMinidumpSafeMode ? "disabled" : "enabled");
    }

    return Status;
}

#endif // FEATURE_PAL

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to get the MethodDesc for a given eip     *
*                                                                      *
\**********************************************************************/
DECLARE_API(IP2MD)
{
    INIT_API_PROBE_MANAGED("ip2md");
    MINIDUMP_NOT_SUPPORTED();

    BOOL dml = FALSE;
    TADDR IP = 0;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&IP, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    EnableDMLHolder dmlHolder(dml);

    if (IP == 0)
    {
        ExtOut("%s is not IP\n", args);
        return E_INVALIDARG;
    }

    CLRDATA_ADDRESS cdaStart = TO_CDADDR(IP);
    CLRDATA_ADDRESS pMD;


    if ((Status = g_sos->GetMethodDescPtrFromIP(cdaStart, &pMD)) != S_OK)
    {
        ExtOut("Failed to request MethodData, not in JIT code range\n");
        return Status;
    }

    DMLOut("MethodDesc:   %s\n", DMLMethodDesc(pMD));
    DumpMDInfo(TO_TADDR(pMD), cdaStart, FALSE /* fStackTraceFormat */);

    WCHAR filename[MAX_LONGPATH];
    ULONG linenum;
    // symlines will be non-zero only if SYMOPT_LOAD_LINES was set in the symbol options
    ULONG symlines = 0;
    if (SUCCEEDED(g_ExtSymbols->GetSymbolOptions(&symlines)))
    {
        symlines &= SYMOPT_LOAD_LINES;
    }

    if (symlines != 0 &&
        SUCCEEDED(GetLineByOffset(TO_CDADDR(IP), &linenum, filename, ARRAY_SIZE(filename))))
    {
        ExtOut("Source file:  %S @ %d\n", filename, linenum);
    }

    return Status;
}

// (MAX_STACK_FRAMES is also used by x86 to prevent infinite loops in _EFN_StackTrace)
#define MAX_STACK_FRAMES 1000

// I use a global set of frames for stack walking on win64 because the debugger's
// GetStackTrace function doesn't provide a way to find out the total size of a stackwalk,
// and I'd like to have a reasonably big maximum without overflowing the stack by declaring
// the buffer locally and I also want to get a managed trace in a low memory environment
// (so no dynamic allocation if possible).
DEBUG_STACK_FRAME g_Frames[MAX_STACK_FRAMES];
CROSS_PLATFORM_CONTEXT g_FrameContexts[MAX_STACK_FRAMES];

static HRESULT
GetContextStackTrace(ULONG osThreadId, PULONG pnumFrames)
{
    PDEBUG_CONTROL4 debugControl4;
    HRESULT hr = S_OK;
    *pnumFrames = 0;

    // Do we have advanced capability?
    if (g_ExtControl->QueryInterface(__uuidof(IDebugControl4), (void **)&debugControl4) == S_OK)
    {
        ULONG oldId, id;
        g_ExtSystem->GetCurrentThreadId(&oldId);

        if ((hr = g_ExtSystem->GetThreadIdBySystemId(osThreadId, &id)) != S_OK) {
            return hr;
        }
        g_ExtSystem->SetCurrentThreadId(id);

        // GetContextStackTrace fills g_FrameContexts as an array of
        // contexts packed as target architecture contexts. We cannot
        // safely cast this as an array of CROSS_PLATFORM_CONTEXT, since
        // sizeof(CROSS_PLATFORM_CONTEXT) != sizeof(TGT_CONTEXT)
        hr = debugControl4->GetContextStackTrace(
            NULL,
            0,
            g_Frames,
            MAX_STACK_FRAMES,
            g_FrameContexts,
            MAX_STACK_FRAMES*g_targetMachine->GetContextSize(),
            g_targetMachine->GetContextSize(),
            pnumFrames);

        g_ExtSystem->SetCurrentThreadId(oldId);
        debugControl4->Release();
    }
    return hr;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function displays the stack trace.  It looks at each DWORD   *
*    on stack.  If the DWORD is a return address, the symbol name or
*    managed function name is displayed.                               *
*                                                                      *
\**********************************************************************/
void DumpStackInternal(DumpStackFlag *pDSFlag)
{
    ReloadSymbolWithLineInfo();

    ULONG64 StackOffset;
    g_ExtRegisters->GetStackOffset (&StackOffset);
    if (pDSFlag->top == 0) {
        pDSFlag->top = TO_TADDR(StackOffset);
    }
    size_t value;
    while (g_ExtData->ReadVirtual(TO_CDADDR(pDSFlag->top), &value, sizeof(size_t), NULL) != S_OK) {
        if (IsInterrupt())
            return;
        pDSFlag->top = NextOSPageAddress(pDSFlag->top);
    }

#ifndef FEATURE_PAL
    if (IsWindowsTarget() && (pDSFlag->end == 0)) {
        // Find the current stack range
        NT_TIB teb;
        ULONG64 dwTebAddr = 0;
        if (SUCCEEDED(g_ExtSystem->GetCurrentThreadTeb(&dwTebAddr)))
        {
            if (SafeReadMemory(TO_TADDR(dwTebAddr), &teb, sizeof(NT_TIB), NULL))
            {
                if (pDSFlag->top > TO_TADDR(teb.StackLimit)
                    && pDSFlag->top <= TO_TADDR(teb.StackBase))
                {
                    if (pDSFlag->end == 0 || pDSFlag->end > TO_TADDR(teb.StackBase))
                        pDSFlag->end = TO_TADDR(teb.StackBase);
                }
            }
        }
    }
#endif // FEATURE_PAL

    if (pDSFlag->end == 0)
    {
        ExtOut("TEB information is not available so a stack size of 0xFFFF is assumed\n");
        pDSFlag->end = pDSFlag->top + 0xFFFF;
    }

    if (pDSFlag->end < pDSFlag->top)
    {
        ExtOut("Wrong option: stack selection wrong\n");
        return;
    }

    DumpStackWorker(*pDSFlag);
}


DECLARE_API(DumpStack)
{
    INIT_API_NO_RET_ON_FAILURE("dumpstack");

    MINIDUMP_NOT_SUPPORTED();

    DumpStackFlag DSFlag;
    DSFlag.fEEonly = FALSE;
    DSFlag.fSuppressSrcInfo = FALSE;
    DSFlag.top = 0;
    DSFlag.end = 0;

    BOOL unwind = FALSE;
    BOOL dml = FALSE;
    CMDOption option[] = {
        // name, vptr, type, hasValue
        {"-EE", &DSFlag.fEEonly, COBOOL, FALSE},
        {"-n",  &DSFlag.fSuppressSrcInfo, COBOOL, FALSE},
        {"-unwind",  &unwind, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE}
    };
    CMDValue arg[] = {
        // vptr, type
        {&DSFlag.top, COHEX},
        {&DSFlag.end, COHEX}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    // symlines will be non-zero only if SYMOPT_LOAD_LINES was set in the symbol options
    ULONG symlines = 0;
    if (!DSFlag.fSuppressSrcInfo && SUCCEEDED(g_ExtSymbols->GetSymbolOptions(&symlines)))
    {
        symlines &= SYMOPT_LOAD_LINES;
    }
    DSFlag.fSuppressSrcInfo = DSFlag.fSuppressSrcInfo || (symlines == 0);

    EnableDMLHolder enabledml(dml);

    ULONG sysId = 0, id = 0;
    g_ExtSystem->GetCurrentThreadSystemId(&sysId);
    ExtOut("OS Thread Id: 0x%x ", sysId);
    g_ExtSystem->GetCurrentThreadId(&id);
    ExtOut("(%d)\n", id);

    DumpStackInternal(&DSFlag);

    return Status;
}


/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function displays the stack trace for threads that EE knows  *
*    from ThreadStore.                                                 *
*                                                                      *
\**********************************************************************/
DECLARE_API (EEStack)
{
    INIT_API();

    MINIDUMP_NOT_SUPPORTED();

    DumpStackFlag DSFlag;
    DSFlag.fEEonly = FALSE;
    DSFlag.fSuppressSrcInfo = FALSE;
    DSFlag.top = 0;
    DSFlag.end = 0;

    BOOL bShortList = FALSE;
    BOOL dml = FALSE;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-EE", &DSFlag.fEEonly, COBOOL, FALSE},
        {"-short", &bShortList, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE}
    };

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder enableDML(dml);

    ULONG Tid;
    g_ExtSystem->GetCurrentThreadId(&Tid);

    DacpThreadStoreData ThreadStore;
    if ((Status = ThreadStore.Request(g_sos)) != S_OK)
    {
        ExtOut("Failed to request ThreadStore\n");
        return Status;
    }

    CLRDATA_ADDRESS CurThread = ThreadStore.firstThread;
    while (CurThread)
    {
        if (IsInterrupt())
            break;

        DacpThreadData Thread;
        if ((Status = Thread.Request(g_sos, CurThread)) != S_OK)
        {
            ExtOut("Failed to request Thread at %p\n", SOS_PTR(CurThread));
            return Status;
        }

        ULONG id=0;
        if (g_ExtSystem->GetThreadIdBySystemId (Thread.osThreadId, &id) != S_OK)
        {
            CurThread = Thread.nextThread;
            continue;
        }

        ExtOut("---------------------------------------------\n");
        ExtOut("Thread %3d\n", id);
        BOOL doIt = FALSE;


#define TS_Hijacked 0x00000080

        if (!bShortList)
        {
            doIt = TRUE;
        }
        else if ((Thread.lockCount > 0) || (Thread.state & TS_Hijacked))
        {
            // TODO: bring back || (int)vThread.m_pFrame != -1  {
            doIt = TRUE;
        }
        else
        {
            ULONG64 IP;
            g_ExtRegisters->GetInstructionOffset (&IP);
            JITTypes jitType;
            DWORD_PTR methodDesc;
            DWORD_PTR gcinfoAddr;
            IP2MethodDesc (TO_TADDR(IP), methodDesc, jitType, gcinfoAddr);
            if (methodDesc)
            {
                doIt = TRUE;
            }
        }

        if (doIt)
        {
            g_ExtSystem->SetCurrentThreadId(id);
            DSFlag.top = 0;
            DSFlag.end = 0;
            DumpStackInternal(&DSFlag);
        }

        CurThread = Thread.nextThread;
    }

    g_ExtSystem->SetCurrentThreadId(Tid);
    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a MethodDesc      *
*    for a given address                                               *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpMD)
{
    INIT_API_PROBE_MANAGED("dumpmd");
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR dwStartAddr = (TADDR)0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    DumpMDInfo(dwStartAddr);

    return Status;
}

BOOL GatherDynamicInfo(TADDR DynamicMethodObj, DacpObjectData *codeArray,
                       DacpObjectData *tokenArray, TADDR *ptokenArrayAddr)
{
    BOOL bRet = FALSE;
    int iOffset;
    DacpObjectData objData; // temp object

    if (codeArray == NULL || tokenArray == NULL)
        return bRet;

    if (objData.Request(g_sos, TO_CDADDR(DynamicMethodObj)) != S_OK)
        return bRet;

    iOffset = GetObjFieldOffset(TO_CDADDR(DynamicMethodObj), objData.MethodTable, W("m_resolver"));
    if (iOffset <= 0)
    {
        iOffset = GetObjFieldOffset(TO_CDADDR(DynamicMethodObj), objData.MethodTable, W("_resolver"));
        if (iOffset <= 0)
            return bRet;
    }

    TADDR resolverPtr;
    if (FAILED(MOVE(resolverPtr, DynamicMethodObj + iOffset)))
        return bRet;

    if (objData.Request(g_sos, TO_CDADDR(resolverPtr)) != S_OK)
        return bRet;

    iOffset = GetObjFieldOffset(TO_CDADDR(resolverPtr), objData.MethodTable, W("m_code"));
    if (iOffset <= 0)
        return bRet;

    TADDR codePtr;
    if (FAILED(MOVE(codePtr, resolverPtr + iOffset)))
        return bRet;

    if (codeArray->Request(g_sos, TO_CDADDR(codePtr)) != S_OK)
        return bRet;

    if (codeArray->dwComponentSize != 1)
        return bRet;

    // We also need the resolution table
    iOffset = GetObjFieldOffset (TO_CDADDR(resolverPtr), objData.MethodTable, W("m_scope"));
    if (iOffset <= 0)
        return bRet;

    TADDR scopePtr;
    if (FAILED(MOVE(scopePtr, resolverPtr + iOffset)))
        return bRet;

    if (objData.Request(g_sos, TO_CDADDR(scopePtr)) != S_OK)
        return bRet;

    iOffset = GetObjFieldOffset (TO_CDADDR(scopePtr), objData.MethodTable, W("m_tokens"));
    if (iOffset <= 0)
        return bRet;

    TADDR tokensPtr;
    if (FAILED(MOVE(tokensPtr, scopePtr + iOffset)))
        return bRet;

    if (objData.Request(g_sos, TO_CDADDR(tokensPtr)) != S_OK)
        return bRet;

    iOffset = GetObjFieldOffset(TO_CDADDR(tokensPtr), objData.MethodTable, W("_items"));
    if (iOffset <= 0)
        return bRet;

    TADDR itemsPtr;
    MOVE (itemsPtr, tokensPtr + iOffset);

    *ptokenArrayAddr = itemsPtr;

    if (tokenArray->Request(g_sos, TO_CDADDR(itemsPtr)) != S_OK)
        return bRet;

    bRet = TRUE; // whew.
    return bRet;
}

typedef std::tuple<TADDR, IMetaDataImport* > GetILAddressResult;
GetILAddressResult GetILAddress(const DacpMethodDescData& MethodDescData);

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    Displays the Microsoft intermediate language (MSIL) that is       *
*    associated with a managed method.                                 *
\**********************************************************************/
DECLARE_API(DumpIL)
{
    INIT_API_PROBE_MANAGED("dumpil");
    MINIDUMP_NOT_SUPPORTED();
    DWORD_PTR dwStartAddr = (TADDR)0;
    DWORD_PTR dwDynamicMethodObj = (TADDR)0;
    BOOL dml = FALSE;
    BOOL fILPointerDirectlySpecified = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-i", &fILPointerDirectlySpecified, COBOOL, FALSE},
        {"/i", &fILPointerDirectlySpecified, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (dwStartAddr == (TADDR)0)
    {
        ExtOut("Must pass a valid expression\n");
        return Status;
    }

    if (fILPointerDirectlySpecified)
    {
        return DecodeILFromAddress(NULL, dwStartAddr);
    }

    if (sos::IsObject(dwStartAddr))
    {
        dwDynamicMethodObj = dwStartAddr;
    }

    if (dwDynamicMethodObj == (TADDR)0)
    {
        // We have been given a MethodDesc
        DacpMethodDescData MethodDescData;
        if (MethodDescData.Request(g_sos, TO_CDADDR(dwStartAddr)) != S_OK)
        {
            ExtOut("%p is not a MethodDesc\n", SOS_PTR(dwStartAddr));
            return Status;
        }

        if (MethodDescData.bIsDynamic && MethodDescData.managedDynamicMethodObject)
        {
            dwDynamicMethodObj = TO_TADDR(MethodDescData.managedDynamicMethodObject);
            if (dwDynamicMethodObj == (TADDR)0)
            {
                ExtOut("Unable to print IL for DynamicMethodDesc %p\n", SOS_PTR(dwDynamicMethodObj));
                return Status;
            }
        }
        else
        {
            GetILAddressResult result = GetILAddress(MethodDescData);
            if (std::get<0>(result) == (TADDR)0)
            {
                ExtOut("ilAddr is %p\n", SOS_PTR(std::get<0>(result)));
                return E_FAIL;
            }
            ExtOut("ilAddr is %p pImport is %p\n", SOS_PTR(std::get<0>(result)), SOS_PTR(std::get<1>(result)));
            TADDR ilAddr = std::get<0>(result);
            ToRelease<IMetaDataImport> pImport(std::get<1>(result));
            IfFailRet(DecodeILFromAddress(pImport, ilAddr));
        }
    }

    if (dwDynamicMethodObj != (TADDR)0)
    {
        // We have a DynamicMethod managed object, let us visit the town and paint.
        DacpObjectData codeArray;
        DacpObjectData tokenArray;
        TADDR tokenArrayAddr;
        if (!GatherDynamicInfo (dwDynamicMethodObj, &codeArray, &tokenArray, &tokenArrayAddr))
        {
            DMLOut("Error gathering dynamic info from object at %s.\n", DMLObject(dwDynamicMethodObj));
            return Status;
        }

        // Read the memory into a local buffer
        BYTE *pArray = new NOTHROW BYTE[(SIZE_T)codeArray.dwNumComponents];
        if (pArray == NULL)
        {
            ExtOut("Not enough memory to read IL\n");
            return Status;
        }

        Status = g_ExtData->ReadVirtual(UL64_TO_CDA(codeArray.ArrayDataPtr), pArray, (ULONG)codeArray.dwNumComponents, NULL);
        if (Status != S_OK)
        {
            ExtOut("Failed to read memory\n");
            delete [] pArray;
            return Status;
        }

        // Now we have a local copy of the IL, and a managed array for token resolution.
        // Visit our IL parser with this info.
        ExtOut("This is dynamic IL. Exception info is not reported at this time.\n");
        ExtOut("If a token is unresolved, run \"%sdumpobj <addr>\" on the addr given\n", SOSPrefix);
        ExtOut("in parenthesis. You can also look at the token table yourself, by\n");
        ExtOut("running \"%sdumparray %p\".\n\n", SOSPrefix, SOS_PTR(tokenArrayAddr));
        DecodeDynamicIL(pArray, (ULONG)codeArray.dwNumComponents, tokenArray);

        delete [] pArray;
    }
    return Status;
}

static void DumpSigWorker (
        DWORD_PTR dwSigAddr,
        DWORD_PTR dwModuleAddr,
        BOOL fMethod)
{
    //
    // Find the length of the signature and copy it into the debugger process.
    //

    ULONG cbSig = 0;
    const ULONG cbSigInc = 256;
    ArrayHolder<COR_SIGNATURE> pSig = new NOTHROW COR_SIGNATURE[cbSigInc];
    if (pSig == NULL)
    {
        ReportOOM();
        return;
    }

    CQuickBytes sigString;
    for (;;)
    {
        if (IsInterrupt())
            return;

        ULONG cbCopied;
        if (!SafeReadMemory(TO_TADDR(dwSigAddr + cbSig), pSig + cbSig, cbSigInc, &cbCopied))
            return;
        cbSig += cbCopied;

        sigString.ReSize(0);
        GetSignatureStringResults result;
        if (fMethod)
            result = GetMethodSignatureString(pSig, cbSig, dwModuleAddr, &sigString);
        else
            result = GetSignatureString(pSig, cbSig, dwModuleAddr, &sigString);

        if (GSS_ERROR == result)
            return;

        if (GSS_SUCCESS == result)
            break;

        // If we didn't get the full amount back, and we failed to parse the
        // signature, it's not valid because of insufficient data
        if (cbCopied < 256)
        {
            ExtOut("Invalid signature\n");
            return;
        }

#ifdef _PREFAST_
#pragma warning(push)
#pragma warning(disable:6280) // "Suppress PREFast warning about mismatch alloc/free"
#endif

        PCOR_SIGNATURE pSigNew = (PCOR_SIGNATURE)realloc(pSig, cbSig+cbSigInc);

#ifdef _PREFAST_
#pragma warning(pop)
#endif

        if (pSigNew == NULL)
        {
            ExtOut("Out of memory\n");
            return;
        }

        pSig = pSigNew;
    }

    ExtOut("%S\n", (PCWSTR)sigString.Ptr());
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump a signature object.               *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpSig)
{
    INIT_API();

    MINIDUMP_NOT_SUPPORTED();

    //
    // Fetch arguments
    //

    StringHolder sigExpr;
    StringHolder moduleExpr;
    CMDValue arg[] =
    {
        {&sigExpr.data, COSTRING},
        {&moduleExpr.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (nArg < 1 || nArg > 2)
    {
        ExtOut("%sdumpsig <sigaddr> [<moduleaddr>]?\n", SOSPrefix);
        return E_INVALIDARG;
    }

    DWORD_PTR dwSigAddr = GetExpression(sigExpr.data);
    if (dwSigAddr == 0)
    {
        ExtOut("Invalid parameter %s\n", sigExpr.data);
        return E_INVALIDARG;
    }

    DWORD_PTR dwModuleAddr = 0;
    if (nArg == 2)
        dwModuleAddr = GetExpression(moduleExpr.data);

    DumpSigWorker(dwSigAddr, dwModuleAddr, TRUE);
    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump a portion of a signature object.  *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpSigElem)
{
    INIT_API();

    MINIDUMP_NOT_SUPPORTED();

    //
    // Fetch arguments
    //

    StringHolder sigExpr;
    StringHolder moduleExpr;
    CMDValue arg[] =
    {
        {&sigExpr.data, COSTRING},
        {&moduleExpr.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (nArg < 1 || nArg > 2)
    {
        ExtOut("%sdumpsigelem <sigaddr> [<moduleaddr>]?\n", SOSPrefix);
        return E_INVALIDARG;
    }

    DWORD_PTR dwSigAddr = GetExpression(sigExpr.data);
    if (dwSigAddr == 0)
    {
        ExtOut("Invalid parameter %s\n", sigExpr.data);
        return E_INVALIDARG;
    }

    DWORD_PTR dwModuleAddr = 0;
    if (nArg == 2)
        dwModuleAddr = GetExpression(moduleExpr.data);

    DumpSigWorker(dwSigAddr, dwModuleAddr, FALSE);
    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of an EEClass from   *
*    a given address
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpClass)
{
    INIT_API_PROBE_MANAGED("dumpclass");
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR dwStartAddr = 0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX}
    };

    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (nArg == 0)
    {
        ExtOut("Missing EEClass address\n");
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    CLRDATA_ADDRESS methodTable;
    BOOL preferMT = FALSE;
    if (!SUCCEEDED(Status = PreferCanonMTOverEEClass(TO_CDADDR(dwStartAddr), &preferMT, &methodTable)))
    {
        ExtOut("Invalid EEClass address\n");
        return Status;
    }

    DacpMethodTableData mtdata;
    if ((Status=mtdata.Request(g_sos, TO_CDADDR(methodTable)))!=S_OK)
    {
        ExtOut("EEClass has an invalid MethodTable address\n");
        return Status;
    }

    sos::MethodTable mt = TO_TADDR(methodTable);
    ExtOut("Class Name:      %S\n", mt.GetName());

    WCHAR fileName[MAX_LONGPATH];
    FileNameForModule(TO_TADDR(mtdata.Module), fileName);
    ExtOut("mdToken:         %p\n", SOS_PTR(mtdata.cl));
    ExtOut("File:            %S\n", fileName);

    CLRDATA_ADDRESS ParentEEClass = (TADDR)0;
    if (mtdata.ParentMethodTable)
    {
        DacpMethodTableData mtdataparent;
        if ((Status=mtdataparent.Request(g_sos, TO_CDADDR(mtdata.ParentMethodTable)))!=S_OK)
        {
            ExtOut("EEClass has an invalid MethodTable address\n");
            return Status;
        }
        ParentEEClass = mtdataparent.Class;
    }

    if (!preferMT)
    {
        DMLOut("Parent Class:    %s\n", DMLClass(ParentEEClass));
    }
    else
    {
        DMLOut("Parent MethodTable: %s\n", DMLMethodTable(mtdata.ParentMethodTable));
    }
    DMLOut("Module:          %s\n", DMLModule(mtdata.Module));
    DMLOut("Method Table:    %s\n", DMLMethodTable(methodTable));
    if (preferMT)
    {
        DMLOut("Canonical MethodTable: %s\n", DMLClass(mtdata.Class));
    }
    if (mtdata.wNumVirtuals != 0)
    {
        ExtOut("Vtable Slots:    %x\n", mtdata.wNumVirtuals);
    }
    if (mtdata.wNumVtableSlots != 0)
    {
        ExtOut("Total Method Slots:  %x\n", mtdata.wNumVtableSlots);
    }
    ExtOut("Class Attributes:    %x  ", mtdata.dwAttrClass);

    if (IsTdInterface(mtdata.dwAttrClass))
        ExtOut("Interface, ");
    if (IsTdAbstract(mtdata.dwAttrClass))
        ExtOut("Abstract, ");
    if (IsTdImport(mtdata.dwAttrClass))
        ExtOut("ComImport, ");

    ExtOut("\n");

    DacpMethodTableFieldData vMethodTableFields;
    if (SUCCEEDED(vMethodTableFields.Request(g_sos, methodTable)))
    {
        ExtOut("NumInstanceFields:   %x\n", vMethodTableFields.wNumInstanceFields);
        ExtOut("NumStaticFields:     %x\n", vMethodTableFields.wNumStaticFields);

        if (vMethodTableFields.wNumThreadStaticFields != 0)
        {
            ExtOut("NumThreadStaticFields: %x\n", vMethodTableFields.wNumThreadStaticFields);
        }


        if (vMethodTableFields.wContextStaticsSize)
        {
            ExtOut("ContextStaticOffset: %x\n", vMethodTableFields.wContextStaticOffset);
            ExtOut("ContextStaticsSize:  %x\n", vMethodTableFields.wContextStaticsSize);
        }


        if (vMethodTableFields.wNumInstanceFields + vMethodTableFields.wNumStaticFields > 0)
        {
            DisplayFields(methodTable, &mtdata, &vMethodTableFields, (TADDR)0, TRUE, FALSE);
        }
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a MethodTable     *
*    from a given address                                              *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpMT)
{
    DWORD_PTR dwStartAddr=0;
    DWORD_PTR dwOriginalAddr;

    INIT_API_PROBE_MANAGED("dumpmt");

    MINIDUMP_NOT_SUPPORTED();

    BOOL bDumpMDTable = FALSE;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-MD", &bDumpMDTable, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE}
    };
    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    TableOutput table(2, 20, AlignLeft, false);

    if (nArg == 0)
    {
        Print("Missing MethodTable address\n");
        return E_INVALIDARG;
    }

    dwOriginalAddr = dwStartAddr;
    dwStartAddr = dwStartAddr&~3;

    if (!IsMethodTable(dwStartAddr))
    {
        Print(dwOriginalAddr, " is not a MethodTable\n");
        return E_INVALIDARG;
    }

    DacpMethodTableData vMethTable;
    vMethTable.Request(g_sos, TO_CDADDR(dwStartAddr));

    if (vMethTable.bIsFree)
    {
        Print("Free MethodTable\n");
        return E_INVALIDARG;
    }

    DacpMethodTableCollectibleData vMethTableCollectible;
    vMethTableCollectible.Request(g_sos, TO_CDADDR(dwStartAddr));

    BOOL preferCanonMT = FALSE;
    if (SUCCEEDED(PreferCanonMTOverEEClass(vMethTable.Class, &preferCanonMT)) && preferCanonMT)
    {
        table.WriteRow("Canonical MethodTable:", EEClassPtr(vMethTable.Class));
    }
    else
    {
        table.WriteRow("EEClass:", EEClassPtr(vMethTable.Class));
    }

    table.WriteRow("Module:", ModulePtr(vMethTable.Module));

    sos::MethodTable mt = (TADDR)dwStartAddr;
    table.WriteRow("Name:", mt.GetName());

    WCHAR fileName[MAX_LONGPATH];
    FileNameForModule(TO_TADDR(vMethTable.Module), fileName);
    table.WriteRow("mdToken:", Pointer(vMethTable.cl));
    table.WriteRow("File:", fileName[0] ? fileName : W("Unknown Module"));

    if (vMethTableCollectible.LoaderAllocatorObjectHandle != (TADDR)0)
    {
        TADDR loaderAllocator;
        if (SUCCEEDED(MOVE(loaderAllocator, vMethTableCollectible.LoaderAllocatorObjectHandle)))
        {
            table.WriteRow("LoaderAllocator:", ObjectPtr(loaderAllocator));
        }
    }

    ReleaseHolder<ISOSDacInterface8> sos8;
    if (SUCCEEDED(g_sos->QueryInterface(__uuidof(ISOSDacInterface8), &sos8)))
    {
        CLRDATA_ADDRESS assemblyLoadContext = 0;
        if (SUCCEEDED(sos8->GetAssemblyLoadContext(TO_CDADDR(dwStartAddr), &assemblyLoadContext)))
        {
            const char* title = "AssemblyLoadContext:";
            if (assemblyLoadContext != 0)
            {
                table.WriteRow(title, ObjectPtr(assemblyLoadContext));
            }
            else
            {
                table.WriteRow(title, "Default ALC - The managed instance of this context doesn't exist yet.");
            }
        }
    }

    table.WriteRow("BaseSize:", PrefixHex(vMethTable.BaseSize));
    table.WriteRow("ComponentSize:", PrefixHex(vMethTable.ComponentSize));
    table.WriteRow("DynamicStatics:", vMethTable.bIsDynamic ? "true" : "false");
    table.WriteRow("ContainsPointers:", vMethTable.bContainsPointers ? "true" : "false");
    table.WriteRow("Number of Methods:", Decimal(vMethTable.wNumMethods));

    table.SetColWidth(0, 29);
    table.WriteRow("Number of IFaces in IFaceMap:", Decimal(vMethTable.wNumInterfaces));

    if (bDumpMDTable)
    {
        table.ReInit(5, POINTERSIZE_HEX, AlignRight);
        table.SetColAlignment(3, AlignLeft);
        table.SetColWidth(2, 6);

        Print("--------------------------------------\n");
        Print("MethodDesc Table\n");

        table.WriteRow("Entry", "MethodDesc", "JIT", "Slot", "Name");

        ISOSMethodEnum *pMethodEnumerator;
        if (SUCCEEDED(g_sos15->GetMethodTableSlotEnumerator(dwStartAddr, &pMethodEnumerator)))
        {
            SOSMethodData entry;
            unsigned int fetched;
            while (SUCCEEDED(pMethodEnumerator->Next(1, &entry, &fetched)) && fetched != 0)
            {
                JITTypes jitType = TYPE_UNKNOWN;
                DWORD_PTR methodDesc = (DWORD_PTR)entry.MethodDesc;
                DWORD_PTR methodDescFromIP2MD = 0;
                DWORD_PTR gcinfoAddr = 0;

                if (entry.Entrypoint != 0)
                {
                    IP2MethodDesc((DWORD_PTR)entry.Entrypoint, methodDescFromIP2MD, jitType, gcinfoAddr);
                    if ((methodDescFromIP2MD != methodDesc) && methodDesc != 0)
                    {
                        ExtOut("MethodDesc from IP2MD does not match MethodDesc from enumerator\n");
                    }
                }

                table.WriteColumn(0, entry.Entrypoint);
                table.WriteColumn(1, MethodDescPtr(methodDesc));

                if (jitType == TYPE_UNKNOWN && methodDesc != (TADDR)0)
                {
                    // We can get a more accurate jitType from NativeCodeAddr of the methoddesc,
                    // because the methodtable entry hasn't always been patched.
                    DacpMethodDescData tmpMethodDescData;
                    if (tmpMethodDescData.Request(g_sos, TO_CDADDR(methodDesc)) == S_OK)
                    {
                        DacpCodeHeaderData codeHeaderData;
                        if (codeHeaderData.Request(g_sos,tmpMethodDescData.NativeCodeAddr) == S_OK)
                        {
                            jitType = (JITTypes) codeHeaderData.JITType;
                        }
                    }
                }

                const char *pszJitType = "NONE";
                if (jitType == TYPE_JIT)
                    pszJitType = "JIT";
                else if (jitType == TYPE_PJIT)
                    pszJitType = "PreJIT";
                else
                {
                    DacpMethodDescData MethodDescData;
                    if (MethodDescData.Request(g_sos, TO_CDADDR(methodDesc)) == S_OK)
                    {
                        // Is it an fcall?
                        ULONG64 baseAddress = g_pRuntime->GetModuleAddress();
                        ULONG64 size = g_pRuntime->GetModuleSize();
                        if ((TO_TADDR(MethodDescData.NativeCodeAddr) >=  TO_TADDR(baseAddress)) &&
                            ((TO_TADDR(MethodDescData.NativeCodeAddr) <  TO_TADDR(baseAddress + size))))
                        {
                            pszJitType = "FCALL";
                        }
                    }
                }

                table.WriteColumn(2, pszJitType);
                table.WriteColumn(3, entry.Slot);

                if (methodDesc != 0)
                    NameForMD_s(methodDesc,g_mdName,mdNameLen);
                else
                {
                    DacpModuleData moduleData;
                    if(moduleData.Request(g_sos, entry.DefiningModule)==S_OK)
                    {
                        NameForToken_s(&moduleData, entry.Token, g_mdName, mdNameLen, true);
                    }
                    else
                    {
                        _snwprintf_s(g_mdName, mdNameLen, _TRUNCATE, W("Unknown Module!%08x"), entry.Token);
                    }
                }
                table.WriteColumn(4, g_mdName);
            }
        }
    }
    return Status;
}

extern size_t Align (size_t nbytes);

HRESULT PrintVC(TADDR taMT, TADDR taObject, BOOL bPrintFields = TRUE)
{
    HRESULT Status;
    DacpMethodTableData mtabledata;
    if ((Status = mtabledata.Request(g_sos, TO_CDADDR(taMT)))!=S_OK)
        return Status;

    size_t size = mtabledata.BaseSize;
    if ((Status=g_sos->GetMethodTableName(TO_CDADDR(taMT), mdNameLen, g_mdName, NULL))!=S_OK)
        return Status;

    ExtOut("Name:        %S\n", g_mdName);
    DMLOut("MethodTable: %s\n", DMLMethodTable(taMT));
    BOOL preferCanonMT = FALSE;
    if (SUCCEEDED(PreferCanonMTOverEEClass(TO_CDADDR(taMT), &preferCanonMT)) && preferCanonMT)
    {
        DMLOut("Canonical MethodTable: %s\n", DMLClass(mtabledata.Class));
    }
    else
    {
        DMLOut("EEClass:     %s\n", DMLClass(mtabledata.Class));
    }
    ExtOut("Size:        %d(0x%x) bytes\n", size, size);

    FileNameForModule(TO_TADDR(mtabledata.Module), g_mdName);
    ExtOut("File:        %S\n", g_mdName[0] ? g_mdName : W("Unknown Module"));

    if (bPrintFields)
    {
        DacpMethodTableFieldData vMethodTableFields;
        if ((Status = vMethodTableFields.Request(g_sos,TO_CDADDR(taMT)))!=S_OK)
            return Status;

        ExtOut("Fields:\n");

        if (vMethodTableFields.wNumInstanceFields + vMethodTableFields.wNumStaticFields > 0)
            DisplayFields(TO_CDADDR(taMT), &mtabledata, &vMethodTableFields, taObject, TRUE, TRUE);
    }

    return S_OK;
}

// If this bit is set in the RuntimeType.m_handle field, the value is a TypeDesc pointer, otherwise it is a MethodTable pointer.
#define RUNTIMETYPE_HANDLE_IS_TYPEDESC 0x2

void PrintRuntimeTypeInfo(TADDR p_rtObject, const DacpObjectData & rtObjectData)
{
    // Get the method table
    int iOffset = GetObjFieldOffset(TO_CDADDR(p_rtObject), rtObjectData.MethodTable, W("m_handle"));
    if (iOffset > 0)
    {
        TADDR mtPtr;
        if (MOVE(mtPtr, p_rtObject + iOffset) == S_OK)
        {
            // Check if TypeDesc
            if ((mtPtr & RUNTIMETYPE_HANDLE_IS_TYPEDESC) != 0)
            {
                ExtOut("TypeDesc:    %p\n", mtPtr & ~RUNTIMETYPE_HANDLE_IS_TYPEDESC);
            }
            else
            {
                sos::MethodTable mt = mtPtr;
                ExtOut("Type Name:   %S\n", mt.GetName());
                DMLOut("Type MT:     %s\n", DMLMethodTable(mtPtr));
            }
        }
    }
}

void DisplayInvalidStructuresMessage()
{
    ExtOut("The garbage collector data structures are not in a valid state for traversal.\n");
    ExtOut("It is either in the \"plan phase,\" where objects are being moved around, or\n");
    ExtOut("we are at the initialization or shutdown of the gc heap. Commands related to\n");
    ExtOut("displaying, finding or traversing objects as well as gc heap segments may not\n");
    ExtOut("work properly. %sdumpheap and %sverifyheap may incorrectly complain of heap\n", SOSPrefix, SOSPrefix);
    ExtOut("consistency errors.\n");
}

HRESULT PrintObj(TADDR taObj, BOOL bPrintFields = TRUE)
{
    if (!sos::IsObject(taObj, true))
    {
        ExtOut("<Note: this object has an invalid CLASS field>\n");
        if (!GetGcStructuresValid())
        {
            DisplayInvalidStructuresMessage();
        }
    }

    DacpObjectData objData;
    HRESULT Status;
    if ((Status=objData.Request(g_sos, TO_CDADDR(taObj))) != S_OK)
    {
        ExtOut("Invalid object\n");
        return Status;
    }

    if (objData.ObjectType==OBJ_FREE)
    {
        ExtOut("Free Object\n");
        DWORD_PTR size = (DWORD_PTR)objData.Size;
        ExtOut("Size:        %" POINTERSIZE_TYPE "d(0x%" POINTERSIZE_TYPE "x) bytes\n", size, size);
        return S_OK;
    }

    sos::Object obj = taObj;
    ExtOut("Name:        %S\n", obj.GetTypeName());
    DMLOut("MethodTable: %s\n", DMLMethodTable(objData.MethodTable));


    DacpMethodTableData mtabledata;
    if ((Status=mtabledata.Request(g_sos,objData.MethodTable)) == S_OK)
    {
        BOOL preferCanonMT = FALSE;
        if (SUCCEEDED(PreferCanonMTOverEEClass(mtabledata.Class, &preferCanonMT)) && preferCanonMT)
        {
            DMLOut("Canonical MethodTable: %s\n", DMLClass(mtabledata.Class));
        }
        else
        {
            DMLOut("EEClass:     %s\n", DMLClass(mtabledata.Class));
        }
    }
    else
    {
        ExtOut("Invalid EEClass address\n");
        return Status;
    }

    if (objData.RCW != (TADDR)0)
    {
        DMLOut("RCW:         %s\n", DMLRCWrapper(objData.RCW));
    }
    if (objData.CCW != (TADDR)0)
    {
        DMLOut("CCW:         %s\n", DMLCCWrapper(objData.CCW));
    }

    // Check for ComWrappers CCWs
    ReleaseHolder<ISOSDacInterface10> sos10;
    if (SUCCEEDED(Status = g_sos->QueryInterface(__uuidof(ISOSDacInterface10), &sos10)))
    {
        CLRDATA_ADDRESS objAddr = TO_CDADDR(taObj);
        CLRDATA_ADDRESS rcwNative;
        unsigned int needed;
       if (SUCCEEDED(sos10->GetObjectComWrappersData(objAddr, &rcwNative, 0, NULL, &needed))
            && (needed > 0 || rcwNative != 0))
        {
            ArrayHolder<CLRDATA_ADDRESS> pArray = new NOTHROW CLRDATA_ADDRESS[needed];
            if (pArray != NULL)
            {
                if (SUCCEEDED(sos10->GetObjectComWrappersData(objAddr, &rcwNative, needed, pArray, NULL)))
                {
                    if (rcwNative != 0)
                    {
                        DMLOut("ComWrappers RCW: %s\n", DMLRCWrapper(rcwNative));
                    }

                    if (needed > 0)
                    {
                        ExtOut("ComWrappers CCWs:\n");
                    }

                    for (unsigned int i = 0; i < needed; ++i)
                    {
                        DMLOut("             %s\n", DMLCCWrapper(pArray[i]));
                    }
                }
                else
                {
                    ExtOut("Failed to get ComWrappers RCW/CCW data for the object\n");
                }
            }
            else
            {
                ReportOOM();
            }

        }
    }

    // Check for Tracked Type and tagged memory
    ReleaseHolder<ISOSDacInterface11> sos11;
    if (SUCCEEDED(g_sos->QueryInterface(__uuidof(ISOSDacInterface11), &sos11)))
    {
        CLRDATA_ADDRESS objAddr = TO_CDADDR(taObj);
        BOOL isTrackedType;
        BOOL hasTaggedMemory;
        if (SUCCEEDED(sos11->IsTrackedType(objAddr, &isTrackedType, &hasTaggedMemory)))
        {
            ExtOut("Tracked Type: %s\n", isTrackedType ? "true" : "false");
            if (hasTaggedMemory)
            {
                CLRDATA_ADDRESS taggedMemory = (TADDR)0;
                size_t taggedMemorySizeInBytes = 0;
                (void)sos11->GetTaggedMemory(objAddr, &taggedMemory, &taggedMemorySizeInBytes);
                DMLOut("Tagged Memory: %s (%" POINTERSIZE_TYPE "d(0x%" POINTERSIZE_TYPE "x) bytes)\n",
                    DMLTaggedMemory(taggedMemory, taggedMemorySizeInBytes / sizeof(void*)), taggedMemorySizeInBytes, taggedMemorySizeInBytes);
            }
        }
    }

    DWORD_PTR size = (DWORD_PTR)objData.Size;
    ExtOut("Size:        %" POINTERSIZE_TYPE "d(0x%" POINTERSIZE_TYPE "x) bytes\n", size, size);

    if (_wcscmp(obj.GetTypeName(), W("System.RuntimeType")) == 0)
    {
        PrintRuntimeTypeInfo(taObj, objData);
    }

    if (_wcscmp(obj.GetTypeName(), W("System.RuntimeType+RuntimeTypeCache")) == 0)
    {
        // Get the method table
        int iOffset = GetObjFieldOffset (TO_CDADDR(taObj), objData.MethodTable, W("m_runtimeType"));
        if (iOffset > 0)
        {
            TADDR rtPtr;
            if (MOVE(rtPtr, taObj + iOffset) == S_OK)
            {
                DacpObjectData rtObjectData;
                if ((Status=rtObjectData.Request(g_sos, TO_CDADDR(rtPtr))) != S_OK)
                {
                    ExtOut("Error when reading RuntimeType field\n");
                    return Status;
                }

                PrintRuntimeTypeInfo(rtPtr, rtObjectData);
            }
        }
    }

    if (objData.ObjectType==OBJ_ARRAY)
    {
        ExtOut("Array:       Rank %d, Number of elements %" POINTERSIZE_TYPE "d, Type %s",
                objData.dwRank, (DWORD_PTR)objData.dwNumComponents, ElementTypeName(objData.ElementType));

        IfDMLOut(" (<exec cmd=\"!DumpArray /d %p\">Print Array</exec>)", SOS_PTR(taObj));
        ExtOut("\n");

        if (objData.ElementType == ELEMENT_TYPE_I1 ||
            objData.ElementType == ELEMENT_TYPE_U1 ||
            objData.ElementType == ELEMENT_TYPE_CHAR)
        {
            bool wide = objData.ElementType == ELEMENT_TYPE_CHAR;

            // Get the size of the character array, but clamp it to a reasonable length.
            TADDR pos = taObj + (2 * sizeof(DWORD_PTR));
            DWORD_PTR num;
            moveN(num, taObj + sizeof(DWORD_PTR));

            if (IsDMLEnabled())
                DMLOut("<exec cmd=\"%s %" POINTERSIZE_TYPE "x L%x\">Content</exec>:     ", (wide) ? "dw" : "db", pos, num);
            else
                ExtOut("Content:     ");
            CharArrayContent(pos, (ULONG)(num <= 128 ? num : 128), wide);
            ExtOut("\n");
        }
    }
    else
    {
        FileNameForModule(TO_TADDR(mtabledata.Module), g_mdName);
        ExtOut("File:        %S\n", g_mdName[0] ? g_mdName : W("Unknown Module"));
    }

    if (objData.ObjectType == OBJ_STRING)
    {
        ExtOut("String:      ");
        StringObjectContent(taObj);
        ExtOut("\n");
    }
    else if (objData.ObjectType == OBJ_OBJECT)
    {
        ExtOut("Object\n");
    }

    if (bPrintFields)
    {
        DacpMethodTableFieldData vMethodTableFields;
        if ((Status = vMethodTableFields.Request(g_sos,TO_CDADDR(objData.MethodTable)))!=S_OK)
            return Status;

        ExtOut("Fields:\n");
        if (vMethodTableFields.wNumInstanceFields + vMethodTableFields.wNumStaticFields > 0)
        {
            DisplayFields(objData.MethodTable, &mtabledata, &vMethodTableFields, taObj, TRUE, FALSE);
        }
        else
        {
            ExtOut("None\n");
        }
    }

    sos::ThinLockInfo lockInfo;
    if (obj.GetThinLock(lockInfo))
    {
        ExtOut("ThinLock owner %x (%p), Recursive %x\n", lockInfo.ThreadId,
            SOS_PTR(lockInfo.ThreadPtr), lockInfo.Recursion);
    }

    return S_OK;
}

HRESULT PrintALC(TADDR taObj)
{
    if (!sos::IsObject(taObj, true))
    {
        ExtOut("<Note: this object has an invalid CLASS field>\n");
    }

    DacpObjectData objData;
    HRESULT Status;
    if ((Status=objData.Request(g_sos, TO_CDADDR(taObj))) != S_OK)
    {
        ExtOut("Invalid object\n");
        return Status;
    }

    if (objData.ObjectType==OBJ_FREE)
    {
        ExtOut("Free Object\n");
        DWORD_PTR size = (DWORD_PTR)objData.Size;
        ExtOut("Size:        %" POINTERSIZE_TYPE "d(0x%" POINTERSIZE_TYPE "x) bytes\n", size, size);
        return S_OK;
    }

    CLRDATA_ADDRESS assemblyLoadContext = 0;
    ReleaseHolder<ISOSDacInterface8> sos8;
    if (SUCCEEDED(Status = g_sos->QueryInterface(__uuidof(ISOSDacInterface8), &sos8)))
    {
        Status = sos8->GetAssemblyLoadContext(objData.MethodTable, &assemblyLoadContext);
        if (FAILED(Status))
        {
            ExtOut("Failed to get the AssemblyLoadContext\n");
            return Status;
        }
    }

    if (assemblyLoadContext == 0)
    {
        ExtOut("Name:        System.Runtime.Loader.DefaultAssemblyLoadContext\n");
        ExtOut("The managed instance of this context doesn't exist yet\n");
        return S_OK;
    }

    return PrintObj(TO_TADDR(assemblyLoadContext));
}

BOOL IndicesInRange (DWORD * indices, DWORD * lowerBounds, DWORD * bounds, DWORD rank)
{
    int i = 0;
    if (!ClrSafeInt<int>::subtraction((int)rank, 1, i))
    {
        ExtOut("<integer underflow>\n");
        return FALSE;
    }

    for (; i >= 0; i--)
    {
        if (indices[i] >= bounds[i] + lowerBounds[i])
        {
            if (i == 0)
            {
                return FALSE;
            }

            indices[i] = lowerBounds[i];
            indices[i - 1]++;
        }
    }

    return TRUE;
}

void ExtOutIndices (DWORD * indices, DWORD rank)
{
    for (DWORD i = 0; i < rank; i++)
    {
        ExtOut("[%d]", indices[i]);
    }
}

size_t OffsetFromIndices (DWORD * indices, DWORD * lowerBounds, DWORD * bounds, DWORD rank)
{
    _ASSERTE(rank >= 0);
    size_t multiplier = 1;
    size_t offset = 0;
    int i = 0;
    if (!ClrSafeInt<int>::subtraction((int)rank, 1, i))
    {
        ExtOut("<integer underflow>\n");
        return 0;
    }

    for (; i >= 0; i--)
    {
        DWORD curIndex = indices[i] - lowerBounds[i];
        offset += curIndex * multiplier;
        multiplier *= bounds[i];
    }

    return offset;
}
HRESULT PrintArray(DacpObjectData& objData, DumpArrayFlags& flags, BOOL isPermSetPrint);
#ifdef _DEBUG
HRESULT PrintPermissionSet (TADDR p_PermSet)
{
    HRESULT Status = S_OK;

    DacpObjectData PermSetData;
    if ((Status=PermSetData.Request(g_sos, TO_CDADDR(p_PermSet))) != S_OK)
    {
        ExtOut("Invalid object\n");
        return Status;
    }


    sos::MethodTable mt = TO_TADDR(PermSetData.MethodTable);
    if (_wcscmp (W("System.Security.PermissionSet"), mt.GetName()) != 0 && _wcscmp(W("System.Security.NamedPermissionSet"), mt.GetName()) != 0)
    {
        ExtOut("Invalid PermissionSet object\n");
        return S_FALSE;
    }

    ExtOut("PermissionSet object: %p\n", SOS_PTR(p_PermSet));

    // Print basic info

    // Walk the fields, printing some fields in a special way.

    int iOffset = GetObjFieldOffset (TO_CDADDR(p_PermSet), PermSetData.MethodTable, W("m_Unrestricted"));

    if (iOffset > 0)
    {
        BYTE unrestricted;
        MOVE(unrestricted, p_PermSet + iOffset);
        if (unrestricted)
            ExtOut("Unrestricted: TRUE\n");
        else
            ExtOut("Unrestricted: FALSE\n");
    }

    iOffset = GetObjFieldOffset (TO_CDADDR(p_PermSet), PermSetData.MethodTable, W("m_permSet"));
    if (iOffset > 0)
    {
        TADDR tbSetPtr;
        MOVE(tbSetPtr, p_PermSet + iOffset);
        if (tbSetPtr != (TADDR)0)
        {
            DacpObjectData tbSetData;
            if ((Status=tbSetData.Request(g_sos, TO_CDADDR(tbSetPtr))) != S_OK)
            {
                ExtOut("Invalid object\n");
                return Status;
            }

            iOffset = GetObjFieldOffset (TO_CDADDR(tbSetPtr), tbSetData.MethodTable, W("m_Set"));
            if (iOffset > 0)
            {
                DWORD_PTR PermsArrayPtr;
                MOVE(PermsArrayPtr, tbSetPtr + iOffset);
                if (PermsArrayPtr != (TADDR)0)
                {
                    // Print all the permissions in the array
                    DacpObjectData objData;
                    if ((Status=objData.Request(g_sos, TO_CDADDR(PermsArrayPtr))) != S_OK)
                    {
                        ExtOut("Invalid object\n");
                        return Status;
                    }
                    DumpArrayFlags flags;
                    flags.bDetail = TRUE;
                    return PrintArray(objData, flags, TRUE);
                }
            }

            iOffset = GetObjFieldOffset (TO_CDADDR(tbSetPtr), tbSetData.MethodTable, W("m_Obj"));
            if (iOffset > 0)
            {
                DWORD_PTR PermObjPtr;
                MOVE(PermObjPtr, tbSetPtr + iOffset);
                if (PermObjPtr != (TADDR)0)
                {
                    // Print the permission object
                    return PrintObj(PermObjPtr);
                }
            }


        }
    }
    return Status;
}

#endif // _DEBUG

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of an object from a  *
*    given address
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpArray)
{
    INIT_API_PROBE_MANAGED("dumparray");

    DumpArrayFlags flags;

    MINIDUMP_NOT_SUPPORTED();

    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-start", &flags.startIndex, COSIZE_T, TRUE},
        {"-length", &flags.Length, COSIZE_T, TRUE},
        {"-details", &flags.bDetail, COBOOL, FALSE},
        {"-nofields", &flags.bNoFieldsForElement, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&flags.strObject, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    DWORD_PTR p_Object = GetExpression (flags.strObject);
    if (p_Object == 0)
    {
        ExtOut("Invalid parameter %s\n", flags.strObject);
        return E_INVALIDARG;
    }

    if (!sos::IsObject(p_Object, true))
    {
        ExtOut("<Note: this object has an invalid CLASS field>\n");
    }

    DacpObjectData objData;
    if ((Status=objData.Request(g_sos, TO_CDADDR(p_Object))) != S_OK)
    {
        ExtOut("Invalid object\n");
        return Status;
    }

    if (objData.ObjectType != OBJ_ARRAY)
    {
        ExtOut("Not an array, please use %sdumpobj instead\n", SOSPrefix);
        return S_OK;
    }
    return PrintArray(objData, flags, FALSE);
}


HRESULT PrintArray(DacpObjectData& objData, DumpArrayFlags& flags, BOOL isPermSetPrint)
{
    HRESULT Status = S_OK;

    if (objData.dwRank != 1 && (flags.Length != (DWORD_PTR)-1 ||flags.startIndex != 0))
    {
        ExtOut("For multi-dimension array, length and start index are supported\n");
        return S_OK;
    }

    if (flags.startIndex > objData.dwNumComponents)
    {
        ExtOut("Start index out of range\n");
        return S_OK;
    }

    if (!flags.bDetail && flags.bNoFieldsForElement)
    {
        ExtOut("-nofields has no effect unless -details is specified\n");
    }

    DWORD i;
    if (!isPermSetPrint)
    {
        // TODO: don't depend on this being a MethodTable
        NameForMT_s(TO_TADDR(objData.ElementTypeHandle), g_mdName, mdNameLen);

        ExtOut("Name:        %S[", g_mdName);
        for (i = 1; i < objData.dwRank; i++)
            ExtOut(",");
        ExtOut("]\n");

        DMLOut("MethodTable: %s\n", DMLMethodTable(objData.MethodTable));

        {
            DacpMethodTableData mtdata;
            if (SUCCEEDED(mtdata.Request(g_sos, objData.MethodTable)))
            {
                DMLOut("EEClass:     %s\n", DMLClass(mtdata.Class));
            }
        }

        DWORD_PTR size = (DWORD_PTR)objData.Size;
        ExtOut("Size:        %" POINTERSIZE_TYPE "d(0x%" POINTERSIZE_TYPE "x) bytes\n", size, size);

        ExtOut("Array:       Rank %d, Number of elements %" POINTERSIZE_TYPE "d, Type %s\n",
                objData.dwRank, (DWORD_PTR)objData.dwNumComponents, ElementTypeName(objData.ElementType));
        DMLOut("Element Methodtable: %s\n", DMLMethodTable(objData.ElementTypeHandle));
    }

    BOOL isElementValueType = IsElementValueType(objData.ElementType);

    DWORD dwRankAllocSize;
    if (!ClrSafeInt<DWORD>::multiply(sizeof(DWORD), objData.dwRank, dwRankAllocSize))
    {
        ExtOut("Integer overflow on array rank\n");
        return Status;
    }

    DWORD *lowerBounds = (DWORD *)alloca(dwRankAllocSize);
    if (!SafeReadMemory(TO_TADDR(objData.ArrayLowerBoundsPtr), lowerBounds, dwRankAllocSize, NULL))
    {
        ExtOut("Failed to read lower bounds info from the array\n");
        return S_OK;
    }

    DWORD *bounds = (DWORD *)alloca(dwRankAllocSize);
    if (!SafeReadMemory(TO_TADDR(objData.ArrayBoundsPtr), bounds, dwRankAllocSize, NULL))
    {
        ExtOut("Failed to read bounds info from the array\n");
        return S_OK;
    }

    //length is only supported for single-dimension array
    if (objData.dwRank == 1 && flags.Length != (DWORD_PTR)-1)
    {
        bounds[0] = _min(bounds[0], (DWORD)(flags.Length + flags.startIndex) - lowerBounds[0]);
    }

    DWORD *indices = (DWORD *)alloca(dwRankAllocSize);
    for (i = 0; i < objData.dwRank; i++)
    {
        indices[i] = lowerBounds[i];
    }

    //start index is only supported for single-dimension array
    if (objData.dwRank == 1)
    {
        indices[0] = (DWORD)flags.startIndex;
    }

    //Offset should be calculated by OffsetFromIndices. However because of the way
    //how we grow indices, incrementing offset by one happens to match indices in every iteration
    for (size_t offset = OffsetFromIndices (indices, lowerBounds, bounds, objData.dwRank);
        IndicesInRange (indices, lowerBounds, bounds, objData.dwRank);
        indices[objData.dwRank - 1]++, offset++)
    {
        if (IsInterrupt())
        {
            ExtOut("interrupted by user\n");
            break;
        }

        TADDR elementAddress = TO_TADDR(objData.ArrayDataPtr + offset * objData.dwComponentSize);
        TADDR p_Element = (TADDR)0;
        if (isElementValueType)
        {
            p_Element = elementAddress;
        }
        else if (!SafeReadMemory (elementAddress, &p_Element, sizeof (p_Element), NULL))
        {
            ExtOut("Failed to read element at ");
            ExtOutIndices(indices, objData.dwRank);
            ExtOut("\n");
            continue;
        }

        if (p_Element)
        {
            ExtOutIndices(indices, objData.dwRank);

            if (isElementValueType)
            {
                DMLOut( " %s\n", DMLValueClass(objData.ElementTypeHandle, p_Element));
            }
            else
            {
                DMLOut(" %s\n", DMLObject(p_Element));
            }
        }
        else if (!isPermSetPrint)
        {
            ExtOutIndices(indices, objData.dwRank);
            ExtOut(" null\n");
        }

        if (flags.bDetail)
        {
            IncrementIndent();
            if (isElementValueType)
            {
                PrintVC(TO_TADDR(objData.ElementTypeHandle), elementAddress, !flags.bNoFieldsForElement);
            }
            else if (p_Element != (TADDR)0)
            {
                PrintObj(p_Element, !flags.bNoFieldsForElement);
            }
            DecrementIndent();
        }
    }

    return S_OK;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of an object from a  *
*    given address
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpObj)
{
    INIT_API_PROBE_MANAGED("dumpobj");

    MINIDUMP_NOT_SUPPORTED();

    BOOL dml = FALSE;
    BOOL bNoFields = FALSE;
    BOOL bRefs = FALSE;
    StringHolder str_Object;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-nofields", &bNoFields, COBOOL, FALSE},
        {"-refs", &bRefs, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&str_Object.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    DWORD_PTR p_Object = GetExpression(str_Object.data);
    EnableDMLHolder dmlHolder(dml);
    if (p_Object == 0)
    {
        ExtOut("Invalid parameter %s\n", args);
        return E_INVALIDARG;
    }

    try {
        Status = PrintObj(p_Object, !bNoFields);

        if (SUCCEEDED(Status) && bRefs)
        {
            std::stringstream argsBuilder;
            argsBuilder << std::hex << p_Object << " ";
            return ExecuteCommand("dumpobjgcrefs", argsBuilder.str().c_str());
        }
    }
    catch(const sos::Exception &e)
    {
        ExtOut("%s\n", e.what());
        return E_FAIL;
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of an object from a  *
*    given address                                                     *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpALC)
{
    INIT_API();

    MINIDUMP_NOT_SUPPORTED();

    BOOL dml = FALSE;
    StringHolder str_Object;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&str_Object.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    DWORD_PTR p_Object = GetExpression(str_Object.data);
    EnableDMLHolder dmlHolder(dml);
    if (p_Object == 0)
    {
        ExtOut("Invalid parameter %s\n", args);
        return E_INVALIDARG;
    }

    try
    {
        Status = PrintALC(p_Object);
    }
    catch(const sos::Exception &e)
    {
        ExtOut("%s\n", e.what());
        return E_FAIL;
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a delegate from a *
*    given address.                                                    *
*                                                                      *
\**********************************************************************/

DECLARE_API(DumpDelegate)
{
    INIT_API_PROBE_MANAGED("dumpdelegate");
    MINIDUMP_NOT_SUPPORTED();

    try
    {
        BOOL dml = FALSE;
        DWORD_PTR dwAddr = 0;

        CMDOption option[] =
        {   // name, vptr, type, hasValue
            {"/d", &dml, COBOOL, FALSE}
        };
        CMDValue arg[] =
        {   // vptr, type
            {&dwAddr, COHEX}
        };
        size_t nArg;
        if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
        {
            return E_INVALIDARG;
        }
        if (nArg != 1)
        {
            ExtOut("Usage: %sdumpdelegate <delegate object address>\n", SOSPrefix);
            return E_INVALIDARG;
        }

        EnableDMLHolder dmlHolder(dml);
        CLRDATA_ADDRESS delegateAddr = TO_CDADDR(dwAddr);

        if (!sos::IsObject(delegateAddr))
        {
            ExtOut("Invalid object.\n");
        }
        else
        {
            sos::Object delegateObj = TO_TADDR(delegateAddr);
            if (!IsDerivedFrom(TO_CDADDR(delegateObj.GetMT()), W("System.Delegate")))
            {
                ExtOut("Object of type '%S' is not a delegate.", delegateObj.GetTypeName());
            }
            else
            {
                ExtOut("Target           Method           Name\n");

                std::vector<CLRDATA_ADDRESS> delegatesRemaining;
                delegatesRemaining.push_back(delegateAddr);
                while (delegatesRemaining.size() > 0)
                {
                    delegateAddr = delegatesRemaining.back();
                    delegatesRemaining.pop_back();
                    delegateObj = TO_TADDR(delegateAddr);

                    int offset;
                    if ((offset = GetObjFieldOffset(delegateObj.GetAddress(), delegateObj.GetMT(), W("_target"))) != 0)
                    {
                        CLRDATA_ADDRESS target;
                        MOVE(target, delegateObj.GetAddress() + offset);

                        if ((offset = GetObjFieldOffset(delegateObj.GetAddress(), delegateObj.GetMT(), W("_invocationList"))) != 0)
                        {
                            CLRDATA_ADDRESS invocationList;
                            MOVE(invocationList, delegateObj.GetAddress() + offset);

                            if ((offset = GetObjFieldOffset(delegateObj.GetAddress(), delegateObj.GetMT(), W("_invocationCount"))) != 0)
                            {
                                int invocationCount;
                                MOVE(invocationCount, delegateObj.GetAddress() + offset);

                                if (invocationList == (TADDR)0)
                                {
                                    CLRDATA_ADDRESS md;
                                    DMLOut("%s ", DMLObject(target));
                                    if (TryGetMethodDescriptorForDelegate(delegateAddr, &md))
                                    {
                                        DMLOut("%s ", DMLMethodDesc(md));
                                        NameForMD_s((DWORD_PTR)md, g_mdName, mdNameLen);
                                        ExtOut("%S\n", g_mdName);
                                    }
                                    else
                                    {
                                        ExtOut("(unknown)\n");
                                    }
                                }
                                else if (sos::IsObject(invocationList, false))
                                {
                                    DacpObjectData objData;
                                    if (objData.Request(g_sos, invocationList) == S_OK &&
                                        objData.ObjectType == OBJ_ARRAY &&
                                        invocationCount <= objData.dwNumComponents)
                                    {
                                        for (int i = 0; i < invocationCount; i++)
                                        {
                                            CLRDATA_ADDRESS elementPtr;
                                            MOVE(elementPtr, TO_CDADDR(objData.ArrayDataPtr + (i * objData.dwComponentSize)));
                                            if (elementPtr != (TADDR)0 && sos::IsObject(elementPtr, false))
                                            {
                                                delegatesRemaining.push_back(elementPtr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return S_OK;
    }
    catch (const sos::Exception &e)
    {
        ExtOut("%s\n", e.what());
        return E_FAIL;
    }
}

CLRDATA_ADDRESS isExceptionObj(CLRDATA_ADDRESS mtObj)
{
    // We want to follow back until we get the mt for System.Exception
    DacpMethodTableData dmtd;
    CLRDATA_ADDRESS walkMT = mtObj;
    while(walkMT != (TADDR)0)
    {
        if (dmtd.Request(g_sos, walkMT) != S_OK)
        {
            break;
        }
        if (walkMT == g_special_usefulGlobals.ExceptionMethodTable)
        {
            return walkMT;
        }
        walkMT = dmtd.ParentMethodTable;
    }
    return (TADDR)0;
}

CLRDATA_ADDRESS isSecurityExceptionObj(CLRDATA_ADDRESS mtObj)
{
    // We want to follow back until we get the mt for System.Exception
    DacpMethodTableData dmtd;
    CLRDATA_ADDRESS walkMT = mtObj;
    while(walkMT != (TADDR)0)
    {
        if (dmtd.Request(g_sos, walkMT) != S_OK)
        {
            break;
        }
        NameForMT_s(TO_TADDR(walkMT), g_mdName, mdNameLen);
        if (_wcscmp(W("System.Security.SecurityException"), g_mdName) == 0)
        {
            return walkMT;
        }
        walkMT = dmtd.ParentMethodTable;
    }
    return (TADDR)0;
}

// Fill the passed in buffer with a text header for generated exception information.
// Returns the number of characters in the wszBuffer array on exit.
// If NULL is passed for wszBuffer, just returns the number of characters needed.
size_t AddExceptionHeader (__out_ecount_opt(bufferLength) WCHAR *wszBuffer, size_t bufferLength)
{
#ifdef _TARGET_WIN64_
    const WCHAR *wszHeader = W("    SP               IP               Function\n");
#else
    const WCHAR *wszHeader = W("    SP       IP       Function\n");
#endif // _TARGET_WIN64_
    if (wszBuffer)
    {
        swprintf_s(wszBuffer, bufferLength, wszHeader);
    }
    return _wcslen(wszHeader);
}

enum StackTraceElementFlags
{
    // Set if this element represents the last frame of the foreign exception stack trace
    STEF_LAST_FRAME_FROM_FOREIGN_STACK_TRACE = 0x0001,

    // Set if the "ip" field has already been adjusted (decremented)
    STEF_IP_ADJUSTED = 0x0002,
};

// This struct needs to match the definition in the runtime.
// See: https://github.com/dotnet/runtime/blob/main/src/coreclr/vm/clrex.h
struct StackTraceElement
{
    UINT_PTR        ip;
    UINT_PTR        sp;
    DWORD_PTR       pFunc;  // MethodDesc
    INT             flags;  // This is StackTraceElementFlags but it needs to always be "int" sized for backward compatibility.
};

#include "sos_stacktrace.h"

#include "sildasm.h"

class StringOutput
{
public:
    CQuickString cs;
    StringOutput()
    {
        cs.Alloc(1024);
        cs.String()[0] = L'\0';
    }

    BOOL Append(__in_z LPCWSTR pszStr)
    {
        size_t iInputLen = _wcslen (pszStr);
        size_t iCurLen = _wcslen (cs.String());
        if ((iCurLen + iInputLen + 1) > cs.Size())
        {
            if (cs.ReSize(iCurLen + iInputLen + 1) != S_OK)
            {
                return FALSE;
            }
        }

        wcsncat_s (cs.String(), cs.Size(), pszStr, _TRUNCATE);
        return TRUE;
    }

    size_t Length()
    {
        return _wcslen(cs.String());
    }

    WCHAR *String()
    {
        return cs.String();
    }
};

static HRESULT DumpMDInfoBuffer(DWORD_PTR dwStartAddr, DWORD Flags, ULONG64 Esp, ULONG64 IPAddr, StringOutput& so);

// Using heuristics to determine if an exception object represented an async (hardware) or a
// managed exception
// We need to use these heuristics when the System.Exception object is not the active exception
// on some thread, but it's something found somewhere on the managed heap.

// uses the MapWin32FaultToCOMPlusException to figure out how we map async exceptions
// to managed exceptions and their HRESULTs
static const HRESULT AsyncHResultValues[] =
{
    COR_E_ARITHMETIC,    // kArithmeticException
    COR_E_OVERFLOW,      // kOverflowException
    COR_E_DIVIDEBYZERO,  // kDivideByZeroException
    COR_E_FORMAT,        // kFormatException
    COR_E_NULLREFERENCE, // kNullReferenceException
    E_POINTER,           // kAccessViolationException
    // the EE is raising the next exceptions more often than the OS will raise an async
    // exception for these conditions, so in general treat these as Synchronous
      // COR_E_INDEXOUTOFRANGE, // kIndexOutOfRangeException
      // COR_E_OUTOFMEMORY,   // kOutOfMemoryException
      // COR_E_STACKOVERFLOW, // kStackOverflowException
    COR_E_DATAMISALIGNED, // kDataMisalignedException

};
BOOL IsAsyncException(CLRDATA_ADDRESS taObj, CLRDATA_ADDRESS mtObj)
{
    // by default we'll treat exceptions as synchronous
    UINT32 xcode = EXCEPTION_COMPLUS;
    int iOffset = GetObjFieldOffset (taObj, mtObj, W("_xcode"));
    if (iOffset > 0)
    {
        HRESULT hr = MOVE(xcode, taObj + iOffset);
        if (hr != S_OK)
        {
            xcode = EXCEPTION_COMPLUS;
            goto Done;
        }
    }

    if (xcode == EXCEPTION_COMPLUS)
    {
        HRESULT ehr = 0;
        iOffset = GetObjFieldOffset (taObj, mtObj, W("_HResult"));
        if (iOffset > 0)
        {
            HRESULT hr = MOVE(ehr, taObj + iOffset);
            if (hr != S_OK)
            {
                xcode = EXCEPTION_COMPLUS;
                goto Done;
            }
            for (size_t idx = 0; idx < ARRAY_SIZE(AsyncHResultValues); ++idx)
            {
                if (ehr == AsyncHResultValues[idx])
                {
                    xcode = ehr;
                    break;
                }
            }
        }
    }
Done:
    return xcode != EXCEPTION_COMPLUS;
}

// Overload that mirrors the code above when the ExceptionObjectData was already retrieved from LS
BOOL IsAsyncException(const DacpExceptionObjectData & excData)
{
    if (excData.XCode != EXCEPTION_COMPLUS)
        return TRUE;

    HRESULT ehr = excData.HResult;
    for (size_t idx = 0; idx < ARRAY_SIZE(AsyncHResultValues); ++idx)
    {
        if (ehr == AsyncHResultValues[idx])
        {
            return TRUE;
        }
    }

    return FALSE;
}


#define SOS_STACKTRACE_SHOWEXPLICITFRAMES  0x00000002
size_t FormatGeneratedException (DWORD_PTR dataPtr,
    UINT bytes,
    __out_ecount_opt(bufferLength) WCHAR *wszBuffer,
    size_t bufferLength,
    BOOL bAsync,                // hardware exception if true
    BOOL bNestedCase = FALSE,
    BOOL bLineNumbers = FALSE)
{
    UINT count = bytes / sizeof(StackTraceElement);
    size_t Length = 0;

    _ASSERTE(g_targetMachine != nullptr);

    if (wszBuffer && bufferLength > 0)
    {
        wszBuffer[0] = L'\0';
    }

    // Buffer is calculated for sprintf below ("   %p %p %S\n");
    WCHAR wszLineBuffer[mdNameLen + 8 + sizeof(size_t)*2 + MAX_LONGPATH + 8];

    if (count == 0)
    {
        return 0;
    }

    if (bNestedCase)
    {
        // If we are computing the call stack for a nested exception, we
        // don't want to print the last frame, because the outer exception
        // will have that frame.
        count--;
    }

    for (UINT i = 0; i < count; i++)
    {
        StackTraceElement ste;
        MOVE (ste, dataPtr + i*sizeof(StackTraceElement));

        // ste.ip must be adjusted because of an ancient workaround in the exception
        // infrastructure. The workaround is that the exception needs to have
        // an ip address that will map to the line number where the exception was thrown.
        // (It doesn't matter that it's not a valid instruction). (see /vm/excep.cpp)
        //
        // This "counterhack" is not 100% accurate
        // The biggest issue is that PrintException must work with exception objects
        // that may not be currently active; as a consequence we cannot rely on the
        // state of some "current thread" to infer whether the IP values stored in
        // the exception object have been adjusted or not. If we could, we may examine
        // the topmost "Frame" and make the decision based on whether it's a
        // FaultingExceptionFrame or not.
        // 1. On IA64 the IP values are never adjusted by the EE so there's nothing
        //    to adjust back.
        // 2. On AMD64:
        //    (a) if the exception was an async (hardware) exception add 1 to all
        //        IP values in the exception object
        //    (b) if the exception was a managed exception (either raised by the
        //        EE or thrown by managed code) do not adjust any IP values
        // 3. On X86:
        //    (a) if the exception was an async (hardware) exception add 1 to
        //        all but the topmost IP value in the exception object
        //    (b) if the exception was a managed exception (either raised by
        //        the EE or thrown by managed code) add 1 to all IP values in
        //        the exception object
#if defined(_TARGET_AMD64_)
        if (bAsync)
        {
            ste.ip += 1;
        }
#elif defined(_TARGET_X86_)
        if (IsDbgTargetX86() && (!bAsync || i != 0))
        {
            ste.ip += 1;
        }
#endif // defined(_TARGET_AMD64_) || defined(_TARGET__X86_)

        StringOutput so;
        HRESULT Status = DumpMDInfoBuffer(ste.pFunc, SOS_STACKTRACE_SHOWADDRESSES|SOS_STACKTRACE_SHOWEXPLICITFRAMES, ste.sp, ste.ip, so);

        // If DumpMDInfoBuffer failed (due to out of memory or missing metadata),
        // or did not update so (when ste is an explicit frames), do not update wszBuffer
        if (Status == S_OK)
        {
            WCHAR filename[MAX_LONGPATH] = W("");
            ULONG linenum = 0;
            if (bLineNumbers &&
                // To get the source line number of the actual code that threw an exception, the IP needs
                // to be adjusted in certain cases.
                //
                // The IP of the stack frame points to either:
                //
                // 1) The instruction that caused a hardware exception (div by zero, null ref, etc).
                // 2) The instruction after the call to an internal runtime function (FCALL like IL_Throw,
                //    IL_Rethrow, JIT_OverFlow, etc.) that caused a software exception.
                // 3) The instruction after the call to a managed function (non-leaf node).
                //
                // #2 and #3 are the cases that need to adjust IP because they point after the call instruction
                // and may point to the next (incorrect) IL instruction/source line.  We distinguish these from
                // #1 by the bAsync flag which is set to true for hardware exceptions and that it is a leaf node
                // (i == 0).
                //
                // When the IP needs to be adjusted it is a lot simpler to decrement IP instead of trying to figure
                // out the beginning of the instruction. It is enough for GetLineByOffset to return the correct line number.
                //
                // The unmodified IP is displayed (above by DumpMDInfoBuffer) which points after the exception in most
                // cases. This means that the printed IP and the printed line number often will not map to one another
                // and this is intentional.
                SUCCEEDED(GetLineByOffset(TO_CDADDR(ste.ip), &linenum, filename, ARRAY_SIZE(filename), !bAsync || i > 0)))
            {
                swprintf_s(wszLineBuffer, ARRAY_SIZE(wszLineBuffer), W("    %s [%s @ %d]\n"), so.String(), filename, linenum);
            }
            else
            {
                swprintf_s(wszLineBuffer, ARRAY_SIZE(wszLineBuffer), W("    %s\n"), so.String());
            }

            Length += _wcslen(wszLineBuffer);

            if (wszBuffer)
            {
                wcsncat_s(wszBuffer, bufferLength, wszLineBuffer, _TRUNCATE);
            }
        }
    }

    return Length;
}

// ExtOut has an internal limit for the string size
void SosExtOutLargeString(__inout_z __inout_ecount_opt(len) WCHAR * pwszLargeString, size_t len)
{
    const size_t chunkLen = 2048;

    WCHAR *pwsz = pwszLargeString;  // beginning of a chunk
    size_t count = len/chunkLen;
    // write full chunks
    for (size_t idx = 0; idx < count; ++idx)
    {
        WCHAR *pch = pwsz + chunkLen; // after the chunk
        // zero terminate the chunk
        WCHAR ch = *pch;
        *pch = L'\0';

        ExtOut("%S", pwsz);

        // restore whacked char
        *pch = ch;

        // advance to next chunk
        pwsz += chunkLen;
    }

    // last chunk
    ExtOut("%S", pwsz);
}

DWORD_PTR GetFirstArrayElementPointer(TADDR taArray)
{
#ifdef _TARGET_WIN64_
    return taArray + sizeof(DWORD_PTR) + sizeof(DWORD) + sizeof(DWORD);
#else
    return taArray + sizeof(DWORD_PTR) + sizeof(DWORD);
#endif // _TARGET_WIN64_
}

TADDR GetStackTraceArray(CLRDATA_ADDRESS taExceptionObj, DacpObjectData *pExceptionObjData, DacpExceptionObjectData *pExcData)
{
    TADDR taStackTrace = 0;
    if (pExcData)
    {
        taStackTrace = TO_TADDR(pExcData->StackTrace);
    }
    else
    {
        int iOffset = GetObjFieldOffset (taExceptionObj, pExceptionObjData->MethodTable, W("_stackTrace"));
        if (iOffset > 0)
        {
            MOVE(taStackTrace, taExceptionObj + iOffset);
        }
    }

    if (taStackTrace)
    {
        // If the stack trace is object[], the stack trace array is actually referenced by its first element
        sos::Object objStackTrace(taStackTrace);
        TADDR stackTraceComponentMT = objStackTrace.GetComponentMT();
        if (stackTraceComponentMT == g_special_usefulGlobals.ObjectMethodTable)
        {
            DWORD_PTR arrayDataPtr = GetFirstArrayElementPointer(taStackTrace);
            MOVE(taStackTrace, arrayDataPtr);
        }
    }

    return taStackTrace;
}

HRESULT FormatException(CLRDATA_ADDRESS taObj, BOOL bLineNumbers = FALSE)
{
    HRESULT Status = S_OK;

    DacpObjectData objData;
    if ((Status=objData.Request(g_sos, taObj)) != S_OK)
    {
        ExtOut("Invalid exception object: %016llx\n", taObj);
        if (!GetGcStructuresValid())
        {
            DisplayInvalidStructuresMessage();
        }
        return Status;
    }

    // Make sure it is an exception object, and get the MT of Exception
    CLRDATA_ADDRESS exceptionMT = isExceptionObj(objData.MethodTable);
    if (exceptionMT == (TADDR)0)
    {
        ExtOut("Not a valid exception object\n");
        return Status;
    }

    DMLOut("Exception object: %s\n", DMLObject(taObj));

    if (NameForMT_s(TO_TADDR(objData.MethodTable), g_mdName, mdNameLen))
    {
        ExtOut("Exception type:   %S\n", g_mdName);
    }
    else
    {
        ExtOut("Exception type:   <Unknown>\n");
    }

    // Print basic info

    // First try to get exception object data using ISOSDacInterface2
    DacpExceptionObjectData excData;
    BOOL bGotExcData = SUCCEEDED(excData.Request(g_sos, taObj));

    // Walk the fields, printing some fields in a special way.
    // HR, InnerException, Message, StackTrace, StackTraceString

    {
        TADDR taMsg = 0;
        if (bGotExcData)
        {
            taMsg = TO_TADDR(excData.Message);
        }
        else
        {
            int iOffset = GetObjFieldOffset(taObj, objData.MethodTable, W("_message"));
            if (iOffset > 0)
            {
                MOVE (taMsg, taObj + iOffset);
            }
        }

        ExtOut("Message:          ");

        if (taMsg)
            StringObjectContent(taMsg);
        else
            ExtOut("<none>");

        ExtOut("\n");
    }

    {
        TADDR taInnerExc = 0;
        if (bGotExcData)
        {
            taInnerExc = TO_TADDR(excData.InnerException);
        }
        else
        {
            int iOffset = GetObjFieldOffset(taObj, objData.MethodTable, W("_innerException"));
            if (iOffset > 0)
            {
                MOVE (taInnerExc, taObj + iOffset);
            }
        }

        ExtOut("InnerException:   ");
        if (taInnerExc)
        {
            TADDR taMT;
            if (SUCCEEDED(GetMTOfObject(taInnerExc, &taMT)))
            {
                NameForMT_s(taMT, g_mdName, mdNameLen);
                ExtOut("%S, ", g_mdName);
                if (IsDMLEnabled())
                    DMLOut("Use <exec cmd=\"!PrintException /d %p\">!PrintException %p</exec> to see more.\n", SOS_PTR(taInnerExc), SOS_PTR(taInnerExc));
                else
                    ExtOut("Use %sprintexception %p to see more.\n", SOSPrefix, SOS_PTR(taInnerExc));
            }
            else
            {
                ExtOut("<invalid MethodTable of inner exception>");
            }
        }
        else
        {
            ExtOut("<none>\n");
        }
    }

    BOOL bAsync = bGotExcData ? IsAsyncException(excData)
                              : IsAsyncException(taObj, objData.MethodTable);

    {
        TADDR taStackTrace = GetStackTraceArray(taObj, &objData, bGotExcData ? &excData : NULL);

        ExtOut("StackTrace (generated):\n");
        if (taStackTrace)
        {
            DWORD arrayLen;
            HRESULT hr = MOVE(arrayLen, taStackTrace + sizeof(DWORD_PTR));

            if (arrayLen != 0 && hr == S_OK)
            {
                DWORD_PTR dataPtr = GetFirstArrayElementPointer(taStackTrace);
                size_t stackTraceSize = 0;
                MOVE (stackTraceSize, dataPtr);

                DWORD cbStackSize = static_cast<DWORD>(stackTraceSize * sizeof(StackTraceElement));

                if (IsRuntimeVersionAtLeast(9))
                {
                    dataPtr += sizeof(uint32_t) + sizeof(uint32_t) + sizeof(DWORD_PTR); // skip the 9.0 array header
                }
                else
                {
                    dataPtr += sizeof(size_t) + sizeof(DWORD_PTR);                      // skip the array header, then goes the data
                }

                if (stackTraceSize == 0)
                {
                    ExtOut("Unable to decipher generated stack trace\n");
                }
                else
                {
                    size_t iHeaderLength = AddExceptionHeader (NULL, 0);
                    size_t iLength = FormatGeneratedException (dataPtr, cbStackSize, NULL, 0, bAsync, FALSE, bLineNumbers);
                    WCHAR *pwszBuffer = new NOTHROW WCHAR[iHeaderLength + iLength + 1];
                    if (pwszBuffer)
                    {
                        AddExceptionHeader(pwszBuffer, iHeaderLength + 1);
                        FormatGeneratedException(dataPtr, cbStackSize, pwszBuffer + iHeaderLength, iLength + 1, bAsync, FALSE, bLineNumbers);
                        SosExtOutLargeString(pwszBuffer, iHeaderLength + iLength + 1);
                        delete[] pwszBuffer;
                    }
                    ExtOut("\n");
                }
            }
            else
            {
                ExtOut("<Not Available>\n");
            }
        }
        else
        {
            ExtOut("<none>\n");
        }
    }

    {
        TADDR taStackString;
        if (bGotExcData)
        {
            taStackString = TO_TADDR(excData.StackTraceString);
        }
        else
        {
            int iOffset = GetObjFieldOffset (taObj, objData.MethodTable, W("_stackTraceString"));
            MOVE (taStackString, taObj + iOffset);
        }

        ExtOut("StackTraceString: ");
        if (taStackString)
        {
            StringObjectContent(taStackString);
            ExtOut("\n\n"); // extra newline looks better
        }
        else
        {
            ExtOut("<none>\n");
        }
    }

    {
        DWORD hResult;
        if (bGotExcData)
        {
            hResult = excData.HResult;
        }
        else
        {
            int iOffset = GetObjFieldOffset (taObj, objData.MethodTable, W("_HResult"));
            MOVE (hResult, taObj + iOffset);
        }

        ExtOut("HResult: %lx\n", hResult);
    }

    if (isSecurityExceptionObj(objData.MethodTable) != (TADDR)0)
    {
        // We have a SecurityException Object: print out the debugString if present
        int iOffset = GetObjFieldOffset (taObj, objData.MethodTable, W("m_debugString"));
        if (iOffset > 0)
        {
            TADDR taDebugString;
            MOVE (taDebugString, taObj + iOffset);

            if (taDebugString)
            {
                ExtOut("SecurityException Message: ");
                StringObjectContent(taDebugString);
                ExtOut("\n\n"); // extra newline looks better
            }
        }
    }

    return Status;
}

DECLARE_API(PrintException)
{
    INIT_API_PROBE_MANAGED("printexception");

    BOOL dml = FALSE;
    BOOL bShowNested = FALSE;
    BOOL bLineNumbers = FALSE;
    BOOL bCCW = FALSE;
    StringHolder strObject;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-nested", &bShowNested, COBOOL, FALSE},
        {"-lines", &bLineNumbers, COBOOL, FALSE},
        {"-l", &bLineNumbers, COBOOL, FALSE},
        {"-ccw", &bCCW, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE}
    };
    CMDValue arg[] =
    {   // vptr, type
        {&strObject, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (CheckBreakingRuntimeChange())
    {
        return E_FAIL;
    }

    if (bLineNumbers)
    {
        ULONG symlines = 0;
        if (SUCCEEDED(g_ExtSymbols->GetSymbolOptions(&symlines)))
        {
            symlines &= SYMOPT_LOAD_LINES;
        }
        if (symlines == 0)
        {
            ExtOut("In order for the option -lines to enable display of source information\n"
                   "the debugger must be configured to load the line number information from\n"
                   "the symbol files. Use the \".lines; .reload\" command to achieve this.\n");
            // don't even try
            bLineNumbers = FALSE;
        }
    }

    EnableDMLHolder dmlHolder(dml);
    DWORD_PTR p_Object = (TADDR)0;
    if (nArg == 0)
    {
        if (bCCW)
        {
            ExtOut("No CCW pointer specified\n");
            return Status;
        }

        // Look at the last exception object on this thread

        CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
        DacpThreadData Thread;

        if ((threadAddr == (TADDR)0) || (Thread.Request(g_sos, threadAddr) != S_OK))
        {
            ExtOut("The current thread is unmanaged\n");
            return Status;
        }

        DWORD_PTR dwAddr = (TADDR)0;
        if ((!SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle),
                            &dwAddr,
                            sizeof(dwAddr), NULL)) || (dwAddr==(TADDR)0))
        {
            ExtOut("There is no current managed exception on this thread\n");
        }
        else
        {
            p_Object = dwAddr;
        }
    }
    else
    {
        p_Object = GetExpression(strObject.data);
        if (p_Object == 0)
        {
            if (bCCW)
            {
                ExtOut("Invalid CCW pointer %s\n", args);
            }
            else
            {
                ExtOut("Invalid exception object %s\n", args);
            }
            return E_INVALIDARG;
        }

        if (bCCW)
        {
            // check if the address is a CCW pointer and then
            // get the exception object from it
            DacpCCWData ccwData;
            if (ccwData.Request(g_sos, p_Object) == S_OK)
            {
                p_Object = TO_TADDR(ccwData.managedObject);
            }
        }
    }

    if (p_Object)
    {
        FormatException(TO_CDADDR(p_Object), bLineNumbers);
    }

    // Are there nested exceptions?
    CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
    DacpThreadData Thread;

    if ((threadAddr == (TADDR)0) || (Thread.Request(g_sos, threadAddr) != S_OK))
    {
        ExtOut("The current thread is unmanaged\n");
        return E_INVALIDARG;
    }

    if (Thread.firstNestedException)
    {
        if (!bShowNested)
        {
            ExtOut("There are nested exceptions on this thread. Run with -nested for details\n");
            return Status;
        }

        CLRDATA_ADDRESS currentNested = Thread.firstNestedException;
        do
        {
            CLRDATA_ADDRESS obj = 0, next = 0;
            Status = g_sos->GetNestedExceptionData(currentNested, &obj, &next);

            if (Status != S_OK)
            {
                ExtOut("Error retrieving nested exception info %p\n", SOS_PTR(currentNested));
                return Status;
            }

            if (IsInterrupt())
            {
                ExtOut("<aborted>\n");
                return Status;
            }

            ExtOut("\nNested exception -------------------------------------------------------------\n");
            Status = FormatException(obj, bLineNumbers);
            if (Status != S_OK)
            {
                return Status;
            }

            currentNested = next;
        }
        while(currentNested != (TADDR)0);
    }
    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of an object from a  *
*    given address
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpVC)
{
    INIT_API_PROBE_MANAGED("dumpvc");
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR p_MT = (TADDR)0;
    DWORD_PTR p_Object = (TADDR)0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE}
    };
    CMDValue arg[] =
    {   // vptr, type
        {&p_MT, COHEX},
        {&p_Object, COHEX},
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (nArg!=2)
    {
        ExtOut("Usage: %sdumpvc <Method Table> <Value object start addr>\n", SOSPrefix);
        return Status;
    }

    if (!IsMethodTable(p_MT))
    {
        ExtOut("Not a managed object\n");
        return S_OK;
    }

    return PrintVC(p_MT, p_Object);
}

#ifndef FEATURE_PAL

#ifdef FEATURE_COMINTEROP

DECLARE_API(DumpRCW)
{
    INIT_API();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    BOOL dml = FALSE;
    StringHolder strObject;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&strObject, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (nArg == 0)
    {
        ExtOut("Missing RCW address\n");
        return Status;
    }
    else
    {
        DWORD_PTR p_RCW = GetExpression(strObject.data);
        if (p_RCW == 0)
        {
            ExtOut("Invalid RCW %s\n", args);
        }
        else
        {
            ReleaseHolder<ISOSDacInterface10> sos10;
            BOOL isComWrappersRCW = FALSE;
            if (SUCCEEDED(Status = g_sos->QueryInterface(__uuidof(ISOSDacInterface10), &sos10))
                && SUCCEEDED(sos10->IsComWrappersRCW(p_RCW, &isComWrappersRCW))
                && isComWrappersRCW)
            {
                CLRDATA_ADDRESS identity;
                if (SUCCEEDED(Status = sos10->GetComWrappersRCWData(p_RCW, &identity)))
                {
                    ExtOut("ComWrappers RCW found\n");
                    DMLOut("Identity:       %p\n", SOS_PTR(identity));
                }
                else
                {
                    ExtOut("Error requesting RCW data\n");
                    return Status;
                }
            }
            else
            {
                DacpRCWData rcwData;
                if ((Status = rcwData.Request(g_sos, p_RCW)) != S_OK)
                {
                    ExtOut("Error requesting RCW data\n");
                    return Status;
                }
                BOOL isDCOMProxy;
                if (FAILED(rcwData.IsDCOMProxy(g_sos, p_RCW, &isDCOMProxy)))
                {
                    isDCOMProxy = FALSE;
                }

                DMLOut("Managed object:             %s\n", DMLObject(rcwData.managedObject));
                DMLOut("Creating thread:            %p\n", SOS_PTR(rcwData.creatorThread));
                ExtOut("IUnknown pointer:           %p\n", SOS_PTR(rcwData.unknownPointer));
                ExtOut("COM Context:                %p\n", SOS_PTR(rcwData.ctxCookie));
                ExtOut("Managed ref count:          %d\n", rcwData.refCount);
                ExtOut("IUnknown V-table pointer :  %p (captured at RCW creation time)\n", SOS_PTR(rcwData.vtablePtr));

                ExtOut("Flags:                      %s%s%s%s%s%s%s%s\n",
                    (rcwData.isDisconnected? "IsDisconnected " : ""),
                    (rcwData.supportsIInspectable? "SupportsIInspectable " : ""),
                    (rcwData.isAggregated? "IsAggregated " : ""),
                    (rcwData.isContained? "IsContained " : ""),
                    (rcwData.isJupiterObject? "IsJupiterObject " : ""),
                    (rcwData.isFreeThreaded? "IsFreeThreaded " : ""),
                    (rcwData.identityPointer == TO_CDADDR(p_RCW)? "IsUnique " : ""),
                    (isDCOMProxy ? "IsDCOMProxy " : "")
                    );

                // Jupiter data hidden by default
                if (rcwData.isJupiterObject)
                {
                    ExtOut("IJupiterObject:    %p\n", SOS_PTR(rcwData.jupiterObject));
                }

                ExtOut("COM interface pointers:\n");

                ArrayHolder<DacpCOMInterfacePointerData> pArray = new NOTHROW DacpCOMInterfacePointerData[rcwData.interfaceCount];
                if (pArray == NULL)
                {
                    ReportOOM();
                    return Status;
                }

                if ((Status = g_sos->GetRCWInterfaces(p_RCW, rcwData.interfaceCount, pArray, NULL)) != S_OK)
                {
                    ExtOut("Error requesting COM interface pointers\n");
                    return Status;
                }

                ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s Type\n", "IP", "Context", "MT");
                for (int i = 0; i < rcwData.interfaceCount; i++)
                {
                    // Ignore any NULL MethodTable interface cache. At this point only IJupiterObject
                    // is saved as NULL MethodTable at first slot, and we've already printed outs its
                    // value earlier.
                    if (pArray[i].methodTable == NULL)
                        continue;

                    NameForMT_s(TO_TADDR(pArray[i].methodTable), g_mdName, mdNameLen);

                    DMLOut("%p %p %s %S\n", SOS_PTR(pArray[i].interfacePtr), SOS_PTR(pArray[i].comContext), DMLMethodTable(pArray[i].methodTable), g_mdName);
                }
            }
        }
    }

    return Status;
}

DECLARE_API(DumpCCW)
{
    INIT_API();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    BOOL dml = FALSE;
    StringHolder strObject;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&strObject, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (nArg == 0)
    {
        ExtOut("Missing CCW address\n");
        return Status;
    }


    DWORD_PTR p_CCW = GetExpression(strObject.data);
    if (p_CCW == 0)
    {
        ExtOut("Invalid CCW %s\n", args);
    }
    else
    {
        ReleaseHolder<ISOSDacInterface10> sos10;
        BOOL isComWrappersCCW = FALSE;
        if (SUCCEEDED(Status = g_sos->QueryInterface(__uuidof(ISOSDacInterface10), &sos10))
            && SUCCEEDED(sos10->IsComWrappersCCW(p_CCW, &isComWrappersCCW))
            && isComWrappersCCW)
        {
            CLRDATA_ADDRESS managedObject;
            int refCount;
            if (SUCCEEDED(Status = sos10->GetComWrappersCCWData(p_CCW, &managedObject, &refCount)))
            {
                ExtOut("ComWrappers CCW found\n");
                DMLOut("Managed object:    %s\n", DMLObject(managedObject));
                ExtOut("Ref count:         %d\n", refCount);
            }
            else
            {
                ExtOut("Error requesting CCW data\n");
                return Status;
            }
        }
        else
        {
            DacpCCWData ccwData;
            if ((Status = ccwData.Request(g_sos, p_CCW)) != S_OK)
            {
                ExtOut("Error requesting CCW data\n");
                return Status;
            }

            if (ccwData.ccwAddress != p_CCW)
                ExtOut("CCW:               %p\n", SOS_PTR(ccwData.ccwAddress));

            DMLOut("Managed object:    %s\n", DMLObject(ccwData.managedObject));
            ExtOut("Outer IUnknown:    %p\n", SOS_PTR(ccwData.outerIUnknown));
            ExtOut("Ref count:         %d%s\n", ccwData.refCount, ccwData.isNeutered ? " (NEUTERED)" : "");
            ExtOut("Flags:             %s%s\n",
                (ccwData.isExtendsCOMObject? "IsExtendsCOMObject " : ""),
                (ccwData.isAggregated? "IsAggregated " : "")
                );

            // Jupiter information hidden by default
            if (ccwData.jupiterRefCount > 0)
            {
                ExtOut("Jupiter ref count: %d%s%s%s%s\n",
                    ccwData.jupiterRefCount,
                    (ccwData.isPegged || ccwData.isGlobalPegged) ? ", Pegged by" : "",
                    ccwData.isPegged ? " Jupiter " : "",
                    (ccwData.isPegged && ccwData.isGlobalPegged) ? "&" : "",
                    ccwData.isGlobalPegged ? " CLR " : ""
                    );
            }

            ExtOut("RefCounted Handle: %p%s\n",
                SOS_PTR(ccwData.handle),
                (ccwData.hasStrongRef ? " (STRONG)" : " (WEAK)"));

            ExtOut("COM interface pointers:\n");

            ArrayHolder<DacpCOMInterfacePointerData> pArray = new NOTHROW DacpCOMInterfacePointerData[ccwData.interfaceCount];
            if (pArray == NULL)
            {
                ReportOOM();
                return Status;
            }

            if ((Status = g_sos->GetCCWInterfaces(p_CCW, ccwData.interfaceCount, pArray, NULL)) != S_OK)
            {
                ExtOut("Error requesting COM interface pointers\n");
                return Status;
            }

            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s Type\n", "IP", "MT", "Type");
            for (int i = 0; i < ccwData.interfaceCount; i++)
            {
                if (pArray[i].methodTable == NULL)
                {
                    wcscpy_s(g_mdName, mdNameLen, W("IDispatch/IUnknown"));
                }
                else
                {
                    NameForMT_s(TO_TADDR(pArray[i].methodTable), g_mdName, mdNameLen);
                }

                DMLOut("%p %s %S\n", SOS_PTR(pArray[i].interfacePtr), DMLMethodTable(pArray[i].methodTable), g_mdName);
            }
        }
    }

    return Status;
}

#endif // FEATURE_COMINTEROP

#ifdef _DEBUG
/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a PermissionSet   *
*    from a given address.                                             *
*                                                                      *
\**********************************************************************/
/*
    COMMAND: dumppermissionset.
    !DumpPermissionSet <PermissionSet object address>

    This command allows you to examine a PermissionSet object. Note that you can
    also use DumpObj such an object in greater detail. DumpPermissionSet attempts
    to extract all the relevant information from a PermissionSet that you might be
    interested in when performing Code Access Security (CAS) related debugging.

    Here is a simple PermissionSet object:

    0:000> !DumpPermissionSet 014615f4
    PermissionSet object: 014615f4
    Unrestricted: TRUE

    Note that this is an unrestricted PermissionSet object that does not contain
    any individual permissions.

    Here is another example of a PermissionSet object, one that is not unrestricted
    and contains a single permission:

    0:003> !DumpPermissionSet 01469fa8
    PermissionSet object: 01469fa8
    Unrestricted: FALSE
    Name: System.Security.Permissions.ReflectionPermission
    MethodTable: 5b731308
    EEClass: 5b7e0d78
    Size: 12(0xc) bytes
     (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_32\mscorlib\2.0.
    0.0__b77a5c561934e089\mscorlib.dll)

    Fields:
          MT    Field   Offset                 Type VT     Attr    Value Name
    5b73125c  4001d66        4         System.Int32  0 instance        2 m_flags

    Here is another example of an unrestricted PermissionSet, one that contains
    several permissions. The numbers in parentheses before each Permission object
    represents the index of that Permission in the PermissionSet.

    0:003> !DumpPermissionSet 01467bd8
    PermissionSet object: 01467bd8
    Unrestricted: FALSE
    [1] 01467e90
        Name: System.Security.Permissions.FileDialogPermission
        MethodTable: 5b73023c
        EEClass: 5b7dfb18
        Size: 12(0xc) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_32\mscorlib\2.0.0.0__b77a5c561934e089\mscorlib.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5b730190  4001cc2        4         System.Int32  0 instance        1 access
    [4] 014682a8
        Name: System.Security.Permissions.ReflectionPermission
        MethodTable: 5b731308
        EEClass: 5b7e0d78
        Size: 12(0xc) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_32\mscorlib\2.0.0.0__b77a5c561934e089\mscorlib.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5b73125c  4001d66        4         System.Int32  0 instance        0 m_flags
    [17] 0146c060
        Name: System.Diagnostics.EventLogPermission
        MethodTable: 569841c4
        EEClass: 56a03e5c
        Size: 28(0x1c) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5b6d65d4  4003078        4      System.Object[]  0 instance 0146c190 tagNames
        5b6c9ed8  4003079        8          System.Type  0 instance 0146c17c permissionAccessType
        5b6cd928  400307a       10       System.Boolean  0 instance        0 isUnrestricted
        5b6c45f8  400307b        c ...ections.Hashtable  0 instance 0146c1a4 rootTable
        5b6c090c  4003077      bfc        System.String  0   static 00000000 computerName
        56984434  40030e7       14 ...onEntryCollection  0 instance 00000000 innerCollection
    [18] 0146ceb4
        Name: System.Net.WebPermission
        MethodTable: 5696dfc4
        EEClass: 569e256c
        Size: 20(0x14) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5b6cd928  400238e        c       System.Boolean  0 instance        0 m_Unrestricted
        5b6cd928  400238f        d       System.Boolean  0 instance        0 m_UnrestrictedConnect
        5b6cd928  4002390        e       System.Boolean  0 instance        0 m_UnrestrictedAccept
        5b6c639c  4002391        4 ...ections.ArrayList  0 instance 0146cf3c m_connectList
        5b6c639c  4002392        8 ...ections.ArrayList  0 instance 0146cf54 m_acceptList
        569476f8  4002393      8a4 ...Expressions.Regex  0   static 00000000 s_MatchAllRegex
    [19] 0146a5fc
        Name: System.Net.DnsPermission
        MethodTable: 56966408
        EEClass: 569d3c08
        Size: 12(0xc) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5b6cd928  4001d2c        4       System.Boolean  0 instance        1 m_noRestriction
    [20] 0146d8ec
        Name: System.Web.AspNetHostingPermission
        MethodTable: 569831bc
        EEClass: 56a02ccc
        Size: 12(0xc) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        56983090  4003074        4         System.Int32  0 instance      600 _level
    [21] 0146e394
        Name: System.Net.NetworkInformation.NetworkInformationPermission
        MethodTable: 5697ac70
        EEClass: 569f7104
        Size: 16(0x10) bytes
         (C:\WINDOWS\Microsoft.NET\Framework\v2.0.x86chk\assembly\GAC_MSIL\System\2.0.0.0__b77a5c561934e089\System.dll)
        Fields:
              MT    Field   Offset                 Type VT     Attr    Value Name
        5697ab38  4002c34        4         System.Int32  0 instance        0 access
        5b6cd928  4002c35        8       System.Boolean  0 instance        0 unrestricted


    The abbreviation !dps can be used for brevity.

    \\
*/
DECLARE_API(DumpPermissionSet)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    DWORD_PTR p_Object = NULL;

    CMDValue arg[] =
    {
        {&p_Object, COHEX}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg!=1)
    {
        ExtOut("Usage: %sdumppermissionset <PermissionSet object addr>\n", SOSPrefix);
        return Status;
    }


    return PrintPermissionSet(p_Object);
}

#endif // _DEBUG
#endif // FEATURE_PAL

void PrintGCStat(HeapStat *inStat, const char* label=NULL)
{
    if (inStat)
    {
        bool sorted = false;
        try
        {
            inStat->Sort();
            sorted = true;
            inStat->Print(label);
        }
        catch(...)
        {
            ExtOut("Exception occurred while trying to %s the GC stats.\n", sorted ? "print" : "sort");
        }

        inStat->Delete();
    }
}

namespace sos
{
    class FragmentationBlock
    {
    public:
        FragmentationBlock(TADDR addr, size_t size, TADDR next, TADDR mt)
            : mAddress(addr), mSize(size), mNext(next), mNextMT(mt)
        {
        }

        inline TADDR GetAddress() const
        {
            return mAddress;
        }
        inline size_t GetSize() const
        {
            return mSize;
        }

        inline TADDR GetNextObject() const
        {
            return mNext;
        }

        inline TADDR GetNextMT() const
        {
            return mNextMT;
        }

    private:
        TADDR mAddress;
        size_t mSize;
        TADDR mNext;
        TADDR mNextMT;
    };
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function dumps what is in the syncblock cache.  By default   *
*    it dumps all active syncblocks.  Using -all to dump all syncblocks
*                                                                      *
\**********************************************************************/
DECLARE_API(SyncBlk)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    BOOL bDumpAll = FALSE;
    size_t nbAsked = 0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-all", &bDumpAll, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE}
    };
    CMDValue arg[] =
    {   // vptr, type
        {&nbAsked, COSIZE_T}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    DacpSyncBlockData syncBlockData;
    if (syncBlockData.Request(g_sos,1) != S_OK)
    {
        ExtOut("Error requesting SyncBlk data\n");
        return Status;
    }

    DWORD dwCount = syncBlockData.SyncBlockCount;

    ExtOut("Index" WIN64_8SPACES " SyncBlock MonitorHeld Recursion Owning Thread Info" WIN64_8SPACES "  SyncBlock Owner\n");
    ULONG freeCount = 0;
    ULONG CCWCount = 0;
    ULONG RCWCount = 0;
    ULONG CFCount = 0;
    for (DWORD nb = 1; nb <= dwCount; nb++)
    {
        if (IsInterrupt())
            return Status;

        if (nbAsked && nb != nbAsked)
        {
            continue;
        }

        if (syncBlockData.Request(g_sos,nb) != S_OK)
        {
            ExtOut("SyncBlock %d is invalid%s\n", nb,
                (nb != nbAsked) ? ", continuing..." : "");
            continue;
        }

        BOOL bPrint = (bDumpAll || nb == nbAsked || (syncBlockData.MonitorHeld > 0 && !syncBlockData.bFree));

        if (bPrint)
        {
            ExtOut("%5d ", nb);
            if (!syncBlockData.bFree || nb != nbAsked)
            {
                ExtOut("%p  ", SOS_PTR(syncBlockData.SyncBlockPointer));
                ExtOut("%11d ", syncBlockData.MonitorHeld);
                ExtOut("%9d ", syncBlockData.Recursion);
                ExtOut("%p ", SOS_PTR(syncBlockData.HoldingThread));

                if (syncBlockData.HoldingThread == ~0ul)
                {
                    ExtOut(" orphaned ");
                }
                else if (syncBlockData.HoldingThread != (TADDR)0)
                {
                    DacpThreadData Thread;
                    if ((Status = Thread.Request(g_sos, syncBlockData.HoldingThread)) != S_OK)
                    {
                        ExtOut("Failed to request Thread at %p\n", SOS_PTR(syncBlockData.HoldingThread));
                        return Status;
                    }

                    DMLOut(DMLThreadID(Thread.osThreadId));
                    ULONG id;
                    if (g_ExtSystem->GetThreadIdBySystemId(Thread.osThreadId, &id) == S_OK)
                    {
                        ExtOut("%4d ", id);
                    }
                    else
                    {
                        ExtOut(" XXX ");
                    }
                }
                else
                {
                    ExtOut("    none  ");
                }

                if (syncBlockData.bFree)
                {
                    ExtOut("  %8d", 0);    // TODO: do we need to print the free synctable list?
                }
                else
                {
                    sos::Object obj = TO_TADDR(syncBlockData.Object);
                    DMLOut("  %s %S", DMLObject(syncBlockData.Object), obj.GetTypeName());
                }
            }
        }

        if (syncBlockData.bFree)
        {
            freeCount ++;
            if (bPrint) {
                ExtOut(" Free");
            }
        }
        else
        {
#ifdef FEATURE_COMINTEROP
            if (syncBlockData.COMFlags) {
                switch (syncBlockData.COMFlags) {
                case SYNCBLOCKDATA_COMFLAGS_CCW:
                    CCWCount ++;
                    break;
                case SYNCBLOCKDATA_COMFLAGS_RCW:
                    RCWCount ++;
                    break;
                case SYNCBLOCKDATA_COMFLAGS_CF:
                    CFCount ++;
                    break;
                }
            }
#endif // FEATURE_COMINTEROP
        }

        if (syncBlockData.MonitorHeld > 1)
        {
            // TODO: implement this
            /*
            ExtOut(" ");
            DWORD_PTR pHead = (DWORD_PTR)vSyncBlock.m_Link.m_pNext;
            DWORD_PTR pNext = pHead;
            Thread vThread;

            while (1)
            {
                if (IsInterrupt())
                    return Status;
                DWORD_PTR pWaitEventLink = pNext - offsetLinkSB;
                WaitEventLink vWaitEventLink;
                vWaitEventLink.Fill(pWaitEventLink);
                if (!CallStatus) {
                    break;
                }
                DWORD_PTR dwAddr = (DWORD_PTR)vWaitEventLink.m_Thread;
                ExtOut("%x ", dwAddr);
                vThread.Fill (dwAddr);
                if (!CallStatus) {
                    break;
                }
                if (bPrint)
                    DMLOut("%s,", DMLThreadID(vThread.m_OSThreadId));
                pNext = (DWORD_PTR)vWaitEventLink.m_LinkSB.m_pNext;
                if (pNext == 0)
                    break;
            }
            */
        }

        if (bPrint)
            ExtOut("\n");
    }

    ExtOut("-----------------------------\n");
    ExtOut("Total           %d\n", dwCount);
#ifdef FEATURE_COMINTEROP
    ExtOut("CCW             %d\n", CCWCount);
    ExtOut("RCW             %d\n", RCWCount);
    ExtOut("ComClassFactory %d\n", CFCount);
#endif
    ExtOut("Free            %d\n", freeCount);

    return Status;
}

#ifdef FEATURE_COMINTEROP
struct VisitRcwArgs
{
    BOOL bDetail;
    UINT MTACount;
    UINT STACount;
    ULONG FTMCount;
};

void VisitRcw(CLRDATA_ADDRESS RCW,CLRDATA_ADDRESS Context,CLRDATA_ADDRESS Thread, BOOL bIsFreeThreaded, LPVOID token)
{
    VisitRcwArgs *pArgs = (VisitRcwArgs *) token;

    if (pArgs->bDetail)
    {
        if (pArgs->MTACount == 0 && pArgs->STACount == 0 && pArgs->FTMCount == 0)
        {
            // First time, print a header
            ExtOut("RuntimeCallableWrappers (RCW) to be cleaned:\n");
            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s Apartment\n",
                "RCW", "CONTEXT", "THREAD");
        }
        LPCSTR szThreadApartment;
        if (bIsFreeThreaded)
        {
            szThreadApartment = "(FreeThreaded)";
            pArgs->FTMCount++;
        }
        else if (Thread == NULL)
        {
            szThreadApartment = "(MTA)";
            pArgs->MTACount++;
        }
        else
        {
            szThreadApartment = "(STA)";
            pArgs->STACount++;
        }

        ExtOut("%" POINTERSIZE "p %" POINTERSIZE "p %" POINTERSIZE "p %9s\n",
            SOS_PTR(RCW),
            SOS_PTR(Context),
            SOS_PTR(Thread),
            szThreadApartment);
    }
}

DECLARE_API(RCWCleanupList)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    DWORD_PTR p_CleanupList = GetExpression(args);

    VisitRcwArgs travArgs;
    ZeroMemory(&travArgs,sizeof(VisitRcwArgs));
    travArgs.bDetail = TRUE;

    // We need to detect when !RCWCleanupList is called with an expression which evaluates to 0
    // (to print out an Invalid parameter message), but at the same time we need to allow an
    // empty argument list which would result in p_CleanupList equaling 0.
    if (p_CleanupList || strlen(args) == 0)
    {
        HRESULT hr = g_sos->TraverseRCWCleanupList(p_CleanupList, (VISITRCWFORCLEANUP)VisitRcw, &travArgs);

        if (SUCCEEDED(hr))
        {
            ExtOut("Free-Threaded Interfaces to be released: %d\n", travArgs.FTMCount);
            ExtOut("MTA Interfaces to be released: %d\n", travArgs.MTACount);
            ExtOut("STA Interfaces to be released: %d\n", travArgs.STACount);
        }
        else
        {
            ExtOut("An error occurred while traversing the cleanup list.\n");
        }
    }
    else
    {
        ExtOut("Invalid parameter %s\n", args);
    }

    return Status;
}
#endif // FEATURE_COMINTEROP

enum {
    // These are the values set in m_dwTransientFlags.
    // Note that none of these flags survive a prejit save/restore.

    MODULE_IS_TENURED           = 0x00000001,   // Set once we know for sure the Module will not be freed until the appdomain itself exits
    // unused                   = 0x00000002,
    CLASSES_FREED               = 0x00000004,
    IS_EDIT_AND_CONTINUE        = 0x00000008,   // is EnC Enabled for this module

    IS_PROFILER_NOTIFIED        = 0x00000010,
    IS_ETW_NOTIFIED             = 0x00000020,

    //
    // Note: the order of these must match the order defined in
    // cordbpriv.h for DebuggerAssemblyControlFlags. The three
    // values below should match the values defined in
    // DebuggerAssemblyControlFlags when shifted right
    // DEBUGGER_INFO_SHIFT bits.
    //
    DEBUGGER_USER_OVERRIDE_PRIV = 0x00000400,
    DEBUGGER_ALLOW_JIT_OPTS_PRIV= 0x00000800,
    DEBUGGER_TRACK_JIT_INFO_PRIV= 0x00001000,
    DEBUGGER_ENC_ENABLED_PRIV   = 0x00002000,   // this is what was attempted to be set.  IS_EDIT_AND_CONTINUE is actual result.
    DEBUGGER_PDBS_COPIED        = 0x00004000,
    DEBUGGER_IGNORE_PDBS        = 0x00008000,
    DEBUGGER_INFO_MASK_PRIV     = 0x0000Fc00,
    DEBUGGER_INFO_SHIFT_PRIV    = 10,

    // Used to indicate that this module has had it's IJW fixups properly installed.
    IS_IJW_FIXED_UP             = 0x00080000,
    IS_BEING_UNLOADED           = 0x00100000,

    // Used to indicate that the module is loaded sufficiently for generic candidate instantiations to work
    MODULE_READY_FOR_TYPELOAD  = 0x00200000,

    // Used during NGen only
    TYPESPECS_TRIAGED           = 0x40000000,
    MODULE_SAVED                = 0x80000000,
};

void ModuleMapTraverse(UINT index, CLRDATA_ADDRESS methodTable, LPVOID token)
{
    ULONG32 rid = (ULONG32)(size_t)token;
    NameForMT_s(TO_TADDR(methodTable), g_mdName, mdNameLen);

    DMLOut("%s 0x%08x %S\n", DMLMethodTable(methodTable), (ULONG32)TokenFromRid(rid, index), g_mdName);
}


/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a Module          *
*    for a given address                                               *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpModule)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();


    DWORD_PTR p_ModuleAddr = (TADDR)0;
    BOOL bMethodTables = FALSE;
    BOOL bProfilerModified = FALSE;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-mt", &bMethodTables, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
        {"-prof", &bProfilerModified, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&p_ModuleAddr, COHEX}
    };

    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg != 1)
    {
        ExtOut("Usage: DumpModule [-mt] <Module Address>\n");
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    DacpModuleData module;
    if ((Status=module.Request(g_sos, TO_CDADDR(p_ModuleAddr)))!=S_OK)
    {
        ExtOut("Fail to fill Module %p\n", SOS_PTR(p_ModuleAddr));
        return Status;
    }


    WCHAR FileName[MAX_LONGPATH];
    FileNameForModule (&module, FileName);
    ExtOut("Name: %S\n", FileName[0] ? FileName : W("Unknown Module"));

    ExtOut("Attributes:              ");
    if (module.bIsPEFile)
        ExtOut("PEFile ");
    if (module.bIsReflection)
        ExtOut("Reflection ");

    ToRelease<IXCLRDataModule> dataModule;
    if (SUCCEEDED(g_sos->GetModule(TO_CDADDR(p_ModuleAddr), &dataModule)))
    {
        DacpGetModuleData moduleData;
        if (SUCCEEDED(moduleData.Request(dataModule)))
        {
            if (moduleData.IsDynamic)
                ExtOut("IsDynamic ");
            if (moduleData.IsInMemory)
                ExtOut("IsInMemory ");
            if (moduleData.IsFileLayout)
                ExtOut("IsFileLayout ");
        }
    }
    ExtOut("\n");

    ExtOut("TransientFlags:          %08x ", module.dwTransientFlags);
    if (module.dwTransientFlags & IS_EDIT_AND_CONTINUE)
        ExtOut("IS_EDIT_AND_CONTINUE");
    ExtOut("\n");

    DMLOut("Assembly:                %s\n", DMLAssembly(module.Assembly));

    ExtOut("BaseAddress:             %p\n", SOS_PTR(module.ilBase));
    ExtOut("LoaderHeap:              %p\n", SOS_PTR(module.LoaderAllocator));
    ExtOut("TypeDefToMethodTableMap: %p\n", SOS_PTR(module.TypeDefToMethodTableMap));
    ExtOut("TypeRefToMethodTableMap: %p\n", SOS_PTR(module.TypeRefToMethodTableMap));
    ExtOut("MethodDefToDescMap:      %p\n", SOS_PTR(module.MethodDefToDescMap));
    ExtOut("FieldDefToDescMap:       %p\n", SOS_PTR(module.FieldDefToDescMap));
    ExtOut("MemberRefToDescMap:      %p\n", SOS_PTR(module.MemberRefToDescMap));
    ExtOut("FileReferencesMap:       %p\n", SOS_PTR(module.FileReferencesMap));
    ExtOut("AssemblyReferencesMap:   %p\n", SOS_PTR(module.ManifestModuleReferencesMap));


    if (module.ilBase && module.metadataStart)
        ExtOut("MetaData start address:  %p (%d bytes)\n", SOS_PTR(module.metadataStart), module.metadataSize);

    if (bMethodTables)
    {
        ExtOut("\nTypes defined in this module\n\n");
        ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %s\n", "MT", "TypeDef", "Name");

        ExtOut("------------------------------------------------------------------------------\n");
        g_sos->TraverseModuleMap(TYPEDEFTOMETHODTABLE, TO_CDADDR(p_ModuleAddr), ModuleMapTraverse, (LPVOID)mdTypeDefNil);

        ExtOut("\nTypes referenced in this module\n\n");
        ExtOut("%" POINTERSIZE "s   %" POINTERSIZE "s %s\n", "MT", "TypeRef", "Name");

        ExtOut("------------------------------------------------------------------------------\n");
        g_sos->TraverseModuleMap(TYPEREFTOMETHODTABLE, TO_CDADDR(p_ModuleAddr), ModuleMapTraverse, (LPVOID)mdTypeDefNil);
    }

    if (bProfilerModified)
    {
        CLRDATA_ADDRESS methodDescs[kcMaxMethodDescsForProfiler];
        int cMethodDescs;

        ReleaseHolder<ISOSDacInterface7> sos7;
        if (SUCCEEDED(g_sos->QueryInterface(__uuidof(ISOSDacInterface7), &sos7)) &&
            SUCCEEDED(sos7->GetMethodsWithProfilerModifiedIL(TO_CDADDR(p_ModuleAddr),
                                                             methodDescs,
                                                             kcMaxMethodDescsForProfiler,
                                                             &cMethodDescs)))
        {
            if (cMethodDescs > 0)
            {
                ExtOut("\nMethods in this module with profiler modified IL:\n");
                for (int i = 0; i < cMethodDescs; ++i)
                {
                    CLRDATA_ADDRESS md = methodDescs[i];

                    DMLOut("MethodDesc: %s ", DMLMethodDesc(md));

                    // Easiest to get full parameterized method name from ..::GetMethodName
                    if (g_sos->GetMethodDescName(md, mdNameLen, g_mdName, NULL) == S_OK)
                    {
                        ExtOut("Name: %S", g_mdName);
                    }

                    struct DacpProfilerILData ilData;
                    if (SUCCEEDED(sos7->GetProfilerModifiedILInformation(md, &ilData)))
                    {
                        if (ilData.type == DacpProfilerILData::ILModified)
                        {
                            ExtOut(" (IL Modified)");
                        }
                        else if (ilData.type == DacpProfilerILData::ReJITModified)
                        {
                            ExtOut(" (ReJIT Modified)");
                        }
                    }

                    ExtOut("\n");
                }

                if (cMethodDescs == kcMaxMethodDescsForProfiler)
                {
                    ExtOut("Profiler modified methods truncated, reached max value.\n");
                }
            }
            else
            {
                ExtOut("\nThis module has no methods with profiler modified IL.\n");
            }
        }
        else
        {
            ExtOut("\nThis runtime version does not support listing the profiler modified functions.\n");
        }
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a Domain          *
*    for a given address                                               *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpDomain)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR p_DomainAddr = 0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&p_DomainAddr, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    DacpAppDomainStoreData adsData;
    if ((Status=adsData.Request(g_sos))!=S_OK)
    {
        ExtOut("Unable to get AppDomain information\n");
        return Status;
    }

    if (p_DomainAddr)
    {
        DacpAppDomainData appDomain1;
        if ((Status=appDomain1.Request(g_sos, TO_CDADDR(p_DomainAddr)))!=S_OK)
        {
            ExtOut("Fail to fill AppDomain\n");
            return Status;
        }

        ExtOut("--------------------------------------\n");

        if (p_DomainAddr == adsData.sharedDomain)
        {
            DMLOut("Shared Domain:      %s\n", DMLDomain(adsData.sharedDomain));
        }
        else if (p_DomainAddr == adsData.systemDomain)
        {
            DMLOut("System Domain:      %s\n", DMLDomain(adsData.systemDomain));
        }
        else
        {
            DMLOut("Domain %d:%s          %s\n", appDomain1.dwId, (appDomain1.dwId >= 10) ? "" : " ", DMLDomain(p_DomainAddr));
        }

        DomainInfo(&appDomain1);
        return Status;
    }

    ExtOut("--------------------------------------\n");
    DMLOut("System Domain:      %s\n", DMLDomain(adsData.systemDomain));
    DacpAppDomainData appDomain;
    if ((Status=appDomain.Request(g_sos,adsData.systemDomain))!=S_OK)
    {
        ExtOut("Unable to get system domain info.\n");
        return Status;
    }
    DomainInfo(&appDomain);

    if (adsData.sharedDomain != (TADDR)0)
    {
        ExtOut("--------------------------------------\n");
        DMLOut("Shared Domain:      %s\n", DMLDomain(adsData.sharedDomain));
        if ((Status=appDomain.Request(g_sos, adsData.sharedDomain))!=S_OK)
        {
            ExtOut("Unable to get shared domain info\n");
            return Status;
        }
        DomainInfo(&appDomain);
    }

    ArrayHolder<CLRDATA_ADDRESS> pArray = new NOTHROW CLRDATA_ADDRESS[adsData.DomainCount];
    if (pArray==NULL)
    {
        ReportOOM();
        return Status;
    }

    if ((Status=g_sos->GetAppDomainList(adsData.DomainCount, pArray, NULL))!=S_OK)
    {
        ExtOut("Unable to get array of AppDomains\n");
        return Status;
    }

    for (int n=0;n<adsData.DomainCount;n++)
    {
        if (IsInterrupt())
            break;

        if ((Status=appDomain.Request(g_sos, pArray[n])) != S_OK)
        {
            ExtOut("Failed to get appdomain %p, error %lx\n", SOS_PTR(pArray[n]), Status);
            return Status;
        }

        ExtOut("--------------------------------------\n");
        DMLOut("Domain %d:%s          %s\n", appDomain.dwId, (appDomain.dwId >= 10) ? "" : " ", DMLDomain(pArray[n]));
        DomainInfo(&appDomain);
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the contents of a Assembly        *
*    for a given address                                               *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpAssembly)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR p_AssemblyAddr = 0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&p_AssemblyAddr, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    if (p_AssemblyAddr == 0)
    {
        ExtOut("Invalid Assembly %s\n", args);
        return Status;
    }

    DacpAssemblyData Assembly;
    if ((Status=Assembly.Request(g_sos, TO_CDADDR(p_AssemblyAddr)))!=S_OK)
    {
        ExtOut("Fail to fill Assembly\n");
        return Status;
    }
    DMLOut("Parent Domain:      %s\n", DMLDomain(Assembly.ParentDomain));
    if (g_sos->GetAssemblyName(TO_CDADDR(p_AssemblyAddr), mdNameLen, g_mdName, NULL)==S_OK)
        ExtOut("Name:               %S\n", g_mdName);
    else
        ExtOut("Name:               Unknown\n");

    AssemblyInfo(&Assembly);
    return Status;
}


String GetHostingCapabilities(DWORD hostConfig)
{
    String result;

    bool bAnythingPrinted = false;

#define CHK_AND_PRINT(hType,hStr)                                \
    if (hostConfig & (hType)) {                                  \
        if (bAnythingPrinted) result += ", ";                    \
        result += hStr;                                          \
        bAnythingPrinted = true;                                 \
    }

    CHK_AND_PRINT(CLRMEMORYHOSTED, "Memory");
    CHK_AND_PRINT(CLRTASKHOSTED, "Task");
    CHK_AND_PRINT(CLRSYNCHOSTED, "Sync");
    CHK_AND_PRINT(CLRTHREADPOOLHOSTED, "Threadpool");
    CHK_AND_PRINT(CLRIOCOMPLETIONHOSTED, "IOCompletion");
    CHK_AND_PRINT(CLRASSEMBLYHOSTED, "Assembly");
    CHK_AND_PRINT(CLRGCHOSTED, "GC");
    CHK_AND_PRINT(CLRSECURITYHOSTED, "Security");

#undef CHK_AND_PRINT

    return result;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the managed threads               *
*                                                                      *
\**********************************************************************/
HRESULT PrintThreadsFromThreadStore(BOOL bMiniDump, BOOL bPrintLiveThreadsOnly)
{
    HRESULT Status;

    DacpThreadStoreData ThreadStore;
    if ((Status = ThreadStore.Request(g_sos)) != S_OK)
    {
        Print("Failed to request ThreadStore\n");
        return Status;
    }

    TableOutput table(2, 17);

    table.WriteRow("ThreadCount:", Decimal(ThreadStore.threadCount));
    table.WriteRow("UnstartedThread:", Decimal(ThreadStore.unstartedThreadCount));
    table.WriteRow("BackgroundThread:", Decimal(ThreadStore.backgroundThreadCount));
    table.WriteRow("PendingThread:", Decimal(ThreadStore.pendingThreadCount));
    table.WriteRow("DeadThread:", Decimal(ThreadStore.deadThreadCount));

    if (ThreadStore.fHostConfig & ~CLRHOSTED)
    {
        String hosting = "yes";

        hosting += " (";
        hosting += GetHostingCapabilities(ThreadStore.fHostConfig);
        hosting += ")";

        table.WriteRow("Hosted Runtime:", hosting);
    }
    else
    {
        table.WriteRow("Hosted Runtime:", "no");
    }

    const bool hosted = (ThreadStore.fHostConfig & CLRTASKHOSTED) != 0;
    table.ReInit(hosted ? 12 : 11, POINTERSIZE_HEX);
    table.SetWidths(10, 4, 4, 8, _max(9, POINTERSIZE_HEX), 8, 11, 1+POINTERSIZE_HEX*2, POINTERSIZE_HEX, 5, 3, POINTERSIZE_HEX);

    table.SetColAlignment(0, AlignRight);
    table.SetColAlignment(1, AlignRight);
    table.SetColAlignment(2, AlignRight);
    table.SetColAlignment(4, AlignRight);

    table.WriteColumn(8, "Lock");
    table.WriteRow("DBG", "ID", "OSID", "ThreadOBJ", "State", "GC Mode", "GC Alloc Context", "Domain", "Count", "Apt");

    if (hosted)
        table.WriteColumn("Fiber");

    table.WriteColumn("Exception");

    DacpThreadData Thread;
    CLRDATA_ADDRESS CurThread = ThreadStore.firstThread;
    while (CurThread)
    {
        if (IsInterrupt())
            break;

        if ((Status = Thread.Request(g_sos, CurThread)) != S_OK)
        {
            PrintLn("Failed to request Thread at ", Pointer(CurThread));
            return Status;
        }

        BOOL bSwitchedOutFiber = Thread.osThreadId == SWITCHED_OUT_FIBER_OSID;
        if (!IsKernelDebugger())
        {
            ULONG id = 0;

            if (bSwitchedOutFiber)
            {
                table.WriteColumn(0, "<<<< ");
            }
            else if (g_ExtSystem->GetThreadIdBySystemId(Thread.osThreadId, &id) == S_OK)
            {
                table.WriteColumn(0, Decimal(id));
            }
            else if (bPrintLiveThreadsOnly)
            {
                CurThread = Thread.nextThread;
                continue;
            }
            else
            {
                table.WriteColumn(0, "XXXX ");
            }
        }

        table.WriteColumn(1, Decimal(Thread.corThreadId));
        table.WriteColumn(2, ThreadID(bSwitchedOutFiber ? 0 : Thread.osThreadId));
        table.WriteColumn(3, Pointer(CurThread));
        table.WriteColumn(4, ThreadState(Thread.state));
        table.WriteColumn(5,  Thread.preemptiveGCDisabled == 1 ? "Cooperative" : "Preemptive");
        table.WriteColumnFormat(6, "%p:%p", SOS_PTR(Thread.allocContextPtr), SOS_PTR(Thread.allocContextLimit));

        if (Thread.domain)
        {
            table.WriteColumn(7, AppDomainPtr(Thread.domain));
        }
        else
        {
            CLRDATA_ADDRESS domain = 0;
            if (FAILED(g_sos->GetDomainFromContext(Thread.context, &domain)))
                table.WriteColumn(7, "<error>");
            else
                table.WriteColumn(7, AppDomainPtr(domain));
        }

        table.WriteColumn(8, Decimal(Thread.lockCount));

        // Apartment state
#ifndef FEATURE_PAL
        DWORD_PTR OleTlsDataAddr;
        if (IsWindowsTarget() && !bSwitchedOutFiber
                && SafeReadMemory(TO_TADDR(Thread.teb + offsetof(TEB, ReservedForOle)),
                            &OleTlsDataAddr,
                            sizeof(OleTlsDataAddr), NULL) && OleTlsDataAddr != 0)
        {
            DWORD AptState;
            if (SafeReadMemory(TO_TADDR(OleTlsDataAddr+offsetof(SOleTlsData,dwFlags)),
                               &AptState,
                               sizeof(AptState), NULL))
            {
                if (AptState & OLETLS_APARTMENTTHREADED)
                    table.WriteColumn(9, "STA");
                else if (AptState & OLETLS_MULTITHREADED)
                    table.WriteColumn(9, "MTA");
                else if (AptState & OLETLS_INNEUTRALAPT)
                    table.WriteColumn(9, "NTA");
                else
                    table.WriteColumn(9, "Ukn");
            }
            else
            {
                table.WriteColumn(9, "Ukn");
            }
        }
        else
#endif // FEATURE_PAL
            table.WriteColumn(9, "Ukn");

        if (hosted)
            table.WriteColumn(10, Thread.fiberData);

        WString lastCol;
        if (CurThread == ThreadStore.finalizerThread)
            lastCol += W("(Finalizer) ");
        if (CurThread == ThreadStore.gcThread)
            lastCol += W("(GC) ");

        const int TS_TPWorkerThread         = 0x01000000;    // is this a threadpool worker thread?
        const int TS_CompletionPortThread   = 0x08000000;    // is this is a completion port thread?

        if (Thread.state & TS_TPWorkerThread)
            lastCol += W("(Threadpool Worker) ");
        else if (Thread.state & TS_CompletionPortThread)
            lastCol += W("(Threadpool Completion Port) ");


        TADDR taLTOH;
        if (Thread.lastThrownObjectHandle && SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle),
                                                            &taLTOH, sizeof(taLTOH), NULL) && taLTOH)
        {
            TADDR taMT;
            if (SafeReadMemory(taLTOH, &taMT, sizeof(taMT), NULL))
            {
                if (NameForMT_s(taMT, g_mdName, mdNameLen))
                    lastCol += WString(g_mdName) + W(" ") + ExceptionPtr(taLTOH);
                else
                    lastCol += WString(W("<Invalid Object> (")) + Pointer(taLTOH) + W(")");

                // Print something if there are nested exceptions on the thread
                if (Thread.firstNestedException)
                    lastCol += W(" (nested exceptions)");
            }
        }

        table.WriteColumn(lastCol);
        CurThread = Thread.nextThread;
    }

    return Status;
}

#ifndef FEATURE_PAL
HRESULT PrintSpecialThreads()
{
    Print("\n");

    DWORD dwCLRTLSDataIndex = 0;
    HRESULT Status = g_sos->GetTLSIndex(&dwCLRTLSDataIndex);

    if (!SUCCEEDED (Status))
    {
        Print("Failed to retrieve Tls Data index\n");
        return Status;
    }


    ULONG ulOriginalThreadID = 0;
    Status = g_ExtSystem->GetCurrentThreadId (&ulOriginalThreadID);
    if (!SUCCEEDED (Status))
    {
        Print("Failed to require current Thread ID\n");
        return Status;
    }

    ULONG ulTotalThreads = 0;
    Status = g_ExtSystem->GetNumberThreads (&ulTotalThreads);
    if (!SUCCEEDED (Status))
    {
        Print("Failed to require total thread number\n");
        return Status;
    }

    TableOutput table(3, 4, AlignRight, 5);
    table.WriteRow("", "OSID", "Special thread type");

    for (ULONG ulThread = 0; ulThread < ulTotalThreads; ulThread++)
    {
        ULONG Id = 0;
        ULONG SysId = 0;
        HRESULT threadStatus = g_ExtSystem->GetThreadIdsByIndex(ulThread, 1, &Id, &SysId);
        if (!SUCCEEDED (threadStatus))
        {
            PrintLn("Failed to get thread ID for thread ", Decimal(ulThread));
            continue;
        }

        threadStatus = g_ExtSystem->SetCurrentThreadId(Id);
        if (!SUCCEEDED (threadStatus))
        {
            PrintLn("Failed to switch to thread ", ThreadID(SysId));
            continue;
        }

        CLRDATA_ADDRESS cdaTeb = 0;
        threadStatus = g_ExtSystem->GetCurrentThreadTeb(&cdaTeb);
        if (!SUCCEEDED (threadStatus))
        {
            PrintLn("Failed to get Teb for Thread ", ThreadID(SysId));
            continue;
        }

        TADDR CLRTLSDataAddr = 0;

        TADDR tlsArrayAddr = NULL;
        if (!SafeReadMemory (TO_TADDR(cdaTeb) + WINNT_OFFSETOF__TEB__ThreadLocalStoragePointer , &tlsArrayAddr, sizeof (void**), NULL))
        {
            PrintLn("Failed to get Tls expansion slots for thread ", ThreadID(SysId));
            continue;
        }

        if (tlsArrayAddr == NULL)
        {
            continue;
        }

        TADDR moduleTlsDataAddr = 0;
        if (!SafeReadMemory (tlsArrayAddr + sizeof (void*) * (dwCLRTLSDataIndex & 0xFFFF), &moduleTlsDataAddr, sizeof (void**), NULL))
        {
            PrintLn("Failed to get Tls expansion slots for thread ", ThreadID(SysId));
            continue;
        }

        CLRTLSDataAddr = moduleTlsDataAddr + ((dwCLRTLSDataIndex & 0x7FFF0000) >> 16) + OFFSETOF__TLS__tls_EETlsData;

        TADDR CLRTLSData = NULL;
        if (!SafeReadMemory (CLRTLSDataAddr, &CLRTLSData, sizeof (TADDR), NULL))
        {
            PrintLn("Failed to get CLR Tls data for thread ", ThreadID(SysId));
            continue;
        }

        if (CLRTLSData == NULL)
        {
            continue;
        }

        size_t ThreadType = 0;
        if (!SafeReadMemory (CLRTLSData + sizeof (TADDR) * TlsIdx_ThreadType, &ThreadType, sizeof (size_t), NULL))
        {
            PrintLn("Failed to get thread type info not found for thread ", ThreadID(SysId));
            continue;
        }

        if (ThreadType == 0)
        {
            continue;
        }

        table.WriteColumn(0, Decimal(Id));
        table.WriteColumn(1, ThreadID(SysId));

        String type;
        if (ThreadType & ThreadType_GC)
        {
            type += "GC ";
        }
        if (ThreadType & ThreadType_Timer)
        {
            type += "Timer ";
        }
        if (ThreadType & ThreadType_Gate)
        {
            type += "Gate ";
        }
        if (ThreadType & ThreadType_DbgHelper)
        {
            type += "DbgHelper ";
        }
        if (ThreadType & ThreadType_Shutdown)
        {
            type += "Shutdown ";
        }
        if (ThreadType & ThreadType_DynamicSuspendEE)
        {
            type += "SuspendEE ";
        }
        if (ThreadType & ThreadType_Finalizer)
        {
            type += "Finalizer ";
        }
        if (ThreadType & ThreadType_ADUnloadHelper)
        {
            type += "ADUnloadHelper ";
        }
        if (ThreadType & ThreadType_ShutdownHelper)
        {
            type += "ShutdownHelper ";
        }
        if (ThreadType & ThreadType_Threadpool_IOCompletion)
        {
            type += "IOCompletion ";
        }
        if (ThreadType & ThreadType_Threadpool_Worker)
        {
            type += "ThreadpoolWorker ";
        }
        if (ThreadType & ThreadType_Wait)
        {
            type += "Wait ";
        }
        if (ThreadType & ThreadType_ProfAPI_Attach)
        {
            type += "ProfilingAPIAttach ";
        }
        if (ThreadType & ThreadType_ProfAPI_Detach)
        {
            type += "ProfilingAPIDetach ";
        }

        table.WriteColumn(2, type);
    }

    Status = g_ExtSystem->SetCurrentThreadId (ulOriginalThreadID);
    if (!SUCCEEDED (Status))
    {
        ExtOut("Failed to switch to original thread\n");
        return Status;
    }

    return Status;
}
#endif //FEATURE_PAL

HRESULT SwitchToExceptionThread()
{
    HRESULT Status;

    DacpThreadStoreData ThreadStore;
    if ((Status = ThreadStore.Request(g_sos)) != S_OK)
    {
        Print("Failed to request ThreadStore\n");
        return Status;
    }

    DacpThreadData Thread;
    CLRDATA_ADDRESS CurThread = ThreadStore.firstThread;
    while (CurThread)
    {
        if (IsInterrupt())
            break;

        if ((Status = Thread.Request(g_sos, CurThread)) != S_OK)
        {
            PrintLn("Failed to request Thread at ", Pointer(CurThread));
            return Status;
        }

        TADDR taLTOH;
        if (Thread.lastThrownObjectHandle != (TADDR)0)
        {
            if (SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle), &taLTOH, sizeof(taLTOH), NULL))
            {
                if (taLTOH != (TADDR)0)
                {
                    ULONG id;
                    if (g_ExtSystem->GetThreadIdBySystemId(Thread.osThreadId, &id) == S_OK)
                    {
                        if (g_ExtSystem->SetCurrentThreadId(id) == S_OK)
                        {
                            PrintLn("Found managed exception on thread ", ThreadID(Thread.osThreadId));
                            break;
                        }
                    }
                }
            }
        }

        CurThread = Thread.nextThread;
    }

    return Status;
}

struct ThreadStateTable
{
    unsigned int State;
    const char * Name;
};
static const struct ThreadStateTable ThreadStates[] =
{
    {0x1, "Thread Abort Requested"},
    {0x2, "GC Suspend Pending"},
    {0x4, "User Suspend Pending"},
    {0x8, "Debug Suspend Pending"},
    {0x10, "GC On Transitions"},
    {0x20, "Legal to Join"},
    {0x40, "Yield Requested"},
    {0x80, "Hijacked by the GC"},
    {0x100, "Blocking GC for Stack Overflow"},
    {0x200, "Background"},
    {0x400, "Unstarted"},
    {0x800, "Dead"},
    {0x1000, "CLR Owns"},
    {0x2000, "CoInitialized"},
    {0x4000, "In Single Threaded Apartment"},
    {0x8000, "In Multi Threaded Apartment"},
    {0x10000, "Reported Dead"},
    {0x20000, "Fully initialized"},
    {0x40000, "Task Reset"},
    {0x80000, "Sync Suspended"},
    {0x100000, "Debug Will Sync"},
    {0x200000, "Stack Crawl Needed"},
    {0x400000, "Suspend Unstarted"},
    {0x800000, "Aborted"},
    {0x1000000, "Thread Pool Worker Thread"},
    {0x2000000, "Interruptible"},
    {0x4000000, "Interrupted"},
    {0x8000000, "Completion Port Thread"},
    {0x10000000, "Abort Initiated"},
    {0x20000000, "Finalized"},
    {0x40000000, "Failed to Start"},
    {0x80000000, "Detached"},
};

DECLARE_API(ThreadState)
{
    INIT_API_NODAC();

    size_t state = GetExpression(args);
    int count = 0;

    if (state)
    {

        for (unsigned int i = 0; i < ARRAY_SIZE(ThreadStates); ++i)
            if (state & ThreadStates[i].State)
            {
                ExtOut("    %s\n", ThreadStates[i].Name);
                count++;
            }
    }

    // If we did not find any thread states, print out a message to let the user
    // know that the function is working correctly.
    if (count == 0)
        ExtOut("    No thread states for '%s'\n", args);

    return Status;
}

DECLARE_API(Threads)
{
    INIT_API_PROBE_MANAGED("clrthreads");

    BOOL bPrintSpecialThreads = FALSE;
    BOOL bPrintLiveThreadsOnly = FALSE;
    BOOL bSwitchToManagedExceptionThread = FALSE;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-special", &bPrintSpecialThreads, COBOOL, FALSE},
        {"-live", &bPrintLiveThreadsOnly, COBOOL, FALSE},
        {"-managedexception", &bSwitchToManagedExceptionThread, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }

    if (bSwitchToManagedExceptionThread)
    {
        return SwitchToExceptionThread();
    }

    // We need to support minidumps for this command.
    BOOL bMiniDump = IsMiniDumpFile();

    EnableDMLHolder dmlHolder(dml);

    try
    {
        Status = PrintThreadsFromThreadStore(bMiniDump, bPrintLiveThreadsOnly);
        if (bPrintSpecialThreads)
        {
#ifdef FEATURE_PAL
            Print("\n-special not supported.\n");
#else //FEATURE_PAL
            BOOL bSupported = true;

            if (!IsWindowsTarget())
            {
                Print("Special thread information is only supported on Windows targets.\n");
                bSupported = false;
            }
            else if (bMiniDump)
            {
                Print("Special thread information is not available in mini dumps.\n");
                bSupported = false;
            }

            if (bSupported)
            {
                if (CheckBreakingRuntimeChange())
                {
                    return E_FAIL;
                }
                HRESULT Status2 = PrintSpecialThreads();
                if (!SUCCEEDED(Status2))
                    Status = Status2;
            }
#endif // FEATURE_PAL
        }
    }
    catch (sos::Exception &e)
    {
        ExtOut("%s\n", e.what());
    }

    return Status;
}

#ifndef FEATURE_PAL
/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the Watson Buckets.               *
*                                                                      *
\**********************************************************************/
DECLARE_API(WatsonBuckets)
{
    INIT_API();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    // We don't need to support minidumps for this command.
    if (IsMiniDumpFile())
    {
        ExtOut("Not supported on mini dumps.\n");
    }

    // Get the current managed thread.
    CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
    DacpThreadData Thread;

    if ((threadAddr == NULL) || ((Status = Thread.Request(g_sos, threadAddr)) != S_OK))
    {
        ExtOut("The current thread is unmanaged\n");
        return Status;
    }

    // Get the definition of GenericModeBlock.
#include <msodw.h>
    GenericModeBlock gmb;

    if ((Status = g_sos->GetClrWatsonBuckets(threadAddr, &gmb)) != S_OK)
    {
        ExtOut("Can't get Watson Buckets\n");
        return Status;
    }

    ExtOut("Watson Bucket parameters:\n");
    ExtOut("b1: %S\n", gmb.wzP1);
    ExtOut("b2: %S\n", gmb.wzP2);
    ExtOut("b3: %S\n", gmb.wzP3);
    ExtOut("b4: %S\n", gmb.wzP4);
    ExtOut("b5: %S\n", gmb.wzP5);
    ExtOut("b6: %S\n", gmb.wzP6);
    ExtOut("b7: %S\n", gmb.wzP7);
    ExtOut("b8: %S\n", gmb.wzP8);
    ExtOut("b9: %S\n", gmb.wzP9);

    return Status;
} // WatsonBuckets()
#endif // FEATURE_PAL

struct PendingBreakpoint
{
    WCHAR szModuleName[MAX_LONGPATH];
    WCHAR szFunctionName[mdNameLen];
    WCHAR szFilename[MAX_LONGPATH];
    DWORD lineNumber;
    TADDR pModule;
    DWORD ilOffset;
    mdMethodDef methodToken;
    void SetModule(TADDR module)
    {
        pModule = module;
    }

    bool ModuleMatches(TADDR compare)
    {
        return (compare == pModule);
    }

    PendingBreakpoint *pNext;
    PendingBreakpoint() : lineNumber(0), ilOffset(0), methodToken(0), pNext(NULL)
    {
        szModuleName[0] = L'\0';
        szFunctionName[0] = L'\0';
        szFilename[0] = L'\0';
    }
};

void IssueDebuggerBPCommand ( CLRDATA_ADDRESS addr )
{
    const int MaxBPsCached = 1024;
    static CLRDATA_ADDRESS alreadyPlacedBPs[MaxBPsCached];
    static int curLimit = 0;

    // on ARM the debugger requires breakpoint addresses to be sanitized
    if (IsDbgTargetArm())
#ifndef FEATURE_PAL
      addr &= ~THUMB_CODE;
#else
      addr |= THUMB_CODE; // lldb expects thumb code bit set
#endif

    // if we overflowed our cache consider all new BPs unique...
    BOOL bUnique = curLimit >= MaxBPsCached;
    if (!bUnique)
    {
        bUnique = TRUE;
        for (int i = 0; i < curLimit; ++i)
        {
            if (alreadyPlacedBPs[i] == addr)
            {
                bUnique = FALSE;
                break;
            }
        }
    }
    if (bUnique)
    {
        char buffer[64]; // sufficient for "bp <pointersize>"
        static WCHAR wszNameBuffer[1024]; // should be large enough

        // get the MethodDesc name
        CLRDATA_ADDRESS pMD;
        if (g_sos->GetMethodDescPtrFromIP(addr, &pMD) != S_OK
            || g_sos->GetMethodDescName(pMD, 1024, wszNameBuffer, NULL) != S_OK)
        {
            wcscpy_s(wszNameBuffer, ARRAY_SIZE(wszNameBuffer), W("UNKNOWN"));
        }

#ifndef FEATURE_PAL
        sprintf_s(buffer, ARRAY_SIZE(buffer), "bp %p", SOS_PTR(addr));
#else
        sprintf_s(buffer, ARRAY_SIZE(buffer), "breakpoint set --address 0x%p", SOS_PTR(addr));
#endif
        ExtOut("Setting breakpoint: %s [%S]\n", buffer, wszNameBuffer);
        g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);

        if (curLimit < MaxBPsCached)
        {
            alreadyPlacedBPs[curLimit++] = addr;
        }
    }
}

class Breakpoints
{
    PendingBreakpoint* m_breakpoints;
public:
    Breakpoints()
    {
        m_breakpoints = NULL;
    }
    ~Breakpoints()
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            PendingBreakpoint *pNext = pCur->pNext;
            delete pCur;
            pCur = pNext;
        }
        m_breakpoints = NULL;
    }

    void Add(__in_z LPWSTR szModule, __in_z LPWSTR szName, TADDR mod, DWORD ilOffset)
    {
        if (!IsIn(szModule, szName, mod))
        {
            PendingBreakpoint *pNew = new PendingBreakpoint();
            wcscpy_s(pNew->szModuleName, MAX_LONGPATH, szModule);
            wcscpy_s(pNew->szFunctionName, mdNameLen, szName);
            pNew->SetModule(mod);
            pNew->ilOffset = ilOffset;
            pNew->pNext = m_breakpoints;
            m_breakpoints = pNew;
        }
    }

    void Add(__in_z LPWSTR szModule, __in_z LPWSTR szName, mdMethodDef methodToken, TADDR mod, DWORD ilOffset)
    {
        if (!IsIn(methodToken, mod, ilOffset))
        {
            PendingBreakpoint *pNew = new PendingBreakpoint();
            wcscpy_s(pNew->szModuleName, MAX_LONGPATH, szModule);
            wcscpy_s(pNew->szFunctionName, mdNameLen, szName);
            pNew->methodToken = methodToken;
            pNew->SetModule(mod);
            pNew->ilOffset = ilOffset;
            pNew->pNext = m_breakpoints;
            m_breakpoints = pNew;
        }
    }

    void Add(__in_z LPWSTR szFilename, DWORD lineNumber, TADDR mod)
    {
        if (!IsIn(szFilename, lineNumber, mod))
        {
            PendingBreakpoint *pNew = new PendingBreakpoint();
            wcscpy_s(pNew->szFilename, MAX_LONGPATH, szFilename);
            pNew->lineNumber = lineNumber;
            pNew->SetModule(mod);
            pNew->pNext = m_breakpoints;
            m_breakpoints = pNew;
        }
    }

    void Add(__in_z LPWSTR szFilename, DWORD lineNumber, mdMethodDef methodToken, TADDR mod, DWORD ilOffset)
    {
        if (!IsIn(methodToken, mod, ilOffset))
        {
            PendingBreakpoint *pNew = new PendingBreakpoint();
            wcscpy_s(pNew->szFilename, MAX_LONGPATH, szFilename);
            pNew->lineNumber = lineNumber;
            pNew->methodToken = methodToken;
            pNew->SetModule(mod);
            pNew->ilOffset = ilOffset;
            pNew->pNext = m_breakpoints;
            m_breakpoints = pNew;
        }
    }

    //returns true if updates are still needed for this module, FALSE if all BPs are now bound
    BOOL Update(TADDR mod, BOOL isNewModule)
    {
        BOOL bNeedUpdates = FALSE;
        PendingBreakpoint *pCur = NULL;

        if(isNewModule)
        {
            SymbolReader symbolReader;
            SymbolReader* pSymReader = &symbolReader;
            if(LoadSymbolsForModule(mod, &symbolReader) != S_OK)
                pSymReader = NULL;

            // Get tokens for any modules that match. If there was a change,
            // update notifications.
            pCur = m_breakpoints;
            while(pCur)
            {
                PendingBreakpoint *pNext = pCur->pNext;
                ResolvePendingNonModuleBoundBreakpoint(mod, pCur, pSymReader);
                pCur = pNext;
            }
        }

        pCur = m_breakpoints;
        while(pCur)
        {
            PendingBreakpoint *pNext = pCur->pNext;
            if (ResolvePendingBreakpoint(mod, pCur))
            {
                bNeedUpdates = TRUE;
            }
            pCur = pNext;
        }
        return bNeedUpdates;
    }

    BOOL UpdateKnownCodeAddress(TADDR mod, CLRDATA_ADDRESS bpLocation)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        BOOL bpSet = FALSE;

        while(pCur)
        {
            PendingBreakpoint *pNext = pCur->pNext;
            if (pCur->ModuleMatches(mod))
            {
                IssueDebuggerBPCommand(bpLocation);
                bpSet = TRUE;
                break;
            }

            pCur = pNext;
        }

        return bpSet;
    }

    void RemovePendingForModule(TADDR mod)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            PendingBreakpoint *pNext = pCur->pNext;
            if (pCur->ModuleMatches(mod))
            {
                // Delete the current node, and keep going
                Delete(pCur);
            }

            pCur = pNext;
        }
    }

    void ListBreakpoints()
    {
        PendingBreakpoint *pCur = m_breakpoints;
        size_t iBreakpointIndex = 1;
        ExtOut("%sbpmd pending breakpoint list\n Breakpoint index - Location, ModuleID, Method Token\n", SOSPrefix);
        while(pCur)
        {
            //windbg likes to format %p as always being 64 bits
            ULONG64 modulePtr = (ULONG64)pCur->pModule;

            if(pCur->szModuleName[0] != L'\0')
                ExtOut("%d - %ws!%ws+%d, 0x%p, 0x%08x\n", iBreakpointIndex, pCur->szModuleName, pCur->szFunctionName, pCur->ilOffset, SOS_PTR(modulePtr), pCur->methodToken);
            else
                ExtOut("%d - %ws:%d, 0x%p, 0x%08x\n",  iBreakpointIndex, pCur->szFilename, pCur->lineNumber, SOS_PTR(modulePtr), pCur->methodToken);
            iBreakpointIndex++;
            pCur = pCur->pNext;
        }
    }

#ifndef FEATURE_PAL
    void SaveBreakpoints(FILE* pFile)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            if(pCur->szModuleName[0] != L'\0')
                fprintf_s(pFile, "!bpmd %ws %ws %d\n", pCur->szModuleName, pCur->szFunctionName, pCur->ilOffset);
            else
                fprintf_s(pFile, "!bpmd %ws:%d\n",  pCur->szFilename, pCur->lineNumber);
            pCur = pCur->pNext;
        }
    }
#endif

    void CleanupNotifications()
    {
#ifdef FEATURE_PAL
        if (m_breakpoints == NULL)
        {
            g_ExtServices->ClearExceptionCallback();
        }
#endif
    }

    void ClearBreakpoint(size_t breakPointToClear)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        size_t iBreakpointIndex = 1;
        while(pCur)
        {
            if (breakPointToClear == iBreakpointIndex)
            {
                ExtOut("%d - %ws, %ws, %p\n", iBreakpointIndex, pCur->szModuleName, pCur->szFunctionName, SOS_PTR(pCur->pModule));
                ExtOut("Cleared\n");
                Delete(pCur);
                break;
            }
            iBreakpointIndex++;
            pCur = pCur->pNext;
        }

        if (pCur == NULL)
        {
            ExtOut("Invalid pending breakpoint index.\n");
        }
        CleanupNotifications();
    }

    void ClearAllBreakpoints()
    {
        size_t iBreakpointIndex = 1;
        for (PendingBreakpoint *pCur = m_breakpoints; pCur != NULL; )
        {
            PendingBreakpoint* pNext = pCur->pNext;
            Delete(pCur);
            iBreakpointIndex++;
            pCur = pNext;
        }
        CleanupNotifications();

        ExtOut("All pending breakpoints cleared.\n");
    }

    HRESULT LoadSymbolsForModule(TADDR mod, SymbolReader* pSymbolReader)
    {
        HRESULT Status = S_OK;
        ToRelease<IXCLRDataModule> pModule;
        IfFailRet(g_sos->GetModule(mod, &pModule));

        ToRelease<IMetaDataImport> pMDImport = NULL;
        pModule->QueryInterface(IID_IMetaDataImport, (LPVOID *) &pMDImport);

        IfFailRet(pSymbolReader->LoadSymbols(pMDImport, pModule));

        return S_OK;
    }

    HRESULT ResolvePendingNonModuleBoundBreakpoint(__in_z WCHAR* pFilename, DWORD lineNumber, TADDR mod, SymbolReader* pSymbolReader)
    {
        HRESULT Status = S_OK;
        if(pSymbolReader == NULL)
            return S_FALSE; // no symbols, can't bind here

        mdMethodDef methodDef;
        ULONG32 ilOffset;
        if(FAILED(Status = pSymbolReader->ResolveSequencePoint(pFilename, lineNumber, &methodDef, &ilOffset)))
        {
            return S_FALSE; // not binding in a module is typical
        }

        Add(pFilename, lineNumber, methodDef, mod, ilOffset);
        return Status;
    }

    HRESULT ResolvePendingNonModuleBoundBreakpoint(__in_z WCHAR* pModuleName, __in_z WCHAR* pMethodName, TADDR mod, DWORD ilOffset)
    {
        HRESULT Status = S_OK;
        char szName[mdNameLen];
        int numModule;

        ToRelease<IXCLRDataModule> module;
        IfFailRet(g_sos->GetModule(mod, &module));

        WideCharToMultiByte(CP_ACP, 0, pModuleName, (int)(_wcslen(pModuleName) + 1), szName, mdNameLen, NULL, NULL);

        ArrayHolder<DWORD_PTR> moduleList = ModuleFromName(szName, &numModule);
        if (moduleList == NULL)
        {
            ExtOut("Failed to request module list.\n");
            return E_FAIL;
        }

        for (int i = 0; i < numModule; i++)
        {
            // If any one entry in moduleList matches, then the current PendingBreakpoint
            // is the right one.
            if(moduleList[i] != TO_TADDR(mod))
                continue;

            CLRDATA_ENUM h;
            if (module->StartEnumMethodDefinitionsByName(pMethodName, 0, &h) == S_OK)
            {
                IXCLRDataMethodDefinition *pMeth = NULL;
                while (module->EnumMethodDefinitionByName(&h, &pMeth) == S_OK)
                {
                    mdMethodDef methodToken;
                    ToRelease<IXCLRDataModule> pUnusedModule;
                    IfFailRet(pMeth->GetTokenAndScope(&methodToken, &pUnusedModule));

                    Add(pModuleName, pMethodName, methodToken, mod, ilOffset);
                    pMeth->Release();
                }
                module->EndEnumMethodDefinitionsByName(h);
            }
        }
        return S_OK;
    }

    // Return TRUE if there might be more instances that will be JITTED later
    static BOOL ResolveMethodInstances(IXCLRDataMethodDefinition *pMeth, DWORD ilOffset)
    {
        BOOL bFoundCode = FALSE;
        BOOL bNeedDefer = FALSE;
        CLRDATA_ENUM h1;

        if (pMeth->StartEnumInstances (NULL, &h1) == S_OK)
        {
            IXCLRDataMethodInstance *inst = NULL;
            while (pMeth->EnumInstance (&h1, &inst) == S_OK)
            {
                BOOL foundByIlOffset = FALSE;
                ULONG32 rangesNeeded = 0;
                if(inst->GetAddressRangesByILOffset(ilOffset, 0, &rangesNeeded, NULL) == S_OK)
                {
                    ArrayHolder<CLRDATA_ADDRESS_RANGE> ranges = new NOTHROW CLRDATA_ADDRESS_RANGE[rangesNeeded];
                    if (ranges != NULL)
                    {
                        if (inst->GetAddressRangesByILOffset(ilOffset, rangesNeeded, NULL, ranges) == S_OK)
                        {
                            for (DWORD i = 0; i < rangesNeeded; i++)
                            {
                                IssueDebuggerBPCommand(ranges[i].startAddress);
                                bFoundCode = TRUE;
                                foundByIlOffset = TRUE;
                            }
                        }
                    }
                }

                if (!foundByIlOffset && ilOffset == 0)
                {
                    CLRDATA_ADDRESS addr = 0;
                    if (inst->GetRepresentativeEntryAddress(&addr) == S_OK)
                    {
                        IssueDebuggerBPCommand(addr);
                        bFoundCode = TRUE;
                    }
                }
            }
            pMeth->EndEnumInstances (h1);
        }

        // if this is a generic method we need to add a deferred bp
        BOOL bGeneric = FALSE;
        pMeth->HasClassOrMethodInstantiation(&bGeneric);

        bNeedDefer = !bFoundCode || bGeneric;
        // This is down here because we only need to call SetCodeNofiication once.
        if (bNeedDefer)
        {
            if (pMeth->SetCodeNotification (CLRDATA_METHNOTIFY_GENERATED) != S_OK)
            {
                bNeedDefer = FALSE;
                ExtOut("Failed to set code notification\n");
            }
        }
        return bNeedDefer;
    }

private:
    BOOL IsIn(__in_z LPWSTR szModule, __in_z LPWSTR szName, TADDR mod)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            if (pCur->ModuleMatches(mod) &&
                _wcsicmp(pCur->szModuleName, szModule) == 0 &&
                _wcscmp(pCur->szFunctionName, szName) == 0)
            {
                return TRUE;
            }
            pCur = pCur->pNext;
        }
        return FALSE;
    }

    BOOL IsIn(__in_z LPWSTR szFilename, DWORD lineNumber, TADDR mod)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            if (pCur->ModuleMatches(mod) &&
                _wcsicmp(pCur->szFilename, szFilename) == 0 &&
                pCur->lineNumber == lineNumber)
            {
                return TRUE;
            }
            pCur = pCur->pNext;
        }
        return FALSE;
    }

    BOOL IsIn(mdMethodDef token, TADDR mod, DWORD ilOffset)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        while(pCur)
        {
            if (pCur->ModuleMatches(mod) &&
                pCur->methodToken == token &&
                pCur->ilOffset == ilOffset)
            {
                return TRUE;
            }
            pCur = pCur->pNext;
        }
        return FALSE;
    }

    void Delete(PendingBreakpoint *pDelete)
    {
        PendingBreakpoint *pCur = m_breakpoints;
        PendingBreakpoint *pPrev = NULL;
        while(pCur)
        {
            if (pCur == pDelete)
            {
                if (pPrev == NULL)
                {
                    m_breakpoints = pCur->pNext;
                }
                else
                {
                    pPrev->pNext = pCur->pNext;
                }
                delete pCur;
                return;
            }
            pPrev = pCur;
            pCur = pCur->pNext;
        }
    }



    HRESULT ResolvePendingNonModuleBoundBreakpoint(TADDR mod, PendingBreakpoint *pCur, SymbolReader* pSymbolReader)
    {
        // This function only works with pending breakpoints that are not module bound.
        if (pCur->pModule == (TADDR)0)
        {
            if (pCur->szModuleName[0] != L'\0')
            {
                return ResolvePendingNonModuleBoundBreakpoint(pCur->szModuleName, pCur->szFunctionName, mod, pCur->ilOffset);
            }
            else
            {
                return ResolvePendingNonModuleBoundBreakpoint(pCur->szFilename, pCur->lineNumber, mod, pSymbolReader);
            }
        }
        else
        {
            return S_OK;
        }
    }

    // Returns TRUE if further instances may be jitted, FALSE if all instances are now resolved
    BOOL ResolvePendingBreakpoint(TADDR addr, PendingBreakpoint *pCur)
    {
        // Only go forward if the module matches the current PendingBreakpoint
        if (!pCur->ModuleMatches(addr))
        {
            return FALSE;
        }

        ToRelease<IXCLRDataModule> mod;
        if (FAILED(g_sos->GetModule(addr, &mod)))
        {
            return FALSE;
        }

        if(pCur->methodToken == 0)
        {
            return FALSE;
        }

        ToRelease<IXCLRDataMethodDefinition> pMeth = NULL;
        mod->GetMethodDefinitionByToken(pCur->methodToken, &pMeth);

        // We may not need the code notification. Maybe it was ngen'd and we
        // already have the method?
        // We can delete the current entry if ResolveMethodInstances() set all BPs
        return ResolveMethodInstances(pMeth, pCur->ilOffset);
    }
};

Breakpoints g_bpoints;

// If true, call the HandleRuntimeLoadedNotification function to enable the assembly load and JIT exceptions
#ifndef FEATURE_PAL
bool g_breakOnRuntimeModuleLoad = false;
#endif

// Controls whether optimizations are disabled on module load and whether NGEN can be used
BOOL g_fAllowJitOptimization = TRUE;

// Controls whether a one-shot breakpoint should be inserted the next time
// execution is about to enter a catch clause
BOOL g_stopOnNextCatch = FALSE;

// According to the latest debuggers these callbacks will not get called
// unless the user (or an extension, like SOS :-)) had previously enabled
// clrn with "sxe clrn".
class CNotification : public IXCLRDataExceptionNotification5
{
    static int s_condemnedGen;

    int m_count;
    int m_dbgStatus;
public:
    CNotification()
        : m_count(0)
        , m_dbgStatus(DEBUG_STATUS_NO_CHANGE)
    {}

    int GetDebugStatus()
    {
        return m_dbgStatus;
    }

    STDMETHODIMP QueryInterface (REFIID iid, void **ppvObject)
    {
        if (ppvObject == NULL)
            return E_INVALIDARG;

        if (IsEqualIID(iid, IID_IUnknown)
            || IsEqualIID(iid, IID_IXCLRDataExceptionNotification)
            || IsEqualIID(iid, IID_IXCLRDataExceptionNotification2)
            || IsEqualIID(iid, IID_IXCLRDataExceptionNotification3)
            || IsEqualIID(iid, IID_IXCLRDataExceptionNotification4)
            || IsEqualIID(iid, IID_IXCLRDataExceptionNotification5))
        {
            *ppvObject = static_cast<IXCLRDataExceptionNotification5*>(this);
            AddRef();
            return S_OK;
        }
        else
            return E_NOINTERFACE;

    }

    STDMETHODIMP_(ULONG) AddRef(void) { return ++m_count; }
    STDMETHODIMP_(ULONG) Release(void)
    {
        m_count--;
        if (m_count < 0)
        {
            m_count = 0;
        }
        return m_count;
    }

    /*
     * New code was generated or discarded for a method.:
     */
    STDMETHODIMP OnCodeGenerated(IXCLRDataMethodInstance* method)
    {
#ifndef FEATURE_PAL
        _ASSERTE(g_pRuntime != nullptr);

        // This is only needed for desktop runtime because OnCodeGenerated2
        // isn't supported by the desktop DAC.
        if (g_pRuntime->GetRuntimeConfiguration() == IRuntime::WindowsDesktop)
        {
            // Some method has been generated, make a breakpoint and remove it.
            ULONG32 len = mdNameLen;
            LPWSTR szModuleName = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));
            if (method->GetName(0, mdNameLen, &len, g_mdName) == S_OK)
            {
                ToRelease<IXCLRDataModule> pMod;
                HRESULT hr = method->GetTokenAndScope(NULL, &pMod);
                if (SUCCEEDED(hr))
                {
                    len = mdNameLen;
                    if (pMod->GetName(mdNameLen, &len, szModuleName) == S_OK)
                    {
                        ExtOut("JITTED %S!%S\n", szModuleName, g_mdName);

                        // Add breakpoint, perhaps delete pending breakpoint
                        DacpGetModuleAddress dgma;
                        if (SUCCEEDED(dgma.Request(pMod)))
                        {
                            g_bpoints.Update(TO_TADDR(dgma.ModulePtr), FALSE);
                        }
                        else
                        {
                            ExtOut("Failed to request module address.\n");
                        }
                    }
                }
            }
        }
#endif
        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;
        return S_OK;
    }

    STDMETHODIMP OnCodeGenerated2(IXCLRDataMethodInstance* method, CLRDATA_ADDRESS nativeCodeLocation)
    {
        // Some method has been generated, make a breakpoint.
        ULONG32 len = mdNameLen;
        LPWSTR szModuleName = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));
        if (method->GetName(0, mdNameLen, &len, g_mdName) == S_OK)
        {
            ToRelease<IXCLRDataModule> pMod;
            HRESULT hr = method->GetTokenAndScope(NULL, &pMod);
            if (SUCCEEDED(hr))
            {
                len = mdNameLen;
                if (pMod->GetName(mdNameLen, &len, szModuleName) == S_OK)
                {
                    ExtOut("JITTED %S!%S\n", szModuleName, g_mdName);

                    DacpGetModuleAddress dgma;
                    if (SUCCEEDED(dgma.Request(pMod)))
                    {
                        g_bpoints.UpdateKnownCodeAddress(TO_TADDR(dgma.ModulePtr), nativeCodeLocation);
                    }
                    else
                    {
                        ExtOut("Failed to request module address.\n");
                    }
                }
            }
        }

        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;
        return S_OK;
    }

    STDMETHODIMP OnCodeDiscarded(IXCLRDataMethodInstance* method)
    {
        return E_NOTIMPL;
    }

    /*
     * The process or task reached the desired execution state.
     */
    STDMETHODIMP OnProcessExecution(ULONG32 state) { return E_NOTIMPL; }
    STDMETHODIMP OnTaskExecution(IXCLRDataTask* task,
                            ULONG32 state) { return E_NOTIMPL; }

    /*
     * The given module was loaded or unloaded.
     */
    STDMETHODIMP OnModuleLoaded(IXCLRDataModule* mod)
    {
        DacpGetModuleAddress dgma;
        if (SUCCEEDED(dgma.Request(mod)))
        {
            g_bpoints.Update(TO_TADDR(dgma.ModulePtr), TRUE);
        }

        if (!g_fAllowJitOptimization)
        {
            HRESULT hr;
            ToRelease<IXCLRDataModule2> mod2;
            if (FAILED(mod->QueryInterface(__uuidof(IXCLRDataModule2), (void**) &mod2)))
            {
                ExtOut("SOS: warning, optimizations for this module could not be suppressed because this CLR version doesn't support the functionality\n");
            }
            else if(FAILED(hr = mod2->SetJITCompilerFlags(CORDEBUG_JIT_DISABLE_OPTIMIZATION)))
            {
                if(hr == CORDBG_E_CANT_CHANGE_JIT_SETTING_FOR_ZAP_MODULE)
                    ExtOut("SOS: warning, optimizations for this module could not be suppressed because an optimized prejitted image was loaded\n");
                else
                    ExtOut("SOS: warning, optimizations for this module could not be suppressed hr=0x%x\n", hr);
            }
        }

        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;
        return S_OK;
    }

    STDMETHODIMP OnModuleUnloaded(IXCLRDataModule* mod)
    {
        DacpGetModuleAddress dgma;
        if (SUCCEEDED(dgma.Request(mod)))
        {
            g_bpoints.RemovePendingForModule(TO_TADDR(dgma.ModulePtr));
        }

        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;
        return S_OK;
    }

    /*
     * The given type was loaded or unloaded.
     */
    STDMETHODIMP OnTypeLoaded(IXCLRDataTypeInstance* typeInst)
    { return E_NOTIMPL; }
    STDMETHODIMP OnTypeUnloaded(IXCLRDataTypeInstance* typeInst)
    { return E_NOTIMPL; }

    STDMETHODIMP OnAppDomainLoaded(IXCLRDataAppDomain* domain)
    { return E_NOTIMPL; }
    STDMETHODIMP OnAppDomainUnloaded(IXCLRDataAppDomain* domain)
    { return E_NOTIMPL; }
    STDMETHODIMP OnException(IXCLRDataExceptionState* exception)
    { return E_NOTIMPL; }

    STDMETHODIMP OnGcEvent(GcEvtArgs gcEvtArgs)
{
        // by default don't stop on these notifications...
        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;

        IXCLRDataProcess2* idp2 = NULL;
        if (SUCCEEDED(g_clrData->QueryInterface(IID_IXCLRDataProcess2, (void**) &idp2)))
        {
            if (gcEvtArgs.typ == GC_MARK_END)
            {
                // erase notification request
                GcEvtArgs gea = { GC_MARK_END, { 0 } };
                idp2->SetGcNotification(gea);

                s_condemnedGen = bitidx(gcEvtArgs.condemnedGeneration);

                ExtOut("CLR notification: GC - Performing a gen %d collection. Determined surviving objects...\n", s_condemnedGen);

                // GC_MARK_END notification means: give the user a chance to examine the debuggee
                m_dbgStatus = DEBUG_STATUS_BREAK;
            }
        }

        return S_OK;
    }

     /*
     * Catch is about to be entered
     */
    STDMETHODIMP ExceptionCatcherEnter(IXCLRDataMethodInstance* method, DWORD catcherNativeOffset)
    {
        if(g_stopOnNextCatch)
        {
            CLRDATA_ADDRESS startAddr;
            if(method->GetRepresentativeEntryAddress(&startAddr) == S_OK)
            {
                CHAR buffer[100];
#ifndef FEATURE_PAL
                sprintf_s(buffer, ARRAY_SIZE(buffer), "bp /1 %p", SOS_PTR(startAddr+catcherNativeOffset));
#else
                sprintf_s(buffer, ARRAY_SIZE(buffer), "breakpoint set --one-shot --address 0x%p", SOS_PTR(startAddr+catcherNativeOffset));
#endif
                g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
            }
            g_stopOnNextCatch = FALSE;
        }

        m_dbgStatus = DEBUG_STATUS_GO_HANDLED;
        return S_OK;
    }

    static int GetCondemnedGen()
    {
        return s_condemnedGen;
    }

};

int CNotification::s_condemnedGen = -1;

BOOL CheckCLRNotificationEvent(DEBUG_LAST_EVENT_INFO_EXCEPTION* pdle)
{
    ISOSDacInterface4 *psos4 = NULL;
    CLRDATA_ADDRESS arguments[3];
    HRESULT Status;

    if (SUCCEEDED(Status = g_sos->QueryInterface(__uuidof(ISOSDacInterface4), (void**) &psos4)))
    {
        int count = ARRAY_SIZE(arguments);
        int countNeeded = 0;

        Status = psos4->GetClrNotification(arguments, count, &countNeeded);
        psos4->Release();

        if (SUCCEEDED(Status))
        {
            memset(&pdle->ExceptionRecord, 0, sizeof(pdle->ExceptionRecord));
            pdle->FirstChance = TRUE;
            pdle->ExceptionRecord.ExceptionCode = CLRDATA_NOTIFY_EXCEPTION;

            _ASSERTE(count <= EXCEPTION_MAXIMUM_PARAMETERS);
            for (int i = 0; i < count; i++)
            {
                pdle->ExceptionRecord.ExceptionInformation[i] = arguments[i];
            }
            // The rest of the ExceptionRecord isn't used by TranslateExceptionRecordToNotification
            return TRUE;
        }
        // No pending exception notification
        return FALSE;
    }

    // The new DAC based interface doesn't exists so ask the debugger for the last exception information.

#ifdef HOST_WINDOWS
    ULONG Type, ProcessId, ThreadId;
    ULONG ExtraInformationUsed;
    Status = g_ExtControl->GetLastEventInformation(
        &Type,
        &ProcessId,
        &ThreadId,
        pdle,
        sizeof(DEBUG_LAST_EVENT_INFO_EXCEPTION),
        &ExtraInformationUsed,
        NULL,
        0,
        NULL);

    if (Status != S_OK || Type != DEBUG_EVENT_EXCEPTION)
    {
        return FALSE;
    }

    if (!pdle->FirstChance || pdle->ExceptionRecord.ExceptionCode != CLRDATA_NOTIFY_EXCEPTION)
    {
        return FALSE;
    }
    return TRUE;
#else
    return FALSE;
#endif
}

HRESULT HandleCLRNotificationEvent()
{
    /*
     * Did we get module load notification? If so, check if any in our pending list
     * need to be registered for jit notification.
     *
     * Did we get a jit notification? If so, check if any can be removed and
     * real breakpoints be set.
     */
    DEBUG_LAST_EVENT_INFO_EXCEPTION dle;
    CNotification Notification;

    if (!CheckCLRNotificationEvent(&dle))
    {
#ifndef FEATURE_PAL
        ExtOut("Expecting first chance CLRN exception\n");
        return E_FAIL;
#else
        g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "process continue", 0);
        return S_OK;
#endif
    }

    // Notification only needs to live for the lifetime of the call below, so it's a non-static
    // local.
    HRESULT Status = g_clrData->TranslateExceptionRecordToNotification(&dle.ExceptionRecord, &Notification);
    if (Status != S_OK)
    {
        ExtErr("Error processing exception notification\n");
        return Status;
    }
    else
    {
        switch (Notification.GetDebugStatus())
        {
            case DEBUG_STATUS_GO:
            case DEBUG_STATUS_GO_HANDLED:
            case DEBUG_STATUS_GO_NOT_HANDLED:
#ifndef FEATURE_PAL
                g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "g", 0);
#else
                g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "process continue", 0);
#endif
                break;
            default:
                break;
        }
    }

    return S_OK;
}

void EnableModuleLoadUnloadCallbacks()
{
    _ASSERTE(g_clrData != nullptr);

    ULONG32 flags = 0;
    g_clrData->GetOtherNotificationFlags(&flags);
    flags |= (CLRDATA_NOTIFY_ON_MODULE_LOAD | CLRDATA_NOTIFY_ON_MODULE_UNLOAD);
    g_clrData->SetOtherNotificationFlags(flags);
}

#ifndef FEATURE_PAL

DECLARE_API(SOSHandleCLRN)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    return HandleCLRNotificationEvent();
}

HRESULT HandleRuntimeLoadedNotification(IDebugClient* client)
{
    INIT_API_EFN();
    EnableModuleLoadUnloadCallbacks();
    return g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "sxe -c \"!SOSHandleCLRN\" clrn", 0);
}

#else // FEATURE_PAL

HRESULT HandleExceptionNotification(ILLDBServices *client)
{
    INIT_API_EFN();
    return HandleCLRNotificationEvent();
}

HRESULT HandleRuntimeLoadedNotification(ILLDBServices *client)
{
    INIT_API_EFN();
    EnableModuleLoadUnloadCallbacks();
    return g_ExtServices->SetExceptionCallback(HandleExceptionNotification);
}

#endif // FEATURE_PAL

DECLARE_API(bpmd)
{
    INIT_API_NOEE();
    MINIDUMP_NOT_SUPPORTED();
    char buffer[1024];

    if (IsDumpFile())
    {
        ExtOut("%sbpmd is not supported on a dump file.\n", SOSPrefix);
        return Status;
    }


    // We keep a list of managed breakpoints the user wants to set, and display pending bps
    // bpmd. If you call bpmd <module name> <method> we will set or update an existing bp.
    // bpmd acts as a feeder of breakpoints to bp when the time is right.
    //

    StringHolder DllName,TypeName;
    int lineNumber = 0;
    size_t Offset = 0;

    DWORD_PTR pMD = (TADDR)0;
    BOOL fNoFutureModule = FALSE;
    BOOL fList = FALSE;
    size_t clearItem = 0;
    BOOL fClearAll = FALSE;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-md", &pMD, COHEX, TRUE},
        {"-nofuturemodule", &fNoFutureModule, COBOOL, FALSE},
        {"-list", &fList, COBOOL, FALSE},
        {"-clear", &clearItem, COSIZE_T, TRUE},
        {"-clearall", &fClearAll, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&DllName.data, COSTRING},
        {&TypeName.data, COSTRING},
        {&Offset, COSIZE_T},
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    bool fBadParam = false;
    bool fIsFilename = false;
    int commandsParsed = 0;

    if (pMD != (TADDR)0)
    {
        if (nArg != 0)
        {
            fBadParam = true;
        }
        commandsParsed++;
    }
    if (fList)
    {
        commandsParsed++;
        if (nArg != 0)
        {
            fBadParam = true;
        }
    }
    if (fClearAll)
    {
        commandsParsed++;
        if (nArg != 0)
        {
            fBadParam = true;
        }
    }
    if (clearItem != 0)
    {
        commandsParsed++;
        if (nArg != 0)
        {
            fBadParam = true;
        }
    }
    if (1 <= nArg && nArg <= 3)
    {
        commandsParsed++;
        // did we get dll and type name or file:line#? Search for a colon in the first arg
        // to see if it is in fact a file:line#
        CHAR* pColon = strchr(DllName.data, ':');
        if(NULL != pColon)
        {
            fIsFilename = true;
            *pColon = '\0';
            pColon++;
            if(1 != sscanf_s(pColon, "%d", &lineNumber))
            {
                ExtOut("Unable to parse line number\n");
                fBadParam = true;
            }
            else if(lineNumber < 0)
            {
                ExtOut("Line number must be positive\n");
                fBadParam = true;
            }
            if(nArg != 1) fBadParam = 1;
        }
    }

    if (fBadParam || (commandsParsed != 1))
    {
        ExtOut("Usage: %sbpmd -md <MethodDesc pointer>\n", SOSPrefix);
        ExtOut("Usage: %sbpmd [-nofuturemodule] <module name> <managed function name> [<il offset>]\n", SOSPrefix);
        ExtOut("Usage: %sbpmd <filename>:<line number>\n", SOSPrefix);
        ExtOut("Usage: %sbpmd -list\n", SOSPrefix);
        ExtOut("Usage: %sbpmd -clear <pending breakpoint number>\n", SOSPrefix);
        ExtOut("Usage: %sbpmd -clearall\n", SOSPrefix);
        ExtOut("See \"%ssoshelp bpmd\" for more details.\n", SOSPrefix);
        return Status;
    }

    if (fList)
    {
        g_bpoints.ListBreakpoints();
        return Status;
    }
    if (clearItem != 0)
    {
        g_bpoints.ClearBreakpoint(clearItem);
        return Status;
    }
    if (fClearAll)
    {
        g_bpoints.ClearAllBreakpoints();
        return Status;
    }
    // Add a breakpoint
    // Do we already have this breakpoint?
    // Or, before setting it, is the module perhaps already loaded and code
    // is available? If so, don't add to our pending list, just go ahead and
    // set the real breakpoint.

    LPWSTR ModuleName = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));
    LPWSTR FunctionName = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));
    LPWSTR Filename = (LPWSTR)alloca(MAX_LONGPATH * sizeof(WCHAR));

    BOOL bNeedNotificationExceptions = FALSE;

    if (pMD == (TADDR)0)
    {
        int numModule = 0;
        int numMethods = 0;

        ArrayHolder<DWORD_PTR> moduleList = NULL;

        if(!fIsFilename)
        {
            MultiByteToWideChar(CP_ACP, 0, DllName.data, -1, ModuleName, mdNameLen);
            MultiByteToWideChar(CP_ACP, 0, TypeName.data, -1, FunctionName, mdNameLen);
        }
        else
        {
            MultiByteToWideChar(CP_ACP, 0, DllName.data, -1, Filename, MAX_LONGPATH);
        }

        // Get modules that may need a breakpoint bound
        if ((Status = GetRuntime(&g_pRuntime)) == S_OK)
        {
            if ((Status = LoadClrDebugDll()) != S_OK)
            {
                // if the EE is loaded but DAC isn't we should stop.
                DACMessage(Status);
                return Status;
            }
            g_bDacBroken = FALSE;                                       \

            // Get the module list
            moduleList = ModuleFromName(fIsFilename ? NULL : DllName.data, &numModule);

            // Its OK if moduleList is NULL
            // There is a very normal case when checking for modules after clr is loaded
            // but before any AppDomains or assemblies are created
            // for example:
            // >sxe ld:clr
            // >g
            // ...
            // ModLoad: clr.dll
            // >!bpmd Foo.dll Foo.Bar
        }
        // If LoadClrDebugDll() succeeded make sure we release g_clrData
        ToRelease<IXCLRDataProcess> spIDP(g_clrData);
        ToRelease<ISOSDacInterface> spISD(g_sos);
        if (g_sos != nullptr)
        {
            ResetGlobals();
        }
        // we can get here with EE not loaded => 0 modules
        //                      EE is loaded => 0 or more modules
        ArrayHolder<DWORD_PTR> pMDs = NULL;
        for (int iModule = 0; iModule < numModule; iModule++)
        {
            ToRelease<IXCLRDataModule> ModDef;
            if (g_sos->GetModule(moduleList[iModule], &ModDef) != S_OK)
            {
                continue;
            }

            HRESULT symbolsLoaded = S_FALSE;
            if(!fIsFilename)
            {
                g_bpoints.ResolvePendingNonModuleBoundBreakpoint(ModuleName, FunctionName, moduleList[iModule], (DWORD)Offset);
            }
            else
            {
                SymbolReader symbolReader;
                symbolsLoaded = g_bpoints.LoadSymbolsForModule(moduleList[iModule], &symbolReader);
                if(symbolsLoaded == S_OK &&
                   g_bpoints.ResolvePendingNonModuleBoundBreakpoint(Filename, lineNumber, moduleList[iModule], &symbolReader) == S_OK)
                {
                    // if we have symbols then get the function name so we can lookup the MethodDescs
                    mdMethodDef methodDefToken;
                    ULONG32 ilOffset;
                    if(SUCCEEDED(symbolReader.ResolveSequencePoint(Filename, lineNumber, &methodDefToken, &ilOffset)))
                    {
                        ToRelease<IXCLRDataMethodDefinition> pMethodDef = NULL;
                        if (SUCCEEDED(ModDef->GetMethodDefinitionByToken(methodDefToken, &pMethodDef)))
                        {
                            ULONG32 nameLen = 0;
                            pMethodDef->GetName(0, mdNameLen, &nameLen, FunctionName);

                            // get the size of the required buffer
                            int buffSize = WideCharToMultiByte(CP_ACP, 0, FunctionName, -1, TypeName.data, 0, NULL, NULL);

                            TypeName.data = new NOTHROW char[buffSize];
                            if (TypeName.data != NULL)
                            {
                                INDEBUG(int bytesWritten =) WideCharToMultiByte(CP_ACP, 0, FunctionName, -1, TypeName.data, buffSize, NULL, NULL);
                                _ASSERTE(bytesWritten == buffSize);
                            }
                        }
                    }
                }
            }

            HRESULT gotMethodDescs = GetMethodDescsFromName(moduleList[iModule], ModDef, TypeName.data, &pMDs, &numMethods);
            if (FAILED(gotMethodDescs) && (!fIsFilename))
            {
                // BPs via file name will enumerate through modules so there will be legitimate failures.
                // for module/type name we already found a match so this shouldn't fail (this is the original behavior).
                ExtOut("Error getting MethodDescs for module %p\n", SOS_PTR(moduleList[iModule]));
                return Status;
            }

            // for filename+line number only print extra info if symbols for this module are loaded (it can get quite noisy otherwise).
            if ((!fIsFilename) || (fIsFilename && symbolsLoaded == S_OK))
            {
                for (int i = 0; i < numMethods; i++)
                {
                    if (pMDs[i] == MD_NOT_YET_LOADED)
                    {
                        continue;
                    }
                    ExtOut("MethodDesc = %p\n", SOS_PTR(pMDs[i]));
                }
            }

            if (g_bpoints.Update(moduleList[iModule], FALSE))
            {
                bNeedNotificationExceptions = TRUE;
            }
        }

        if (!fNoFutureModule)
        {
            // add a pending breakpoint that will find future loaded modules, and
            // wait for the module load notification.
            if (!fIsFilename)
            {
                g_bpoints.Add(ModuleName, FunctionName, (TADDR)0, (DWORD)Offset);
            }
            else
            {
                g_bpoints.Add(Filename, lineNumber, (TADDR)0);
            }
            if (g_clrData != nullptr)
            {
                bNeedNotificationExceptions = TRUE;
                EnableModuleLoadUnloadCallbacks();
            }
            else
            {
#ifdef FEATURE_PAL
                Status = g_ExtServices2->SetRuntimeLoadedCallback(HandleRuntimeLoadedNotification);
#else
                g_breakOnRuntimeModuleLoad = true;
#endif
            }
        }
    }
    else /* We were given a MethodDesc already */
    {
        // if we've got an explicit MD, then we better have runtime and dac loaded
        INIT_API_EE()
        INIT_API_DAC();

        DacpMethodDescData MethodDescData;
        ExtOut("MethodDesc = %p\n", SOS_PTR(pMD));
        if (MethodDescData.Request(g_sos, TO_CDADDR(pMD)) != S_OK)
        {
            ExtOut("%p is not a valid MethodDesc\n", SOS_PTR(pMD));
            return Status;
        }

        if (MethodDescData.bHasNativeCode)
        {
            IssueDebuggerBPCommand((size_t) MethodDescData.NativeCodeAddr);
        }
        else if (MethodDescData.bIsDynamic)
        {
#ifndef FEATURE_PAL
            // Dynamic methods don't have JIT notifications. This is something we must
            // fix in the next release. Until then, you have a cumbersome user experience.
            ExtOut("This DynamicMethodDesc is not yet JITTED. Placing memory breakpoint at %p\n",
                SOS_PTR(MethodDescData.AddressOfNativeCodeSlot));

            sprintf_s(buffer, ARRAY_SIZE(buffer),
#ifdef _TARGET_WIN64_
                "ba w8"
#else
                "ba w4"
#endif // _TARGET_WIN64_

                " /1 %p \"bp poi(%p); g\"",
                SOS_PTR(MethodDescData.AddressOfNativeCodeSlot),
                SOS_PTR(MethodDescData.AddressOfNativeCodeSlot));

            Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
            if (FAILED(Status))
            {
                ExtOut("Unable to set breakpoint with IDebugControl::Execute: %x\n",Status);
                ExtOut("Attempted to run: %s\n", buffer);
            }
#else
            ExtErr("This DynamicMethodDesc is not yet JITTED %p\n", SOS_PTR(MethodDescData.AddressOfNativeCodeSlot));
#endif // FEATURE_PAL
        }
        else
        {
            // Must issue a pending breakpoint.
            if (g_sos->GetMethodDescName(pMD, mdNameLen, FunctionName, NULL) != S_OK)
            {
                ExtOut("Unable to get method name for MethodDesc %p\n", SOS_PTR(pMD));
                return Status;
            }

            FileNameForModule ((DWORD_PTR) MethodDescData.ModulePtr, ModuleName);

            // We didn't find code, add a breakpoint.
            g_bpoints.ResolvePendingNonModuleBoundBreakpoint(ModuleName, FunctionName, TO_TADDR(MethodDescData.ModulePtr), 0);
            g_bpoints.Update(TO_TADDR(MethodDescData.ModulePtr), FALSE);
            bNeedNotificationExceptions = TRUE;
        }
    }

    if (bNeedNotificationExceptions)
    {
        ExtOut("Adding pending breakpoints...\n");
#ifndef FEATURE_PAL
        Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "sxe -c \"!SOSHandleCLRN\" clrn", 0);
#else
        Status = g_ExtServices->SetExceptionCallback(HandleExceptionNotification);
#endif // FEATURE_PAL
    }

    return Status;
}

DECLARE_API(FindAppDomain)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR p_Object = (TADDR)0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&p_Object, COHEX},
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    if ((p_Object == 0) || !sos::IsObject(p_Object))
    {
        ExtOut("%p is not a valid object\n", SOS_PTR(p_Object));
        return Status;
    }

    DacpAppDomainStoreData adstore;
    if (adstore.Request(g_sos) != S_OK)
    {
        ExtOut("Error getting AppDomain information\n");
        return Status;
    }

    CLRDATA_ADDRESS appDomain = GetAppDomain (TO_CDADDR(p_Object));

    if (appDomain != (TADDR)0)
    {
        DMLOut("AppDomain: %s\n", DMLDomain(appDomain));
        if (appDomain == adstore.sharedDomain)
        {
            ExtOut("Name:      Shared Domain\n");
            ExtOut("ID:        (shared domain)\n");
        }
        else if (appDomain == adstore.systemDomain)
        {
            ExtOut("Name:      System Domain\n");
            ExtOut("ID:        (system domain)\n");
        }
        else
        {
            DacpAppDomainData domain;
            if ((domain.Request(g_sos, appDomain) != S_OK) ||
                (g_sos->GetAppDomainName(appDomain,mdNameLen,g_mdName, NULL)!=S_OK))
            {
                ExtOut("Error getting AppDomain %p.\n", SOS_PTR(appDomain));
                return Status;
            }

            ExtOut("Name:      %S\n", (g_mdName[0]!=L'\0') ? g_mdName : W("None"));
            ExtOut("ID:        %d\n", domain.dwId);
        }
    }
    else
    {
        ExtOut("The type is declared in the shared domain and other\n");
        ExtOut("methods of finding the AppDomain failed. Try running\n");
        if (IsDMLEnabled())
            DMLOut("<exec cmd=\"!gcroot /d %p\">!gcroot %p</exec>, and if you find a root on a\n", SOS_PTR(p_Object), SOS_PTR(p_Object));
        else
            ExtOut("%sgcroot %p, and if you find a root on a\n", SOSPrefix, SOS_PTR(p_Object));
        ExtOut("stack, check the AppDomain of that stack with %sclrthreads.\n", SOSPrefix);
        ExtOut("Note that the Thread could have transitioned between\n");
        ExtOut("multiple AppDomains.\n");
    }

    return Status;
}

#ifndef FEATURE_PAL

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to get the COM state (e.g. APT,contexe    *
*    activity.                                                         *
*                                                                      *
\**********************************************************************/
#ifdef FEATURE_COMINTEROP
DECLARE_API(COMState)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    ULONG numThread;
    ULONG maxId;
    g_ExtSystem->GetTotalNumberThreads(&numThread,&maxId);

    ULONG curId;
    g_ExtSystem->GetCurrentThreadId(&curId);

    SIZE_T AllocSize;
    if (!ClrSafeInt<SIZE_T>::multiply(sizeof(ULONG), numThread, AllocSize))
    {
        ExtOut("  Error!  integer overflow on numThread 0x%08x\n", numThread);
        return Status;
    }
    ULONG *ids = (ULONG*)alloca(AllocSize);
    ULONG *sysIds = (ULONG*)alloca(AllocSize);
    g_ExtSystem->GetThreadIdsByIndex(0,numThread,ids,sysIds);
#if defined(_TARGET_WIN64_)
    ExtOut("      ID             TEB  APT    APTId CallerTID          Context\n");
#else
    ExtOut("     ID     TEB   APT    APTId CallerTID Context\n");
#endif
    for (ULONG i = 0; i < numThread; i ++) {
        g_ExtSystem->SetCurrentThreadId(ids[i]);
        CLRDATA_ADDRESS cdaTeb;
        g_ExtSystem->GetCurrentThreadTeb(&cdaTeb);
        ExtOut("%3d %4x %p", ids[i], sysIds[i], SOS_PTR(CDA_TO_UL64(cdaTeb)));
        // Apartment state
        TADDR OleTlsDataAddr;
        if (SafeReadMemory(TO_TADDR(cdaTeb) + offsetof(TEB,ReservedForOle),
                            &OleTlsDataAddr,
                            sizeof(OleTlsDataAddr), NULL) && OleTlsDataAddr != 0) {
            DWORD AptState;
            if (SafeReadMemory(OleTlsDataAddr+offsetof(SOleTlsData,dwFlags),
                               &AptState,
                               sizeof(AptState), NULL)) {
                if (AptState & OLETLS_APARTMENTTHREADED) {
                    ExtOut(" STA");
                }
                else if (AptState & OLETLS_MULTITHREADED) {
                    ExtOut(" MTA");
                }
                else if (AptState & OLETLS_INNEUTRALAPT) {
                    ExtOut(" NTA");
                }
                else {
                    ExtOut(" Ukn");
                }

                // Read these fields only if we were able to read anything of the SOleTlsData structure
                DWORD dwApartmentID;
                if (SafeReadMemory(OleTlsDataAddr+offsetof(SOleTlsData,dwApartmentID),
                                   &dwApartmentID,
                                   sizeof(dwApartmentID), NULL)) {
                    ExtOut(" %8x", dwApartmentID);
                }
                else
                    ExtOut(" %8x", 0);

                DWORD dwTIDCaller;
                if (SafeReadMemory(OleTlsDataAddr+offsetof(SOleTlsData,dwTIDCaller),
                                   &dwTIDCaller,
                                   sizeof(dwTIDCaller), NULL)) {
                    ExtOut("  %8x", dwTIDCaller);
                }
                else
                    ExtOut("  %8x", 0);

                size_t Context;
                if (SafeReadMemory(OleTlsDataAddr+offsetof(SOleTlsData,pCurrentCtx),
                                   &Context,
                                   sizeof(Context), NULL)) {
                    ExtOut(" %p", SOS_PTR(Context));
                }
                else
                    ExtOut(" %p", SOS_PTR(0));

            }
            else
                ExtOut(" Ukn");
        }
        else
            ExtOut(" Ukn");
        ExtOut("\n");
    }

    g_ExtSystem->SetCurrentThreadId(curId);
    return Status;
}
#endif // FEATURE_COMINTEROP

#endif // FEATURE_PAL

BOOL traverseEh(UINT clauseIndex,UINT totalClauses,DACEHInfo *pEHInfo,LPVOID token)
{
    size_t methodStart = (size_t) token;

    if (IsInterrupt())
    {
        return FALSE;
    }

    ExtOut("EHHandler %d: %s ", clauseIndex, EHTypeName(pEHInfo->clauseType));

    LPCWSTR typeName = EHTypedClauseTypeName(pEHInfo);
    if (typeName != NULL)
    {
        ExtOut("catch(%S) ", typeName);
    }

    if (IsClonedFinally(pEHInfo))
        ExtOut("(cloned finally)");
    else if (pEHInfo->isDuplicateClause)
        ExtOut("(duplicate)");

    ExtOut("\n");
    ExtOut("Clause:  ");

    ULONG64 addrStart = pEHInfo->tryStartOffset + methodStart;
    ULONG64 addrEnd   = pEHInfo->tryEndOffset   + methodStart;

#ifdef _WIN64
    ExtOut("[%08x`%08x, %08x`%08x]",
            (ULONG)(addrStart >> 32), (ULONG)addrStart,
            (ULONG)(addrEnd   >> 32), (ULONG)addrEnd);
#else
    ExtOut("[%08x, %08x]", (ULONG)addrStart, (ULONG)addrEnd);
#endif

    ExtOut(" [%x, %x]\n",
        (UINT32) pEHInfo->tryStartOffset,
        (UINT32) pEHInfo->tryEndOffset);

    ExtOut("Handler: ");

    addrStart = pEHInfo->handlerStartOffset + methodStart;
    addrEnd   = pEHInfo->handlerEndOffset   + methodStart;

#ifdef _WIN64
    ExtOut("[%08x`%08x, %08x`%08x]",
            (ULONG)(addrStart >> 32), (ULONG)addrStart,
            (ULONG)(addrEnd   >> 32), (ULONG)addrEnd);
#else
    ExtOut("[%08x, %08x]", (ULONG)addrStart, (ULONG)addrEnd);
#endif

    ExtOut(" [%x, %x]\n",
        (UINT32) pEHInfo->handlerStartOffset,
        (UINT32) pEHInfo->handlerEndOffset);

    if (pEHInfo->clauseType == EHFilter)
    {
        ExtOut("Filter: ");

        addrStart = pEHInfo->filterOffset + methodStart;

#ifdef _WIN64
        ExtOut("[%08x`%08x]", (ULONG)(addrStart >> 32), (ULONG)addrStart);
#else
        ExtOut("[%08x]", (ULONG)addrStart);
#endif

        ExtOut(" [%x]\n",
            (UINT32) pEHInfo->filterOffset);
    }

    ExtOut("\n");
    return TRUE;
}

DECLARE_API(EHInfo)
{
    INIT_API_PROBE_MANAGED("ehinfo");
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR dwStartAddr = (TADDR)0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };

    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX},
    };

    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg) || (0 == nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    DWORD_PTR tmpAddr = dwStartAddr;

    if (!IsMethodDesc(dwStartAddr))
    {
        JITTypes jitType;
        DWORD_PTR methodDesc;
        DWORD_PTR gcinfoAddr;
        IP2MethodDesc (dwStartAddr, methodDesc, jitType, gcinfoAddr);
        tmpAddr = methodDesc;
    }

    DacpMethodDescData MD;
    if ((tmpAddr == 0) || (MD.Request(g_sos, TO_CDADDR(tmpAddr)) != S_OK))
    {
        ExtOut("%p is not a MethodDesc\n", SOS_PTR(tmpAddr));
        return Status;
    }

    if (1 == nArg && !MD.bHasNativeCode)
    {
        ExtOut("No EH info available\n");
        return Status;
    }

    DacpCodeHeaderData codeHeaderData;
    if (codeHeaderData.Request(g_sos, TO_CDADDR(MD.NativeCodeAddr)) != S_OK)
    {
        ExtOut("Unable to get codeHeader information\n");
        return Status;
    }

    DMLOut("MethodDesc:   %s\n", DMLMethodDesc(MD.MethodDescPtr));
    DumpMDInfo(TO_TADDR(MD.MethodDescPtr));

    ExtOut("\n");
    Status = g_sos->TraverseEHInfo(TO_CDADDR(MD.NativeCodeAddr), traverseEh, (LPVOID)MD.NativeCodeAddr);

    if (Status == E_ABORT)
    {
        ExtOut("<user aborted>\n");
    }
    else if (Status != S_OK)
    {
        ExtOut("Failed to perform EHInfo traverse\n");
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the GC encoding of a managed      *
*    function.                                                         *
*                                                                      *
\**********************************************************************/
DECLARE_API(GCInfo)
{
    INIT_API_PROBE_MANAGED("gcinfo");
    MINIDUMP_NOT_SUPPORTED();

    TADDR taStartAddr = (TADDR)0;
    TADDR taGCInfoAddr;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&taStartAddr, COHEX},
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg) || (0 == nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    TADDR tmpAddr = taStartAddr;

    if (!IsMethodDesc(taStartAddr))
    {
        JITTypes jitType;
        DWORD_PTR methodDesc;
        DWORD_PTR gcinfoAddr;
        IP2MethodDesc(taStartAddr, methodDesc, jitType, gcinfoAddr);
        tmpAddr = methodDesc;
    }

    DacpMethodDescData MD;
    if ((tmpAddr == 0) || (MD.Request(g_sos, TO_CDADDR(tmpAddr)) != S_OK))
    {
        ExtOut("%p is not a valid MethodDesc\n", SOS_PTR(taStartAddr));
        return Status;
    }

    if (1 == nArg && !MD.bHasNativeCode)
    {
        ExtOut("No GC info available\n");
        return Status;
    }

    DacpCodeHeaderData codeHeaderData;

    if (
        // Try to get code header data from taStartAddr.  This will get the code
        // header corresponding to the IP address, even if the function was rejitted
        (codeHeaderData.Request(g_sos, TO_CDADDR(taStartAddr)) != S_OK) &&

        // If that didn't work, just try to use the code address that the MD
        // points to.  If the function was rejitted, this will only give you the
        // original JITted code, but that's better than nothing
        (codeHeaderData.Request(g_sos, TO_CDADDR(MD.NativeCodeAddr)) != S_OK)
        )
    {
        // We always used to emit this (before rejit support), even if we couldn't get
        // the code header, so keep on doing so.
        ExtOut("entry point %p\n", SOS_PTR(MD.NativeCodeAddr));

        // And now the error....
        ExtOut("Unable to get codeHeader information\n");
        return Status;
    }

    // We have the code header, so use it to determine the method start

    ExtOut("entry point %p\n", SOS_PTR(codeHeaderData.MethodStart));

    if (codeHeaderData.JITType == TYPE_UNKNOWN)
    {
        ExtOut("unknown Jit\n");
        return Status;
    }
    else if (codeHeaderData.JITType == TYPE_JIT)
    {
        ExtOut("Normal JIT generated code\n");
    }
    else if (codeHeaderData.JITType == TYPE_PJIT)
    {
        ExtOut("preJIT generated code\n");
    }

    taGCInfoAddr = TO_TADDR(codeHeaderData.GCInfo);

    ExtOut("GC info %p\n", SOS_PTR(taGCInfoAddr));

    // assume that GC encoding table is never more than
    // 40 + methodSize * 2
    int tableSize = 0;
    if (!ClrSafeInt<int>::multiply(codeHeaderData.MethodSize, 2, tableSize) ||
        !ClrSafeInt<int>::addition(tableSize, 40, tableSize))
    {
        ExtOut("<integer overflow>\n");
        return E_FAIL;
    }
    ArrayHolder<BYTE> table = new NOTHROW BYTE[tableSize];
    if (table == NULL)
    {
        ExtOut("Could not allocate memory to read the gc info.\n");
        return E_OUTOFMEMORY;
    }

    memset(table, 0, tableSize);
    // We avoid using move here, because we do not want to return
    if (!SafeReadMemory(taGCInfoAddr, table, tableSize, NULL))
    {
        ExtOut("Could not read memory %p\n", SOS_PTR(taGCInfoAddr));
        return Status;
    }

    // Mutable table pointer since we need to pass the appropriate
    // offset into the table to DumpGCTable.
    GCInfoToken gcInfoToken = { table, GCInfoVersion() };
    unsigned int methodSize = (unsigned int)codeHeaderData.MethodSize;

    g_targetMachine->DumpGCInfo(gcInfoToken, methodSize, ExtOut, true /*encBytes*/, true /*bPrintHeader*/);

    return Status;
}

GCEncodingInfo g_gcEncodingInfo; // The constructor should run to create the initial buffer allocation.

void DecodeGCTableEntry (const char *fmt, ...)
{
    va_list va;

    //
    // Append the new data to the buffer. If it doesn't fit, allocate a new buffer that is bigger and try again.
    //

    va_start(va, fmt);

    // Make sure there's at least a minimum amount of free space in the buffer. We need to minimally
    // ensure that 'maxCchToWrite' is >0. 20 is an arbitrary smallish number.
    if (!g_gcEncodingInfo.EnsureAdequateBufferSpace(20))
    {
        ExtOut("Could not allocate memory for GC info\n");
        return;
    }

    while (true)
    {
        char* buffer = &g_gcEncodingInfo.buf[g_gcEncodingInfo.cchBuf];
        size_t sizeOfBuffer = g_gcEncodingInfo.cchBufAllocation - g_gcEncodingInfo.cchBuf;
        size_t maxCchToWrite = sizeOfBuffer - 1; // -1 to leave space for the null terminator
        int cch = _vsnprintf_s(buffer, sizeOfBuffer, maxCchToWrite, fmt, va);

        // cch == -1 should be the only negative result, but checking < 0 is defensive in case some runtime returns something else.
        // We should also check "errno == ERANGE", but it seems that some runtimes don't set that properly.
        if (cch < 0)
        {
            if (sizeOfBuffer > 1000)
            {
                // There must be some unexpected problem if we can't write the GC info into such a large buffer, so bail.
                ExtOut("Error generating GC info\n");
                break;
            }
            else if (!g_gcEncodingInfo.ReallocBuf())
            {
                // We couldn't reallocate the buffer; skip the rest of the text.
                ExtOut("Could not allocate memory for GC info\n");
                break;
            }

            // If we get here, we successfully reallocated the buffer larger, so we'll try again to write this entry
            // into the larger buffer.
        }
        else
        {
            // We successfully added this entry to the GC info we're accumulating.
            // cch is the number of characters written, not including the terminating null.
            g_gcEncodingInfo.cchBuf += cch;
            break;
        }
    }

    va_end(va);
}

BOOL gatherEh(UINT clauseIndex,UINT totalClauses,DACEHInfo *pEHInfo,LPVOID token)
{
    SOSEHInfo *pInfo = (SOSEHInfo *) token;

    if (pInfo == NULL)
    {
        return FALSE;
    }

    if (pInfo->m_pInfos == NULL)
    {
        // First time, initialize structure
        pInfo->EHCount = totalClauses;
        pInfo->m_pInfos = new NOTHROW DACEHInfo[totalClauses];
        if (pInfo->m_pInfos == NULL)
        {
            ReportOOM();
            return FALSE;
        }
    }

    pInfo->m_pInfos[clauseIndex] = *((DACEHInfo*)pEHInfo);
    return TRUE;
}

HRESULT
GetClrMethodInstance(
    ___in ULONG64 NativeOffset,
    ___out IXCLRDataMethodInstance** Method);

typedef std::tuple<DacpMethodDescData, DacpCodeHeaderData, HRESULT> ExtractionCodeHeaderResult;

ExtractionCodeHeaderResult extractCodeHeaderData(DWORD_PTR methodDesc, DWORD_PTR dwStartAddr);
HRESULT displayGcInfo(BOOL fWithGCInfo, const DacpCodeHeaderData& codeHeaderData);
HRESULT GetIntermediateLangMap(BOOL bIL, const DacpCodeHeaderData& codeHeaderData,
                               std::unique_ptr<CLRDATA_IL_ADDRESS_MAP[]>& map,
                               ULONG32& mapCount,
                               BOOL dumpMap);

GetILAddressResult GetILAddress(const DacpMethodDescData& MethodDescData)
{
    GetILAddressResult error = std::make_tuple((TADDR)0, nullptr);
    TADDR ilAddr = (TADDR)0;
    struct DacpProfilerILData ilData;
    ReleaseHolder<ISOSDacInterface7> sos7;
    if (SUCCEEDED(g_sos->QueryInterface(__uuidof(ISOSDacInterface7), &sos7)) &&
        SUCCEEDED(sos7->GetProfilerModifiedILInformation(MethodDescData.MethodDescPtr, &ilData)))
    {
        if (ilData.type == DacpProfilerILData::ILModified)
        {
            ExtOut("Found profiler modified IL\n");
            ilAddr = TO_TADDR(ilData.il);
        }
    }

    // Factor this so that it returns a map from IL offset to the textual representation of the decoding
    // to be consumed by !u -il
    // The disassemble function can give a MethodDescData as well as the set of keys IL offsets

    // This is not a dynamic method, print the IL for it.
    // Get the module
    DacpModuleData dmd;
    if (dmd.Request(g_sos, MethodDescData.ModulePtr) != S_OK)
    {
        ExtOut("Unable to get module\n");
        return error;
    }

    ToRelease<IMetaDataImport> pImport(MDImportForModule(&dmd));
    if (pImport == NULL)
    {
        ExtOut("bad import\n");
        return error;
    }

    if (ilAddr == (TADDR)0)
    {
        ULONG pRva;
        DWORD dwFlags;
        if (pImport->GetRVA(MethodDescData.MDToken, &pRva, &dwFlags) != S_OK)
        {
            ExtOut("error in import\n");
            return error;
        }

        CLRDATA_ADDRESS ilAddrClr;
        if (g_sos->GetILForModule(MethodDescData.ModulePtr, pRva, &ilAddrClr) != S_OK)
        {
            ExtOut("FindIL failed\n");
            return error;
        }

        ilAddr = TO_TADDR(ilAddrClr);
    }

    if (ilAddr == (TADDR)0)
    {
        ExtOut("Unknown error in reading function IL\n");
        return error;
    }
    GetILAddressResult result = std::make_tuple(ilAddr, pImport.Detach());
    return result;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to unassembly a managed function.         *
*    It tries to print symbolic info for function call, contants...    *
*                                                                      *
\**********************************************************************/
DECLARE_API(u)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    DWORD_PTR dwStartAddr = (TADDR)0;
    BOOL fWithGCInfo = FALSE;
    BOOL fWithEHInfo = FALSE;
    BOOL bSuppressLines = FALSE;
    BOOL bDisplayOffsets = FALSE;
    BOOL bDisplayILMap = FALSE;
    BOOL bIL = FALSE;
    BOOL dml = FALSE;
    size_t nArg;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-gcinfo", &fWithGCInfo, COBOOL, FALSE},
        {"-ehinfo", &fWithEHInfo, COBOOL, FALSE},
        {"-n", &bSuppressLines, COBOOL, FALSE},
        {"-o", &bDisplayOffsets, COBOOL, FALSE},
        {"-il", &bIL, COBOOL, FALSE},
        {"-map", &bDisplayILMap, COBOOL, FALSE},
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&dwStartAddr, COHEX},
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg) || (nArg < 1))
    {
        return E_INVALIDARG;
    }
    // symlines will be non-zero only if SYMOPT_LOAD_LINES was set in the symbol options
    ULONG symlines = 0;
    if (!bSuppressLines && SUCCEEDED(g_ExtSymbols->GetSymbolOptions(&symlines)))
    {
        symlines &= SYMOPT_LOAD_LINES;
    }
    bSuppressLines = bSuppressLines || (symlines == 0);

    EnableDMLHolder dmlHolder(dml);
    // dwStartAddr is either some IP address or a MethodDesc.  Start off assuming it's a
    // MethodDesc.
    DWORD_PTR methodDesc = dwStartAddr;
    if (!IsMethodDesc(methodDesc))
    {
        // Not a methodDesc, so gotta find it ourselves
        DWORD_PTR tmpAddr = dwStartAddr;
        JITTypes jt;
        DWORD_PTR gcinfoAddr;
        IP2MethodDesc (tmpAddr, methodDesc, jt,
                       gcinfoAddr);
        if (!methodDesc || jt == TYPE_UNKNOWN)
        {
            // It is not managed code.
            ExtOut("Unmanaged code\n");
            UnassemblyUnmanaged(dwStartAddr, bSuppressLines);
            return Status;
        }
    }

    ExtractionCodeHeaderResult p = extractCodeHeaderData(methodDesc, dwStartAddr);
    Status = std::get<2>(p);
    if (Status != S_OK)
    {
        return Status;
    }

    NameForMD_s(methodDesc, g_mdName, mdNameLen);
    ExtOut("%S\n", g_mdName);

    DacpMethodDescData& MethodDescData = std::get<0>(p);
    DacpCodeHeaderData& codeHeaderData = std::get<1>(p);
    std::unique_ptr<CLRDATA_IL_ADDRESS_MAP[]> map(nullptr);
    ULONG32 mapCount = 0;
    Status = GetIntermediateLangMap(bIL, codeHeaderData, map /*out*/, mapCount /* out */, bDisplayILMap);
    if (Status != S_OK)
    {
        return Status;
    }

    // ///////////////////////////////////////////////////////////////////////////
    // This can be reused with sildasm but kept as-is largely since it just
    // works so it can be fixed later.
    // ///////////////////////////////////////////////////////////////////////////

    if (MethodDescData.bIsDynamic && MethodDescData.managedDynamicMethodObject)
    {
        ExtOut("Can only work with dynamic not implemented\n");
        return Status;
    }

    GetILAddressResult result = GetILAddress(MethodDescData);
    if (std::get<0>(result) == (TADDR)0)
    {
        ExtOut("ilAddr is %p\n", SOS_PTR(std::get<0>(result)));
        return E_FAIL;
    }
    ExtOut("ilAddr is %p pImport is %p\n", SOS_PTR(std::get<0>(result)), SOS_PTR(std::get<1>(result)));
    TADDR ilAddr = std::get<0>(result);
    ToRelease<IMetaDataImport> pImport(std::get<1>(result));

    /// Taken from DecodeILFromAddress(IMetaDataImport *pImport, TADDR ilAddr)
    ULONG Size = GetILSize(ilAddr);
    if (Size == 0)
    {
        ExtOut("error decoding IL\n");
        return Status;
    }
    // Read the memory into a local buffer
    ArrayHolder<BYTE> pArray = new BYTE[Size];
    Status = g_ExtData->ReadVirtual(TO_CDADDR(ilAddr), pArray, Size, NULL);
    if (Status != S_OK)
    {
        ExtOut("Failed to read memory\n");
        return Status;
    }
    /// Taken from DecodeIL(pImport, pArray, Size);
    // First decode the header
    BYTE *buffer = pArray;
    ULONG bufSize = Size;
    COR_ILMETHOD *pHeader = (COR_ILMETHOD *) buffer;
    COR_ILMETHOD_DECODER header(pHeader);
    ULONG position = 0;
    BYTE* pBuffer = const_cast<BYTE*>(header.Code);
    UINT indentCount = 0;
    ULONG endCodePosition = header.GetCodeSize();
    struct ILLocationRange {
        ULONG mStartPosition;
        ULONG mEndPosition;
        BYTE* mStartAddress;
        BYTE* mEndAddress;
    };
    std::deque<ILLocationRange> ilCodePositions;

    if (mapCount > 0)
    {
        while (position < endCodePosition)
        {
            ULONG mapIndex = 0;
            do
            {
                while ((mapIndex < mapCount) && (position != map[mapIndex].ilOffset))
                {
                    ++mapIndex;
                }
                if (map[mapIndex].endAddress > map[mapIndex].startAddress)
                {
                    break;
                }
                ++mapIndex;
            } while (mapIndex < mapCount);
            std::tuple<ULONG, UINT> r = DecodeILAtPosition(
                pImport, pBuffer, bufSize,
                position, indentCount, header);
            ExtOut("\n");
            if (mapIndex < mapCount)
            {
                ILLocationRange entry = {
                    position,
                    std::get<0>(r) - 1,
                    (BYTE*)map[mapIndex].startAddress,
                    (BYTE*)map[mapIndex].endAddress
                };
                ilCodePositions.push_back(std::move(entry));
            }
            else
            {
                if (!ilCodePositions.empty())
                {
                    auto& entry = ilCodePositions.back();
                    entry.mEndPosition = position;
                }
            }
            position = std::get<0>(r);
            indentCount = std::get<1>(r);
        }
    }

    position = 0;
    indentCount = 0;
    std::function<void(ULONG*, UINT*, BYTE*)> displayILFun =
        [&pImport, &pBuffer, bufSize, &header, &ilCodePositions](ULONG *pPosition, UINT *pIndentCount,
                                                BYTE *pIp) -> void {
                for (auto iter = ilCodePositions.begin(); iter != ilCodePositions.end(); ++iter)
                {
                    if ((pIp >= iter->mStartAddress) && (pIp < iter->mEndAddress))
                    {
                        ULONG position = iter->mStartPosition;
                        ULONG endPosition = iter->mEndPosition;
                        while (position <= endPosition)
                        {
                            std::tuple<ULONG, UINT> r = DecodeILAtPosition(
                                pImport, pBuffer, bufSize,
                                position, *pIndentCount, header);
                            ExtOut("\n");
                            position = std::get<0>(r);
                            *pIndentCount = std::get<1>(r);
                        }
                        ilCodePositions.erase(iter);
                        break;
                    }
                }
    };

    if (codeHeaderData.ColdRegionStart != (TADDR)0)
    {
        ExtOut("Begin %p, size %x. Cold region begin %p, size %x\n",
            SOS_PTR(codeHeaderData.MethodStart), codeHeaderData.HotRegionSize,
            SOS_PTR(codeHeaderData.ColdRegionStart), codeHeaderData.ColdRegionSize);
    }
    else
    {
        ExtOut("Begin %p, size %x\n", SOS_PTR(codeHeaderData.MethodStart), codeHeaderData.MethodSize);
    }

    Status = displayGcInfo(fWithGCInfo, codeHeaderData);
    if (Status != S_OK)
    {
        return Status;
    }

    SOSEHInfo *pInfo = NULL;
    if (fWithEHInfo)
    {
        pInfo = new NOTHROW SOSEHInfo;
        if (pInfo == NULL)
        {
            ReportOOM();
        }
        else if (g_sos->TraverseEHInfo(codeHeaderData.MethodStart, gatherEh, (LPVOID)pInfo) != S_OK)
        {
            ExtOut("Failed to gather EHInfo data\n");
            delete pInfo;
            pInfo = NULL;
        }
    }

    if (codeHeaderData.ColdRegionStart == (TADDR)0)
    {
        g_targetMachine->Unassembly (
                (DWORD_PTR) codeHeaderData.MethodStart,
                ((DWORD_PTR)codeHeaderData.MethodStart) + codeHeaderData.MethodSize,
                dwStartAddr,
                (DWORD_PTR) MethodDescData.GCStressCodeCopy,
                fWithGCInfo ? &g_gcEncodingInfo : NULL,
                pInfo,
                bSuppressLines,
                bDisplayOffsets,
                displayILFun);
    }
    else
    {
        ExtOut("Hot region:\n");
        g_targetMachine->Unassembly (
                (DWORD_PTR) codeHeaderData.MethodStart,
                ((DWORD_PTR)codeHeaderData.MethodStart) + codeHeaderData.HotRegionSize,
                dwStartAddr,
                (DWORD_PTR) MethodDescData.GCStressCodeCopy,
                fWithGCInfo ? &g_gcEncodingInfo : NULL,
                pInfo,
                bSuppressLines,
                bDisplayOffsets,
                displayILFun);

        ExtOut("Cold region:\n");

        // Displaying gcinfo for a cold region requires knowing the size of
        // the hot region preceeding.
        g_gcEncodingInfo.hotSizeToAdd = codeHeaderData.HotRegionSize;

        g_targetMachine->Unassembly (
                (DWORD_PTR) codeHeaderData.ColdRegionStart,
                ((DWORD_PTR)codeHeaderData.ColdRegionStart) + codeHeaderData.ColdRegionSize,
                dwStartAddr,
                ((DWORD_PTR) MethodDescData.GCStressCodeCopy) + codeHeaderData.HotRegionSize,
                fWithGCInfo ? &g_gcEncodingInfo : NULL,
                pInfo,
                bSuppressLines,
                bDisplayOffsets,
                displayILFun);

    }

    if (pInfo)
    {
        delete pInfo;
        pInfo = NULL;
    }

    if (fWithGCInfo)
    {
        g_gcEncodingInfo.Deinitialize();
    }

    return Status;
}

inline ExtractionCodeHeaderResult extractCodeHeaderData(DWORD_PTR methodDesc, DWORD_PTR dwStartAddr)
{
    DacpMethodDescData MethodDescData;
    HRESULT Status =
        g_sos->GetMethodDescData(
            TO_CDADDR(methodDesc),
            dwStartAddr == methodDesc ? (TADDR)0 : dwStartAddr,
            &MethodDescData,
            0, // cRevertedRejitVersions
            NULL, // rgRevertedRejitData
            NULL); // pcNeededRevertedRejitData
    if (Status != S_OK)
    {
        ExtOut("Failed to get method desc for %p.\n", SOS_PTR(dwStartAddr));
        return ExtractionCodeHeaderResult(std::move(MethodDescData), DacpCodeHeaderData(), Status);
    }

    if (!MethodDescData.bHasNativeCode)
    {
        ExtOut("Not jitted yet\n");
        return ExtractionCodeHeaderResult(std::move(MethodDescData), DacpCodeHeaderData(), S_FALSE);
    }

    // Get the appropriate code header. If we were passed an MD, then use
    // MethodDescData.NativeCodeAddr to find the code header; if we were passed an IP, use
    // that IP to find the code header. This ensures that, for rejitted functions, we
    // disassemble the rejit version that the user explicitly specified with their IP.
    DacpCodeHeaderData codeHeaderData;
    if (codeHeaderData.Request(
        g_sos,
        TO_CDADDR(
        (dwStartAddr == methodDesc) ? MethodDescData.NativeCodeAddr : dwStartAddr)
    ) != S_OK)
    {
        ExtOut("Unable to get codeHeader information\n");
        return ExtractionCodeHeaderResult(std::move(MethodDescData), DacpCodeHeaderData(), S_FALSE);
    }

    if (codeHeaderData.MethodStart == 0)
    {
        ExtOut("not a valid MethodDesc\n");
        return ExtractionCodeHeaderResult(std::move(MethodDescData), DacpCodeHeaderData(), S_FALSE);
    }

    if (codeHeaderData.JITType == TYPE_UNKNOWN)
    {
        ExtOut("unknown Jit\n");
        return ExtractionCodeHeaderResult(std::move(MethodDescData), DacpCodeHeaderData(), S_FALSE);
    }
    else if (codeHeaderData.JITType == TYPE_JIT)
    {
        ExtOut("Normal JIT generated code\n");
    }
    else if (codeHeaderData.JITType == TYPE_PJIT)
    {
        ExtOut("preJIT generated code\n");
    }
    return ExtractionCodeHeaderResult(std::move(MethodDescData), std::move(codeHeaderData), S_OK);
}

HRESULT displayGcInfo(BOOL fWithGCInfo, const DacpCodeHeaderData& codeHeaderData)
{
    //
    // Set up to mix gc info with the code if requested. To do this, we first generate all the textual
    // gc info up front. This text is the same as the "!gcinfo" command, and looks like:
    //
    // Prolog size: 0
    // Security object: <none>
    // GS cookie: <none>
    // PSPSym: <none>
    // Generics inst context: <none>
    // PSP slot: <none>
    // GenericInst slot: <none>
    // Varargs: 0
    // Frame pointer: rbp
    // Wants Report Only Leaf: 0
    // Size of parameter area: 20
    // Return Kind: Scalar
    // Code size: 1ec
    // Untracked: +rbp-10 +rbp-30 +rbp-48 +rbp-50 +rbp-58 +rbp-60 +rbp-68 +rbp-70
    // 0000001e interruptible
    // 0000003c +rax
    // 0000004d +rdx
    // 00000051 +rcx
    // 00000056 -rdx -rcx -rax
    // 0000005a +rcx
    // 00000067 -rcx
    // 00000080 +rcx
    // 00000085 -rcx
    // 0000009e +rcx
    // 000000a3 -rcx
    // 000000bc +rcx
    // 000000c1 -rcx
    // 000000d7 +rcx
    // 000000e5 -rcx
    // 000000ef +rax
    // 0000010a +r8
    // 00000119 +rcx
    // 00000120 -r8 -rcx -rax
    // 0000012f +rax
    // 00000137 +r8
    // 00000146 +rcx
    // 00000150 -r8 -rcx -rax
    // 0000015f +rax
    // 00000167 +r8
    // 00000176 +rcx
    // 00000180 -r8 -rcx -rax
    // 0000018f +rax
    // 00000197 +r8
    // 000001a6 +rcx
    // 000001b0 -r8 -rcx -rax
    // 000001b4 +rcx
    // 000001b8 +rdx
    // 000001bd -rdx -rcx
    // 000001c8 +rcx
    // 000001cd -rcx
    // 000001d2 +rcx
    // 000001d7 -rcx
    // 000001e5 not interruptible
    //
    // For the entries without offset prefixes, we output them before the first offset of code.
    // (Previously, we only displayed the "Untracked:" element, but displaying all this additional
    // GC info is useful, and then the user doesn't need to also do a "!gcinfo" to see it.)
    // For the entries with offset prefixes, we parse the offset, and display all relevant information
    // before the current instruction offset being disassembled, that is, all the lines of GC info
    // with an offset greater than the previous instruction and with an offset less than or equal
    // to the offset of the current instruction.

    // The actual GC Encoding Table, this is updated during the course of the function.
    // Use a holder to make sure we clean up the memory for the table.
    ArrayHolder<BYTE> table = NULL;

    if (fWithGCInfo)
    {
        // assume that GC encoding table is never more than 40 + methodSize * 2
        int tableSize = 0;
        if (!ClrSafeInt<int>::multiply(codeHeaderData.MethodSize, 2, tableSize) ||
            !ClrSafeInt<int>::addition(tableSize, 40, tableSize))
        {
            ExtOut("<integer overflow>\n");
            return E_FAIL;
        }

        // Assign the new array to the mutable gcEncodingInfo table and to the
        // table ArrayHolder to clean this up when the function exits.
        table = new NOTHROW BYTE[tableSize];
        if (table == NULL)
        {
            ExtOut("Could not allocate memory to read the gc info.\n");
            return E_OUTOFMEMORY;
        }

        memset(table, 0, tableSize);
        // We avoid using move here, because we do not want to return
        if (!SafeReadMemory(TO_TADDR(codeHeaderData.GCInfo), table, tableSize, NULL))
        {
            ExtOut("Could not read memory %p\n", SOS_PTR(codeHeaderData.GCInfo));
            return ERROR_INVALID_DATA;
        }

        //
        // Skip the info header
        //
        unsigned int methodSize = (unsigned int)codeHeaderData.MethodSize;

        if (!g_gcEncodingInfo.Initialize())
        {
            return E_OUTOFMEMORY;
        }

        GCInfoToken gcInfoToken = { table, GCInfoVersion() };
        g_targetMachine->DumpGCInfo(gcInfoToken, methodSize, DecodeGCTableEntry, false /*encBytes*/, false /*bPrintHeader*/);
    }
    return S_OK;
}

HRESULT GetIntermediateLangMap(BOOL bIL, const DacpCodeHeaderData& codeHeaderData,
                               std::unique_ptr<CLRDATA_IL_ADDRESS_MAP[]>& map,
                               ULONG32& mapCount,
                               BOOL dumpMap)
{
    HRESULT Status = S_OK;
    if (bIL)
    {
        ToRelease<IXCLRDataMethodInstance> pMethodInst(NULL);

        if ((Status = GetClrMethodInstance(codeHeaderData.MethodStart, &pMethodInst)) != S_OK)
        {
            return Status;
        }

        if ((Status = pMethodInst->GetILAddressMap(mapCount, &mapCount, map.get())) != S_OK)
        {
            return Status;
        }

        map.reset(new NOTHROW CLRDATA_IL_ADDRESS_MAP[mapCount]);
        if (map.get() == NULL)
        {
            ReportOOM();
            return E_OUTOFMEMORY;
        }

        if ((Status = pMethodInst->GetILAddressMap(mapCount, &mapCount, map.get())) != S_OK)
        {
            return Status;
        }

        if (dumpMap)
        {
            for (ULONG32 i = 0; i < mapCount; i++)
            {
                // TODO: These information should be interleaved with the disassembly
                // Decoded IL can be obtained through refactoring DumpIL code.
                ExtOut("%08x %p %p\n", map[i].ilOffset, SOS_PTR(map[i].startAddress), SOS_PTR(map[i].endAddress));
            }
        }
    }
    return S_OK;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the in-memory stress log          *
*    !DumpLog [filename]                                               *
*             will dump the stress log corresponding to the clr.dll    *
*             loaded in the debuggee's VAS                             *
*    !DumpLog -addr <addr_of_StressLog::theLog> [filename]             *
*             will dump the stress log associated with any DLL linked  *
*             against utilcode.lib, most commonly mscordbi.dll         *
*             (e.g. !DumpLog -addr mscordbi!StressLog::theLog)         *
*                                                                      *
\**********************************************************************/
DECLARE_API(DumpLog)
{
    INIT_API_NO_RET_ON_FAILURE("dumplog");
    MINIDUMP_NOT_SUPPORTED();
    _ASSERTE(g_pRuntime != nullptr);

    // Not supported on desktop runtime
    if (g_pRuntime->GetRuntimeConfiguration() == IRuntime::WindowsDesktop)
    {
        ExtErr("DumpLog not supported on desktop runtime\n");
        return E_FAIL;
    }

    if (CheckBreakingRuntimeChange())
    {
        return E_FAIL;
    }

    LoadRuntimeSymbols();

    const char* fileName = "StressLog.txt";
    CLRDATA_ADDRESS StressLogAddress = (TADDR)0;

    StringHolder sFileName, sLogAddr;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-addr", &sLogAddr.data, COSTRING, TRUE}
    };
    CMDValue arg[] =
    {   // vptr, type
        {&sFileName.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg > 0 && sFileName.data != NULL)
    {
        fileName = sFileName.data;
    }

    // allow users to specify -addr mscordbdi!StressLog::theLog, for example.
    if (sLogAddr.data != NULL)
    {
        StressLogAddress = GetExpression(sLogAddr.data);
    }

    if (StressLogAddress == (TADDR)0)
    {
        if (g_bDacBroken)
        {
#ifndef FEATURE_PAL
            if (IsWindowsTarget())
            {
                // Try to find stress log symbols
                DWORD_PTR dwAddr = GetValueFromExpression("StressLog::theLog");
                StressLogAddress = dwAddr;
            }
            else
#endif
            {
                ExtOut("No stress log address. DAC is broken; can't get it\n");
                return E_FAIL;
            }
        }
        else if (g_sos->GetStressLogAddress(&StressLogAddress) != S_OK)
        {
            ExtOut("Unable to find stress log via DAC\n");
            return E_FAIL;
        }
    }

    if (StressLogAddress == (TADDR)0)
    {
        ExtOut("Please provide the -addr argument for the address of the stress log, since no recognized runtime is loaded.\n");
        return E_FAIL;
    }

    ExtOut("Attempting to dump Stress log to file '%s'\n", fileName);



    Status = StressLog::Dump(StressLogAddress, fileName, g_ExtData);

    if (Status == S_OK)
        ExtOut("SUCCESS: Stress log dumped\n");
    else if (Status == S_FALSE)
        ExtOut("No Stress log in the image, no file written\n");
    else
        ExtOut("FAILURE: Stress log not dumped\n");

    return Status;
}

#ifdef TRACE_GC

DECLARE_API (DumpGCLog)
{
    INIT_API_NODAC();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    const char* fileName = "GCLog.txt";

    while (isspace (*args))
        args ++;

    if (*args != 0)
        fileName = args;

    DWORD_PTR dwAddr = GetValueFromExpression("SVR::gc_log_buffer");
    moveN (dwAddr, dwAddr);

    if (dwAddr == 0)
    {
        dwAddr = GetValueFromExpression("WKS::gc_log_buffer");
        moveN (dwAddr, dwAddr);
        if (dwAddr == 0)
        {
            ExtOut("Can't get either WKS or SVR GC's log file");
            return E_FAIL;
        }
    }

    ExtOut("Dumping GC log at %08x\n", dwAddr);

    g_bDacBroken = FALSE;

    ExtOut("Attempting to dump GC log to file '%s'\n", fileName);

    Status = E_FAIL;

    HANDLE hGCLog = CreateFileA(
        fileName,
        GENERIC_WRITE,
        FILE_SHARE_READ,
        NULL,
        CREATE_ALWAYS,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (hGCLog == INVALID_HANDLE_VALUE)
    {
        ExtOut("failed to create file: %d\n", GetLastError());
        goto exit;
    }

    int iLogSize = 1024*1024;
    BYTE* bGCLog = new NOTHROW BYTE[iLogSize];
    if (bGCLog == NULL)
    {
        ReportOOM();
        goto exit;
    }

    memset (bGCLog, 0, iLogSize);
    if (!SafeReadMemory(dwAddr, bGCLog, iLogSize, NULL))
    {
        ExtOut("failed to read memory from %08x\n", dwAddr);
    }

    int iRealLogSize = iLogSize - 1;
    while (iRealLogSize >= 0)
    {
        if (bGCLog[iRealLogSize] != '*')
        {
            break;
        }

        iRealLogSize--;
    }

    DWORD dwWritten = 0;
    WriteFile (hGCLog, bGCLog, iRealLogSize + 1, &dwWritten, NULL);

    Status = S_OK;

exit:

    if (hGCLog != INVALID_HANDLE_VALUE)
    {
        CloseHandle (hGCLog);
    }

    if (Status == S_OK)
        ExtOut("SUCCESS: Stress log dumped\n");
    else if (Status == S_FALSE)
        ExtOut("No Stress log in the image, no file written\n");
    else
        ExtOut("FAILURE: Stress log not dumped\n");

    return Status;
}
#endif //TRACE_GC

#ifndef FEATURE_PAL
DECLARE_API (DumpGCConfigLog)
{
    INIT_API();
#ifdef GC_CONFIG_DRIVEN
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    const char* fileName = "GCConfigLog.txt";

    while (isspace (*args))
        args ++;

    if (*args != 0)
        fileName = args;

    if (!InitializeHeapData ())
    {
        ExtOut("GC Heap not initialized yet.\n");
        return S_OK;
    }

    BOOL fIsServerGC = IsServerBuild();

    DWORD_PTR dwAddr = 0;
    DWORD_PTR dwAddrOffset = 0;

    if (fIsServerGC)
    {
        dwAddr = GetValueFromExpression("SVR::gc_config_log_buffer");
        dwAddrOffset = GetValueFromExpression("SVR::gc_config_log_buffer_offset");
    }
    else
    {
        dwAddr = GetValueFromExpression("WKS::gc_config_log_buffer");
        dwAddrOffset = GetValueFromExpression("WKS::gc_config_log_buffer_offset");
    }

    moveN (dwAddr, dwAddr);
    moveN (dwAddrOffset, dwAddrOffset);

    if (dwAddr == 0)
    {
        ExtOut("Can't get either WKS or SVR GC's config log buffer");
        return E_FAIL;
    }

    ExtOut("Dumping GC log at %08x\n", dwAddr);

    g_bDacBroken = FALSE;

    ExtOut("Attempting to dump GC log to file '%s'\n", fileName);

    Status = E_FAIL;

    HANDLE hGCLog = CreateFileA(
        fileName,
        GENERIC_WRITE,
        FILE_SHARE_READ,
        NULL,
        OPEN_ALWAYS,
        FILE_ATTRIBUTE_NORMAL,
        NULL);

    if (hGCLog == INVALID_HANDLE_VALUE)
    {
        ExtOut("failed to create file: %d\n", GetLastError());
        goto exit;
    }

    {
        int iLogSize = (int)dwAddrOffset;

        ArrayHolder<BYTE> bGCLog = new NOTHROW BYTE[iLogSize];
        if (bGCLog == NULL)
        {
            ReportOOM();
            goto exit;
        }

        memset (bGCLog, 0, iLogSize);
        if (!SafeReadMemory(dwAddr, bGCLog, iLogSize, NULL))
        {
            ExtOut("failed to read memory from %08x\n", dwAddr);
        }

        SetFilePointer (hGCLog, 0, 0, FILE_END);
        DWORD dwWritten;
        WriteFile (hGCLog, bGCLog, iLogSize, &dwWritten, NULL);
    }

    Status = S_OK;

exit:

    if (hGCLog != INVALID_HANDLE_VALUE)
    {
        CloseHandle (hGCLog);
    }

    if (Status == S_OK)
        ExtOut("SUCCESS: Stress log dumped\n");
    else if (Status == S_FALSE)
        ExtOut("No Stress log in the image, no file written\n");
    else
        ExtOut("FAILURE: Stress log not dumped\n");

    return Status;
#else
    ExtOut("Not implemented\n");
    return S_OK;
#endif //GC_CONFIG_DRIVEN
}
#endif // FEATURE_PAL

#ifdef GC_CONFIG_DRIVEN
static const char * const str_interesting_data_points[] =
{
    "pre short", // 0
    "post short", // 1
    "merged pins", // 2
    "converted pins", // 3
    "pre pin", // 4
    "post pin", // 5
    "pre and post pin", // 6
    "pre short padded", // 7
    "post short padded", // 7
};

static const char * const str_heap_compact_reasons[] =
{
    "low on ephemeral space",
    "high fragmentation",
    "couldn't allocate gaps",
    "user specfied compact LOH",
    "last GC before OOM",
    "induced compacting GC",
    "fragmented gen0 (ephemeral GC)",
    "high memory load (ephemeral GC)",
    "high memory load and frag",
    "very high memory load and frag",
    "no gc mode"
};

static BOOL gc_heap_compact_reason_mandatory_p[] =
{
    TRUE, //compact_low_ephemeral = 0,
    FALSE, //compact_high_frag = 1,
    TRUE, //compact_no_gaps = 2,
    TRUE, //compact_loh_forced = 3,
    TRUE, //compact_last_gc = 4
    TRUE, //compact_induced_compacting = 5,
    FALSE, //compact_fragmented_gen0 = 6,
    FALSE, //compact_high_mem_load = 7,
    TRUE, //compact_high_mem_frag = 8,
    TRUE, //compact_vhigh_mem_frag = 9,
    TRUE //compact_no_gc_mode = 10
};

static const char * const str_heap_expand_mechanisms[] =
{
    "reused seg with normal fit",
    "reused seg with best fit",
    "expand promoting eph",
    "expand with a new seg",
    "no memory for a new seg",
    "expand in next full GC"
};

static const char * const str_bit_mechanisms[] =
{
    "using mark list",
    "demotion"
};

static const char * const str_gc_global_mechanisms[] =
{
    "concurrent GCs",
    "compacting GCs",
    "promoting GCs",
    "GCs that did demotion",
    "card bundles",
    "elevation logic"
};

void PrintInterestingGCInfo(DacpGCInterestingInfoData* dataPerHeap)
{
    ExtOut("Interesting data points\n");
    size_t* data = dataPerHeap->interestingDataPoints;
    for (int i = 0; i < DAC_NUM_GC_DATA_POINTS; i++)
    {
        ExtOut("%20s: %d\n", str_interesting_data_points[i], data[i]);
    }

    ExtOut("\nCompacting reasons\n");
    data = dataPerHeap->compactReasons;
    for (int i = 0; i < DAC_MAX_COMPACT_REASONS_COUNT; i++)
    {
        ExtOut("[%s]%35s: %d\n", (gc_heap_compact_reason_mandatory_p[i] ? "M" : "W"), str_heap_compact_reasons[i], data[i]);
    }

    ExtOut("\nExpansion mechanisms\n");
    data = dataPerHeap->expandMechanisms;
    for (int i = 0; i < DAC_MAX_EXPAND_MECHANISMS_COUNT; i++)
    {
        ExtOut("%30s: %d\n", str_heap_expand_mechanisms[i], data[i]);
    }

    ExtOut("\nOther mechanisms enabled\n");
    data = dataPerHeap->bitMechanisms;
    for (int i = 0; i < DAC_MAX_GC_MECHANISM_BITS_COUNT; i++)
    {
        ExtOut("%20s: %d\n", str_bit_mechanisms[i], data[i]);
    }
}
#endif //GC_CONFIG_DRIVEN

DECLARE_API(DumpGCData)
{
    INIT_API();

#ifdef GC_CONFIG_DRIVEN
    MINIDUMP_NOT_SUPPORTED();

    if (!InitializeHeapData ())
    {
        ExtOut("GC Heap not initialized yet.\n");
        return S_OK;
    }

    DacpGCInterestingInfoData interestingInfo;
    if (!IsServerBuild())
    {
        // Doesn't work (segfaults) for server GCs
        interestingInfo.RequestGlobal(g_sos);
        for (int i = 0; i < DAC_MAX_GLOBAL_GC_MECHANISMS_COUNT; i++)
        {
            ExtOut("%-30s: %d\n", str_gc_global_mechanisms[i], interestingInfo.globalMechanisms[i]);
        }

        ExtOut("\n[info per heap]\n");

        if (interestingInfo.Request(g_sos) != S_OK)
        {
            ExtOut("Error requesting interesting GC info\n");
            return E_FAIL;
        }

        PrintInterestingGCInfo(&interestingInfo);
    }
    else
    {
        ExtOut("\n[info per heap]\n");

        DWORD dwNHeaps = GetGcHeapCount();
        DWORD dwAllocSize;
        if (!ClrSafeInt<DWORD>::multiply(sizeof(CLRDATA_ADDRESS), dwNHeaps, dwAllocSize))
        {
            ExtOut("Failed to get GCHeaps:  integer overflow\n");
            return Status;
        }

        CLRDATA_ADDRESS *heapAddrs = (CLRDATA_ADDRESS*)alloca(dwAllocSize);
        if (g_sos->GetGCHeapList(dwNHeaps, heapAddrs, NULL) != S_OK)
        {
            ExtOut("Failed to get GCHeaps\n");
            return Status;
        }

        for (DWORD n = 0; n < dwNHeaps; n ++)
        {
            if (interestingInfo.Request(g_sos, heapAddrs[n]) != S_OK)
            {
                ExtOut("Heap %d: Error requesting interesting GC info\n", n);
                return E_FAIL;
            }

            ExtOut("--------info for heap %d--------\n", n);
            PrintInterestingGCInfo(&interestingInfo);
            ExtOut("\n");
        }
    }

    return S_OK;
#else
    ExtOut("Not implemented\n");
    return S_OK;
#endif //GC_CONFIG_DRIVEN
}

#ifdef FEATURE_PAL
extern char sccsid[];
#endif

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to dump the build number and type of the  *
*    runtime and SOS.                                                  *
*                                                                      *
\**********************************************************************/
DECLARE_API(EEVersion)
{
    INIT_API_NO_RET_ON_FAILURE("eeversion");

    static const int fileVersionBufferSize = 1024;
    ArrayHolder<char> fileVersionBuffer = new char[fileVersionBufferSize];
    VS_FIXEDFILEINFO version;

    HRESULT hr = g_pRuntime->GetEEVersion(&version, fileVersionBuffer.GetPtr(), fileVersionBufferSize);
    if (SUCCEEDED(hr))
    {
        ExtOut("%u.%u.%u.%u",
            HIWORD(version.dwFileVersionMS),
            LOWORD(version.dwFileVersionMS),
            HIWORD(version.dwFileVersionLS),
            LOWORD(version.dwFileVersionLS));

        if (IsRuntimeVersion(version, 3)) {
            ExtOut(" (3.x runtime)");
        }

#ifndef FEATURE_PAL
        if (IsWindowsTarget())
        {
            if (version.dwFileFlags & VS_FF_DEBUG) {
                ExtOut(" checked or debug build");
            }
            else
            {
                BOOL fRet = IsRetailBuild((size_t)g_pRuntime->GetModuleAddress());
                if (fRet)
                    ExtOut(" retail");
                else
                    ExtOut(" free");
            }
        }
#endif
        ExtOut("\n");

        if (fileVersionBuffer[0] != '\0') {
            ExtOut("%s\n", fileVersionBuffer.GetPtr());
        }
    }

    // Only print if DAC was loaded/initialized
    if (g_sos != nullptr)
    {
        if (!InitializeHeapData())
            ExtOut("GC Heap not initialized, so GC mode is not determined yet.\n");
        else if (IsServerBuild())
        {
            ExtOut("Server mode with %d gc heaps\n", GetGcHeapCount());
            int gcDynamicAdaptationMode;
            if (g_sos16 && (g_sos16->GetGCDynamicAdaptationMode(&gcDynamicAdaptationMode) == S_OK))
            {
                ExtOut("DATAS %d \n", gcDynamicAdaptationMode);
            }
        }
        else
        {
            ExtOut("Workstation mode\n");
        }

        if (!GetGcStructuresValid())
        {
            ExtOut("In plan phase of garbage collection\n");
        }
    }

    // Print SOS version
#ifdef FEATURE_PAL
    ExtOut("SOS Version: %s\n", sccsid + sizeof("@(#)Version"));
#else
    VS_FIXEDFILEINFO sosVersion;
    if (GetSOSVersion(&sosVersion))
    {
        ExtOut("SOS Version: %u.%u.%u.%u",
            HIWORD(sosVersion.dwFileVersionMS),
            LOWORD(sosVersion.dwFileVersionMS),
            HIWORD(sosVersion.dwFileVersionLS),
            LOWORD(sosVersion.dwFileVersionLS));

        if (sosVersion.dwFileFlags & VS_FF_DEBUG) {
            ExtOut(" debug build");
        }
        else {
            ExtOut(" retail build");
        }
        ExtOut("\n");
    }
#endif // FEATURE_PAL
    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function the global SOS status                               *
*                                                                      *
\**********************************************************************/
DECLARE_API(SOSStatus)
{
    INIT_API_NOEE_PROBE_MANAGED("sosstatus");

    BOOL bReset = FALSE;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-reset", &bReset, COBOOL, FALSE},
        {"--reset", &bReset, COBOOL, FALSE},
        {"-r", &bReset, COBOOL, FALSE},
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }
    if (bReset)
    {
        ITarget* target = GetTarget();
        if (target != nullptr)
        {
            target->Flush();
        }
        ExtOut("Internal cached state reset\n");
        return S_OK;
    }
    Target::DisplayStatus();
    ExtOut("Using no runtime to host the managed SOS code. Some commands are not availible.\n");
    return S_OK;
}

#ifndef FEATURE_PAL
/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to print the environment setting for      *
*    the current process.                                              *
*                                                                      *
\**********************************************************************/
DECLARE_API (ProcInfo)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    if (IsDumpFile())
    {
        ExtOut("!ProcInfo is not supported on a dump file.\n");
        return Status;
    }

#define INFO_ENV        0x00000001
#define INFO_TIME       0x00000002
#define INFO_MEM        0x00000004
#define INFO_ALL        0xFFFFFFFF

    DWORD fProcInfo = INFO_ALL;

    if (_stricmp (args, "-env") == 0) {
        fProcInfo = INFO_ENV;
    }

    if (_stricmp (args, "-time") == 0) {
        fProcInfo = INFO_TIME;
    }

    if (_stricmp (args, "-mem") == 0) {
        fProcInfo = INFO_MEM;
    }

    if (fProcInfo & INFO_ENV) {
        ULONG64 pPeb;
        if (FAILED(g_ExtSystem->GetCurrentProcessPeb(&pPeb)))
        {
            return Status;
        }
        ExtOut("---------------------------------------\n");
        ExtOut("Environment\n");

        static ULONG Offset_ProcessParam = -1;
        static ULONG Offset_Environment = -1;
        if (Offset_ProcessParam == -1)
        {
            ULONG TypeId;
            ULONG64 NtDllBase;
            if (SUCCEEDED(g_ExtSymbols->GetModuleByModuleName ("ntdll",0,NULL,
                                                               &NtDllBase)))
            {
                if (SUCCEEDED(g_ExtSymbols->GetTypeId (NtDllBase, "PEB", &TypeId)))
                {
                    if (FAILED (g_ExtSymbols->GetFieldOffset(NtDllBase, TypeId,
                                                         "ProcessParameters", &Offset_ProcessParam)))
                        Offset_ProcessParam = -1;
                }
                if (SUCCEEDED(g_ExtSymbols->GetTypeId (NtDllBase, "_RTL_USER_PROCESS_PARAMETERS", &TypeId)))
                {
                    if (FAILED (g_ExtSymbols->GetFieldOffset(NtDllBase, TypeId,
                                                         "Environment", &Offset_Environment)))
                        Offset_Environment = -1;
                }
            }
        }
        // We can not get it from PDB.  Use the fixed one.
        if (Offset_ProcessParam == -1)
            Offset_ProcessParam = offsetof (DT_PEB, ProcessParameters);

        if (Offset_Environment == -1)
            Offset_Environment = offsetof (DT_RTL_USER_PROCESS_PARAMETERS, Environment);


        ULONG64 addr = pPeb + Offset_ProcessParam;
        DWORD_PTR value;
        g_ExtData->ReadVirtual(UL64_TO_CDA(addr), &value, sizeof(PVOID), NULL);
        addr = value + Offset_Environment;
        g_ExtData->ReadVirtual(UL64_TO_CDA(addr), &value, sizeof(PVOID), NULL);

        static WCHAR buffer[DT_OS_PAGE_SIZE/2];
        ULONG readBytes = DT_OS_PAGE_SIZE;
        ULONG64 Page;
        if ((g_ExtData->ReadDebuggerData( DEBUG_DATA_MmPageSize, &Page, sizeof(Page), NULL)) == S_OK
            && Page > 0)
        {
            ULONG uPageSize = (ULONG)(ULONG_PTR)Page;
            if (readBytes > uPageSize) {
                readBytes = uPageSize;
            }
        }
        addr = value;
        while (1) {
            if (IsInterrupt())
                return Status;
            if (FAILED(g_ExtData->ReadVirtual(UL64_TO_CDA(addr), &buffer, readBytes, NULL)))
                break;
            addr += readBytes;
            const WCHAR *pt = buffer;
            const WCHAR *end = pt;
            while (pt < &buffer[DT_OS_PAGE_SIZE/2]) {
                end = _wcschr (pt, L'\0');
                if (end == NULL) {
                    char format[20];
                    sprintf_s (format, ARRAY_SIZE(format), "%dS", &buffer[DT_OS_PAGE_SIZE/2] - pt);
                    ExtOut(format, pt);
                    break;
                }
                else if (end == pt) {
                    break;
                }
                ExtOut("%S\n", pt);
                pt = end + 1;
            }
            if (end == pt) {
                break;
            }
        }
    }

    HANDLE hProcess = INVALID_HANDLE_VALUE;
    if (fProcInfo & (INFO_TIME | INFO_MEM)) {
        ULONG64 handle;
        if (FAILED(g_ExtSystem->GetCurrentProcessHandle(&handle)))
        {
            return Status;
        }
        hProcess = (HANDLE)handle;
    }

    if (!IsDumpFile() && fProcInfo & INFO_TIME) {
        FILETIME CreationTime;
        FILETIME ExitTime;
        FILETIME KernelTime;
        FILETIME UserTime;

        typedef BOOL (WINAPI *FntGetProcessTimes)(HANDLE, LPFILETIME, LPFILETIME, LPFILETIME, LPFILETIME);
        static FntGetProcessTimes pFntGetProcessTimes = (FntGetProcessTimes)-1;
        if (pFntGetProcessTimes == (FntGetProcessTimes)-1) {
            HINSTANCE hstat = LoadLibraryA("kernel32.dll");
            if (hstat != 0)
            {
                pFntGetProcessTimes = (FntGetProcessTimes)GetProcAddress (hstat, "GetProcessTimes");
                FreeLibrary (hstat);
            }
            else
                pFntGetProcessTimes = NULL;
        }

        if (pFntGetProcessTimes && pFntGetProcessTimes (hProcess,&CreationTime,&ExitTime,&KernelTime,&UserTime)) {
            ExtOut("---------------------------------------\n");
            ExtOut("Process Times\n");
            static const char *Month[] = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep",
                        "Oct", "Nov", "Dec"};
            SYSTEMTIME SystemTime;
            FILETIME LocalFileTime;
            if (FileTimeToLocalFileTime (&CreationTime,&LocalFileTime)
                && FileTimeToSystemTime (&LocalFileTime,&SystemTime)) {
                ExtOut("Process Started at: %4d %s %2d %d:%d:%d.%02d\n",
                        SystemTime.wYear, Month[SystemTime.wMonth-1], SystemTime.wDay,
                        SystemTime.wHour, SystemTime.wMinute,
                        SystemTime.wSecond, SystemTime.wMilliseconds/10);
            }

            DWORD nDay = 0;
            DWORD nHour = 0;
            DWORD nMin = 0;
            DWORD nSec = 0;
            DWORD nHundred = 0;

            ULONG64 totalTime;

            totalTime = KernelTime.dwLowDateTime + (((ULONG64)KernelTime.dwHighDateTime) << 32);
            nDay = (DWORD)(totalTime/(24*3600*10000000ui64));
            totalTime %= 24*3600*10000000ui64;
            nHour = (DWORD)(totalTime/(3600*10000000ui64));
            totalTime %= 3600*10000000ui64;
            nMin = (DWORD)(totalTime/(60*10000000));
            totalTime %= 60*10000000;
            nSec = (DWORD)(totalTime/10000000);
            totalTime %= 10000000;
            nHundred = (DWORD)(totalTime/100000);
            ExtOut("Kernel CPU time   : %d days %02d:%02d:%02d.%02d\n",
                    nDay, nHour, nMin, nSec, nHundred);

            DWORD sDay = nDay;
            DWORD sHour = nHour;
            DWORD sMin = nMin;
            DWORD sSec = nSec;
            DWORD sHundred = nHundred;

            totalTime = UserTime.dwLowDateTime + (((ULONG64)UserTime.dwHighDateTime) << 32);
            nDay = (DWORD)(totalTime/(24*3600*10000000ui64));
            totalTime %= 24*3600*10000000ui64;
            nHour = (DWORD)(totalTime/(3600*10000000ui64));
            totalTime %= 3600*10000000ui64;
            nMin = (DWORD)(totalTime/(60*10000000));
            totalTime %= 60*10000000;
            nSec = (DWORD)(totalTime/10000000);
            totalTime %= 10000000;
            nHundred = (DWORD)(totalTime/100000);
            ExtOut("User   CPU time   : %d days %02d:%02d:%02d.%02d\n",
                    nDay, nHour, nMin, nSec, nHundred);

            sDay += nDay;
            sHour += nHour;
            sMin += nMin;
            sSec += nSec;
            sHundred += nHundred;
            if (sHundred >= 100) {
                sSec += sHundred/100;
                sHundred %= 100;
            }
            if (sSec >= 60) {
                sMin += sSec/60;
                sSec %= 60;
            }
            if (sMin >= 60) {
                sHour += sMin/60;
                sMin %= 60;
            }
            if (sHour >= 24) {
                sDay += sHour/24;
                sHour %= 24;
            }
            ExtOut("Total  CPU time   : %d days %02d:%02d:%02d.%02d\n",
                    sDay, sHour, sMin, sSec, sHundred);
        }
    }

    if (!IsDumpFile() && fProcInfo & INFO_MEM) {
        typedef
        NTSTATUS
        (NTAPI
         *FntNtQueryInformationProcess)(HANDLE, PROCESSINFOCLASS, PVOID, ULONG, PULONG);

        static FntNtQueryInformationProcess pFntNtQueryInformationProcess = (FntNtQueryInformationProcess)-1;
        if (pFntNtQueryInformationProcess == (FntNtQueryInformationProcess)-1) {
            HINSTANCE hstat = LoadLibraryA("ntdll.dll");
            if (hstat != 0)
            {
                pFntNtQueryInformationProcess = (FntNtQueryInformationProcess)GetProcAddress (hstat, "NtQueryInformationProcess");
                FreeLibrary (hstat);
            }
            else
                pFntNtQueryInformationProcess = NULL;
        }
        VM_COUNTERS memory;
        if (pFntNtQueryInformationProcess &&
            NT_SUCCESS (pFntNtQueryInformationProcess (hProcess,ProcessVmCounters,&memory,sizeof(memory),NULL))) {
            ExtOut("---------------------------------------\n");
            ExtOut("Process Memory\n");
            ExtOut("WorkingSetSize: %8d KB       PeakWorkingSetSize: %8d KB\n",
                    memory.WorkingSetSize/1024, memory.PeakWorkingSetSize/1024);
            ExtOut("VirtualSize:    %8d KB       PeakVirtualSize:    %8d KB\n",
                    memory.VirtualSize/1024, memory.PeakVirtualSize/1024);
            ExtOut("PagefileUsage:  %8d KB       PeakPagefileUsage:  %8d KB\n",
                    memory.PagefileUsage/1024, memory.PeakPagefileUsage/1024);
        }

        MEMORYSTATUS memstat;
        GlobalMemoryStatus (&memstat);
        ExtOut("---------------------------------------\n");
        ExtOut("%ld percent of memory is in use.\n\n",
                memstat.dwMemoryLoad);
        ExtOut("Memory Availability (Numbers in MB)\n\n");
        ExtOut("                  %8s     %8s\n", "Total", "Avail");
        ExtOut("Physical Memory   %8d     %8d\n", memstat.dwTotalPhys/1024/1024, memstat.dwAvailPhys/1024/1024);
        ExtOut("Page File         %8d     %8d\n", memstat.dwTotalPageFile/1024/1024, memstat.dwAvailPageFile/1024/1024);
        ExtOut("Virtual Memory    %8d     %8d\n", memstat.dwTotalVirtual/1024/1024, memstat.dwAvailVirtual/1024/1024);
    }

    return Status;
}
#endif // FEATURE_PAL

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to find the address of EE data for a      *
*    metadata token.                                                   *
*                                                                      *
\**********************************************************************/
DECLARE_API(Token2EE)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    StringHolder DllName;
    ULONG64 token = 0;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };

    CMDValue arg[] =
    {   // vptr, type
        {&DllName.data, COSTRING},
        {&token, COHEX}
    };

    size_t nArg;
    if (!GetCMDOption(args,option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg!=2)
    {
        ExtOut("Usage: %stoken2ee module_name mdToken\n", SOSPrefix);
        ExtOut("       You can pass * for module_name to search all modules.\n");
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    int numModule;
    ArrayHolder<DWORD_PTR> moduleList = NULL;

    if (strcmp(DllName.data, "*") == 0)
    {
        moduleList = ModuleFromName(NULL, &numModule);
    }
    else
    {
        moduleList = ModuleFromName(DllName.data, &numModule);
    }

    if (moduleList == NULL)
    {
        ExtOut("Failed to request module list.\n");
    }
    else
    {
        for (int i = 0; i < numModule; i ++)
        {
            if (IsInterrupt())
                break;

            if (i > 0)
            {
                ExtOut("--------------------------------------\n");
            }

            DWORD_PTR dwAddr = moduleList[i];
            WCHAR FileName[MAX_LONGPATH];
            FileNameForModule(dwAddr, FileName);

            // We'd like a short form for this output
            LPCWSTR pszFilename = _wcsrchr (FileName, GetTargetDirectorySeparatorW());
            if (pszFilename == NULL)
            {
                pszFilename = FileName;
            }
            else
            {
                pszFilename++; // skip past the last "\" character
            }

            DMLOut("Module:      %s\n", DMLModule(dwAddr));
            ExtOut("Assembly:    %S\n", pszFilename);

            GetInfoFromModule(dwAddr, (ULONG)token);
        }
    }

    return Status;
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function is called to find the address of EE data for a      *
*    metadata token.                                                   *
*                                                                      *
\**********************************************************************/
DECLARE_API(Name2EE)
{
    INIT_API_PROBE_MANAGED("name2ee");
    MINIDUMP_NOT_SUPPORTED();

    StringHolder DllName, TypeName;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };

    CMDValue arg[] =
    {   // vptr, type
        {&DllName.data, COSTRING},
        {&TypeName.data, COSTRING}
    };
    size_t nArg;

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    if (nArg == 1)
    {
        // The input may be in the form <modulename>!<type>
        // If so, do some surgery on the input params.

        // There should be only 1 ! character
        LPSTR pszSeperator = strchr (DllName.data, '!');
        if (pszSeperator != NULL)
        {
            if (strchr (pszSeperator + 1, '!') == NULL)
            {
                size_t capacity_TypeName_data = strlen(pszSeperator + 1) + 1;
                TypeName.data = new NOTHROW char[capacity_TypeName_data];
                if (TypeName.data)
                {
                    // get the type name,
                    strcpy_s (TypeName.data, capacity_TypeName_data, pszSeperator + 1);
                    // and truncate DllName
                    *pszSeperator = '\0';

                    // Do some extra validation
                    if (strlen (DllName.data) >= 1 &&
                        strlen (TypeName.data) > 1)
                    {
                        nArg = 2;
                    }
                }
            }
        }
    }

    if (nArg != 2)
    {
        ExtOut("Usage: %sname2ee module_name item_name\n", SOSPrefix);
        ExtOut("  or   %sname2ee module_name!item_name\n", SOSPrefix);
        ExtOut("       use * for module_name to search all loaded modules\n");
        ExtOut("Examples: %sname2ee  mscorlib.dll System.String.ToString\n", SOSPrefix);
        ExtOut("          %sname2ee *!System.String\n", SOSPrefix);
        return E_INVALIDARG;
    }

    int numModule;
    ArrayHolder<DWORD_PTR> moduleList = NULL;
    if (strcmp(DllName.data, "*") == 0)
    {
        moduleList = ModuleFromName(NULL, &numModule);
    }
    else
    {
        moduleList = ModuleFromName(DllName.data, &numModule);
    }


    if (moduleList == NULL)
    {
        ExtOut("Failed to request module list.\n", DllName.data);
    }
    else
    {
        for (int i = 0; i < numModule; i ++)
        {
            if (IsInterrupt())
                break;

            if (i > 0)
            {
                ExtOut("--------------------------------------\n");
            }

            DWORD_PTR dwAddr = moduleList[i];
            WCHAR FileName[MAX_LONGPATH];
            FileNameForModule (dwAddr, FileName);

            // We'd like a short form for this output
            LPCWSTR pszFilename = _wcsrchr (FileName, GetTargetDirectorySeparatorW());
            if (pszFilename == NULL)
            {
                pszFilename = FileName;
            }
            else
            {
                pszFilename++; // skip past the last "\" character
            }

            DMLOut("Module:      %s\n", DMLModule(dwAddr));
            ExtOut("Assembly:    %S\n", pszFilename);
            GetInfoFromName(dwAddr, TypeName.data);
        }
    }

    return Status;
}

DECLARE_API(FindRoots)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    if (IsDumpFile())
    {
        ExtOut("%sfindroots is not supported on a dump file.\n", SOSPrefix);
        return Status;
    }

    LONG_PTR gen = -100; // initialized outside the legal range: [-1, 2]
    StringHolder sgen;
    TADDR taObj = (TADDR)0;
    BOOL dml = FALSE;
    size_t nArg;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-gen", &sgen.data, COSTRING, TRUE},
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&taObj, COHEX}
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (sgen.data != NULL)
    {
        if (_stricmp(sgen.data, "any") == 0)
        {
            gen = -1;
        }
        else
        {
            gen = GetExpression(sgen.data);
        }
    }
    if ((gen < -1 || gen > 2) && (taObj == 0))
    {
        ExtOut("Incorrect options.  Usage:\n\t%sfindroots -gen <N>\n\t\twhere N is 0, 1, 2, or \"any\". OR\n\t%sfindroots <obj>\n", SOSPrefix, SOSPrefix);
        return Status;
    }

    if (gen >= -1 && gen <= 2)
    {
        IXCLRDataProcess2* idp2 = NULL;
        if (FAILED(g_clrData->QueryInterface(IID_IXCLRDataProcess2, (void**) &idp2)))
        {
            ExtOut("Your version of the runtime/DAC do not support this command.\n");
            return Status;
        }

        // Request GC_MARK_END notifications from debuggee
        GcEvtArgs gea = { GC_MARK_END, { ((gen == -1) ? 7 : (1 << gen)) } };
        idp2->SetGcNotification(gea);
        // ... and register the notification handler
#ifndef FEATURE_PAL
        g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "sxe -c \"!SOSHandleCLRN\" clrn", 0);
#else
        g_ExtServices->SetExceptionCallback(HandleExceptionNotification);
#endif // FEATURE_PAL
        // the above notification is removed in CNotification::OnGcEvent()
    }
    else
    {
        // verify that the last event in the debugger was indeed a CLRN exception
        DEBUG_LAST_EVENT_INFO_EXCEPTION dle;
        CNotification Notification;

        if (!CheckCLRNotificationEvent(&dle))
        {
            ExtOut("The command %sfindroots can only be used after the debugger stopped on a CLRN GC notification.\n", SOSPrefix);
            ExtOut("At this time %sgcroot should be used instead.\n", SOSPrefix);
            return Status;
        }

        std::stringstream argsBuilder;
        argsBuilder << "-gcgen " << CNotification::GetCondemnedGen() << " " << std::hex << taObj;

        return ExecuteCommand("gcroot", argsBuilder.str().c_str());
    }

    return Status;
}

class GCHandleStatsForDomains
{
public:
    GCHandleStatsForDomains()
        : m_singleDomainMode(FALSE), m_numDomains(0), m_pStatistics(NULL), m_pDomainPointers(NULL), m_sharedDomainIndex(-1), m_systemDomainIndex(-1)
    {
    }

    ~GCHandleStatsForDomains()
    {
        if (m_pStatistics)
        {
            if (m_singleDomainMode)
                delete m_pStatistics;
            else
                delete [] m_pStatistics;
        }

        if (m_pDomainPointers)
            delete [] m_pDomainPointers;
    }

    BOOL Init(BOOL singleDomainMode)
    {
        m_singleDomainMode = singleDomainMode;
        if (m_singleDomainMode)
        {
            m_numDomains = 1;
            m_pStatistics = new NOTHROW GCHandleStatistics();
            if (m_pStatistics == NULL)
                return FALSE;
        }
        else
        {
            DacpAppDomainStoreData adsData;
            if (adsData.Request(g_sos) != S_OK)
                return FALSE;

            LONG numSpecialDomains = (adsData.sharedDomain != (TADDR)0) ? 2 : 1;
            m_numDomains = adsData.DomainCount + numSpecialDomains;
            ArrayHolder<CLRDATA_ADDRESS> pArray = new NOTHROW CLRDATA_ADDRESS[m_numDomains];
            if (pArray == NULL)
                return FALSE;

            int i = 0;
            if (adsData.sharedDomain != (TADDR)0)
            {
                pArray[i++] = adsData.sharedDomain;
            }

            pArray[i] = adsData.systemDomain;

            m_sharedDomainIndex = i - 1; // The m_sharedDomainIndex is set to -1 if there is no shared domain
            m_systemDomainIndex = i;

            if (g_sos->GetAppDomainList(adsData.DomainCount, pArray+numSpecialDomains, NULL) != S_OK)
                return FALSE;

            m_pDomainPointers = pArray.Detach();
            m_pStatistics = new NOTHROW GCHandleStatistics[m_numDomains];
            if (m_pStatistics == NULL)
                return FALSE;
        }

        return TRUE;
    }

    GCHandleStatistics *LookupStatistics(CLRDATA_ADDRESS appDomainPtr) const
    {
        if (m_singleDomainMode)
        {
            // You can pass NULL appDomainPtr if you are in singleDomainMode
            return m_pStatistics;
        }
        else
        {
            for (int i=0; i < m_numDomains; i++)
                if (m_pDomainPointers[i] == appDomainPtr)
                    return m_pStatistics + i;
        }

        return NULL;
    }


    GCHandleStatistics *GetStatistics(int appDomainIndex) const
    {
        SOS_Assert(appDomainIndex >= 0);
        SOS_Assert(appDomainIndex < m_numDomains);

        return m_singleDomainMode ? m_pStatistics : m_pStatistics + appDomainIndex;
    }

    int GetNumDomains() const
    {
        return m_numDomains;
    }

    CLRDATA_ADDRESS GetDomain(int index) const
    {
        SOS_Assert(index >= 0);
        SOS_Assert(index < m_numDomains);
        return m_pDomainPointers[index];
    }

    int GetSharedDomainIndex()
    {
        return m_sharedDomainIndex;
    }

    int GetSystemDomainIndex()
    {
        return m_systemDomainIndex;
    }

private:
    BOOL m_singleDomainMode;
    int m_numDomains;
    GCHandleStatistics *m_pStatistics;
    CLRDATA_ADDRESS *m_pDomainPointers;
    int m_sharedDomainIndex;
    int m_systemDomainIndex;
};

class GCHandlesImpl
{
public:
    GCHandlesImpl(PCSTR args)
        : mPerDomain(FALSE), mStat(FALSE), mDML(FALSE), mType((int)~0)
    {
        ArrayHolder<char> type = NULL;
        CMDOption option[] =
        {
            {"-perdomain", &mPerDomain, COBOOL, FALSE},
            {"-stat", &mStat, COBOOL, FALSE},
            {"-type", &type, COSTRING, TRUE},
            {"/d", &mDML, COBOOL, FALSE},
        };

        if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
        {
            sos::Throw<sos::Exception>("Failed to parse command line arguments.");
        }

        if (type != NULL) {
            if (_stricmp(type, "Pinned") == 0)
                mType = HNDTYPE_PINNED;
            else if (_stricmp(type, "RefCounted") == 0)
                mType = HNDTYPE_REFCOUNTED;
            else if (_stricmp(type, "WeakShort") == 0)
                mType = HNDTYPE_WEAK_SHORT;
            else if (_stricmp(type, "WeakLong") == 0)
                mType = HNDTYPE_WEAK_LONG;
            else if (_stricmp(type, "Strong") == 0)
                mType = HNDTYPE_STRONG;
            else if (_stricmp(type, "Variable") == 0)
                mType = HNDTYPE_VARIABLE;
            else if (_stricmp(type, "AsyncPinned") == 0)
                mType = HNDTYPE_ASYNCPINNED;
            else if (_stricmp(type, "SizedRef") == 0)
                mType = HNDTYPE_SIZEDREF;
            else if (_stricmp(type, "Dependent") == 0)
                mType = HNDTYPE_DEPENDENT;
            else if (_stricmp(type, "WeakWinRT") == 0)
                mType = HNDTYPE_WEAK_WINRT;
            else if (_stricmp(type, "WeakInteriorPointer") == 0)
                mType = HNDTYPE_WEAK_INTERIOR_POINTER;
            else
                sos::Throw<sos::Exception>("Unknown handle type '%s'.", type.GetPtr());
        }
    }

    void Run()
    {
        EnableDMLHolder dmlHolder(mDML);

        mOut.ReInit(6, POINTERSIZE_HEX, AlignRight);
        mOut.SetWidths(5, POINTERSIZE_HEX, 11, POINTERSIZE_HEX, 8, POINTERSIZE_HEX);
        mOut.SetColAlignment(1, AlignLeft);

        if (mHandleStat.Init(!mPerDomain) == FALSE)
            sos::Throw<sos::Exception>("Error getting per-appdomain handle information");

        if (!mStat)
            mOut.WriteRow("Handle", "Type", "Object", "Size", "Data", "Type");

        WalkHandles();

        for (int i=0; (i < mHandleStat.GetNumDomains()) && !IsInterrupt(); i++)
        {
            GCHandleStatistics *pStats = mHandleStat.GetStatistics(i);

            if (mPerDomain)
            {
                Print( "------------------------------------------------------------------------------\n");
                Print("GC Handle Statistics for AppDomain ", AppDomainPtr(mHandleStat.GetDomain(i)));

                if (i == mHandleStat.GetSharedDomainIndex())
                    Print(" (Shared Domain)\n");
                else if (i == mHandleStat.GetSystemDomainIndex())
                    Print(" (System Domain)\n");
                else
                    Print("\n");
            }

            if (!mStat)
                Print("\n");
            PrintGCStat(&pStats->hs);

            // Don't print handle stats if the user has filtered by type.  All handles will be the same
            // type, and the total count will be displayed by PrintGCStat.
            if (mType == (unsigned int)~0)
            {
                Print("\n");
                PrintGCHandleStats(pStats);
            }
        }
    }

private:
    void WalkHandles()
    {
        ToRelease<ISOSHandleEnum> handles;
        if (FAILED(g_sos->GetHandleEnum(&handles)))
        {
            if (IsMiniDumpFile())
                sos::Throw<sos::Exception>("Unable to display GC handles.\nA minidump without full memory may not have this information.");
            else
                sos::Throw<sos::Exception>("Failed to walk the handle table.");
        }

        // GCC can't handle stacks which are too large.
#ifndef FEATURE_PAL
        SOSHandleData data[256];
#else
        SOSHandleData data[4];
#endif

        unsigned int fetched = 0;
        HRESULT hr = S_OK;
        do
        {
            if (FAILED(hr = handles->Next(ARRAY_SIZE(data), data, &fetched)))
            {
                ExtOut("Error %x while walking the handle table.\n", hr);
                break;
            }

            WalkHandles(data, fetched);
        } while (ARRAY_SIZE(data) == fetched);
    }

    void WalkHandles(SOSHandleData data[], unsigned int count)
    {
        for (unsigned int i = 0; i < count; ++i)
        {
            sos::CheckInterrupt();

            if (mType != (unsigned int)~0 && mType != data[i].Type)
                continue;

            GCHandleStatistics *pStats = mHandleStat.LookupStatistics(data[i].AppDomain);
            TADDR objAddr = 0;
            TADDR mtAddr = 0;
            size_t size = 0;
            const WCHAR *mtName = 0;
            const char *type = 0;

            if (FAILED(MOVE(objAddr, data[i].Handle)))
            {
                objAddr = 0;
                mtName = W("<error>");
            }
            else
            {
                sos::Object obj(TO_TADDR(objAddr));
                mtAddr = obj.GetMT();
                if (sos::MethodTable::IsFreeMT(mtAddr))
                {
                    mtName = W("<free>");
                }
                else if (!sos::MethodTable::IsValid(mtAddr))
                {
                    mtName = W("<error>");
                }
                else
                {
                    size = obj.GetSize();
                    if (mType == (unsigned int)~0 || mType == data[i].Type)
                        pStats->hs.Add(obj.GetMT(), (DWORD)size);
                }
            }

            switch(data[i].Type)
            {
                case HNDTYPE_PINNED:
                    type = "Pinned";
                    if (pStats) pStats->pinnedHandleCount++;
                    break;
                case HNDTYPE_REFCOUNTED:
                    type = "RefCounted";
                    if (pStats) pStats->refCntHandleCount++;
                    break;
                case HNDTYPE_STRONG:
                    type = "Strong";
                    if (pStats) pStats->strongHandleCount++;
                    break;
                case HNDTYPE_WEAK_SHORT:
                    type = "WeakShort";
                    if (pStats) pStats->weakShortHandleCount++;
                    break;
                case HNDTYPE_WEAK_LONG:
                    type = "WeakLong";
                    if (pStats) pStats->weakLongHandleCount++;
                    break;
                case HNDTYPE_ASYNCPINNED:
                    type = "AsyncPinned";
                    if (pStats) pStats->asyncPinnedHandleCount++;
                    break;
                case HNDTYPE_VARIABLE:
                    type = "Variable";
                    if (pStats) pStats->variableCount++;
                    break;
                case HNDTYPE_SIZEDREF:
                    type = "SizedRef";
                    if (pStats) pStats->sizedRefCount++;
                    break;
                case HNDTYPE_DEPENDENT:
                    type = "Dependent";
                    if (pStats) pStats->dependentCount++;
                    break;
                case HNDTYPE_WEAK_WINRT:
                    type = "WeakWinRT";
                    if (pStats) pStats->weakWinRTHandleCount++;
                    break;
                case HNDTYPE_WEAK_INTERIOR_POINTER:
                    type = "WeakInteriorPointer";
                    if (pStats) pStats->weakInteriorPointerHandleCount++;
                    break;
                default:
                    DebugBreak();
                    type = "Unknown";
                    pStats->unknownHandleCount++;
                    break;
            }

            if (type && !mStat)
            {
                sos::MethodTable mt = mtAddr;
                if (mtName == 0)
                    mtName = mt.GetName();

                if (data[i].Type == HNDTYPE_REFCOUNTED)
                    mOut.WriteRow(data[i].Handle, type, ObjectPtr(objAddr), Decimal(size), Decimal(data[i].RefCount), mtName);
                else if (data[i].Type == HNDTYPE_DEPENDENT)
                    mOut.WriteRow(data[i].Handle, type, ObjectPtr(objAddr), Decimal(size), ObjectPtr(data[i].Secondary), mtName);
                else if (data[i].Type == HNDTYPE_WEAK_WINRT)
                    mOut.WriteRow(data[i].Handle, type, ObjectPtr(objAddr), Decimal(size), Pointer(data[i].Secondary), mtName);
                else if (data[i].Type == HNDTYPE_WEAK_INTERIOR_POINTER)
                    mOut.WriteRow(data[i].Handle, type, ObjectPtr(objAddr), Decimal(size), Pointer(data[i].Secondary), mtName);
                else
                    mOut.WriteRow(data[i].Handle, type, ObjectPtr(objAddr), Decimal(size), "", mtName);
            }
        }
    }

    inline void PrintHandleRow(const char *text, int count)
    {
        if (count)
            mOut.WriteRow(text, Decimal(count));
    }

    void PrintGCHandleStats(GCHandleStatistics *pStats)
    {
        Print("Handles:\n");
        mOut.ReInit(2, 21, AlignLeft, 4);

        PrintHandleRow("Strong Handles:", pStats->strongHandleCount);
        PrintHandleRow("Pinned Handles:", pStats->pinnedHandleCount);
        PrintHandleRow("Async Pinned Handles:", pStats->asyncPinnedHandleCount);
        PrintHandleRow("Ref Count Handles:", pStats->refCntHandleCount);
        PrintHandleRow("Weak Long Handles:", pStats->weakLongHandleCount);
        PrintHandleRow("Weak Short Handles:", pStats->weakShortHandleCount);
        PrintHandleRow("Weak WinRT Handles:", pStats->weakWinRTHandleCount);
        PrintHandleRow("Weak Interior Pointer Handles:", pStats->weakInteriorPointerHandleCount);
        PrintHandleRow("Variable Handles:", pStats->variableCount);
        PrintHandleRow("SizedRef Handles:", pStats->sizedRefCount);
        PrintHandleRow("Dependent Handles:", pStats->dependentCount);
        PrintHandleRow("Other Handles:", pStats->unknownHandleCount);
    }

private:
    BOOL mPerDomain, mStat, mDML;
    unsigned int mType;
    TableOutput mOut;
    GCHandleStatsForDomains mHandleStat;
};

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function dumps GC Handle statistics        *
*                                                                      *
\**********************************************************************/
DECLARE_API(GCHandles)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    try
    {
        GCHandlesImpl gchandles(args);
        gchandles.Run();
    }
    catch(const sos::Exception &e)
    {
        Print(e.what());
    }

    return Status;
}

// This is an experimental and undocumented SOS API that attempts to step through code
// stopping once jitted code is reached. It currently has some issues - it can take arbitrarily long
// to reach jitted code and canceling it is terrible. At best it doesn't cancel, at worst it
// kills the debugger. IsInterrupt() doesn't work nearly as nicely as one would hope :/
#ifndef FEATURE_PAL
DECLARE_API(TraceToCode)
{
    INIT_API_NODAC();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();
    _ASSERTE(g_pRuntime != nullptr);

    while(true)
    {
        if (IsInterrupt())
        {
            ExtOut("Interrupted\n");
            return S_FALSE;
        }

        ULONG64 Offset;
        g_ExtRegisters->GetInstructionOffset(&Offset);

        DWORD codeType = 0;
        ULONG64 base = 0;
        CLRDATA_ADDRESS cdaStart = TO_CDADDR(Offset);
        DacpMethodDescData MethodDescData;
        if (g_ExtSymbols->GetModuleByOffset(Offset, 0, NULL, &base) == S_OK)
        {
            ULONG64 clrBaseAddr = g_pRuntime->GetModuleAddress();
            if(clrBaseAddr == base)
            {
                ExtOut("Compiled code in CLR\n");
                codeType = 4;
            }
            else
            {
                ExtOut("Compiled code in module @ 0x%I64x\n", base);
                codeType = 8;
            }
        }
        else if (g_sos != NULL || LoadClrDebugDll()==S_OK)
        {
            CLRDATA_ADDRESS addr;
            if(g_sos->GetMethodDescPtrFromIP(cdaStart, &addr) == S_OK)
            {
                WCHAR wszNameBuffer[1024]; // should be large enough

                // get the MethodDesc name
                if ((g_sos->GetMethodDescName(addr, 1024, wszNameBuffer, NULL) == S_OK) &&
                    _wcsncmp(W("DomainBoundILStubClass"), wszNameBuffer, 22)==0)
                {
                    ExtOut("ILStub\n");
                    codeType = 2;
                }
                else
                {
                    ExtOut("Jitted code\n");
                    codeType = 1;
                }
            }
            else
            {
                ExtOut("Not compiled or jitted, assuming stub\n");
                codeType = 16;
            }
        }
        else
        {
            // not compiled but CLR isn't loaded... some other code generator?
            return E_FAIL;
        }

        if(codeType == 1)
        {
            return S_OK;
        }
        else
        {
            Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "thr; .echo wait" ,0);
            if (FAILED(Status))
            {
                ExtOut("Error tracing instruction\n");
                return Status;
            }
        }
    }

    return Status;

}
#endif // FEATURE_PAL

// This is an experimental and undocumented API that sets a debugger pseudo-register based
// on the type of code at the given IP. It can be used in scripts to keep stepping until certain
// kinds of code have been reached. Presumably its slower than !TraceToCode but at least it
// cancels much better
#ifndef FEATURE_PAL
DECLARE_API(GetCodeTypeFlags)
{
    INIT_API();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();
    _ASSERTE(g_pRuntime != nullptr);

    char buffer[100+mdNameLen];
    size_t ip;
    StringHolder PReg;

    CMDValue arg[] = {
        // vptr, type
        {&ip, COSIZE_T},
        {&PReg.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    size_t preg = 1; // by default
    if (nArg == 2)
    {
        preg = GetExpression(PReg.data);
        if (preg > 19)
        {
            ExtOut("Pseudo-register number must be between 0 and 19\n");
            return E_INVALIDARG;
        }
    }

    sprintf_s(buffer, ARRAY_SIZE(buffer),
        "r$t%d=0",
        preg);
    Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer ,0);
    if (FAILED(Status))
    {
        ExtOut("Error initialized register $t%d to zero\n", preg);
        return Status;
    }

    ULONG64 base = 0;
    CLRDATA_ADDRESS cdaStart = TO_CDADDR(ip);
    DWORD codeType = 0;
    CLRDATA_ADDRESS addr;
    if (g_sos->GetMethodDescPtrFromIP(cdaStart, &addr) == S_OK)
    {
        WCHAR wszNameBuffer[1024]; // should be large enough

        // get the MethodDesc name
        if (g_sos->GetMethodDescName(addr, 1024, wszNameBuffer, NULL) == S_OK &&
            _wcsncmp(W("DomainBoundILStubClass"), wszNameBuffer, 22)==0)
        {
            ExtOut("ILStub\n");
            codeType = 2;
        }
        else
        {
            ExtOut("Jitted code");
            codeType = 1;
        }
    }
    else if(g_ExtSymbols->GetModuleByOffset (ip, 0, NULL, &base) == S_OK)
    {
        ULONG64 clrBaseAddr = g_pRuntime->GetModuleAddress();
        if (base == clrBaseAddr)
        {
            ExtOut("Compiled code in CLR");
            codeType = 4;
        }
        else
        {
            ExtOut("Compiled code in module @ 0x%I64x\n", base);
            codeType = 8;
        }
    }
    else
    {
        ExtOut("Not compiled or jitted, assuming stub\n");
        codeType = 16;
    }

    sprintf_s(buffer, ARRAY_SIZE(buffer),
        "r$t%d=%x",
        preg, codeType);
    Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
    if (FAILED(Status))
    {
        ExtOut("Error setting register $t%d\n", preg);
        return Status;
    }
    return Status;

}
#endif // FEATURE_PAL

DECLARE_API(StopOnException)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();


    char buffer[100+mdNameLen];

    BOOL fDerived = FALSE;
    BOOL fCreate1 = FALSE;
    BOOL fCreate2 = FALSE;

    CMDOption option[] = {
        // name, vptr, type, hasValue
        {"-derived", &fDerived, COBOOL, FALSE}, // catch derived exceptions
        {"-create", &fCreate1, COBOOL, FALSE}, // create 1st chance handler
        {"-create2", &fCreate2, COBOOL, FALSE}, // create 2nd chance handler
    };

    StringHolder TypeName,PReg;

    CMDValue arg[] = {
        // vptr, type
        {&TypeName.data, COSTRING},
        {&PReg.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (IsDumpFile())
    {
        ExtOut("Live debugging session required\n");
        return E_INVALIDARG;
    }
    if (nArg < 1 || nArg > 2)
    {
        ExtOut("usage: StopOnException [-derived] [-create | -create2] <type name>\n");
        ExtOut("                       [<pseudo-register number for result>]\n");
        ExtOut("ex:    StopOnException -create System.OutOfMemoryException 1\n");
        return E_INVALIDARG;
    }

    size_t preg = 1; // by default
    if (nArg == 2)
    {
        preg = GetExpression(PReg.data);
        if (preg > 19)
        {
            ExtOut("Pseudo-register number must be between 0 and 19\n");
            return E_INVALIDARG;
        }
    }

    sprintf_s(buffer, ARRAY_SIZE(buffer),
        "r$t%d=0",
        preg);
    Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
    if (FAILED(Status))
    {
        ExtOut("Error initialized register $t%d to zero\n", preg);
        return Status;
    }

    if (fCreate1 || fCreate2)
    {
        sprintf_s(buffer, ARRAY_SIZE(buffer),
            "sxe %s \"!soe %s %s %d;.if(@$t%d==0) {g} .else {.echo '%s hit'}\" %x",
            fCreate1 ? "-c" : "-c2",
            fDerived ? "-derived" : "",
            TypeName.data,
            preg,
            preg,
            TypeName.data,
            EXCEPTION_COMPLUS
            );

        Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
        if (FAILED(Status))
        {
            ExtOut("Error setting breakpoint: %s\n", buffer);
            return Status;
        }

        ExtOut("Breakpoint set\n");
        return Status;
    }

    // Find the last thrown exception on this thread.
    // Does it match? If so, set the register.
    CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
    DacpThreadData Thread;

    if ((threadAddr == (TADDR)0) || (Thread.Request(g_sos, threadAddr) != S_OK))
    {
        ExtOut("The current thread is unmanaged\n");
        return Status;
    }

    TADDR taLTOH;
    if (!SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle),
                        &taLTOH,
                        sizeof(taLTOH), NULL))
    {
        ExtOut("There is no current managed exception on this thread\n");
        return Status;
    }

    if (taLTOH)
    {
        LPWSTR typeNameWide = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));
        MultiByteToWideChar(CP_ACP,0,TypeName.data,-1,typeNameWide,mdNameLen);

        TADDR taMT;
        if (SafeReadMemory(taLTOH, &taMT, sizeof(taMT), NULL))
        {
            NameForMT_s (taMT, g_mdName, mdNameLen);
            if ((_wcscmp(g_mdName,typeNameWide) == 0) ||
                (fDerived && IsDerivedFrom(taMT, typeNameWide)))
            {
                sprintf_s(buffer, ARRAY_SIZE(buffer),
                    "r$t%d=1",
                    preg);
                Status = g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, buffer, 0);
                if (FAILED(Status))
                {
                    ExtOut("Failed to execute the following command: %s\n", buffer);
                }
            }
        }
    }

    return Status;
}

#ifndef FEATURE_PAL
// For FEATURE_PAL, MEMORY_BASIC_INFORMATION64 doesn't exist yet. TODO?
DECLARE_API(GCHandleLeaks)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    ExtOut("-------------------------------------------------------------------------------\n");
    ExtOut("GCHandleLeaks will report any GCHandles that couldn't be found in memory.      \n");
    ExtOut("Strong and Pinned GCHandles are reported at this time. You can safely abort the\n");
    ExtOut("memory scan with Control-C or Control-Break.                                   \n");
    ExtOut("-------------------------------------------------------------------------------\n");

    static DWORD_PTR array[2000];
    UINT i;
    BOOL dml = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"/d", &dml, COBOOL, FALSE},
    };

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);

    UINT iFinal = FindAllPinnedAndStrong(array,sizeof(array)/sizeof(DWORD_PTR));
    ExtOut("Found %d handles:\n",iFinal);
    for (i=1;i<=iFinal;i++)
    {
        ExtOut("%p\t", SOS_PTR(array[i-1]));
        if ((i % 4) == 0)
            ExtOut("\n");
    }

    ExtOut("\nSearching memory\n");
    // Now search memory for this:
    DWORD_PTR buffer[1024];
    ULONG64 memCur = 0x0;
    BOOL bAbort = FALSE;

    //find out memory used by stress log
    StressLogMem stressLog;
    CLRDATA_ADDRESS StressLogAddress = NULL;
    if (LoadClrDebugDll() != S_OK)
    {
        // Try to find stress log symbols
        DWORD_PTR dwAddr = GetValueFromExpression("StressLog::theLog");
        StressLogAddress = dwAddr;
        g_bDacBroken = TRUE;
    }
    else
    {
        if (g_sos->GetStressLogAddress(&StressLogAddress) != S_OK)
        {
            ExtOut("Unable to find stress log via DAC\n");
        }
        g_bDacBroken = FALSE;
    }

    if (stressLog.Init (StressLogAddress, g_ExtData))
    {
        ExtOut("Reference found in stress log will be ignored\n");
    }
    else
    {
        ExtOut("Failed to read whole or part of stress log, some references may come from stress log\n");
    }


    while (!bAbort)
    {
        NTSTATUS status;
        MEMORY_BASIC_INFORMATION64 memInfo;

        status = g_ExtData2->QueryVirtual(UL64_TO_CDA(memCur), &memInfo);

        if( !NT_SUCCESS(status) )
        {
            break;
        }

        if (memInfo.State == MEM_COMMIT)
        {
            for (ULONG64 memIter = memCur; memIter < (memCur + memInfo.RegionSize); memIter+=sizeof(buffer))
            {
                if (IsInterrupt())
                {
                    ExtOut("Quitting at %p due to user abort\n", SOS_PTR(memIter));
                    bAbort = TRUE;
                    break;
                }

                if ((memIter % 0x10000000)==0x0)
                {
                    ExtOut("Searching %p...\n", SOS_PTR(memIter));
                }

                ULONG size = 0;
                HRESULT ret;
                ret = g_ExtData->ReadVirtual(UL64_TO_CDA(memIter), buffer, sizeof(buffer), &size);
                if (ret == S_OK)
                {
                    for (UINT x=0;x<1024;x++)
                    {
                        DWORD_PTR value = buffer[x];
                        // We don't care about the low bit. Also, the GCHandle class turns on the
                        // low bit for pinned handles, so without the statement below, we wouldn't
                        // notice pinned handles.
                        value = value & ~1;
                        for (i=0;i<iFinal;i++)
                        {
                            ULONG64 addrInDebugee = (ULONG64)memIter+(x*sizeof(DWORD_PTR));
                            if ((array[i] & ~1) == value)
                            {
                                if (stressLog.IsInStressLog (addrInDebugee))
                                {
                                    ExtOut("Found %p in stress log at location %p, reference not counted\n", SOS_PTR(value), addrInDebugee);
                                }
                                else
                                {
                                    ExtOut("Found %p at location %p\n", SOS_PTR(value), addrInDebugee);
                                    array[i] |= 0x1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (size > 0)
                    {
                        ExtOut("only read %x bytes at %p\n", size, SOS_PTR(memIter));
                    }
                }
            }
        }

        memCur += memInfo.RegionSize;
    }

    int numNotFound = 0;
    for (i=0;i<iFinal;i++)
    {
        if ((array[i] & 0x1) == 0)
        {
            numNotFound++;
            // ExtOut("WARNING: %p not found\n", SOS_PTR(array[i]));
        }
    }

    if (numNotFound > 0)
    {
        ExtOut("------------------------------------------------------------------------------\n");
        ExtOut("Some handles were not found. If the number of not-found handles grows over the\n");
        ExtOut("lifetime of your application, you may have a GCHandle leak. This will cause   \n");
        ExtOut("the GC Heap to grow larger as objects are being kept alive, referenced only   \n");
        ExtOut("by the orphaned handle. If the number doesn't grow over time, note that there \n");
        ExtOut("may be some noise in this output, as an unmanaged application may be storing  \n");
        ExtOut("the handle in a non-standard way, perhaps with some bits flipped. The memory  \n");
        ExtOut("scan wouldn't be able to find those.                                          \n");
        ExtOut("------------------------------------------------------------------------------\n");

        ExtOut("Didn't find %d handles:\n", numNotFound);
        int numPrinted=0;
        for (i=0;i<iFinal;i++)
        {
            if ((array[i] & 0x1) == 0)
            {
                numPrinted++;
                ExtOut("%p\t", SOS_PTR(array[i]));
                if ((numPrinted % 4) == 0)
                    ExtOut("\n");
            }
        }
        ExtOut("\n");
    }
    else
    {
        ExtOut("------------------------------------------------------------------------------\n");
        ExtOut("All handles found");
        if (bAbort)
            ExtOut(" even though you aborted.\n");
        else
            ExtOut(".\n");
        ExtOut("A leak may still exist because in a general scan of process memory SOS can't  \n");
        ExtOut("differentiate between garbage and valid structures, so you may have false     \n");
        ExtOut("positives. If you still suspect a leak, use this function over time to        \n");
        ExtOut("identify a possible trend.                                                    \n");
        ExtOut("------------------------------------------------------------------------------\n");
    }

    return Status;
}
#endif // FEATURE_PAL

class ClrStackImplWithICorDebug
{
private:
    static HRESULT DereferenceAndUnboxValue(ICorDebugValue * pValue, ICorDebugValue** ppOutputValue, BOOL * pIsNull = NULL)
    {
        HRESULT Status = S_OK;
        *ppOutputValue = NULL;
        if(pIsNull != NULL) *pIsNull = FALSE;

        ToRelease<ICorDebugReferenceValue> pReferenceValue;
        Status = pValue->QueryInterface(IID_ICorDebugReferenceValue, (LPVOID*) &pReferenceValue);
        if (SUCCEEDED(Status))
        {
            BOOL isNull = FALSE;
            IfFailRet(pReferenceValue->IsNull(&isNull));
            if(!isNull)
            {
                ToRelease<ICorDebugValue> pDereferencedValue;
                IfFailRet(pReferenceValue->Dereference(&pDereferencedValue));
                return DereferenceAndUnboxValue(pDereferencedValue, ppOutputValue);
            }
            else
            {
                if(pIsNull != NULL) *pIsNull = TRUE;
                *ppOutputValue = pValue;
                (*ppOutputValue)->AddRef();
                return S_OK;
            }
        }

        ToRelease<ICorDebugBoxValue> pBoxedValue;
        Status = pValue->QueryInterface(IID_ICorDebugBoxValue, (LPVOID*) &pBoxedValue);
        if (SUCCEEDED(Status))
        {
            ToRelease<ICorDebugObjectValue> pUnboxedValue;
            IfFailRet(pBoxedValue->GetObject(&pUnboxedValue));
            return DereferenceAndUnboxValue(pUnboxedValue, ppOutputValue);
        }
        *ppOutputValue = pValue;
        (*ppOutputValue)->AddRef();
        return S_OK;
    }

    static BOOL ShouldExpandVariable(__in_z WCHAR* varToExpand, __in_z WCHAR* currentExpansion)
    {
        if(currentExpansion == NULL || varToExpand == NULL) return FALSE;

        size_t varToExpandLen = _wcslen(varToExpand);
        size_t currentExpansionLen = _wcslen(currentExpansion);
        if(currentExpansionLen > varToExpandLen) return FALSE;
        if(currentExpansionLen < varToExpandLen && varToExpand[currentExpansionLen] != L'.') return FALSE;
        if(_wcsncmp(currentExpansion, varToExpand, currentExpansionLen) != 0) return FALSE;

        return TRUE;
    }

    static BOOL IsEnum(ICorDebugValue * pInputValue)
    {
        ToRelease<ICorDebugValue> pValue;
        if(FAILED(DereferenceAndUnboxValue(pInputValue, &pValue, NULL))) return FALSE;

        WCHAR baseTypeName[mdNameLen];
        ToRelease<ICorDebugValue2> pValue2;
        ToRelease<ICorDebugType> pType;
        ToRelease<ICorDebugType> pBaseType;

        if(FAILED(pValue->QueryInterface(IID_ICorDebugValue2, (LPVOID *) &pValue2))) return FALSE;
        if(FAILED(pValue2->GetExactType(&pType))) return FALSE;
        if(FAILED(pType->GetBase(&pBaseType)) || pBaseType == NULL) return FALSE;
        if(FAILED(GetTypeOfValue(pBaseType, baseTypeName, mdNameLen))) return  FALSE;

        return (_wcsncmp(baseTypeName, W("System.Enum"), 11) == 0);
    }

    static HRESULT AddGenericArgs(ICorDebugType * pType, __inout_ecount(typeNameLen) WCHAR* typeName, ULONG typeNameLen)
    {
        bool isFirst = true;
        ToRelease<ICorDebugTypeEnum> pTypeEnum;
        if(SUCCEEDED(pType->EnumerateTypeParameters(&pTypeEnum)))
        {
            ULONG numTypes = 0;
            ToRelease<ICorDebugType> pCurrentTypeParam;

            while(SUCCEEDED(pTypeEnum->Next(1, &pCurrentTypeParam, &numTypes)))
            {
                if(numTypes == 0) break;

                if(isFirst)
                {
                    isFirst = false;
                    wcsncat_s(typeName, typeNameLen, W("&lt;"), typeNameLen);
                }
                else wcsncat_s(typeName, typeNameLen, W(","), typeNameLen);

                WCHAR typeParamName[mdNameLen];
                typeParamName[0] = L'\0';
                GetTypeOfValue(pCurrentTypeParam, typeParamName, mdNameLen);
                wcsncat_s(typeName, typeNameLen, typeParamName, typeNameLen);
            }
            if(!isFirst)
                wcsncat_s(typeName, typeNameLen, W("&gt;"), typeNameLen);
        }

        return S_OK;
    }

    static HRESULT GetTypeOfValue(ICorDebugType * pType, __inout_ecount(typeNameLen) WCHAR* typeName, ULONG typeNameLen)
    {
        HRESULT Status = S_OK;

        CorElementType corElemType;
        IfFailRet(pType->GetType(&corElemType));

        switch (corElemType)
        {
        //List of unsupported CorElementTypes:
        //ELEMENT_TYPE_END            = 0x0,
        //ELEMENT_TYPE_VAR            = 0x13,     // a class type variable VAR <U1>
        //ELEMENT_TYPE_GENERICINST    = 0x15,     // GENERICINST <generic type> <argCnt> <arg1> ... <argn>
        //ELEMENT_TYPE_TYPEDBYREF     = 0x16,     // TYPEDREF  (it takes no args) a typed referece to some other type
        //ELEMENT_TYPE_MVAR           = 0x1e,     // a method type variable MVAR <U1>
        //ELEMENT_TYPE_CMOD_REQD      = 0x1F,     // required C modifier : E_T_CMOD_REQD <mdTypeRef/mdTypeDef>
        //ELEMENT_TYPE_CMOD_OPT       = 0x20,     // optional C modifier : E_T_CMOD_OPT <mdTypeRef/mdTypeDef>
        //ELEMENT_TYPE_INTERNAL       = 0x21,     // INTERNAL <typehandle>
        //ELEMENT_TYPE_MAX            = 0x22,     // first invalid element type
        //ELEMENT_TYPE_MODIFIER       = 0x40,
        //ELEMENT_TYPE_SENTINEL       = 0x01 | ELEMENT_TYPE_MODIFIER, // sentinel for varargs
        //ELEMENT_TYPE_PINNED         = 0x05 | ELEMENT_TYPE_MODIFIER,
        //ELEMENT_TYPE_R4_HFA         = 0x06 | ELEMENT_TYPE_MODIFIER, // used only internally for R4 HFA types
        //ELEMENT_TYPE_R8_HFA         = 0x07 | ELEMENT_TYPE_MODIFIER, // used only internally for R8 HFA types
        default:
            swprintf_s(typeName, typeNameLen, W("(Unhandled CorElementType: 0x%x)\0"), corElemType);
            break;

        case ELEMENT_TYPE_VALUETYPE:
        case ELEMENT_TYPE_CLASS:
            {
                //Defaults in case we fail...
                if(corElemType == ELEMENT_TYPE_VALUETYPE) swprintf_s(typeName, typeNameLen, W("struct\0"));
                else swprintf_s(typeName, typeNameLen, W("class\0"));

                mdTypeDef typeDef;
                ToRelease<ICorDebugClass> pClass;
                if(SUCCEEDED(pType->GetClass(&pClass)) && SUCCEEDED(pClass->GetToken(&typeDef)))
                {
                    ToRelease<ICorDebugModule> pModule;
                    IfFailRet(pClass->GetModule(&pModule));

                    ToRelease<IUnknown> pMDUnknown;
                    ToRelease<IMetaDataImport> pMD;
                    IfFailRet(pModule->GetMetaDataInterface(IID_IMetaDataImport, &pMDUnknown));
                    IfFailRet(pMDUnknown->QueryInterface(IID_IMetaDataImport, (LPVOID*) &pMD));

                    if(SUCCEEDED(NameForToken_s(TokenFromRid(typeDef, mdtTypeDef), pMD, g_mdName, mdNameLen, false)))
                        swprintf_s(typeName, typeNameLen, W("%s\0"), g_mdName);
                }
                AddGenericArgs(pType, typeName, typeNameLen);
            }
            break;
        case ELEMENT_TYPE_VOID:
            swprintf_s(typeName, typeNameLen, W("void\0"));
            break;
        case ELEMENT_TYPE_BOOLEAN:
            swprintf_s(typeName, typeNameLen, W("bool\0"));
            break;
        case ELEMENT_TYPE_CHAR:
            swprintf_s(typeName, typeNameLen, W("char\0"));
            break;
        case ELEMENT_TYPE_I1:
            swprintf_s(typeName, typeNameLen, W("signed byte\0"));
            break;
        case ELEMENT_TYPE_U1:
            swprintf_s(typeName, typeNameLen, W("byte\0"));
            break;
        case ELEMENT_TYPE_I2:
            swprintf_s(typeName, typeNameLen, W("short\0"));
            break;
        case ELEMENT_TYPE_U2:
            swprintf_s(typeName, typeNameLen, W("unsigned short\0"));
            break;
        case ELEMENT_TYPE_I4:
            swprintf_s(typeName, typeNameLen, W("int\0"));
            break;
        case ELEMENT_TYPE_U4:
            swprintf_s(typeName, typeNameLen, W("unsigned int\0"));
            break;
        case ELEMENT_TYPE_I8:
            swprintf_s(typeName, typeNameLen, W("long\0"));
            break;
        case ELEMENT_TYPE_U8:
            swprintf_s(typeName, typeNameLen, W("unsigned long\0"));
            break;
        case ELEMENT_TYPE_R4:
            swprintf_s(typeName, typeNameLen, W("float\0"));
            break;
        case ELEMENT_TYPE_R8:
            swprintf_s(typeName, typeNameLen, W("double\0"));
            break;
        case ELEMENT_TYPE_OBJECT:
            swprintf_s(typeName, typeNameLen, W("object\0"));
            break;
        case ELEMENT_TYPE_STRING:
            swprintf_s(typeName, typeNameLen, W("string\0"));
            break;
        case ELEMENT_TYPE_I:
            swprintf_s(typeName, typeNameLen, W("IntPtr\0"));
            break;
        case ELEMENT_TYPE_U:
            swprintf_s(typeName, typeNameLen, W("UIntPtr\0"));
            break;
        case ELEMENT_TYPE_SZARRAY:
        case ELEMENT_TYPE_ARRAY:
        case ELEMENT_TYPE_BYREF:
        case ELEMENT_TYPE_PTR:
            {
                ToRelease<ICorDebugType> pFirstParameter;
                if(SUCCEEDED(pType->GetFirstTypeParameter(&pFirstParameter)))
                    GetTypeOfValue(pFirstParameter, typeName, typeNameLen);
                else
                    swprintf_s(typeName, typeNameLen, W("<unknown>\0"));

                switch(corElemType)
                {
                case ELEMENT_TYPE_SZARRAY:
                    wcsncat_s(typeName, typeNameLen, W("[]\0"), typeNameLen);
                    return S_OK;
                case ELEMENT_TYPE_ARRAY:
                    {
                        ULONG32 rank = 0;
                        pType->GetRank(&rank);
                        wcsncat_s(typeName, typeNameLen, W("["), typeNameLen);
                        for(ULONG32 i = 0; i < rank - 1; i++)
                        {
                            //
                            wcsncat_s(typeName, typeNameLen, W(","), typeNameLen);
                        }
                        wcsncat_s(typeName, typeNameLen, W("]\0"), typeNameLen);
                    }
                    return S_OK;
                case ELEMENT_TYPE_BYREF:
                    wcsncat_s(typeName, typeNameLen, W("&\0"), typeNameLen);
                    return S_OK;
                case ELEMENT_TYPE_PTR:
                    wcsncat_s(typeName, typeNameLen, W("*\0"), typeNameLen);
                    return S_OK;
                default:
                    // note we can never reach here as this is a nested switch
                    // and corElemType can only be one of the values above
                    break;
                }
            }
            break;
        case ELEMENT_TYPE_FNPTR:
            swprintf_s(typeName, typeNameLen, W("*(...)\0"));
            break;
        case ELEMENT_TYPE_TYPEDBYREF:
            swprintf_s(typeName, typeNameLen, W("typedbyref\0"));
            break;
        }
        return S_OK;
    }

    static HRESULT GetTypeOfValue(ICorDebugValue * pValue, __inout_ecount(typeNameLen) WCHAR* typeName, ULONG typeNameLen)
    {
        HRESULT Status = S_OK;

        CorElementType corElemType;
        IfFailRet(pValue->GetType(&corElemType));

        ToRelease<ICorDebugType> pType;
        ToRelease<ICorDebugValue2> pValue2;
        if(SUCCEEDED(pValue->QueryInterface(IID_ICorDebugValue2, (void**) &pValue2)) && SUCCEEDED(pValue2->GetExactType(&pType)))
            return GetTypeOfValue(pType, typeName, typeNameLen);
        else
            swprintf_s(typeName, typeNameLen, W("<unknown>\0"));

        return S_OK;
    }

    static HRESULT PrintEnumValue(ICorDebugValue* pInputValue, BYTE* enumValue)
    {
        HRESULT Status = S_OK;

        ToRelease<ICorDebugValue> pValue;
        IfFailRet(DereferenceAndUnboxValue(pInputValue, &pValue, NULL));

        mdTypeDef currentTypeDef;
        ToRelease<ICorDebugClass> pClass;
        ToRelease<ICorDebugValue2> pValue2;
        ToRelease<ICorDebugType> pType;
        ToRelease<ICorDebugModule> pModule;
        IfFailRet(pValue->QueryInterface(IID_ICorDebugValue2, (LPVOID *) &pValue2));
        IfFailRet(pValue2->GetExactType(&pType));
        IfFailRet(pType->GetClass(&pClass));
        IfFailRet(pClass->GetModule(&pModule));
        IfFailRet(pClass->GetToken(&currentTypeDef));

        ToRelease<IUnknown> pMDUnknown;
        ToRelease<IMetaDataImport> pMD;
        IfFailRet(pModule->GetMetaDataInterface(IID_IMetaDataImport, &pMDUnknown));
        IfFailRet(pMDUnknown->QueryInterface(IID_IMetaDataImport, (LPVOID*) &pMD));


        //First, we need to figure out the underlying enum type so that we can correctly type cast the raw values of each enum constant
        //We get that from the non-static field of the enum variable (I think the field is called __value or something similar)
        ULONG numFields = 0;
        HCORENUM fEnum = NULL;
        mdFieldDef fieldDef;
        CorElementType enumUnderlyingType = ELEMENT_TYPE_END;
        while(SUCCEEDED(pMD->EnumFields(&fEnum, currentTypeDef, &fieldDef, 1, &numFields)) && numFields != 0)
        {
            DWORD             fieldAttr = 0;
            PCCOR_SIGNATURE   pSignatureBlob = NULL;
            ULONG             sigBlobLength = 0;
            if(SUCCEEDED(pMD->GetFieldProps(fieldDef, NULL, NULL, 0, NULL, &fieldAttr, &pSignatureBlob, &sigBlobLength, NULL, NULL, NULL)))
            {
                if((fieldAttr & fdStatic) == 0)
                {
                    CorSigUncompressCallingConv(pSignatureBlob);
                    enumUnderlyingType = CorSigUncompressElementType(pSignatureBlob);
                    break;
                }
            }
        }
        pMD->CloseEnum(fEnum);


        //Now that we know the underlying enum type, let's decode the enum variable into OR-ed, human readable enum contants
        fEnum = NULL;
        bool isFirst = true;
        ULONG64 remainingValue = *((ULONG64*)enumValue);
        while(SUCCEEDED(pMD->EnumFields(&fEnum, currentTypeDef, &fieldDef, 1, &numFields)) && numFields != 0)
        {
            ULONG             nameLen = 0;
            DWORD             fieldAttr = 0;
            WCHAR             mdName[mdNameLen];
            UVCP_CONSTANT     pRawValue = NULL;
            ULONG             rawValueLength = 0;
            if(SUCCEEDED(pMD->GetFieldProps(fieldDef, NULL, mdName, mdNameLen, &nameLen, &fieldAttr, NULL, NULL, NULL, &pRawValue, &rawValueLength)))
            {
                DWORD enumValueRequiredAttributes = fdPublic | fdStatic | fdLiteral | fdHasDefault;
                if((fieldAttr & enumValueRequiredAttributes) != enumValueRequiredAttributes)
                    continue;

                ULONG64 currentConstValue = 0;
                switch (enumUnderlyingType)
                {
                    case ELEMENT_TYPE_CHAR:
                    case ELEMENT_TYPE_I1:
                        currentConstValue = (ULONG64)(*((CHAR*)pRawValue));
                        break;
                    case ELEMENT_TYPE_U1:
                        currentConstValue = (ULONG64)(*((BYTE*)pRawValue));
                        break;
                    case ELEMENT_TYPE_I2:
                        currentConstValue = (ULONG64)(*((SHORT*)pRawValue));
                        break;
                    case ELEMENT_TYPE_U2:
                        currentConstValue = (ULONG64)(*((USHORT*)pRawValue));
                        break;
                    case ELEMENT_TYPE_I4:
                        currentConstValue = (ULONG64)(*((INT32*)pRawValue));
                        break;
                    case ELEMENT_TYPE_U4:
                        currentConstValue = (ULONG64)(*((UINT32*)pRawValue));
                        break;
                    case ELEMENT_TYPE_I8:
                        currentConstValue = (ULONG64)(*((LONG*)pRawValue));
                        break;
                    case ELEMENT_TYPE_U8:
                        currentConstValue = (ULONG64)(*((ULONG*)pRawValue));
                        break;
                    case ELEMENT_TYPE_I:
                        currentConstValue = (ULONG64)(*((int*)pRawValue));
                        break;
                    case ELEMENT_TYPE_U:
                    case ELEMENT_TYPE_R4:
                    case ELEMENT_TYPE_R8:
                    // Technically U and the floating-point ones are options in the CLI, but not in the CLS or C#, so these are NYI
                    default:
                        currentConstValue = 0;
                }

                if((currentConstValue == remainingValue) || ((currentConstValue != 0) && ((currentConstValue & remainingValue) == currentConstValue)))
                {
                    remainingValue &= ~currentConstValue;
                    if(isFirst)
                    {
                        ExtOut(" = %S", mdName);
                        isFirst = false;
                    }
                    else ExtOut(" | %S", mdName);
                }
            }
        }
        pMD->CloseEnum(fEnum);

        return S_OK;
    }

    static HRESULT PrintStringValue(ICorDebugValue * pValue)
    {
        HRESULT Status;

        ToRelease<ICorDebugStringValue> pStringValue;
        IfFailRet(pValue->QueryInterface(IID_ICorDebugStringValue, (LPVOID*) &pStringValue));

        ULONG32 cchValue;
        IfFailRet(pStringValue->GetLength(&cchValue));
        cchValue++;         // Allocate one more for null terminator

        CQuickString quickString;
        quickString.Alloc(cchValue);

        ULONG32 cchValueReturned;
        IfFailRet(pStringValue->GetString(
            cchValue,
            &cchValueReturned,
            quickString.String()));

        ExtOut(" = \"%S\"\n", quickString.String());

        return S_OK;
    }

    static HRESULT PrintSzArrayValue(ICorDebugValue * pValue, ICorDebugILFrame * pILFrame, IMetaDataImport * pMD, int indent, __in_z WCHAR* varToExpand, __inout_ecount(currentExpansionSize) WCHAR* currentExpansion, DWORD currentExpansionSize, int currentFrame)
    {
        HRESULT Status = S_OK;

        ToRelease<ICorDebugArrayValue> pArrayValue;
        IfFailRet(pValue->QueryInterface(IID_ICorDebugArrayValue, (LPVOID*) &pArrayValue));

        ULONG32 nRank;
        IfFailRet(pArrayValue->GetRank(&nRank));
        if (nRank != 1)
        {
            return E_UNEXPECTED;
        }

        ULONG32 cElements;
        IfFailRet(pArrayValue->GetCount(&cElements));

        if (cElements == 0) ExtOut("   (empty)\n");
        else if (cElements == 1) ExtOut("   (1 element)\n");
        else ExtOut("   (%d elements)\n", cElements);

        if(!ShouldExpandVariable(varToExpand, currentExpansion)) return S_OK;
        size_t currentExpansionLen = _wcslen(currentExpansion);

        for (ULONG32 i=0; i < cElements; i++)
        {
            for(int j = 0; j <= indent; j++) ExtOut("    ");
            currentExpansion[currentExpansionLen] = L'\0';
            swprintf_s(currentExpansion, mdNameLen, W("%s.[%d]\0"), currentExpansion, i);

            bool printed = false;
            CorElementType corElemType;
            ToRelease<ICorDebugType> pFirstParameter;
            ToRelease<ICorDebugValue2> pValue2;
            ToRelease<ICorDebugType> pType;
            if(SUCCEEDED(pArrayValue->QueryInterface(IID_ICorDebugValue2, (LPVOID *) &pValue2)) && SUCCEEDED(pValue2->GetExactType(&pType)))
            {
                if(SUCCEEDED(pType->GetFirstTypeParameter(&pFirstParameter)) && SUCCEEDED(pFirstParameter->GetType(&corElemType)))
                {
                    switch(corElemType)
                    {
                    //If the array element is something that we can expand with !clrstack, show information about the type of this element
                    case ELEMENT_TYPE_VALUETYPE:
                    case ELEMENT_TYPE_CLASS:
                    case ELEMENT_TYPE_SZARRAY:
                        {
                            WCHAR typeOfElement[mdNameLen];
                            GetTypeOfValue(pFirstParameter, typeOfElement, mdNameLen);
                            DMLOut(" |- %s = %S", DMLManagedVar(currentExpansion, currentFrame, i), typeOfElement);
                            printed = true;
                        }
                        break;
                    default:
                        break;
                    }
                }
            }
            if(!printed) DMLOut(" |- %s", DMLManagedVar(currentExpansion, currentFrame, i));

            ToRelease<ICorDebugValue> pElementValue;
            IfFailRet(pArrayValue->GetElementAtPosition(i, &pElementValue));
            IfFailRet(PrintValue(pElementValue, pILFrame, pMD, indent + 1, varToExpand, currentExpansion, currentExpansionSize, currentFrame));
        }

        return S_OK;
    }

    static HRESULT PrintValue(ICorDebugValue * pInputValue, ICorDebugILFrame * pILFrame, IMetaDataImport * pMD, int indent, __in_z WCHAR* varToExpand, __inout_ecount(currentExpansionSize) WCHAR* currentExpansion, DWORD currentExpansionSize, int currentFrame)
    {
        HRESULT Status = S_OK;

        BOOL isNull = TRUE;
        ToRelease<ICorDebugValue> pValue;
        IfFailRet(DereferenceAndUnboxValue(pInputValue, &pValue, &isNull));

        if(isNull)
        {
            ExtOut(" = null\n");
            return S_OK;
        }

        ULONG32 cbSize;
        IfFailRet(pValue->GetSize(&cbSize));
        ArrayHolder<BYTE> rgbValue = new NOTHROW BYTE[cbSize];
        if (rgbValue == NULL)
        {
            ReportOOM();
            return E_OUTOFMEMORY;
        }

        memset(rgbValue.GetPtr(), 0, cbSize * sizeof(BYTE));

        CorElementType corElemType;
        IfFailRet(pValue->GetType(&corElemType));
        if (corElemType == ELEMENT_TYPE_STRING)
        {
            return PrintStringValue(pValue);
        }

        if (corElemType == ELEMENT_TYPE_SZARRAY)
        {
            return PrintSzArrayValue(pValue, pILFrame, pMD, indent, varToExpand, currentExpansion, currentExpansionSize, currentFrame);
        }

        ToRelease<ICorDebugGenericValue> pGenericValue;
        IfFailRet(pValue->QueryInterface(IID_ICorDebugGenericValue, (LPVOID*) &pGenericValue));
        IfFailRet(pGenericValue->GetValue((LPVOID) &(rgbValue[0])));

        if(IsEnum(pValue))
        {
            Status = PrintEnumValue(pValue, rgbValue);
            ExtOut("\n");
            return Status;
        }

        switch (corElemType)
        {
        default:
            ExtOut("  (Unhandled CorElementType: 0x%x)\n", corElemType);
            break;

        case ELEMENT_TYPE_PTR:
            ExtOut("  = <pointer>\n");
            break;

        case ELEMENT_TYPE_FNPTR:
            {
                CORDB_ADDRESS addr = 0;
                ToRelease<ICorDebugReferenceValue> pReferenceValue = NULL;
                if(SUCCEEDED(pValue->QueryInterface(IID_ICorDebugReferenceValue, (LPVOID*) &pReferenceValue)))
                    pReferenceValue->GetValue(&addr);
                ExtOut("  = <function pointer 0x%x>\n", addr);
            }
            break;

        case ELEMENT_TYPE_VALUETYPE:
        case ELEMENT_TYPE_CLASS:
            CORDB_ADDRESS addr;
            if(SUCCEEDED(pValue->GetAddress(&addr)))
            {
                ExtOut(" @ 0x%I64x\n", addr);
            }
            else
            {
                ExtOut("\n");
            }
            ProcessFields(pValue, NULL, pILFrame, indent + 1, varToExpand, currentExpansion, currentExpansionSize, currentFrame);
            break;

        case ELEMENT_TYPE_BOOLEAN:
            ExtOut("  = %s\n", rgbValue[0] == 0 ? "false" : "true");
            break;

        case ELEMENT_TYPE_CHAR:
            ExtOut("  = '%C'\n", *(WCHAR *) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_I1:
            ExtOut("  = %d\n", *(char*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_U1:
            ExtOut("  = %d\n", *(unsigned char*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_I2:
            ExtOut("  = %hd\n", *(short*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_U2:
            ExtOut("  = %hu\n", *(unsigned short*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_I:
            ExtOut("  = %d\n", *(int*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_U:
            ExtOut("  = %u\n", *(unsigned int*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_I4:
            ExtOut("  = %d\n", *(int*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_U4:
            ExtOut("  = %u\n", *(unsigned int*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_I8:
            ExtOut("  = %I64d\n", *(__int64*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_U8:
            ExtOut("  = %I64u\n", *(unsigned __int64*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_R4:
            ExtOut("  = %f\n", (double) *(float*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_R8:
            ExtOut("  = %f\n", *(double*) &(rgbValue[0]));
            break;

        case ELEMENT_TYPE_OBJECT:
            ExtOut("  = object\n");
            break;

            // TODO: The following corElementTypes are not yet implemented here.  Array
            // might be interesting to add, though the others may be of rather limited use:
            // ELEMENT_TYPE_ARRAY          = 0x14,     // MDARRAY <type> <rank> <bcount> <bound1> ... <lbcount> <lb1> ...
            //
            // ELEMENT_TYPE_GENERICINST    = 0x15,     // GENERICINST <generic type> <argCnt> <arg1> ... <argn>
        }

        return S_OK;
    }

    static HRESULT PrintParameters(BOOL bParams, BOOL bLocals, IMetaDataImport * pMD, mdTypeDef typeDef, mdMethodDef methodDef, ICorDebugILFrame * pILFrame, ICorDebugModule * pModule, __in_z WCHAR* varToExpand, int currentFrame)
    {
        HRESULT Status = S_OK;

        ULONG cParams = 0;
        ToRelease<ICorDebugValueEnum> pParamEnum;
        IfFailRet(pILFrame->EnumerateArguments(&pParamEnum));
        IfFailRet(pParamEnum->GetCount(&cParams));
        if (cParams > 0 && bParams)
        {
            DWORD methAttr = 0;
            IfFailRet(pMD->GetMethodProps(methodDef, NULL, NULL, 0, NULL, &methAttr, NULL, NULL, NULL, NULL));

            ExtOut("\nPARAMETERS:\n");
            for (ULONG i=0; i < cParams; i++)
            {
                ULONG paramNameLen = 0;
                mdParamDef paramDef;
                WCHAR paramName[mdNameLen] = W("\0");

                if(i == 0 && (methAttr & mdStatic) == 0)
                    swprintf_s(paramName, mdNameLen, W("this\0"));
                else
                {
                    int idx = ((methAttr & mdStatic) == 0)? i : (i + 1);
                    if(SUCCEEDED(pMD->GetParamForMethodIndex(methodDef, idx, &paramDef)))
                        pMD->GetParamProps(paramDef, NULL, NULL, paramName, mdNameLen, &paramNameLen, NULL, NULL, NULL, NULL);
                }
                if(_wcslen(paramName) == 0)
                    swprintf_s(paramName, mdNameLen, W("param_%d\0"), i);

                ToRelease<ICorDebugValue> pValue;
                ULONG cArgsFetched;
                Status = pParamEnum->Next(1, &pValue, &cArgsFetched);

                if (FAILED(Status))
                {
                    ExtOut("  + (Error 0x%x retrieving parameter '%S')\n", Status, paramName);
                    continue;
                }

                if (Status == S_FALSE)
                {
                    break;
                }

                WCHAR typeName[mdNameLen] = W("\0");
                GetTypeOfValue(pValue, typeName, mdNameLen);
                DMLOut("  + %S %s", typeName, DMLManagedVar(paramName, currentFrame, paramName));

                ToRelease<ICorDebugReferenceValue> pRefValue;
                if(SUCCEEDED(pValue->QueryInterface(IID_ICorDebugReferenceValue, (void**)&pRefValue)) && pRefValue != NULL)
                {
                    BOOL bIsNull = TRUE;
                    pRefValue->IsNull(&bIsNull);
                    if(bIsNull)
                    {
                        ExtOut(" = null\n");
                        continue;
                    }
                }

                WCHAR currentExpansion[mdNameLen];
                swprintf_s(currentExpansion, mdNameLen, W("%s\0"), paramName);
                if((Status=PrintValue(pValue, pILFrame, pMD, 0, varToExpand, currentExpansion, mdNameLen, currentFrame)) != S_OK)
                    ExtOut("  + (Error 0x%x printing parameter %d)\n", Status, i);
            }
        }
        else if (cParams == 0 && bParams)
            ExtOut("\nPARAMETERS: (none)\n");

        ULONG cLocals = 0;
        ToRelease<ICorDebugValueEnum> pLocalsEnum;
        IfFailRet(pILFrame->EnumerateLocalVariables(&pLocalsEnum));
        IfFailRet(pLocalsEnum->GetCount(&cLocals));
        if (cLocals > 0 && bLocals)
        {
            bool symbolsAvailable = false;
            SymbolReader symReader;
            if(SUCCEEDED(symReader.LoadSymbols(pMD, pModule)))
                symbolsAvailable = true;
            ExtOut("\nLOCALS:\n");
            for (ULONG i=0; i < cLocals; i++)
            {
                WCHAR paramName[mdNameLen] = W("\0");

                ToRelease<ICorDebugValue> pValue;
                if(symbolsAvailable)
                {
                    Status = symReader.GetNamedLocalVariable(pILFrame, i, paramName, mdNameLen, &pValue);
                }
                else
                {
                    ULONG cArgsFetched;
                    Status = pLocalsEnum->Next(1, &pValue, &cArgsFetched);
                }
                if(_wcslen(paramName) == 0)
                    swprintf_s(paramName, mdNameLen, W("local_%d\0"), i);

                if (FAILED(Status))
                {
                    ExtOut("  + (Error 0x%x retrieving local variable '%S')\n", Status, paramName);
                    continue;
                }

                if (Status == S_FALSE)
                {
                    break;
                }

                WCHAR typeName[mdNameLen] = W("\0");
                GetTypeOfValue(pValue, typeName, mdNameLen);
                DMLOut("  + %S %s", typeName, DMLManagedVar(paramName, currentFrame, paramName));

                ToRelease<ICorDebugReferenceValue> pRefValue = NULL;
                if(SUCCEEDED(pValue->QueryInterface(IID_ICorDebugReferenceValue, (void**)&pRefValue)) && pRefValue != NULL)
                {
                    BOOL bIsNull = TRUE;
                    pRefValue->IsNull(&bIsNull);
                    if(bIsNull)
                    {
                        ExtOut(" = null\n");
                        continue;
                    }
                }

                WCHAR currentExpansion[mdNameLen];
                swprintf_s(currentExpansion, mdNameLen, W("%s\0"), paramName);
                if((Status=PrintValue(pValue, pILFrame, pMD, 0, varToExpand, currentExpansion, mdNameLen, currentFrame)) != S_OK)
                    ExtOut("  + (Error 0x%x printing local variable %d)\n", Status, i);
            }
        }
        else if (cLocals == 0 && bLocals)
            ExtOut("\nLOCALS: (none)\n");

        if(bParams || bLocals)
            ExtOut("\n");

        return S_OK;
    }

    static HRESULT ProcessFields(ICorDebugValue* pInputValue, ICorDebugType* pTypeCast, ICorDebugILFrame * pILFrame, int indent, __in_z WCHAR* varToExpand, __inout_ecount(currentExpansionSize) WCHAR* currentExpansion, DWORD currentExpansionSize, int currentFrame)
    {
        if(!ShouldExpandVariable(varToExpand, currentExpansion)) return S_OK;
        size_t currentExpansionLen = _wcslen(currentExpansion);

        HRESULT Status = S_OK;

        BOOL isNull = FALSE;
        ToRelease<ICorDebugValue> pValue;
        IfFailRet(DereferenceAndUnboxValue(pInputValue, &pValue, &isNull));

        if(isNull) return S_OK;

        mdTypeDef currentTypeDef;
        ToRelease<ICorDebugClass> pClass;
        ToRelease<ICorDebugValue2> pValue2;
        ToRelease<ICorDebugType> pType;
        ToRelease<ICorDebugModule> pModule;
        IfFailRet(pValue->QueryInterface(IID_ICorDebugValue2, (LPVOID *) &pValue2));
        if(pTypeCast == NULL)
            IfFailRet(pValue2->GetExactType(&pType));
        else
        {
            pType = pTypeCast;
            pType->AddRef();
        }
        IfFailRet(pType->GetClass(&pClass));
        IfFailRet(pClass->GetModule(&pModule));
        IfFailRet(pClass->GetToken(&currentTypeDef));

        ToRelease<IUnknown> pMDUnknown;
        ToRelease<IMetaDataImport> pMD;
        IfFailRet(pModule->GetMetaDataInterface(IID_IMetaDataImport, &pMDUnknown));
        IfFailRet(pMDUnknown->QueryInterface(IID_IMetaDataImport, (LPVOID*) &pMD));

        WCHAR baseTypeName[mdNameLen] = W("\0");
        ToRelease<ICorDebugType> pBaseType;
        if(SUCCEEDED(pType->GetBase(&pBaseType)) && pBaseType != NULL && SUCCEEDED(GetTypeOfValue(pBaseType, baseTypeName, mdNameLen)))
        {
            if(_wcsncmp(baseTypeName, W("System.Enum"), 11) == 0)
                return S_OK;
            else if(_wcsncmp(baseTypeName, W("System.Object"), 13) != 0 && _wcsncmp(baseTypeName, W("System.ValueType"), 16) != 0)
            {
                currentExpansion[currentExpansionLen] = W('\0');
                wcscat_s(currentExpansion, currentExpansionSize, W(".\0"));
                wcscat_s(currentExpansion, currentExpansionSize, W("[basetype]"));
                for(int i = 0; i < indent; i++) ExtOut("    ");
                DMLOut(" |- %S %s\n", baseTypeName, DMLManagedVar(currentExpansion, currentFrame, W("[basetype]")));

                if(ShouldExpandVariable(varToExpand, currentExpansion))
                    ProcessFields(pInputValue, pBaseType, pILFrame, indent + 1, varToExpand, currentExpansion, currentExpansionSize, currentFrame);
            }
        }


        ULONG numFields = 0;
        HCORENUM fEnum = NULL;
        mdFieldDef fieldDef;
        while(SUCCEEDED(pMD->EnumFields(&fEnum, currentTypeDef, &fieldDef, 1, &numFields)) && numFields != 0)
        {
            ULONG             nameLen = 0;
            DWORD             fieldAttr = 0;
            WCHAR             mdName[mdNameLen];
            WCHAR             typeName[mdNameLen];
            if(SUCCEEDED(pMD->GetFieldProps(fieldDef, NULL, mdName, mdNameLen, &nameLen, &fieldAttr, NULL, NULL, NULL, NULL, NULL)))
            {
                currentExpansion[currentExpansionLen] = W('\0');
                wcscat_s(currentExpansion, currentExpansionSize, W(".\0"));
                wcscat_s(currentExpansion, currentExpansionSize, mdName);

                ToRelease<ICorDebugValue> pFieldVal;
                if(fieldAttr & fdLiteral)
                {
                    //TODO: Is it worth it??
                    //ExtOut(" |- const %S", mdName);
                }
                else
                {
                    for(int i = 0; i < indent; i++) ExtOut("    ");

                    if (fieldAttr & fdStatic)
                        pType->GetStaticFieldValue(fieldDef, pILFrame, &pFieldVal);
                    else
                    {
                        ToRelease<ICorDebugObjectValue> pObjValue;
                        if (SUCCEEDED(pValue->QueryInterface(IID_ICorDebugObjectValue, (LPVOID*) &pObjValue)))
                            pObjValue->GetFieldValue(pClass, fieldDef, &pFieldVal);
                    }

                    if(pFieldVal != NULL)
                    {
                        typeName[0] = L'\0';
                        GetTypeOfValue(pFieldVal, typeName, mdNameLen);
                        DMLOut(" |- %S %s", typeName, DMLManagedVar(currentExpansion, currentFrame, mdName));
                        PrintValue(pFieldVal, pILFrame, pMD, indent, varToExpand, currentExpansion, currentExpansionSize, currentFrame);
                    }
                    else if(!(fieldAttr & fdLiteral))
                        ExtOut(" |- < unknown type > %S\n", mdName);
                }
            }
        }
        pMD->CloseEnum(fEnum);
        return S_OK;
    }

public:

    // This is the main worker function used if !clrstack is called with "-i" to indicate
    // that the public ICorDebug* should be used instead of the private DAC interface. NOTE:
    // Currently only bParams is supported. NOTE: This is a work in progress and the
    // following would be good to do:
    //     * More thorough testing with interesting stacks, especially with transitions into
    //         and out of managed code.
    //     * Consider interleaving this code back into the main body of !clrstack if it turns
    //         out that there's a lot of duplication of code between these two functions.
    //         (Still unclear how things will look once locals is implemented.)
    static HRESULT ClrStackFromPublicInterface(BOOL bParams, BOOL bLocals, BOOL bSuppressLines, __in_z WCHAR* varToExpand = NULL, int onlyShowFrame = -1)
    {
        HRESULT Status;

        ICorDebugProcess* pCorDebugProcess;
        if (FAILED(Status = g_pRuntime->GetCorDebugInterface(&pCorDebugProcess)))
        {
            ExtOut("\n%sclrstack -i is unsupported on this target.\nThe ICorDebug interface cannot be constructed.\n\n", SOSPrefix);
            return Status;
        }

        ExtOut("\n\n\nDumping managed stack and managed variables using ICorDebug.\n");
        ExtOut("=============================================================================\n");

        ToRelease<ICorDebugThread> pThread;
        ToRelease<ICorDebugThread3> pThread3;
        ToRelease<ICorDebugStackWalk> pStackWalk;
        ULONG ulThreadID = 0;
        g_ExtSystem->GetCurrentThreadSystemId(&ulThreadID);

        IfFailRet(pCorDebugProcess->GetThread(ulThreadID, &pThread));
        IfFailRet(pThread->QueryInterface(IID_ICorDebugThread3, (LPVOID *) &pThread3));
        IfFailRet(pThread3->CreateStackWalk(&pStackWalk));

        InternalFrameManager internalFrameManager;
        IfFailRet(internalFrameManager.Init(pThread3));

    #if defined(_AMD64_) || defined(_ARM64_) || defined(_RISCV64_) || defined(_LOONGARCH64_)
        ExtOut("%-16s %-16s %s\n", "Child SP", "IP", "Call Site");
    #elif defined(_X86_) || defined(_ARM_)
        ExtOut("%-8s %-8s %s\n", "Child SP", "IP", "Call Site");
    #endif

        int currentFrame = -1;

        for (Status = S_OK; ; Status = pStackWalk->Next())
        {
            currentFrame++;

            if (Status == CORDBG_S_AT_END_OF_STACK)
            {
                ExtOut("Stack walk complete.\n");
                break;
            }
            IfFailRet(Status);

            if (IsInterrupt())
            {
                ExtOut("<interrupted>\n");
                break;
            }

            // This is a workaround for a problem in the MacOS DAC/DBI PAL. The PAL exception
            // handling is unnecessarily enabled for DLLs and is not properly passing what I
            // think is recoverable stack fault on to the OS. Instead it is causing a fault
            // GP fault. Putting this struct in the heap works around this fault.
            ArrayHolder<CROSS_PLATFORM_CONTEXT> context = new CROSS_PLATFORM_CONTEXT[1];
            ULONG32 cbContextActual;
            if ((Status = pStackWalk->GetContext(
                g_targetMachine->GetFullContextFlags(),
                sizeof(CROSS_PLATFORM_CONTEXT),
                &cbContextActual,
                (BYTE *)context.GetPtr())) != S_OK)
            {
                if (FAILED(Status))
                {
                    ExtOut("GetFrameContext failed: %lx\n", Status);
                }
                break;
            }

            // First find the info for the Frame object, if the current frame has an associated clr!Frame.
            CLRDATA_ADDRESS sp = GetSP(*context.GetPtr());
            CLRDATA_ADDRESS ip = GetIP(*context.GetPtr());

            ToRelease<ICorDebugFrame> pFrame;
            IfFailRet(pStackWalk->GetFrame(&pFrame));
            if (Status == S_FALSE)
            {
                DMLOut("%p %s [NativeStackFrame]\n", SOS_PTR(sp), DMLIP(ip));
                continue;
            }

            // TODO: What about internal frames preceding the above native stack frame?
            // Should I just exclude the above native stack frame from the output?
            // TODO: Compare caller frame (instead of current frame) against internal frame,
            // to deal with issues of current frame's current SP being closer to leaf than
            // EE Frames it pushes.  By "caller" I mean not just managed caller, but the
            // very next non-internal frame dbi would return (native or managed). OR...
            // perhaps I should use GetStackRange() instead, to see if the internal frame
            // appears leafier than the base-part of the range of the currently iterated
            // stack frame?  I think I like that better.
            _ASSERTE(pFrame != NULL);
            IfFailRet(internalFrameManager.PrintPrecedingInternalFrames(pFrame));

            // Print the stack and instruction pointers.
            DMLOut("%p %s ", SOS_PTR(sp), DMLIP(ip));

            ToRelease<ICorDebugRuntimeUnwindableFrame> pRuntimeUnwindableFrame;
            Status = pFrame->QueryInterface(IID_ICorDebugRuntimeUnwindableFrame, (LPVOID *) &pRuntimeUnwindableFrame);
            if (SUCCEEDED(Status))
            {
                ExtOut("[RuntimeUnwindableFrame]\n");
                continue;
            }

            // Print the method/Frame info

            // TODO: IS THE FOLLOWING NECESSARY, OR AM I GUARANTEED THAT ALL INTERNAL FRAMES
            // CAN BE FOUND VIA GetActiveInternalFrames?
            ToRelease<ICorDebugInternalFrame> pInternalFrame;
            Status = pFrame->QueryInterface(IID_ICorDebugInternalFrame, (LPVOID *) &pInternalFrame);
            if (SUCCEEDED(Status))
            {
                // This is a clr!Frame.
                LPCWSTR pwszFrameName = W("TODO: Implement GetFrameName");
                ExtOut("[%S: p] ", pwszFrameName);
            }

            // Print the frame's associated function info, if it has any.
            ToRelease<ICorDebugILFrame> pILFrame;
            HRESULT hrILFrame = pFrame->QueryInterface(IID_ICorDebugILFrame, (LPVOID*) &pILFrame);

            if (SUCCEEDED(hrILFrame))
            {
                ToRelease<ICorDebugFunction> pFunction;
                Status = pFrame->GetFunction(&pFunction);
                if (FAILED(Status))
                {
                    // We're on a JITted frame, but there's no Function for it.  So it must
                    // be...
                    ExtOut("[IL Stub or LCG]\n");
                    continue;
                }

                ToRelease<ICorDebugClass> pClass;
                ToRelease<ICorDebugModule> pModule;
                mdMethodDef methodDef;
                IfFailRet(pFunction->GetClass(&pClass));
                IfFailRet(pFunction->GetModule(&pModule));
                IfFailRet(pFunction->GetToken(&methodDef));

                WCHAR wszModuleName[MAX_LONGPATH];
                ULONG32 cchModuleNameActual;
                IfFailRet(pModule->GetName(ARRAY_SIZE(wszModuleName), &cchModuleNameActual, wszModuleName));

                ToRelease<IUnknown> pMDUnknown;
                ToRelease<IMetaDataImport> pMD;
                IfFailRet(pModule->GetMetaDataInterface(IID_IMetaDataImport, &pMDUnknown));
                IfFailRet(pMDUnknown->QueryInterface(IID_IMetaDataImport, (LPVOID*) &pMD));

                mdTypeDef typeDef;
                IfFailRet(pClass->GetToken(&typeDef));

                // Note that we don't need to pretty print the class, as class name is
                // already printed from GetMethodName below

                CQuickBytes functionName;
                // TODO: WARNING: GetMethodName() appears to include lots of unexercised
                // code, as evidenced by some fundamental bugs I found.  It should either be
                // thoroughly reviewed, or some other more exercised code path to grab the
                // name should be used.
                // TODO: If we do stay with GetMethodName, it should be updated to print
                // generics properly.  Today, it does not show generic type parameters, and
                // if any arguments have a generic type, those arguments are just shown as
                // "__Canon", even when they're value types.
                GetMethodName(methodDef, pMD, &functionName);

                DMLOut(DMLManagedVar(W("-a"), currentFrame, (LPWSTR)functionName.Ptr()));
                ExtOut(" (%S)\n", wszModuleName);

                if (SUCCEEDED(hrILFrame) && (bParams || bLocals))
                {
                    if(onlyShowFrame == -1 || (onlyShowFrame >= 0 && currentFrame == onlyShowFrame))
                        IfFailRet(PrintParameters(bParams, bLocals, pMD, typeDef, methodDef, pILFrame, pModule, varToExpand, currentFrame));
                }
            }
        }
        ExtOut("=============================================================================\n");

        return S_OK;
    }
};

WString BuildRegisterOutput(const SOSStackRefData &ref, bool printObj)
{
    WString res;

    if (ref.HasRegisterInformation)
    {
        WCHAR reg[32];
        HRESULT hr = g_sos->GetRegisterName(ref.Register, ARRAY_SIZE(reg), reg, NULL);
        if (SUCCEEDED(hr))
            res = reg;
        else
            res = W("<unknown register>");

        if (ref.Offset)
        {
            int offset = ref.Offset;
            if (offset > 0)
            {
                res += W("+");
            }
            else
            {
                res += W("-");
                offset = -offset;
            }

            res += Hex(offset);
        }

        res += W(": ");
    }

    if (ref.Address)
        res += WString(Pointer(ref.Address));

    if (printObj)
    {
        if (ref.Address)
            res += W(" -> ");

        res += WString(ObjectPtr(ref.Object));
    }

    if (ref.Flags & SOSRefPinned)
    {
        res += W(" (pinned)");
    }

    if (ref.Flags & SOSRefInterior)
    {
        res += W(" (interior)");
    }

    return res;
}

void PrintRef(const SOSStackRefData &ref, TableOutput &out)
{
    WString res = BuildRegisterOutput(ref);

    if (ref.Object && (ref.Flags & SOSRefInterior) == 0)
    {
        WCHAR type[128];
        sos::BuildTypeWithExtraInfo(TO_TADDR(ref.Object), ARRAY_SIZE(type), type);

        res += WString(W(" - ")) + type;
    }

    out.WriteColumn(2, res);
}


class ClrStackImpl
{
public:
    static void PrintThread(ULONG osID, BOOL bParams, BOOL bLocals, BOOL bSuppressLines, BOOL bGC, BOOL bFull, BOOL bDisplayRegVals)
    {
        _ASSERTE(g_targetMachine != nullptr);

        // Symbols variables
        ULONG symlines = 0; // symlines will be non-zero only if SYMOPT_LOAD_LINES was set in the symbol options
        if (!bSuppressLines && SUCCEEDED(g_ExtSymbols->GetSymbolOptions(&symlines)))
        {
            symlines &= SYMOPT_LOAD_LINES;
        }

        if (symlines == 0)
            bSuppressLines = TRUE;

        ToRelease<IXCLRDataStackWalk> pStackWalk;

        HRESULT hr = CreateStackWalk(osID, &pStackWalk);
        if (FAILED(hr) || pStackWalk == NULL)
        {
            ExtOut("Failed to start stack walk: %lx\n", hr);
            return;
        }

        PDEBUG_STACK_FRAME currentNativeFrame = NULL;
        ULONG numNativeFrames = 0;
        if (bFull)
        {
            hr = GetContextStackTrace(osID, &numNativeFrames);
            if (FAILED(hr))
            {
                ExtOut("Failed to get native stack frames: %lx\n", hr);
                return;
            }
            currentNativeFrame = &g_Frames[0];
        }

        unsigned int refCount = 0, errCount = 0;
        ArrayHolder<SOSStackRefData> pRefs = NULL;
        ArrayHolder<SOSStackRefError> pErrs = NULL;
        if (bGC && FAILED(GetGCRefs(osID, &pRefs, &refCount, &pErrs, &errCount)))
            refCount = 0;

        TableOutput out(3, POINTERSIZE_HEX, AlignRight);
        out.WriteRow("Child SP", "IP", "Call Site");

        int frameNumber = 0;
        int internalFrames = 0;
        do
        {
            if (IsInterrupt())
            {
                ExtOut("<interrupted>\n");
                break;
            }
            CLRDATA_ADDRESS ip = 0, sp = 0;
            hr = GetFrameLocation(pStackWalk, &ip, &sp);
            if (SUCCEEDED(hr))
            {
                DacpFrameData FrameData;
                HRESULT frameDataResult = FrameData.Request(pStackWalk);
                if (SUCCEEDED(frameDataResult) && FrameData.frameAddr)
                    sp = FrameData.frameAddr;

                while ((numNativeFrames > 0) && (currentNativeFrame->StackOffset <= CDA_TO_UL64(sp)))
                {
                    if (currentNativeFrame->StackOffset != CDA_TO_UL64(sp))
                    {
                        PrintNativeStackFrame(out, currentNativeFrame, bSuppressLines);
                    }
                    currentNativeFrame++;
                    numNativeFrames--;
                }

                // Print the stack pointer.
                out.WriteColumn(0, sp);

                // Print the method/Frame info
                if (SUCCEEDED(frameDataResult) && FrameData.frameAddr)
                {
                    internalFrames++;

                    // Skip the instruction pointer because it doesn't really mean anything for method frames
                    out.WriteColumn(1, bFull ? String("") : NativePtr(ip));

                    // This is a clr!Frame.
                    out.WriteColumn(2, GetFrameFromAddress(TO_TADDR(FrameData.frameAddr), pStackWalk, bFull));

                    // Print out gc references for the Frame.
                    for (unsigned int i = 0; i < refCount; ++i)
                        if (pRefs[i].Source == sp)
                            PrintRef(pRefs[i], out);

                    // Print out an error message if we got one.
                    for (unsigned int i = 0; i < errCount; ++i)
                        if (pErrs[i].Source == sp)
                            out.WriteColumn(2, "Failed to enumerate GC references.");
                }
                else
                {
                    // To get the source line number of the actual code that threw an exception, the IP needs
                    // to be adjusted in certain cases.
                    //
                    // The IP of stack frame points to either:
                    //
                    // 1) Currently executing instruction (if you hit a breakpoint or are single stepping through).
                    // 2) The instruction that caused a hardware exception (div by zero, null ref, etc).
                    // 3) The instruction after the call to an internal runtime function (FCALL like IL_Throw,
                    //    JIT_OverFlow, etc.) that caused a software exception.
                    // 4) The instruction after the call to a managed function (non-leaf node).
                    //
                    // #3 and #4 are the cases that need IP adjusted back because they point after the call instruction
                    // and may point to the next (incorrect) IL instruction/source line.  We distinguish these from #1
                    // or #2 by either being non-leaf node stack frame (#4) or the present of an internal stack frame (#3).
                    bool bAdjustIPForLineNumber = frameNumber > 0 || internalFrames > 0;
                    frameNumber++;

                    // The unmodified IP is displayed which points after the exception in most cases. This means that the
                    // printed IP and the printed line number often will not map to one another and this is intentional.
                    out.WriteColumn(1, InstructionPtr(ip));
                    out.WriteColumn(2, MethodNameFromIP(ip, bSuppressLines, bFull, bFull, bAdjustIPForLineNumber));

                    // Print out gc references.  refCount will be zero if bGC is false (or if we
                    // failed to fetch gc reference information).
                    for (unsigned int i = 0; i < refCount; ++i)
                        if (pRefs[i].Source == ip && pRefs[i].StackPointer == sp)
                            PrintRef(pRefs[i], out);

                    // Print out an error message if we got one.
                    for (unsigned int i = 0; i < errCount; ++i)
                        if (pErrs[i].Source == sp)
                            out.WriteColumn(2, "Failed to enumerate GC references.");

                    if (bParams || bLocals)
                        PrintArgsAndLocals(pStackWalk, bParams, bLocals);
                }
            }

            if (bDisplayRegVals)
                PrintManagedFrameContext(pStackWalk);

            hr = pStackWalk->Next();
        } while (hr == S_OK);

        if (FAILED(hr))
        {
            // Normal stack walk ends with S_FALSE
            // Failure means the stalk walk did not complete normally
            ExtOut("<failed>\nStack Walk failed. Reported stack incomplete.\n");
#ifndef FEATURE_PAL
            if (!IsWindowsTarget())
            {
                ExtOut("Native stack walking is not supported on this target.\nStack walk will terminate at the first native frame.\n");
            }
#endif // FEATURE_PAL
        }

        while (numNativeFrames > 0)
        {
            PrintNativeStackFrame(out, currentNativeFrame, bSuppressLines);
            currentNativeFrame++;
            numNativeFrames--;
        }
    }

    static HRESULT PrintManagedFrameContext(IXCLRDataStackWalk *pStackWalk)
    {
        CROSS_PLATFORM_CONTEXT context;
        HRESULT hr = pStackWalk->GetContext(g_targetMachine->GetFullContextFlags(), g_targetMachine->GetContextSize(), NULL, (BYTE*)&context);
        if (FAILED(hr))
        {
            ExtOut("GetFrameContext failed: %lx\n", hr);
            return hr;
        }
        if (hr == S_FALSE)
        {
            // GetFrameContext returns S_FALSE if the frame iterator is invalid.  That's basically an error for us.
            return E_FAIL;
        }
        bool foundPlatform = false;
#if defined(SOS_TARGET_AMD64)
        if (IsDbgTargetAmd64())
        {
            foundPlatform = true;
            String outputFormat3 = "    %3s=%016llx %3s=%016llx %3s=%016llx\n";
            String outputFormat2 = "    %3s=%016llx %3s=%016llx\n";
            ExtOut(outputFormat3, "rsp", context.Amd64Context.Rsp, "rbp", context.Amd64Context.Rbp, "rip", context.Amd64Context.Rip);
            ExtOut(outputFormat3, "rax", context.Amd64Context.Rax, "rbx", context.Amd64Context.Rbx, "rcx", context.Amd64Context.Rcx);
            ExtOut(outputFormat3, "rdx", context.Amd64Context.Rdx, "rsi", context.Amd64Context.Rsi, "rdi", context.Amd64Context.Rdi);
            ExtOut(outputFormat3, "r8", context.Amd64Context.R8, "r9", context.Amd64Context.R9, "r10", context.Amd64Context.R10);
            ExtOut(outputFormat3, "r11", context.Amd64Context.R11, "r12", context.Amd64Context.R12, "r13", context.Amd64Context.R13);
            ExtOut(outputFormat2, "r14", context.Amd64Context.R14, "r15", context.Amd64Context.R15);
        }
#endif
#if defined(SOS_TARGET_X86)
        if (IsDbgTargetX86())
        {
            foundPlatform = true;
            String outputFormat3 = "    %3s=%08x %3s=%08x %3s=%08x\n";
            String outputFormat2 = "    %3s=%08x %3s=%08x\n";
            ExtOut(outputFormat3, "esp", context.X86Context.Esp, "ebp", context.X86Context.Ebp, "eip", context.X86Context.Eip);
            ExtOut(outputFormat3, "eax", context.X86Context.Eax, "ebx", context.X86Context.Ebx, "ecx", context.X86Context.Ecx);
            ExtOut(outputFormat3, "edx", context.X86Context.Edx, "esi", context.X86Context.Esi, "edi", context.X86Context.Edi);
        }
#endif
#if defined(SOS_TARGET_ARM)
        if (IsDbgTargetArm())
        {
            foundPlatform = true;
            String outputFormat3 = "    %3s=%08x %3s=%08x %3s=%08x\n";
            String outputFormat2 = "    %s=%08x %s=%08x\n";
            String outputFormat1 = "    %s=%08x\n";
            ExtOut(outputFormat3, "r0", context.ArmContext.R0, "r1", context.ArmContext.R1, "r2", context.ArmContext.R2);
            ExtOut(outputFormat3, "r3", context.ArmContext.R3, "r4", context.ArmContext.R4, "r5", context.ArmContext.R5);
            ExtOut(outputFormat3, "r6", context.ArmContext.R6, "r7", context.ArmContext.R7, "r8", context.ArmContext.R8);
            ExtOut(outputFormat3, "r9", context.ArmContext.R9, "r10", context.ArmContext.R10, "r11", context.ArmContext.R11);
            ExtOut(outputFormat1, "r12", context.ArmContext.R12);
            ExtOut(outputFormat3, "sp", context.ArmContext.Sp, "lr", context.ArmContext.Lr, "pc", context.ArmContext.Pc);
            ExtOut(outputFormat2, "cpsr", context.ArmContext.Cpsr, "fpscr", context.ArmContext.Fpscr);
        }
#endif
#if defined(SOS_TARGET_ARM64)
        if (IsDbgTargetArm64())
        {
            foundPlatform = true;
            DWORD64* X = context.Arm64Context.X;
            // Formatting is three columns or registers with registers values aligned (right justified)
            ExtOut("   ");
            for (int i = 0; i < 29; ++i)
            {
                if (i <10) ExtOut(" ");
                ExtOut(" x%d=%016llx", i, X[i]);
                if ((i % 3) == 2) ExtOut("\n   ");
            }
            ExtOut("  fp=%016llx\n", context.Arm64Context.Fp);
            ExtOut("     lr=%016llx  sp=%016llx  pc=%016llx\n", context.Arm64Context.Lr, context.Arm64Context.Sp, context.Arm64Context.Pc);
            ExtOut("           cpsr=%08x        fpcr=%08x        fpsr=%08x\n", context.Arm64Context.Cpsr, context.Arm64Context.Fpcr, context.Arm64Context.Fpsr);
        }
#endif
#if defined(SOS_TARGET_RISCV64)
        if (IsDbgTargetRiscV64())
        {
            foundPlatform = true;
            String outputFormat3 = "    %3s=%016llx %3s=%016llx %3s=%016llx\n";
            ExtOut(outputFormat3, "r0", context.RiscV64Context.R0, "ra", context.RiscV64Context.Ra, "sp", context.RiscV64Context.Sp);
            ExtOut(outputFormat3, "gp", context.RiscV64Context.Gp, "tp", context.RiscV64Context.Tp, "t0", context.RiscV64Context.T0);
            ExtOut(outputFormat3, "t1", context.RiscV64Context.T1, "t2", context.RiscV64Context.T2, "fp", context.RiscV64Context.Fp);
            ExtOut(outputFormat3, "s1", context.RiscV64Context.S1, "a0", context.RiscV64Context.A0, "a1", context.RiscV64Context.A1);
            ExtOut(outputFormat3, "a2", context.RiscV64Context.A2, "a3", context.RiscV64Context.A3, "a4", context.RiscV64Context.A4);
            ExtOut(outputFormat3, "a5", context.RiscV64Context.A5, "a6", context.RiscV64Context.A6, "a7", context.RiscV64Context.A7);
            ExtOut(outputFormat3, "s2", context.RiscV64Context.S2, "s3", context.RiscV64Context.S3, "s4", context.RiscV64Context.S4);
            ExtOut(outputFormat3, "s5", context.RiscV64Context.S5, "s6", context.RiscV64Context.S6, "s7", context.RiscV64Context.S7);
            ExtOut(outputFormat3, "s8", context.RiscV64Context.S8, "s9", context.RiscV64Context.S9, "s10", context.RiscV64Context.S10);
            ExtOut(outputFormat3, "s11", context.RiscV64Context.S11, "t3", context.RiscV64Context.T3, "t4", context.RiscV64Context.T4);
            ExtOut(outputFormat3, "t5", context.RiscV64Context.T5, "t6", context.RiscV64Context.T6, "pc", context.RiscV64Context.Pc);
        }
#endif
#if defined(SOS_TARGET_LOONGARCH64)
        if (IsDbgTargetLoongArch64())
        {
            foundPlatform = true;
            String outputFormat3 = "    %3s=%016llx %3s=%016llx %3s=%016llx\n";
            ExtOut(outputFormat3, "r0", context.LoongArch64Context.R0, "ra", context.LoongArch64Context.Ra, "tp", context.LoongArch64Context.Tp);
            ExtOut(outputFormat3, "sp", context.LoongArch64Context.Sp, "a0", context.LoongArch64Context.A0, "a1", context.LoongArch64Context.A1);
            ExtOut(outputFormat3, "a2", context.LoongArch64Context.A2, "a3", context.LoongArch64Context.A3, "a4", context.LoongArch64Context.A4);
            ExtOut(outputFormat3, "a5", context.LoongArch64Context.A5, "a6", context.LoongArch64Context.A6, "a7", context.LoongArch64Context.A7);
            ExtOut(outputFormat3, "t0", context.LoongArch64Context.T0, "t1", context.LoongArch64Context.T1, "t2", context.LoongArch64Context.T2);
            ExtOut(outputFormat3, "t3", context.LoongArch64Context.T3, "t4", context.LoongArch64Context.T4, "t5", context.LoongArch64Context.T5);
            ExtOut(outputFormat3, "t6", context.LoongArch64Context.T6, "t7", context.LoongArch64Context.T7, "t8", context.LoongArch64Context.T8);
            ExtOut(outputFormat3, "x0", context.LoongArch64Context.X0, "fp", context.LoongArch64Context.Fp, "s0", context.LoongArch64Context.S0);
            ExtOut(outputFormat3, "s1", context.LoongArch64Context.S1, "s2", context.LoongArch64Context.S2, "s3", context.LoongArch64Context.S3);
            ExtOut(outputFormat3, "s4", context.LoongArch64Context.S4, "s5", context.LoongArch64Context.S5, "s6", context.LoongArch64Context.S6);
            ExtOut(outputFormat3, "s7", context.LoongArch64Context.S7, "s8", context.LoongArch64Context.S8, "pc", context.LoongArch64Context.Pc);
        }
#endif

        if (!foundPlatform)
        {
            ExtOut("Can't display register values for this platform\n");
        }
        return S_OK;

    }

    static HRESULT GetFrameLocation(IXCLRDataStackWalk *pStackWalk, CLRDATA_ADDRESS *ip, CLRDATA_ADDRESS *sp)
    {
        CROSS_PLATFORM_CONTEXT context;
        HRESULT hr = pStackWalk->GetContext(g_targetMachine->GetFullContextFlags(), g_targetMachine->GetContextSize(), NULL, (BYTE *)&context);
        if (FAILED(hr))
        {
            ExtOut("GetFrameContext failed: %lx\n", hr);
            return hr;
        }
        if (hr == S_FALSE)
        {
            // GetFrameContext returns S_FALSE if the frame iterator is invalid.  That's basically an error for us.
            return E_FAIL;
        }
        // First find the info for the Frame object, if the current frame has an associated clr!Frame.
        *ip = GetIP(context);
        *sp = GetSP(context);

        if (IsDbgTargetArm())
            *ip = *ip & ~THUMB_CODE;

        return S_OK;
    }

    static void PrintNativeStackFrame(TableOutput out, PDEBUG_STACK_FRAME frame, BOOL bSuppressLines)
    {
        char filename[MAX_LONGPATH + 1];
        char symbol[1024];
        ULONG64 displacement;

        ULONG64 ip = frame->InstructionOffset;

        out.WriteColumn(0, frame->StackOffset);
        out.WriteColumn(1, NativePtr(ip));

        HRESULT hr = g_ExtSymbols->GetNameByOffset(TO_CDADDR(ip), symbol, ARRAY_SIZE(symbol), NULL, &displacement);
        if (SUCCEEDED(hr) && symbol[0] != '\0')
        {
            String frameOutput;
            frameOutput += symbol;

            if (displacement)
            {
                frameOutput += " + ";
                frameOutput += Decimal(displacement);
            }

            if (!bSuppressLines)
            {
                ULONG line;
                hr = g_ExtSymbols->GetLineByOffset(TO_CDADDR(ip), &line, filename, ARRAY_SIZE(filename), NULL, NULL);
                if (SUCCEEDED(hr))
                {
                    frameOutput += " at ";
                    frameOutput += filename;
                    frameOutput += ":";
                    frameOutput += Decimal(line);
                }
            }

            out.WriteColumn(2, frameOutput);
        }
        else
        {
            out.WriteColumn(2, "");
        }
    }

    static void PrintCurrentThread(BOOL bParams, BOOL bLocals, BOOL bSuppressLines, BOOL bGC, BOOL bNative, BOOL bDisplayRegVals)
    {
        ULONG id = 0;
        ULONG osid = 0;

        g_ExtSystem->GetCurrentThreadSystemId(&osid);
        ExtOut("OS Thread Id: 0x%x ", osid);
        g_ExtSystem->GetCurrentThreadId(&id);
        ExtOut("(%d)\n", id);

        PrintThread(osid, bParams, bLocals, bSuppressLines, bGC, bNative, bDisplayRegVals);
    }

    static void PrintAllThreads(BOOL bParams, BOOL bLocals, BOOL bSuppressLines, BOOL bGC, BOOL bNative, BOOL bDisplayRegVals)
    {
        HRESULT Status;

        DacpThreadStoreData ThreadStore;
        if ((Status = ThreadStore.Request(g_sos)) != S_OK)
        {
            ExtErr("Failed to request ThreadStore\n");
            return;
        }

        DacpThreadData Thread;
        CLRDATA_ADDRESS CurThread = ThreadStore.firstThread;
        while (CurThread != 0)
        {
            if (IsInterrupt())
                break;

            if ((Status = Thread.Request(g_sos, CurThread)) != S_OK)
            {
                ExtErr("Failed to request thread at %p\n", SOS_PTR(CurThread));
                return;
            }
            if (Thread.osThreadId != 0)
            {
                ExtOut("OS Thread Id: 0x%x\n", Thread.osThreadId);
                PrintThread(Thread.osThreadId, bParams, bLocals, bSuppressLines, bGC, bNative, bDisplayRegVals);
            }
            CurThread = Thread.nextThread;
        }
    }

private:
    static HRESULT CreateStackWalk(ULONG osID, IXCLRDataStackWalk **ppStackwalk)
    {
        HRESULT hr = S_OK;
        ToRelease<IXCLRDataTask> pTask;

        if ((hr = g_clrData->GetTaskByOSThreadID(osID, &pTask)) != S_OK)
        {
            ExtOut("Unable to walk the managed stack. The current thread is likely not a \n");
            ExtOut("managed thread. You can run %sclrthreads to get a list of managed threads in\n", SOSPrefix);
            ExtOut("the process\n");
            return hr;
        }

        return pTask->CreateStackWalk(CLRDATA_SIMPFRAME_UNRECOGNIZED |
                                      CLRDATA_SIMPFRAME_MANAGED_METHOD |
                                      CLRDATA_SIMPFRAME_RUNTIME_MANAGED_CODE |
                                      CLRDATA_SIMPFRAME_RUNTIME_UNMANAGED_CODE,
                                      ppStackwalk);
    }

    /* Prints the args and locals of for a thread's stack.
     * Params:
     *      pStackWalk - the stack we are printing
     *      bArgs - whether to print args
     *      bLocals - whether to print locals
     */
    static void PrintArgsAndLocals(IXCLRDataStackWalk *pStackWalk, BOOL bArgs, BOOL bLocals)
    {
        ToRelease<IXCLRDataFrame> pFrame;
        ULONG32 argCount = 0;
        ULONG32 localCount = 0;
        HRESULT hr = S_OK;

        hr = pStackWalk->GetFrame(&pFrame);

        // Print arguments
        if (SUCCEEDED(hr) && bArgs)
            hr = pFrame->GetNumArguments(&argCount);

        if (SUCCEEDED(hr) && bArgs)
            hr = ShowArgs(argCount, pFrame);

        // Print locals
        if (SUCCEEDED(hr) && bLocals)
            hr = pFrame->GetNumLocalVariables(&localCount);

        if (SUCCEEDED(hr) && bLocals)
            ShowLocals(localCount, pFrame);

        ExtOut("\n");
    }



    /* Displays the arguments to a function
     * Params:
     *      argy - the number of arguments the function has
     *      pFramey - the frame we are inspecting
     */
    static HRESULT ShowArgs(ULONG32 argy, IXCLRDataFrame *pFramey)
    {
        CLRDATA_ADDRESS addr = 0;
        BOOL fPrintedLocation = FALSE;
        ULONG64 outVar = 0;
        ULONG32 tmp;
        HRESULT hr = S_OK;

        ArrayHolder<WCHAR> argName = new NOTHROW WCHAR[mdNameLen];
        if (!argName)
        {
            ReportOOM();
            return E_FAIL;
        }

        for (ULONG32 i=0; i < argy; i++)
        {
            if (i == 0)
            {
                ExtOut("    PARAMETERS:\n");
            }

            ToRelease<IXCLRDataValue> pVal;
            hr = pFramey->GetArgumentByIndex(i,
                                   &pVal,
                                   mdNameLen,
                                   &tmp,
                                   argName);

            if (FAILED(hr))
                return hr;

            ExtOut("        ");

            if (argName[0] != L'\0')
            {
                ExtOut("%S ", argName.GetPtr());
            }

            // At times we cannot print the value of a parameter (most
            // common case being a non-primitive value type).  In these
            // cases we need to print the location of the parameter,
            // so that we can later examine it (e.g. using !dumpvc)
            {
                bool result = SUCCEEDED(pVal->GetNumLocations(&tmp)) && tmp == 1;
                if (result)
                    result = SUCCEEDED(pVal->GetLocationByIndex(0, &tmp, &addr));

                if (result)
                {
                    if (tmp == CLRDATA_VLOC_REGISTER)
                    {
                        ExtOut("(<CLR reg>) ");
                    }
                    else
                    {
                        ExtOut("(0x%p) ", SOS_PTR(CDA_TO_UL64(addr)));
                    }
                    fPrintedLocation = TRUE;
                }
            }

            if (argName[0] != L'\0' || fPrintedLocation)
            {
                ExtOut("= ");
            }

            if (HRESULT_CODE(pVal->GetBytes(0,&tmp,NULL)) == ERROR_BUFFER_OVERFLOW)
            {
                ArrayHolder<BYTE> pByte = new NOTHROW BYTE[tmp + 1];
                if (pByte == NULL)
                {
                    ReportOOM();
                    return E_FAIL;
                }

                hr = pVal->GetBytes(tmp, &tmp, pByte);

                if (FAILED(hr))
                {
                    ExtOut("<unable to retrieve data>\n");
                }
                else
                {
                    switch(tmp)
                    {
                        case 1: outVar = *((BYTE *)pByte.GetPtr()); break;
                        case 2: outVar = *((short *)pByte.GetPtr()); break;
                        case 4: outVar = *((DWORD *)pByte.GetPtr()); break;
                        case 8: outVar = *((ULONG64 *)pByte.GetPtr()); break;
                        default: outVar = 0;
                    }

                    if (outVar)
                        DMLOut("0x%s\n", DMLObject(outVar));
                    else
                        ExtOut("0x%p\n", SOS_PTR(outVar));
                }

            }
            else
            {
                ExtOut("<no data>\n");
            }
        }

        return S_OK;
    }


    /* Prints the locals of a frame.
     * Params:
     *      localy - the number of locals in the frame
     *      pFramey - the frame we are inspecting
     */
    static HRESULT ShowLocals(ULONG32 localy, IXCLRDataFrame *pFramey)
    {
        for (ULONG32 i=0; i < localy; i++)
        {
            if (i == 0)
                ExtOut("    LOCALS:\n");

            HRESULT hr;
            ExtOut("        ");

            // local names don't work in Whidbey.
            ToRelease<IXCLRDataValue> pVal;
            hr = pFramey->GetLocalVariableByIndex(i, &pVal, mdNameLen, NULL, g_mdName);
            if (FAILED(hr))
            {
                return hr;
            }

            ULONG32 numLocations;
            if (SUCCEEDED(pVal->GetNumLocations(&numLocations)) &&
                numLocations == 1)
            {
                ULONG32 flags;
                CLRDATA_ADDRESS addr;
                if (SUCCEEDED(pVal->GetLocationByIndex(0, &flags, &addr)))
                {
                    if (flags == CLRDATA_VLOC_REGISTER)
                    {
                        ExtOut("<CLR reg> ");
                    }
                    else
                    {
                        ExtOut("0x%p ", SOS_PTR(CDA_TO_UL64(addr)));
                    }
                }

                // Can I get a name for the item?

                ExtOut("= ");
            }
            ULONG32 dwSize = 0;
            hr = pVal->GetBytes(0, &dwSize, NULL);

            if (HRESULT_CODE(hr) == ERROR_BUFFER_OVERFLOW)
            {
                ArrayHolder<BYTE> pByte = new NOTHROW BYTE[dwSize + 1];
                if (pByte == NULL)
                {
                    ReportOOM();
                    return E_FAIL;
                }

                hr = pVal->GetBytes(dwSize,&dwSize,pByte);

                if (FAILED(hr))
                {
                    ExtOut("<unable to retrieve data>\n");
                }
                else
                {
                    ULONG64 outVar = 0;
                    switch(dwSize)
                    {
                        case 1: outVar = *((BYTE *) pByte.GetPtr()); break;
                        case 2: outVar = *((short *) pByte.GetPtr()); break;
                        case 4: outVar = *((DWORD *) pByte.GetPtr()); break;
                        case 8: outVar = *((ULONG64 *) pByte.GetPtr()); break;
                        default: outVar = 0;
                    }

                    if (outVar)
                        DMLOut("0x%s\n", DMLObject(outVar));
                    else
                        ExtOut("0x%p\n", SOS_PTR(outVar));
                }
            }
            else
            {
                ExtOut("<no data>\n");
            }
        }

        return S_OK;
    }

};

#ifndef FEATURE_PAL

WatchCmd g_watchCmd;

// The grand new !Watch command, private to Apollo for now
DECLARE_API(Watch)
{
    INIT_API_NOEE();
    StringHolder addExpression;
    StringHolder aExpression;
    StringHolder saveName;
    StringHolder sName;
    StringHolder expression;
    StringHolder filterName;
    StringHolder renameOldName;
    size_t expandIndex = -1;
    size_t removeIndex = -1;
    BOOL clear = FALSE;

    size_t nArg = 0;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-add", &addExpression.data, COSTRING, TRUE},
        {"-a", &aExpression.data, COSTRING, TRUE},
        {"-save", &saveName.data, COSTRING, TRUE},
        {"-s", &sName.data, COSTRING, TRUE},
        {"-clear", &clear, COBOOL, FALSE},
        {"-c", &clear, COBOOL, FALSE},
        {"-expand", &expandIndex, COSIZE_T, TRUE},
        {"-filter", &filterName.data, COSTRING, TRUE},
        {"-r", &removeIndex, COSIZE_T, TRUE},
        {"-remove", &removeIndex, COSIZE_T, TRUE},
        {"-rename", &renameOldName.data, COSTRING, TRUE},
    };

    CMDValue arg[] =
    {   // vptr, type
        {&expression.data, COSTRING}
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if(addExpression.data != NULL || aExpression.data != NULL)
    {
        WCHAR pAddExpression[MAX_EXPRESSION];
        swprintf_s(pAddExpression, MAX_EXPRESSION, W("%S"), addExpression.data != NULL ? addExpression.data : aExpression.data);
        Status = g_watchCmd.Add(pAddExpression);
    }
    else if(removeIndex != -1)
    {
        if(removeIndex <= 0)
        {
            ExtOut("Index must be a postive decimal number\n");
        }
        else
        {
            Status = g_watchCmd.Remove((int)removeIndex);
            if(Status == S_OK)
                ExtOut("Watch expression #%d has been removed\n", removeIndex);
            else if(Status == S_FALSE)
                ExtOut("There is no watch expression with index %d\n", removeIndex);
            else
                ExtOut("Unknown failure 0x%x removing watch expression\n", Status);
        }
    }
    else if(saveName.data != NULL || sName.data != NULL)
    {
        WCHAR pSaveName[MAX_EXPRESSION];
        swprintf_s(pSaveName, MAX_EXPRESSION, W("%S"), saveName.data != NULL ? saveName.data : sName.data);
        Status = g_watchCmd.SaveList(pSaveName);
    }
    else if(clear)
    {
        g_watchCmd.Clear();
    }
    else if(renameOldName.data != NULL)
    {
        if(nArg != 1)
        {
             ExtOut("Must provide an old and new name. Usage: !watch -rename <old_name> <new_name>.\n");
             return S_FALSE;
        }
        WCHAR pOldName[MAX_EXPRESSION];
        swprintf_s(pOldName, MAX_EXPRESSION, W("%S"), renameOldName.data);
        WCHAR pNewName[MAX_EXPRESSION];
        swprintf_s(pNewName, MAX_EXPRESSION, W("%S"), expression.data);
        g_watchCmd.RenameList(pOldName, pNewName);
    }
    // print the tree, possibly with filtering and/or expansion
    else if(expandIndex != -1 || expression.data == NULL)
    {
        WCHAR pExpression[MAX_EXPRESSION];
        pExpression[0] = '\0';

        if(expandIndex != -1)
        {
            if(expression.data != NULL)
            {
                swprintf_s(pExpression, MAX_EXPRESSION, W("%S"), expression.data);
            }
            else
            {
                ExtOut("No expression was provided. Usage !watch -expand <index> <expression>\n");
                return S_FALSE;
            }
        }
        WCHAR pFilterName[MAX_EXPRESSION];
        pFilterName[0] = '\0';

        if(filterName.data != NULL)
        {
            swprintf_s(pFilterName, MAX_EXPRESSION, W("%S"), filterName.data);
        }

        g_watchCmd.Print((int)expandIndex, pExpression, pFilterName);
    }
    else
    {
        ExtOut("Unrecognized argument: %s\n", expression.data);
    }

    return Status;
}

#endif // FEATURE_PAL

DECLARE_API(ClrStack)
{
    INIT_API_PROBE_MANAGED("clrstack");

    BOOL bAll = FALSE;
    BOOL bParams = FALSE;
    BOOL bLocals = FALSE;
    BOOL bSuppressLines = FALSE;
    BOOL bICorDebug = FALSE;
    BOOL bGC = FALSE;
    BOOL dml = FALSE;
    BOOL bFull = FALSE;
    BOOL bDisplayRegVals = FALSE;
    BOOL bAllThreads = FALSE;
    DWORD frameToDumpVariablesFor = -1;
    StringHolder cvariableName;
    ArrayHolder<WCHAR> wvariableName = new NOTHROW WCHAR[mdNameLen];
    if (wvariableName == NULL)
    {
        ReportOOM();
        return E_OUTOFMEMORY;
    }

    memset(wvariableName, 0, sizeof(wvariableName));

    size_t nArg = 0;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-a", &bAll, COBOOL, FALSE},
        {"-all", &bAllThreads, COBOOL, FALSE},
        {"-p", &bParams, COBOOL, FALSE},
        {"-l", &bLocals, COBOOL, FALSE},
        {"-n", &bSuppressLines, COBOOL, FALSE},
        {"-i", &bICorDebug, COBOOL, FALSE},
        {"-gc", &bGC, COBOOL, FALSE},
        {"-f", &bFull, COBOOL, FALSE},
        {"-r", &bDisplayRegVals, COBOOL, FALSE },
        {"/d", &dml, COBOOL, FALSE},
    };
    CMDValue arg[] =
    {   // vptr, type
        {&cvariableName.data, COSTRING},
        {&frameToDumpVariablesFor, COSIZE_T},
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    EnableDMLHolder dmlHolder(dml);
    if (bAll || bParams || bLocals)
    {
        // No parameter or local supports for minidump case!
        MINIDUMP_NOT_SUPPORTED();
    }

    if (bAll)
    {
        bParams = bLocals = TRUE;
    }

    if (bICorDebug)
    {
        if(nArg > 0)
        {
            bool firstParamIsNumber = true;
            for(DWORD i = 0; i < strlen(cvariableName.data); i++)
                firstParamIsNumber = firstParamIsNumber && isdigit(cvariableName.data[i]);

            if(firstParamIsNumber && nArg == 1)
            {
                frameToDumpVariablesFor = (DWORD)GetExpression(cvariableName.data);
                cvariableName.data[0] = '\0';
            }
        }
        if(cvariableName.data != NULL && strlen(cvariableName.data) > 0)
            swprintf_s(wvariableName, mdNameLen, W("%S\0"), cvariableName.data);

        if(_wcslen(wvariableName) > 0)
            bParams = bLocals = TRUE;

        EnableDMLHolder dmlHolder(TRUE);
        return ClrStackImplWithICorDebug::ClrStackFromPublicInterface(bParams, bLocals, FALSE, wvariableName, frameToDumpVariablesFor);
    }

    if (bAllThreads) {
        ClrStackImpl::PrintAllThreads(bParams, bLocals, bSuppressLines, bGC, bFull, bDisplayRegVals);
    }
    else {
        ClrStackImpl::PrintCurrentThread(bParams, bLocals, bSuppressLines, bGC, bFull, bDisplayRegVals);
    }

    return S_OK;
}

#ifndef FEATURE_PAL

BOOL IsMemoryInfoAvailable()
{
    ULONG Class;
    ULONG Qualifier;
    g_ExtControl->GetDebuggeeType(&Class,&Qualifier);
    if (Qualifier == DEBUG_DUMP_SMALL)
    {
        g_ExtControl->GetDumpFormatFlags(&Qualifier);
        if ((Qualifier & DEBUG_FORMAT_USER_SMALL_FULL_MEMORY) == 0)
        {
            if ((Qualifier & DEBUG_FORMAT_USER_SMALL_FULL_MEMORY_INFO) == 0)
            {
                return FALSE;
            }
        }
    }
    return TRUE;
}

DECLARE_API(VMMap)
{
    INIT_API();

    if (IsMiniDumpFile() || !IsMemoryInfoAvailable())
    {
        ExtOut("!VMMap requires a full memory dump (.dump /ma) or a live process.\n");
    }
    else
    {
        vmmap();
    }

    return Status;
}

#endif // FEATURE_PAL

DECLARE_API(SOSFlush)
{
    INIT_API_NOEE_PROBE_MANAGED("sosflush");

    ITarget* target = GetTarget();
    if (target != nullptr)
    {
        target->Flush();
    }
    ExtOut("Internal cached state reset\n");
    return S_OK;
}

#ifndef FEATURE_PAL

DECLARE_API( VMStat )
{
    INIT_API();

    if (IsMiniDumpFile() || !IsMemoryInfoAvailable())
    {
        ExtOut("!VMStat requires a full memory dump (.dump /ma) or a live process.\n");
    }
    else
    {
        vmstat();
    }

    return Status;
}   // DECLARE_API( vmmap )

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function saves a dll to a file.                              *
*                                                                      *
\**********************************************************************/
HRESULT SaveModuleToFile(CLRDATA_ADDRESS moduleAddr, LPSTR file)
    {
    DWORD_PTR dllBase = 0;
    ULONG64 base;
    if (g_ExtSymbols->GetModuleByOffset(TO_CDADDR(moduleAddr), 0, NULL, &base) == S_OK)
    {
        dllBase = TO_TADDR(base);
    }
    else if (IsModule((DWORD_PTR)moduleAddr))
    {
        DacpModuleData module;
        module.Request(g_sos, TO_CDADDR(moduleAddr));
        dllBase = TO_TADDR(module.ilBase);
        if (dllBase == 0)
        {
            ExtOut("Module does not have base address\n");
            return E_INVALIDARG;
        }
    }
    else
    {
        ExtOut("%p is not a Module or base address\n", SOS_PTR(moduleAddr));
        return E_INVALIDARG;
    }

    MEMORY_BASIC_INFORMATION64 mbi;
    if (FAILED(g_ExtData2->QueryVirtual(TO_CDADDR(dllBase), &mbi)))
    {
        ExtOut("Failed to retrieve information about segment %p", SOS_PTR(dllBase));
        return E_FAIL;
    }

    // module loaded as an image or mapped as a flat file?
    BOOL bIsImage = (mbi.Type == MEM_IMAGE);

    IMAGE_DOS_HEADER DosHeader;
    if (g_ExtData->ReadVirtual(TO_CDADDR(dllBase), &DosHeader, sizeof(DosHeader), NULL) != S_OK)
        return S_FALSE;

    IMAGE_NT_HEADERS Header;
    if (g_ExtData->ReadVirtual(TO_CDADDR(dllBase + DosHeader.e_lfanew), &Header, sizeof(Header), NULL) != S_OK)
        return S_FALSE;

    DWORD_PTR sectionAddr = dllBase + DosHeader.e_lfanew + offsetof(IMAGE_NT_HEADERS, OptionalHeader)
            + Header.FileHeader.SizeOfOptionalHeader;

    IMAGE_SECTION_HEADER section;
    struct MemLocation
    {
        DWORD_PTR VAAddr;
        DWORD_PTR VASize;
        DWORD_PTR FileAddr;
        DWORD_PTR FileSize;
    };

    int nSection = Header.FileHeader.NumberOfSections;
    ExtOut("%u sections in file\n", nSection);
    MemLocation* memLoc = (MemLocation*)_alloca(nSection * sizeof(MemLocation));
    int indxSec = -1;
    int slot;
    for (int n = 0; n < nSection; n++)
    {
        if (g_ExtData->ReadVirtual(TO_CDADDR(sectionAddr), &section, sizeof(section), NULL) == S_OK)
        {
            for (slot = 0; slot <= indxSec; slot++)
                if (section.PointerToRawData < memLoc[slot].FileAddr)
                    break;

            for (int k = indxSec; k >= slot; k--)
                memcpy(&memLoc[k + 1], &memLoc[k], sizeof(MemLocation));

            memLoc[slot].VAAddr = section.VirtualAddress;
            memLoc[slot].VASize = section.Misc.VirtualSize;
            memLoc[slot].FileAddr = section.PointerToRawData;
            memLoc[slot].FileSize = section.SizeOfRawData;
            ExtOut("section %d - VA=%x, VASize=%x, FileAddr=%x, FileSize=%x\n",
                n, memLoc[slot].VAAddr, memLoc[slot].VASize, memLoc[slot].FileAddr,
                memLoc[slot].FileSize);
            indxSec++;
        }
        else
        {
            ExtOut("Fail to read PE section info\n");
            return E_FAIL;
        }
        sectionAddr += sizeof(section);
    }

    char* ptr = file;

    if (ptr[0] == '\0')
    {
        ExtOut("File not specified\n");
        return E_INVALIDARG;
    }

    ptr += strlen(ptr) - 1;
    while (isspace(*ptr))
    {
        *ptr = '\0';
        ptr--;
    }

    HANDLE hFile = CreateFileA(file, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, 0, NULL);
    if (hFile == INVALID_HANDLE_VALUE)
    {
        ExtOut("Fail to create file %s\n", file);
        return E_FAIL;
    }

    ULONG pageSize = OSPageSize();
    char* buffer = (char*)_alloca(pageSize);
    DWORD nRead;
    DWORD nWrite;

    // NT PE Headers
    TADDR dwAddr = dllBase;
    TADDR dwEnd = dllBase + Header.OptionalHeader.SizeOfHeaders;
    while (dwAddr < dwEnd)
    {
        nRead = pageSize;
        if (dwEnd - dwAddr < nRead)
            nRead = (ULONG)(dwEnd - dwAddr);

        if (g_ExtData->ReadVirtual(TO_CDADDR(dwAddr), buffer, nRead, &nRead) == S_OK)
        {
            WriteFile(hFile, buffer, nRead, &nWrite, NULL);
        }
        else
        {
            ExtOut("Fail to read memory\n");
            goto end;
        }
        dwAddr += nRead;
    }

    for (slot = 0; slot <= indxSec; slot++)
    {
        dwAddr = dllBase + (bIsImage ? memLoc[slot].VAAddr : memLoc[slot].FileAddr);
        dwEnd = memLoc[slot].FileSize + dwAddr - 1;

        while (dwAddr <= dwEnd)
        {
            nRead = pageSize;
            if (dwEnd - dwAddr + 1 < pageSize)
                nRead = (ULONG)(dwEnd - dwAddr + 1);

            if (g_ExtData->ReadVirtual(TO_CDADDR(dwAddr), buffer, nRead, &nRead) == S_OK)
            {
                WriteFile(hFile, buffer, nRead, &nWrite, NULL);
            }
            else
            {
                ExtOut("Fail to read memory\n");
                goto end;
            }
            dwAddr += pageSize;
        }
    }
end:
    CloseHandle(hFile);
    return S_OK;
}

HRESULT SaveModulesFromDomain(CLRDATA_ADDRESS domain, LPSTR destinationFolder)
{
    HRESULT Status;

    DacpAppDomainData appDomain;
    if ((Status = appDomain.Request(g_sos, domain)) != S_OK)
    {
        ExtOut("Failed to get appdomain %p, error %lx\n", SOS_PTR(domain), Status);
        return Status;
    }

    ExtOut("--------------------------------------\n");
    DMLOut("Domain %d:%s          %s\n", appDomain.dwId, (appDomain.dwId >= 10) ? "" : " ", DMLDomain(domain));

    if (appDomain.AssemblyCount == 0)
    {
        return Status;
    }

    ArrayHolder<CLRDATA_ADDRESS> assemblies = new CLRDATA_ADDRESS[appDomain.AssemblyCount];
    if (assemblies == NULL)
    {
        ReportOOM();
        return Status;
    }

    if (g_sos->GetAssemblyList(appDomain.AppDomainPtr, appDomain.AssemblyCount, assemblies, NULL) != S_OK)
    {
        ExtOut("Unable to get array of Assemblies\n");
        return Status;
    }

    for (int i = 0; i < appDomain.AssemblyCount; i++)
    {
        DacpAssemblyData assembly;
        if (assembly.Request(g_sos, assemblies[i], appDomain.AppDomainPtr) == S_OK)
        {
            if (assembly.isDynamic)
            {
                continue;
            }

            ArrayHolder<CLRDATA_ADDRESS> modules = new CLRDATA_ADDRESS[assembly.ModuleCount];
            if (modules == NULL
                || g_sos->GetAssemblyModuleList(assembly.AssemblyPtr, assembly.ModuleCount, modules, NULL) != S_OK)
            {
                ReportOOM();
                return Status;
            }

            for (UINT j = 0; j < assembly.ModuleCount; j++)
            {
                DacpModuleData module;
                if (module.Request(g_sos, modules[j]) == S_OK)
                {
                    ExtOut("Module %s\n", DMLModule(module.Address));

                    DacpModuleData moduleData;
                    if (moduleData.Request(g_sos, module.Address) == S_OK)
                    {
                        WCHAR fullFileName[MAX_LONGPATH];
                        FileNameForModule(&moduleData, fullFileName);

                        if (fullFileName[0])
                        {
                            std::wstring fullFileNameStr(fullFileName);
                            std::wstring fileName;

                            size_t pos = fullFileNameStr.find_last_of(L"\\/");
                            if (pos == std::wstring::npos)
                            {
                                fileName = fullFileNameStr; // No directory separator found, return the whole path
                            }
                            else
                            {
                                fileName = fullFileNameStr.substr(pos + 1);
                            }

                            WCHAR destinationFolderW[MAX_LONGPATH];

                            // Build the full path
                            MultiByteToWideChar(CP_ACP, 0, destinationFolder, -1, destinationFolderW, MAX_LONGPATH);

                            WCHAR destinationPath[MAX_LONGPATH];
                            swprintf_s(destinationPath, MAX_LONGPATH, W("%s\\%s"), destinationFolderW, fileName.c_str());

                            CHAR finalPath[MAX_LONGPATH];
                            WideCharToMultiByte(CP_ACP, 0, destinationPath, -1, finalPath, MAX_LONGPATH, NULL, NULL);

                            if (SaveModuleToFile(modules[j], finalPath) == S_OK)
                            {
                                ExtOut("Saved module to %s\n", finalPath);
                            }
                        }
                        else
                        {
                            ExtOut("Skipping module (%s)\n", (moduleData.bIsReflection) ? W("Dynamic Module") : W("Unknown Module"));
                        }
                    }
                }
            }
        }
    }

    return Status;
}

DECLARE_API(SaveModule)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    StringHolder Location;
    DWORD_PTR moduleAddr = NULL;

    CMDValue arg[] =
    {   // vptr, type
        {&moduleAddr, COHEX},
        {&Location.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }
    if (nArg != 2)
    {
        ExtOut("Usage: SaveModule <address> <file to save>\n");
        return E_INVALIDARG;
    }
    if (moduleAddr == 0) {
        ExtOut("Invalid arg\n");
        return E_INVALIDARG;
    }

    return SaveModuleToFile(moduleAddr, Location.data);
}


DECLARE_API(SaveAllModules)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    StringHolder Location;

    CMDValue arg[] =
    {   // vptr, type
        {&Location.data, COSTRING}
    };

    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (nArg != 1)
    {
        ExtOut("Usage: SaveAllModules <destination folder>\n");
        return E_INVALIDARG;
    }

    ExtOut("Saving all modules to %s\n", Location.data);

    DacpAppDomainStoreData adsData;
    if ((Status = adsData.Request(g_sos)) != S_OK)
    {
        ExtOut("Unable to get AppDomain information\n");
        return Status;
    }

    ArrayHolder<CLRDATA_ADDRESS> pArray = new NOTHROW CLRDATA_ADDRESS[adsData.DomainCount];
    if (pArray == NULL)
    {
        ReportOOM();
        return Status;
    }

    if ((Status = g_sos->GetAppDomainList(adsData.DomainCount, pArray, NULL)) != S_OK)
    {
        ExtOut("Unable to get array of AppDomains\n");
        return Status;
    }

    if (adsData.systemDomain != (TADDR)0)
    {
        Status = SaveModulesFromDomain(adsData.systemDomain, Location.data);

        if (Status != S_OK)
        {
            return Status;
        }
    }

    if (adsData.sharedDomain != (TADDR)0)
    {
        Status = SaveModulesFromDomain(adsData.sharedDomain, Location.data);

        if (Status != S_OK)
        {
            return Status;
        }
    }

    for (int n = 0; n < adsData.DomainCount; n++)
    {
        if (IsInterrupt())
            break;

        Status = SaveModulesFromDomain(pArray[n], Location.data);

        if (Status != S_OK)
        {
            break;
        }
    }

    return Status;
}

#endif // FEATURE_PAL

DECLARE_API(dbgout)
{
    INIT_API_EXT();

    BOOL bOff = FALSE;

    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-off", &bOff, COBOOL, FALSE},
    };

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }

    Output::SetDebugOutputEnabled(!bOff);
    ExtOut("Debug output logging %s\n", Output::IsDebugOutputEnabled() ? "enabled" : "disabled");
    return Status;
}

static HRESULT DumpMDInfoBuffer(DWORD_PTR dwStartAddr, DWORD Flags, ULONG64 Esp,
        ULONG64 IPAddr, StringOutput& so)
{
#define DOAPPEND(str)         \
    do { \
    if (!so.Append((str))) {  \
    return E_OUTOFMEMORY; \
    }} while (0)

    // Should we skip explicit frames?  They are characterized by Esp = 0, && Eip = 0 or 1.
    // See comment in FormatGeneratedException() for explanation why on non_IA64 Eip is 1, and not 0
    if (!(Flags & SOS_STACKTRACE_SHOWEXPLICITFRAMES) && (Esp == 0) && (IPAddr == 1))
    {
        return S_FALSE;
    }

    DacpMethodDescData MethodDescData;
    if (MethodDescData.Request(g_sos, TO_CDADDR(dwStartAddr)) != S_OK)
    {
        return E_FAIL;
    }

    ArrayHolder<WCHAR> wszNameBuffer = new WCHAR[MAX_LONGPATH+1];

    if (Flags & SOS_STACKTRACE_SHOWADDRESSES)
    {
        _snwprintf_s(wszNameBuffer, MAX_LONGPATH, MAX_LONGPATH, W("%p %p "), SOS_PTR(Esp), SOS_PTR(IPAddr)); // _TRUNCATE
        DOAPPEND(wszNameBuffer);
    }

    DacpModuleData dmd;
    BOOL bModuleNameWorked = FALSE;
    ULONG64 addrInModule = IPAddr;
    if (dmd.Request(g_sos, MethodDescData.ModulePtr) == S_OK)
    {
        CLRDATA_ADDRESS base = 0;
        if (g_sos->GetPEFileBase(dmd.PEAssembly, &base) == S_OK)
        {
            if (base)
            {
                addrInModule = base;
            }
        }
    }
    ULONG index;
    ULONG64 base;
    if (g_ExtSymbols->GetModuleByOffset(UL64_TO_CDA(addrInModule), 0, &index, &base) == S_OK)
    {
        ArrayHolder<char> szModuleName = new char[MAX_LONGPATH+1];
        if (g_ExtSymbols->GetModuleNames(index, base, NULL, 0, NULL, szModuleName, MAX_LONGPATH, NULL, NULL, 0, NULL) == S_OK)
        {
            MultiByteToWideChar (CP_ACP, 0, szModuleName, MAX_LONGPATH, wszNameBuffer, MAX_LONGPATH);
            DOAPPEND (wszNameBuffer);
            bModuleNameWorked = TRUE;
        }
    }
    // If the dbgeng functions fail to get the module/assembly name, use the DAC API
    if (!bModuleNameWorked)
    {
        wszNameBuffer[0] = W('\0');
        if (FAILED(g_sos->GetPEFileName(dmd.PEAssembly, MAX_LONGPATH, wszNameBuffer, NULL)) || wszNameBuffer[0] == W('\0'))
        {
            ToRelease<IXCLRDataModule> pModule;
            if (SUCCEEDED(g_sos->GetModule(dmd.Address, &pModule)))
            {
                ULONG32 nameLen = 0;
                pModule->GetFileName(MAX_LONGPATH, &nameLen, wszNameBuffer);
            }
        }
        if (wszNameBuffer[0] != W('\0'))
        {
            const WCHAR *pJustName = _wcsrchr(wszNameBuffer, GetTargetDirectorySeparatorW());
            if (pJustName == NULL)
                pJustName = wszNameBuffer - 1;

            DOAPPEND(pJustName + 1);
            bModuleNameWorked = TRUE;
        }
    }

    // Under certain circumstances DacpMethodDescData::GetMethodDescName()
    //   returns a module qualified method name
    HRESULT hr = g_sos->GetMethodDescName(dwStartAddr, MAX_LONGPATH, wszNameBuffer, NULL);

    const WCHAR* pwszMethNameBegin = (hr != S_OK ? NULL : _wcschr(wszNameBuffer, L'!'));
    if (!bModuleNameWorked && hr == S_OK && pwszMethNameBegin != NULL)
    {
        // if we weren't able to get the module name, but GetMethodDescName returned
        // the module as part of the returned method name, use this data
        DOAPPEND(wszNameBuffer);
    }
    else
    {
        if (!bModuleNameWorked)
        {
            DOAPPEND (W("UNKNOWN"));
        }
        DOAPPEND(W("!"));
        if (hr == S_OK)
        {
            // the module name we retrieved above from debugger will take
            // precedence over the name possibly returned by GetMethodDescName()
            DOAPPEND(pwszMethNameBegin != NULL ? (pwszMethNameBegin+1) : (WCHAR *)wszNameBuffer);
        }
        else
        {
            DOAPPEND(W("UNKNOWN"));
        }
    }

    ULONG64 Displacement = (IPAddr - MethodDescData.NativeCodeAddr);
    if (Displacement)
    {
        _snwprintf_s(wszNameBuffer, MAX_LONGPATH, MAX_LONGPATH, W("+%#x"), Displacement); // _TRUNCATE
        DOAPPEND (wszNameBuffer);
    }

    return S_OK;
#undef DOAPPEND
}

#ifndef FEATURE_PAL

BOOL AppendContext(LPVOID pTransitionContexts, size_t maxCount, size_t *pcurCount, size_t uiSizeOfContext,
    CROSS_PLATFORM_CONTEXT *context)
{
    if (pTransitionContexts == NULL || *pcurCount >= maxCount)
    {
        ++(*pcurCount);
        return FALSE;
    }
    if (uiSizeOfContext == sizeof(StackTrace_SimpleContext))
    {
        StackTrace_SimpleContext *pSimple = (StackTrace_SimpleContext *) pTransitionContexts;
        g_targetMachine->FillSimpleContext(&pSimple[*pcurCount], context);
    }
    else if (uiSizeOfContext == g_targetMachine->GetContextSize())
    {
        // FillTargetContext ensures we only write uiSizeOfContext bytes in pTransitionContexts
        // and not sizeof(CROSS_PLATFORM_CONTEXT) bytes (which would overrun).
        g_targetMachine->FillTargetContext(pTransitionContexts, context, (int)(*pcurCount));
    }
    else
    {
        return FALSE;
    }
    ++(*pcurCount);
    return TRUE;
}

HRESULT CALLBACK ImplementEFNStackTrace(
    PDEBUG_CLIENT client,
    __out_ecount_opt(*puiTextLength) WCHAR wszTextOut[],
    size_t *puiTextLength,
    LPVOID pTransitionContexts,
    size_t *puiTransitionContextCount,
    size_t uiSizeOfContext,
    DWORD Flags)
{

#define DOAPPEND(str) if (!so.Append((str))) { \
    Status = E_OUTOFMEMORY;                    \
    goto Exit;                                 \
}

    HRESULT Status = E_FAIL;
    StringOutput so;
    size_t transitionContextCount = 0;

    if (puiTextLength == NULL)
    {
        return E_INVALIDARG;
    }

    if (pTransitionContexts)
    {
        if (puiTransitionContextCount == NULL)
        {
            return E_INVALIDARG;
        }

        // Do error checking on context size
        if ((uiSizeOfContext != g_targetMachine->GetContextSize()) &&
            (uiSizeOfContext != sizeof(StackTrace_SimpleContext)))
        {
            return E_INVALIDARG;
        }
    }

    IXCLRDataStackWalk *pStackWalk = NULL;
    IXCLRDataTask* Task;
    ULONG ThreadId;

    if ((Status = g_ExtSystem->GetCurrentThreadSystemId(&ThreadId)) != S_OK ||
        (Status = g_clrData->GetTaskByOSThreadID(ThreadId, &Task)) != S_OK)
    {
        // Not a managed thread.
        return SOS_E_NOMANAGEDCODE;
    }

    Status = Task->CreateStackWalk(CLRDATA_SIMPFRAME_UNRECOGNIZED |
                                   CLRDATA_SIMPFRAME_MANAGED_METHOD |
                                   CLRDATA_SIMPFRAME_RUNTIME_MANAGED_CODE |
                                   CLRDATA_SIMPFRAME_RUNTIME_UNMANAGED_CODE,
                                   &pStackWalk);

    Task->Release();

    if (Status != S_OK)
    {
        if (Status == E_FAIL)
        {
            return SOS_E_NOMANAGEDCODE;
        }
        return Status;
    }

#ifdef _TARGET_WIN64_
    ULONG numFrames = 0;
    BOOL bInNative = TRUE;

    Status = GetContextStackTrace(ThreadId, &numFrames);
    if (FAILED(Status))
    {
        goto Exit;
    }

    for (ULONG i = 0; i < numFrames; i++)
    {
        PDEBUG_STACK_FRAME pCur = g_Frames + i;

        CLRDATA_ADDRESS pMD;
        if (g_sos->GetMethodDescPtrFromIP(pCur->InstructionOffset, &pMD) == S_OK)
        {
            if (bInNative || transitionContextCount==0)
            {
                // We only want to list one transition frame if there are multiple frames.
                bInNative = FALSE;

                DOAPPEND (W("(TransitionMU)\n"));
                // For each transition, we need to store the context information
                if (puiTransitionContextCount)
                {
                    // below we cast the i-th AMD64_CONTEXT to CROSS_PLATFORM_CONTEXT
                    AppendContext (pTransitionContexts, *puiTransitionContextCount,
                        &transitionContextCount, uiSizeOfContext, (CROSS_PLATFORM_CONTEXT*)(&(g_FrameContexts[i])));
                }
                else
                {
                    transitionContextCount++;
                }
            }

            Status = DumpMDInfoBuffer((DWORD_PTR) pMD, Flags,
                    pCur->StackOffset, pCur->InstructionOffset, so);
            if (FAILED(Status))
            {
                goto Exit;
            }
            else if (Status == S_OK)
            {
                DOAPPEND (W("\n"));
            }
            // for S_FALSE do not append anything

        }
        else
        {
            if (!bInNative)
            {
                // We only want to list one transition frame if there are multiple frames.
                bInNative = TRUE;

                DOAPPEND (W("(TransitionUM)\n"));
                // For each transition, we need to store the context information
                if (puiTransitionContextCount)
                {
                    AppendContext (pTransitionContexts, *puiTransitionContextCount,
                        &transitionContextCount, uiSizeOfContext, (CROSS_PLATFORM_CONTEXT*)(&(g_FrameContexts[i])));
                }
                else
                {
                    transitionContextCount++;
                }
            }
        }
    }

Exit:
#else // _TARGET_WIN64_

#ifdef _DEBUG
    size_t prevLength = 0;
    static WCHAR wszNameBuffer[1024]; // should be large enough
    wcscpy_s(wszNameBuffer, 1024, W("Frame")); // default value
#endif

    BOOL bInNative = TRUE;

    UINT frameCount = 0;
    do
    {
        DacpFrameData FrameData;
        if ((Status = FrameData.Request(pStackWalk)) != S_OK)
        {
            goto Exit;
        }

        CROSS_PLATFORM_CONTEXT context;
        if ((Status=pStackWalk->GetContext(g_targetMachine->GetFullContextFlags(), g_targetMachine->GetContextSize(), NULL, (BYTE *)&context)) != S_OK)
        {
            goto Exit;
        }

        ExtDbgOut ( " * Ctx[BSI]:  %08x  %08x  %08x    ", GetBP(context), GetSP(context), GetIP(context) );

        CLRDATA_ADDRESS pMD;
        if (!FrameData.frameAddr)
        {
            if (bInNative || transitionContextCount==0)
            {
                // We only want to list one transition frame if there are multiple frames.
                bInNative = FALSE;

                DOAPPEND (W("(TransitionMU)\n"));
                // For each transition, we need to store the context information
                if (puiTransitionContextCount)
                {
                    AppendContext (pTransitionContexts, *puiTransitionContextCount,
                            &transitionContextCount, uiSizeOfContext, &context);
                }
                else
                {
                    transitionContextCount++;
                }
            }

            // we may have a method, try to get the methoddesc
            if (g_sos->GetMethodDescPtrFromIP(GetIP(context), &pMD)==S_OK)
            {
                Status = DumpMDInfoBuffer((DWORD_PTR) pMD, Flags,
                                          GetSP(context), GetIP(context), so);
                if (FAILED(Status))
                {
                    goto Exit;
                }
                else if (Status == S_OK)
                {
                    DOAPPEND (W("\n"));
                }
                // for S_FALSE do not append anything
            }
        }
        else
        {
#ifdef _DEBUG
            if (Output::IsDebugOutputEnabled())
            {
                DWORD_PTR vtAddr;
                MOVE(vtAddr, TO_TADDR(FrameData.frameAddr));
                if (g_sos->GetFrameName(TO_CDADDR(vtAddr), 1024, wszNameBuffer, NULL) == S_OK)
                    ExtDbgOut("[%ls: %08x] ", wszNameBuffer, FrameData.frameAddr);
                else
                    ExtDbgOut("[Frame: %08x] ", FrameData.frameAddr);
            }
#endif
            if (!bInNative)
            {
                // We only want to list one transition frame if there are multiple frames.
                bInNative = TRUE;

                DOAPPEND (W("(TransitionUM)\n"));
                // For each transition, we need to store the context information
                if (puiTransitionContextCount)
                {
                    AppendContext (pTransitionContexts, *puiTransitionContextCount,
                            &transitionContextCount, uiSizeOfContext, &context);
                }
                else
                {
                    transitionContextCount++;
                }
            }
        }

#ifdef _DEBUG
        if (so.Length() > prevLength)
        {
            ExtDbgOut ( "%ls", so.String()+prevLength );
            prevLength = so.Length();
        }
        else
            ExtDbgOut ( "\n" );
#endif

    }
    while ((frameCount++) < MAX_STACK_FRAMES && pStackWalk->Next()==S_OK);

    Status = S_OK;

Exit:
#endif // _TARGET_WIN64_

    if (pStackWalk)
    {
        pStackWalk->Release();
        pStackWalk = NULL;
    }

    // We have finished. Does the user want to copy this data to a buffer?
    if (Status == S_OK)
    {
        if(wszTextOut)
        {
            // They want at least partial output
            wcsncpy_s (wszTextOut, *puiTextLength, so.String(),  *puiTextLength-1); // _TRUNCATE
        }
        else
        {
            *puiTextLength = _wcslen (so.String()) + 1;
        }

        if (puiTransitionContextCount)
        {
            *puiTransitionContextCount = transitionContextCount;
        }
    }

    return Status;
}

// TODO: Convert PAL_TRY_NAKED to something that works on the Mac.
HRESULT CALLBACK ImplementEFNStackTraceTry(
    PDEBUG_CLIENT client,
    __out_ecount_opt(*puiTextLength) WCHAR wszTextOut[],
    size_t *puiTextLength,
    LPVOID pTransitionContexts,
    size_t *puiTransitionContextCount,
    size_t uiSizeOfContext,
    DWORD Flags)
{
    HRESULT Status = E_FAIL;

    PAL_TRY_NAKED
    {
        Status = ImplementEFNStackTrace(client, wszTextOut, puiTextLength,
            pTransitionContexts, puiTransitionContextCount,
            uiSizeOfContext, Flags);
    }
    PAL_EXCEPT_NAKED (EXCEPTION_EXECUTE_HANDLER)
    {
    }
    PAL_ENDTRY_NAKED

    return Status;
}

// See sos_stacktrace.h for the contract with the callers regarding the LPVOID arguments.
HRESULT CALLBACK _EFN_StackTrace(
    PDEBUG_CLIENT client,
    __out_ecount_opt(*puiTextLength) WCHAR wszTextOut[],
    size_t *puiTextLength,
    __out_bcount_opt(uiSizeOfContext*(*puiTransitionContextCount)) LPVOID pTransitionContexts,
    size_t *puiTransitionContextCount,
    size_t uiSizeOfContext,
    DWORD Flags)
{
    INIT_API_EFN();

    Status = ImplementEFNStackTraceTry(client, wszTextOut, puiTextLength,
        pTransitionContexts, puiTransitionContextCount,
        uiSizeOfContext, Flags);

    return Status;
}

BOOL FormatFromRemoteString(DWORD_PTR strObjPointer, __out_ecount(cchString) PWSTR wszBuffer, ULONG cchString)
{
    BOOL bRet = FALSE;

    wszBuffer[0] = L'\0';

    DacpObjectData objData;
    if (objData.Request(g_sos, TO_CDADDR(strObjPointer))!=S_OK)
    {
        return bRet;
    }

    strobjInfo stInfo;

    if (MOVE(stInfo, strObjPointer) != S_OK)
    {
        return bRet;
    }

    DWORD dwBufLength = 0;
    if (!ClrSafeInt<DWORD>::addition(stInfo.m_StringLength, 1, dwBufLength))
    {
        ExtOut("<integer overflow>\n");
        return bRet;
    }

    LPWSTR pwszBuf = new NOTHROW WCHAR[dwBufLength];
    if (pwszBuf == NULL)
    {
        return bRet;
    }

    if (g_sos->GetObjectStringData(TO_CDADDR(strObjPointer), stInfo.m_StringLength+1, pwszBuf, NULL)!=S_OK)
    {
        delete [] pwszBuf;
        return bRet;
    }

    // String is in format
    // <SP><SP><SP>at <function name>(args,...)\n
    // ...
    // Parse and copy just <function name>(args,...)

    LPWSTR pwszPointer = pwszBuf;

    WCHAR PSZSEP[] = W("   at ");

    UINT Length = 0;
    while(1)
    {
        if (wcsncmp(pwszPointer, PSZSEP, ARRAY_SIZE(PSZSEP)-1) != 0)
        {
            delete [] pwszBuf;
            return bRet;
        }

        pwszPointer += wcslen(PSZSEP);
        LPWSTR nextPos = (LPWSTR)wcsstr(pwszPointer, PSZSEP);
        if (nextPos == NULL)
        {
            // Done! Note that we are leaving the function before we add the last
            // line of stack trace to the output string. This is on purpose because
            // this string needs to be merged with a real trace, and the last line
            // of the trace will be common to the real trace.
            break;
        }
        WCHAR c = *nextPos;
        *nextPos = L'\0';

        // Buffer is calculated for sprintf below ("   %p %p %S\n");
        WCHAR wszLineBuffer[mdNameLen + 8 + sizeof(size_t)*2];

        // Note that we don't add a newline because we have this embedded in wszLineBuffer
        swprintf_s(wszLineBuffer, ARRAY_SIZE(wszLineBuffer), W("    %p %p %s"), SOS_PTR(-1), SOS_PTR(-1), pwszPointer);
        Length += (UINT)wcslen(wszLineBuffer);

        if (wszBuffer)
        {
            wcsncat_s(wszBuffer, cchString, wszLineBuffer, _TRUNCATE);
        }

        *nextPos = c;
        // Move to the next line.
        pwszPointer = nextPos;
    }

    delete [] pwszBuf;

    // Return TRUE only if the stack string had any information that was successfully parsed.
    // (Length > 0) is a good indicator of that.
    bRet = (Length > 0);
    return bRet;
}

HRESULT AppendExceptionInfo(CLRDATA_ADDRESS cdaObj,
    __out_ecount(cchString) PWSTR wszStackString,
    ULONG cchString,
    BOOL bNestedCase) // If bNestedCase is TRUE, the last frame of the computed stack is left off
{
    DacpObjectData objData;
    if (objData.Request(g_sos, cdaObj) != S_OK)
    {
        return E_FAIL;
    }

    // Make sure it is an exception object, and get the MT of Exception
    CLRDATA_ADDRESS exceptionMT = isExceptionObj(objData.MethodTable);
    if (exceptionMT == NULL)
    {
        return E_INVALIDARG;
    }

    // First try to get exception object data using ISOSDacInterface2
    DacpExceptionObjectData excData;
    BOOL bGotExcData = SUCCEEDED(excData.Request(g_sos, cdaObj));

    int iOffset;
    // Is there a _remoteStackTraceString? We'll want to prepend that data.
    // We only have string data, so IP/SP info has to be set to -1.
    DWORD_PTR strPointer;
    if (bGotExcData)
    {
        strPointer = TO_TADDR(excData.RemoteStackTraceString);
    }
    else
    {
        iOffset = GetObjFieldOffset (cdaObj, objData.MethodTable, W("_remoteStackTraceString"));
        MOVE (strPointer, TO_TADDR(cdaObj) + iOffset);
    }
    if (strPointer)
    {
        WCHAR *pwszBuffer = new NOTHROW WCHAR[cchString];
        if (pwszBuffer == NULL)
        {
            return E_OUTOFMEMORY;
        }

        if (FormatFromRemoteString(strPointer, pwszBuffer, cchString))
        {
            // Prepend this stuff to the string for the user
            wcsncat_s(wszStackString, cchString, pwszBuffer, _TRUNCATE);
        }
        delete[] pwszBuffer;
    }

    BOOL bAsync = bGotExcData ? IsAsyncException(excData)
                              : IsAsyncException(cdaObj, objData.MethodTable);

    DWORD_PTR arrayPtr = GetStackTraceArray(cdaObj, &objData, bGotExcData ? &excData : NULL);

    if (arrayPtr)
    {
        DWORD arrayLen;
        MOVE (arrayLen, arrayPtr + sizeof(DWORD_PTR));

        if (arrayLen)
        {
            DWORD_PTR dataPtr = GetFirstArrayElementPointer(arrayPtr);
            size_t stackTraceSize = 0;
            MOVE (stackTraceSize, dataPtr); // data length is stored at the beginning of the array in this case

            DWORD cbStackSize = static_cast<DWORD>(stackTraceSize * sizeof(StackTraceElement));
            dataPtr += sizeof(size_t) + sizeof(size_t); // skip the array header, then goes the data

            if (stackTraceSize != 0)
            {
                size_t iLength = FormatGeneratedException (dataPtr, cbStackSize, NULL, 0, bAsync, bNestedCase);
                WCHAR *pwszBuffer = new NOTHROW WCHAR[iLength + 1];
                if (pwszBuffer)
                {
                    FormatGeneratedException(dataPtr, cbStackSize, pwszBuffer, iLength + 1, bAsync, bNestedCase);
                    wcsncat_s(wszStackString, cchString, pwszBuffer, _TRUNCATE);
                    delete[] pwszBuffer;
                }
                else
                {
                    return E_OUTOFMEMORY;
                }
            }
        }
    }
    return S_OK;
}

HRESULT ImplementEFNGetManagedExcepStack(
    CLRDATA_ADDRESS cdaStackObj,
    __out_ecount(cchString) PWSTR wszStackString,
    ULONG cchString)
{
    HRESULT Status = E_FAIL;

    if (wszStackString == NULL || cchString == 0)
    {
        return E_INVALIDARG;
    }

    CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
    DacpThreadData Thread;
    BOOL bCanUseThreadContext = TRUE;

    ZeroMemory(&Thread, sizeof(DacpThreadData));

    if ((threadAddr == NULL) || (Thread.Request(g_sos, threadAddr) != S_OK))
    {
        // The current thread is unmanaged
        bCanUseThreadContext = FALSE;
    }

    if (cdaStackObj == NULL)
    {
        if (!bCanUseThreadContext)
        {
            return E_INVALIDARG;
        }

        TADDR taLTOH = NULL;
        if ((!SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle),
                            &taLTOH,
                            sizeof(taLTOH), NULL)) || (taLTOH==NULL))
        {
            return Status;
        }
        else
        {
            cdaStackObj = TO_CDADDR(taLTOH);
        }
    }

    // Put the stack trace header on
    AddExceptionHeader(wszStackString, cchString);

    // First is there a nested exception?
    if (bCanUseThreadContext && Thread.firstNestedException)
    {
        CLRDATA_ADDRESS obj = 0, next = 0;
        CLRDATA_ADDRESS currentNested = Thread.firstNestedException;
        do
        {
            Status = g_sos->GetNestedExceptionData(currentNested, &obj, &next);

            // deal with the inability to read a nested exception gracefully
            if (Status != S_OK)
            {
                break;
            }

            Status = AppendExceptionInfo(obj, wszStackString, cchString, TRUE);
            currentNested = next;
        }
        while(currentNested != NULL);
    }

    Status = AppendExceptionInfo(cdaStackObj, wszStackString, cchString, FALSE);

    return Status;
}


// TODO: Enable this when ImplementEFNStackTraceTry is fixed.
// This function, like VerifyDAC, exists for the purpose of testing
// hard-to-get-to SOS APIs.
DECLARE_API(VerifyStackTrace)
{
    INIT_API();

    BOOL bVerifyManagedExcepStack = FALSE;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-ManagedExcepStack", &bVerifyManagedExcepStack, COBOOL, FALSE},
    };

    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL,0,NULL))
    {
        return E_INVALIDARG;
    }

    if (bVerifyManagedExcepStack)
    {
        CLRDATA_ADDRESS threadAddr = GetCurrentManagedThread();
        DacpThreadData Thread;

        TADDR taExc = NULL;
        if ((threadAddr == NULL) || (Thread.Request(g_sos, threadAddr) != S_OK))
        {
            ExtOut("The current thread is unmanaged\n");
            return Status;
        }

        TADDR taLTOH = NULL;
        if ((!SafeReadMemory(TO_TADDR(Thread.lastThrownObjectHandle),
                            &taLTOH,
                            sizeof(taLTOH), NULL)) || (taLTOH == NULL))
        {
            ExtOut("There is no current managed exception on this thread\n");
            return Status;
        }
        else
        {
            taExc = taLTOH;
        }

        const SIZE_T cchStr = 4096;
        WCHAR *wszStr = (WCHAR *)alloca(cchStr * sizeof(WCHAR));
        if (ImplementEFNGetManagedExcepStack(TO_CDADDR(taExc), wszStr, cchStr) != S_OK)
        {
            ExtOut("Error!\n");
            return Status;
        }

        ExtOut("_EFN_GetManagedExcepStack(%P, wszStr, sizeof(wszStr)) returned:\n", SOS_PTR(taExc));
        ExtOut("%S\n", wszStr);

        if (ImplementEFNGetManagedExcepStack((ULONG64)NULL, wszStr, cchStr) != S_OK)
        {
            ExtOut("Error!\n");
            return Status;
        }

        ExtOut("_EFN_GetManagedExcepStack(NULL, wszStr, sizeof(wszStr)) returned:\n");
        ExtOut("%S\n", wszStr);
    }
    else
    {
        size_t textLength = 0;
        size_t contextLength = 0;
        Status = ImplementEFNStackTraceTry(client,
                                 NULL,
                                 &textLength,
                                 NULL,
                                 &contextLength,
                                 0,
                                 0);

        if (Status != S_OK)
        {
            ExtOut("Error: %lx\n", Status);
            return Status;
        }

        ExtOut("Number of characters requested: %d\n", textLength);
        WCHAR *wszBuffer = new NOTHROW WCHAR[textLength + 1];
        if (wszBuffer == NULL)
        {
            ReportOOM();
            return Status;
        }

        // For the transition contexts buffer the callers are expected to allocate
        // contextLength * sizeof(TARGET_CONTEXT), and not
        // contextLength * sizeof(CROSS_PLATFORM_CONTEXT). See sos_stacktrace.h for
        // details.
        LPBYTE pContexts = new NOTHROW BYTE[contextLength * g_targetMachine->GetContextSize()];

        if (pContexts == NULL)
        {
            ReportOOM();
            delete[] wszBuffer;
            return Status;
        }

        Status = ImplementEFNStackTrace(client,
                                 wszBuffer,
                                 &textLength,
                                 pContexts,
                                 &contextLength,
                                 g_targetMachine->GetContextSize(),
                                 0);

        if (Status != S_OK)
        {
            ExtOut("Error: %lx\n", Status);
            delete[] wszBuffer;
            delete [] pContexts;
            return Status;
        }

        ExtOut("%S\n", wszBuffer);

        ExtOut("Context information:\n");
        if (IsDbgTargetX86())
        {
            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                   "Ebp", "Esp", "Eip");
        }
        else if (IsDbgTargetAmd64())
        {
            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                   "Rbp", "Rsp", "Rip");
        }
        else if (IsDbgTargetArm())
        {
            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                   "FP", "SP", "PC");
        }
        else
        {
            ExtOut("Unsupported platform");
            delete [] pContexts;
            delete[] wszBuffer;
            return S_FALSE;
        }

        for (size_t j=0; j < contextLength; j++)
        {
            CROSS_PLATFORM_CONTEXT *pCtx = (CROSS_PLATFORM_CONTEXT*)(pContexts + j*g_targetMachine->GetContextSize());
            ExtOut("%p %p %p\n", SOS_PTR(GetBP(*pCtx)), SOS_PTR(GetSP(*pCtx)), SOS_PTR(GetIP(*pCtx)));
        }

        delete [] pContexts;

        StackTrace_SimpleContext *pSimple = new NOTHROW StackTrace_SimpleContext[contextLength];
        if (pSimple == NULL)
        {
            ReportOOM();
            delete[] wszBuffer;
            return Status;
        }

        Status = ImplementEFNStackTrace(client,
                                 wszBuffer,
                                 &textLength,
                                 pSimple,
                                 &contextLength,
                                 sizeof(StackTrace_SimpleContext),
                                 0);

        if (Status != S_OK)
        {
            ExtOut("Error: %lx\n", Status);
            delete[] wszBuffer;
            delete [] pSimple;
            return Status;
        }

        ExtOut("Simple Context information:\n");
        if (IsDbgTargetX86())
            ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                       "Ebp", "Esp", "Eip");
        else if (IsDbgTargetAmd64())
                ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                       "Rbp", "Rsp", "Rip");
        else if (IsDbgTargetArm())
                ExtOut("%" POINTERSIZE "s %" POINTERSIZE "s %" POINTERSIZE "s\n",
                       "FP", "SP", "PC");
        else
        {
            ExtOut("Unsupported platform");
            delete[] wszBuffer;
            delete [] pSimple;
            return S_FALSE;
        }
        for (size_t j=0; j < contextLength; j++)
        {
            ExtOut("%p %p %p\n", SOS_PTR(pSimple[j].FrameOffset),
                    SOS_PTR(pSimple[j].StackOffset),
                    SOS_PTR(pSimple[j].InstructionOffset));
        }
        delete [] pSimple;
        delete[] wszBuffer;
    }

    return Status;
}

// This is an internal-only Apollo extension to save breakpoint/watch state
DECLARE_API(SaveState)
{
    INIT_API_NOEE();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    StringHolder filePath;
    CMDValue arg[] =
    {   // vptr, type
        {&filePath.data, COSTRING},
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if(nArg == 0)
    {
        ExtOut("Usage: !SaveState <file_path>\n");
    }

    FILE* pFile;
    errno_t error = fopen_s(&pFile, filePath.data, "w");
    if(error != 0)
    {
        ExtOut("Failed to open file %s, error=0x%x\n", filePath.data, error);
        return E_FAIL;
    }

    g_bpoints.SaveBreakpoints(pFile);
    g_watchCmd.SaveListToFile(pFile);

    fclose(pFile);
    ExtOut("Session breakpoints and watch expressions saved to %s\n", filePath.data);
    return S_OK;
}

#endif // FEATURE_PAL

DECLARE_API(SuppressJitOptimization)
{
    INIT_API_NOEE();
    MINIDUMP_NOT_SUPPORTED();
    ONLY_SUPPORTED_ON_WINDOWS_TARGET();

    StringHolder onOff;
    CMDValue arg[] =
    {   // vptr, type
        {&onOff.data, COSTRING},
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return E_INVALIDARG;
    }

    if (nArg == 1 && (_stricmp(onOff.data, "On") == 0))
    {
        SetNGENCompilerFlags(CORDEBUG_JIT_DISABLE_OPTIMIZATION);

        if (!g_fAllowJitOptimization)
        {
            ExtOut("JIT optimization is already suppressed\n");
        }
        else
        {
            g_fAllowJitOptimization = FALSE;
            g_ExtControl->Execute(DEBUG_OUTCTL_NOT_LOGGED, "sxe -c \"!SOSHandleCLRN\" clrn", 0);
            ExtOut("JIT optimization will be suppressed\n");
        }
    }
    else if(nArg == 1 && (_stricmp(onOff.data, "Off") == 0))
    {
        SetNGENCompilerFlags(CORDEBUG_JIT_DEFAULT);

        if (g_fAllowJitOptimization)
            ExtOut("JIT optimization is already permitted\n");
        else
        {
            g_fAllowJitOptimization = TRUE;
            ExtOut("JIT optimization will be permitted\n");
        }
    }
    else
    {
        ExtOut("Usage: %ssuppressjitoptimization <on|off>\n", SOSPrefix);
    }

    return S_OK;
}

// Uses ICorDebug to set the state of desired NGEN compiler flags. This can suppress pre-jitted optimized
// code
HRESULT SetNGENCompilerFlags(DWORD flags)
{
    HRESULT hr;

    // if CLR is already loaded, try to change the flags now
    hr = GetRuntime(&g_pRuntime);
    if (SUCCEEDED(hr))
    {
        ToRelease<ICorDebugProcess2> proc2;
        ICorDebugProcess* pCorDebugProcess;
        if (FAILED(hr = g_pRuntime->GetCorDebugInterface(&pCorDebugProcess)))
        {
            ExtOut("SOS: warning, prejitted code optimizations could not be changed. Failed to load ICorDebug HR = 0x%x\n", hr);
        }
        else if (FAILED(pCorDebugProcess->QueryInterface(__uuidof(ICorDebugProcess2), (void**)&proc2)))
        {
            if (flags != CORDEBUG_JIT_DEFAULT)
            {
                ExtOut("SOS: warning, prejitted code optimizations could not be changed. This CLR version doesn't support the functionality\n");
            }
            else
            {
                hr = S_OK;
            }
        }
        else if (FAILED(hr = proc2->SetDesiredNGENCompilerFlags(flags)))
        {
            // Versions of CLR that don't have SetDesiredNGENCompilerFlags DAC-ized will return E_FAIL.
            // This was first supported in the clr_triton branch around 4/1/12, Apollo release
            // It will likely be supported in desktop CLR during Dev12
            if (hr == E_FAIL)
            {
                if (flags != CORDEBUG_JIT_DEFAULT)
                {
                    ExtOut("SOS: warning, prejitted code optimizations could not be changed. This CLR version doesn't support the functionality\n");
                }
                else
                {
                    hr = S_OK;
                }
            }
            else if (hr == CORDBG_E_NGEN_NOT_SUPPORTED)
            {
                if (flags != CORDEBUG_JIT_DEFAULT)
                {
                    ExtOut("SOS: warning, prejitted code optimizations could not be changed. This CLR version doesn't support NGEN\n");
                }
                else
                {
                    hr = S_OK;
                }
            }
            else if (hr == CORDBG_E_MUST_BE_IN_CREATE_PROCESS)
            {
                DWORD currentFlags = 0;
                if (FAILED(hr = proc2->GetDesiredNGENCompilerFlags(&currentFlags)))
                {
                    ExtOut("SOS: warning, prejitted code optimizations could not be changed. GetDesiredNGENCompilerFlags failed hr=0x%x\n", hr);
                }
                else if (currentFlags != flags)
                {
                    ExtOut("SOS: warning, prejitted code optimizations could not be changed at this time. This setting is fixed once CLR starts\n");
                }
                else
                {
                    hr = S_OK;
                }
            }
            else
            {
                ExtOut("SOS: warning, prejitted code optimizations could not be changed at this time. SetDesiredNGENCompilerFlags hr = 0x%x\n", hr);
            }
        }
    }

    return hr;
}

#ifndef FEATURE_PAL

HRESULT LoadModuleEvent(IDebugClient* client, PCSTR moduleName)
{
    HRESULT handleEventStatus = DEBUG_STATUS_NO_CHANGE;

    if (moduleName != NULL)
    {
        bool isRuntimeModule = false;

        for (int runtime = 0; runtime < IRuntime::ConfigurationEnd; ++runtime)
        {
            if (_stricmp(moduleName, GetRuntimeModuleName((IRuntime::RuntimeConfiguration)runtime)) == 0)
            {
                isRuntimeModule = true;
                break;
            }
        }

        if (isRuntimeModule)
        {
            Extensions::GetInstance()->FlushTarget();
            if (g_breakOnRuntimeModuleLoad)
            {
                g_breakOnRuntimeModuleLoad = false;
                HandleRuntimeLoadedNotification(client);
            }
            // if we don't want the JIT to optimize, we should also disable optimized NGEN images
            if (!g_fAllowJitOptimization)
            {
                ExtQuery(client);

                // If we aren't successful SetNGENCompilerFlags will print relevant error messages
                // and then we need to stop the debugger so the user can intervene if desired
                if (FAILED(SetNGENCompilerFlags(CORDEBUG_JIT_DISABLE_OPTIMIZATION)))
                {
                    handleEventStatus = DEBUG_STATUS_BREAK;
                }
                ExtRelease();
            }
        }
    }

    return handleEventStatus;
}

#endif // FEATURE_PAL

DECLARE_API(StopOnCatch)
{
    INIT_API();
    MINIDUMP_NOT_SUPPORTED();

    g_stopOnNextCatch = TRUE;
    ULONG32 flags = 0;
    g_clrData->GetOtherNotificationFlags(&flags);
    flags |= CLRDATA_NOTIFY_ON_EXCEPTION_CATCH_ENTER;
    g_clrData->SetOtherNotificationFlags(flags);
    ExtOut("Debuggee will break the next time a managed exception is caught during execution\n");
    return S_OK;
}

class EnumMemoryCallback : public ICLRDataEnumMemoryRegionsCallback, ICLRDataLoggingCallback
{
private:
    LONG m_ref;
    bool m_log;
    bool m_valid;

public:
    EnumMemoryCallback(bool log, bool valid) :
        m_ref(1),
        m_log(log),
        m_valid(valid)
    {
    }

    virtual ~EnumMemoryCallback()
    {
    }

    STDMETHODIMP QueryInterface(
        ___in REFIID InterfaceId,
        ___out PVOID* Interface)
    {
        if (InterfaceId == IID_IUnknown ||
            InterfaceId == IID_ICLRDataEnumMemoryRegionsCallback)
        {
            *Interface = (ICLRDataEnumMemoryRegionsCallback*)this;
            AddRef();
            return S_OK;
        }
        else if (InterfaceId == IID_ICLRDataLoggingCallback)
        {
            *Interface = (ICLRDataLoggingCallback*)this;
            AddRef();
            return S_OK;
        }
        else
        {
            *Interface = nullptr;
            return E_NOINTERFACE;
        }
    }

    STDMETHODIMP_(ULONG) AddRef()
    {
        LONG ref = InterlockedIncrement(&m_ref);
        return ref;
    }

    STDMETHODIMP_(ULONG) Release()
    {
        LONG ref = InterlockedDecrement(&m_ref);
        if (ref == 0)
        {
            delete this;
        }
        return ref;
    }

    HRESULT STDMETHODCALLTYPE EnumMemoryRegion(
        /* [in] */ CLRDATA_ADDRESS address,
        /* [in] */ ULONG32 size)
    {
        if (m_log)
        {
            ExtOut("%016llx %08x\n", address, size);
        }
        if (m_valid)
        {
            uint64_t start = address;
            uint64_t numberPages = (size + DT_OS_PAGE_SIZE - 1) / DT_OS_PAGE_SIZE;
            for (size_t p = 0; p < numberPages; p++, start += DT_OS_PAGE_SIZE)
            {
                BYTE buffer[1];
                ULONG read;
                if (FAILED(g_ExtData->ReadVirtual(start, buffer, ARRAY_SIZE(buffer), &read)))
                {
                    ExtOut("Invalid: %016llx %08x start %016llx\n", address, size, start);
                    break;
                }
            }
        }
        if (IsInterrupt())
        {
            return COR_E_OPERATIONCANCELED;
        }
        return S_OK;
    }

    HRESULT STDMETHODCALLTYPE LogMessage(
        /* [in] */ LPCSTR message)
    {
        ExtOut("%s", message);
        if (IsInterrupt())
        {
            return COR_E_OPERATIONCANCELED;
        }
        return S_OK;
    }
};

DECLARE_API(enummem)
{
    INIT_API();

    ToRelease<ICLRDataEnumMemoryRegions> enumMemoryRegions;
    Status = g_clrData->QueryInterface(__uuidof(ICLRDataEnumMemoryRegions), (void**)&enumMemoryRegions);
    if (SUCCEEDED(Status))
    {
        ToRelease<ICLRDataEnumMemoryRegionsCallback> callback = new EnumMemoryCallback(false, true);
        ULONG32 minidumpType =
           (MiniDumpWithPrivateReadWriteMemory |
            MiniDumpWithDataSegs |
            MiniDumpWithHandleData |
            MiniDumpWithUnloadedModules |
            MiniDumpWithFullMemoryInfo |
            MiniDumpWithThreadInfo |
            MiniDumpWithTokenInformation);
        Status = enumMemoryRegions->EnumMemoryRegions(callback, minidumpType, CLRDataEnumMemoryFlags::CLRDATA_ENUM_MEM_DEFAULT);
        if (FAILED(Status))
        {
            ExtErr("EnumMemoryRegions FAILED %08x\n", Status);
        }
    }
    return Status;
}

#ifndef FEATURE_PAL

// This is an undocumented SOS extension command intended to help test SOS
// It causes the Dml output to be printed to the console uninterpretted so
// that a test script can read the commands which are hidden in the markup
DECLARE_API(ExposeDML)
{
    Output::SetDMLExposed(true);
    return S_OK;
}

// According to kksharma the Windows debuggers always sign-extend
// arguments when calling externally, therefore StackObjAddr
// conforms to CLRDATA_ADDRESS contract.
HRESULT CALLBACK
_EFN_GetManagedExcepStack(
    PDEBUG_CLIENT client,
    ULONG64 StackObjAddr,
   __out_ecount (cbString) PSTR szStackString,
    ULONG cbString
    )
{
    INIT_API_EFN();

    ArrayHolder<WCHAR> tmpStr = new NOTHROW WCHAR[cbString];
    if (tmpStr == NULL)
    {
        ReportOOM();
        return E_OUTOFMEMORY;
    }

    if (FAILED(Status = ImplementEFNGetManagedExcepStack(StackObjAddr, tmpStr, cbString)))
    {
        return Status;
    }

    if (WideCharToMultiByte(CP_ACP, WC_NO_BEST_FIT_CHARS, tmpStr, -1, szStackString, cbString, NULL, NULL) == 0)
    {
        return E_FAIL;
    }

    return S_OK;
}

// same as _EFN_GetManagedExcepStack, but returns the stack as a wide string.
HRESULT CALLBACK
_EFN_GetManagedExcepStackW(
    PDEBUG_CLIENT client,
    ULONG64 StackObjAddr,
    __out_ecount(cchString) PWSTR wszStackString,
    ULONG cchString
    )
{
    INIT_API_EFN();

    return ImplementEFNGetManagedExcepStack(StackObjAddr, wszStackString, cchString);
}

// According to kksharma the Windows debuggers always sign-extend
// arguments when calling externally, therefore objAddr
// conforms to CLRDATA_ADDRESS contract.
HRESULT CALLBACK
_EFN_GetManagedObjectName(
    PDEBUG_CLIENT client,
    ULONG64 objAddr,
    __out_ecount (cbName) PSTR szName,
    ULONG cbName
    )
{
    INIT_API_EFN();

    if (!sos::IsObject(objAddr, false))
    {
        return E_INVALIDARG;
    }

    sos::Object obj = TO_TADDR(objAddr);

    if (WideCharToMultiByte(CP_ACP, 0, obj.GetTypeName(), (int) (_wcslen(obj.GetTypeName()) + 1),
                            szName, cbName, NULL, NULL) == 0)
    {
        return E_FAIL;
    }
    return S_OK;
}

// According to kksharma the Windows debuggers always sign-extend
// arguments when calling externally, therefore objAddr
// conforms to CLRDATA_ADDRESS contract.
HRESULT CALLBACK
_EFN_GetManagedObjectFieldInfo(
    PDEBUG_CLIENT client,
    ULONG64 objAddr,
    __out_ecount (mdNameLen) PSTR szFieldName,
    PULONG64 pValue,
    PULONG pOffset
    )
{
    INIT_API_EFN();
    DacpObjectData objData;
    LPWSTR fieldName = (LPWSTR)alloca(mdNameLen * sizeof(WCHAR));

    if (szFieldName == NULL || *szFieldName == '\0' ||
        objAddr == NULL)
    {
        return E_FAIL;
    }

    if (pOffset == NULL && pValue == NULL)
    {
        // One of these needs to be valid
        return E_FAIL;
    }

    if (FAILED(objData.Request(g_sos, objAddr)))
    {
        return E_FAIL;
    }

    MultiByteToWideChar(CP_ACP,0,szFieldName,-1,fieldName,mdNameLen);

    int iOffset = GetObjFieldOffset (objAddr, objData.MethodTable, fieldName);
    if (iOffset <= 0)
    {
        return E_FAIL;
    }

    if (pOffset)
    {
        *pOffset = (ULONG) iOffset;
    }

    if (pValue)
    {
        if (FAILED(g_ExtData->ReadVirtual(UL64_TO_CDA(objAddr + iOffset), pValue, sizeof(ULONG64), NULL)))
        {
            return E_FAIL;
        }
    }

    return S_OK;
}

DECLARE_API(VerifyGMT)
{
    ULONG osThreadId;
    {
        INIT_API();

        CMDValue arg[] =
        {   // vptr, type
            {&osThreadId, COHEX},
        };
        size_t nArg;

        if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
        {
            return E_INVALIDARG;
        }
    }
    ULONG64 managedThread;
    HRESULT hr = _EFN_GetManagedThread(client, osThreadId, &managedThread);
    {
        INIT_API();
        ONLY_SUPPORTED_ON_WINDOWS_TARGET();

        if (SUCCEEDED(hr)) {
            ExtOut("%08x %p\n", osThreadId, SOS_PTR(managedThread));
        }
        else {
            ExtErr("_EFN_GetManagedThread FAILED %08x\n", hr);
        }
    }
    return hr;
}

HRESULT CALLBACK
_EFN_GetManagedThread(
    PDEBUG_CLIENT client,
    ULONG osThreadId,
    PULONG64 pManagedThread)
{
    INIT_API_EFN();

    _ASSERTE(pManagedThread != nullptr);
    *pManagedThread = 0;

    DacpThreadStoreData threadStore;
    if ((Status = threadStore.Request(g_sos)) != S_OK)
    {
        return Status;
    }

    CLRDATA_ADDRESS curThread = threadStore.firstThread;
    while (curThread)
    {
        DacpThreadData thread;
        if ((Status = thread.Request(g_sos, curThread)) != S_OK)
        {
            return Status;
        }
        if (thread.osThreadId == osThreadId)
        {
            *pManagedThread = (ULONG64)curThread;
            return S_OK;
        }
        curThread = thread.nextThread;
    }

    return E_INVALIDARG;
}

//
// Sets the .NET Core runtime path to use to run the managed code within SOS/native debugger.
//
DECLARE_API(SetHostRuntime)
{
    INIT_API_EXT();

    BOOL bNetFx = FALSE;
    BOOL bNetCore = FALSE;
    BOOL bNone = FALSE;
    BOOL bClear = FALSE;
    DWORD_PTR majorRuntimeVersion = 0;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-netfx", &bNetFx, COBOOL, FALSE},
        {"-f", &bNetFx, COBOOL, FALSE},
        {"-netcore", &bNetCore, COBOOL, FALSE},
        {"-c", &bNetCore, COBOOL, FALSE},
        {"-none", &bNone, COBOOL, FALSE},
        {"-clear", &bClear, COBOOL, FALSE},
        {"-major", &majorRuntimeVersion, COSIZE_T, TRUE},
    };
    StringHolder hostRuntimeDirectory;
    CMDValue arg[] =
    {
        {&hostRuntimeDirectory.data, COSTRING},
    };
    size_t narg;
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), arg, ARRAY_SIZE(arg), &narg))
    {
        return E_INVALIDARG;
    }
    HostRuntimeFlavor flavor = HostRuntimeFlavor::NetCore;
    int major = 0, minor = 0;
    if (narg > 0 || majorRuntimeVersion > 0 || bClear || bNetCore || bNetFx || bNone)
    {
        if (IsHostingInitialized())
        {
            ExtErr("Runtime hosting already initialized\n");
            goto exit;
        }
        if (bClear)
        {
            SetHostRuntime(HostRuntimeFlavor::NetCore, 0, 0, nullptr);
        }
        if (bNone)
        {
            flavor = HostRuntimeFlavor::None;
        }
        else if (bNetCore)
        {
            flavor = HostRuntimeFlavor::NetCore;
        }
        else if (bNetFx)
        {
            flavor = HostRuntimeFlavor::NetFx;
        }
        major = (int)majorRuntimeVersion;
        if (!SetHostRuntime(flavor, major, minor, hostRuntimeDirectory.data))
        {
            ExtErr("Invalid host runtime path %s\n", hostRuntimeDirectory.data);
            return E_FAIL;
        }
    }
exit:
    LPCSTR directory = nullptr;
    GetHostRuntime(flavor, major, minor, directory);
    switch (flavor)
    {
        case HostRuntimeFlavor::None:
            ExtOut("Using no runtime to host the managed SOS code. Some commands are not availible.\n");
            break;
        case HostRuntimeFlavor::NetCore:
            if (major == 0)
            {
                ExtOut("Using .NET Core runtime to host the managed SOS code\n");
            }
            else
            {
                ExtOut("Using .NET Core runtime (version %d.%d) to host the managed SOS code\n", major, minor);
            }
            break;
        case HostRuntimeFlavor::NetFx:
            ExtOut("Using desktop .NET Framework runtime to host the managed SOS code\n");
            break;
        default:
            break;
    }
    if (directory != nullptr)
    {
        ExtOut("Host runtime path: %s\n", directory);
    }
    return S_OK;
}

DECLARE_API(processor)
{
    INIT_API_EXT();
    ULONG executingType;
    if (SUCCEEDED(g_ExtControl->GetExecutingProcessorType(&executingType)))
    {
        ExtOut("Executing processor type: %04x '%s'\n", executingType, GetProcessorName(executingType));
    }
    ULONG actualType;
    if (SUCCEEDED(g_ExtControl->GetActualProcessorType(&actualType)))
    {
        ExtOut("Actual processor type:    %04x '%s'\n", actualType, GetProcessorName(actualType));
    }
    ULONG effectiveType;
    if (SUCCEEDED(g_ExtControl->GetEffectiveProcessorType(&effectiveType)))
    {
        ExtOut("Effective processor type: %04x '%s'\n", effectiveType, GetProcessorName(effectiveType));
    }
    return S_OK;
}

#endif // FEATURE_PAL

//
// Sets the runtime module path
//
DECLARE_API(SetClrPath)
{
    INIT_API_NODAC_PROBE_MANAGED("setclrpath");

    StringHolder runtimeModulePath;
    CMDValue arg[] =
    {
        {&runtimeModulePath.data, COSTRING},
    };
    size_t narg;
    if (!GetCMDOption(args, nullptr, 0, arg, ARRAY_SIZE(arg), &narg))
    {
        return E_FAIL;
    }
    if (narg > 0)
    {
        std::string fullPath;
        if (!GetAbsolutePath(runtimeModulePath.data, fullPath))
        {
            ExtErr("Invalid runtime directory %s\n", fullPath.c_str());
            return E_FAIL;
        }
        g_pRuntime->SetRuntimeDirectory(fullPath.c_str());
    }
    const char* runtimeDirectory = g_pRuntime->GetRuntimeDirectory();
    if (runtimeDirectory != nullptr) {
        ExtOut("Runtime module directory: %s\n", runtimeDirectory);
    }
    return S_OK;
}

//
// Lists and selects the current runtime
//
DECLARE_API(runtimes)
{
    INIT_API_NODAC_PROBE_MANAGED("runtimes");

    BOOL bNetFx = FALSE;
    BOOL bNetCore = FALSE;
    CMDOption option[] =
    {   // name, vptr, type, hasValue
        {"-netfx", &bNetFx, COBOOL, FALSE},
        {"-netcore", &bNetCore, COBOOL, FALSE},
    };
    if (!GetCMDOption(args, option, ARRAY_SIZE(option), NULL, 0, NULL))
    {
        return E_INVALIDARG;
    }
    if (bNetCore || bNetFx)
    {
#ifndef FEATURE_PAL
        if (IsWindowsTarget())
        {
            PCSTR name = bNetFx ? "desktop .NET Framework" : ".NET Core";
            if (!Target::SwitchRuntime(bNetFx))
            {
                ExtErr("The %s runtime is not loaded\n", name);
                return E_INVALIDARG;
            }
            ExtOut("Switched to %s runtime successfully\n", name);
        }
        else
#endif
        {
            ExtErr("The '-netfx' and '-netcore' options are only supported on Windows targets\n");
            return E_INVALIDARG;
        }
    }
    else
    {
        Target::DisplayStatus();
    }
    return Status;
}

const std::string
GetDirectory(const std::string& fileName)
{
    size_t last = fileName.rfind(DIRECTORY_SEPARATOR_STR_A);
    if (last != std::string::npos) {
        last++;
    }
    else {
        last = 0;
    }
    return fileName.substr(0, last);
}

void PrintHelp (__in_z LPCSTR pszCmdName)
{
    static LPSTR pText = NULL;

    if (pText == NULL) {
#ifndef FEATURE_PAL
        HGLOBAL hResource = NULL;
        HRSRC hResInfo = FindResourceW(g_hInstance, TEXT ("DOCUMENTATION"), TEXT ("TEXT"));
        if (hResInfo) hResource = LoadResource(g_hInstance, hResInfo);
        if (hResource) pText = (LPSTR)LockResource(hResource);
        if (pText == NULL)
        {
            ExtErr("Error loading documentation resource\n");
            return;
        }
#else
        Dl_info info;
        if (dladdr((PVOID)&PrintHelp, &info) == 0)
        {
            ExtErr("Error: Failed to get SOS module directory\n");
            return;
        }
        char lpFilename[MAX_LONGPATH + 12]; // + 12 to make enough room for strcat function.
        strcpy_s(lpFilename, ARRAY_SIZE(lpFilename), GetDirectory(info.dli_fname).c_str());
        strcat_s(lpFilename, ARRAY_SIZE(lpFilename), "sosdocsunix.txt");

        HANDLE hSosDocFile = CreateFileA(lpFilename, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
        if (hSosDocFile == INVALID_HANDLE_VALUE) {
            ExtErr("Error finding documentation file\n");
            return;
        }

        HANDLE hMappedSosDocFile = CreateFileMappingA(hSosDocFile, NULL, PAGE_READONLY, 0, 0, NULL);
        CloseHandle(hSosDocFile);
        if (hMappedSosDocFile == NULL) {
            ExtErr("Error mapping documentation file\n");
            return;
        }

        pText = (LPSTR)MapViewOfFile(hMappedSosDocFile, FILE_MAP_READ, 0, 0, 0);
        CloseHandle(hMappedSosDocFile);
        if (pText == NULL)
        {
            ExtErr("Error loading documentation file\n");
            return;
        }
#endif
    }

    // Find our line in the text file
    char searchString[MAX_LONGPATH];
    sprintf_s(searchString, ARRAY_SIZE(searchString), "COMMAND: %s.", pszCmdName);

    LPSTR pStart = strstr(pText, searchString);
    LPSTR pEnd = NULL;
    if (!pStart)
    {
        ExtErr("Documentation for %s not found.\n", pszCmdName);
        return;
    }

    // Go to the end of this line:
    pStart = strchr(pStart, '\n');
    if (!pStart)
    {
        ExtErr("Expected newline in documentation resource.\n");
        return;
    }

    // Bypass the newline that pStart points to and setup pEnd for the loop below. We set
    // pEnd to be the old pStart since we add one to it when we call strstr.
    pEnd = pStart++;

    // Find the first occurrence of \\ followed by an \r or an \n on a line by itself.
    do
    {
        pEnd = strstr(pEnd+1, "\\\\");
    } while (pEnd && ((pEnd[-1] != '\r' && pEnd[-1] != '\n') || (pEnd[3] != '\r' && pEnd[3] != '\n')));

    if (pEnd)
    {
        // We have found a \\ followed by a \r or \n.  Do not print out the character pEnd points
        // to, as this will be the first \ (this is why we don't add one to the second parameter).
        ExtOut("%.*s", pEnd - pStart, pStart);
    }
    else
    {
        // If pEnd is false then we have run to the end of the document.  However, we did find
        // the command to print, so we should simply print to the end of the file.  We'll add
        // an extra newline here in case the file does not contain one.
        ExtOut("%s\n", pStart);
    }
}

/**********************************************************************\
* Routine Description:                                                 *
*                                                                      *
*    This function displays the commands available in strike and the   *
*    arguments passed into each.
*                                                                      *
\**********************************************************************/
DECLARE_API(Help)
{
    INIT_API_NOEE_PROBE_MANAGED("help");

    StringHolder commandName;
    CMDValue arg[] =
    {
        {&commandName.data, COSTRING}
    };
    size_t nArg;
    if (!GetCMDOption(args, NULL, 0, arg, ARRAY_SIZE(arg), &nArg))
    {
        return Status;
    }

    ExtOut("-------------------------------------------------------------------------------\n");

    if (nArg == 1)
    {
        // Convert commandName to lower-case
        LPSTR curChar = commandName.data;
        while (*curChar != '\0')
        {
            if ( ((unsigned) *curChar <= 0x7F) && isupper(*curChar))
            {
                *curChar = (CHAR) tolower(*curChar);
            }
            curChar++;
        }

        // Strip off leading "!" if the user put that.
        curChar = commandName.data;
        if (*curChar == '!')
            curChar++;

        PrintHelp (curChar);
    }
    else
    {
        PrintHelp ("contents");
    }

    return S_OK;
}
