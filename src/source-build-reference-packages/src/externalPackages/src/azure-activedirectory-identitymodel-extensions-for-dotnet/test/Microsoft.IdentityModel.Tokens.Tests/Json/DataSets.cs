// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

/// <summary>
/// Data sets for testing 
/// </summary>
namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class DataSets
    {
        public static string X5C_1 = "MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng";
        public static string X5C_2 = "MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ";

        #region JsonWebKeys
        public static string JsonWebKeyWithOneRsaKey =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""20am7"",
                                    ""kty"":""RSA"",
                                    ""n"":""mhupHfUtg_gHIqwu2wm8CprXY-gKqbPMV6tEYVqkyYrHugzQ_YDYAHr7vWo5Pe_3gIujSFwpqIfXaP8-Fl3O5fQhMo1lMv4DdRabyDLEpv7YO9qoVKTmDOZqYZx-AYBr5x1Zh2xWByI6_0dsPtCjD1pFZfg_SxNEcLPyH1aY6dT8CWYu32qG4O0WF4EihZzMkzSn8fyh8RXbMf5U9Wm2kgb0g8jK62S7MoF4IlhFaJreq898wgUohhPwR8P3X-gk0XQJAFcogEf04Fw4UmKo3z1B6mcNbPRfImhWw4wtLkhp_KIqKNOkMsSpYGSLrCvqQpgK56EJZExrmb7WozjwHw"",
                                    ""use"":""sig""
                                }";

        public static string JsonWebKeyBadFormatString1 =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                                                                                                                            
                                    ""x5c"":[""MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ""],
                                    ""x5t"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""use""::""sig""
                                }";

        public static string JsonWebKeyBadFormatString2 =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",
                                    ""x5c"":""M""IIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ""],
                                    ""x5t"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""use""::""sig""
                                    }";

        public static string JsonWebKeyBadRsaExponentString =
                                @"{ ""e"":""AQABC"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                               
                                    ""use"":""sig""
                                }";

        public static string JsonWebKeyBadRsaModulusString =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                               
                                    ""use"":""sig""
                                }";

        public static string JsonWebKeyRsaNoKidString =
                                @"{ ""e"":""AQAB"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                               
                                    ""use"":""sig""
                                }";

        public static string JsonWebKeyKtyNotRsaString =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSAA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                               
                                    ""use"":""sig""
                                }";

        public static string JsonWebKeyUseNotSigString =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                                    ""kty"":""RSA"",
                                    ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",                                               
                                    ""use"":""sigg""
                                }";

        public static string JsonWebKeyX509DataString =
                                @"{ ""kid"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                                    ""kty"":""RSA"",
                                    ""use"":""sig"",
                                    ""x5t"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                                    ""x5c"":[""MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng""]
                                }";

        public static string JsonWebKeyBadECCurveString =
                                @"{
                                    ""kty"": ""EC"",
                                    ""alg"": ""ES521"",
                                    ""use"": ""sig"",
                                    ""crv"": ""P-999"",
                                    ""kid"": ""unknownCrv"",
                                    ""x"": ""AX0BXx6mpDjvGk-NLTwobKNjfAP4QCRjtKi8UQsuPqQ2sRKITAcSti3UMn0COcrG_FVgEDNPyPVlSi5LnUl0dREr"",
                                    ""y"": ""AZ8DlNxsA6eCj_JL9Rz8uU4eacd-XX--ek8-VCOgv3YNRPeN_2PJauJL7q9Pg1MSe8zEaLIRhM4SGWJ4SI1rMhlW""
                                }";

        public static string JsonWebKeyOnlyX5tString =
                                @"{
                                    ""kty"": ""RSA"",
                                    ""use"": ""sig"",
                                    ""kid"":""pqoeamb2e5YVzR6_rqFpiCrFZgw"",
                                    ""x5t"":""pqoeamb2e5YVzR6_rqFpiCrFZgw""
                                }";

        public static string JsonWebKeyNoKtyString =
                                @"{ ""e"":""AQAB"",
                                    ""kid"":""20am7"",
                                    ""n"":""mhupHfUtg_gHIqwu2wm8CprXY-gKqbPMV6tEYVqkyYrHugzQ_YDYAHr7vWo5Pe_3gIujSFwpqIfXaP8-Fl3O5fQhMo1lMv4DdRabyDLEpv7YO9qoVKTmDOZqYZx-AYBr5x1Zh2xWByI6_0dsPtCjD1pFZfg_SxNEcLPyH1aY6dT8CWYu32qG4O0WF4EihZzMkzSn8fyh8RXbMf5U9Wm2kgb0g8jK62S7MoF4IlhFaJreq898wgUohhPwR8P3X-gk0XQJAFcogEf04Fw4UmKo3z1B6mcNbPRfImhWw4wtLkhp_KIqKNOkMsSpYGSLrCvqQpgK56EJZExrmb7WozjwHw"",
                                    ""use"":""sig""
                                }";

        public static JsonWebKey JsonWebKeyFromPing1 =>
            new JsonWebKey
            {
                E = "AQAB",
                Kid = "20am7",
                Kty = "RSA",
                N = "mhupHfUtg_gHIqwu2wm8CprXY-gKqbPMV6tEYVqkyYrHugzQ_YDYAHr7vWo5Pe_3gIujSFwpqIfXaP8-Fl3O5fQhMo1lMv4DdRabyDLEpv7YO9qoVKTmDOZqYZx-AYBr5x1Zh2xWByI6_0dsPtCjD1pFZfg_SxNEcLPyH1aY6dT8CWYu32qG4O0WF4EihZzMkzSn8fyh8RXbMf5U9Wm2kgb0g8jK62S7MoF4IlhFaJreq898wgUohhPwR8P3X-gk0XQJAFcogEf04Fw4UmKo3z1B6mcNbPRfImhWw4wtLkhp_KIqKNOkMsSpYGSLrCvqQpgK56EJZExrmb7WozjwHw",
                Use = "sig"
            };

        public static JsonWebKey JsonWebKeyFromPing2 =>
            new JsonWebKey
            {
                E = "AQAB",
                Kid = "20am3",
                Kty = "RSA",
                N = "wY2KNRyiEvyBFkr1IC_1UGWMPInkzVYpoap_-Zw5fYAXLVxKMSPdZVVLt9AVhuNtagOOQqlZ_Y32e4l19REHym6RGV9Sm1noKRxDUjkz7U8OVeUew7D7h4Dk6E2rrlIYpy9OmhhzWSS68pBTf0_ESdekKv3OQbEs99avEXOPK5uH3V-NHsy1YP3DAvl7HJaV6fn-1Nch1quLrg1G7ohBuTb4Zr-499TJ6bkfabaACz8bf-RHuPezFBjoY0LHNNu6-KQ-qqHVkoki_1OQwj2s_Lui3qYWOmLoaVN9ZzO90rBdhhg8t0JZv6pSlc7o0XT4fie5RRjiqCuOpuGQvNYKpQ",
                Use = "sig"
            };

        public static JsonWebKey JsonWebKeyFromPing3 =>
            new JsonWebKey
            {
                E = "AQAB",
                Kid = "20alz",
                Kty = "RSA",
                N = "tgLZUXY8mo2Y1TaXHjOYrFGs23jZxgpzEKfBz004AEeOMHFbEP1h1Lrqf2B7f49mOpXRkBgEm4tnSYzX7pDWrMvNeRVkTFXSXwHYvda1R1kmwiTxnrC9IWjvizrr22DtzHhSSpL_7xuXtmaid2orOF8mUoXnKesPQVfq33pCKm1QUV6oFNSVxAiOKJkzFmxjYvcqzryjYi10glxPSx3cmSI8RGqlxolJr0negfLmI9bNxuAvStf_L6zXB5NFqccmkCQXn_QC3P1N3j-HgwwHTVFxkrS8kZQOMTw3TMXbtTFNrVAx1QC_3M0ze4cVncr2zTSECS_2qXM5RS7xBTEDvQ",
                Use = "sig"
            };

        public static JsonWebKey JsonWebKey1
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Alg = "SHA256",
                    E = "AQAB",
                    Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                    Kty = "RSA",
                    N = "rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw==",
                    Use = "sig",
                    X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                    X5u = "https://jsonkeyurl",
                };

                jsonWebKey.X5c.Add(X5C_1);
                jsonWebKey.KeyOps.Add("signing");

                return jsonWebKey;
            }
        }

        public static JsonWebKey JsonWebKey2
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Alg = "SHA256",
                    E = "AQAB",
                    Kid = "kriMPdmBvx68skT8-mPAB3BseeA",
                    Kty = "RSA",
                    N = "kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw==",
                    X5t = "kriMPdmBvx68skT8-mPAB3BseeA",
                    Use = "sig"
                };

                jsonWebKey.X5c.Add(X5C_2);

                return jsonWebKey;
            }
        }

        public static string JsonWebKeyString =>
            @"{ ""alg"":""SHA256"",
                ""e"":""AQAB"",
                ""key_ops"":[""signing""],
                ""kid"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                ""kty"":""RSA"",
                ""n"":""rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw=="",
                ""use"":""sig"",
                ""x5c"":[""MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng""],
                ""x5t"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                ""x5u"":""https://jsonkeyurl""
            }";

        public static string JsonWebKeyString2 =
            @"{ ""alg"":""SHA256"",
                ""e"":""AQAB"",
                ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                ""kty"":""RSA"",
                ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",
                ""use"":""sig"",
                ""x5c"":[""MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ""],
                ""x5t"":""kriMPdmBvx68skT8-mPAB3BseeA""
            }";

        public static JsonWebKey JsonWebKeyES256 =>
            new JsonWebKey
            {
                Alg = "ES256",
                Crv = "P-256",
                Kid = "JsonWebKeyEcdsa256",
                Kty = "EC",
                Use = "sig",
                X = "luR290c8sXxbOGhNquQ3J3rh763Os4D609cHK-L_5fA",
                Y = "tUqUwtaVHwc7_CXnuBrCpMQTF5BJKdFnw9_JkSIXWpQ"
            };

        public static string JsonWebKeyES256String =>
            @"{
                ""kty"": ""EC"",
                ""alg"": ""ES256"",
                ""use"": ""sig"",
                ""crv"": ""P-256"",
                ""kid"": ""JsonWebKeyEcdsa256"",
                ""x"": ""luR290c8sXxbOGhNquQ3J3rh763Os4D609cHK-L_5fA"",
                ""y"": ""tUqUwtaVHwc7_CXnuBrCpMQTF5BJKdFnw9_JkSIXWpQ""
            }";

        public static JsonWebKey JsonWebKeyES384 =>
            new JsonWebKey
            {
                Alg = "ES384",
                Crv = "P-384",
                Kid = "JsonWebKeyEcdsa384",
                Kty = "EC",
                Use = "sig",
                X = "5mn3HaDoUgdNTFCACaWIvrpriQTloEbMbx4eUu_XvB4pyExig45VIozMnj7FedJg",
                Y = "Vh872HVKNHrzlVu0Ko-3dN-eHoDYBeZgdGLAqenyZ0_X_TctwT6MVLxcAvwbJG5l"
            };

        public static string JsonWebKeyES384String =
            @"{
                ""kty"": ""EC"",
                ""alg"": ""ES384"",
                ""use"": ""sig"",
                ""crv"": ""P-384"",
                ""kid"": ""JsonWebKeyEcdsa384"",
                ""x"": ""5mn3HaDoUgdNTFCACaWIvrpriQTloEbMbx4eUu_XvB4pyExig45VIozMnj7FedJg"",
                ""y"": ""Vh872HVKNHrzlVu0Ko-3dN-eHoDYBeZgdGLAqenyZ0_X_TctwT6MVLxcAvwbJG5l""
            }";

        public static JsonWebKey JsonWebKeyES512 =>
            new JsonWebKey
            {
                Alg = "ES512",
                Crv = "P-521",
                Kid = "JsonWebKeyEcdsa521",
                Kty = "EC",
                Use = "sig",
                X = "AX0BXx6mpDjvGk-NLTwobKNjfAP4QCRjtKi8UQsuPqQ2sRKITAcSti3UMn0COcrG_FVgEDNPyPVlSi5LnUl0dREr",
                Y = "AZ8DlNxsA6eCj_JL9Rz8uU4eacd-XX--ek8-VCOgv3YNRPeN_2PJauJL7q9Pg1MSe8zEaLIRhM4SGWJ4SI1rMhlW"
            };

        public static string JsonWebKeyES512String =>
            @"{
                ""kty"": ""EC"",
                ""alg"": ""ES512"",
                ""use"": ""sig"",
                ""crv"": ""P-521"",
                ""kid"": ""JsonWebKeyEcdsa521"",
                ""x"": ""AX0BXx6mpDjvGk-NLTwobKNjfAP4QCRjtKi8UQsuPqQ2sRKITAcSti3UMn0COcrG_FVgEDNPyPVlSi5LnUl0dREr"",
                ""y"": ""AZ8DlNxsA6eCj_JL9Rz8uU4eacd-XX--ek8-VCOgv3YNRPeN_2PJauJL7q9Pg1MSe8zEaLIRhM4SGWJ4SI1rMhlW""
            }";

        public static string JsonWebKeyBadX509DataString =>
            @"{ ""e"":""AQAB"",
                ""kid"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                ""kty"":""RSA"",
                ""n"":""kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw=="",
                ""x5c"":[""==MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ""],
                ""x5t"":""kriMPdmBvx68skT8-mPAB3BseeA"",
                ""use"":""sig""
            }";

        public static JsonWebKey JsonWebKeyBadX509Data 
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    E = "AQAB",
                    Kid = "kriMPdmBvx68skT8-mPAB3BseeA",
                    Kty = "RSA",
                    N = "kSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuw==",
                    X5t = "kriMPdmBvx68skT8-mPAB3BseeA",
                    Use = "sig"
                };

                jsonWebKey.X5c.Add("==MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ");

                return jsonWebKey;
            }
        }
        #endregion

        #region JsonWebKeySets
        public static string JsonWebKeySetString1 = @"{ ""keys"":[" + JsonWebKeyString + "," + JsonWebKeyString2 + "]}";
        public static string JsonWebKeySetECCString = @"{ ""keys"":[" + JsonWebKeyES256String + "," + JsonWebKeyES384String + "," + JsonWebKeyES512String + "]}";
        public static string JsonWebKeySetOneValidRsaOneInvalidRsaString = @"{ ""keys"":[" + JsonWebKeyWithOneRsaKey + "," + JsonWebKeyBadRsaExponentString + "]}";
        public static string JsonWebKeySetOneInvalidEcOneValidEcString = @"{ ""keys"":[" + JsonWebKeyBadECCurveString + "," + JsonWebKeyES256String + "]}";
        public static string JsonWebKeySetOneValidRsaOneInvalidEcString = @"{ ""keys"":[" + JsonWebKeyWithOneRsaKey + "," + JsonWebKeyBadECCurveString + "]}";
        public static string JsonWebKeySetBadFormatingString =
                                @"{ ""keys"":[
                                    {   ""e"":""AQAB"",
                                        ""kid"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                                        ""kty"":""RSA"",
                                        ""n"":""rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw=="",
                                        ""x5c"":[""MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng""
                                        ""x5t"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                                        ""use"":""sig""
                                    }
                                ]}";

        public static string JsonWebKeySetEvoString =
                                @"{ ""keys"":[
                                    {
                                        ""kty"":""RSA"",
                                        ""use"":""sig"",
                                        ""kid"":""HBxl9mAe6gxavCkcoOU2THsDNa0"",
                                        ""x5t"":""HBxl9mAe6gxavCkcoOU2THsDNa0"",
                                        ""n"":""0afCaiPd_xl_ewZGfOkxKwYPfI4Efu0COfzajK_gnviWk7w3R-88Dmb0j24DSn1qVR3ptCnA1-QUfUMyhvl8pT5-t7oRkLNPzp0hVV-dAG3ZoMaSEMW0wapshA6LVGROpBncDmc66hx5-t3eOFA24fiKfQiv2TJth3Y9jhHnLe7GBOoomWYx_pJiEG3mhYFIt7shaEwNcEjo34vr1WWzRm8D8gogjrJWd1moyeGftWLzvfp9e79QwHYJv907vQbFrT7LYuy8g7-Rpxujgumw2mx7CewcCZXwPiZ-raM3Ap1FhINiGpd5mbbYrFDDFIWAjWPUY6KNvXtc24yUfZr4MQ"",
                                        ""e"":""AQAB"",
                                        ""x5c"":[
                                            ""MIIDBTCCAe2gAwIBAgIQWcq84CdVhKVEcKbZdMOMGjANBgkqhkiG9w0BAQsFADAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MB4XDTE5MDMxNDAwMDAwMFoXDTIxMDMxNDAwMDAwMFowLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBANGnwmoj3f8Zf3sGRnzpMSsGD3yOBH7tAjn82oyv4J74lpO8N0fvPA5m9I9uA0p9alUd6bQpwNfkFH1DMob5fKU+fre6EZCzT86dIVVfnQBt2aDGkhDFtMGqbIQOi1RkTqQZ3A5nOuocefrd3jhQNuH4in0Ir9kybYd2PY4R5y3uxgTqKJlmMf6SYhBt5oWBSLe7IWhMDXBI6N+L69Vls0ZvA/IKII6yVndZqMnhn7Vi8736fXu/UMB2Cb/dO70Gxa0+y2LsvIO/kacbo4LpsNpsewnsHAmV8D4mfq2jNwKdRYSDYhqXeZm22KxQwxSFgI1j1GOijb17XNuMlH2a+DECAwEAAaMhMB8wHQYDVR0OBBYEFIkZ5wrSV8lohIsreOmig7h5wQDkMA0GCSqGSIb3DQEBCwUAA4IBAQAd8sKZLwZBocM4pMIRKarK60907jQCOi1m449WyToUcYPXmU7wrjy9fkYwJdC5sniItVBJ3RIQbF/hyjwnRoIaEcWYMAftBnH+c19WIuiWjR3EHnIdxmSopezl/9FaTNghbKjZtrKK+jL/RdkMY9uWxwUFLjTAtMm24QOt2+CGntBA9ohQUgiML/mlUpf4qEqa2/Lh+bjiHl3smg4TwuIl0i/TMN9Rg7UgQ6BnqfgiuMl6BtBiatNollwgGNI2zJEi47MjdeMf8+C3tXs//asqqlqJCyVLwN7AN47ynYmkl89MleOfKIojhrGRxryZG2nRjD9u/kZbPJ8e3JE9px67""
                                        ],
                                        ""issuer"":""https://login.microsoftonline.com/{tenantid}/v2.0""
                                    }
                                ]}";

        public static string JsonWebKeySetBadRsaExponentString = @"{ ""keys"":[" + JsonWebKeyBadRsaExponentString + "]}";
        public static string JsonWebKeySetBadRsaModulusString = @"{ ""keys"":[" + JsonWebKeyBadRsaModulusString + "]}";
        public static string JsonWebKeySetUseNoKidString = @"{ ""keys"":[" + JsonWebKeyRsaNoKidString + "]}";
        public static string JsonWebKeySetKtyNotRsaString = @"{ ""keys"":[" + JsonWebKeyKtyNotRsaString + "]}";
        public static string JsonWebKeySetUseNotSigString = @"{ ""keys"":[" + JsonWebKeyUseNotSigString + "]}";
        public static string JsonWebKeySetX509DataString = @"{ ""keys"":[" + JsonWebKeyX509DataString + "]}";
        public static string JsonWebKeySetBadX509String = @"{ ""keys"":[" + JsonWebKeyBadX509DataString + "]}";
        public static string JsonWebKeySetBadECCurveString = @"{ ""keys"":[" + JsonWebKeyBadECCurveString + "]}";
        public static string JsonWebKeySetOnlyX5tString = @"{ ""keys"":[" + JsonWebKeyOnlyX5tString + "]}";
        public static string JsonWebKeySetUseNoKtyString = @"{ ""keys"":[" + JsonWebKeyNoKtyString + "]}";

        #region GOOGLE 2/2/2024 https://www.googleapis.com/oauth2/v3/certs
        public static string AccountsGoogleJson =
            """
            {
                "keys": [
                    {
                        "use": "sig",
                        "alg": "RS256",
                        "kty": "RSA",
                        "e": "AQAB",
                        "kid": "85e55107466b7e29836199c58c7581f5b923be44",
                        "n": "4tVDrq5RbeDtlJ2Xh2dikE840LWflr89Cm3cGI9mQGlskTigV0anoViOH92Z1sqWAp5e1aRkLlCm-KAWc69uvOW_X70jEhzDJVREeB3h-RAnzxYrbUgDEgltiUaM8Zxtt8hiVh_GDAudRmSP9kDxXL5xnJETF1gnwAHa0j7cM4STLKbtwKi73CEmTjTLqGAES8XVnXp8VWGb6IuQzdmBIJkfcFog4Inq93F4Cj_SXsSjECG3j56VxgwnloPCHTXVn_xS1s3OjoBCOvOVSJfg2nSTWNi93JGR9pWZevh7Sq8Clw8H2lvIAPV_HYdxvsucWg8sJuTa6ZZSxT1WmBkW6Q"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "bdc4e109815f469460e63d34cd684215148d7b59",
                        "n": "v3dZL2R2PuebbAChYXKVW6R-FJDUVmZ8TyVMWH0-VpVjFYZvy7BZaE5ApLWc3UhpXug6r6230AJI0ow5yePnqmZnI5qckxz0br0Fj27Zdg-X4PWN95gdk6fpI4JwNmZFsgiWzmDiP118j8jIxMNBiIVPT7RyykhAZeNnGC2kDU-81iop850K205EwfSi_TBT6HCbRj_TSQ2oJfIXDPX8s7Kg4PRjDOHt3D8CiqsIWbxSkRRuTiU_1Ahsbuc3d9hkD1rOOThVT6T7LVZT710WtPa1QbKUgGIu2pmiPo0BCdnbqozsRVOwY901R77VlVwpTuGonPZuyO1B2FgGuYgotw",
                        "e": "AQAB",
                        "alg": "RS256"
                    }
                ]
            }
            """;

        public static JsonWebKey AccountsGoogleKey1
        {
            get
            {
                return new JsonWebKey
                {
                    Alg = "RS256",
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "85e55107466b7e29836199c58c7581f5b923be44",
                    E = "AQAB",
                    N = "4tVDrq5RbeDtlJ2Xh2dikE840LWflr89Cm3cGI9mQGlskTigV0anoViOH92Z1sqWAp5e1aRkLlCm-KAWc69uvOW_X70jEhzDJVREeB3h-RAnzxYrbUgDEgltiUaM8Zxtt8hiVh_GDAudRmSP9kDxXL5xnJETF1gnwAHa0j7cM4STLKbtwKi73CEmTjTLqGAES8XVnXp8VWGb6IuQzdmBIJkfcFog4Inq93F4Cj_SXsSjECG3j56VxgwnloPCHTXVn_xS1s3OjoBCOvOVSJfg2nSTWNi93JGR9pWZevh7Sq8Clw8H2lvIAPV_HYdxvsucWg8sJuTa6ZZSxT1WmBkW6Q"
                };
            }
        }

        public static JsonWebKey AccountsGoogleKey2
        {
            get
            {
                return new JsonWebKey
                {
                    Alg = "RS256",
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "bdc4e109815f469460e63d34cd684215148d7b59",
                    E = "AQAB",
                    N = "v3dZL2R2PuebbAChYXKVW6R-FJDUVmZ8TyVMWH0-VpVjFYZvy7BZaE5ApLWc3UhpXug6r6230AJI0ow5yePnqmZnI5qckxz0br0Fj27Zdg-X4PWN95gdk6fpI4JwNmZFsgiWzmDiP118j8jIxMNBiIVPT7RyykhAZeNnGC2kDU-81iop850K205EwfSi_TBT6HCbRj_TSQ2oJfIXDPX8s7Kg4PRjDOHt3D8CiqsIWbxSkRRuTiU_1Ahsbuc3d9hkD1rOOThVT6T7LVZT710WtPa1QbKUgGIu2pmiPo0BCdnbqozsRVOwY901R77VlVwpTuGonPZuyO1B2FgGuYgotw"
                };
            }
        }

        public static JsonWebKeySet AccountsGoogleKeySet
        {
            get
            {
                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(AccountsGoogleKey1);
                jsonWebKeySet.Keys.Add(AccountsGoogleKey2);

                return jsonWebKeySet;
            }
        }
        #endregion

        #region AADCommonV1 2/2/2024 https://login.microsoftonline.com/common/discovery/keys
        public static string AADCommonV1KeySetJson =
            """
            {
                "keys": [
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                        "x5t": "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                        "n": "t6Q2XSeWnMA_-crH2UbftfS01QDAqHoPQFqsRtVkxG4eyamnNlTl3Da07QQkjpPEbLoLtgtMI2Pr0plO7xU9f94mhbfK_UJ6Y0KcWxhwKMkCgnzcFOQF4eH_AICHLOKa8vPthtcprNcCmjbksW5TYBZi6uLhFLw_HsjGOxhK0VaDWnWizNVeqvzVB0jt9Vdmfhs6Zohy_1b2Wusdad1NmSKzhC74IDjlIaFoik_ZJJdtLOgoIwOZTLW0M1UKhRrWtj7AjVCnE_zBiloACm1IrIM_PymE10cJJ6WFz29ep4g7X65xCEU6zJ5oIFibvk6cKKcFNB7FFjbehYVpw5BxVQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIICHb5qy8hKKgwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDAxMTUxODA0MTRaFw0yOTAxMTUxODA0MTRaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC3pDZdJ5acwD/5ysfZRt+19LTVAMCoeg9AWqxG1WTEbh7Jqac2VOXcNrTtBCSOk8Rsugu2C0wjY+vSmU7vFT1/3iaFt8r9QnpjQpxbGHAoyQKCfNwU5AXh4f8AgIcs4pry8+2G1yms1wKaNuSxblNgFmLq4uEUvD8eyMY7GErRVoNadaLM1V6q/NUHSO31V2Z+GzpmiHL/VvZa6x1p3U2ZIrOELvggOOUhoWiKT9kkl20s6CgjA5lMtbQzVQqFGta2PsCNUKcT/MGKWgAKbUisgz8/KYTXRwknpYXPb16niDtfrnEIRTrMnmggWJu+TpwopwU0HsUWNt6FhWnDkHFVAgMBAAGjITAfMB0GA1UdDgQWBBQLGQYqt7pRrKWQ25XWSi6lGN818DANBgkqhkiG9w0BAQsFAAOCAQEAtky1EYTKZvbTAveLmL3VCi+bJMjY5wyDO4Yulpv0VP1RS3dksmEALOsa1Bfz2BXVpIKPUJLdvFFoFhDqReAqRRqxylhI+oMwTeAsZ1lYCV4hTWDrN/MML9SYyeQ441Xp7xHIzu1ih4rSkNwrsx231GTfzo6dHMsi12oEdyn6mXavWehBDbzVDxbeqR+0ymhCgeYjIfCX6z2SrSMGYiG2hzs/xzypnIPnv6cBMQQDS4sdquoCsvIqJRWmF9ow79oHhzSTwGJj4+jEQi7QMTDR30rYiPTIdE63bnuARdgNF/dqB7n4ZJv566jvbzHpfCTqrJyj7Guvjr9i56NpLmz2DA=="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                        "x5t": "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                        "n": "6Jiu4AU4ZWHBFEbO1-41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey_ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB_XtmMOtuG45WyPlbARQl8hok7y_hbydS8uyfm_ZQXN7MLgju0f4_cYo-dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm-oHE2_Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep_Pfxptzmpd0rwd0F87IneYu-jtKUvHVVPJQ7yQvgin0rZR8tXIp_IzComGipktu_AJ89z3atOEt0_vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1Q",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6TCCAdGgAwIBAgIIV6K/4n2M5VAwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTIzMTEzMDAwMTAxNVoXDTI4MTEzMDAwMTAxNVowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6Jiu4AU4ZWHBFEbO1+41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey/ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB/XtmMOtuG45WyPlbARQl8hok7y/hbydS8uyfm/ZQXN7MLgju0f4/cYo+dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm+oHE2/Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep/Pfxptzmpd0rwd0F87IneYu+jtKUvHVVPJQ7yQvgin0rZR8tXIp/IzComGipktu/AJ89z3atOEt0/vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1QIDAQABoyEwHzAdBgNVHQ4EFgQUTtiYd3S6DOacXBmYsKyr1EK67f4wDQYJKoZIhvcNAQELBQADggEBAGbSzomDLsU7BX6Vohf/VweoJ9TgYs4cYcdFwMrRQVMpMGKYU6HT7f8mDzRGqpursuJTsN9yIOk7s5xp+N7EL6XKauzo+VHOGJT1qbwzJXT1XY6DuzBrlhtY9C7AUHlpYAD4uWyt+JfuB+z5Qq5cbGr0dMS/EEKR/m0iBboUNDji6u9sUzGqUYn4tpBoE+y0J8UttankG/09PPHwQIjMxMXcBDmGi5VTp9eY5RFk9GQ4qdQJUp2hhdQZDVpz6lcPxhG92RPO/ca3P/9dvfI5aNaiSyV7vuK2NGCVGCTeo/okA+V5dm5jeuf0bupNEPnXSGyM8EHjcRjR+cHsby5pIGs="
                        ]
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "qor_VePWgmxWy3r1dpfsWsw2-zY",
                        "x5t": "qor_VePWgmxWy3r1dpfsWsw2-zY",
                        "n": "rV8eXna9NCyzvgVZvbz18NhLIAfo1Qzn-VQQCbQzyGi2KDe3RI2sLeHltv9mVI2sahcRjgvhYNSETyxqHaKw3w8L4jg0kJdfzhD8dvpl32hunOCzuY2WpyJVq6CkxzGN4iikWTEIe_GMGsu9qhdxybaTCBTAya8qyKL1sbEByk8FiY6nsm6BhuRUVCh_rzfAp3HY-U_58ORLF1tmZrmSljHMFwlxvYuOIlKHacXy9gen8HsT7PUSA4n2PdnT1XAmlKJG1mzvdqyG2L3iRQJ45tcmrERKcd1pYwhb7ZtTyKypkeR9lkKbaYiQUt1QhpeO12pH1bRB1_k9MMzOm8Ca1Q",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAMqvEglnjttEMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDAxMjQyMjE0MjNaFw0yOTAxMjQyMjE0MjNaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAK1fHl52vTQss74FWb289fDYSyAH6NUM5/lUEAm0M8hotig3t0SNrC3h5bb/ZlSNrGoXEY4L4WDUhE8sah2isN8PC+I4NJCXX84Q/Hb6Zd9obpzgs7mNlqciVaugpMcxjeIopFkxCHvxjBrLvaoXccm2kwgUwMmvKsii9bGxAcpPBYmOp7JugYbkVFQof683wKdx2PlP+fDkSxdbZma5kpYxzBcJcb2LjiJSh2nF8vYHp/B7E+z1EgOJ9j3Z09VwJpSiRtZs73ashti94kUCeObXJqxESnHdaWMIW+2bU8isqZHkfZZCm2mIkFLdUIaXjtdqR9W0Qdf5PTDMzpvAmtUCAwEAAaMhMB8wHQYDVR0OBBYEFKOxwRo5B0oCCCLMp8I/cHosF4cPMA0GCSqGSIb3DQEBCwUAA4IBAQCifXD9bW3dh6WAwfFHnL2HefUhuDQzisAZBR0o6kPASObicJK91BtfVg6iYW0LUCE70EVnFkyypTy19JIPf3zutQkHFAdXtS2/0NiR0vRJ561gi5Yqjl9BW9Az6Eb/O4UEzqBpe313FNt2co8I0OjRNhbKB1lIPUu6UZs5qBdfTwQFB6fU/XfXHnpZERZgRUZu5mku/n2EHZ1iMe9of1Qv/AtXgB51ZlfpT6YbrqMJBJs1yHxLd+rYqoXCwWLoBlJ3xYm4jEzSHPLjFgqHrUb9Cl2SazRKhV/UBAGqq0xG6qZMoiWvcfv0equddGa/r84lrEU2y1RBGUGw15jiLL/y"
                        ]
                    }
                ]
            }
            """;

        public static JsonWebKey AADCommonV1Key1
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                    X5t = "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                    N = "t6Q2XSeWnMA_-crH2UbftfS01QDAqHoPQFqsRtVkxG4eyamnNlTl3Da07QQkjpPEbLoLtgtMI2Pr0plO7xU9f94mhbfK_UJ6Y0KcWxhwKMkCgnzcFOQF4eH_AICHLOKa8vPthtcprNcCmjbksW5TYBZi6uLhFLw_HsjGOxhK0VaDWnWizNVeqvzVB0jt9Vdmfhs6Zohy_1b2Wusdad1NmSKzhC74IDjlIaFoik_ZJJdtLOgoIwOZTLW0M1UKhRrWtj7AjVCnE_zBiloACm1IrIM_PymE10cJJ6WFz29ep4g7X65xCEU6zJ5oIFibvk6cKKcFNB7FFjbehYVpw5BxVQ",
                    E = "AQAB"
                };
                jsonWebKey.X5c.Add("MIIC/TCCAeWgAwIBAgIICHb5qy8hKKgwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDAxMTUxODA0MTRaFw0yOTAxMTUxODA0MTRaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC3pDZdJ5acwD/5ysfZRt+19LTVAMCoeg9AWqxG1WTEbh7Jqac2VOXcNrTtBCSOk8Rsugu2C0wjY+vSmU7vFT1/3iaFt8r9QnpjQpxbGHAoyQKCfNwU5AXh4f8AgIcs4pry8+2G1yms1wKaNuSxblNgFmLq4uEUvD8eyMY7GErRVoNadaLM1V6q/NUHSO31V2Z+GzpmiHL/VvZa6x1p3U2ZIrOELvggOOUhoWiKT9kkl20s6CgjA5lMtbQzVQqFGta2PsCNUKcT/MGKWgAKbUisgz8/KYTXRwknpYXPb16niDtfrnEIRTrMnmggWJu+TpwopwU0HsUWNt6FhWnDkHFVAgMBAAGjITAfMB0GA1UdDgQWBBQLGQYqt7pRrKWQ25XWSi6lGN818DANBgkqhkiG9w0BAQsFAAOCAQEAtky1EYTKZvbTAveLmL3VCi+bJMjY5wyDO4Yulpv0VP1RS3dksmEALOsa1Bfz2BXVpIKPUJLdvFFoFhDqReAqRRqxylhI+oMwTeAsZ1lYCV4hTWDrN/MML9SYyeQ441Xp7xHIzu1ih4rSkNwrsx231GTfzo6dHMsi12oEdyn6mXavWehBDbzVDxbeqR+0ymhCgeYjIfCX6z2SrSMGYiG2hzs/xzypnIPnv6cBMQQDS4sdquoCsvIqJRWmF9ow79oHhzSTwGJj4+jEQi7QMTDR30rYiPTIdE63bnuARdgNF/dqB7n4ZJv566jvbzHpfCTqrJyj7Guvjr9i56NpLmz2DA==");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV1Key2
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                    X5t = "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                    N = "6Jiu4AU4ZWHBFEbO1-41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey_ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB_XtmMOtuG45WyPlbARQl8hok7y_hbydS8uyfm_ZQXN7MLgju0f4_cYo-dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm-oHE2_Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep_Pfxptzmpd0rwd0F87IneYu-jtKUvHVVPJQ7yQvgin0rZR8tXIp_IzComGipktu_AJ89z3atOEt0_vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1Q",
                    E = "AQAB"
                };
                jsonWebKey.X5c.Add("MIIC6TCCAdGgAwIBAgIIV6K/4n2M5VAwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTIzMTEzMDAwMTAxNVoXDTI4MTEzMDAwMTAxNVowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6Jiu4AU4ZWHBFEbO1+41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey/ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB/XtmMOtuG45WyPlbARQl8hok7y/hbydS8uyfm/ZQXN7MLgju0f4/cYo+dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm+oHE2/Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep/Pfxptzmpd0rwd0F87IneYu+jtKUvHVVPJQ7yQvgin0rZR8tXIp/IzComGipktu/AJ89z3atOEt0/vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1QIDAQABoyEwHzAdBgNVHQ4EFgQUTtiYd3S6DOacXBmYsKyr1EK67f4wDQYJKoZIhvcNAQELBQADggEBAGbSzomDLsU7BX6Vohf/VweoJ9TgYs4cYcdFwMrRQVMpMGKYU6HT7f8mDzRGqpursuJTsN9yIOk7s5xp+N7EL6XKauzo+VHOGJT1qbwzJXT1XY6DuzBrlhtY9C7AUHlpYAD4uWyt+JfuB+z5Qq5cbGr0dMS/EEKR/m0iBboUNDji6u9sUzGqUYn4tpBoE+y0J8UttankG/09PPHwQIjMxMXcBDmGi5VTp9eY5RFk9GQ4qdQJUp2hhdQZDVpz6lcPxhG92RPO/ca3P/9dvfI5aNaiSyV7vuK2NGCVGCTeo/okA+V5dm5jeuf0bupNEPnXSGyM8EHjcRjR+cHsby5pIGs=");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV1Key3
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "qor_VePWgmxWy3r1dpfsWsw2-zY",
                    X5t = "qor_VePWgmxWy3r1dpfsWsw2-zY",
                    N = "rV8eXna9NCyzvgVZvbz18NhLIAfo1Qzn-VQQCbQzyGi2KDe3RI2sLeHltv9mVI2sahcRjgvhYNSETyxqHaKw3w8L4jg0kJdfzhD8dvpl32hunOCzuY2WpyJVq6CkxzGN4iikWTEIe_GMGsu9qhdxybaTCBTAya8qyKL1sbEByk8FiY6nsm6BhuRUVCh_rzfAp3HY-U_58ORLF1tmZrmSljHMFwlxvYuOIlKHacXy9gen8HsT7PUSA4n2PdnT1XAmlKJG1mzvdqyG2L3iRQJ45tcmrERKcd1pYwhb7ZtTyKypkeR9lkKbaYiQUt1QhpeO12pH1bRB1_k9MMzOm8Ca1Q",
                    E = "AQAB"
                };
                jsonWebKey.X5c.Add("MIIC6jCCAdKgAwIBAgIJAMqvEglnjttEMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDAxMjQyMjE0MjNaFw0yOTAxMjQyMjE0MjNaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAK1fHl52vTQss74FWb289fDYSyAH6NUM5/lUEAm0M8hotig3t0SNrC3h5bb/ZlSNrGoXEY4L4WDUhE8sah2isN8PC+I4NJCXX84Q/Hb6Zd9obpzgs7mNlqciVaugpMcxjeIopFkxCHvxjBrLvaoXccm2kwgUwMmvKsii9bGxAcpPBYmOp7JugYbkVFQof683wKdx2PlP+fDkSxdbZma5kpYxzBcJcb2LjiJSh2nF8vYHp/B7E+z1EgOJ9j3Z09VwJpSiRtZs73ashti94kUCeObXJqxESnHdaWMIW+2bU8isqZHkfZZCm2mIkFLdUIaXjtdqR9W0Qdf5PTDMzpvAmtUCAwEAAaMhMB8wHQYDVR0OBBYEFKOxwRo5B0oCCCLMp8I/cHosF4cPMA0GCSqGSIb3DQEBCwUAA4IBAQCifXD9bW3dh6WAwfFHnL2HefUhuDQzisAZBR0o6kPASObicJK91BtfVg6iYW0LUCE70EVnFkyypTy19JIPf3zutQkHFAdXtS2/0NiR0vRJ561gi5Yqjl9BW9Az6Eb/O4UEzqBpe313FNt2co8I0OjRNhbKB1lIPUu6UZs5qBdfTwQFB6fU/XfXHnpZERZgRUZu5mku/n2EHZ1iMe9of1Qv/AtXgB51ZlfpT6YbrqMJBJs1yHxLd+rYqoXCwWLoBlJ3xYm4jEzSHPLjFgqHrUb9Cl2SazRKhV/UBAGqq0xG6qZMoiWvcfv0equddGa/r84lrEU2y1RBGUGw15jiLL/y");
                return jsonWebKey;
            }
        }

        public static JsonWebKeySet AADCommonV1KeySet
        {
            get
            {
                return new JsonWebKeySet
                {
                    Keys = new List<JsonWebKey>
                    {
                        AADCommonV1Key1,
                        AADCommonV1Key2,
                        AADCommonV1Key3
                    }
                };
            }
        }
        #endregion

        #region AADCommonV2 2/2/2024 https://login.microsoftonline.com/common/discovery/v2.0/keys
        public static string AADCommonV2KeySetJson =
            """
            {
                "keys":
                [
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                        "x5t": "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                        "n": "t6Q2XSeWnMA_-crH2UbftfS01QDAqHoPQFqsRtVkxG4eyamnNlTl3Da07QQkjpPEbLoLtgtMI2Pr0plO7xU9f94mhbfK_UJ6Y0KcWxhwKMkCgnzcFOQF4eH_AICHLOKa8vPthtcprNcCmjbksW5TYBZi6uLhFLw_HsjGOxhK0VaDWnWizNVeqvzVB0jt9Vdmfhs6Zohy_1b2Wusdad1NmSKzhC74IDjlIaFoik_ZJJdtLOgoIwOZTLW0M1UKhRrWtj7AjVCnE_zBiloACm1IrIM_PymE10cJJ6WFz29ep4g7X65xCEU6zJ5oIFibvk6cKKcFNB7FFjbehYVpw5BxVQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC/TCCAeWgAwIBAgIICHb5qy8hKKgwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDAxMTUxODA0MTRaFw0yOTAxMTUxODA0MTRaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC3pDZdJ5acwD/5ysfZRt+19LTVAMCoeg9AWqxG1WTEbh7Jqac2VOXcNrTtBCSOk8Rsugu2C0wjY+vSmU7vFT1/3iaFt8r9QnpjQpxbGHAoyQKCfNwU5AXh4f8AgIcs4pry8+2G1yms1wKaNuSxblNgFmLq4uEUvD8eyMY7GErRVoNadaLM1V6q/NUHSO31V2Z+GzpmiHL/VvZa6x1p3U2ZIrOELvggOOUhoWiKT9kkl20s6CgjA5lMtbQzVQqFGta2PsCNUKcT/MGKWgAKbUisgz8/KYTXRwknpYXPb16niDtfrnEIRTrMnmggWJu+TpwopwU0HsUWNt6FhWnDkHFVAgMBAAGjITAfMB0GA1UdDgQWBBQLGQYqt7pRrKWQ25XWSi6lGN818DANBgkqhkiG9w0BAQsFAAOCAQEAtky1EYTKZvbTAveLmL3VCi+bJMjY5wyDO4Yulpv0VP1RS3dksmEALOsa1Bfz2BXVpIKPUJLdvFFoFhDqReAqRRqxylhI+oMwTeAsZ1lYCV4hTWDrN/MML9SYyeQ441Xp7xHIzu1ih4rSkNwrsx231GTfzo6dHMsi12oEdyn6mXavWehBDbzVDxbeqR+0ymhCgeYjIfCX6z2SrSMGYiG2hzs/xzypnIPnv6cBMQQDS4sdquoCsvIqJRWmF9ow79oHhzSTwGJj4+jEQi7QMTDR30rYiPTIdE63bnuARdgNF/dqB7n4ZJv566jvbzHpfCTqrJyj7Guvjr9i56NpLmz2DA=="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                        "x5t": "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                        "n": "6Jiu4AU4ZWHBFEbO1-41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey_ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB_XtmMOtuG45WyPlbARQl8hok7y_hbydS8uyfm_ZQXN7MLgju0f4_cYo-dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm-oHE2_Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep_Pfxptzmpd0rwd0F87IneYu-jtKUvHVVPJQ7yQvgin0rZR8tXIp_IzComGipktu_AJ89z3atOEt0_vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1Q",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6TCCAdGgAwIBAgIIV6K/4n2M5VAwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTIzMTEzMDAwMTAxNVoXDTI4MTEzMDAwMTAxNVowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6Jiu4AU4ZWHBFEbO1+41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey/ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB/XtmMOtuG45WyPlbARQl8hok7y/hbydS8uyfm/ZQXN7MLgju0f4/cYo+dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm+oHE2/Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep/Pfxptzmpd0rwd0F87IneYu+jtKUvHVVPJQ7yQvgin0rZR8tXIp/IzComGipktu/AJ89z3atOEt0/vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1QIDAQABoyEwHzAdBgNVHQ4EFgQUTtiYd3S6DOacXBmYsKyr1EK67f4wDQYJKoZIhvcNAQELBQADggEBAGbSzomDLsU7BX6Vohf/VweoJ9TgYs4cYcdFwMrRQVMpMGKYU6HT7f8mDzRGqpursuJTsN9yIOk7s5xp+N7EL6XKauzo+VHOGJT1qbwzJXT1XY6DuzBrlhtY9C7AUHlpYAD4uWyt+JfuB+z5Qq5cbGr0dMS/EEKR/m0iBboUNDji6u9sUzGqUYn4tpBoE+y0J8UttankG/09PPHwQIjMxMXcBDmGi5VTp9eY5RFk9GQ4qdQJUp2hhdQZDVpz6lcPxhG92RPO/ca3P/9dvfI5aNaiSyV7vuK2NGCVGCTeo/okA+V5dm5jeuf0bupNEPnXSGyM8EHjcRjR+cHsby5pIGs="
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "qor_VePWgmxWy3r1dpfsWsw2-zY",
                        "x5t": "qor_VePWgmxWy3r1dpfsWsw2-zY",
                        "n": "rV8eXna9NCyzvgVZvbz18NhLIAfo1Qzn-VQQCbQzyGi2KDe3RI2sLeHltv9mVI2sahcRjgvhYNSETyxqHaKw3w8L4jg0kJdfzhD8dvpl32hunOCzuY2WpyJVq6CkxzGN4iikWTEIe_GMGsu9qhdxybaTCBTAya8qyKL1sbEByk8FiY6nsm6BhuRUVCh_rzfAp3HY-U_58ORLF1tmZrmSljHMFwlxvYuOIlKHacXy9gen8HsT7PUSA4n2PdnT1XAmlKJG1mzvdqyG2L3iRQJ45tcmrERKcd1pYwhb7ZtTyKypkeR9lkKbaYiQUt1QhpeO12pH1bRB1_k9MMzOm8Ca1Q",
                        "e": "AQAB",
                        "x5c": [
                            "MIIC6jCCAdKgAwIBAgIJAMqvEglnjttEMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDAxMjQyMjE0MjNaFw0yOTAxMjQyMjE0MjNaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAK1fHl52vTQss74FWb289fDYSyAH6NUM5/lUEAm0M8hotig3t0SNrC3h5bb/ZlSNrGoXEY4L4WDUhE8sah2isN8PC+I4NJCXX84Q/Hb6Zd9obpzgs7mNlqciVaugpMcxjeIopFkxCHvxjBrLvaoXccm2kwgUwMmvKsii9bGxAcpPBYmOp7JugYbkVFQof683wKdx2PlP+fDkSxdbZma5kpYxzBcJcb2LjiJSh2nF8vYHp/B7E+z1EgOJ9j3Z09VwJpSiRtZs73ashti94kUCeObXJqxESnHdaWMIW+2bU8isqZHkfZZCm2mIkFLdUIaXjtdqR9W0Qdf5PTDMzpvAmtUCAwEAAaMhMB8wHQYDVR0OBBYEFKOxwRo5B0oCCCLMp8I/cHosF4cPMA0GCSqGSIb3DQEBCwUAA4IBAQCifXD9bW3dh6WAwfFHnL2HefUhuDQzisAZBR0o6kPASObicJK91BtfVg6iYW0LUCE70EVnFkyypTy19JIPf3zutQkHFAdXtS2/0NiR0vRJ561gi5Yqjl9BW9Az6Eb/O4UEzqBpe313FNt2co8I0OjRNhbKB1lIPUu6UZs5qBdfTwQFB6fU/XfXHnpZERZgRUZu5mku/n2EHZ1iMe9of1Qv/AtXgB51ZlfpT6YbrqMJBJs1yHxLd+rYqoXCwWLoBlJ3xYm4jEzSHPLjFgqHrUb9Cl2SazRKhV/UBAGqq0xG6qZMoiWvcfv0equddGa/r84lrEU2y1RBGUGw15jiLL/y"
                        ],
                        "issuer": "https://login.microsoftonline.com/{tenantid}/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "jCScSBGaA6xAieLw3-sAuvjRM_0",
                        "x5t": "jCScSBGaA6xAieLw3-sAuvjRM_0",
                        "n": "4xSpkfJJRaZXxUlWoAd6FD4DP48ZvFH7R7BJ42sZt5PJlP_H8Byq2D39a0MH1NyButs0kO8nHTrT79uSLlMA6FKgtTAyJvk0omn3oumpxqYYr34BZK1geofPJ0k9uTHuPnznC8LEuYCtp8PlDXIYALQVZz8u59uCQOdLF908DdvcL89ui50xTQqknS7PgHJcih4sdH7hLWwWASBmx6vBuK0ZBc9iNfLzqCrTU843mh3arI34tA93babCYCGq_bwcxPEguQ_4rXDEKKPIt8NuhKRl3y6flgQX7pIT30Usx484ouCJ-2mO9YNMlFzNIbEuVIN-0pXoUdTxMwwWhHt_tQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIDAzCCAeugAwIBAgIJANzd9PcaNKIuMA0GCSqGSIb3DQEBCwUAMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTAeFw0yMzEyMTQyMzQ2MjVaFw0yODEyMTQyMzQ2MjVaMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAOMUqZHySUWmV8VJVqAHehQ+Az+PGbxR+0ewSeNrGbeTyZT/x/Acqtg9/WtDB9TcgbrbNJDvJx060+/bki5TAOhSoLUwMib5NKJp96LpqcamGK9+AWStYHqHzydJPbkx7j585wvCxLmArafD5Q1yGAC0FWc/LufbgkDnSxfdPA3b3C/PboudMU0KpJ0uz4ByXIoeLHR+4S1sFgEgZserwbitGQXPYjXy86gq01PON5od2qyN+LQPd22mwmAhqv28HMTxILkP+K1wxCijyLfDboSkZd8un5YEF+6SE99FLMePOKLgiftpjvWDTJRczSGxLlSDftKV6FHU8TMMFoR7f7UCAwEAAaMuMCwwCwYDVR0PBAQDAgLEMB0GA1UdDgQWBBRq/vfu4DJgGRvfHSrPfgKcPfgLEjANBgkqhkiG9w0BAQsFAAOCAQEAAQ4W8nG90EiAsC1eDmMJgALMASOTZzmWjupNQLhpwzlyA8lH23wwFPtr66ZZbq9Wwv2wnh3NULsj9iiBazGc/jmd/lIsGRTHsDVJPXK9gErhxS8g1k77fvjInaE+hIaWIN1rpu6zv4aDO3XEGmsdXgYL1LswqrDRaA5+wT54YfVl0dp+QMBkO+6yWWaRiqLOoE/eiTNUjwuyUFVA7cuBmyDP1QjIfDFKqFNucXwW6HHxrAwtF8y9GA96ibaH9QvNbXCw+NzkBrNoLwxGH/q2eWrSWck87AryF6px3WYNe3AibOHythDW+J8SL0/1uelqOmWlkNIpgU7z+QiWuNM6ZA=="
                        ],
                        "issuer": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0"
                    },
                    {
                        "kty": "RSA",
                        "use": "sig",
                        "kid": "2Spohh9y2me52nKrhai7GxWJibU",
                        "x5t": "2Spohh9y2me52nKrhai7GxWJibU",
                        "n": "nwLh9Ii-9iSmvLT9WiMxSb7MIv__um6y_ying_Rz9WzuP9MDAPxsGw26aiC6W_BieXd5WKJy3h03t5xNQL-oUyC74VOfD_pmwKdzdiY_qn6ERy8HrxHp8HDRMNSYYYDGXPdkrM8rFtiixW6wo15rnGwtrKNBPQPH-S8W6sGbY-pcXehbayMIcHhAs0ywn7fGPZ5X8LIxOtucVmEILs2BcPIhqGEYKuY9C_Cy-KWZiKbg72E-Fp5erUIlYsA85bC6Yyb5Yw9yhhjmqJ3m5B7jjrPqPc0cd_x8ZaVW0fSaHz8TtOYsNbN5qW1ajDvzDZkl6fba0SkwaNmJHLEdubeXEQ",
                        "e": "AQAB",
                        "x5c": [
                            "MIIDAzCCAeugAwIBAgIJAJZMbrNaGyxWMA0GCSqGSIb3DQEBCwUAMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTAeFw0yNDAxMTkxNzExNDRaFw0yOTAxMTkxNzExNDRaMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJ8C4fSIvvYkpry0/VojMUm+zCL//7pusv8op4P0c/Vs7j/TAwD8bBsNumogulvwYnl3eViict4dN7ecTUC/qFMgu+FTnw/6ZsCnc3YmP6p+hEcvB68R6fBw0TDUmGGAxlz3ZKzPKxbYosVusKNea5xsLayjQT0Dx/kvFurBm2PqXF3oW2sjCHB4QLNMsJ+3xj2eV/CyMTrbnFZhCC7NgXDyIahhGCrmPQvwsvilmYim4O9hPhaeXq1CJWLAPOWwumMm+WMPcoYY5qid5uQe446z6j3NHHf8fGWlVtH0mh8/E7TmLDWzealtWow78w2ZJen22tEpMGjZiRyxHbm3lxECAwEAAaMuMCwwCwYDVR0PBAQDAgLEMB0GA1UdDgQWBBRXpNiRf2930gemHXJQsfP+RQz+7TANBgkqhkiG9w0BAQsFAAOCAQEATvXhuCEg8l8rvy8CdD8hDPpqBMr+8ynpqWKeUBEdsNqXilOSgRxTgEespB9ahFzqFUxXEZF0SqK4TkDDL5Cze8zo4H+DX7WnTyySHD+dxwYNdU8hV+kpA6UH5QSN+zOE7TtDWH0NR00987tXEht16ZvrPR3vNkkEG5sORMGLP20lTVgod+ycTvl8hfDAYagI3qqmfUtPwr2rONoTXDbV2ETlSGnpwq1feddyzWvfOJT/UHVIKO93JsrALrIZ8lXqYvZ2GiRffiuwpD5/QATsLFfs+CR698cxIqyOxuzx3NVqs4rdabUGO2RH5tWzODdFshkaM3u0Uk8hgmGwraJH5Q=="
                        ],
                        "issuer": "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0"
                    }
                ]
            }
            """;

        public static JsonWebKey AADCommonV2Key1
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                    X5t = "kWbkaa6qs8wsTnBwiiNYOhHbnAw",
                    N = "t6Q2XSeWnMA_-crH2UbftfS01QDAqHoPQFqsRtVkxG4eyamnNlTl3Da07QQkjpPEbLoLtgtMI2Pr0plO7xU9f94mhbfK_UJ6Y0KcWxhwKMkCgnzcFOQF4eH_AICHLOKa8vPthtcprNcCmjbksW5TYBZi6uLhFLw_HsjGOxhK0VaDWnWizNVeqvzVB0jt9Vdmfhs6Zohy_1b2Wusdad1NmSKzhC74IDjlIaFoik_ZJJdtLOgoIwOZTLW0M1UKhRrWtj7AjVCnE_zBiloACm1IrIM_PymE10cJJ6WFz29ep4g7X65xCEU6zJ5oIFibvk6cKKcFNB7FFjbehYVpw5BxVQ",
                    E = "AQAB"
                };
                jsonWebKey.AdditionalData.Add("issuer", "https://login.microsoftonline.com/{tenantid}/v2.0");
                jsonWebKey.X5c.Add("MIIC/TCCAeWgAwIBAgIICHb5qy8hKKgwDQYJKoZIhvcNAQELBQAwLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldDAeFw0yNDAxMTUxODA0MTRaFw0yOTAxMTUxODA0MTRaMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC3pDZdJ5acwD/5ysfZRt+19LTVAMCoeg9AWqxG1WTEbh7Jqac2VOXcNrTtBCSOk8Rsugu2C0wjY+vSmU7vFT1/3iaFt8r9QnpjQpxbGHAoyQKCfNwU5AXh4f8AgIcs4pry8+2G1yms1wKaNuSxblNgFmLq4uEUvD8eyMY7GErRVoNadaLM1V6q/NUHSO31V2Z+GzpmiHL/VvZa6x1p3U2ZIrOELvggOOUhoWiKT9kkl20s6CgjA5lMtbQzVQqFGta2PsCNUKcT/MGKWgAKbUisgz8/KYTXRwknpYXPb16niDtfrnEIRTrMnmggWJu+TpwopwU0HsUWNt6FhWnDkHFVAgMBAAGjITAfMB0GA1UdDgQWBBQLGQYqt7pRrKWQ25XWSi6lGN818DANBgkqhkiG9w0BAQsFAAOCAQEAtky1EYTKZvbTAveLmL3VCi+bJMjY5wyDO4Yulpv0VP1RS3dksmEALOsa1Bfz2BXVpIKPUJLdvFFoFhDqReAqRRqxylhI+oMwTeAsZ1lYCV4hTWDrN/MML9SYyeQ441Xp7xHIzu1ih4rSkNwrsx231GTfzo6dHMsi12oEdyn6mXavWehBDbzVDxbeqR+0ymhCgeYjIfCX6z2SrSMGYiG2hzs/xzypnIPnv6cBMQQDS4sdquoCsvIqJRWmF9ow79oHhzSTwGJj4+jEQi7QMTDR30rYiPTIdE63bnuARdgNF/dqB7n4ZJv566jvbzHpfCTqrJyj7Guvjr9i56NpLmz2DA==");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV2Key2
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                    X5t = "fwNs8F_h9KrHMA_OcTP1pGFWwyc",
                    N = "6Jiu4AU4ZWHBFEbO1-41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey_ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB_XtmMOtuG45WyPlbARQl8hok7y_hbydS8uyfm_ZQXN7MLgju0f4_cYo-dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm-oHE2_Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep_Pfxptzmpd0rwd0F87IneYu-jtKUvHVVPJQ7yQvgin0rZR8tXIp_IzComGipktu_AJ89z3atOEt0_vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1Q",
                    E = "AQAB"
                };
                jsonWebKey.AdditionalData.Add("issuer", "https://login.microsoftonline.com/{tenantid}/v2.0");
                jsonWebKey.X5c.Add("MIIC6TCCAdGgAwIBAgIIV6K/4n2M5VAwDQYJKoZIhvcNAQELBQAwIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMB4XDTIzMTEzMDAwMTAxNVoXDTI4MTEzMDAwMTAxNVowIzEhMB8GA1UEAxMYbG9naW4ubWljcm9zb2Z0b25saW5lLnVzMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA6Jiu4AU4ZWHBFEbO1+41P6dxKgGx7J31i5wNzH5eoJlsNjWrWoGlZip8ey/ZppcNMY0GY330p8YwdazqRX24mPkyOxbYF1uGEGB/XtmMOtuG45WyPlbARQl8hok7y/hbydS8uyfm/ZQXN7MLgju0f4/cYo+dgic5OaR3W6CWfgOrNnf287ZZ2HtJ8DZNm+oHE2/Tg9FFTIIkpltNIZ4rJ0uwzuy7zkep/Pfxptzmpd0rwd0F87IneYu+jtKUvHVVPJQ7yQvgin0rZR8tXIp/IzComGipktu/AJ89z3atOEt0/vZPizQIMRpToHjUTNXuXaDWIvCIJYMkvvl0HJxf1QIDAQABoyEwHzAdBgNVHQ4EFgQUTtiYd3S6DOacXBmYsKyr1EK67f4wDQYJKoZIhvcNAQELBQADggEBAGbSzomDLsU7BX6Vohf/VweoJ9TgYs4cYcdFwMrRQVMpMGKYU6HT7f8mDzRGqpursuJTsN9yIOk7s5xp+N7EL6XKauzo+VHOGJT1qbwzJXT1XY6DuzBrlhtY9C7AUHlpYAD4uWyt+JfuB+z5Qq5cbGr0dMS/EEKR/m0iBboUNDji6u9sUzGqUYn4tpBoE+y0J8UttankG/09PPHwQIjMxMXcBDmGi5VTp9eY5RFk9GQ4qdQJUp2hhdQZDVpz6lcPxhG92RPO/ca3P/9dvfI5aNaiSyV7vuK2NGCVGCTeo/okA+V5dm5jeuf0bupNEPnXSGyM8EHjcRjR+cHsby5pIGs=");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV2Key3
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "qor_VePWgmxWy3r1dpfsWsw2-zY",
                    X5t = "qor_VePWgmxWy3r1dpfsWsw2-zY",
                    N = "rV8eXna9NCyzvgVZvbz18NhLIAfo1Qzn-VQQCbQzyGi2KDe3RI2sLeHltv9mVI2sahcRjgvhYNSETyxqHaKw3w8L4jg0kJdfzhD8dvpl32hunOCzuY2WpyJVq6CkxzGN4iikWTEIe_GMGsu9qhdxybaTCBTAya8qyKL1sbEByk8FiY6nsm6BhuRUVCh_rzfAp3HY-U_58ORLF1tmZrmSljHMFwlxvYuOIlKHacXy9gen8HsT7PUSA4n2PdnT1XAmlKJG1mzvdqyG2L3iRQJ45tcmrERKcd1pYwhb7ZtTyKypkeR9lkKbaYiQUt1QhpeO12pH1bRB1_k9MMzOm8Ca1Q",
                    E = "AQAB"
                };
                jsonWebKey.AdditionalData.Add("issuer", "https://login.microsoftonline.com/{tenantid}/v2.0");
                jsonWebKey.X5c.Add("MIIC6jCCAdKgAwIBAgIJAMqvEglnjttEMA0GCSqGSIb3DQEBCwUAMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czAeFw0yNDAxMjQyMjE0MjNaFw0yOTAxMjQyMjE0MjNaMCMxITAfBgNVBAMTGGxvZ2luLm1pY3Jvc29mdG9ubGluZS51czCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAK1fHl52vTQss74FWb289fDYSyAH6NUM5/lUEAm0M8hotig3t0SNrC3h5bb/ZlSNrGoXEY4L4WDUhE8sah2isN8PC+I4NJCXX84Q/Hb6Zd9obpzgs7mNlqciVaugpMcxjeIopFkxCHvxjBrLvaoXccm2kwgUwMmvKsii9bGxAcpPBYmOp7JugYbkVFQof683wKdx2PlP+fDkSxdbZma5kpYxzBcJcb2LjiJSh2nF8vYHp/B7E+z1EgOJ9j3Z09VwJpSiRtZs73ashti94kUCeObXJqxESnHdaWMIW+2bU8isqZHkfZZCm2mIkFLdUIaXjtdqR9W0Qdf5PTDMzpvAmtUCAwEAAaMhMB8wHQYDVR0OBBYEFKOxwRo5B0oCCCLMp8I/cHosF4cPMA0GCSqGSIb3DQEBCwUAA4IBAQCifXD9bW3dh6WAwfFHnL2HefUhuDQzisAZBR0o6kPASObicJK91BtfVg6iYW0LUCE70EVnFkyypTy19JIPf3zutQkHFAdXtS2/0NiR0vRJ561gi5Yqjl9BW9Az6Eb/O4UEzqBpe313FNt2co8I0OjRNhbKB1lIPUu6UZs5qBdfTwQFB6fU/XfXHnpZERZgRUZu5mku/n2EHZ1iMe9of1Qv/AtXgB51ZlfpT6YbrqMJBJs1yHxLd+rYqoXCwWLoBlJ3xYm4jEzSHPLjFgqHrUb9Cl2SazRKhV/UBAGqq0xG6qZMoiWvcfv0equddGa/r84lrEU2y1RBGUGw15jiLL/y");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV2Key4
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "jCScSBGaA6xAieLw3-sAuvjRM_0",
                    X5t = "jCScSBGaA6xAieLw3-sAuvjRM_0",
                    N = "4xSpkfJJRaZXxUlWoAd6FD4DP48ZvFH7R7BJ42sZt5PJlP_H8Byq2D39a0MH1NyButs0kO8nHTrT79uSLlMA6FKgtTAyJvk0omn3oumpxqYYr34BZK1geofPJ0k9uTHuPnznC8LEuYCtp8PlDXIYALQVZz8u59uCQOdLF908DdvcL89ui50xTQqknS7PgHJcih4sdH7hLWwWASBmx6vBuK0ZBc9iNfLzqCrTU843mh3arI34tA93babCYCGq_bwcxPEguQ_4rXDEKKPIt8NuhKRl3y6flgQX7pIT30Usx484ouCJ-2mO9YNMlFzNIbEuVIN-0pXoUdTxMwwWhHt_tQ",
                    E = "AQAB"
                };
                jsonWebKey.AdditionalData["issuer"] = "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";
                jsonWebKey.X5c.Add("MIIDAzCCAeugAwIBAgIJANzd9PcaNKIuMA0GCSqGSIb3DQEBCwUAMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTAeFw0yMzEyMTQyMzQ2MjVaFw0yODEyMTQyMzQ2MjVaMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAOMUqZHySUWmV8VJVqAHehQ+Az+PGbxR+0ewSeNrGbeTyZT/x/Acqtg9/WtDB9TcgbrbNJDvJx060+/bki5TAOhSoLUwMib5NKJp96LpqcamGK9+AWStYHqHzydJPbkx7j585wvCxLmArafD5Q1yGAC0FWc/LufbgkDnSxfdPA3b3C/PboudMU0KpJ0uz4ByXIoeLHR+4S1sFgEgZserwbitGQXPYjXy86gq01PON5od2qyN+LQPd22mwmAhqv28HMTxILkP+K1wxCijyLfDboSkZd8un5YEF+6SE99FLMePOKLgiftpjvWDTJRczSGxLlSDftKV6FHU8TMMFoR7f7UCAwEAAaMuMCwwCwYDVR0PBAQDAgLEMB0GA1UdDgQWBBRq/vfu4DJgGRvfHSrPfgKcPfgLEjANBgkqhkiG9w0BAQsFAAOCAQEAAQ4W8nG90EiAsC1eDmMJgALMASOTZzmWjupNQLhpwzlyA8lH23wwFPtr66ZZbq9Wwv2wnh3NULsj9iiBazGc/jmd/lIsGRTHsDVJPXK9gErhxS8g1k77fvjInaE+hIaWIN1rpu6zv4aDO3XEGmsdXgYL1LswqrDRaA5+wT54YfVl0dp+QMBkO+6yWWaRiqLOoE/eiTNUjwuyUFVA7cuBmyDP1QjIfDFKqFNucXwW6HHxrAwtF8y9GA96ibaH9QvNbXCw+NzkBrNoLwxGH/q2eWrSWck87AryF6px3WYNe3AibOHythDW+J8SL0/1uelqOmWlkNIpgU7z+QiWuNM6ZA==");
                return jsonWebKey;
            }
        }

        public static JsonWebKey AADCommonV2Key5
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = "2Spohh9y2me52nKrhai7GxWJibU",
                    X5t = "2Spohh9y2me52nKrhai7GxWJibU",
                    N = "nwLh9Ii-9iSmvLT9WiMxSb7MIv__um6y_ying_Rz9WzuP9MDAPxsGw26aiC6W_BieXd5WKJy3h03t5xNQL-oUyC74VOfD_pmwKdzdiY_qn6ERy8HrxHp8HDRMNSYYYDGXPdkrM8rFtiixW6wo15rnGwtrKNBPQPH-S8W6sGbY-pcXehbayMIcHhAs0ywn7fGPZ5X8LIxOtucVmEILs2BcPIhqGEYKuY9C_Cy-KWZiKbg72E-Fp5erUIlYsA85bC6Yyb5Yw9yhhjmqJ3m5B7jjrPqPc0cd_x8ZaVW0fSaHz8TtOYsNbN5qW1ajDvzDZkl6fba0SkwaNmJHLEdubeXEQ",
                    E = "AQAB"
                };
                jsonWebKey.AdditionalData["issuer"] = "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";
                jsonWebKey.X5c.Add("MIIDAzCCAeugAwIBAgIJAJZMbrNaGyxWMA0GCSqGSIb3DQEBCwUAMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTAeFw0yNDAxMTkxNzExNDRaFw0yOTAxMTkxNzExNDRaMCkxJzAlBgNVBAMTHkxpdmUgSUQgU1RTIFNpZ25pbmcgUHVibGljIEtleTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAJ8C4fSIvvYkpry0/VojMUm+zCL//7pusv8op4P0c/Vs7j/TAwD8bBsNumogulvwYnl3eViict4dN7ecTUC/qFMgu+FTnw/6ZsCnc3YmP6p+hEcvB68R6fBw0TDUmGGAxlz3ZKzPKxbYosVusKNea5xsLayjQT0Dx/kvFurBm2PqXF3oW2sjCHB4QLNMsJ+3xj2eV/CyMTrbnFZhCC7NgXDyIahhGCrmPQvwsvilmYim4O9hPhaeXq1CJWLAPOWwumMm+WMPcoYY5qid5uQe446z6j3NHHf8fGWlVtH0mh8/E7TmLDWzealtWow78w2ZJen22tEpMGjZiRyxHbm3lxECAwEAAaMuMCwwCwYDVR0PBAQDAgLEMB0GA1UdDgQWBBRXpNiRf2930gemHXJQsfP+RQz+7TANBgkqhkiG9w0BAQsFAAOCAQEATvXhuCEg8l8rvy8CdD8hDPpqBMr+8ynpqWKeUBEdsNqXilOSgRxTgEespB9ahFzqFUxXEZF0SqK4TkDDL5Cze8zo4H+DX7WnTyySHD+dxwYNdU8hV+kpA6UH5QSN+zOE7TtDWH0NR00987tXEht16ZvrPR3vNkkEG5sORMGLP20lTVgod+ycTvl8hfDAYagI3qqmfUtPwr2rONoTXDbV2ETlSGnpwq1feddyzWvfOJT/UHVIKO93JsrALrIZ8lXqYvZ2GiRffiuwpD5/QATsLFfs+CR698cxIqyOxuzx3NVqs4rdabUGO2RH5tWzODdFshkaM3u0Uk8hgmGwraJH5Q==");
                return jsonWebKey;
            }
        }

        public static JsonWebKeySet AADCommonV2KeySet
        {
            get
            {
                return new JsonWebKeySet
                {
                    Keys = new List<JsonWebKey>
                    {
                        AADCommonV2Key1,
                        AADCommonV2Key2,
                        AADCommonV2Key3,
                        AADCommonV2Key4,
                        AADCommonV2Key5
                    }
                };
            }
        }
        #endregion

        public static JsonWebKeySet JsonWebKeySet1
        {
            get
            {
                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(JsonWebKey1);
                jsonWebKeySet.Keys.Add(JsonWebKey2);

                return jsonWebKeySet;
            }
        }

        public static JsonWebKeySet JsonWebKeySetX509Data
        {
            get
            {
                var jsonWebKey = new JsonWebKey
                {
                    Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                    Kty = "RSA",
                    Use = "sig",
                    X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A"
                };

                jsonWebKey.X5c.Add("MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng");

                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(jsonWebKey);

                return jsonWebKeySet;
            }
        }

        public static JsonWebKeySet JsonWebKeySetEC
        {
            get
            {
                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(JsonWebKeyES256);
                jsonWebKeySet.Keys.Add(JsonWebKeyES384);
                jsonWebKeySet.Keys.Add(JsonWebKeyES512);

                return jsonWebKeySet;
            }
        }

        public static JsonWebKeySet JsonWebKeySetOnlyX5t
        {
            get
            {
                JsonWebKey jsonWebKey = new JsonWebKey
                {
                    Kid = "pqoeamb2e5YVzR6_rqFpiCrFZgw",
                    Kty = "RSA",
                    Use = "sig",
                    X5t = "pqoeamb2e5YVzR6_rqFpiCrFZgw"
                };

                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(jsonWebKey);

                return jsonWebKeySet;
            }
        }
        #endregion
    }
}
