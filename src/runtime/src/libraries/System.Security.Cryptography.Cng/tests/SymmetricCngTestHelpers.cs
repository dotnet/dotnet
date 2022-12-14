// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Security.Principal;
using Xunit;

namespace System.Security.Cryptography.Cng.Tests
{
    public static class SymmetricCngTestHelpers
    {
        // The ephemeral key has already been validated by the AesCipherTests suite.
        // Therefore we can use the ephemeral key to validate the persisted key.
        internal static void VerifyPersistedKey(
            CngAlgorithm algorithm,
            int keySize,
            int plainBytesCount,
            Func<string, SymmetricAlgorithm> persistedFunc,
            Func<SymmetricAlgorithm> ephemeralFunc,
            CipherMode cipherMode,
            PaddingMode paddingMode,
            int feedbackSizeInBits)
        {
            string keyName = Guid.NewGuid().ToString();
            CngKeyCreationParameters creationParameters = new CngKeyCreationParameters
            {
                Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
                ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                Parameters =
                {
                    new CngProperty("Length", BitConverter.GetBytes(keySize), CngPropertyOptions.None),
                }
            };

            CngKey cngKey = CngKey.Create(algorithm, keyName, creationParameters);

            try
            {
                VerifyPersistedKey(
                    keyName,
                    plainBytesCount,
                    persistedFunc,
                    ephemeralFunc,
                    cipherMode,
                    paddingMode,
                    feedbackSizeInBits);
            }
            finally
            {
                // Delete also Disposes the key, no using should be added here.
                cngKey.Delete();
            }
        }

        internal static void VerifyPersistedKey(
            string keyName,
            int plainBytesCount,
            Func<string, SymmetricAlgorithm> persistedFunc,
            Func<SymmetricAlgorithm> ephemeralFunc,
            CipherMode cipherMode,
            PaddingMode paddingMode,
            int feedbackSizeInBits)
        {
            byte[] plainBytes = RandomNumberGenerator.GetBytes(plainBytesCount);

            using (SymmetricAlgorithm persisted = persistedFunc(keyName))
            using (SymmetricAlgorithm ephemeral = ephemeralFunc())
            {
                persisted.Mode = ephemeral.Mode = cipherMode;
                persisted.Padding = ephemeral.Padding = paddingMode;

                if (cipherMode == CipherMode.CFB)
                {
                    persisted.FeedbackSize = ephemeral.FeedbackSize = feedbackSizeInBits;
                }

                ephemeral.Key = persisted.Key;
                ephemeral.GenerateIV();
                persisted.IV = ephemeral.IV;

                using (ICryptoTransform persistedEncryptor = persisted.CreateEncryptor())
                using (ICryptoTransform persistedDecryptor = persisted.CreateDecryptor())
                using (ICryptoTransform ephemeralEncryptor = ephemeral.CreateEncryptor())
                {
                    Assert.True(
                        persistedEncryptor.CanTransformMultipleBlocks,
                        "Pre-condition: persistedEncryptor.CanTransformMultipleBlocks");

                    byte[] persistedEncrypted = persistedEncryptor.TransformFinalBlock(plainBytes, 0, plainBytesCount);
                    byte[] ephemeralEncrypted = ephemeralEncryptor.TransformFinalBlock(plainBytes, 0, plainBytesCount);

                    Assert.Equal(ephemeralEncrypted, persistedEncrypted);

                    byte[] cipherBytes = persistedEncrypted;
                    byte[] persistedDecrypted = persistedDecryptor.TransformFinalBlock(cipherBytes, 0,
                        cipherBytes.Length);

                    byte[] expectedBytes = plainBytes;

                    if (persistedDecrypted.Length > plainBytes.Length)
                    {
                        // This should only ever happen in
                        Assert.Equal(PaddingMode.Zeros, paddingMode);

                        expectedBytes = new byte[persistedDecrypted.Length];
                        Buffer.BlockCopy(plainBytes, 0, expectedBytes, 0, plainBytesCount);
                    }

                    Assert.Equal(expectedBytes, persistedDecrypted);
                }

                byte[] oneShotPersistedEncrypted = null;
                byte[] oneShotEphemeralEncrypted = null;
                byte[] oneShotPersistedDecrypted = null;

                if (cipherMode == CipherMode.ECB)
                {
                    oneShotPersistedEncrypted = persisted.EncryptEcb(plainBytes, paddingMode);
                    oneShotEphemeralEncrypted = ephemeral.EncryptEcb(plainBytes, paddingMode);
                    oneShotPersistedDecrypted = persisted.DecryptEcb(oneShotEphemeralEncrypted, paddingMode);
                }
                else if (cipherMode == CipherMode.CBC)
                {
                    oneShotPersistedEncrypted = persisted.EncryptCbc(plainBytes, persisted.IV, paddingMode);
                    oneShotEphemeralEncrypted = ephemeral.EncryptCbc(plainBytes, ephemeral.IV, paddingMode);
                    oneShotPersistedDecrypted = persisted.DecryptCbc(oneShotEphemeralEncrypted, persisted.IV, paddingMode);
                }
                else if (cipherMode == CipherMode.CFB)
                {
                    oneShotPersistedEncrypted = persisted.EncryptCfb(plainBytes, persisted.IV, paddingMode, feedbackSizeInBits);
                    oneShotEphemeralEncrypted = ephemeral.EncryptCfb(plainBytes, ephemeral.IV, paddingMode, feedbackSizeInBits);
                    oneShotPersistedDecrypted = persisted.DecryptCfb(oneShotEphemeralEncrypted, persisted.IV, paddingMode, feedbackSizeInBits);
                }

                if (oneShotPersistedEncrypted is not null)
                {
                    Assert.Equal(oneShotEphemeralEncrypted, oneShotPersistedEncrypted);

                    if (paddingMode == PaddingMode.Zeros)
                    {
                        byte[] plainPadded = new byte[oneShotPersistedDecrypted.Length];
                        plainBytes.AsSpan().CopyTo(plainPadded);
                        Assert.Equal(plainPadded, oneShotPersistedDecrypted);
                    }
                    else
                    {
                        Assert.Equal(plainBytes, oneShotPersistedDecrypted);
                    }
                }
            }
        }

        public static void GetKey_NonExportable(
            CngAlgorithm algorithm,
            Func<string, SymmetricAlgorithm> persistedFunc)
        {
            string keyName = Guid.NewGuid().ToString();
            CngKey cngKey = CngKey.Create(algorithm, keyName);

            try
            {
                using (SymmetricAlgorithm persisted = persistedFunc(keyName))
                {
                    Assert.ThrowsAny<CryptographicException>(() => persisted.Key);
                }
            }
            finally
            {
                // Delete also Disposes the key, no using should be added here.
                cngKey.Delete();
            }
        }

        public static void SetKey_DetachesFromPersistedKey(
            CngAlgorithm algorithm,
            Func<string, SymmetricAlgorithm> persistedFunc)
        {
            // This test verifies that:
            // * [Algorithm]Cng.set_Key does not change the persisted key value
            // * [Algorithm]Cng.GenerateKey is "the same" as set_Key
            // * ICryptoTransform objects opened before the change do not react to it

            string keyName = Guid.NewGuid().ToString();
            CngKey cngKey = CngKey.Create(algorithm, keyName);

            try
            {
                using (SymmetricAlgorithm replaceKey = persistedFunc(keyName))
                using (SymmetricAlgorithm regenKey = persistedFunc(keyName))
                using (SymmetricAlgorithm stable = persistedFunc(keyName))
                {
                    // Ensure that we get no padding violations on decrypting with a bad key
                    replaceKey.Padding = regenKey.Padding = stable.Padding = PaddingMode.None;

                    stable.GenerateIV();

                    // Generate (4 * 8) = 32 blocks of plaintext
                    byte[] plainTextBytes = RandomNumberGenerator.GetBytes(4 * stable.BlockSize);
                    byte[] iv = stable.IV;

                    regenKey.IV = replaceKey.IV = iv;

                    byte[] encryptedBytes;

                    using (ICryptoTransform encryptor = replaceKey.CreateEncryptor())
                    {
                        encryptedBytes = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                    }

                    using (ICryptoTransform replaceBefore = replaceKey.CreateDecryptor())
                    using (ICryptoTransform replaceBeforeDelayed = replaceKey.CreateDecryptor())
                    using (ICryptoTransform regenBefore = regenKey.CreateDecryptor())
                    using (ICryptoTransform regenBeforeDelayed = regenKey.CreateDecryptor())
                    using (ICryptoTransform stableBefore = stable.CreateDecryptor())
                    using (ICryptoTransform stableBeforeDelayed = stable.CreateDecryptor())
                    {
                        // Before we regenerate the regen key it should validly decrypt
                        AssertTransformsEqual(plainTextBytes, regenBefore, encryptedBytes);

                        // Before we regenerate the replace key it should validly decrypt
                        AssertTransformsEqual(plainTextBytes, replaceBefore, encryptedBytes);

                        // The stable handle definitely should validly read before.
                        AssertTransformsEqual(plainTextBytes, stableBefore, encryptedBytes);

                        regenKey.GenerateKey();
                        replaceKey.Key = regenKey.Key;

                        using (ICryptoTransform replaceAfter = replaceKey.CreateDecryptor())
                        using (ICryptoTransform regenAfter = regenKey.CreateDecryptor())
                        using (ICryptoTransform stableAfter = stable.CreateDecryptor())
                        {
                            // All of the Befores, and the BeforeDelayed (which have not accessed their key material)
                            // should still decrypt correctly.  And so should stableAfter.
                            AssertTransformsEqual(plainTextBytes, regenBefore, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, regenBeforeDelayed, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, replaceBefore, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, replaceBeforeDelayed, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, stableBefore, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, stableBeforeDelayed, encryptedBytes);
                            AssertTransformsEqual(plainTextBytes, stableAfter, encryptedBytes);

                            // There's a 1 in 2^128 chance that the regenerated key matched the original generated key.
                            byte[] badDecrypt = replaceAfter.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                            Assert.NotEqual(plainTextBytes, badDecrypt);

                            // Regen and replace should come up with the same bad value, since they have the same
                            // key value.
                            AssertTransformsEqual(badDecrypt, regenAfter, encryptedBytes);
                        }
                    }

                    // And, finally, a newly opened handle to the key is also unaffected.
                    using (SymmetricAlgorithm openedAfter = persistedFunc(keyName))
                    {
                        openedAfter.Padding = PaddingMode.None;
                        openedAfter.IV = iv;

                        using (ICryptoTransform decryptor = openedAfter.CreateDecryptor())
                        {
                            AssertTransformsEqual(plainTextBytes, decryptor, encryptedBytes);
                        }
                    }
                }
            }
            finally
            {
                // Delete also Disposes the key, no using should be added here.
                cngKey.Delete();
            }
        }

        public static void VerifyMachineKey(
            CngAlgorithm algorithm,
            int plainBytesCount,
            Func<string, SymmetricAlgorithm> persistedFunc,
            Func<SymmetricAlgorithm> ephemeralFunc)
        {
            string keyName = Guid.NewGuid().ToString();
            CngKeyCreationParameters creationParameters = new CngKeyCreationParameters
            {
                Provider = CngProvider.MicrosoftSoftwareKeyStorageProvider,
                ExportPolicy = CngExportPolicies.AllowPlaintextExport,
                KeyCreationOptions = CngKeyCreationOptions.MachineKey,
            };

            CngKey cngKey = CngKey.Create(algorithm, keyName, creationParameters);

            try
            {
                VerifyPersistedKey(
                    keyName,
                    plainBytesCount,
                    persistedFunc,
                    ephemeralFunc,
                    CipherMode.CBC,
                    PaddingMode.PKCS7,
                    feedbackSizeInBits: 0);
            }
            finally
            {
                // Delete also Disposes the key, no using should be added here.
                cngKey.Delete();
            }
        }

        public static void VerifyCfbPersistedUnsupportedFeedbackSize(
            CngAlgorithm algorithm,
            Func<string, SymmetricAlgorithm> persistedFunc,
            int notSupportedFeedbackSizeInBits)
        {
            string keyName = Guid.NewGuid().ToString();
            string feedbackSizeString = notSupportedFeedbackSizeInBits.ToString();

            // We try to delete the key later which will also dispose of it, so no need
            // to put this in a using.
            CngKey cngKey = CngKey.Create(algorithm, keyName);

            try
            {
                using (SymmetricAlgorithm alg = persistedFunc(keyName))
                {
                    alg.Mode = CipherMode.CFB;
                    alg.FeedbackSize = notSupportedFeedbackSizeInBits;
                    alg.Padding = PaddingMode.None;

                    byte[] destination = new byte[alg.BlockSize / 8];
                    CryptographicException ce = Assert.ThrowsAny<CryptographicException>(() =>
                        alg.EncryptCfb(Array.Empty<byte>(), destination, PaddingMode.None, notSupportedFeedbackSizeInBits));

                    Assert.Contains(feedbackSizeString, ce.Message);

                    ce = Assert.ThrowsAny<CryptographicException>(() =>
                        alg.DecryptCfb(Array.Empty<byte>(), destination, PaddingMode.None, notSupportedFeedbackSizeInBits));

                    Assert.Contains(feedbackSizeString, ce.Message);

                    ce = Assert.ThrowsAny<CryptographicException>(() => alg.CreateDecryptor());
                    Assert.Contains(feedbackSizeString, ce.Message);

                    ce = Assert.ThrowsAny<CryptographicException>(() => alg.CreateEncryptor());
                    Assert.Contains(feedbackSizeString, ce.Message);
                }
            }
            finally
            {
                cngKey.Delete();
            }
        }

        internal static void VerifyMismatchAlgorithmFails(
            CngAlgorithm algorithm,
            Func<string, SymmetricAlgorithm> createFromKey)
        {
            string keyName = Guid.NewGuid().ToString();

            // We try to delete the key later which will also dispose of it, so no need
            // to put this in a using.
            CngKey cngKey = CngKey.Create(algorithm, keyName);

            try
            {
                CryptographicException ce = Assert.Throws<CryptographicException>(() => createFromKey(keyName));
                Assert.Contains($"'{algorithm.Algorithm}'", ce.Message);
            }
            finally
            {
                cngKey.Delete();
            }
        }

        // Windows 7 (Microsoft Windows 6.1) does not support persisted symmetric keys
        // in the Microsoft Software KSP
        internal static bool SupportsPersistedSymmetricKeys => PlatformDetection.IsWindows8xOrLater;

        internal static bool IsAdministrator => PlatformDetection.IsWindows && PlatformDetection.IsPrivilegedProcess;

        internal static void AssertTransformsEqual(byte[] plainTextBytes, ICryptoTransform decryptor, byte[] encryptedBytes)
        {
            byte[] decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            Assert.Equal(plainTextBytes, decrypted);
        }
    }
}
