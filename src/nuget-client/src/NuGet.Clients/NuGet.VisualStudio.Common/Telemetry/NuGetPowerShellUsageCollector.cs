// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.VisualStudio.Common.Telemetry.PowerShell;
using NuGet.VisualStudio.Telemetry;

namespace NuGet.VisualStudio.Common.Telemetry
{
    public sealed class NuGetPowerShellUsageCollector : IDisposable
    {
        // PMC, PMUI powershell telemetry consts
        public const string PmcExecuteCommandCount = nameof(PmcExecuteCommandCount);
        public const string PmuiExecuteCommandCount = nameof(PmuiExecuteCommandCount);
        public const string NuGetCommandUsed = nameof(NuGetCommandUsed);
        public const string InitPs1LoadPmui = nameof(InitPs1LoadPmui);
        public const string InitPs1LoadPmc = nameof(InitPs1LoadPmc);
        public const string InitPs1LoadedFromPmcFirst = nameof(InitPs1LoadedFromPmcFirst);
        public const string LoadedFromPmui = nameof(LoadedFromPmui);
        public const string FirstTimeLoadedFromPmui = nameof(FirstTimeLoadedFromPmui);
        public const string LoadedFromPmc = nameof(LoadedFromPmc);
        public const string FirstTimeLoadedFromPmc = nameof(FirstTimeLoadedFromPmc);
        public const string SolutionLoaded = nameof(SolutionLoaded);
        public const string Trigger = nameof(Trigger);
        public const string PSVersion = nameof(PSVersion);
        public const string Pmc = nameof(Pmc);
        public const string Pmui = nameof(Pmui);

        // PMC UI Console Container telemetry consts
        public const string PmcWindowLoadCount = nameof(PmcWindowLoadCount);
        public const string ReOpenAtStart = nameof(ReOpenAtStart);

        // Const name for emitting when VS solution close or VS instance close.
        public const string SolutionClose = nameof(SolutionClose);
        public const string InstanceClose = nameof(InstanceClose);
        public const string PowerShellHost = "PowerShellHost.";
        public const string SolutionCount = nameof(SolutionCount);
        public const string PmcPowerShellLoadedSolutionCount = nameof(PmcPowerShellLoadedSolutionCount);
        public const string PmuiPowerShellLoadedSolutionCount = nameof(PmuiPowerShellLoadedSolutionCount);
        public const string PowerShellLoaded = nameof(PowerShellLoaded);

        private int _solutionCount;
        private SolutionData _vsSolutionData;
        private readonly InstanceData _vsInstanceData;
        private object _lock = new object();

        public NuGetPowerShellUsageCollector()
        {
            _vsSolutionData = new SolutionData();
            _vsInstanceData = new InstanceData();

            NuGetPowerShellUsage.PowerShellLoadEvent += NuGetPowerShellUsage_PMCLoadEventHandler;
            NuGetPowerShellUsage.PowerShellCommandExecuteEvent += NuGetPowerShellUsage_PowerShellCommandExecuteEventHandler;
            NuGetPowerShellUsage.NuGetCmdletExecutedEvent += NuGetPowerShellUsage_NuGetCmdletExecutedEventHandler;
            NuGetPowerShellUsage.InitPs1LoadEvent += NuGetPowerShellUsage_InitPs1LoadEventHandler;
            NuGetPowerShellUsage.PmcWindowsEvent += NuGetPowerShellUsage_PMCWindowsEventHandler;
            NuGetPowerShellUsage.SolutionOpenEvent += NuGetPowerShellUsage_SolutionOpenHandler;
            NuGetPowerShellUsage.SolutionCloseEvent += NuGetPowerShellUsage_SolutionCloseHandler;

            InstanceCloseTelemetryEmitter.AddEventsOnShutdown += NuGetPowerShellUsage_VSInstanseCloseHandler;
        }

        private void NuGetPowerShellUsage_PMCLoadEventHandler(bool isPMC)
        {
            AddPowerShellLoadedData(isPMC, _vsSolutionData);
        }

        internal void AddPowerShellLoadedData(bool isPMC, SolutionData vsSolutionData)
        {
            lock (_lock)
            {
                if (isPMC)
                {
                    if (!vsSolutionData.LoadedFromPmc)
                    {
                        vsSolutionData.FirstTimeLoadedFromPmc = true;
                        _vsInstanceData.PmcLoadedSolutionCount++;
                    }

                    vsSolutionData.LoadedFromPmc = true;
                }
                else
                {
                    if (!vsSolutionData.LoadedFromPmui)
                    {
                        vsSolutionData.FirstTimeLoadedFromPmui = true;
                        _vsInstanceData.PmuiLoadedSolutionCount++;
                    }

                    vsSolutionData.LoadedFromPmui = true;
                }
            }
        }

        private void NuGetPowerShellUsage_PowerShellCommandExecuteEventHandler(bool isPMC)
        {
            AddPowerShellCommandExecuteData(isPMC, _vsSolutionData);
        }

        internal void AddPowerShellCommandExecuteData(bool isPMC, SolutionData vsSolutionData)
        {
            lock (_lock)
            {
                // Please note: Direct PMC and PMUI don't share same code path for installing packages with *.ps1 files
                if (isPMC)
                {
                    // For PMC all installation done in one pass so no double counting.
                    vsSolutionData.PmcExecuteCommandCount++;
                    _vsInstanceData.PmcExecuteCommandCount++;
                }
                else
                {
                    // This one is called for both init.ps1 and install.ps1 seperately.
                    // install.ps1 running inside MSBuildNuGetProject.cs (InstallPackageAsync method) may result in duplicate counting.
                    // Also this concern valid for dependent packages (of installing package) with *.ps1 files.
                    vsSolutionData.PmuiExecuteCommandCount++;
                    _vsInstanceData.PmuiExecuteCommandCount++;
                }
            }
        }

        private void NuGetPowerShellUsage_NuGetCmdletExecutedEventHandler()
        {
            AddNuGetCmdletExecutedData(_vsSolutionData);
        }

        internal void AddNuGetCmdletExecutedData(SolutionData vsSolutionData)
        {
            lock (_lock)
            {
                vsSolutionData.NuGetCommandUsed = true;
            }
        }

        private void NuGetPowerShellUsage_InitPs1LoadEventHandler(bool isPMC)
        {
            AddInitPs1LoadData(isPMC, _vsSolutionData);
        }

        internal void AddInitPs1LoadData(bool isPMC, SolutionData vsSolutionData)
        {
            lock (_lock)
            {
                if (isPMC && (!vsSolutionData.FirstTimeLoadedFromPmc && !vsSolutionData.FirstTimeLoadedFromPmui))
                {
                    vsSolutionData.InitPs1LoadedFromPmcFirst = true;
                }

                if (isPMC)
                {
                    vsSolutionData.InitPs1LoadPmc = true;

                    if (!vsSolutionData.LoadedFromPmc)
                    {
                        vsSolutionData.FirstTimeLoadedFromPmc = true;
                        _vsInstanceData.PmcLoadedSolutionCount++;
                    }

                    vsSolutionData.LoadedFromPmc = true;
                }
                else
                {
                    vsSolutionData.InitPs1LoadPmui = true;

                    if (!vsSolutionData.LoadedFromPmui)
                    {
                        vsSolutionData.FirstTimeLoadedFromPmui = true;
                        _vsInstanceData.PmuiLoadedSolutionCount++;
                    }

                    vsSolutionData.LoadedFromPmui = true;
                }
            }
        }

        private void NuGetPowerShellUsage_PMCWindowsEventHandler(bool isLoad)
        {
            AddPMCWindowsEventData(isLoad);
        }

        internal void AddPMCWindowsEventData(bool isLoad)
        {
            lock (_lock)
            {
                if (isLoad)
                {
                    _vsSolutionData.PmcWindowLoadCount++;
                    _vsInstanceData.PmcWindowLoadCount++;
                }

                // Shutdown call happen before Unload event for PMCWindow so VSInstanceClose event going to have current up to date status.
                _vsInstanceData.ReOpenAtStart = isLoad;
            }
        }

        private void NuGetPowerShellUsage_SolutionOpenHandler()
        {
            lock (_lock)
            {
                // Edge case: PMC used without solution load
                if (!_vsSolutionData.SolutionLoaded && _vsSolutionData.PmcExecuteCommandCount > 0)
                {
                    // PMC used before any solution is loaded, let's emit what we have for nugetvsinstanceclose event.
                    TelemetryActivity.EmitTelemetryEvent(_vsSolutionData.ToTelemetryEvent());
                    ClearSolutionData();
                }

                if (_vsSolutionData.LoadedFromPmc)
                {
                    _vsInstanceData.PmcLoadedSolutionCount++;
                }

                if (_vsSolutionData.LoadedFromPmui)
                {
                    _vsInstanceData.PmuiLoadedSolutionCount++;
                }

                _vsInstanceData.SolutionCount = ++_solutionCount;
                _vsSolutionData.SolutionLoaded = true;
            }
        }

        private void NuGetPowerShellUsage_SolutionCloseHandler()
        {
            lock (_lock)
            {
                TelemetryActivity.EmitTelemetryEvent(_vsSolutionData.ToTelemetryEvent());
                ClearSolutionData();
            }
        }

        private void NuGetPowerShellUsage_VSInstanseCloseHandler(object sender, TelemetryEvent telemetryEvent)
        {
            lock (_lock)
            {
                // Edge case: PMC used without solution load
                if (!_vsSolutionData.SolutionLoaded && _vsSolutionData.PmcExecuteCommandCount > 0)
                {
                    // PMC used before any solution is loaded, let's emit what we have for nugetvsinstanceclose event.
                    TelemetryActivity.EmitTelemetryEvent(_vsSolutionData.ToTelemetryEvent());
                }

                // Add VS Instance telemetry
                _vsInstanceData.AddProperties(telemetryEvent);
            }
        }

        // If open new solution then need to clear previous solution events. But powershell remain loaded in memory.
        private void ClearSolutionData()
        {
            bool pmcPowershellLoad = _vsSolutionData.LoadedFromPmc;
            bool pmuiPowershellLoad = _vsSolutionData.LoadedFromPmui;

            _vsSolutionData = new SolutionData();

            _vsSolutionData.LoadedFromPmc = pmcPowershellLoad;
            _vsSolutionData.LoadedFromPmui = pmuiPowershellLoad;
        }

        public void Dispose()
        {
            NuGetPowerShellUsage.PowerShellLoadEvent -= NuGetPowerShellUsage_PMCLoadEventHandler;
            NuGetPowerShellUsage.PowerShellCommandExecuteEvent -= NuGetPowerShellUsage_PowerShellCommandExecuteEventHandler;
            NuGetPowerShellUsage.NuGetCmdletExecutedEvent -= NuGetPowerShellUsage_NuGetCmdletExecutedEventHandler;
            NuGetPowerShellUsage.InitPs1LoadEvent -= NuGetPowerShellUsage_InitPs1LoadEventHandler;
            NuGetPowerShellUsage.PmcWindowsEvent -= NuGetPowerShellUsage_PMCWindowsEventHandler;
            NuGetPowerShellUsage.SolutionOpenEvent -= NuGetPowerShellUsage_SolutionOpenHandler;
            NuGetPowerShellUsage.SolutionCloseEvent -= NuGetPowerShellUsage_SolutionCloseHandler;

            InstanceCloseTelemetryEmitter.AddEventsOnShutdown -= NuGetPowerShellUsage_VSInstanseCloseHandler;
        }

        internal class SolutionData
        {
            internal bool FirstTimeLoadedFromPmc { get; set; }
            internal bool FirstTimeLoadedFromPmui { get; set; }
            internal bool InitPs1LoadedFromPmcFirst { get; set; }
            internal bool InitPs1LoadPmc { get; set; }
            internal bool InitPs1LoadPmui { get; set; }
            internal bool LoadedFromPmc { get; set; }
            internal bool LoadedFromPmui { get; set; }
            internal bool NuGetCommandUsed { get; set; }
            internal int PmcExecuteCommandCount { get; set; }
            internal int PmcWindowLoadCount { get; set; }
            internal int PmuiExecuteCommandCount { get; set; }
            internal bool SolutionLoaded { get; set; }

            internal SolutionData()
            {
                FirstTimeLoadedFromPmc = false;
                FirstTimeLoadedFromPmui = false;
                InitPs1LoadedFromPmcFirst = false;
                InitPs1LoadPmc = false;
                InitPs1LoadPmui = false;
                LoadedFromPmc = false;
                LoadedFromPmui = false;
                NuGetCommandUsed = false;
                PmcExecuteCommandCount = 0;
                PmcWindowLoadCount = 0;
                PmuiExecuteCommandCount = 0;
                SolutionLoaded = false;

            }

            internal TelemetryEvent ToTelemetryEvent()
            {
                var telemetry = new SolutionCloseEvent(
                    firstTimeLoadedFromPmc: FirstTimeLoadedFromPmc,
                    firstTimeLoadedFromPmui: FirstTimeLoadedFromPmui,
                    initPs1LoadedFromPmcFirst: InitPs1LoadedFromPmcFirst,
                    initPs1LoadPmc: InitPs1LoadPmc,
                    initPs1LoadPmui: InitPs1LoadPmui,
                    loadedFromPmc: LoadedFromPmc,
                    loadedFromPmui: LoadedFromPmui,
                    nuGetCommandUsed: NuGetCommandUsed,
                    pmcExecuteCommandCount: PmcExecuteCommandCount,
                    pmcWindowLoadCount: PmcWindowLoadCount,
                    pmuiExecuteCommandCount: PmuiExecuteCommandCount,
                    solutionLoaded: SolutionLoaded
                    );
                return telemetry;
            }
        }

        internal class InstanceData
        {
            internal int PmcExecuteCommandCount { get; set; }
            internal int PmcWindowLoadCount { get; set; }
            internal int PmuiExecuteCommandCount { get; set; }
            internal int PmcLoadedSolutionCount { get; set; }
            internal int PmuiLoadedSolutionCount { get; set; }
            internal bool ReOpenAtStart { get; set; }
            internal int SolutionCount { get; set; }

            internal InstanceData()
            {
                PmcExecuteCommandCount = 0;
                PmcWindowLoadCount = 0;
                PmuiExecuteCommandCount = 0;
                PmcLoadedSolutionCount = 0;
                PmuiLoadedSolutionCount = 0;
                ReOpenAtStart = false;
                SolutionCount = 0;
            }

            internal void AddProperties(TelemetryEvent telemetryEvent)
            {
                telemetryEvent[PowerShellHost + NuGetPowerShellUsageCollector.PmcExecuteCommandCount] = PmcExecuteCommandCount;
                telemetryEvent[PowerShellHost + NuGetPowerShellUsageCollector.PmcWindowLoadCount] = PmcWindowLoadCount;
                telemetryEvent[PowerShellHost + NuGetPowerShellUsageCollector.PmuiExecuteCommandCount] = PmuiExecuteCommandCount;
                telemetryEvent[PowerShellHost + PmcPowerShellLoadedSolutionCount] = PmcLoadedSolutionCount;
                telemetryEvent[PowerShellHost + PmuiPowerShellLoadedSolutionCount] = PmuiLoadedSolutionCount;
                telemetryEvent[PowerShellHost + NuGetPowerShellUsageCollector.ReOpenAtStart] = ReOpenAtStart;
                telemetryEvent[PowerShellHost + NuGetPowerShellUsageCollector.SolutionCount] = SolutionCount;
            }
        }
    }
}
