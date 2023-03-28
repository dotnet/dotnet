// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Deployment.Internal.CodeSigning;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Deployment.CommandLineParser;
using Microsoft.Deployment.Utilities;

namespace Microsoft.Deployment.MageCLI
{
    /// <summary>
    /// Validation and execution of commands.
    /// </summary>
    internal class Command
    {
        // Constants for the namespace management
        private const string NamespaceAssemblyUri = "urn:schemas-microsoft-com:asm.v1";
        private const string NamespaceAssemblyV2Uri = "urn:schemas-microsoft-com:asm.v2";
        private const string NamespaceMsrelUri = "http://schemas.microsoft.com/windows/rel/2005/reldata";
        private const string NamespaceLicenseUri = "urn:mpeg:mpeg21:2003:01-REL-R-NS";
        private const string NamespaceAuthenticodeUri = "http://schemas.microsoft.com/windows/pki/2005/Authenticode";
        private const string Sha256SignatureMethodUri = @"http://www.w3.org/2000/09/xmldsig#rsa-sha256";
        private const string Sha256DigestMethod = @"http://www.w3.org/2000/09/xmldsig#sha256";

        [DllImport("Dfshim.dll", CharSet = CharSet.Auto)]
        public static extern bool CleanOnlineAppCache();

        /// <summary>
        /// Operations are identified with these bit-flag constants.
        /// </summary>
        /// 
        private enum Operations
        {
            LaunchGUI = 1,
            ShowHelp = 2,
            ShowVerboseHelp = 4,
            GenerateSomething = 8,
            GenerateApplicationManifest = 16,
            GenerateDeploymentManifest = 32,
            UpdateSomething = 128,
            UpdateApplicationManifest = 256,
            UpdateDeploymentManifest = 512,
            SignSomething = 2048,
            SignApplicationManifest = 4096,
            SignDeploymentManifest = 8192,
            CleanApplicationCache = 16384,
            VerifyManifest = 32768,
            AddLauncher = 65536,
        }

        /// <summary>
        /// Operations to process. Individual operations are identified via bit-flags,
        /// and more than one may be specified.
        /// </summary>
        private Operations operations;

        /// <summary>
        /// Trust levels.
        /// </summary>
        public enum TrustLevels
        {
            None, FullTrust, LocalIntranet, Internet
        }

        public enum DigestAlgorithmValue
        {
            // Supporting only sha256
            sha256RSA
        }

        /// <summary>
        /// File to be updated or signed.
        /// </summary>
        private string inputPath = null;

#region - Command line argument members -

        //////////////////////////////////////////////////////////////////////
        // The following members are public to allow the command-line-argument
        // support class to access them.  
        //////////////////////////////////////////////////////////////////////

        [CommandLineArgument(LongName = "ToFile", ShortName = "t")]
        public string outputPath = null;

        [CommandLineArgument(LongName = "Version", ShortName = "v")]
        public string applicationVersionString = null;

        [CommandLineArgument(LongName = "Name", ShortName = "n")]
        public string applicationName = null;

        [CommandLineArgument(LongName = "Processor", ShortName = "p")]
        public string processorString = null;

        [CommandLineArgument(LongName = "FromDirectory", ShortName = "fd")]
        public string fromDirectory = null;

        [CommandLineArgument(LongName = "CertFile", ShortName = "cf")]
        public string certPath = null;

        [CommandLineArgument(LongName = "CertHash", ShortName = "ch")]
        public string certHash = null;

        [CommandLineArgument(LongName = "Password", ShortName = "pwd")]
        public string certPassword = null;

        [CommandLineArgument(LongName = "TrustLevel", ShortName = "tr")]
        public string trustLevelString = null;

        [CommandLineArgument(LongName = "AppManifest", ShortName = "appm")]
        public string applicationManifestPath = null;

        [CommandLineArgument(LongName = "AppCodeBase", ShortName = "appc")]
        public string applicationCodeBase = null;

        [CommandLineArgument(LongName = "ProviderUrl", ShortName = "pu")]
        public string applicationProviderUrl = null;

        [CommandLineArgument(LongName = "MinVersion", ShortName = "mv")]
        public string isRequiredUpdateString = null;

        [CommandLineArgument(LongName = "TimeStampUri", ShortName = "ti")]
        public string timestamp = null;

        [CommandLineArgument(LongName = "Install", ShortName = "i")]
        public string installString = null;

        [CommandLineArgument(LongName = "IncludeProviderURL", ShortName = "ip")]
        public string includeDeploymentProviderUrlString = null;

        [CommandLineArgument(LongName = "KeyContainer", ShortName = "kc")]
        public string keyContainer = null;

        [CommandLineArgument(LongName = "CryptoProvider", ShortName = "csp")]
        public string cryptoProviderName = null;

        [CommandLineArgument(LongName = "TrustURLParameters", ShortName = "tu")]
        public string trustUrlParametersString = null;

        [CommandLineArgument(LongName = "UseManifestForTrust", ShortName = "um")]
        public string useApplicationManifestForTrustInfoString = null;

        [CommandLineArgument(LongName = "IconFile", ShortName = "if")]
        public string iconFile = null;

        [CommandLineArgument(LongName = "Publisher", ShortName = "pub")]
        public string publisherName = null;

        [CommandLineArgument(LongName = "SupportUrl", ShortName = "s")]
        public string supportUrl = null;

        [CommandLineArgument(LongName = "Algorithm", ShortName = "a")]
        public string digestAlgorithmValue = null;

        [CommandLineArgument(LongName = "TargetDirectory", ShortName = "td")]
        public string targetDirectory = null;

#endregion


        /// <summary>
        /// Algorithm used to calculate the digest hashes inside a manifest
        /// </summary>
        private DigestAlgorithmValue algorithm = DigestAlgorithmValue.sha256RSA;

        /// <summary>
        /// Application version object, parsed from applicationVersionString member
        /// </summary>
        private Version applicationVersion;

        /// <summary>
        /// Processor type, in enum form, parsed from processorString member
        /// </summary>
        private Processors processor = Processors.Undefined;

        /// <summary>
        /// Trust level, in enum form, parsed from trustLevelString member
        /// </summary>
        private TrustLevels trustLevel = TrustLevels.None;

        /// <summary>
        /// RequiredUpdate, in boolean form, parsed from the isRequiredUpdateString member
        /// </summary>
        private string minVersion = null;

        /// <summary>
        /// Install, in boolean form, parsed from the installString member
        /// </summary>
        private TriStateBool install = TriStateBool.Undefined;

        /// <summary>
        /// IncludeProviderURL, in boolean form, parsed from the includeDeploymentProviderUrlString member
        /// </summary>
        private TriStateBool includeDeploymentProviderUrl = TriStateBool.Undefined;

        /// <summary>
        /// UseManifestForTrust, in boolean form, parsed from the useApplicationManifestForTrustInfoString member
        /// </summary>
        private TriStateBool useApplicationManifestForTrustInfo = TriStateBool.Undefined;

        /// <summary>
        /// TrustURLParameters, in boolean form, parsed from the trustUrlParametersString member
        /// </summary>
        private TriStateBool trustUrlParameters = TriStateBool.Undefined;

        /// <summary>
        /// This object is cached between CanExecute(), where it is opened and
        /// tested for validity, and Execute(), where it actually gets used.
        /// </summary>
        private ApplicationManifest cachedAppManifest = null;

        /// <summary>
        /// This object is cached between CanExecute(), where it is opened and
        /// tested for validity, and Execute(), where it actually gets used.
        /// </summary>
        private DeployManifest cachedDepManifest = null;

        private X509Certificate2 storedCert;

        /// <summary>
        /// Indicates whether the user used the -sign verb on the command 
        /// line.  This is used to determine which other command-line options
        /// are legal.
        /// </summary>
        private bool signVerb = false;

        /// <summary>
        /// Name of the binary to be launched by Launcher.
        /// </summary>
        private string binaryToLaunch = null;

        /// <summary>
        /// Initialize member variables with command-line arguments.
        /// </summary>
        /// <param name="args">command-line arguments, passed in from Main()</param>
        public Command(string[] args)
        {
            // The command-line parser doesn't support the Mage grammar until
            // the first two arguments are processed.
            bool help = false;
            bool helpVerbose = false;
            String verb = null;
            String noun = null;
            int colonIndex = -1;

            if (args.Length > 0)
            {
                verb = args[0];
                args[0] = null;
                colonIndex = verb.IndexOf(':');
            }
            else
            {
                operations |= Operations.ShowHelp;
                return;
            }

            if (colonIndex == -1)
            {
                if (args.Length > 1)
                {
                    noun = args[1];
                    args[1] = null;
                }
            }
            else
            {
                noun = verb.Substring(colonIndex + 1);
                verb = verb.Substring(0, colonIndex);
            }

            if ((verb != null) && (verb.Length > 0))
            {
                // The first parameter will be one of the following.
                switch (verb[0])
                {
                    case '-':
                    case '/':
                        verb = verb.Substring(1);

                        switch (verb.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "new":
                            case "n":
                                operations |= Operations.GenerateSomething;
                                break;

                            case "update":
                            case "u":
                                operations |= Operations.UpdateSomething;
                                break;

                            case "sign":
                            case "s":
                                operations |= Operations.SignSomething;
                                signVerb = true;
                                break;

                            case "clearapplicationcache":
                            case "cc":                                
                                operations |= Operations.CleanApplicationCache;
                                break;

                            case "verify":
                            case "ver":
                                operations |= Operations.VerifyManifest;
                                break;

                            case "addlauncher":
                            case "al":
                                operations |= Operations.AddLauncher;
                                break;

                            case "help":
                            case "h":
                            case "?":
                                help = true;
                                if ((noun != null) && (noun.ToLower(CultureInfo.InvariantCulture) == "verbose"))
                                {
                                    helpVerbose = true;
                                }

                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }

            if (help)
            {
                if (helpVerbose)
                {
                    operations |= Operations.ShowVerboseHelp;
                }
                else
                {
                    operations |= Operations.ShowHelp;
                }

                return;
            }

            // If -new was specified, find out what kind of file to generate.
            if (Requested(Operations.GenerateSomething))
            {
                if (noun == null)
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidFileType, "");
                    return;
                }

                switch (noun.ToLower(CultureInfo.InvariantCulture))
                {
                    case "application":
                        operations |= Operations.GenerateApplicationManifest;
                        break;

                    case "deployment":
                        operations |= Operations.GenerateDeploymentManifest;
                        break;

                    default:
                        Application.PrintErrorMessage(ErrorMessages.InvalidFileType, noun);
                        operations = 0;
                        return;
                }
            }

            // If update or sign was selected, the next parameter has to be the filename
            if (Requested(Operations.UpdateSomething) || Requested(Operations.SignSomething))
            {
                // File existence will be checked in CanExecute()
                inputPath = noun;
            }

            // If verify operation was selected, the next parameter must be the manifest
            if (Requested(Operations.VerifyManifest))
            {
                // File existence will be checked in CanExecute()
                inputPath = noun;
            }

            // If add launcher operation was selected, the next parameter must be the name of the app to launch
            if (Requested(Operations.AddLauncher))
            {
                if (noun == null)
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingBinaryToLaunch, "");
                    return;
                }

                binaryToLaunch = noun;
            }

            // The rest of the command-line grammar is handled by command-line
            // parsing utility.
            CommandLineArgumentParser parser = new CommandLineArgumentParser(GetType(), new ErrorReporter(Console.Error.WriteLine));
            if (!parser.Parse(args, this))
            {
                operations |= Operations.ShowHelp;
            }

            // Check for existence of any of the signing options.
            // Signing can be requested via the -sign switch, or via -update
            // with a key-related option.
            if ((certPath != null) || (certHash != null) || (certPassword != null) || (keyContainer != null) || (cryptoProviderName != null))
            {
                operations |= Operations.SignSomething;
            }
        }

        /// <summary>
        /// Helper method to check if option is selected.
        /// </summary>
        /// <param name="op">An operation bit to test for in this.operations</param>
        /// <returns>true if the bit was set, false if not</returns>
        private bool Requested(Operations op)
        {
            return ((operations & op) > 0);
        }

        /// <summary>
        /// Determines whether or not the requested operations can be executed 
        /// with the given parameters.
        /// </summary>
        /// <returns>true or false</returns>
        private bool CanExecuteInner()
        {
            bool result = true;

            // Make sure that one of the required verbs was present     
            if (!(Requested(Operations.GenerateSomething) || Requested(Operations.UpdateSomething)
                || Requested(Operations.SignSomething) || Requested(Operations.VerifyManifest)
                || Requested(Operations.AddLauncher)))
            {
                Application.PrintErrorMessage(ErrorMessages.NoVerb);
                result = false;
            }
            
            // Verification operation is exclusive
            if (Requested(Operations.VerifyManifest) && (Operations.VerifyManifest != operations))
            {
                Application.PrintErrorMessage(ErrorMessages.VerifyIsExclusive);
                return false;
            }

            // Add Launcher operation is exclusive
            if (Requested(Operations.AddLauncher) && (Operations.AddLauncher != operations))
            {
                Application.PrintErrorMessage(ErrorMessages.AddLauncherIsExclusive);
                return false;
            }

            if (Requested(Operations.UpdateSomething) &&
                !CanUpdate())
            {
                return false;
            }

            if (Requested(Operations.SignSomething) &&
                !CanSign())
            {
                return false;
            }

            // Check for parameters that are not valid with the chosen operation
            if (!CheckForInvalidParameters())
            {
                result = false;
            }

            // If doing something with deployment manifest, validate the application manifest path
            if (Requested(Operations.GenerateDeploymentManifest) || Requested(Operations.UpdateDeploymentManifest))
            {
                if (applicationManifestPath != null)
                {
                    if (File.Exists(applicationManifestPath))
                    {
                        OpenAndCacheManifestInputFile(applicationManifestPath);
                        if (cachedAppManifest == null)
                        {
                            Application.PrintErrorMessage(ErrorMessages.InvalidInputFile, applicationManifestPath);
                            result = false;
                        }
                        else
                        {
                            // Make sure the application manifest does not include the HostInBrowser tag
                            if (cachedAppManifest.HostInBrowser)
                            {
                                Application.PrintErrorMessage(ErrorMessages.ApplicationManifestCannotHaveHostInBrowserTag);
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        Application.PrintErrorMessage(ErrorMessages.InvalidPath, applicationManifestPath);
                        result = false;
                    }
                }
            }

            if (outputPath == null && !Requested(Operations.AddLauncher))
            {
                // If a command-line operation was requested, and no output file 
                // was specified, use default filename.
                if (Requested(Operations.GenerateApplicationManifest))
                {
                    outputPath = "application.exe.manifest";
                }

                if (Requested(Operations.GenerateDeploymentManifest))
                {
                    outputPath = "deploy.application";
                }

                // Still null?
                if (outputPath == null)
                {
                    if (inputPath != null)
                    {
                        outputPath = inputPath;
                    }
                    else
                    {
                        // This should never happen, but just in case it does...
                        Application.PrintErrorMessage(ErrorMessages.NoOutputFileSpecified, null);
                        result = false;
                    }
                }
            }

            // Ensure the output file is writeable
            // (The test fails if the reader/writer libraries locks the 
            // files after opening it, and that may be the case with 
            // ManifestUtil if the file exists but is invalid (zero length).)
            if (result && (outputPath != null))
            {
                FileStream stream = null;
                bool delete = false;

                // If the file doesn't exist yet, this test will leave behind
                // a zero-length file, which will be deleted after the test.
                if (!File.Exists(outputPath))
                {
                    delete = true;
                }

                try
                {
                    stream = File.OpenWrite(outputPath);
                }
                catch (System.Exception)
                {
                    Application.PrintErrorMessage(ErrorMessages.FileNotWriteable, outputPath);
                    result = false;
                }

                if (stream != null)
                {
                    stream.Close();

                    if (delete)
                    {
                        File.Delete(outputPath);
                    }
                }
            }
            else
            {
                // This can only happen if the user supplied an unsupported command,
                // e.g. "-new appliance"
                outputPath = "";
            }

            // Check the format of the version option, if given
            if (applicationVersionString != null)
            {
                try
                {
                    applicationVersion = new Version(applicationVersionString);
                }
                catch (System.Exception)
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidVersion, applicationVersionString);
                }
            }

            // The default is sha256
            algorithm = DigestAlgorithmValue.sha256RSA;

            if (digestAlgorithmValue != null)
            {
                // Supporting only sha256
                if (digestAlgorithmValue.Equals(DigestAlgorithmValue.sha256RSA.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    algorithm = DigestAlgorithmValue.sha256RSA;
                }
                else
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidAlgorithmValue, digestAlgorithmValue);
                }
            }

            // Validate the processor type, if given
            if (processorString != null)
            {
                try
                {
                    processor = (Processors)Enum.Parse(typeof(Processors), processorString, true);
                    if (processor == Processors.Undefined)
                    {
                        throw new System.Exception();
                    }
                }
                catch (System.Exception)
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidProcessor, processorString);
                }
            }

            // Validate the source directory, if given
            if (fromDirectory != null)
            {
                if (!Directory.Exists(fromDirectory))
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidDirectory, fromDirectory);
                }
            }

            // Validate the trust level, if given
            if (trustLevelString != null)
            {
#if RUNTIME_TYPE_NETCORE
                result = false;
                Application.PrintErrorMessage(ErrorMessages.TrustLevelsNotSupportedOnNETCore);
#else
                trustLevelString = trustLevelString.ToLower(CultureInfo.InvariantCulture);

                if (trustLevelString == TrustLevels.Internet.ToString().ToLower(CultureInfo.InvariantCulture))
                {
                    trustLevel = TrustLevels.Internet;
                }
                else if (trustLevelString == TrustLevels.LocalIntranet.ToString().ToLower(CultureInfo.InvariantCulture))
                {
                    trustLevel = TrustLevels.LocalIntranet;
                }
                else if (trustLevelString == TrustLevels.FullTrust.ToString().ToLower(CultureInfo.InvariantCulture))
                {
                    trustLevel = TrustLevels.FullTrust;
                }
                else
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidTrustLevel, trustLevelString);
                }
#endif
            }

            // Validate the application code base, if given
            if (applicationCodeBase != null)
            {
                if (!Utilities.Misc.IsValidCodebase(applicationCodeBase))
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidCodebase, applicationCodeBase);
                    result = false;
                }
            }

            // Validate the application provider URL, if given
            if (applicationProviderUrl != null)
            {
                if (!Utilities.Misc.IsValidUrl(applicationProviderUrl))
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidUrl, applicationProviderUrl);
                    result = false;
                }
            }

            // Validate the RequiredUpdate option, if given
            if (isRequiredUpdateString != null)
            {
                if (isRequiredUpdateString.ToLower(CultureInfo.InvariantCulture) == "none")
                {
                    minVersion = string.Empty;
                }
                else
                {
                    try
                    {
                        Version v = new Version(isRequiredUpdateString);
                        minVersion = isRequiredUpdateString;

                        // Specifying minimum version implies that the app will get installed.
                        // We will set 'install' to true, otherwise, app will not check for future updates.
                        // Install value can still be overridden on command line, see next code block.
                        install = TriStateBool.True;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Application.PrintErrorMessage(ErrorMessages.InvalidMinVersion, minVersion);
                        result = false;
                    }

                }
            }

            // Validate the Install option, if given
            if (installString != null)
            {
                switch (installString.ToLower(CultureInfo.InvariantCulture))
                {
                    case "true":
                    case "t":
                        install = TriStateBool.True;
                        break;

                    case "false":
                    case "f":
                        install = TriStateBool.False;
                        break;

                    default:
                        result = false;
                        Application.PrintErrorMessage(ErrorMessages.InvalidInstall, installString);
                        break;
                }
            }

            // Validate the IncludeProviderURL option, if given
            if (includeDeploymentProviderUrlString != null)
            {
                switch (includeDeploymentProviderUrlString.ToLower(CultureInfo.InvariantCulture))
                {
                    case "true":
                    case "t":
                        includeDeploymentProviderUrl = TriStateBool.True;
                        break;

                    case "false":
                    case "f":
                        includeDeploymentProviderUrl = TriStateBool.False;
                        break;

                    default:
                        result = false;
                        Application.PrintErrorMessage(ErrorMessages.InvalidIncludeProviderURL, includeDeploymentProviderUrlString);
                        break;
                }

                if (includeDeploymentProviderUrl == TriStateBool.True &&
                    string.IsNullOrEmpty(applicationProviderUrl))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingDeploymentProviderUrl);
                    result = false;
                }
            }

            // Validate the UseManifestForTrust option, if given
            if (useApplicationManifestForTrustInfoString != null)
            {
                switch (useApplicationManifestForTrustInfoString.ToLower(CultureInfo.InvariantCulture))
                {
                    case "true":
                    case "t":
                        useApplicationManifestForTrustInfo = TriStateBool.True;
                        break;

                    case "false":
                    case "f":
                        useApplicationManifestForTrustInfo = TriStateBool.False;
                        break;

                    default:
                        result = false;
                        Application.PrintErrorMessage(ErrorMessages.InvalidUseManifestForTrust, useApplicationManifestForTrustInfoString);
                        break;
                }
            }

            // Validate the TrustUrlParameters option, if given
            if (trustUrlParametersString != null)
            {
                switch (trustUrlParametersString.ToLower(CultureInfo.InvariantCulture))
                {
                    case "true":
                    case "t":
                        trustUrlParameters = TriStateBool.True;
                        break;

                    case "false":
                    case "f":
                        trustUrlParameters = TriStateBool.False;
                        break;

                    default:
                        result = false;
                        Application.PrintErrorMessage(ErrorMessages.InvalidTrustURLParameters, trustUrlParametersString);
                        break;
                }
            }

            if ((Requested(Operations.GenerateApplicationManifest) || Requested(Operations.UpdateApplicationManifest)) &&
                useApplicationManifestForTrustInfo != TriStateBool.True)
            {
                if (!string.IsNullOrEmpty(publisherName))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingUseApplicationManifestForTrustInfo, "Publisher");
                    result = false;
                }
                if (!string.IsNullOrEmpty(supportUrl))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingUseApplicationManifestForTrustInfo, "SupportURL");
                    result = false;
                }
            }

            if (Requested(Operations.AddLauncher))
            {
                if (string.IsNullOrEmpty(targetDirectory))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingAddLauncherOption, "TargetDirectory");
                    result = false;
                }
            }

            //Return true if the command line was all good, false if anything wasn't right
            return result;
        }

        public bool CanExecute()
        {
            if (operations == 0)
            {
                Application.PrintErrorMessage(ErrorMessages.NoVerb);
                return false;
            }

            if (Requested(Operations.ShowHelp) ||
                Requested(Operations.ShowVerboseHelp) ||
                Requested(Operations.LaunchGUI) ||
                Requested(Operations.VerifyManifest) ||
                Requested(Operations.CleanApplicationCache))
            {
                return true;
            }
            else
            {
                // continue with the execution process
                return CanExecuteInner();
            }
        }

        /// <summary>
        /// Check for parameters that are not valid for the given operation
        /// </summary>
        private bool CheckForInvalidParameters()
        {
            int errors = 0;

            if (Requested(Operations.GenerateApplicationManifest) ||
                Requested(Operations.UpdateApplicationManifest) ||
                Requested(Operations.AddLauncher))
            {
                errors += CheckForFileTypeSpecificOption("Deployment", "AppManifest", applicationManifestPath);
                errors += CheckForFileTypeSpecificOption("Deployment", "AppCodeBase", applicationCodeBase);
                errors += CheckForFileTypeSpecificOption("Deployment", "AppProviderUrl", applicationProviderUrl);
                errors += CheckForFileTypeSpecificOption("Deployment", "RequiredUpdate", isRequiredUpdateString);
                errors += CheckForFileTypeSpecificOption("Deployment", "Install", installString);
                errors += CheckForFileTypeSpecificOption("Deployment", "IncludeProviderUrl", includeDeploymentProviderUrlString);
                errors += CheckForFileTypeSpecificOption("Deployment", "TrustUrlParameters", trustUrlParametersString);
            }

            if (Requested(Operations.GenerateDeploymentManifest) ||
                Requested(Operations.UpdateDeploymentManifest) ||
                Requested(Operations.AddLauncher))
            {
                errors += CheckForFileTypeSpecificOption("Application", "FromDirectory", fromDirectory);
                errors += CheckForFileTypeSpecificOption("Application", "TrustLevel", trustLevelString);
                errors += CheckForFileTypeSpecificOption("Application", "IconFile", iconFile);
            }

            if (Requested(Operations.AddLauncher))
            {
                errors += CheckForInvalidLauncherOption("ToFile", outputPath);
                errors += CheckForInvalidLauncherOption("CertFile", certPath);
                errors += CheckForInvalidLauncherOption("CertHash", certHash);
                errors += CheckForInvalidLauncherOption("Password", certPassword);
                errors += CheckForInvalidLauncherOption("TimeStampUri", timestamp);
                errors += CheckForInvalidLauncherOption("KeyContainer", keyContainer);
                errors += CheckForInvalidLauncherOption("CryptoProvider", cryptoProviderName);
                errors += CheckForInvalidLauncherOption("Publisher", publisherName);
                errors += CheckForInvalidLauncherOption("SupportUrl", supportUrl);
                errors += CheckForInvalidLauncherOption("Algorithm", digestAlgorithmValue);
            }

            // A signing operation can be caused by the -sign verb, or with 
            // -keyfile, -certhash, etc.  The following tests are applicable
            // when the verb is given, but not when the options are given.
            // The same options are not applicable when adding launcher.
            if (signVerb || Requested(Operations.AddLauncher))
            {
                errors += CheckForInvalidNonManifestOption("Version", applicationVersionString);
                errors += CheckForInvalidNonManifestOption("Name", applicationName);
                errors += CheckForInvalidNonManifestOption("Processor", processorString);
                errors += CheckForInvalidNonManifestOption("FromDirectory", fromDirectory);
                errors += CheckForInvalidNonManifestOption("TrustLevel", trustLevelString);
                errors += CheckForInvalidNonManifestOption("AppManifest", applicationManifestPath);
                errors += CheckForInvalidNonManifestOption("AppCodeBase", applicationCodeBase);
                errors += CheckForInvalidNonManifestOption("AppProviderUrl", applicationProviderUrl);
                errors += CheckForInvalidNonManifestOption("RequiredUpdate", isRequiredUpdateString);
                errors += CheckForInvalidNonManifestOption("IncludeProviderURL", includeDeploymentProviderUrlString);
                errors += CheckForInvalidNonManifestOption("UseManifestForTrust", useApplicationManifestForTrustInfoString);
                errors += CheckForInvalidNonManifestOption("TrustUrlParameters", trustUrlParametersString);
                errors += CheckForInvalidNonManifestOption("IconFile", iconFile);
            }

            if (!Requested(Operations.AddLauncher))
            {
                errors += CheckForInvalidNonLauncherOption("TargetDirectory", targetDirectory);
            }

            return errors == 0;
        }

        int CheckForFileTypeSpecificOption(string manifestType, string name, string value)
        {
            if (value != null)
            {
                Application.PrintErrorMessage(ErrorMessages.FileTypeSpecificOption, name, manifestType);
                return 1;
            }

            return 0;
        }

        int CheckForInvalidLauncherOption(string name, string value)
        {
            if (value != null)
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidAddLauncherOption, name);
                return 1;
            }

            return 0;
        }

        int CheckForInvalidNonManifestOption(string name, string value)
        {
            if (value != null)
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidNonManifestOption, name);
                return 1;
            }

            return 0;
        }

        int CheckForInvalidNonLauncherOption(string name, string value)
        {
            if (value != null)
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidNonAddLauncherOption, name);
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Checks if Update is possible.
        /// </summary>
        /// <returns>true if an update operation is possible, false otherwise</returns>
        private bool CanUpdate()
        {
            bool result = true;

            // Make sure an input file was specified and exists
            if (!EnsureInputFile())
            {
                return false;
            }

            // Open the file that is to be updated, make sure it's a 
            // manifest of some sort.  Once opened, the manifest will
            // be cached for later use so it won't have to be opened
            // again during Execute().
            if (!OpenAndCacheManifestInputFile(inputPath))
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidInputFile, inputPath);
                result = false;
            }
            else
            {
                if (cachedAppManifest != null)
                {
                    operations |= Operations.UpdateApplicationManifest;
                }

                if (cachedDepManifest != null)
                {
                    operations |= Operations.UpdateDeploymentManifest;
                }
            }

            return result;
        }

        /// <summary>
        /// Make sure an input file was specified and exists
        /// </summary>
        /// <returns></returns>
        private bool EnsureInputFile()
        {
            // Make sure the input file was specified
            if (inputPath == null)
            {
                if (Requested(Operations.UpdateSomething))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingArgument, "update");
                }
                else if (Requested(Operations.SignSomething))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingArgument, "sign");
                }
                else if (Requested(Operations.VerifyManifest))
                {
                    Application.PrintErrorMessage(ErrorMessages.MissingArgument, "verify");
                }
                else
                {
                    throw new System.Exception(string.Empty);
                }

                return false;
            }
            else
            {
                // Normalize the input path
                inputPath = Path.GetFullPath(inputPath);
            }

            // Make sure the input file exists    
            if (!File.Exists(inputPath))
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidPath, inputPath);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if we can sign.
        /// </summary>
        /// <returns>true if a signing operation is possible, false otherwise</returns>
        private bool CanSign()
        {
            bool result = true;

            // Signing is only supported on Windows.
            if (!OperatingSystem.IsWindows())
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidSigningPlatform);
                return false;
            }

            if (Requested(Operations.GenerateSomething) == false)
            {
                // Make sure an input file was specified and exists
                if (!EnsureInputFile())
                {
                    return false;
                }

                if (!OpenAndCacheManifestInputFile(inputPath))
                {
                    Application.PrintErrorMessage(ErrorMessages.InvalidInputFile, inputPath);
                    return false;
                }
            }

            // Set a more specific operation flag after deducing exactly what 
            // sort of file is to be signed, and cache the file to operate on

            if (Requested(Operations.GenerateApplicationManifest) || (cachedAppManifest != null))
            {
                operations |= Operations.SignApplicationManifest;
            }

            if (Requested(Operations.GenerateDeploymentManifest) || (cachedDepManifest != null))
            {
                operations |= Operations.SignDeploymentManifest;
            }

            // Look for a signing key
            int signingKeySpecs = 0;

            if (certPath != null)
                signingKeySpecs++;

            if (certHash != null)
                signingKeySpecs++;

            if (timestamp != null)
            {
                Uri stamp = null;
                try
                {
                    stamp = new Uri(timestamp);
                }
                catch (UriFormatException) { }

                if (stamp == null)
                {
                    result = false;
                    Application.PrintErrorMessage(ErrorMessages.InvalidTimestamp);
                }
            }

            if (signingKeySpecs < 1)
            {
                result = false;
                Application.PrintErrorMessage(ErrorMessages.NoKeySpecified);
            }
            else if (signingKeySpecs > 1)
            {
                result = false;
                Application.PrintErrorMessage(ErrorMessages.TooManyKeysSpecified);
            }
            else
            {
                if (certPath != null)
                {
                    if (File.Exists(certPath))
                    {
                        try
                        {
                            X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
                            result = Utilities.Certificate.CanSignWith(cert);
                            if (!result)
                            {
                                Application.PrintErrorMessage(ErrorMessages.InvalidCertUsage, certPath);
                            }
#if RUNTIME_TYPE_NETCORE
                            // SetPrivateKeyIfNeeded API is only available on Windows
                            else if (OperatingSystem.IsWindows())
#else
                            else
#endif
                            {
                                result = Utilities.Certificate.SetPrivateKeyIfNeeded(cert, cryptoProviderName, keyContainer);
                                if (!result)
                                {
                                    Application.PrintErrorMessage(ErrorMessages.MissingCspOrContainer, certPath);
                                }
                                else
                                {
                                    storedCert = cert;
                                }
                            }
                        }
                        catch (System.Security.Cryptography.CryptographicException ex)
                        {
                            Application.PrintErrorMessage(ErrorMessages.UnableToOpenCertificate, certPath);

                            if (ex.Message.EndsWith(Environment.NewLine))
                            {
                                Console.Write(ex.Message);
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                            }

                            result = false;
                        }
                    }
                    else
                    {
                        Application.PrintErrorMessage(ErrorMessages.InvalidPath, certPath);
                        result = false;
                    }
                }

                if (certHash != null)
                {
                    certHash = certHash.ToLower();
                    certHash = certHash.Replace(" ", string.Empty);

                    using (X509Store store = new X509Store("MY", StoreLocation.CurrentUser))
                    {
                        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                        X509Certificate2Collection allCerts = store.Certificates;

                        foreach (X509Certificate2 cert in allCerts)
                        {
                            string thisCertHash = cert.GetCertHashString().ToLower();

                            if (thisCertHash == certHash)
                            {
                                if (result = ValidateCertificate(cert, certHash))
                                {
                                    storedCert = cert;
                                }
                            }
                        }
                    }
                    if (storedCert == null)
                    {
                        // Specified cert hash is invalid
                        result = false;
                        Application.PrintErrorMessage(ErrorMessages.InvalidCertUsage, certHash);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Indicates whether or not the given cert can be used to sign.
        /// </summary>
        /// <param name="cert"></param>
        private static bool ValidateCertificate(X509Certificate2 cert, string hashOrPath)
        {
            bool result = true;

            if (!Utilities.Certificate.CanSignWith(cert))
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidCertUsage, hashOrPath);
                result = false;
            }

            if (!Utilities.Certificate.HasPrivateKey(cert))
            {
                Application.PrintErrorMessage(ErrorMessages.InvalidCertNoPrivateKey, hashOrPath);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Opens and caches the input file.
        /// </summary>
        /// <returns>true if this.inputPath is a manifest file, false otherwise</returns>
        private bool OpenAndCacheManifestInputFile(string path)
        {
            bool result = false;

            try
            {
                Manifest manifest = ManifestReader.ReadManifest(path, true);

                if (manifest != null)
                {
                    if (manifest is ApplicationManifest)
                    {
                        cachedAppManifest = manifest as ApplicationManifest;
                        result = true;
                    }
                    else if (manifest is DeployManifest)
                    {
                        cachedDepManifest = manifest as DeployManifest;
                        result = true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Application.PrintOutputMessage(e.Message, string.Empty);
                result = false;
            }

            return result;
        }

#if !RUNTIME_TYPE_NETCORE
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
#endif
        public void ExecuteManifestRelated()
        {
            Manifest manifest = null;

            // Build a list of files to ignore when working with application manifests
            List<string> filesToIgnore = new List<string>();
            if (inputPath != null)
            {
                filesToIgnore.Add(inputPath.ToLower());
            }

            if (outputPath != null)
            {
                filesToIgnore.Add(outputPath.ToLower());
            }

            string nameArgument = applicationName;
            if (Requested(Operations.GenerateSomething))
            {
                // If an application name wasn't supplied, default it
                if (applicationName == null)
                {
                    string fileName = Path.GetFileName(outputPath);
                    int dot = fileName.IndexOf('.');
                    if (dot == -1)
                    {
                        applicationName = fileName;
                    }
                    else
                    {
                        applicationName = fileName.Substring(0, dot);
                    }
                }
            }

            // Choose hashing algorithm - SHA256 for v4.5, which is the only supported algorithm now.
            string targetFrameworkVersion = "v4.0";
            if (algorithm == DigestAlgorithmValue.sha256RSA)
            {
                targetFrameworkVersion = "v4.5";
            }

            // Generate operations
            if (Requested(Operations.GenerateApplicationManifest))
            {
                applicationName += ".exe";
                manifest = Mage.GenerateApplicationManifest(filesToIgnore, nameArgument, applicationName, applicationVersion, processor, trustLevel, fromDirectory, iconFile, useApplicationManifestForTrustInfo, publisherName, supportUrl, targetFrameworkVersion);
            }
            else if (Requested(Operations.GenerateDeploymentManifest))
            {
                applicationName += ".app";
                manifest = Mage.GenerateDeploymentManifest(outputPath, applicationName, applicationVersion, processor, cachedAppManifest, applicationManifestPath, applicationCodeBase, applicationProviderUrl, minVersion, install, includeDeploymentProviderUrl, publisherName, supportUrl, targetFrameworkVersion, trustUrlParameters);
            }

            // Update operations
            if (Requested(Operations.UpdateApplicationManifest))
            {
                Mage.UpdateApplicationManifest(filesToIgnore, cachedAppManifest, nameArgument, applicationName, applicationVersion, processor, trustLevel, fromDirectory, iconFile, useApplicationManifestForTrustInfo, publisherName, supportUrl, targetFrameworkVersion);
                manifest = cachedAppManifest;
            }
            else if (Requested(Operations.UpdateDeploymentManifest))
            {
                Mage.UpdateDeploymentManifest(cachedDepManifest, outputPath, applicationName, applicationVersion, processor, cachedAppManifest, applicationManifestPath, applicationCodeBase, applicationProviderUrl, minVersion, install, includeDeploymentProviderUrl, publisherName, supportUrl, targetFrameworkVersion, trustUrlParameters);
                manifest = cachedDepManifest;
            }

            // Sign operations
            bool shouldSign = false;
            if (Requested(Operations.SignApplicationManifest))
            {
                // If a manifest is already open, use it...
                // if not and the filenames are different, copy the file then sign it.
                if (manifest == null && inputPath != outputPath)
                {
                    try
                    {
                        File.Copy(inputPath, outputPath, true);
                        shouldSign = true;
                    }
                    catch
                    {
                        Application.PrintErrorMessage(ErrorMessages.InvalidPath, outputPath);
                    }
                }
                else
                {
                    shouldSign = true;
                }
            }
            else if (Requested(Operations.SignDeploymentManifest))
            {
                if (manifest == null && inputPath != outputPath)
                {
                    try
                    {
                        File.Copy(inputPath, outputPath, true);
                        shouldSign = true;
                    }
                    catch
                    {
                        Application.PrintErrorMessage(ErrorMessages.InvalidPath, outputPath);
                    }
                }
                else
                {
                    shouldSign = true;
                }
            }

            // Save the manifest or license
            if (manifest != null)
            {
                // Framework version is used for determining which algorithm to use for
                // generating manifest digests. For v4.5+, sha256 is used.
                string frameworkVersion = "v4.5";
                ManifestWriter.WriteManifest(manifest, outputPath, frameworkVersion);
            }

            if (shouldSign)
            {
                Uri stamp = null;
                if (!string.IsNullOrEmpty(timestamp))
                {
                    try
                    {
                        stamp = new Uri(timestamp);
                    }
                    catch (UriFormatException) { }
                }
                Debug.Assert(storedCert != null);

                SecurityUtilities.SignFile(storedCert, stamp, outputPath);
            }

            Validate(manifest);
        }

        /// <summary>
        /// Executes whatever operations are indicated in this.operations
        /// </summary>
        public void Execute()
        {
            // Simple operations
            if (Requested(Operations.ShowHelp))
            {
                Application.PrintHelpMessage();
                return;
            }
            else if (Requested(Operations.ShowVerboseHelp))
            {
                Application.PrintVerboseHelpMessage();
                return;
            }
            else if (Requested(Operations.CleanApplicationCache))
            {
                try
                {
                    CleanOnlineAppCache();
                    Application.PrintOutputMessage("", Application.Resources.GetString("ApplicationCacheCleared"));
                }
                catch (Exception)
                {
                    Application.PrintOutputMessage(Application.Resources.GetString("ErrorMessage") + ":", Application.Resources.GetString("ErrorApplicactionCachedCleared"));
                }
                
                return;
            }
            else if (Requested(Operations.VerifyManifest))
            {
                ExecuteManifestVerification();
                return;
            }
            else if (Requested(Operations.AddLauncher))
            {
                if (LauncherUtil.AddLauncher(targetDirectory, binaryToLaunch))
                {
                    Application.PrintOutputMessage("", Application.Resources.GetString("LauncherSuccessfullyAdded"));
                }
                return;
            }

            ExecuteManifestRelated();
        }

        /// <summary>
        /// This method will verify a manifest passed into it.
        /// </summary>
        [SuppressMessage("Microsoft.Security.Xml", "CA3053:UseXmlSecureResolver", Justification = "Same parameters are set in trust manager validation code.")]
        [SuppressMessage("Microsoft.Security.Xml", "CA3069:ReviewDtdProcessingAssignment", Justification = "Same parameters are set in trust manager validation code.")]
        [SuppressMessage("Microsoft.Security.Xml", "CA3056:UseXmlReaderForLoad", Justification = "Same parameters are set in trust manager validation code.")]
        private void ExecuteManifestVerification()
        {
            if (!EnsureInputFile())
            {
                // EnsureInputFile prints out the error message
                return;
            }

            // Store valid signature here.
            bool validSignature = false;
            bool validTimestamp = false;

            CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription),
                Sha256SignatureMethodUri);

#if RUNTIME_TYPE_NETCORE
#pragma warning disable SYSLIB0021
            CryptoConfig.AddAlgorithm(typeof(SHA256Managed),
                Sha256DigestMethod);
#pragma warning restore SYSLIB0021
#else
            CryptoConfig.AddAlgorithm(typeof(SHA256Cng),
                Sha256DigestMethod);
#endif

            try
            {
                // Load the manifest as an XML file.
                XmlDocument xmlDoc = new XmlDocument
                {
                    PreserveWhitespace = true
                };
                xmlDoc.Load(inputPath);
                using (StreamReader streamReader = new StreamReader(inputPath))
                {
                    XmlReaderSettings settings = new XmlReaderSettings
                    {
                        DtdProcessing = DtdProcessing.Parse
                    };
                    using (XmlReader reader = XmlReader.Create(streamReader, settings, (new UriBuilder(inputPath)).ToString()))
                    {
                        xmlDoc.Load(reader);
                    }
                }

                // Set up namespace manager.
                XmlNamespaceManager nsm = new XmlNamespaceManager(xmlDoc.NameTable);
                nsm.AddNamespace("asm", NamespaceAssemblyUri);
                nsm.AddNamespace("asm2", NamespaceAssemblyV2Uri);
                nsm.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
                nsm.AddNamespace("msrel", NamespaceMsrelUri);
                nsm.AddNamespace("r", NamespaceLicenseUri);
                nsm.AddNamespace("as", NamespaceAuthenticodeUri);

                // Timestamp object node.
                XmlElement tsNode = xmlDoc.SelectSingleNode("asm:assembly/ds:Signature/ds:KeyInfo/msrel:RelData/r:license/r:issuer/ds:Signature/ds:Object/as:Timestamp", nsm) as XmlElement;
                if (tsNode != null)
                {
                    // Check the timestamp of the XML.
                    // Get the bytes from the base64 string in the XML.
                    byte[] tsbytes = Convert.FromBase64String(tsNode.InnerText);
                    if (tsbytes != null)
                    {
                        SignedCms sd = new SignedCms();
                        sd.Decode(tsbytes);

                        try
                        {
                            foreach (var infos in sd.SignerInfos)
                            {
                                infos.CheckSignature(true);
                            }
                            validTimestamp = true;
                        }
                        catch (Exception ex)
                        {
                            if (Utilities.Misc.IsCriticalException(ex))
                            {
                                throw;
                            }

                            Application.PrintOutputMessage(inputPath, ex.Message);
                        }
                    }
                    else
                    {
                        Application.PrintOutputMessage(inputPath, Application.Resources.GetString("ErrorInvalidTimestampFormat"));
                    }
                }
                else
                {
                    validTimestamp = true;
                }

                // Load signed manifest file.
                SignedXml signedXml = new SignedXml(xmlDoc);
                XmlElement strongNameSignatureNode = xmlDoc.SelectSingleNode("asm:assembly/ds:Signature", nsm) as XmlElement;
                if (strongNameSignatureNode == null)
                {
                    Application.PrintOutputMessage(inputPath, Application.Resources.GetString("ErrorMissingSignatureNode"));
                }
                else
                {
                    // Get the node with the signature.
                    signedXml.LoadXml(strongNameSignatureNode);

                    // Verify the signature using public key in the signature.
                    validSignature = signedXml.CheckSignature();
                }
            }
            catch (Exception ex)
            {
                if (Utilities.Misc.IsCriticalException(ex))
                {
                    throw;
                }

                Application.PrintOutputMessage(string.Empty, ex.Message);
            }

            if (!validTimestamp || !validSignature)
            {
                Application.PrintOutputMessage(string.Empty, Application.Resources.GetString("ErrorInvalidSignature"));
            }
            else
            {
                Application.PrintOutputMessage(string.Empty, Application.Resources.GetString("SignatureValidatedSuccessfully"));
            }
        }

        private void Validate(Manifest manifest)
        {
            bool bOutputMessages = false;
            int suppressedWarningMessageCount = 0;

            // Validates the output. We will usually get warnings in the
            // command line version.
            if (manifest != null)
            {
                if (manifest is ApplicationManifest)
                    (manifest as ApplicationManifest).MaxTargetPath = Utilities.Constants.MAXTARGETPATH;

                manifest.Validate();

                bool launcherBasedDeployment =
                    string.Equals(manifest.EntryPoint.TargetPath.ToLower(), LauncherUtil.LauncherFilename.ToLower(), StringComparison.OrdinalIgnoreCase);

                // Prints out all the information.
                foreach (OutputMessage msg in manifest.OutputMessages)
                {
                    // Suppress expected warning messages
                    if (msg.Type == OutputMessageType.Warning &&
                        ShouldSuppressWarningMessage(msg, launcherBasedDeployment))
                    {
                        suppressedWarningMessageCount++;
                        continue;
                    }

                    string resultMessage;
                    switch (msg.Type)
                    {
                        case OutputMessageType.Error:
                            resultMessage = Application.Resources.GetString("ErrorMessage");
                            break;
                        case OutputMessageType.Info:
                            resultMessage = Application.Resources.GetString("InfoMessage");
                            break;
                        default:
                            resultMessage = Application.Resources.GetString("WarningMessage");
                            break;
                    }

                    Console.WriteLine(resultMessage + " " + msg.Text);
                }

                bOutputMessages = manifest.OutputMessages.ErrorCount + manifest.OutputMessages.WarningCount - suppressedWarningMessageCount > 0;
            }

            string OutputText = null;
            // Prints out the results Message
            if ((operations & Operations.GenerateSomething) != 0)
                OutputText = Application.Resources.GetString("ResultSuccessfullyCreated");

            if ((operations & Operations.UpdateSomething) != 0)
                OutputText = Application.Resources.GetString("ResultSuccessfullyUpdated");

            if ((operations & Operations.SignSomething) != 0)
                OutputText = Application.Resources.GetString("ResultSuccessfullySigned");

            if (OutputText != null)
            {
                if (bOutputMessages)
                    Console.WriteLine();

                Application.PrintOutputMessage(Path.GetFileName(outputPath), OutputText + (bOutputMessages ? ". " + Application.Resources.GetString("ResultSomeErrorsEncountered") : ""));
            }
        }

        private bool ShouldSuppressWarningMessage(OutputMessage msg, bool launcherBasedDeployment)
        {
            if (launcherBasedDeployment &&
                (msg.Name == "GenerateManifest.AssemblyAsFile"))
            {
                // Suppress all AssemblyAsFile warnings for launcher-based deployments.
                //
                // Deployment of .NET (Core) applications includes all .NET (Core) files
                // as simple files, not assembly references. Entry point of deployment is Launcher
                // which is a .NET FX application, as ClickOnce runtime requires
                // a managed EXE as the entry point.

                return true;
            }

            return false;
        }
    }
}
