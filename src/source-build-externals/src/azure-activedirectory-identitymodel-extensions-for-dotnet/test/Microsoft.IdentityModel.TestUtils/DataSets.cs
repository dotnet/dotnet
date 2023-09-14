// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Data sets for testing 
/// </summary>
namespace Microsoft.IdentityModel.TestUtils
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

        public static string JsonWebKeyMixedCaseString =>
            @"{ ""Alg"":""SHA256"",
                ""E"":""AQAB"",
                ""key_Ops"":[""signing""],
                ""kid"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                ""kty"":""RSA"",
                ""n"":""rCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVw=="",
                ""use"":""sig"",
                ""x5C"":[""MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng""],
                ""x5t"":""NGTFvdK-fythEuLwjpwAJOM9n-A"",
                ""x5T#S256"":""NGTFvdK-fythEuLwjpwAJOM9n-A256"",
                ""x5u"":""https://jsonkeyurl""
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
        public static string GoogleCertsFile = "google-certs.json";
        public static JsonWebKeySet GoogleCertsExpected
        {
            get
            {
                JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
                jsonWebKeySet.Keys.Add(
                    new JsonWebKey
                    {
                        Alg = "RS256",
                        E = "AQAB",
                        Kty = "RSA",
                        Kid = "ab844f3d4c69feee0de2501b04e1a4c8d78eead1",
                        N = "AKrMiv5vhYehVKXnSpZZN6lYymUIi+NS97ceYKYClMlNyj2Ln4ErWiOwjwdivG2kZnN0kKCC/XL9E+uEgsZO3ECvvDtgtFhPOR0MiqL7pp/K7d58dbKUWX/cWy8E4bm/Zmwa/g0HDcW6o19+Q85IPYXbY/6Z2oOgA9qDAoGHkjIv",
                        Use = "sig",
                    });

                jsonWebKeySet.Keys.Add(
                     new JsonWebKey
                     {
                         Alg = "RS256",
                         E = "AQAB",
                         Kty = "RSA",
                         Kid = "550326e0aacb4674d22905a1a51a808cfa7463b0",
                         N = "ANLFuJO6EoKczde+YP3b1yuz2b46D7Rd7CjrbvKrzbjkH29iRFLBagT7nojwdMOPrsV+WLp/C8lfkRT7UJ38lnQh3m4oEy98HdRRMZh5Vtpbotgt4S/ugh5ansJdHSXSBTxk+X1ZnTzMOUH7ZROpxw3NcX/IFl0sshFlTbebPrDj",
                         Use = "sig",
                     });

                return jsonWebKeySet;
            }
        }

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
