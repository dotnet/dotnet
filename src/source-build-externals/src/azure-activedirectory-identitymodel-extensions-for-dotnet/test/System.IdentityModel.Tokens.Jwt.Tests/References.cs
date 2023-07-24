// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// references from https://datatracker.ietf.org/doc/html/rfc7520
    /// </summary>
    public static class RFC7520References
    {
        #region Keys

        // 3.1. EC Public Key
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.1
        public static string ECDsaPublicKeyJson
        {
            get
            {
                return @"{
                    ""kty"": ""EC"",
                    ""kid"": ""bilbo.baggins@hobbiton.example"",
                    ""use"": ""sig"",
                    ""crv"": ""P-521"",
                    ""x"": ""AHKZLLOsCOzz5cY97ewNUajB957y-C-U88c3v13nmGZx6sYl_oJXu9A5RkTKqjqvjyekWF-7ytDyRXYgCF5cj0Kt"",
                    ""y"": ""AdymlHvOiLxXkEhayXQnNCvDX4h9htZaCJN34kfmC6pV5OhQHiraVySsUdaQkAgDPrwQrJmbnX9cwlGfP-HqHZR1""}";
            }
        }

        // 3.1. EC Public Key
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.1
        public static JsonWebKey ECDsaPublicKey
        {
            get
            {
                return new JsonWebKey(ECDsaPublicKeyJson);
            }
        }

        // 3.2. EC Private Key Json
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.2
        public static string ECDsaPrivateKeyJson
        {
            get
            {
                return @"{
                    ""kty"": ""EC"",
                    ""kid"": ""bilbo.baggins@hobbiton.example"",
                    ""use"": ""sig"",
                    ""crv"": ""P-521"",
                    ""x"": ""AHKZLLOsCOzz5cY97ewNUajB957y-C-U88c3v13nmGZx6sYl_oJXu9A5RkTKqjqvjyekWF-7ytDyRXYgCF5cj0Kt"",
                    ""y"": ""AdymlHvOiLxXkEhayXQnNCvDX4h9htZaCJN34kfmC6pV5OhQHiraVySsUdaQkAgDPrwQrJmbnX9cwlGfP-HqHZR1"",
                    ""d"": ""AAhRON2r9cqXX1hg-RoI6R1tX5p2rUAYdmpHZoC1XNM56KtscrX6zbKipQrCW9CGZH3T4ubpnoTKLDYJ_fF3_rJt""}";
            }
        }

        // 3.2. EC Private Key
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.2
        public static JsonWebKey ECDsaPrivateKey
        {
            get
            {
                return new JsonWebKey(ECDsaPrivateKeyJson);
            }
        }

        // 3.3.  RSA Public Key Json
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.3
        public static string RSASigningPublicKeyJson
        {
            get
            {
                return @"{
                    ""kty"": ""RSA"",
                    ""kid"": ""bilbo.baggins@hobbiton.example"",
                    ""use"": ""sig"",
                    ""n"": ""n4EPtAOCc9AlkeQHPzHStgAbgs7bTZLwUBZdR8_KuKPEHLd4rHVTeT-O-XV2jRojdNhxJWTDvNd7nqQ0VEiZQHz_AJmSCpMaJMRBSFKrKb2wqVwGU_NsYOYL-QtiWN2lbzcEe6XC0dApr5ydQLrHqkHHig3RBordaZ6Aj-oBHqFEHYpPe7Tpe-OfVfHd1E6cS6M1FZcD1NNLYD5lFHpPI9bTwJlsde3uhGqC0ZCuEHg8lhzwOHrtIQbS0FVbb9k3-tVTU4fg_3L_vniUFAKwuCLqKnS2BYwdq_mzSnbLY7h_qixoR7jig3__kRhuaxwUkRz5iaiQkqgc5gHdrNP5zw"",
                    ""e"": ""AQAB"" }";
            }
        }

        // 3.3.  RSA Public Key
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.3
        public static JsonWebKey RSASigningPublicKey
        {
            get
            {
                return new JsonWebKey(RSASigningPublicKeyJson);
            }
        }

        // 3.4.  RSA Private Key Json
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.4
        public static string RSASigningPrivateKeyJson
        {
            get
            {
                return @"{
                    ""kty"": ""RSA"",
                    ""kid"": ""bilbo.baggins@hobbiton.example"",
                    ""use"": ""sig"",
                    ""n"": ""n4EPtAOCc9AlkeQHPzHStgAbgs7bTZLwUBZdR8_KuKPEHLd4rHVTeT-O-XV2jRojdNhxJWTDvNd7nqQ0VEiZQHz_AJmSCpMaJMRBSFKrKb2wqVwGU_NsYOYL-QtiWN2lbzcEe6XC0dApr5ydQLrHqkHHig3RBordaZ6Aj-oBHqFEHYpPe7Tpe-OfVfHd1E6cS6M1FZcD1NNLYD5lFHpPI9bTwJlsde3uhGqC0ZCuEHg8lhzwOHrtIQbS0FVbb9k3-tVTU4fg_3L_vniUFAKwuCLqKnS2BYwdq_mzSnbLY7h_qixoR7jig3__kRhuaxwUkRz5iaiQkqgc5gHdrNP5zw"",
                    ""e"": ""AQAB"",
                    ""d"": ""bWUC9B-EFRIo8kpGfh0ZuyGPvMNKvYWNtB_ikiH9k20eT-O1q_I78eiZkpXxXQ0UTEs2LsNRS-8uJbvQ-A1irkwMSMkK1J3XTGgdrhCku9gRldY7sNA_AKZGh-Q661_42rINLRCe8W-nZ34ui_qOfkLnK9QWDDqpaIsA-bMwWWSDFu2MUBYwkHTMEzLYGqOe04noqeq1hExBTHBOBdkMXiuFhUq1BU6l-DqEiWxqg82sXt2h-LMnT3046AOYJoRioz75tSUQfGCshWTBnP5uDjd18kKhyv07lhfSJdrPdM5Plyl21hsFf4L_mHCuoFau7gdsPfHPxxjVOcOpBrQzwQ"",
                    ""p"": ""3Slxg_DwTXJcb6095RoXygQCAZ5RnAvZlno1yhHtnUex_fp7AZ_9nRaO7HX_-SFfGQeutao2TDjDAWU4Vupk8rw9JR0AzZ0N2fvuIAmr_WCsmGpeNqQnev1T7IyEsnh8UMt-n5CafhkikzhEsrmndH6LxOrvRJlsPp6Zv8bUq0k"",
                    ""q"": ""uKE2dh-cTf6ERF4k4e_jy78GfPYUIaUyoSSJuBzp3Cubk3OCqs6grT8bR_cu0Dm1MZwWmtdqDyI95HrUeq3MP15vMMON8lHTeZu2lmKvwqW7anV5UzhM1iZ7z4yMkuUwFWoBvyY898EXvRD-hdqRxHlSqAZ192zB3pVFJ0s7pFc"",
                    ""dp"": ""B8PVvXkvJrj2L-GYQ7v3y9r6Kw5g9SahXBwsWUzp19TVlgI-YV85q1NIb1rxQtD-IsXXR3-TanevuRPRt5OBOdiMGQp8pbt26gljYfKU_E9xn-RULHz0-ed9E9gXLKD4VGngpz-PfQ_q29pk5xWHoJp009Qf1HvChixRX59ehik"",
                    ""dq"": ""CLDmDGduhylc9o7r84rEUVn7pzQ6PF83Y-iBZx5NT-TpnOZKF1pErAMVeKzFEl41DlHHqqBLSM0W1sOFbwTxYWZDm6sI6og5iTbwQGIC3gnJKbi_7k_vJgGHwHxgPaX2PnvP-zyEkDERuf-ry4c_Z11Cq9AqC2yeL6kdKT1cYF8"",
                    ""qi"": ""3PiqvXQN0zwMeE-sBvZgi289XP9XCQF3VWqPzMKnIgQp7_Tugo6-NZBKCQsMf3HaEGBjTVJs_jcK8-TRXvaKe-7ZMaQj8VfBdYkssbu0NKDDhjJ-GtiseaDVWt7dcH0cfwxgFUHpQh7FoCrjFJ6h6ZEpMF6xmujs4qMpPz8aaI4"" }";
            }
        }

        // 3.4.  RSA Private Key
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.4
        public static JsonWebKey RSASigningPrivateKey
        {
            get
            {
                return new JsonWebKey(RSASigningPrivateKeyJson);
            }
        }

        // 3.5.  Symmetric Key(MAC Computation)
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.5
        public static string SymmetricKeyMacJson
        {
            get
            {
                return @"{
                    ""kty"": ""oct"",
                    ""kid"": ""018c0ae5-4d9b-471b-bfd6-eef314bc7037"",
                    ""use"": ""sig"",
                    ""alg"": ""HS256"",
                    ""k"": ""hJtXIZ2uSN5kbQfbtTNWbpdmhkV8FJG-Onbc6mxCcYg""}";
            }
        }

        // 3.5.  Symmetric Key(MAC Computation)
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.5
        public static JsonWebKey SymmetricKeyMac
        {
            get
            {
                return new JsonWebKey
                {
                    Alg = "HS256",
                    K = "hJtXIZ2uSN5kbQfbtTNWbpdmhkV8FJG-Onbc6mxCcYg",
                    KeyId = "018c0ae5-4d9b-471b-bfd6-eef314bc7037",
                    Kty = "oct",
                    Use = "sig"
                };
            }
        }

        // 3.6.  Symmetric Key(Encryption)
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.6
        public static string SymmetricKeyEncJson
        {
            get
            {
                return @"{
                    ""kty"": ""oct"",
                    ""kid"": ""1e571774-2e08-40da-8308-e8d68773842d"",
                    ""use"": ""enc"",
                    ""alg"": ""A256GCM"",
                    ""k"": ""AAPapAv4LbFbiVawEjagUBluYqN5rhna-8nuldDvOx8"" }";
            }
        }

        // 3.6.  Symmetric Key(Encryption)
        // https://datatracker.ietf.org/doc/html/rfc7520#section-3.6
        public static JsonWebKey SymmetricKeyEnc
        {
            get
            {
                return new JsonWebKey(SymmetricKeyEncJson);
            }
        }

        // 5.1.1  Key Encryption Using RSA v1.5 and AES-HMAC-SHA2
        // https://datatracker.ietf.org/doc/html/rfc7520#section-5.1.1
        public static string RSA_1_5_PrivateKeyJson
        {
            get
            {
                return @"{
                    ""kty"":""RSA"",
                    ""kid"":""frodo.baggins@hobbiton.example"",
                    ""use"":""enc"",
                    ""n"":""maxhbsmBtdQ3CNrKvprUE6n9lYcregDMLYNeTAWcLj8NnPU9XIYegTHVHQjxKDSHP2l-F5jS7sppG1wgdAqZyhnWvXhYNvcM7RfgKxqNx_xAHx6f3yy7s-M9PSNCwPC2lh6UAkR4I00EhV9lrypM9Pi4lBUop9t5fS9W5UNwaAllhrd-osQGPjIeI1deHTwx-ZTHu3C60Pu_LJIl6hKn9wbwaUmA4cR5Bd2pgbaY7ASgsjCUbtYJaNIHSoHXprUdJZKUMAzV0WOKPfA6OPI4oypBadjvMZ4ZAj3BnXaSYsEZhaueTXvZB4eZOAjIyh2e_VOIKVMsnDrJYAVotGlvMQ"",
                    ""e"":""AQAB"",
                    ""d"":""Kn9tgoHfiTVi8uPu5b9TnwyHwG5dK6RE0uFdlpCGnJN7ZEi963R7wybQ1PLAHmpIbNTztfrheoAniRV1NCIqXaW_qS461xiDTp4ntEPnqcKsyO5jMAji7-CL8vhpYYowNFvIesgMoVaPRYMYT9TW63hNM0aWs7USZ_hLg6Oe1mY0vHTI3FucjSM86Nff4oIENt43r2fspgEPGRrdE6fpLc9Oaq-qeP1GFULimrRdndm-P8q8kvN3KHlNAtEgrQAgTTgz80S-3VD0FgWfgnb1PNmiuPUxO8OpI9KDIfu_acc6fg14nsNaJqXe6RESvhGPH2afjHqSy_Fd2vpzj85bQQ"",
                    ""p"":""2DwQmZ43FoTnQ8IkUj3BmKRf5Eh2mizZA5xEJ2MinUE3sdTYKSLtaEoekX9vbBZuWxHdVhM6UnKCJ_2iNk8Z0ayLYHL0_G21aXf9-unynEpUsH7HHTklLpYAzOOx1ZgVljoxAdWNn3hiEFrjZLZGS7lOH-a3QQlDDQoJOJ2VFmU"",
                    ""q"":""te8LY4-W7IyaqH1ExujjMqkTAlTeRbv0VLQnfLY2xINnrWdwiQ93_VF099aP1ESeLja2nw-6iKIe-qT7mtCPozKfVtUYfz5HrJ_XY2kfexJINb9lhZHMv5p1skZpeIS-GPHCC6gRlKo1q-idn_qxyusfWv7WAxlSVfQfk8d6Et0"",
                    ""dp"":""UfYKcL_or492vVc0PzwLSplbg4L3-Z5wL48mwiswbpzOyIgd2xHTHQmjJpFAIZ8q-zf9RmgJXkDrFs9rkdxPtAsL1WYdeCT5c125Fkdg317JVRDo1inX7x2Kdh8ERCreW8_4zXItuTl_KiXZNU5lvMQjWbIw2eTx1lpsflo0rYU"",
                    ""dq"":""iEgcO-QfpepdH8FWd7mUFyrXdnOkXJBCogChY6YKuIHGc_p8Le9MbpFKESzEaLlN1Ehf3B6oGBl5Iz_ayUlZj2IoQZ82znoUrpa9fVYNot87ACfzIG7q9Mv7RiPAderZi03tkVXAdaBau_9vs5rS-7HMtxkVrxSUvJY14TkXlHE"",
                    ""qi"":""kC-lzZOqoFaZCr5l0tOVtREKoVqaAYhQiqIRGL-MzS4sCmRkxm5vZlXYx6RtE1n_AagjqajlkjieGlxTTThHD8Iga6foGBMaAr5uR1hGQpSc7Gl7CF1DZkBJMTQN6EshYzZfxW08mIO8M6Rzuh0beL6fG9mkDcIyPrBXx2bQ_mM""}";
            }
        }

        // 5.1.1  Key Encryption Using RSA v1.5 and AES-HMAC-SHA2
        // https://datatracker.ietf.org/doc/html/rfc7520#section-5.1.1
        public static JsonWebKey RSA_1_5_PrivateKey
        {
            get
            {
                return new JsonWebKey(RSA_1_5_PrivateKeyJson);
            }
        }

        // 5.2.1.  Key Encryption Using RSA v1.5 and A256GCM
        // https://datatracker.ietf.org/doc/html/rfc7520#section-5.2.1
        public static string RSA_OEAP_PrivateKeyJson
        {
            get
            {
                return @"{
                    ""kty"": ""RSA"",
                    ""kid"": ""samwise.gamgee@hobbiton.example"",
                    ""use"": ""enc"",
                    ""n"": ""wbdxI55VaanZXPY29Lg5hdmv2XhvqAhoxUkanfzf2-5zVUxa6prHRrI4pP1AhoqJRlZfYtWWd5mmHRG2pAHIlh0ySJ9wi0BioZBl1XP2e-C-FyXJGcTy0HdKQWlrfhTm42EW7Vv04r4gfao6uxjLGwfpGrZLarohiWCPnkNrg71S2CuNZSQBIPGjXfkmIy2tl_VWgGnL22GplyXj5YlBLdxXp3XeStsqo571utNfoUTU8E4qdzJ3U1DItoVkPGsMwlmmnJiwA7sXRItBCivR4M5qnZtdw-7v4WuR4779ubDuJ5nalMv2S66-RPcnFAzWSKxtBDnFJJDGIUe7Tzizjg1nms0Xq_yPub_UOlWn0ec85FCft1hACpWG8schrOBeNqHBODFskYpUc2LC5JA2TaPF2dA67dg1TTsC_FupfQ2kNGcE1LgprxKHcVWYQb86B-HozjHZcqtauBzFNV5tbTuB-TpkcvJfNcFLlH3b8mb-H_ox35FjqBSAjLKyoeqfKTpVjvXhd09knwgJf6VKq6UC418_TOljMVfFTWXUxlnfhOOnzW6HSSzD1c9WrCuVzsUMv54szidQ9wf1cYWf3g5qFDxDQKis99gcDaiCAwM3yEBIzuNeeCa5dartHDb1xEB_HcHSeYbghbMjGfasvKn0aZRsnTyC0xhWBlsolZE"",
                    ""e"": ""AQAB"",
                    ""alg"": ""RSA-OAEP"",
                    ""d"": ""n7fzJc3_WG59VEOBTkayzuSMM780OJQuZjN_KbH8lOZG25ZoA7T4Bxcc0xQn5oZE5uSCIwg91oCt0JvxPcpmqzaJZg1nirjcWZ-oBtVk7gCAWq-B3qhfF3izlbkosrzjHajIcY33HBhsy4_WerrXg4MDNE4HYojy68TcxT2LYQRxUOCf5TtJXvM8olexlSGtVnQnDRutxEUCwiewfmmrfveEogLx9EA-KMgAjTiISXxqIXQhWUQX1G7v_mV_Hr2YuImYcNcHkRvp9E7ook0876DhkO8v4UOZLwA1OlUX98mkoqwc58A_Y2lBYbVx1_s5lpPsEqbbH-nqIjh1fL0gdNfihLxnclWtW7pCztLnImZAyeCWAG7ZIfv-Rn9fLIv9jZ6r7r-MSH9sqbuziHN2grGjD_jfRluMHa0l84fFKl6bcqN1JWxPVhzNZo01yDF-1LiQnqUYSepPf6X3a2SOdkqBRiquE6EvLuSYIDpJq3jDIsgoL8Mo1LoomgiJxUwL_GWEOGu28gplyzm-9Q0U0nyhEf1uhSR8aJAQWAiFImWH5W_IQT9I7-yrindr_2fWQ_i1UgMsGzA7aOGzZfPljRy6z-tY_KuBG00-28S_aWvjyUc-Alp8AUyKjBZ-7CWH32fGWK48j1t-zomrwjL_mnhsPbGs0c9WsWgRzI-K8gE"",
                    ""p"": ""7_2v3OQZzlPFcHyYfLABQ3XP85Es4hCdwCkbDeltaUXgVy9l9etKghvM4hRkOvbb01kYVuLFmxIkCDtpi-RFNhO1AoiJhYZj69hjmMRXx-x56HO9cnXNbmzNSCFCKnQmn4GQLmRj9sfbZRqL94bbtE4_e0Zrpo8RNo8vxRLqQNwIy85fc6BRgBJomt8QdQvIgPgWCv5HoQ"",
                    ""q"": ""zqOHk1P6WN_rHuM7ZF1cXH0x6RuOHq67WuHiSknqQeefGBA9PWs6ZyKQCO-O6mKXtcgE8_Q_hA2kMRcKOcvHil1hqMCNSXlflM7WPRPZu2qCDcqssd_uMbP-DqYthH_EzwL9KnYoH7JQFxxmcv5An8oXUtTwk4knKjkIYGRuUwfQTus0w1NfjFAyxOOiAQ37ussIcE6C6ZSsM3n41UlbJ7TCqewzVJaPJN5cxjySPZPD3Vp01a9YgAD6a3IIaKJdIxJS1ImnfPevSJQBE79-EXe2kSwVgOzvt-gsmM29QQ8veHy4uAqca5dZzMs7hkkHtw1z0jHV90epQJJlXXnH8Q"",
                    ""dp"": ""19oDkBh1AXelMIxQFm2zZTqUhAzCIr4xNIGEPNoDt1jK83_FJA-xnx5kA7-1erdHdms_Ef67HsONNv5A60JaR7w8LHnDiBGnjdaUmmuO8XAxQJ_ia5mxjxNjS6E2yD44USo2JmHvzeeNczq25elqbTPLhUpGo1IZuG72FZQ5gTjXoTXC2-xtCDEUZfaUNh4IeAipfLugbpe0JAFlFfrTDAMUFpC3iXjxqzbEanflwPvj6V9iDSgjj8SozSM0dLtxvu0LIeIQAeEgT_yXcrKGmpKdSO08kLBx8VUjkbv_3Pn20Gyu2YEuwpFlM_H1NikuxJNKFGmnAq9LcnwwT0jvoQ"",
                    ""dq"": ""S6p59KrlmzGzaQYQM3o0XfHCGvfqHLYjCO557HYQf72O9kLMCfd_1VBEqeD-1jjwELKDjck8kOBl5UvohK1oDfSP1DleAy-cnmL29DqWmhgwM1ip0CCNmkmsmDSlqkUXDi6sAaZuntyukyflI-qSQ3C_BafPyFaKrt1fgdyEwYa08pESKwwWisy7KnmoUvaJ3SaHmohFS78TJ25cfc10wZ9hQNOrIChZlkiOdFCtxDqdmCqNacnhgE3bZQjGp3n83ODSz9zwJcSUvODlXBPc2AycH6Ci5yjbxt4Ppox_5pjm6xnQkiPgj01GpsUssMmBN7iHVsrE7N2iznBNCeOUIQ"",
                    ""qi"": ""FZhClBMywVVjnuUud-05qd5CYU0dK79akAgy9oX6RX6I3IIIPckCciRrokxglZn-omAY5CnCe4KdrnjFOT5YUZE7G_Pg44XgCXaarLQf4hl80oPEf6-jJ5Iy6wPRx7G2e8qLxnh9cOdf-kRqgOS3F48Ucvw3ma5V6KGMwQqWFeV31XtZ8l5cVI-I3NzBS7qltpUVgz2Ju021eyc7IlqgzR98qKONl27DuEES0aK0WE97jnsyO27Yp88Wa2RiBrEocM89QZI1seJiGDizHRUP4UZxw9zsXww46wy0P6f9grnYp7t8LkyDDk8eoI4KX6SNMNVcyVS9IWjlq8EzqZEKIA""}";
            }
        }

        // 5.2.1.  Key Encryption Using RSA v1.5 and A256GCM
        // https://datatracker.ietf.org/doc/html/rfc7520#section-5.2.1
        public static JsonWebKey RSA_OEAP_PrivateKey
        {
            get
            {
                return new JsonWebKey(RSA_OEAP_PrivateKeyJson);
            }
        }

        #endregion Keys

        // 4.  JSON Web Signature Examples
        // https://datatracker.ietf.org/doc/html/rfc7520#section-4
        public static string Payload
        {
            get { return "It\u2019s a dangerous business, Frodo, going out your door. You step onto the road, and if you don't keep your feet, there\u2019s no knowing where you might be swept off to."; }
        }

        // 4.  JSON Web Signature Examples
        // https://datatracker.ietf.org/doc/html/rfc7520#section-4
        public static string PayloadEncoded
        {
            get { return "SXTigJlzIGEgZGFuZ2Vyb3VzIGJ1c2luZXNzLCBGcm9kbywgZ29pbmcgb3V0IHlvdXIgZG9vci4gWW91IHN0ZXAgb250byB0aGUgcm9hZCwgYW5kIGlmIHlvdSBkb24ndCBrZWVwIHlvdXIgZmVldCwgdGhlcmXigJlzIG5vIGtub3dpbmcgd2hlcmUgeW91IG1pZ2h0IGJlIHN3ZXB0IG9mZiB0by4"; }
        }

        #region 4.1.2

        // 4.1.2.  Signing Operation
        // https://datatracker.ietf.org/doc/html/rfc7520#section-4.1.2
        public static string RSAHeaderJson
        {
            get { return @"{""alg"":""RS256"",""kid"":""bilbo.baggins@hobbiton.example""}"; }
        }

        public static string RSAHeaderEncoded
        {
            get { return "eyJhbGciOiJSUzI1NiIsImtpZCI6ImJpbGJvLmJhZ2dpbnNAaG9iYml0b24uZXhhbXBsZSJ9"; }
        }

        public static string RSAEncoded
        {
            get { return RSAHeaderEncoded + "." + PayloadEncoded; }
        }

        public static JwtHeader RSAJwtHeader
        {
            get
            {
                var header = new JwtHeader();
                header.Clear();
                header["alg"] = "RS256";
                header["kid"] = "bilbo.baggins@hobbiton.example";
                return header;
            }
        }

        public static string RSASignatureEncoded
        {
            get { return "MRjdkly7_-oTPTS3AXP41iQIGKa80A0ZmTuV5MEaHoxnW2e5CZ5NlKtainoFmKZopdHM1O2U4mwzJdQx996ivp83xuglII7PNDi84wnB-BDkoBwA78185hX-Es4JIwmDLJK3lfWRa-XtL0RnltuYv746iYTh_qHRD68BNt1uSNCrUCTJDt5aAE6x8wW1Kt9eRo4QPocSadnHXFxnt8Is9UzpERV0ePPQdLuW3IS_de3xyIrDaLGdjluPxUAhb6L2aXic1U12podGU0KLUQSE_oI-ZnmKJ3F4uOZDnd6QZWJushZ41Axf_fcIe8u9ipH84ogoree7vjbU5y18kDquDg"; }
        }

        #endregion 4.1.2

        #region 4.3.2

        // 4.3.2.  Signing Operation
        // https://datatracker.ietf.org/doc/html/rfc7520#section-4.3.2

        public static string ES512HeaderJson
        {
            get { return @"{""alg"":""ES512"",""kid"":""bilbo.baggins@hobbiton.example""}"; }
        }

        public static string ES512HeaderEncoded
        {
            get { return "eyJhbGciOiJFUzUxMiIsImtpZCI6ImJpbGJvLmJhZ2dpbnNAaG9iYml0b24uZXhhbXBsZSJ9"; }
        }

        public static string ES512Encoded
        {
            get { return ES512HeaderEncoded + "." + PayloadEncoded; }
        }

        public static JwtHeader ES512JwtHeader
        {
            get
            {
                var header = new JwtHeader();
                header.Clear();
                header["alg"] = "ES512";
                header["kid"] = "bilbo.baggins@hobbiton.example";
                return header;
            }
        }

        public static string ES512SignatureEncoded
        {
            get { return "AE_R_YZCChjn4791jSQCrdPZCNYqHXCTZH0-JZGYNlaAjP2kqaluUIIUnC9qvbu9Plon7KRTzoNEuT4Va2cmL1eJAQy3mtPBu_u_sDDyYjnAMDxXPn7XrT0lw-kvAD890jl8e2puQens_IEKBpHABlsbEPX6sFY8OcGDqoRuBomu9xQ2"; }
        }

        #endregion 4.3.2

        #region 4.4.2

        //4.4.2.  Signing Operation
        //https://datatracker.ietf.org/doc/html/rfc7520#section-4.4.1
        public static string SymmetricEncoded
        {
            get { return SymmetricHeaderEncoded + "." + PayloadEncoded; }
        }

        public static string SymmetricHeaderJson
        {
            get { return @"{""alg"":""HS256"",""kid"":""018c0ae5-4d9b-471b-bfd6-eef314bc7037""}"; }
        }

        public static string SymmetricHeaderEncoded
        {
            get { return "eyJhbGciOiJIUzI1NiIsImtpZCI6IjAxOGMwYWU1LTRkOWItNDcxYi1iZmQ2LWVlZjMxNGJjNzAzNyJ9"; }
        }

        public static JwtHeader SymmetricJwtHeader
        {
            get
            {
                var header = new JwtHeader();
                header.Clear();
                header["alg"] = "HS256";
                header["kid"] = "018c0ae5-4d9b-471b-bfd6-eef314bc7037";
                return header;
            }
        }

        public static string SymmetricSignatureEncoded
        {
            get { return "s0h6KThzkfBBBkLspW1h84VsJZFTsPPqMDA7g1Md7p0"; }
        }

        #endregion 4.4.2

        #region 5.1

        // 5.1.2.  Generated Factors
        // https://datatracker.ietf.org/doc/html/rfc7520#section-5.1.2
        public static string RSA_1_5_CEKEncoded
        {
            get { return "3qyTVhIWt5juqZUCpfRqpvauwB956MEJL2Rt-8qXKSo"; }
        }

        public static string RSA_1_5_IVEncoded
        {
            get { return "bbd5sTkYwhAIqfHsx8DayA"; }
        }

        // 5.1.3.  Encrypting the Key
        public static string RSA_1_5_CEKEncryptedEncoded
        {
            get { return "laLxI0j-nLH-_BgLOXMozKxmy9gffy2gTdvqzfTihJBuuzxg0V7yk1WClnQePFvG2K-pvSlWc9BRIazDrn50RcRai__3TDON395H3c62tIouJJ4XaRvYHFjZTZ2GXfz8YAImcc91Tfk0WXC2F5Xbb71ClQ1DDH151tlpH77f2ff7xiSxh9oSewYrcGTSLUeeCt36r1Kt3OSj7EyBQXoZlN7IxbyhMAfgIe7Mv1rOTOI5I8NQqeXXW8VlzNmoxaGMny3YnGir5Wf6Qt2nBq4qDaPdnaAuuGUGEecelIO1wx1BpyIfgvfjOhMBs9M8XL223Fg47xlGsMXdfuY-4jaqVw"; }
        }

        public static string RSA_1_5_ProtectedHeaderJSON
        {
            get
            {
                return @"{""alg"":""RSA1_5"",""kid"":""frodo.baggins@hobbiton.example"",""enc"":""A128CBC-HS256""}";
            }
        }

        public static string RSA_1_5_ProtectedHeaderEncoded
        {
            get
            {
                return "eyJhbGciOiJSU0ExXzUiLCJraWQiOiJmcm9kby5iYWdnaW5zQGhvYmJpdG9uLmV4YW1wbGUiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2In0";
            }
        }

        public static string RSA_1_5_CipherTextEncoded
        {
            get
            {
                return "0fys_TY_na7f8dwSfXLiYdHaA2DxUjD67ieF7fcVbIR62JhJvGZ4_FNVSiGc_raa0HnLQ6s1P2sv3Xzl1p1l_o5wR_RsSzrS8Z-wnI3Jvo0mkpEEnlDmZvDu_k8OWzJv7eZVEqiWKdyVzFhPpiyQU28GLOpRc2VbVbK4dQKPdNTjPPEmRqcaGeTWZVyeSUvf5k59yJZxRuSvWFf6KrNtmRdZ8R4mDOjHSrM_s8uwIFcqt4r5GX8TKaI0zT5CbL5Qlw3sRc7u_hg0yKVOiRytEAEs3vZkcfLkP6nbXdC_PkMdNS-ohP78T2O6_7uInMGhFeX4ctHG7VelHGiT93JfWDEQi5_V9UN1rhXNrYu-0fVMkZAKX3VWi7lzA6BP430m";
            }
        }

        public static string RSA_1_5_AuthenticationTagEncoded
        {
            get
            {
                return "kvKuFBXHe5mQr4lqgobAUg";
            }
        }

        public static string RSA_1_5_JWE
        {
            get
            {
                return "eyJhbGciOiJSU0ExXzUiLCJraWQiOiJmcm9kby5iYWdnaW5zQGhvYmJpdG9uLmV4YW1wbGUiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2In0."
                     + "laLxI0j-nLH-_BgLOXMozKxmy9gffy2gTdvqzfTihJBuuzxg0V7yk1WClnQePFvG2K-pvSlWc9BRIazDrn50RcRai__3TDON395H3c62tIouJJ4XaRvYHFjZTZ2GXfz8YAImcc91Tfk0WXC2F5Xbb71ClQ1DDH151tlpH77f2ff7xiSxh9oSewYrcGTSLUeeCt36r1Kt3OSj7EyBQXoZlN7IxbyhMAfgIe7Mv1rOTOI5I8NQqeXXW8VlzNmoxaGMny3YnGir5Wf6Qt2nBq4qDaPdnaAuuGUGEecelIO1wx1BpyIfgvfjOhMBs9M8XL223Fg47xlGsMXdfuY-4jaqVw."
                     + "bbd5sTkYwhAIqfHsx8DayA."
                     + "0fys_TY_na7f8dwSfXLiYdHaA2DxUjD67ieF7fcVbIR62JhJvGZ4_FNVSiGc_raa0HnLQ6s1P2sv3Xzl1p1l_o5wR_RsSzrS8Z-wnI3Jvo0mkpEEnlDmZvDu_k8OWzJv7eZVEqiWKdyVzFhPpiyQU28GLOpRc2VbVbK4dQKPdNTjPPEmRqcaGeTWZVyeSUvf5k59yJZxRuSvWFf6KrNtmRdZ8R4mDOjHSrM_s8uwIFcqt4r5GX8TKaI0zT5CbL5Qlw3sRc7u_hg0yKVOiRytEAEs3vZkcfLkP6nbXdC_PkMdNS-ohP78T2O6_7uInMGhFeX4ctHG7VelHGiT93JfWDEQi5_V9UN1rhXNrYu-0fVMkZAKX3VWi7lzA6BP430m."
                     + "kvKuFBXHe5mQr4lqgobAUg";
            }
        }

        #endregion 5.1
    }

    public static class EncodedJwts
    {
        public static string Asymmetric_LocalSts { get => @"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1ZCI6Imh0dHA6Ly9Db250b3NvLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvdW50cnkiOiJVU0EiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGNvbnRvc28uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiVG9ueSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2hvbWVwaG9uZSI6IjU1NS4xMjEyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU2FsZXMiLCJzdWIiOiJib2IifQ.QW0Wfw-R5n3BHXE0vG-0giRFeB6W9oFrWJyFTaLI0qICDYx3yZ2eLXJ3zNFLVf3OG-MqytN5tqUdNfK1mRzeubqvdODHLFX36e1o3X8DR_YumyyQvgSeTJ0wwqT8PowbE3nbKfiX4TtJ4jffBelGKnL6vdx3AU2cwvLfSVp8ppA"; }
        public static string Asymmetric_1024 =      @"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1ZCI6Imh0dHA6Ly9Db250b3NvLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvdW50cnkiOiJVU0EiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGNvbnRvc28uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiVG9ueSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2hvbWVwaG9uZSI6IjU1NS4xMjEyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU2FsZXMiLCJzdWIiOiJib2IifQ.WlNiBiAqmS4G-Em5O-uYiWLK5CJO8B-6Hvqjv_DXpoxldGiMWzivuyJocXPIIDVbcLxovmTc5j0KKgA9foOFBSkEEasqESA0VTYE30T1kkrGOaElola5DZagzax2zDipjxhbtBdMsvgF2t6GQJKyF0oFt828_yRGUsUnaXxg_MY";
        public static string Asymmetric_2048 =      @"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1ZCI6Imh0dHA6Ly9Db250b3NvLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvdW50cnkiOiJVU0EiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGNvbnRvc28uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiVG9ueSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2hvbWVwaG9uZSI6IjU1NS4xMjEyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU2FsZXMiLCJzdWIiOiJib2IifQ.XYeDHk0XRs1ybrk2AMWu3ZwNC6gPUYqxacJtUDSfQCGouRFdmkYtZcgvWAhH8iFv3DmPgfX0lI9WCtjN2JOZqOx5w90r9UKCh_9e_vUKZyjLkyUEv3iBl2HTpxfcj3ns5MmZI50N8O2cYq1d6-CRK_oi8oKhLWKfrD8LoMpCtV8zjraEB1GUfJvMrxPTIzHSF-V_nmu5aPIoHVyxAcc1jShkYdnS5Dz8nVqLBleCAQ2Tv-8N9Q8l1362b088y15auc-hBb76KmMU2aCutyJDRz0NqsCkFz-cV-vnIj-hzl562DzSUP48nEMTwEIO_bRKex1R5beZ36ZrKLP1GQxc8Q";
        public static string Symmetric_256   =      @"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1ZCI6Imh0dHA6Ly9Db250b3NvLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvdW50cnkiOiJVU0EiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGNvbnRvc28uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiVG9ueSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2hvbWVwaG9uZSI6IjU1NS4xMjEyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU2FsZXMiLCJzdWIiOiJib2IifQ._IFPA82MzKeV4IrsgZX8mkAEfzWT8-zEE4b5R2nzih4";
        public static string InvalidHeader =        @"eyJcdWQiOiJodHRwOi8vbG9jYWxob3N0L1JQIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdC9TdHMiLCJuYm.eyJpc3MiOiJodHRwOi8vR290Snd0LmNvbSIsImF1ZCI6Imh0dHA6Ly9Db250b3NvLmNvbSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2NvdW50cnkiOiJVU0EiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ1c2VyQGNvbnRvc28uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiVG9ueSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2hvbWVwaG9uZSI6IjU1NS4xMjEyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiU2FsZXMiLCJzdWIiOiJib2IifQ.QW0Wfw-R5n3BHXE0vG-0giRFeB6W9oFrWJyFTaLI0qICDYx3yZ2eLXJ3zNFLVf3OG-MqytN5tqUdNfK1mRzeubqvdODHLFX36e1o3X8DR_YumyyQvgSeTJ0wwqT8PowbE3nbKfiX4TtJ4jffBelGKnL6vdx3AU2cwvLfSVp8ppA"; 
        public static string InvalidPayload =       @"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsIng1dCI6InZ4VThJR1pYdEFtemg0NzdDT05CR2dYRTlfYyJ9.eyJcdWQiOiJodHRwOi8vbG9jYWxob3N0L1JQIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdC9TdHMiLCJuYmYiOjEzNjcyODA0MDUsImV4cCI6MTM2NzMwOTIwNSwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiYWFsIn0.Pro66IUD94jvZNnG_l96Hph78L_LYSx6eobO6QfWF3y038ebLZorhKYgAj1LtsNVAbq7E_I5tnoI1Y4YUV5_wMGtMqT_XTB4N8vktDzf0Y32MhopsDrveofJAAFAUP1npYZtFF89RAWzy1GaXqXw05SbUcyMPWTSvmPk_frzJRTc-utAaBAp-zKqS1KXGB_s99x7lDxy3ZFMDFtFHQlOJiXeClXYCVkB-ZmvrSFSAIasFK4eIG9pOcMY43_wS7ybNjF7WncY6PEi6JmUoh2AwA-SCdY-Bhs80Tf4GMB2HsmuMkSVgoptt6Fgf-q8LhWG0W80g66JRgdhMj85BZ6bxg";
        public static string LiveJwt        =       @"eyJhbGciOiJIUzI1NiIsImtpZCI6IjAiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTM2ODY0ODg2MywidWlkIjoiMzgwZTE3YzMxNGU2ZmMyODA0NzA3MjI5NTc3MjEwZmIiLCJhdWQiOiJ3d3cuc3JpLWRldjEwMC5jb20iLCJ1cm46bWljcm9zb2Z0OmFwcHVyaSI6Im1zLWFwcDovL1MtMS0xNS0yLTM2MzczOTQzNzAtMjIzMTgyMTkzNi01NjUwMTU1MS0xNTE0NjEzNDgyLTQ1NjgzNjc4LTM1NzUyNjE4NTItMjMzNTgyNjkwIiwidXJuOm1pY3Jvc29mdDphcHBpZCI6IjAwMDAwMDAwNEMwRTdBNUMifQ.I-sE7t6IJUho1TfgaLilNuzro-pWOMgg33rQ351GcoM";
        public static string OverClaims =           @"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImtyaU1QZG1Cdng2OHNrVDgtbVBBQjNCc2VlQSJ9.eyJhdWQiOiJodHRwczovL2dyYXBoLndpbmRvd3MubmV0IiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzJmOTg4YmYtODZmMS00MWFmLTkxYWItMmQ3Y2QwMTFkYjQ3LyIsImlhdCI6MTQwNTk2ODkyMiwibmJmIjoxNDA1OTY4OTIyLCJleHAiOjE0MDU5NzI4MjIsInZlciI6IjEuMCIsInRpZCI6IjcyZjk4OGJmLTg2ZjEtNDFhZi05MWFiLTJkN2NkMDExZGI0NyIsImFtciI6WyJwd2QiXSwib2lkIjoiMzVjNzZlZWQtZjY0MC00YWU3LWFhZTItMzI3NzE3MWVhM2U1IiwidXBuIjoibmJhbGlnYUBtaWNyb3NvZnQuY29tIiwidW5pcXVlX25hbWUiOiJuYmFsaWdhQG1pY3Jvc29mdC5jb20iLCJzdWIiOiI1R0UwVkhBSlBuaUdNSWluN3dMNFBFMFE5MjAzTG00bHJBUnBrcEFBYmprIiwicHVpZCI6IjEwMDM3RkZFODAxQjI4QTAiLCJmYW1pbHlfbmFtZSI6IkJhbGlnYSIsImdpdmVuX25hbWUiOiJOYW1yYXRhIiwiX2NsYWltX25hbWVzIjp7Imdyb3VwcyI6InNyYzEifSwiX2NsYWltX3NvdXJjZXMiOnsic3JjMSI6eyJlbmRwb2ludCI6Imh0dHBzOi8vZ3JhcGgud2luZG93cy5uZXQvNzJmOTg4YmYtODZmMS00MWFmLTkxYWItMmQ3Y2QwMTFkYjQ3L3VzZXJzLzM1Yzc2ZWVkLWY2NDAtNGFlNy1hYWUyLTMyNzcxNzFlYTNlNS9nZXRNZW1iZXJPYmplY3RzIn19LCJhcHBpZCI6IjExOGUxNzBmLWNmMjYtNDAwZi1hMGU5LTk2OTEwYjMxMTg3ZSIsImFwcGlkYWNyIjoiMSIsInNjcCI6IlVzZXJQcm9maWxlLlJlYWQiLCJhY3IiOiIxIn0.PWNfaBajC6KAr2dKiG0aJ1295hIXm9XWZPdrCw6zMgT0s46rrcBFMWOJQ-4Cz1aSqour6tslg8cl4_1rAjlkVwsXs7QTekMHxIcf3SPpM6vPTa7OfQ4dzBbPQV_QKif1xBXDkFQfZPAF2tPwcK_VBzHT0Z94_CpOtxChXmGEctW38Rt6f8bC_aaD6nsTZOt6NdAmI2AVOchpp7qNWEdBTvdcoNyz_a5VbUwWsHGCvozcOLjjFLles-K0BhiFw3MyJU_DMG-H6TgeBtwJPiuU2vHUTea26sfKHbpe7GypBo1PjY7odDWMH-d7c1Z0fT-UL15dAV419zX1NGbl-cujsw";
        public static string Cyrano =               @"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImtyaU1QZG1Cdng2OHNrVDgtbVBBQjNCc2VlQSJ9.eyJhdWQiOiJmZTc4ZTBiNC02ZmU3LTQ3ZTYtODEyYy1mYjc1Y2VlMjY2YTQiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9hZGQyOTQ4OS03MjY5LTQxZjQtODg0MS1iNjNjOTU1NjQ0MjAvIiwiaWF0IjoxNDE5MjY4NTIwLCJuYmYiOjE0MTkyNjg1MjAsImV4cCI6MTQxOTI3MjQyMCwidmVyIjoiMS4wIiwidGlkIjoiYWRkMjk0ODktNzI2OS00MWY0LTg4NDEtYjYzYzk1NTY0NDIwIiwiYW1yIjpbInB3ZCJdLCJvaWQiOiI4MDAyNzk2NC1jZDcwLTRmMmMtOTcwMC0yYzFhNmRiNTZlZjYiLCJ1cG4iOiJib2JAY3lyYW5vLm9ubWljcm9zb2Z0LmNvbSIsInVuaXF1ZV9uYW1lIjoiYm9iQGN5cmFuby5vbm1pY3Jvc29mdC5jb20iLCJzdWIiOiJoMnh6WVczbWdUWmZad3B3T1d4QTFZcDJ0am9Xc0ZxOWlGa1AxTjJRUndrIiwiZmFtaWx5X25hbWUiOiJDeXJhbm8iLCJnaXZlbl9uYW1lIjoiQm9iIiwibm9uY2UiOiI2MzU1NDg2NTYxMzk1MzcwNDYuT0RZMU5EWTRaRGt0T1RNNE55MDBaR0l5TFRnMk1EQXRZakkxTWpNME9HVXhOVGRtTkRVek5USXlNR0V0WldJd1lTMDBNMkpoTFRobE4yUXRaVFEwWWpJMk1tRTFaak16IiwiY19oYXNoIjoiMXVHNEVfWWdYcTZkVUctTExzeGtjQSIsInB3ZF9leHAiOiI1MzQ1MDIiLCJwd2RfdXJsIjoiaHR0cHM6Ly9wb3J0YWwubWljcm9zb2Z0b25saW5lLmNvbS9DaGFuZ2VQYXNzd29yZC5hc3B4In0.juYFCrJbDPwqZeNmR9XiFRh3iobf76fKHrE4ViqELbuz0cHhAWzntR_kshoyCCBx5Q_uQcAYnrUyvHuXsQoLqUHot6Ksnlc7uUFAeWBgSIAIRX2np-fCn0_CzgwgvBu9KOUV27uu28tEPBfxHCmU9CCH41aSLoGzGBiorQ_ss0LO3ZapLiB5T2yRaJh-ZCSuGbjTCvMAmUFx4I2rvHSNaJQOqUT02EjkHzU3qAJuYSH1Z_G36Bfyiixpbyq8Txewqaot0sHCwOrBY9yjTx8Ijrnbn7_xQHV2LyvUnSxZjL0bVUZRmWyXJ0st7Cjd9intcMYb60XSmkZwLfKzMtBY2Q";
        public static string ValidJweDirect =       @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..tAYQP0lh6N8FPkuKzb0A6Q.QLHEhzxxL5q05YE6Wuo-eHmvGPGvcexii-fi-SAXI0UeR-TTaFtxgjsOZ8-d4FEujB2InS6X5CLVX6_x216Ze5lGPX5XegKq6d7mwlAmMqqtz7yHnFzSi_rZr_0uBbOlDmdRC98ilNVNjORy1u-6V_aSZTdUGjWNg3Y1gP9q_OUl-Tu9QLWvNI1cAP_PRhLn46b9RpZwUYCCF9a4qpkAZOje5X77aoy55Yw3m-HkTjR6t1j2mU1p0KK3ScW7Lrv3SeQllG5yHEdBwg8E9y9ssJyEaT5GNRQHm0n6wHIkc478pmHe1ME7wt7bX58mqOprKY-bYB1HNnW3PgXfaL-AUXxlCyk7LjLcmd2j0NUBKx17taBERSFF1sH07ynXZTlP0NYZGAr_ChrO-yY1TwMZbdoCOoUKBqcMpv7yjebhq4_50PhyLka7ZfJ5s4quYijfzlBavdnMrOVeoRwJF-kpnVRJJDdpmHTVJJuoKSnHAcDIZ5N17z6SDiKzm6TZEtaQSjOtJOn5hrRAuI4av4nzTAYXc9YqBjlozLQQ9P2SzRJh1wpKFP7XqSBVW2DvlQ_GTZS_qXTlI-fv2NG4dZpno34d-WFlcyJsw2uDeR7mi2ej8rYljH_svWK2cyZXXEyoHUzI8rebzoIS61LoFeMMVtAgpXYSsQUdq4GqPhxcI21_JTGvwJpSTXGtW5s44rwfF3jeZ7KJLSIM14A0OlzNlJX0l-UJ250bmaQ5WNxc6SemvofO4AokC-BuGtNlkM9DvJJ_YvmBLH_BGaK6ENOoyAtJ2fHrcUwKraKc_YErOLBuCrwTHe9ScCp5MHcZZoX3UmNXsX5iWL9qXziXHvlWSIRaYTJwPVa021F0B-Rnccibr_bF7PaXHnR0GIS1MvHJ.SnoMW7P4IcWMt78st0WDFQ";
        public static string ValidJweDirect2 =      @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..NrwIBN5FBvFaWFfK5rcazw.ufmYVG_7gYQToytSrczL4KMWm5LoZ-DWe4Zbos4s3IYBwN4mhxI6tlHj7M4jlU8XYfoT2xq_q3cMaGPIcAHDLrgB1EIfI0L8cWOTTxAnPfAuBbaJuuHCI2OnpCMIKUKKQ-uiZm0MWIWuGUg6pFHtzxysis_grKLLnkssFFPvmxr1ysd01fNlealKoSwqO99OiLIz9hlHnLeqF3c3C07r7tuB-xT95ixtZQBfXI7iCUHlhU30T9dG-m6SwUh0LvYPGZnC987eyUpSNJ-C0bnqEig9KY_cmB9yuQ1UvoWYQzDuRDATOF9UK3s7J32mcGSqehDnsGGFwzTHdwaV7KA1dYRmm85NWupNTLdfmvTNPDRCPj5VPWzNpKfee7MMEZn81J1695N8oVBJe4qwjK2d4gqA3A1mxpgZcK9C1jc4aU2OTkVbjJstPKatqJ-kdl3-L4TJStBcO6LS9nLcO8DvGVXX0XBc2ZBliwLL2mMZ2yIXiwuOtF3UYe8jXCr9nxTX2yLVMtsOFpk-8j3cNxFtfnU8yCE84saldtwDy9X896Caa2kSBRR-tdUSqYA3Zp0xCw3XQ6zuYriFOHxTR3AmwWrxxeaGF1hbkJfoBZriPpD-qvPTiWHUPOsJYDWJ8iY13G2-CQ2lcKXP_CZKi0L9fdXhx8qccBt89DUCUuMo9FjQLS3Mh2HvbkYVouSwUnHnMpnKehaiH0O_bQR0pRCdV6sfVuMPMsO4tARK1rUlIjROlZBKyLyNeR_YFZ50U1SSaAxnsnsk2MucTt5lfOUjFhXko-qWwHnib61lFJlnTMR-ntImTIj1XyMUcf2RZ6HdlZbvXjA92fRaQ2-3Mc7eeJscT3mW4W4Yoe3BccHEmyzSCHVNsskCF0AULjSs-bcE.6gXNNIY0ZAXi6tSbz6l6Fg";
        public static string ValidJwe =             @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string ValidJwe2 =            @"eyJhIjoiYiJ9..eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string InvalidJwe =           @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9"; // 4 parts
        public static string InvalidJwe2 =          @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9"; // 6 parts
        public static string InvalidJwe3 =          @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9*&"; // invalid characters
        public static string InvalidJwe4 =          @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9."; // extra period at end
        public static string InvalidJwe5 =          @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9."; // empty 5th segment
        public static string InvalidJwe6 =          @"eyJhIjoiYiJ9..eyJhIjoiYiJ9.eyJhIjoiYiJ9."; // empty 2nd and 5th segment
        public static string JweTest1 =             @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..CiER3OL0oLzoaO-wk0zXtg.xYf9svt5_wzDIk8QjeENnKYUosWDgDjTf-Y2ULc5OJcnDzZdBpeq4y5RS0AUfB6HNMnfiMfwFpCO5TcMV2kWpLqHNvxc9wEcdBdOQFBdcwo2zbvMV4IjVfBUlyxqkO1tuRM2NkJBNbN90fHvr5D-P2Hnbsc6q5ev0nv8ZFAOAUlcXWr-KM6-Gy7jAXVduBzjkFYRMTvM2H2pRLTvXYcwmczIyrklShxKxX8ZzDkHK71X1l1yiXbvPlN16eC5TglFbW_iqCe-TONjVc8R5AQXoanguCez_imhuvqPbX8TwHGXWjx75A8lGDRFYkJpIulpF2LmCF-wausQRuNK6PmulZbqjb4yV7VyDJ8oypO_pMKzibfRidEO-fV--_PbflCKPyOW0q7y76BjVj-HnLb3fv7yL0DSD5pJbrEop5mhSyQGGvEJas2svrlriZxzQQXpNChg4j6DSE95uxgAeftg3dSP8XnscfKdye4ufyCkF37akt6PpHLqakoV0zh4OA07MnID8Tyhgp9axqhilxuAvxn9iQuEYVYL30RmNWBMNbFnPhr235_E33JyCYP3nvrQ2QnfCZmFpJx8NpH_59kZf3mh2p3BDrp5Pi4hUb8LRgBRqcZ3oJVyXGWbE0Rrj16vzF5Z5rynkh7hAB7dyP_hjdsLIZWsr9jzzwx7EpCZuVQ5Hgo6VLBo7oaNWjOEzgTTHckUoR-tawXXtxNA8pbRdw2WFv_IwGDhNAKs4rtpVg-kS_Pg_vRSjV6X5qGbYVRlwf5nm0Mk9wlt0OWtBY0lLu-B6820lM8aOt0d7YxA3f9gssImrBNDjpstzqnptWqaXKG-ngzZyrPQhTn_3vZJsCFlXrAPBVJVAdRUtvN4vbcZ8z3gldCBpdYW5d70_g7p.uKWeIHpRh3NqXRVfc5y_rw";
        public static string JweTest2 =             @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..HksrbloZr9KUuYlpb3AbdA.aBtVk9Aad6qR81MVQ2NvZVQxEOD8x3_YC2qCGHKTFasjQPvhEbFhm_tZHHOOsRh1c7PG-atrHE6vcG0op8NRgZGpBJAzT8uwmRmJ3w-FuG_u4nfttg8qfD1OfA_4R82vRh6iXg7ZzviVInIa8ZCVntdWjoMN3hManuLdVIYFAWkG4J2Vy0tuGmjbamvbx9MSJWHO84um7Szz03dUai99aKPKAR43PeN3JlXvA95MXAJzY973B7OviFRsYi1MryX_6FUt_OVvJQMJsjUADQeyesgUNw3GP9xT4KI8NjBW8LJ4q2l3as0ztmzJKQWAnvSLSfJNgWpnQrFTX3qThylIqUESshMJjCHQKW6WO7NOFt2RrgR9v1omw-1S8cV1m4SKNnJOqmRF3ZijNJjGpzaPIEfDHzsE0MwU67_-f-uVAlTJzZnxax8d-7KEkd0KZCcO_ILL1xWKxDkdxGy51WcJwBOTcx0x1jpuAOwIi0wT9kTSDw7WpH3T0VpCnbjB1K8MQYrn1y9vkT3SG6IjRVrJnyo_pk8RuSnKRtFFNwAbE4JqwCQg5wthcJ9M1nO1aMgfIrnl7EEbbEaP3PnZTrrZ1UxiIXmk0xocIFDqHxGtMC-Rs6uJ67gUhAxMdi5iji5Ogrencfjat1azGH_89nRETDF0WjAs6EOTWpHB5jp0xx684kcYT4EbUp-ms0XxmcxV7oyUnkM9jxJBmbSEZoS5Dec6dO5sM6J4G5QI6U1_1edzi886mxZg25RA3AGCwjbXAW-zKUiPUP4Xu8TCRsMzpNocDV5dJ9cCb8zLpmtKclckcSVjd27zU3twGl65yS0uRdKradP99npd3rBmdeCgJyJwDJ2lAJpY804LQJSztt81caOnv-fOAI-7MEBQgVI3.EOlYXEsosb6b8tuRxMNQqA";
        public static string JweTest3 =             @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..1mLo9oZ_n0HuNKmS2SR6nw.KBkP2fD0SHWAZhzNzLx7DSaNlqwhd7aPUQ2oEEOVA0i99LGCuEBB2-mHmIOZgbkU5hGF7oCidVLM9ar8_Tek2I5-EdEDFCaT8wctBneSNiyXRJwUHOT7o_HO6f2rp4CfIAaqf_J8iM4UJtmC4eez2nxJc44oWlUvLWVS3Q73le6qiAW3ASpmJIPB47vn5YEa1AtR6I3A3X7f18yMSdtGKaLA6sUL-ZeRPAg0dNUiObMT9MqqtA_Jt61z8g-x3DSNKDvfUv6nIdo0L4KhFt7m5okXAedF-VxOUIryNEzMoYcEef78RX6Nf-5Eq71vzKIK99CFsL5uhfhk_RJl8-8wbIuJjuWByalnw7LW_0-7w7VIWtg24gHCq6mKvDdmVBgL-caWVDL2ILSR4MnJywqx11YzG74gsC-JvsjkEsZL3mH-27eiCsd_Xb2YXiGdMkmveJzlYwiQk5Uos-6kvNGWfzsxhkpGnCRbxBKjeCsj6lXTpHs_16MhACX2xdNmsLKF01waty-cQ5mufEgTsyi98CWCmRtZOs1wWLfmcGEL-j85p7ts1LLS-UPswgSJ3lFobriuSYt_oaBfcemz5emn1xe2VNme7-BvhS6i3axnY2Z6ULtLICI1AbQuSRT336m5WmWhGvu04XVkyilJRy9qUr7kKDR6Ux1PXrSpsd0GiDk3qLwmnv8N9FOROvfx3TtyXDLFuapbQwz1A37nl8Vg2kYoARqOAU73lslhbYLD_DiOIBmDREw85M2sBgTqZGCQMlFzTLxeiM0OZv-s2yMTv4fD7p_Tg1512bV1W3fe_Ja-4wmkwRXJmnF8K8m6oWnofBERieGtA_0HVcjQQh_t5Z2tc3XQEdCJxAqiiCGdmw-SX_9OMjWEjgLbvfqK_eVm.0DUduz5oR0ry2xRlaUnS7A";
        public static string JweTest4 =             @"eyJhbGciOiJkaXIiLCJlbmMiOiJBMTI4Q0JDLUhTMjU2Iiwia2lkIjoiRGVmYXVsdFN5bW1ldHJpY1NlY3VyaXR5S2V5XzI1NiIsInR5cCI6IkpXVCJ9..KgIILz-uysx_8sGUHw6Uig.Jzj6wn_rulSAuaADgUayVttM_h1CxzDeadddOaZ3sDzBu6Z0ffMQ9IKtjbpkLfzawFM0qBRjt1p75IUieTa7esZOWj5yoS8UQWg0e1n2pWpWEx9zpfeMsJjbwrsD4_tFxKRDjlAuDHYX2NehgT8CAiI2vI0dMgzIUR8VU6p7Srg_UpiZy_QnVcFG7hHhAK0R0affj6ViGhc7U_diMnpR5P3s8eG-GImB4iHMltlaUTcUqFDgSJBCmiQ0SK-lDTy9Q4Z3tGdhTZeofG9-fi68E234uznjdSj_Ql2t1E4nMOYc0kRcORlKJNQKUMLqW5ddwZwPqRd0HzccjLwnP20RqUbWntBidIdTTl94dT-1BrNGQMDMCTV7HQQxmf1JKbBN0DD0xkfOnO4UGEoHVhjOQhd1R1gAh7KtDNMJKPIdmGfMoy2SDJm_pPwmJ1ayw8rF4F5MoCDxUcKVfC25DAWOxVtnvvR4rJ9qkc9dWHr6sdFqZi7sRQIG9M7T2qqOktoZdoAEmduhii5_p5B456PsZlt8olRCsI8Xb_p4sR5clsEEMRatiUKvcXzNiVEK__17tGa2ALKGoutYb_8AasavGhz-sUjlLlHlCmaI9MA8s07MfhobBEepgNStymK1IBkl1wb1sidMmkNWSX4R18bA9J5tjAcUH0tsyyXzxix7DkKag-zl3pxxawS0NmjVIAnxXOFM_M8_DWQAySNO3atkHnMP__PneG3J-4vUoZhVeEQj66D-MCnCGzKi7YeEAjvLbPsQSeE6ptZ5lfiCmzmza0OkfeRnxRE8L1UA6-XydZ0Z6P1M7W-eV4OctVwIq1VNJeC461-D0ukouDWGBfVajW0k9Ws-U2l8Sp16TgEHkly1MnU7P7VkclldrIQ18yzd.j6_mHUzwEVeprRmNZMEY7A";
        public static string JWSEmptyHeader =       @".eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string JWSEmptyPayload =      @"eyJhIjoiYiJ9..eyJhIjoiYiJ9";
        public static string JWEEmptyHeader =       @".eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string JWEEmptyEncryptedKey = @"eyJhIjoiYiJ9..eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string JWEEmptyIV =           @"eyJhIjoiYiJ9.eyJhIjoiYiJ9..eyJhIjoiYiJ9.eyJhIjoiYiJ9";
        public static string JWEEmptyCiphertext =   @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9..eyJhIjoiYiJ9";
        public static string JWEEmptyAuthenticationTag = @"eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.eyJhIjoiYiJ9.";

        public static string JwsKidNullX5t
        {
            get
            {
                var jwtToken = new JwtSecurityToken
                (
                    Default.Issuer,
                    Default.Audience,
                    ClaimSets.Simple(Default.Issuer, Default.Issuer),
                    DateTime.UtcNow,
                    DateTime.UtcNow + TimeSpan.FromHours(10),
                    KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2
                );

                jwtToken.Header[JwtHeaderParameterNames.Kid] = null;
                jwtToken.Header[JwtHeaderParameterNames.X5t] = KeyingMaterial.DefaultCert_2048.Thumbprint;

                return (new JwtSecurityTokenHandler()).WriteToken(jwtToken);
            }
        }

        public static string JwsKidLowercase
        {
            get
            {
                var jwtToken = new JwtSecurityToken
                (
                    Default.Issuer,
                    Default.Audience,
                    ClaimSets.Simple(Default.Issuer, Default.Issuer),
                    DateTime.UtcNow,
                    DateTime.UtcNow + TimeSpan.FromHours(10),
                    KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2
                );

                jwtToken.Header[JwtHeaderParameterNames.Kid] = jwtToken.Header.Kid.ToLower();
                jwtToken.Header[JwtHeaderParameterNames.X5t] = KeyingMaterial.DefaultCert_2048.Thumbprint;

                return (new JwtSecurityTokenHandler()).WriteToken(jwtToken);
            }
        }
    }

    public static class Saml2SignedTokens
    {
        public static readonly string AAD_GotJWT = @"<Assertion ID='_775bef7d-0ba5-4df5-98f8-ec57f2b0cbef' IssueInstant='2013-04-11T20:02:45.903Z' Version='2.0' xmlns='urn:oasis:names:tc:SAML:2.0:assertion'><Issuer>https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/</Issuer><ds:Signature xmlns:ds='http://www.w3.org/2000/09/xmldsig#'><ds:SignedInfo><ds:CanonicalizationMethod Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#' /><ds:SignatureMethod Algorithm='http://www.w3.org/2001/04/xmldsig-more#rsa-sha256' /><ds:Reference URI='#_775bef7d-0ba5-4df5-98f8-ec57f2b0cbef'><ds:Transforms><ds:Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' /><ds:Transform Algorithm='http://www.w3.org/2001/10/xml-exc-c14n#' /></ds:Transforms><ds:DigestMethod Algorithm='http://www.w3.org/2001/04/xmlenc#sha256' /><ds:DigestValue>m1jXR5LORy7H+t+axtNauGnJakaKehvy0JhMZomZFrU=</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>IhaRBWAG/QeskVPO5Qa3EGN0A/bJ8qPqmJnuhox5F3aAGgqeoxbMhdwDMuQ7GC/7ScYDtYnAKATV6xP0nKTzwlqj1ltDEDN9scDuJIotwnV+xm4PCqlIeSZBHfyhIDp0rlhZDmhOnNy6+53FqVWMo7aSE1FdMCEh2aBgTRLH4jBkx5pKV0kQOX9D0Hq2GaV0LaXjcguADKxPpb1Gl+g8g/H/79n96fHAPITA8WygZjv1Satow87N8WK49n9qZJBWqmMjaVix6oFHPMOkp/qWE7ZhWKS1ANU2AwLMvZ8AH+RDwhWTWHnLCq2duD4XlCTRiY8juIn+zgJ51sfWQRYa2w==</ds:SignatureValue><KeyInfo xmlns='http://www.w3.org/2000/09/xmldsig#'><X509Data><X509Certificate>MIIDPjCCAiqgAwIBAgIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTIwNjA3MDcwMDAwWhcNMTQwNjA3MDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArCz8Sn3GGXmikH2MdTeGY1D711EORX/lVXpr+ecGgqfUWF8MPB07XkYuJ54DAuYT318+2XrzMjOtqkT94VkXmxv6dFGhG8YZ8vNMPd4tdj9c0lpvWQdqXtL1TlFRpD/P6UMEigfN0c9oWDg9U7Ilymgei0UXtf1gtcQbc5sSQU0S4vr9YJp2gLFIGK11Iqg4XSGdcI0QWLLkkC6cBukhVnd6BCYbLjTYy3fNs4DzNdemJlxGl8sLexFytBF6YApvSdus3nFXaMCtBGx16HzkK9ne3lobAwL2o79bP4imEGqg+ibvyNmbrwFGnQrBc1jTF9LyQX9q+louxVfHs6ZiVwIDAQABo2IwYDBeBgNVHQEEVzBVgBCxDDsLd8xkfOLKm4Q/SzjtoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQVWmXY/+9RqFA/OG9kFulHDAJBgUrDgMCHQUAA4IBAQAkJtxxm/ErgySlNk69+1odTMP8Oy6L0H17z7XGG3w4TqvTUSWaxD4hSFJ0e7mHLQLQD7oV/erACXwSZn2pMoZ89MBDjOMQA+e6QzGB7jmSzPTNmQgMLA8fWCfqPrz6zgH+1F1gNp8hJY57kfeVPBiyjuBmlTEBsBlzolY9dd/55qqfQk6cgSeCbHCy/RU/iep0+UsRMlSgPNNmqhj5gmN2AFVCN96zF694LwuPae5CeR2ZcVknexOWHYjFM0MgUSw0ubnGl0h9AJgGyhvNGcjQqu9vd1xkupFgaN+f7P3p3EVN5csBg5H94jEcQZT7EKeTiZ6bTrpDAnrr8tDCy8ng</X509Certificate></X509Data></KeyInfo></ds:Signature><Subject><NameID>X3hlFkILohbCj9c2Iib1NJg7e3hGhVsJdHuA7WRABp4</NameID><SubjectConfirmation Method='urn:oasis:names:tc:SAML:2.0:cm:bearer' /></Subject><Conditions NotBefore='2013-04-11T20:02:45.840Z' NotOnOrAfter='2013-04-12T08:02:45.840Z'><AudienceRestriction><Audience>https://localhost:44300/</Audience></AudienceRestriction></Conditions><AttributeStatement><Attribute Name='http://schemas.microsoft.com/identity/claims/objectidentifier'><AttributeValue>580e531c-87a4-415a-9f9d-6af51e2e1948</AttributeValue></Attribute><Attribute Name='http://schemas.microsoft.com/identity/claims/tenantid'><AttributeValue>d062b2b0-9aca-4ff7-b32a-ba47231a4002</AttributeValue></Attribute><Attribute Name='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'><AttributeValue>Got</AttributeValue></Attribute><Attribute Name='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'><AttributeValue>GotJwt@GotJwt.onmicrosoft.com</AttributeValue></Attribute><Attribute Name='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname'><AttributeValue>Jwt</AttributeValue></Attribute><Attribute Name='http://schemas.microsoft.com/identity/claims/identityprovider'><AttributeValue>https://sts.windows.net/d062b2b0-9aca-4ff7-b32a-ba47231a4002/</AttributeValue></Attribute></AttributeStatement><AuthnStatement AuthnInstant='2013-04-11T20:02:45.000Z'><AuthnContext><AuthnContextClassRef>urn:oasis:names:tc:SAML:2.0:ac:classes:Password</AuthnContextClassRef></AuthnContext></AuthnStatement></Assertion>";
    }

    public static class JsonClaims
    {
        public static Dictionary<string, object> ClaimSourcesAsDictionary
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "src1",
                        new Dictionary<string,string>
                        {
                            { "endpoint", "https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"},
                            { "access_token", "ksj3n283dke"}
                        }
                    },
                    {
                        "src2",
                        new Dictionary<string,string>
                        {
                            { "endpoint2", "https://graph.windows.net/5803816d-c4ab-4601-a128-e2576e5d6910/users/0c9545d0-a670-4628-8c1f-e90618a3b940/getMemberObjects"},
                            { "access_token2", "ksj3n283dke"}
                        }
                    }

                };
            }
        }

        public static Dictionary<string, object> ClaimNamesAsDictionary
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "groups",
                        "src1"
                    },
                    {
                        "groups2",
                        "src2"
                    }
                };
            }
        }

        public static ClaimsIdentity ClaimsIdentityDistributedClaims(string issuer, string authType, Dictionary<string, object> claimSources, Dictionary<string, object> claimNames)
        {
            var identity = new ClaimsIdentity(authType);
            var claimValue = BuildClaimValue(claimSources);
            identity.AddClaim(new Claim("_claim_sources", claimValue, JsonClaimValueTypes.Json, issuer, issuer, identity));
            claimValue = BuildClaimValue(claimNames);
            identity.AddClaim(new Claim("_claim_names", claimValue, JsonClaimValueTypes.Json, issuer, issuer, identity));
            identity.AddClaim(new Claim("iss", issuer, ClaimValueTypes.String, issuer));
            return identity;
        }

        private static string BuildClaimValue(Dictionary<string, object> claimSources)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            bool first = true;
            foreach (var kv in claimSources)
            {
                if (!first)
                    sb.Append(",");
                sb.Append(@"""" + kv.Key + @""":" + JsonConvert.SerializeObject(kv.Value));
                first = false;
            }

            sb.Append("}");

            return sb.ToString();
        }
    }

    // https://datatracker.ietf.org/doc/html/rfc7516#appendix-A.3
    // A.3 JWE Using AES Key Wrap and AES_128_CBC_HMAC_SHA_256
    public static class AESKeyWrap_AES_128_CBC_HMAC_SHA_256
    {
        public static string Alg
        {
            get { return SecurityAlgorithms.Aes128KW; }
        }

        public static string Enc
        {
            get { return SecurityAlgorithms.Aes128CbcHmacSha256; }
        }

        public static string ProtectedHeader
        {
            get { return "eyJhbGciOiJBMTI4S1ciLCJlbmMiOiJBMTI4Q0JDLUhTMjU2In0"; }
        }

        public static byte[] CEK
        {
            get
            {
                return new byte[] { 4, 211, 31, 197, 84, 157, 252, 254, 11, 100, 157, 250, 63, 170, 106,
                                    206, 107, 124, 212, 45, 111, 107, 9, 219, 200, 177, 0, 240, 143, 156,
                                    44, 207 };
            }
        }

        public static byte[] IV
        {
            get
            {
                return new byte[] { 3, 22, 60, 12, 43, 67, 104, 105, 108, 108, 105, 99, 111, 116, 104, 101 };
            }
        }

        public static byte[] PlainText
        {
            get
            {
                return new byte[] { 76, 105, 118, 101, 32, 108, 111, 110, 103, 32, 97, 110, 100, 32,
                                    112, 114, 111, 115, 112, 101, 114, 46 };
            }
        }

        public static byte[] CipherText
        {
            get
            {
                return new byte[] { 40, 57, 83, 181, 119, 33, 133, 148, 198, 185, 243, 24, 152, 230, 6,
                                    75, 129, 223, 127, 19, 210, 82, 183, 230, 168, 33, 215, 104, 143,
                                    112, 56, 102 };
            }
        }

        public static byte[] AuthenticationTag
        {
            get
            {
                return new byte[] { 83, 73, 191, 98, 104, 205, 211, 128, 201, 189, 199, 133, 32, 38, 194, 85 };
            }
        }
    }
}
