////////////////////////////////////////////////////////////////////////////////////////////
// Copyright 2015 by SafeNet, Inc., (collectively herein  "SafeNet"), Belcamp, Maryland
// All Rights Reserved
// The SafeNet software that accompanies this License (the "Software") is the property of
// SafeNet, or its licensors and is protected by various copyright laws and international
// treaties.
// While SafeNet continues to own the Software, you will have certain non-exclusive,
// non-transferable rights to use the Software, subject to your full compliance with the
// terms and conditions of this License.
// All rights not expressly granted by this License are reserved to SafeNet or
// its licensors.
// SafeNet grants no express or implied right under SafeNet or its licensors� patents,
// copyrights, trademarks or other SafeNet or its licensors� intellectual property rights.
// Any supplemental software code, documentation or supporting materials provided to you
// as part of support services provided by SafeNet for the Software (if any) shall be
// considered part of the Software and subject to the terms and conditions of this License.
// The copyright and all other rights to the Software shall remain with SafeNet or 
// its licensors.
// For the purposes of this Agreement SafeNet, Inc. includes SafeNet, Inc and all of
// its subsidiaries.
//
// Any use of this software is subject to the limitations of warranty and liability
// contained in the end user license.
// SafeNet disclaims all other liability in connection with the use of this software,
// including all claims for  direct, indirect, special  or consequential regardless
// of the type or nature of the cause of action.
////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace eToken
{
  public static partial class PKCS11
  {
    public const int CKR_OK = 0x00000000;
    public const int CKR_CANCEL = 0x00000001;
    public const int CKR_HOST_MEMORY = 0x00000002;
    public const int CKR_SLOT_ID_INVALID = 0x00000003;
    public const int CKR_GENERAL_ERROR = 0x00000005;
    public const int CKR_FUNCTION_FAILED = 0x00000006;
    public const int CKR_ARGUMENTS_BAD = 0x00000007;
    public const int CKR_NO_EVENT = 0x00000008;
    public const int CKR_NEED_TO_CREATE_THREADS = 0x00000009;
    public const int CKR_CANT_LOCK = 0x0000000A;
    public const int CKR_ATTRIBUTE_READ_ONLY = 0x00000010;
    public const int CKR_ATTRIBUTE_SENSITIVE = 0x00000011;
    public const int CKR_ATTRIBUTE_TYPE_INVALID = 0x00000012;
    public const int CKR_ATTRIBUTE_VALUE_INVALID = 0x00000013;
    public const int CKR_DATA_INVALID = 0x00000020;
    public const int CKR_DATA_LEN_RANGE = 0x00000021;
    public const int CKR_DEVICE_ERROR = 0x00000030;
    public const int CKR_DEVICE_MEMORY = 0x00000031;
    public const int CKR_DEVICE_REMOVED = 0x00000032;
    public const int CKR_ENCRYPTED_DATA_INVALID = 0x00000040;
    public const int CKR_ENCRYPTED_DATA_LEN_RANGE = 0x00000041;
    public const int CKR_FUNCTION_CANCELED = 0x00000050;
    public const int CKR_FUNCTION_NOT_PARALLEL = 0x00000051;
    public const int CKR_FUNCTION_NOT_SUPPORTED = 0x00000054;
    public const int CKR_KEY_HANDLE_INVALID = 0x00000060;
    public const int CKR_KEY_SIZE_RANGE = 0x00000062;
    public const int CKR_KEY_TYPE_INCONSISTENT = 0x00000063;
    public const int CKR_KEY_NOT_NEEDED = 0x00000064;
    public const int CKR_KEY_CHANGED = 0x00000065;
    public const int CKR_KEY_NEEDED = 0x00000066;
    public const int CKR_KEY_INDIGESTIBLE = 0x00000067;
    public const int CKR_KEY_FUNCTION_NOT_PERMITTED = 0x00000068;
    public const int CKR_KEY_NOT_WRAPPABLE = 0x00000069;
    public const int CKR_KEY_UNEXTRACTABLE = 0x0000006A;
    public const int CKR_MECHANISM_INVALID = 0x00000070;
    public const int CKR_MECHANISM_PARAM_INVALID = 0x00000071;
    public const int CKR_OBJECT_HANDLE_INVALID = 0x00000082;
    public const int CKR_OPERATION_ACTIVE = 0x00000090;
    public const int CKR_OPERATION_NOT_INITIALIZED = 0x00000091;
    public const int CKR_PIN_INCORRECT = 0x000000A0;
    public const int CKR_PIN_INVALID = 0x000000A1;
    public const int CKR_PIN_LEN_RANGE = 0x000000A2;
    public const int CKR_PIN_EXPIRED = 0x000000A3;
    public const int CKR_PIN_LOCKED = 0x000000A4;
    public const int CKR_SESSION_CLOSED = 0x000000B0;
    public const int CKR_SESSION_COUNT = 0x000000B1;
    public const int CKR_SESSION_HANDLE_INVALID = 0x000000B3;
    public const int CKR_SESSION_PARALLEL_NOT_SUPPORTED = 0x000000B4;
    public const int CKR_SESSION_READ_ONLY = 0x000000B5;
    public const int CKR_SESSION_EXISTS = 0x000000B6;
    public const int CKR_SESSION_READ_ONLY_EXISTS = 0x000000B7;
    public const int CKR_SESSION_READ_WRITE_SO_EXISTS = 0x000000B8;
    public const int CKR_SIGNATURE_INVALID = 0x000000C0;
    public const int CKR_SIGNATURE_LEN_RANGE = 0x000000C1;
    public const int CKR_TEMPLATE_INCOMPLETE = 0x000000D0;
    public const int CKR_TEMPLATE_INCONSISTENT = 0x000000D1;
    public const int CKR_TOKEN_NOT_PRESENT = 0x000000E0;
    public const int CKR_TOKEN_NOT_RECOGNIZED = 0x000000E1;
    public const int CKR_TOKEN_WRITE_PROTECTED = 0x000000E2;
    public const int CKR_UNWRAPPING_KEY_HANDLE_INVALID = 0x000000F0;
    public const int CKR_UNWRAPPING_KEY_SIZE_RANGE = 0x000000F1;
    public const int CKR_UNWRAPPING_KEY_TYPE_INCONSISTENT = 0x000000F2;
    public const int CKR_USER_ALREADY_LOGGED_IN = 0x00000100;
    public const int CKR_USER_NOT_LOGGED_IN = 0x00000101;
    public const int CKR_USER_PIN_NOT_INITIALIZED = 0x00000102;
    public const int CKR_USER_TYPE_INVALID = 0x00000103;
    public const int CKR_USER_ANOTHER_ALREADY_LOGGED_IN = 0x00000104;
    public const int CKR_USER_TOO_MANY_TYPES = 0x00000105;
    public const int CKR_WRAPPED_KEY_INVALID = 0x00000110;
    public const int CKR_WRAPPED_KEY_LEN_RANGE = 0x00000112;
    public const int CKR_WRAPPING_KEY_HANDLE_INVALID = 0x00000113;
    public const int CKR_WRAPPING_KEY_SIZE_RANGE = 0x00000114;
    public const int CKR_WRAPPING_KEY_TYPE_INCONSISTENT = 0x00000115;
    public const int CKR_RANDOM_SEED_NOT_SUPPORTED = 0x00000120;
    public const int CKR_RANDOM_NO_RNG = 0x00000121;
    public const int CKR_DOMAIN_PARAMS_INVALID = 0x00000130;
    public const int CKR_BUFFER_TOO_SMALL = 0x00000150;
    public const int CKR_SAVED_STATE_INVALID = 0x00000160;
    public const int CKR_INFORMATION_SENSITIVE = 0x00000170;
    public const int CKR_STATE_UNSAVEABLE = 0x00000180;
    public const int CKR_CRYPTOKI_NOT_INITIALIZED = 0x00000190;
    public const int CKR_CRYPTOKI_ALREADY_INITIALIZED = 0x00000191;
    public const int CKR_MUTEX_BAD = 0x000001A0;
    public const int CKR_MUTEX_NOT_LOCKED = 0x000001A1;

    public const int CKA_CLASS = 0x00000000;
    public const int CKA_TOKEN = 0x00000001;
    public const int CKA_PRIVATE = 0x00000002;
    public const int CKA_LABEL = 0x00000003;
    public const int CKA_APPLICATION = 0x00000010;
    public const int CKA_VALUE = 0x00000011;
    public const int CKA_CERTIFICATE_TYPE = 0x00000080;
    public const int CKA_ISSUER = 0x00000081;
    public const int CKA_SERIAL_NUMBER = 0x00000082;
    public const int CKA_OWNER = 0x00000084;
    public const int CKA_CERTIFICATE_CATEGORY = 0x00000087;
    public const int CKA_KEY_TYPE = 0x00000100;
    public const int CKA_SUBJECT = 0x00000101;
    public const int CKA_ID = 0x00000102;
    public const int CKA_SENSITIVE = 0x00000103;
    public const int CKA_ENCRYPT = 0x00000104;
    public const int CKA_DECRYPT = 0x00000105;
    public const int CKA_WRAP = 0x00000106;
    public const int CKA_UNWRAP = 0x00000107;
    public const int CKA_SIGN = 0x00000108;
    public const int CKA_SIGN_RECOVER = 0x00000109;
    public const int CKA_VERIFY = 0x0000010A;
    public const int CKA_VERIFY_RECOVER = 0x0000010B;
    public const int CKA_DERIVE = 0x0000010C;
    public const int CKA_START_DATE = 0x00000110;
    public const int CKA_END_DATE = 0x00000111;
    public const int CKA_MODULUS = 0x00000120;
    public const int CKA_MODULUS_BITS = 0x00000121;
    public const int CKA_PUBLIC_EXPONENT = 0x00000122;
    public const int CKA_PRIVATE_EXPONENT = 0x00000123;
    public const int CKA_PRIME_1 = 0x00000124;
    public const int CKA_PRIME_2 = 0x00000125;
    public const int CKA_EXPONENT_1 = 0x00000126;
    public const int CKA_EXPONENT_2 = 0x00000127;
    public const int CKA_COEFFICIENT = 0x00000128;
    public const int CKA_VALUE_LEN = 0x00000161;
    public const int CKA_EXTRACTABLE = 0x00000162;
    public const int CKA_LOCAL = 0x00000163;
    public const int CKA_NEVER_EXTRACTABLE = 0x00000164;
    public const int CKA_ALWAYS_SENSITIVE = 0x00000165;
    public const int CKA_MODIFIABLE = 0x00000170;
    public const int CKA_ALWAYS_AUTHENTICATE = 0x00000202;
    public const int CKA_HW_FEATURE_TYPE = 0x00000300;

    public const int CKF_HW = 0x00000001;
    public const int CKF_ENCRYPT = 0x00000100;
    public const int CKF_DECRYPT = 0x00000200;
    public const int CKF_DIGEST = 0x00000400;
    public const int CKF_SIGN = 0x00000800;
    public const int CKF_SIGN_RECOVER = 0x00001000;
    public const int CKF_VERIFY = 0x00002000;
    public const int CKF_VERIFY_RECOVER = 0x00004000;
    public const int CKF_GENERATE = 0x00008000;
    public const int CKF_GENERATE_KEY_PAIR = 0x00010000;
    public const int CKF_WRAP = 0x00020000;
    public const int CKF_UNWRAP = 0x00040000;
    public const int CKF_DERIVE = 0x00080000;

    public const int CKF_TOKEN_PRESENT = 0x00000001;
    public const int CKF_REMOVABLE_DEVICE = 0x00000002;
    public const int CKF_HW_SLOT = 0x00000004;

    public const int CKF_RNG = 0x00000001;
    public const int CKF_WRITE_PROTECTED = 0x00000002;
    public const int CKF_LOGIN_REQUIRED = 0x00000004;
    public const int CKF_USER_PIN_INITIALIZED = 0x00000008;
    public const int CKF_RESTORE_KEY_NOT_NEEDED = 0x00000020;
    public const int CKF_CLOCK_ON_TOKEN = 0x00000040;
    public const int CKF_PROTECTED_AUTHENTICATION_PATH = 0x00000100;
    public const int CKF_DUAL_CRYPTO_OPERATIONS = 0x00000200;
    public const int CKF_TOKEN_INITIALIZED = 0x00000400;
    public const int CKF_SECONDARY_AUTHENTICATION = 0x00000800;
    public const int CKF_USER_PIN_COUNT_LOW = 0x00010000;
    public const int CKF_USER_PIN_FINAL_TRY = 0x00020000;
    public const int CKF_USER_PIN_LOCKED = 0x00040000;
    public const int CKF_USER_PIN_TO_BE_CHANGED = 0x00080000;
    public const int CKF_SO_PIN_COUNT_LOW = 0x00100000;
    public const int CKF_SO_PIN_FINAL_TRY = 0x00200000;
    public const int CKF_SO_PIN_LOCKED = 0x00400000;
    public const int CKF_SO_PIN_TO_BE_CHANGED = 0x00800000;

    public const int CKO_DATA = 0x00000000;
    public const int CKO_CERTIFICATE = 0x00000001;
    public const int CKO_PUBLIC_KEY = 0x00000002;
    public const int CKO_PRIVATE_KEY = 0x00000003;
    public const int CKO_SECRET_KEY = 0x00000004;
    public const int CKO_HW_FEATURE = 0x00000005;

    public const int CKF_RW_SESSION = 0x00000002;
    public const int CKF_SERIAL_SESSION = 0x00000004;

    public const int CKU_SO = 0;
    public const int CKU_USER = 1;
    public const int CKU_CONTEXT_SPECIFIC = 2;

    public const int CKC_X_509 = 0x00000000;

    public const int  CKK_RSA              = 0x00000000;
    public const int  CKK_DSA              = 0x00000001;
    public const int  CKK_DH               = 0x00000002;
    public const int  CKK_ECDSA            = 0x00000003;
    public const int  CKK_EC               = 0x00000003;
    public const int  CKK_X9_42_DH         = 0x00000004;
    public const int  CKK_KEA              = 0x00000005;
    public const int  CKK_GENERIC_SECRET   = 0x00000010;
    public const int  CKK_RC2              = 0x00000011;
    public const int  CKK_RC4              = 0x00000012;
    public const int  CKK_DES              = 0x00000013;
    public const int  CKK_DES2             = 0x00000014;
    public const int  CKK_DES3             = 0x00000015;
    public const int  CKK_CAST             = 0x00000016;
    public const int  CKK_CAST3            = 0x00000017;
    public const int  CKK_CAST5            = 0x00000018;
    public const int  CKK_CAST128          = 0x00000018;
    public const int  CKK_RC5              = 0x00000019;
    public const int  CKK_IDEA             = 0x0000001A;
    public const int  CKK_SKIPJACK         = 0x0000001B;
    public const int  CKK_BATON            = 0x0000001C;
    public const int  CKK_JUNIPER          = 0x0000001D;
    public const int  CKK_CDMF             = 0x0000001E;
    public const int  CKK_AES              = 0x0000001F;


    public const int  CKM_RSA_PKCS_KEY_PAIR_GEN      = 0x00000000;
    public const int  CKM_RSA_PKCS                   = 0x00000001;
    public const int  CKM_RSA_9796                   = 0x00000002;
    public const int  CKM_RSA_X_509                  = 0x00000003;
    public const int  CKM_MD2_RSA_PKCS               = 0x00000004;
    public const int  CKM_MD5_RSA_PKCS               = 0x00000005;
    public const int  CKM_SHA1_RSA_PKCS              = 0x00000006;
    public const int  CKM_RIPEMD128_RSA_PKCS         = 0x00000007;
    public const int  CKM_RIPEMD160_RSA_PKCS         = 0x00000008;
    public const int  CKM_RSA_PKCS_OAEP              = 0x00000009;
    public const int  CKM_RSA_X9_31_KEY_PAIR_GEN     = 0x0000000A;
    public const int  CKM_RSA_X9_31                  = 0x0000000B;
    public const int  CKM_SHA1_RSA_X9_31             = 0x0000000C;
    public const int  CKM_RSA_PKCS_PSS               = 0x0000000D;
    public const int  CKM_SHA1_RSA_PKCS_PSS          = 0x0000000E;
    public const int  CKM_DSA_KEY_PAIR_GEN           = 0x00000010;
    public const int  CKM_DSA                        = 0x00000011;
    public const int  CKM_DSA_SHA1                   = 0x00000012;
    public const int  CKM_DH_PKCS_KEY_PAIR_GEN       = 0x00000020;
    public const int  CKM_DH_PKCS_DERIVE             = 0x00000021;
    public const int  CKM_X9_42_DH_KEY_PAIR_GEN      = 0x00000030;
    public const int  CKM_X9_42_DH_DERIVE            = 0x00000031;
    public const int  CKM_X9_42_DH_HYBRID_DERIVE     = 0x00000032;
    public const int  CKM_X9_42_MQV_DERIVE           = 0x00000033;
    public const int  CKM_SHA256_RSA_PKCS            = 0x00000040;
    public const int  CKM_SHA384_RSA_PKCS            = 0x00000041;
    public const int  CKM_SHA512_RSA_PKCS            = 0x00000042;
    public const int  CKM_SHA256_RSA_PKCS_PSS        = 0x00000043;
    public const int  CKM_SHA384_RSA_PKCS_PSS        = 0x00000044;
    public const int  CKM_SHA512_RSA_PKCS_PSS        = 0x00000045;
    public const int  CKM_RC2_KEY_GEN                = 0x00000100;
    public const int  CKM_RC2_ECB                    = 0x00000101;
    public const int  CKM_RC2_CBC                    = 0x00000102;
    public const int  CKM_RC2_MAC                    = 0x00000103;
    public const int  CKM_RC2_MAC_GENERAL            = 0x00000104;
    public const int  CKM_RC2_CBC_PAD                = 0x00000105;
    public const int  CKM_RC4_KEY_GEN                = 0x00000110;
    public const int  CKM_RC4                        = 0x00000111;
    public const int  CKM_DES_KEY_GEN                = 0x00000120;
    public const int  CKM_DES_ECB                    = 0x00000121;
    public const int  CKM_DES_CBC                    = 0x00000122;
    public const int  CKM_DES_MAC                    = 0x00000123;
    public const int  CKM_DES_MAC_GENERAL            = 0x00000124;
    public const int  CKM_DES_CBC_PAD                = 0x00000125;
    public const int  CKM_DES2_KEY_GEN               = 0x00000130;
    public const int  CKM_DES3_KEY_GEN               = 0x00000131;
    public const int  CKM_DES3_ECB                   = 0x00000132;
    public const int  CKM_DES3_CBC                   = 0x00000133;
    public const int  CKM_DES3_MAC                   = 0x00000134;
    public const int  CKM_DES3_MAC_GENERAL           = 0x00000135;
    public const int  CKM_DES3_CBC_PAD               = 0x00000136;
    public const int  CKM_CDMF_KEY_GEN               = 0x00000140;
    public const int  CKM_CDMF_ECB                   = 0x00000141;
    public const int  CKM_CDMF_CBC                   = 0x00000142;
    public const int  CKM_CDMF_MAC                   = 0x00000143;
    public const int  CKM_CDMF_MAC_GENERAL           = 0x00000144;
    public const int  CKM_CDMF_CBC_PAD               = 0x00000145;
    public const int  CKM_DES_OFB64                  = 0x00000150;
    public const int  CKM_DES_OFB8                   = 0x00000151;
    public const int  CKM_DES_CFB64                  = 0x00000152;
    public const int  CKM_DES_CFB8                   = 0x00000153;
    public const int  CKM_MD2                        = 0x00000200;
    public const int  CKM_MD2_HMAC                   = 0x00000201;
    public const int  CKM_MD2_HMAC_GENERAL           = 0x00000202;
    public const int  CKM_MD5                        = 0x00000210;
    public const int  CKM_MD5_HMAC                   = 0x00000211;
    public const int  CKM_MD5_HMAC_GENERAL           = 0x00000212;
    public const int  CKM_SHA_1                      = 0x00000220;
    public const int  CKM_SHA_1_HMAC                 = 0x00000221;
    public const int  CKM_SHA_1_HMAC_GENERAL         = 0x00000222;
    public const int  CKM_RIPEMD128                  = 0x00000230;
    public const int  CKM_RIPEMD128_HMAC             = 0x00000231;
    public const int  CKM_RIPEMD128_HMAC_GENERAL     = 0x00000232;
    public const int  CKM_RIPEMD160                  = 0x00000240;
    public const int  CKM_RIPEMD160_HMAC             = 0x00000241;
    public const int  CKM_RIPEMD160_HMAC_GENERAL     = 0x00000242;
    public const int  CKM_SHA256                     = 0x00000250;
    public const int  CKM_SHA256_HMAC                = 0x00000251;
    public const int  CKM_SHA256_HMAC_GENERAL        = 0x00000252;
    public const int  CKM_SHA384                     = 0x00000260;
    public const int  CKM_SHA384_HMAC                = 0x00000261;
    public const int  CKM_SHA384_HMAC_GENERAL        = 0x00000262;
    public const int  CKM_SHA512                     = 0x00000270;
    public const int  CKM_SHA512_HMAC                = 0x00000271;
    public const int  CKM_SHA512_HMAC_GENERAL        = 0x00000272;
    public const int  CKM_CAST_KEY_GEN               = 0x00000300;
    public const int  CKM_CAST_ECB                   = 0x00000301;
    public const int  CKM_CAST_CBC                   = 0x00000302;
    public const int  CKM_CAST_MAC                   = 0x00000303;
    public const int  CKM_CAST_MAC_GENERAL           = 0x00000304;
    public const int  CKM_CAST_CBC_PAD               = 0x00000305;
    public const int  CKM_CAST3_KEY_GEN              = 0x00000310;
    public const int  CKM_CAST3_ECB                  = 0x00000311;
    public const int  CKM_CAST3_CBC                  = 0x00000312;
    public const int  CKM_CAST3_MAC                  = 0x00000313;
    public const int  CKM_CAST3_MAC_GENERAL          = 0x00000314;
    public const int  CKM_CAST3_CBC_PAD              = 0x00000315;
    public const int  CKM_CAST5_KEY_GEN              = 0x00000320;
    public const int  CKM_CAST128_KEY_GEN            = 0x00000320;
    public const int  CKM_CAST5_ECB                  = 0x00000321;
    public const int  CKM_CAST128_ECB                = 0x00000321;
    public const int  CKM_CAST5_CBC                  = 0x00000322;
    public const int  CKM_CAST128_CBC                = 0x00000322;
    public const int  CKM_CAST5_MAC                  = 0x00000323;
    public const int  CKM_CAST128_MAC                = 0x00000323;
    public const int  CKM_CAST5_MAC_GENERAL          = 0x00000324;
    public const int  CKM_CAST128_MAC_GENERAL        = 0x00000324;
    public const int  CKM_CAST5_CBC_PAD              = 0x00000325;
    public const int  CKM_CAST128_CBC_PAD            = 0x00000325;
    public const int  CKM_RC5_KEY_GEN                = 0x00000330;
    public const int  CKM_RC5_ECB                    = 0x00000331;
    public const int  CKM_RC5_CBC                    = 0x00000332;
    public const int  CKM_RC5_MAC                    = 0x00000333;
    public const int  CKM_RC5_MAC_GENERAL            = 0x00000334;
    public const int  CKM_RC5_CBC_PAD                = 0x00000335;
    public const int  CKM_IDEA_KEY_GEN               = 0x00000340;
    public const int  CKM_IDEA_ECB                   = 0x00000341;
    public const int  CKM_IDEA_CBC                   = 0x00000342;
    public const int  CKM_IDEA_MAC                   = 0x00000343;
    public const int  CKM_IDEA_MAC_GENERAL           = 0x00000344;
    public const int  CKM_IDEA_CBC_PAD               = 0x00000345;
    public const int  CKM_GENERIC_SECRET_KEY_GEN     = 0x00000350;
    public const int  CKM_CONCATENATE_BASE_AND_KEY   = 0x00000360;
    public const int  CKM_CONCATENATE_BASE_AND_DATA  = 0x00000362;
    public const int  CKM_CONCATENATE_DATA_AND_BASE  = 0x00000363;
    public const int  CKM_XOR_BASE_AND_DATA          = 0x00000364;
    public const int  CKM_EXTRACT_KEY_FROM_KEY       = 0x00000365;
    public const int  CKM_SSL3_PRE_MASTER_KEY_GEN    = 0x00000370;
    public const int  CKM_SSL3_MASTER_KEY_DERIVE     = 0x00000371;
    public const int  CKM_SSL3_KEY_AND_MAC_DERIVE    = 0x00000372;
    public const int  CKM_SSL3_MASTER_KEY_DERIVE_DH  = 0x00000373;
    public const int  CKM_TLS_PRE_MASTER_KEY_GEN     = 0x00000374;
    public const int  CKM_TLS_MASTER_KEY_DERIVE      = 0x00000375;
    public const int  CKM_TLS_KEY_AND_MAC_DERIVE     = 0x00000376;
    public const int  CKM_TLS_MASTER_KEY_DERIVE_DH   = 0x00000377;
    public const int  CKM_TLS_PRF                    = 0x00000378;
    public const int  CKM_SSL3_MD5_MAC               = 0x00000380;
    public const int  CKM_SSL3_SHA1_MAC              = 0x00000381;
    public const int  CKM_MD5_KEY_DERIVATION         = 0x00000390;
    public const int  CKM_MD2_KEY_DERIVATION         = 0x00000391;
    public const int  CKM_SHA1_KEY_DERIVATION        = 0x00000392;
    public const int  CKM_SHA256_KEY_DERIVATION      = 0x00000393;
    public const int  CKM_SHA384_KEY_DERIVATION      = 0x00000394;
    public const int  CKM_SHA512_KEY_DERIVATION      = 0x00000395;
    public const int  CKM_PBE_MD2_DES_CBC            = 0x000003A0;
    public const int  CKM_PBE_MD5_DES_CBC            = 0x000003A1;
    public const int  CKM_PBE_MD5_CAST_CBC           = 0x000003A2;
    public const int  CKM_PBE_MD5_CAST3_CBC          = 0x000003A3;
    public const int  CKM_PBE_MD5_CAST5_CBC          = 0x000003A4;
    public const int  CKM_PBE_MD5_CAST128_CBC        = 0x000003A4;
    public const int  CKM_PBE_SHA1_CAST5_CBC         = 0x000003A5;
    public const int  CKM_PBE_SHA1_CAST128_CBC       = 0x000003A5;
    public const int  CKM_PBE_SHA1_RC4_128           = 0x000003A6;
    public const int  CKM_PBE_SHA1_RC4_40            = 0x000003A7;
    public const int  CKM_PBE_SHA1_DES3_EDE_CBC      = 0x000003A8;
    public const int  CKM_PBE_SHA1_DES2_EDE_CBC      = 0x000003A9;
    public const int  CKM_PBE_SHA1_RC2_128_CBC       = 0x000003AA;
    public const int  CKM_PBE_SHA1_RC2_40_CBC        = 0x000003AB;
    public const int  CKM_PKCS5_PBKD2                = 0x000003B0;
    public const int  CKM_PBA_SHA1_WITH_SHA1_HMAC    = 0x000003C0;
    public const int  CKM_WTLS_PRE_MASTER_KEY_GEN         = 0x000003D0;
    public const int  CKM_WTLS_MASTER_KEY_DERIVE          = 0x000003D1;
    public const int  CKM_WTLS_MASTER_KEY_DERIVE_DH_ECC   = 0x000003D2;
    public const int  CKM_WTLS_PRF                        = 0x000003D3;
    public const int  CKM_WTLS_SERVER_KEY_AND_MAC_DERIVE  = 0x000003D4;
    public const int  CKM_WTLS_CLIENT_KEY_AND_MAC_DERIVE  = 0x000003D5;
    public const int  CKM_KEY_WRAP_LYNKS             = 0x00000400;
    public const int  CKM_KEY_WRAP_SET_OAEP          = 0x00000401;
    public const int  CKM_CMS_SIG                    = 0x00000500;
    public const int  CKM_SKIPJACK_KEY_GEN           = 0x00001000;
    public const int  CKM_SKIPJACK_ECB64             = 0x00001001;
    public const int  CKM_SKIPJACK_CBC64             = 0x00001002;
    public const int  CKM_SKIPJACK_OFB64             = 0x00001003;
    public const int  CKM_SKIPJACK_CFB64             = 0x00001004;
    public const int  CKM_SKIPJACK_CFB32             = 0x00001005;
    public const int  CKM_SKIPJACK_CFB16             = 0x00001006;
    public const int  CKM_SKIPJACK_CFB8              = 0x00001007;
    public const int  CKM_SKIPJACK_WRAP              = 0x00001008;
    public const int  CKM_SKIPJACK_PRIVATE_WRAP      = 0x00001009;
    public const int  CKM_SKIPJACK_RELAYX            = 0x0000100a;
    public const int  CKM_KEA_KEY_PAIR_GEN           = 0x00001010;
    public const int  CKM_KEA_KEY_DERIVE             = 0x00001011;
    public const int  CKM_FORTEZZA_TIMESTAMP         = 0x00001020;
    public const int  CKM_BATON_KEY_GEN              = 0x00001030;
    public const int  CKM_BATON_ECB128               = 0x00001031;
    public const int  CKM_BATON_ECB96                = 0x00001032;
    public const int  CKM_BATON_CBC128               = 0x00001033;
    public const int  CKM_BATON_COUNTER              = 0x00001034;
    public const int  CKM_BATON_SHUFFLE              = 0x00001035;
    public const int  CKM_BATON_WRAP                 = 0x00001036;
    public const int  CKM_ECDSA_KEY_PAIR_GEN         = 0x00001040;
    public const int  CKM_EC_KEY_PAIR_GEN            = 0x00001040;
    public const int  CKM_ECDSA                      = 0x00001041;
    public const int  CKM_ECDSA_SHA1                 = 0x00001042;
    public const int  CKM_ECDH1_DERIVE               = 0x00001050;
    public const int  CKM_ECDH1_COFACTOR_DERIVE      = 0x00001051;
    public const int  CKM_ECMQV_DERIVE               = 0x00001052;
    public const int  CKM_JUNIPER_KEY_GEN            = 0x00001060;
    public const int  CKM_JUNIPER_ECB128             = 0x00001061;
    public const int  CKM_JUNIPER_CBC128             = 0x00001062;
    public const int  CKM_JUNIPER_COUNTER            = 0x00001063;
    public const int  CKM_JUNIPER_SHUFFLE            = 0x00001064;
    public const int  CKM_JUNIPER_WRAP               = 0x00001065;
    public const int  CKM_FASTHASH                   = 0x00001070;
    public const int  CKM_AES_KEY_GEN                = 0x00001080;
    public const int  CKM_AES_ECB                    = 0x00001081;
    public const int  CKM_AES_CBC                    = 0x00001082;
    public const int  CKM_AES_MAC                    = 0x00001083;
    public const int  CKM_AES_MAC_GENERAL            = 0x00001084;
    public const int  CKM_AES_CBC_PAD                = 0x00001085;
    public const int  CKM_BLOWFISH_KEY_GEN           = 0x00001090;
    public const int  CKM_BLOWFISH_CBC               = 0x00001091;
    public const int  CKM_TWOFISH_KEY_GEN            = 0x00001092;
    public const int  CKM_TWOFISH_CBC                = 0x00001093;
    public const int  CKM_DES_ECB_ENCRYPT_DATA       = 0x00001100;
    public const int  CKM_DES_CBC_ENCRYPT_DATA       = 0x00001101;
    public const int  CKM_DES3_ECB_ENCRYPT_DATA      = 0x00001102;
    public const int  CKM_DES3_CBC_ENCRYPT_DATA      = 0x00001103;
    public const int  CKM_AES_ECB_ENCRYPT_DATA       = 0x00001104;
    public const int  CKM_AES_CBC_ENCRYPT_DATA       = 0x00001105;
    public const int  CKM_DSA_PARAMETER_GEN          = 0x00002000;
    public const int  CKM_DH_PKCS_PARAMETER_GEN      = 0x00002001;
    public const int  CKM_X9_42_DH_PARAMETER_GEN     = 0x00002002;

//Extentions
    public const int  ETCKF_PROPERTY_THREAD          = 0x00000001;

    public const int ETCKO_SHADOW_PRIVATE_KEY           = unchecked((int)0x80005001);
    public const int ETCKH_TOKEN_OBJECT                 = unchecked((int)0x80005002);
    public const int ETCKH_PIN_POLICY                   = unchecked((int)0x80005003);
    public const int ETCKH_SO_UNLOCK                    = unchecked((int)0x80005004);
    public const int ETCKH_PRIVATE_CACHING              = unchecked((int)0x80005005);
    public const int ETCKH_2NDAUTH                      = unchecked((int)0x80005006);
    public const int ETCKH_BATTERY                      = unchecked((int)0x80005007);
    public const int ETCKH_CAPI                         = unchecked((int)0x80005008);

    public const int ETCKM_PBA_LEGACY                   = unchecked((int)0x80006001);

    public const int ETCKA_CAPI_KEY_CONTAINER           = unchecked((int)0x80001301);
    public const int ETCKA_CAPI_KEYSIGNATURE            = unchecked((int)0x80001302);

    public const int ETCKA_OWNER                        = unchecked((int)0x80001401);
    public const int ETCKA_2NDAUTH_PIN                  = unchecked((int)0x80001402);
    public const int ETCKA_DESTROYABLE                  = unchecked((int)0x80001403);

    public const int ETCKA_PBA_MECHANISM                = unchecked((int)0x80001501);
    public const int ETCKA_PBA_ITERATION                = unchecked((int)0x80001502);
    public const int ETCKA_PBA_SALT                     = unchecked((int)0x80001503);

    public const int ETCKA_CACHE_PRIVATE                = unchecked((int)0x80001601);

    public const int ETCK_CACHE_OFF                     = 0x00000000;
    public const int ETCK_CACHE_LOGIN                   = 0x00000001;
    public const int ETCK_CACHE_ON                      = 0x00000002;

    public const int ETCKA_2NDAUTH_CREATE               = unchecked((int)0x80001701);

    public const int ETCK_2NDAUTH_PROMPT_NEVER          = 0x00000000;
    public const int ETCK_2NDAUTH_PROMPT_CONDITIONAL    = 0x00000001;
    public const int ETCK_2NDAUTH_PROMPT_ALWAYS         = 0x00000002;
    public const int ETCK_2NDAUTH_MANDATORY             = 0x00000003;

    public const int ETCKA_OTP_DURATION                 = unchecked((int)0x80001801);
    public const int ETCKA_OTP_MAY_SET_DURATION         = unchecked((int)0x80001802);

    public const int ETCKA_CAPI_DEFAULT_KC              = unchecked((int)0x80001901);
    public const int ETCKA_CAPI_ENROLL_KC               = unchecked((int)0x80001902);
    public const int ETCKA_CAPI_AUX_KC                  = unchecked((int)0x80001903);

      /* Token object's attributes */
    public const int ETCKA_PRODUCT_NAME                 = unchecked((int)0x80001101);
    public const int ETCKA_MODEL                        = unchecked((int)0x80001102);
    public const int ETCKA_FW_REVISION                  = unchecked((int)0x80001104);
    public const int ETCKA_HW_INTERNAL                  = unchecked((int)0x80001106);
    public const int ETCKA_PRODUCTION_DATE              = unchecked((int)0x80001107);
    public const int ETCKA_CASE_MODEL                   = unchecked((int)0x80001108);
    public const int ETCKA_TOKEN_ID                     = unchecked((int)0x80001109);
    public const int ETCKA_CARD_ID                      = unchecked((int)0x8000110a);
    public const int ETCKA_CARD_TYPE                    = unchecked((int)0x8000110b);
    public const int ETCKA_CARD_VERSION                 = unchecked((int)0x8000110c);
    public const int ETCKA_COLOR                        = unchecked((int)0x8000110e);
    public const int ETCKA_RETRY_USER                   = unchecked((int)0x80001110);
    public const int ETCKA_RETRY_SO                     = unchecked((int)0x80001111);
    public const int ETCKA_RETRY_USER_MAX               = unchecked((int)0x80001112);
    public const int ETCKA_RETRY_SO_MAX                 = unchecked((int)0x80001113);
    public const int ETCKA_HAS_LCD                      = unchecked((int)0x8000111b);
    public const int ETCKA_HAS_SO                       = unchecked((int)0x8000111d);
    public const int ETCKA_FIPS                         = unchecked((int)0x8000111e);
    public const int ETCKA_FIPS_SUPPORTED               = unchecked((int)0x8000111f);
    public const int ETCKA_INIT_PIN_REQ                 = unchecked((int)0x80001120);
    public const int ETCKA_RSA_2048                     = unchecked((int)0x80001121);
    public const int ETCKA_RSA_2048_SUPPORTED           = unchecked((int)0x80001122);
    public const int ETCKA_HMAC_SHA1                    = unchecked((int)0x80001123);
    public const int ETCKA_HMAC_SHA1_SUPPORTED          = unchecked((int)0x80001124);
    public const int ETCKA_REAL_COLOR                   = unchecked((int)0x80001125);
    public const int ETCKA_MAY_INIT                     = unchecked((int)0x80001126);
    public const int ETCKA_MASS_STORAGE_PRESENT         = unchecked((int)0x80001127);
    public const int ETCKA_ONE_FACTOR                   = unchecked((int)0x80001128);
    public const int ETCKA_RSA_AREA_SIZE                = unchecked((int)0x80001129);
    public const int ETCKA_FORMAT_VERSION               = unchecked((int)0x8000112a);
    public const int ETCKA_USER_PIN_AGE                 = unchecked((int)0x8000112b);
    public const int ETCKA_CARDMODULE_AREA_SIZE         = unchecked((int)0x8000112c);
    public const int ETCKA_HASHVAL                      = unchecked((int)0x8000112d);
    public const int ETCKA_OS_NAME                      = unchecked((int)0x8000112e);
    public const int ETCKA_MINIDRIVER_COMPATIBLE        = unchecked((int)0x8000112f);
    public const int ETCKA_MASS_STORAGE_SECURED         = unchecked((int)0x80001130);
    public const int ETCKA_INIT_PKI_VERSION             = unchecked((int)0x80001131);
    public const int ETCKA_CRYPTO_LOCK_MODE             = unchecked((int)0x80001132);
    public const int ETCKA_CRYPTO_LOCK_STATE            = unchecked((int)0x80001133);
    public const int ETCKA_USER_PIN_ITER                = unchecked((int)0x80001134);


    /* Battery attributes */
    public const int ETCKA_BATTERY_VALUE                = unchecked((int)0x8000120a);
    public const int ETCKA_BATTERY_HW_WARN1             = unchecked((int)0x8000120b);
    public const int ETCKA_BATTERY_HW_WARN2             = unchecked((int)0x8000120c);
    public const int ETCKA_BATTERY_HW_WARN3             = unchecked((int)0x8000120d);
    public const int ETCKA_BATTERY_REPLACEABLE          = unchecked((int)0x8000120e);


    /* Password policy's attributes */
    public const int ETCKA_PIN_POLICY_TYPE              = unchecked((int)0x80001201);
    public const int ETCKA_PIN_MIN_LEN                  = unchecked((int)0x80001202);
    public const int ETCKA_PIN_MIX_CHARS                = unchecked((int)0x80001203);
    public const int ETCKA_PIN_MAX_AGE                  = unchecked((int)0x80001204);
    public const int ETCKA_PIN_MIN_AGE                  = unchecked((int)0x80001205);
    public const int ETCKA_PIN_WARN_PERIOD              = unchecked((int)0x80001206);
    public const int ETCKA_PIN_HISTORY_SIZE             = unchecked((int)0x80001207);
    public const int ETCKA_PIN_PROXY                    = unchecked((int)0x80001208);
    public const int ETCKA_PIN_MAX_REPEATED             = unchecked((int)0x80001209);
    public const int ETCKA_PIN_NUMBERS                  = unchecked((int)0x8000120a);
    public const int ETCKA_PIN_UPPER_CASE               = unchecked((int)0x8000120b);
    public const int ETCKA_PIN_LOWER_CASE               = unchecked((int)0x8000120c);
    public const int ETCKA_PIN_SPECIAL                  = unchecked((int)0x8000120d);


    /* Password policy's type */
    public const int ETCKPT_GENERAL_PIN_POLICY          = 0x00000001;

    /* Password policy's values */
    public const int ETCK_PIN_DONTCARE                  = 0x00000000;
    public const int ETCK_PIN_FORBIDDEN                 = 0x00000001;
    public const int ETCK_PIN_ENFORCE                   = 0x00000002;

    /* Password problems */
    public const int ETCKF_PIN_MIN_LEN                  = 0x00000001;
    public const int ETCKF_PIN_MIX_CHARS                = 0x00000002;
    public const int ETCKF_PIN_MAX_AGE                  = 0x00000004;
    public const int ETCKF_PIN_MIN_AGE                  = 0x00000008;
    public const int ETCKF_PIN_WARN_PERIOD              = 0x00000010;
    public const int ETCKF_PIN_HISTORY                  = 0x00000020;
    public const int ETCKF_PIN_MUST_BE_CHANGED          = 0x00000040;
    public const int ETCKF_PIN_MAX_REPEATED             = 0x00000100;
    public const int ETCKF_PIN_FORBIDDEN_NUMBERS        = 0x00000200;
    public const int ETCKF_PIN_FORBIDDEN_UPPER_CASE     = 0x00000400;
    public const int ETCKF_PIN_FORBIDDEN_LOWER_CASE     = 0x00000800;
    public const int ETCKF_PIN_FORBIDDEN_SPECIAL        = 0x00001000;
    public const int ETCKF_PIN_ENFORCE_NUMBERS          = 0x00002000;
    public const int ETCKF_PIN_ENFORCE_UPPER_CASE       = 0x00004000;
    public const int ETCKF_PIN_ENFORCE_LOWER_CASE       = 0x00008000;
    public const int ETCKF_PIN_ENFORCE_SPECIAL          = 0x00010000;

    /* Smartcard types */
    public const int ETCK_CARD_NONE                     = 0x00000000;
    public const int ETCK_CARD_OS                       = 0x00000001;
    public const int ETCK_CARD_JAVA_APPLET              = 0x00000002;

    /* Token cases  */
    public const int ETCK_CASE_NONE                     = 0x00000000;
    public const int ETCK_CASE_CLASSIC                  = 0x00000001;
    public const int ETCK_CASE_NG1                      = 0x00000002;
    public const int ETCK_CASE_NG2                      = 0x00000003;
    public const int ETCK_CASE_NG2_NOLCD                = 0x00000004;


    /* Crypto lock modes  */
    public const int ETCK_CRYPTO_LOCK_NONE              = 0x00000000;
    public const int ETCK_CRYPTO_LOCK_MACHINE           = 0x00000001;
    public const int ETCK_CRYPTO_LOCK_DEVICE            = 0x00000002;

    /* Crypto lock states  */
    public const int ETCK_CRYPTO_LOCK_ACTIVATED         = 0x00000001;
    public const int ETCK_CRYPTO_LOCK_DONE              = 0x00000002;

    public const int ETCK_FORMAT_VERSION_LEGACY         = 0;
    public const int ETCK_FORMAT_VERSION_4_0            = 4;
    public const int ETCK_FORMAT_VERSION_5_0            = 5;


    public const int ETCK_IODEV_SOFTWARE_TOKEN_PLUGIN  = 1 ;
    public const int ETCK_IODEV_SOFTWARE_TOKEN_PLUGOUT = 2 ;
    public const int ETCK_IODEV_FULL_NAME              = 3 ;

    public const int ETCK_IODEV_GET_EMULATE            = 4 ;
    public const int ETCK_IODEV_SET_EMULATE            = 5 ;
    public const int ETCK_IODEV_SOFTWARE_GET_EMULATE   = 4 ;
    public const int ETCK_IODEV_SOFTWARE_SET_EMULATE   = 5 ;

    public const int ETCK_IODEV_CHECK_NAME             = 6 ;
    public const int ETCK_IODEV_REMOTE_TOKEN_PLUGIN    = 7 ;
    public const int ETCK_IODEV_REMOTE_TOKEN_PLUGOUT   = 8 ;
    public const int ETCK_IODEV_GET_REMOTE_INFO        = 9 ;

    public const int ETCK_IODEV_SOFTWARE_TOKEN_PRE_PLUGIN  = 10 ;

    public const int ETCK_IOCTL_PIN_EVALUATE           = 1;
    public const int ETCK_IOCTL_CHECK_BROKEN_KEY       = 2;
    public const int ETCK_IOCTL_PIN_GENERATE           = 3;
    public const int ETCK_IOCTL_SET_PROGRESS_CALLBACK  = 4;

    // error information
    public const int ETCKR_EXTENSION                   = 0;
    public const int ETCKR_SYSTEM                      = 1;
    public const int ETCKR_APDU                        = 2;
    public const int ETCKR_PIN_POLICY                  = 3;


    public const int ETCKR_FIPS_CARDOS_OLD             = unchecked((int)0xff000001 );
    public const int ETCKR_FIPS_CARDOS_4               = unchecked((int)0xff000002 );
    public const int ETCKR_FORMAT_UNKNOWN              = unchecked((int)0xff000003 );
    public const int ETCKR_FIPS_ONE_FACTOR             = unchecked((int)0xff000004 );
    public const int ETCKR_ONE_FACTOR_VERSION          = unchecked((int)0xff000005 );
    public const int ETCKR_FORMAT_0_ADMIN_USER         = unchecked((int)0xff000006 );
    public const int ETCKR_HMAC_SHA1_SUPPORT           = unchecked((int)0xff000007 );
    public const int ETCKR_RSA_2048_SUPPORT            = unchecked((int)0xff000008 );
    public const int ETCKR_ONE_FACTOR_2ND_AUTH         = unchecked((int)0xff000009 );
    public const int ETCKR_HMAC_SHA1_RSA_2048          = unchecked((int)0xff00000a );
    public const int ETCKR_CRYPTO_LOCK_SUPPORT         = unchecked((int)0xff00000b );

    public const int ETCKR_CARDOS_FORMAT_5             = unchecked((int)0xff00000c );
    public const int ETCKR_FIPS_FORMAT_5               = unchecked((int)0xff00000d );
    public const int ETCKR_FIPS_RSA_2048               = unchecked((int)0xff00000e );
    public const int ETCKR_LCD_OTP                     = unchecked((int)0xff00000f );
    public const int ETCKR_FIPS_SUPPORT                = unchecked((int)0xff000010 );
    public const int ETCKR_PQ_AGE_WARN                 = unchecked((int)0xff000011 );
    public const int ETCKR_PQ_AGE_MIN_MAX              = unchecked((int)0xff000012 );
    public const int ETCKR_PQ_FORBIDDEN_ALL            = unchecked((int)0xff000013 );
    public const int ETCKR_PQ_FORBIDDEN_MIX            = unchecked((int)0xff000014 );
    public const int ETCKR_DOMAIN_DISCONNECTED         = unchecked((int)0xff000015 );
    public const int ETCKR_DOMAIN_CHANGE_PIN           = unchecked((int)0xff000016 );
    public const int ETCKR_ETV_LOCK2FLASH_DEVICE_REMOVABLE = unchecked((int)0xff000017 );
    public const int ETCKR_ETV_LOCKING                 = unchecked((int)0xff000018 );

  }
}