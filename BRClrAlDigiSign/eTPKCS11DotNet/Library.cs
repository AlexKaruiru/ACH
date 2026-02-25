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
        public class Exception : System.Exception
        {
            public int error;
            internal Exception(int error)
            {
                this.error = error;
            }
            internal static void check(int rv)
            {
                if (rv != 0) throw new Exception(rv);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CK_VERSION
        {
            public byte major;
            public byte minor;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CK_INFO
        {
            public CK_VERSION cryptokiVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] manufacturerID;
            public int flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] libraryDescription;
            public CK_VERSION libraryVersion;
        }

        public struct Info
        {
            public CK_VERSION cryptokiVersion;
            public string manufacturerID;
            public int flags;
            public string libraryDescription;
            public CK_VERSION libraryVersion;
        }

        [DllImport("kernel32")]
        internal extern static IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32")]
        internal extern static bool FreeLibrary(IntPtr module);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        internal extern static IntPtr GetProcAddress(IntPtr module, string lpProcName);

        internal static IntPtr module = IntPtr.Zero;
        internal static CK_FUNCTION_LIST fl = new CK_FUNCTION_LIST();
        internal static ETCK_FUNCTION_LIST_EX flEx = new ETCK_FUNCTION_LIST_EX();

        internal static System.Object locker = new System.Object();
        internal static Info libInfo;


        internal static void checkInitialized()
        {
            bool initialized = false;
            lock (locker)
            {
                initialized = module != IntPtr.Zero;
            }
            if (!initialized) Exception.check(CKR_CRYPTOKI_NOT_INITIALIZED);
        }

        internal static void clean()
        {
            lock (locker)
            {
                if (module == IntPtr.Zero) return;
                FreeLibrary(module);
                module = IntPtr.Zero;
            }
        }

        /*@@C_GetFunctionList
         * Function C_GetFunctionList() returns the function list.
         * @param funcList [out] receives pointer to function list
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetFunctionList(out IntPtr funcList);

        /*@@C_Initialize
         * Function C_Initialize() initializes the Cryptoki library.
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Initialize(IntPtr dummy);

        /*@@C_Finalize
        * Function C_Finalize() indicates that an application is done with the Cryptoki library.
        * @return zero	if successful CK_RV value > zero in case of failure
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Finalize(IntPtr dummy);

        /*@@C_GetInfo
         * Function C_GetInfo() returns general information about Cryptoki.
         * @param pInfo [out]	location that receives information        
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetInfo(out CK_INFO info);

        /*@@C_GetSlotList
         * Function C_GetSlotList() obtains a list of slots in the system.
         * @param present [in]	this parameter determines if only slots with present tokens would be presented in the slot list
         * @param listPtr [out] receives array of slot IDs
         * @param count [out] receives number of slots
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetSlotList(byte present, IntPtr listPtr, out int count);

        /*@@C_GetSlotInfo
         * Function C_GetSlotInfo() obtains information about a particular slot in the system.
         * @param slot [in] the ID of the slot
         * @param info [out] receives the slot information
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetSlotInfo(int slot, out Slot.CK_SLOT_INFO info);

        /*@@C_GetTokenInfo
         * Function C_GetTokenInfo() obtains information about a particular token in the system.
         * @param slot [in] ID of the token's slot
         * @param info [out] receives the token information
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetTokenInfo(int slot, out Slot.CK_TOKEN_INFO info);

        /*@@C_GetMechanismList
         * Function C_GetMechanismList() obtains a list of mechanism types supported by a token.
         * @param slot [in] ID of the token's slot
         * @param listPtr [out] gets mech. array
         * @param count [out] gets number of mechanisms
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetMechanismList(int slot, IntPtr listPtr, out int count);

        /*@@C_GetMechanismInfo
         * Function C_GetMechanismInfo() obtains information about a particular mechanism possibly supported by a token.
         * @param slot [in] ID of the token's slot
         * @param mechanism [in]	type of mechanism
         * @param info [out] receives mechanism info
         * @return zero	if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetMechanismInfo(int slot, int mechanism, out MechanismInfo info);

        /*@@C_OpenSession
         * Function C_OpenSession() opens a session between an application and a token.
         * @param slot [in] the slot's ID
         * @param flags [in] from CK_SESSION_INFO
         * @param app [in] passed to callback
         * @param notify [in] callback function
         * @param session [out] gets session handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_OpenSession(int slot, int flags, IntPtr app, IntPtr notify, out int session);

        /*@@C_GetSessionInfo
         * Function C_GetSessionInfo() obtains information about the session.
         * @param session [in] the session's handle
         * @param info [out] receives session info
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetSessionInfo(int session, out Session.CK_SESSION_INFO info);

        /*@@C_CloseSession
         * Function C_CloseSession() closes a session between an application and a token.
         * @param session [in] the session's handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_CloseSession(int session);

        /*@@C_CloseAllSessions
         * Function C_CloseAllSessions() closes all sessions with a token.
         * @param slotID [in] the token's slot
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_CloseAllSessions(int slot);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]

        /*@@C_Login
        * Function C_Login() logs a user into a token.
        * @param session [in] the session's handle
        * @param type [in] the user type
        * @param pin [in] the user's PIN
        * @param pinLen [in] the length of the PIN
        * @return zero if successful CK_RV value > zero in case of failure
        */
        internal delegate int type_C_Login(int session, int type, IntPtr pin, int pinLen);

        /*@@C_Logout
        * Function C_Logout() logs a user out from a token.
        * @param session [in] the session's handle
        * @return zero if successful CK_RV value > zero in case of failure
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Logout(int session);

        /*@@C_InitToken
         * Function C_InitToken() initializes a token.
         * @param slot [in] ID of the token's slot
         * @param pin [in]	the SO's initial PIN
         * @param pinLen [in] length in bytes of the PIN
         * @param label [in] 32-byte token label (blank padded)
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_InitToken(int slot, IntPtr pin, int pinLen, IntPtr label);
        /*@@C_InitPIN
          * Function C_InitPIN() initializes the normal user's PIN.
          * @param session [in] the session's handle
          *	@param pin [in] the normal user's PIN
          *	@param pinLen [in] length in bytes of the PIN
          *	@return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_InitPIN(int session, IntPtr pin, int cccc);
        /*@@C_SetPIN
          * Function C_SetPIN() modifies the PIN of the user who is logged in.
          * @param session [in] the session's handle
          * @param oldPin [in] the old PIN
          * @param oldPinLen [in] length of the old PIN
          * @param newPin [in] the new PIN
          * @param newPinLen [in] length of the new PIN
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SetPIN(int session, IntPtr oldPin, int oldPinLen, IntPtr newPin, int newPinLen);

        /*@@C_WaitForSlotEvent
         * Function C_WaitForSlotEvent() waits for a slot event (token insertion, removal, etc.) to occur..
         * @param flags	[in] blocking/nonblocking flag
         * @param Slot	[in] location that receives the slot ID
         * @param reserved [in]	reserved. Should be NULL_PTR
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_WaitForSlotEvent(int flags, out int Slot, IntPtr reserved);

        /*@@C_DestroyObject
         * Function C_DestroyObject() destroys an object.
         * @param session [in]	the session's handle
         * @param obj [in] the object's handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DestroyObject(int session, int obj);

        /*@@C_CreateObject
         * Function C_CreateObject() creates a new object.
         * @param session [in]	the session's handle
         * @param t	[in] the object's template
         * @param tLen [in] attributes in template
         * @param obj [out] gets new object's handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_CreateObject(int session, IntPtr t, int tLen, out int obj);

        /*@@C_SetAttributeValue
          *	Function C_SetAttributeValue() modifies the value of one or more object attributes
          * @param session [in] the session's handle
          * @param obj [in]	the object's handle
          * @param t [in] specifies attrs and values
          * @param tLen	[in] attributes in template
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SetAttributeValue(int session, int obj, IntPtr t, int tLen);

        /*@@C_GetAttributeValue
          * Function C_GetAttributeValue() obtains the value of one or more object attributes
          * @param session	[in] the session's handle
          * @param obj [in]	the object's handle
          * @param t [out] specifies attrs; gets vals
          * @param tLen [in] attributes in template
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetAttributeValue(int session, int obj, IntPtr t, int tLen);

        /*@@C_CopyObject
          * Function C_CopyObject() copies an object, creating a new object for the copy.
          *	@param session	[in] the session's handle
          * @param obj [in] the object's handle
          *	@param t [in] template for new object
          *	@param tLen [in]	attributes in template
          *	@param obj2	[out] receives handle of copy
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_CopyObject(int session, int obj, IntPtr t, int tLen, out int obj2);

        /*@@C_GetObjectSize
          * Function C_GetObjectSize() gets the size of an object in bytes. 
          * @param session [in] the session's handle
          * @param obj [in] the object's handle
          * @param size [out] receives size of object
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GetObjectSize(int session, int obj, out int size);

        /*@@C_FindObjectsInit
          * Function C_FindObjectsInit() initializes a search for token and session objects that match a template.
          * @param session [in] the session's handle
          * @param t [in] attribute values to match
          * @param tLen [in] attrs in search template
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_FindObjectsInit(int session, IntPtr t, int tLen);

        /*@@C_FindObjects
          * Function C_FindObjects() continues a search for token and session objects that match a template,  
          * obtaining additional object handles.
          * @param session [in] the session's handle
          * @param obj [in] gets obj. handles
          * @param maxCount [in] max handles to get
          * @param count [out] actual # returned
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_FindObjects(int session, out int obj, int maxCount, out int count);

        /*@@C_FindObjectsFinal
          * Function C_FindObjectsFinal() finishes a search for token and session objects.
          * @param session [in] the session's handle
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_FindObjectsFinal(int session);

        /*@@C_SeedRandom
          * Function C_SeedRandom() mixes additional seed material into the token's random number generator.
          * @param session [in] the session's handle
          * @param ptr [in] the seed material
          * @param size [in] length of seed material
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SeedRandom(int session, IntPtr ptr, int size);

        /*@@C_GenerateRandom
          * Function C_GenerateRandom() generates random data.
          * @param session [in] the session's handle
          * @param ptr [in] receives the random data
          * @param size [in] # of bytes to generate
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GenerateRandom(int session, IntPtr ptr, int size);

        /*@@C_GenerateKeyPair
          * Function C_GenerateKeyPair() generates a public-key/private-key pair, creating new key objects.
          * @param session [in] the session's handle
          * @param mech [in] key generation mech
          * @param tPub [in] template for public key
          * @param tPubSize [in] # of attrs in template for public key
          * @param tPrv [in] template for private key 
          * @param tPrvSize [in] # of attrs in template for private key
          * @param pubKey [out] gets public key handle
          * @param prvKey [out] gets private key handle
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GenerateKeyPair(int session, IntPtr mech, IntPtr tPub, int tPubSize, IntPtr tPrv, int tPrvSize, IntPtr pubKey, out int prvKey);

        /*@@C_GenerateKey
          * Function C_GenerateKey() generates a secret key, creating a new key object.
          * @param session [in] the session's handle
          * @param mech [in] key generation mech
          * @param tPub [in] template for new key
          * @param tPubSize [in] # of attrs in template
          * @param key [out] gets handle of new key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_GenerateKey(int session, IntPtr mech, IntPtr tPub, int tPubSize, out int key);

        /*@@C_EncryptInit
          * Function C_EncryptInit() initializes an encryption operation.
          * @param session [in] the session's handle
          * @param mech [in] the encryption mechanism
          * @param key [in] handle of encryption key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_EncryptInit(int session, IntPtr mech, int key);

        /*@@C_EncryptUpdate
          * Function C_EncryptUpdate() continues a multiple-part encryption operation. 
          * @param session [in] the session's handle
          * @param src [in] the plaintext data
          * @param srcSize [in] bytes of plaintext
          * @param dst [out] gets ciphertext
          * @param dstSize [out] gets c-text size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_EncryptUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_EncryptFinal
          * Function C_EncryptFinal() finishes a multiple-part encryption operation.
          * @param session [in] the session's handle
          * @param dst [out] last c-text
          * @param dstSize [out] gets last size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_EncryptFinal(int session, IntPtr dst, out int dstSize);

        /*@@C_Encrypt
          * Function C_Encrypt() encrypts single-part data. 
          * @param session [in] the session's handle
          * @param src [in] the plaintext data
          * @param srcSize [in] bytes of plaintext
          * @param dst [out] gets ciphertext
          * @param dstSize [out] gets c-text size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Encrypt(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_DecryptInit
          * Function C_DecryptInit() initializes a decryption operation.
          * @param session [in] the session's handle
          * @param mech [in] the decryption mechanism
          * @param key [in] handle of decryption key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DecryptInit(int session, IntPtr mech, int key);

        /*@@C_DecryptUpdate
          * Function C_DecryptUpdate() continues a multiple-part decryption operation.
          * @param session [in] the session's handle
          * @param src [in] encrypted data
          * @param srcSize [in] input length
          * @param dst [out] gets plaintext
          * @param dstSize [out] p-text size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DecryptUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_DecryptFinal
          * Function C_DecryptFinal() finishes a multiple-part decryption operation.
          * @param session [in] the session's handle
          * @param dst [out] gets plaintext
          * @param dstSize [out] p-text size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DecryptFinal(int session, IntPtr dst, out int cc);

        /*@@C_Decrypt
          * Function C_Decrypt() decrypts encrypted data in a single part.
          * @param session [in] the session's handle
          * @param src [in] ciphertext
          * @param srcSize [in] ciphertext length
          * @param dst [out] gets plaintext
          * @param dstSize [out] gets p-text size
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Decrypt(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_SignInit
          * Function C_SignInit() initializes a signature (private key encryption) operation,
          * where the signature is (will be) an appendix to the data,
          * and plaintext cannot be recovered from the signature.
          * @param session [in] the session's handle
          * @param mech [in] the signature mechanism
          * @param key [in] handle of signature key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignInit(int session, IntPtr mech, int key);

        /*@@C_SignUpdate
          * Function C_SignUpdate() continues a multiple-part signature operation, 
          * where the signature is (will be) an appendix to the data, 
          * and plaintext cannot be recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] the data to sign
          * @param srcSize [in] count of bytes to sign
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignUpdate(int session, IntPtr src, int srcSize);

        /*@@C_SignFinal
          * Function C_SignFinal() finishes a multiple-part signature operation,  
          * returning the signature.
          * @param session [in] the session's handle
          * @param dst [out] gets the signature
          * @param dstSize [out] gets signature length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignFinal(int session, IntPtr dst, out int dstSize);

        /*@@C_Sign
          * Function C_Sign() signs (encrypts with private key) data in a single part, 
          * where the signature is (will be) an appendix to the data,
          * and plaintext cannot be recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] the data to sign
          * @param srcSize [in] count of bytes to sign
          * @param dst [out] gets the signature
          * @param dstSize [out] gets signature length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Sign(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_VerifyInit
          * Function C_VerifyInit() initializes a verification operation,
          * where the signature is an appendix to the data,
          * and plaintext cannot be recovered from the signature (e.g. DSA).
          * @param session [in] the session's handle
          * @param mech [in] the verification mechanism
          * @param key [in] verification key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_VerifyInit(int session, IntPtr mech, int key);

        /*@@C_VerifyUpdate
          * Function C_VerifyUpdate() continues a multiple-part verification operation,
          * where the signature is an appendix to the data,
          * and plaintext cannot be recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] signed data
          * @param srcSize [in] length of signed data
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_VerifyUpdate(int session, IntPtr src, int srcSize);

        /*@@C_VerifyFinal
          * Function C_VerifyFinal() finishes a multiple-part verification operation,
          * checking the signature.
          * @param session [in] the session's handle
          * @param signature [in] signature to verify
          * @param signatureSize [in] signature length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_VerifyFinal(int session, IntPtr signature, int signatureSize);

        /*@@C_Verify
          * Function C_Verify() verifies a signature in a single-part operation,
          * where the signature is an appendix to the data,
          * and plaintext cannot be recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] signed data
          * @param srcSize [in] length of signed data
          * @param signature [in] signature
          * @param signatureSize [in] signature length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Verify(int session, IntPtr src, int srcSize, IntPtr signature, int signatureSize);

        /*@@C_DigestInit
          * Function C_DigestInit() initializes a message-digesting operation.
          * @param session [in] the session's handle
          * @param mech [in] the digesting mechanism
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DigestInit(int session, IntPtr mech);

        /*@@C_DigestUpdate
          * Function C_DigestUpdate() continues a multiple-part message-digesting operation.
          * @param session [in] the session's handle
          * @param src [in] data to be digested
          * @param srcSize [in] bytes of data to digested
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DigestUpdate(int session, IntPtr src, int srcSize);

        /*@@C_DigestFinal
          * Function C_DigestFinal() finishes a multiple-part message-digesting operation.
          * @param session [in] the session's handle
          * @param dst [out] gets the message digest
          * @param dstSize [out] gets byte count of digest
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DigestFinal(int session, IntPtr dst, out int dstSize);

        /*@@C_Digest
          * Function C_Digest() digests data in a single part. 
          * @param session [in] the session's handle
          * @param src [in] data to be digested
          * @param srcSize [in] bytes of data to digest
          * @param dst [out] gets the message digest
          * @param dstSize [out] gets digest length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_Digest(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_SignRecoverInit
          * Function C_SignRecoverInit() initializes a signature operation,
          * where the data can be recovered from the signature.
          * @param session [in] the session's handle
          * @param mech [in] the signature mechanis 
          * @param key [in] handle of the signature key
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignRecoverInit(int session, IntPtr mech, int key);

        /*@@C_SignRecover
          * Function C_SignRecover() signs data in a single operation,
          * where the data can be recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] the data to sign
          * @param srcSize [in] count of bytes to sign
          * @param dst [out] gets the signature
          * @param dstSize [out] gets signature length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignRecover(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_VerifyRecoverInit
          * Function C_VerifyRecoverInit() initializes a signature verification operation,
          * where the data is recovered from the signature.
          * @param session [in] the session's handle
          * @param mech [in] the verification mechanism
          * @param key [in] verification key          
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_VerifyRecoverInit(int session, IntPtr mech, int key);

        /*@@C_VerifyRecover
          * Function C_VerifyRecover() verifies a signature in a single-part operation,
          * where the data is recovered from the signature.
          * @param session [in] the session's handle
          * @param src [in] signature to verify
          * @param srcSize [in] signature length
          * @param signature [out] gets signed data
          * @param signatureSize [out] gets signed data len
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_VerifyRecover(int session, IntPtr src, int srcSize, IntPtr signature, int signatureSize);

        /*@@C_DigestEncryptUpdate
          * Function C_DigestEncryptUpdate() continues a multiple-part digesting and encryption operation.
          * @param session [in] the session's handle
          * @param src [in] the plaintext data
          * @param srcSize [in] plaintext length
          * @param dst [out] gets ciphertext
          * @param dstSize [out] gets c-text length         
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DigestEncryptUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_DecryptDigestUpdate
          * Function C_DecryptDigestUpdate() continues a multiple-part digesting and decryption operation.
          * @param session [in] the session's handle
          * @param src [in] ciphertext
          * @param srcSize [in] ciphertext length
          * @param dst [out] gets plaintext
          * @param dstSize [out] gets plaintext len
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DecryptDigestUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_SignEncryptUpdate
          * Function C_SignEncryptUpdate() continues a multiple-part signing and encryption operation.
          * @param session [in] the session's handle
          * @param src [in] the plaintext data
          * @param srcSize [in] plaintext length
          * @param dst [out] gets ciphertext
          * @param dstSize [out] gets c-text length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_SignEncryptUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);

        /*@@C_DecryptVerifyUpdate
          * Function C_DecryptVerifyUpdate() continues a multiple-part decryption and verify operation.
          * @param session [in] the session's handle
          * @param src [in] ciphertext
          * @param srcSize [in] ciphertext length
          * @param dst [out] gets plaintext
          * @param dstSize [out] gets p-text length
          * @return zero if successful CK_RV value > zero in case of failure
          */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_C_DecryptVerifyUpdate(int session, IntPtr src, int srcSize, IntPtr dst, out int dstSize);
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class CK_FUNCTION_LIST
        {
            public CK_VERSION version;
            /** Function C_Initialize() initializes the Cryptoki library.*/
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Initialize C_Initialize;
            /** Function C_Finalize() indicates that an application is done with the Cryptoki library. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Finalize C_Finalize;
            /** Function C_GetInfo() returns general information about Cryptoki. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetInfo C_GetInfo;
            /** Function C_GetFunctionList() returns the function list. */
            IntPtr C_GetFunctionList;
            /** Function C_GetSlotList() obtains a list of slots in the system. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            /** Function C_GetSlotInfo() obtains information about a particular slot in the system. */
            public type_C_GetSlotList C_GetSlotList;
            /** Function C_GetSlotInfo() obtains information about a particular slot in the system. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetSlotInfo C_GetSlotInfo;
            /** Function C_GetTokenInfo() obtains information about a particular token in the system. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetTokenInfo C_GetTokenInfo;
            /** Function C_GetMechanismList() obtains a list of mechanism types supported by a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetMechanismList C_GetMechanismList;
            /** Function C_GetMechanismInfo() obtains information about a particular mechanism possibly supported by a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetMechanismInfo C_GetMechanismInfo;
            /** Function C_InitToken() initializes a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_InitToken C_InitToken;
            /** Function C_InitPIN() initializes the normal user's PIN. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_InitPIN C_InitPIN;
            /** Function C_SetPIN() modifies the PIN of the user who is logged in. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SetPIN C_SetPIN;
            /** Function C_SetPIN() modifies the PIN of the user who is logged in. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_OpenSession C_OpenSession;
            /** Function C_CloseSession() closes a session between an application and a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_CloseSession C_CloseSession;
            /** Function C_CloseAllSessions() closes all sessions with a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_CloseAllSessions C_CloseAllSessions;
            /** Function C_GetSessionInfo() obtains information about the session. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetSessionInfo C_GetSessionInfo;
            /** Function C_GetOperationState() obtains the state of the cryptographic operation in a session. */
            IntPtr C_GetOperationState;
            /** Function C_SetOperationState() restores the state of the cryptographic operation in a session. */
            IntPtr C_SetOperationState;
            /** Function C_Login() logs a user into a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Login C_Login;
            /** Function C_Logout() logs a user out from a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Logout C_Logout;
            /** Function C_Logout() logs a user out from a token. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_CreateObject C_CreateObject;
            /** Function C_CopyObject() copies an object, creating a new object for the copy. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_CopyObject C_CopyObject;
            /** Function C_DestroyObject() destroys an object. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DestroyObject C_DestroyObject;
            /** Function C_GetObjectSize() gets the size of an object in bytes. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetObjectSize C_GetObjectSize;
            /** Function C_GetAttributeValue() obtains the value of one or more object attributes */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GetAttributeValue C_GetAttributeValue;
            /** Function C_SetAttributeValue() modifies the value of one or more object attributes */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SetAttributeValue C_SetAttributeValue;
            /** Function C_FindObjectsInit() initializes a search for token and session objects that match a template. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_FindObjectsInit C_FindObjectsInit;
            /** Function C_FindObjects() continues a search for token and session objects that match a template, obtaining additional object handles. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_FindObjects C_FindObjects;
            /**  Function C_FindObjectsFinal() finishes a search for token and session objects. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_FindObjectsFinal C_FindObjectsFinal;
            /** Function C_EncryptInit() initializes an encryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_EncryptInit C_EncryptInit;
            /** Function C_Encrypt() encrypts single-part data.  */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Encrypt C_Encrypt;
            /** Function C_EncryptUpdate() continues a multiple-part encryption operation.  */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_EncryptUpdate C_EncryptUpdate;
            /** Function C_EncryptFinal() finishes a multiple-part encryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_EncryptFinal C_EncryptFinal;
            /** Function C_DecryptInit() initializes a decryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DecryptInit C_DecryptInit;
            /** Function C_Decrypt() decrypts encrypted data in a single part. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Decrypt C_Decrypt;
            /** Function C_DecryptUpdate() continues a multiple-part decryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DecryptUpdate C_DecryptUpdate;
            /** Function C_DecryptFinal() finishes a multiple-part decryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DecryptFinal C_DecryptFinal;
            /**  Function C_DigestInit() initializes a message-digesting operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DigestInit C_DigestInit;
            /** Function C_Digest() digests data in a single part.  */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Digest C_Digest;
            /** Function C_DigestUpdate() continues a multiple-part message-digesting operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DigestUpdate C_DigestUpdate;
            /** Function C_DigestKey() continues a multi-part message-digesting operation,
              * by digesting the value of a secret key as part of the data already digested.
              */
            IntPtr C_DigestKey;
            /** Function C_DigestFinal() finishes a multiple-part message-digesting operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DigestFinal C_DigestFinal;
            /**  Function C_SignInit() initializes a signature (private key encryption) operation,
              * where the signature is (will be) an appendix to the data,
              * and plaintext cannot be recovered from the signature.
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignInit C_SignInit;
            /** Function C_Sign() signs (encrypts with private key) data in a single part, 
              * where the signature is (will be) an appendix to the data,
              * and plaintext cannot be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Sign C_Sign;
            /** Function C_SignUpdate() continues a multiple-part signature operation, 
              * where the signature is (will be) an appendix to the data, 
              * and plaintext cannot be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignUpdate C_SignUpdate;
            /** Function C_SignFinal() finishes a multiple-part signature operation, returning the signature. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignFinal C_SignFinal;
            /** Function C_SignRecoverInit() initializes a signature operation,
              * where the data can be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignRecoverInit C_SignRecoverInit;
            /**  Function C_SignRecover() signs data in a single operation,
              * where the data can be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignRecover C_SignRecover;
            /** Function C_VerifyInit() initializes a verification operation,
              * where the signature is an appendix to the data,
              * and plaintext cannot be recovered from the signature (e.g. DSA). 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_VerifyInit C_VerifyInit;
            /** Function C_Verify() verifies a signature in a single-part operation,
              * where the signature is an appendix to the data,
              * and plaintext cannot be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_Verify C_Verify;
            /** Function C_VerifyUpdate() continues a multiple-part verification operation,
              * where the signature is an appendix to the data,
              * and plaintext cannot be recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_VerifyUpdate C_VerifyUpdate;
            /** Function C_VerifyFinal() finishes a multiple-part verification operation,
              * checking the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_VerifyFinal C_VerifyFinal;
            /** Function C_VerifyRecoverInit() initializes a signature verification operation,
              * where the data is recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_VerifyRecoverInit C_VerifyRecoverInit;
            /** Function C_VerifyRecover() verifies a signature in a single-part operation,
              * where the data is recovered from the signature. 
              */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_VerifyRecover C_VerifyRecover;
            /** Function C_DigestEncryptUpdate() continues a multiple-part digesting and encryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DigestEncryptUpdate C_DigestEncryptUpdate;
            /** Function C_DecryptDigestUpdate() continues a multiple-part digesting and decryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DecryptDigestUpdate C_DecryptDigestUpdate;
            /** Function C_SignEncryptUpdate() continues a multiple-part signing and encryption operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SignEncryptUpdate C_SignEncryptUpdate;
            /** Function C_DecryptVerifyUpdate() continues a multiple-part decryption and verify operation. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_DecryptVerifyUpdate C_DecryptVerifyUpdate;
            /** Function C_GenerateKey() generates a secret key, creating a new key object. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GenerateKey C_GenerateKey;
            /** Function C_GenerateKeyPair() generates a public-key/private-key pair, creating new key objects. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GenerateKeyPair C_GenerateKeyPair;
            /** Function C_WrapKey() wraps (i.e., encrypts) a key. */
            IntPtr C_WrapKey;
            /** Function C_UnwrapKey() unwraps (decrypts) a wrapped key, creating a new key object. */
            IntPtr C_UnwrapKey;
            /** Function C_DeriveKey() derives a key from a base key, creating a new key object. */
            IntPtr C_DeriveKey;
            /** Function C_SeedRandom() mixes additional seed material into the token's random number generator. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_SeedRandom C_SeedRandom;
            /** Function C_GenerateRandom() generates random data. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_GenerateRandom C_GenerateRandom;
            /** Function C_GetFunctionStatus() is a legacy function it obtains an updated status of a function running in parallel with an application.*/
            IntPtr C_GetFunctionStatus;
            /** Function C_CancelFunction() is a legacy function; it cancels a function running in parallel. */
            IntPtr C_CancelFunction;
            /** Function C_WaitForSlotEvent() waits for a slot event (token insertion, removal, etc.) to occur. */
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_C_WaitForSlotEvent C_WaitForSlotEvent;
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_GetFunctionListEx(out IntPtr funcList);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_DeviceIOCTL(int slotId, int code, IntPtr pInput, int ulInputLength, IntPtr pOutput, out int pulOutputLength);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_TokenIOCTL(int hSession, int hObject, int code, IntPtr pInput, int ulInputLength, IntPtr pOutput, out int pulOutputLength);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_CreateTracker(out int hTracker, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_DestroyTracker(int hTracker);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_BeginTransaction(int hSession);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_EndTransaction(int hSession);

        /*@@ETC_GetProperty
         * Function ETC_GetProperty() retrieves value of the required property. Property has to be defined in property system.
         * @param name [in] property name
         * @param pBuffer [out] allocated buffer for property value
         * @param pulSize [out] the size of the pBuffer
         * @param pReserved [in] reserved parameter, should be NULL
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_GetProperty(IntPtr name, IntPtr pBuffer, out int pulSize, IntPtr pReserved);
        /*@@ETC_SetProperty
         * Function ETC_SetProperty() sets value to required property. Property has to be defined in property system.
         * @param name [in] property name
         * @param pBuffer [in] allocated buffer for property value
         * @parampulSize [in] the size of the pBuffer
         * @param flags
         * @param pReserved [in] reserved parameter, should be NULL
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_SetProperty(IntPtr name, IntPtr pBuffer, int ulSize, int flags, IntPtr pReserved);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_CreateVirtualSession(out int phSession);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_PrepareSoftwareToken(IntPtr pFileName, IntPtr param);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_SingleLogonGetPin(int hSession, IntPtr pPin, out int ulPinLen);

        /*@@ETC_InitTokenInit
         * Function ETC_InitTokenInit() openes session for token initialization.
         * @param slotID [in] a slot to which working token is connected
         * @param pPin [in] the administrator password
         * @param ulPinLen [in] the size of administrator password
         * @param ulRetryCounter [in] the retry counter for administrator password
         * @param pLabel [in] the label for token
         * @param phSession [out] a pointer which will point to valid session handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_InitTokenInit(int slotID, IntPtr pPin, int ulPinLen, int ulRetryCounter, IntPtr pLabel, out int phSession);

        /*@@ETC_InitTokenFinal
         * ETC_InitTokenFinal
         * @param hSession [out] a valid session handle
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_InitTokenFinal(int hSession);

        /*@@ETC_InitPIN
         * Function ETC_InitPIN() initializaion of the user password.
         * @param slotID [in] a slot to which working token is connected
         * @param pPin [in] the user password
         * @param ulPinLen [in] the size of user password
         * @param ulRetryCounter [in] the retry counter for SO password
         * @param toBeChanged [in] should the initialized pin to be changed on first use
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_InitPIN(int hSession, IntPtr pPin, int ulPinLen, int ulRetryCounter, byte toBeChanged);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_UnlockGetChallenge(int hSession, IntPtr pChallenge, out int pulChallengeLen);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_UnlockComplete(int hSession, IntPtr pResponse, int ulResponse, IntPtr pPin, int ulPinLen, int ulRetryCounter, byte toBeChanged);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_SingleLogonClearPin(int hSession);

        /*@@ETC_SetPIN
         * Function ETC_SetPIN() changes the password to the new one
         * @param hSession [in] a valid session handle
         * @param pOldDomainPin [in] the old domain password. If this parameter is set to NULL, domain password won't be changed
         * @param ulOldDomainLen [in] the size of domain password. If the domain password is set to NULL, this parameter is null eather.
         * @param pOldPin [in] the password that should be changed. It might be administrative or user password according to mode of the login.
         * @param ulOldLen [in] the size of old password
         * @param pNewPin [out] a pointer which will point buffer for new password
         * @param ulNewLen [in] the size of the new password
         * @return zero if successful CK_RV value > zero in case of failure
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_SetPIN(int hSession, IntPtr pOldDomainPin, int ulOldDomainLen, IntPtr pOldPin, int ulOldLen, IntPtr pNewPin, int ulNewLen);

        /*@@ETC_CheckFeature
         * Function ETC_CheckFeature() checked if feature exists in PKCS#11.
         * @param code [in] the code of required feature.
         * @return zero if successful CK_RV value > zero in case of failure
        */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_CheckFeature(int ulFeatureCode);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_HandleClientPacket(IntPtr pInput, int ulInputSize, out IntPtr ppOutput, out int pulOutputSize);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_FreeClientMemory(IntPtr pData);

        /*@@ETC_GetErrorInfo
         * Function ETC_GetErrorInfo() deletes passed objects from the token.
         * @param code [in] defined type of error type, for this type extended error should be retrieved
         * @param pParameter [in] reserved parameter, should be NULL
         * @return zero if successful CK_RV value > zero in case of failure
         */
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int type_ETC_GetErrorInfo(int code, IntPtr pParameter);


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal class ETCK_FUNCTION_LIST_EX
        {
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            internal struct ETCK_FUNCTION_LIST_EX_4
            {
                public CK_VERSION version;
                ushort falgs;
                IntPtr ETC_GetFunctionListEx;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_DeviceIOCTL ETC_DeviceIOCTL;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_TokenIOCTL ETC_TokenIOCTL;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_CreateTracker ETC_CreateTracker;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_DestroyTracker ETC_DestroyTracker;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_BeginTransaction ETC_BeginTransaction;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_EndTransaction ETC_EndTransaction;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_GetProperty ETC_GetProperty;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_SetProperty ETC_SetProperty;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_CreateVirtualSession ETC_CreateVirtualSession;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_PrepareSoftwareToken ETC_PrepareSoftwareToken;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_SingleLogonGetPin ETC_SingleLogonGetPin;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_InitTokenInit ETC_InitTokenInit;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_InitTokenFinal ETC_InitTokenFinal;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_InitPIN ETC_InitPIN;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_UnlockGetChallenge ETC_UnlockGetChallenge;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_UnlockComplete ETC_UnlockComplete;
                [MarshalAs(UnmanagedType.FunctionPtr)]
                public type_ETC_SingleLogonClearPin ETC_SingleLogonClearPin;
            };
            public ETCK_FUNCTION_LIST_EX_4 ver4;

            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_ETC_SetPIN ETC_SetPIN;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_ETC_CheckFeature ETC_CheckFeature;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_ETC_HandleClientPacket ETC_HandleClientPacket;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_ETC_FreeClientMemory ETC_FreeClientMemory;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public type_ETC_GetErrorInfo ETC_GetErrorInfo;
        }

        public static void Initialize()
        {
            Initialize("etpkcs11.dll");
        }

        public static void Initialize(string dllName)
        {
            lock (locker)
            {
                module = LoadLibrary(dllName);
                if (module == IntPtr.Zero) throw new Exception(CKR_FUNCTION_FAILED);

                IntPtr ptr_C_GetFunctionList = GetProcAddress(module, "C_GetFunctionList");
                IntPtr ptr_ETC_GetFunctionListEx = GetProcAddress(module, "ETC_GetFunctionListEx");

                if ((ptr_C_GetFunctionList == IntPtr.Zero) || (ptr_ETC_GetFunctionListEx == IntPtr.Zero))
                {
                    clean();
                    throw new Exception(CKR_FUNCTION_FAILED);
                }

                type_C_GetFunctionList C_GetFunctionList = (type_C_GetFunctionList)Marshal.GetDelegateForFunctionPointer(ptr_C_GetFunctionList, typeof(type_C_GetFunctionList));
                IntPtr funcListPtr;
                int rv = C_GetFunctionList(out funcListPtr);
                if (rv != 0)
                {
                    clean();
                    throw new Exception(rv);
                }

                type_ETC_GetFunctionListEx ETC_GetFunctionListEx = (type_ETC_GetFunctionListEx)Marshal.GetDelegateForFunctionPointer(ptr_ETC_GetFunctionListEx, typeof(type_ETC_GetFunctionListEx));
                IntPtr funcListExPtr;
                rv = ETC_GetFunctionListEx(out funcListExPtr);
                if (rv != 0)
                {
                    clean();
                    throw new Exception(rv);
                }

                Marshal.PtrToStructure(funcListPtr, fl);

                libInfo = GetInfo();

                if (libInfo.libraryVersion.major >= 5)
                    Marshal.PtrToStructure(funcListExPtr, flEx);
                else
                {
                    Marshal.PtrToStructure(funcListExPtr, flEx.ver4);
                }

                rv = PKCS11.fl.C_Initialize(IntPtr.Zero);
                if (rv != 0)
                {
                    clean();
                    throw new Exception(CKR_FUNCTION_FAILED);
                }
            }
        }

        public static void Finalize()
        {
            lock (locker)
            {
                if (module == IntPtr.Zero) return;
                PKCS11.fl.C_Finalize(IntPtr.Zero);
                clean();
            }
        }

        internal class Buffer
        {
            public IntPtr ptr = IntPtr.Zero;
            public int size = 0;

            public Buffer(int length)
            {
                allocate(length);
            }
            public Buffer(byte[] src)
            {
                if (src == null) return;
                allocate(src.Length);
                Marshal.Copy(src, 0, ptr, src.Length);
            }
            public Buffer(string src, bool zeroTerm)
            {
                if (src == null) return;
                char[] temp = src.ToCharArray();
                Encoder e = Encoding.UTF8.GetEncoder();
                int length = e.GetByteCount(temp, 0, temp.Length, true);
                byte[] bytes = new byte[length];
                e.GetBytes(temp, 0, temp.Length, bytes, 0, true);

                allocate(bytes.Length + (zeroTerm ? 1 : 0));
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                if (zeroTerm) Marshal.WriteByte(ptr, bytes.Length, 0);
            }
            ~Buffer()
            {
                free();
            }
            internal void free()
            {
                if (ptr != IntPtr.Zero) Marshal.FreeCoTaskMem(ptr);
                ptr = IntPtr.Zero;
            }
            public void allocate(int length)
            {
                free();
                if (length <= 0) return;
                ptr = Marshal.AllocCoTaskMem(length);
                size = length;
            }
            public byte[] data
            {
                get
                {
                    byte[] result = new byte[size];
                    Marshal.Copy(ptr, result, 0, size);
                    return result;
                }
            }
            public string text
            {
                get
                {
                    return buffer_to_string(ptr, size);
                }
            }
        }

        internal static IntPtr add_ptr(IntPtr ptr, int offset)
        {
            return (IntPtr)(ptr.ToInt64() + offset);
        }

        internal static string buffer_to_string(IntPtr ptr, int size)
        {
            int length = size;
            byte[] bytes = new byte[size];
            Marshal.Copy(ptr, bytes, 0, size);
            while (length > 0 && bytes[length - 1] == 0) length--;
            Decoder d = Encoding.UTF8.GetDecoder();
            int count = d.GetCharCount(bytes, 0, length);
            char[] temp = new char[count];
            d.GetChars(bytes, 0, length, temp, 0);
            return new string(temp);
        }

        internal static string utf8_to_string(byte[] utf8, int utf8_size)
        {
            Decoder d = Encoding.UTF8.GetDecoder();

            int charCount = d.GetCharCount(utf8, 0, utf8_size);
            char[] buffer = new char[charCount];
            d.GetChars(utf8, 0, utf8_size, buffer, 0);
            while (charCount > 0 && (buffer[charCount - 1] == ' ' || buffer[charCount - 1] == '\0')) charCount--;
            return new string(buffer, 0, charCount);
        }

        public static Info GetInfo()
        {
            checkInitialized();
            CK_INFO ckInfo;
            int rv = PKCS11.fl.C_GetInfo(out ckInfo);
            Exception.check(rv);
            Info info;
            info.cryptokiVersion = ckInfo.cryptokiVersion;
            info.flags = ckInfo.flags;
            info.libraryVersion = ckInfo.libraryVersion;
            info.manufacturerID = utf8_to_string(ckInfo.manufacturerID, 32);
            info.libraryDescription = utf8_to_string(ckInfo.libraryDescription, 32);
            return info;
        }

        public static int GetPropertyInt(string name)
        {
            Buffer bufName = new Buffer(name, true);
            Buffer bufValue = new Buffer(sizeof(int));
            int rv = PKCS11.flEx.ver4.ETC_GetProperty(bufName.ptr, bufValue.ptr, out bufValue.size, IntPtr.Zero);
            Exception.check(rv);
            return Marshal.ReadInt32(bufValue.ptr);
        }

        public static bool GetPropertyBool(string name)
        {
            return 0 != GetPropertyInt(name);
        }

        public static string GetPropertyStr(string name)
        {
            Buffer bufName = new Buffer(name, true);
            Buffer bufValue = new Buffer(300);
            int rv = PKCS11.flEx.ver4.ETC_GetProperty(bufName.ptr, bufValue.ptr, out bufValue.size, IntPtr.Zero);
            Exception.check(rv);
            return bufValue.text;
        }

        public static void SetPropertyInt(string name, int value)
        {
            SetPropertyInt(name, value, false);
        }

        public static void SetPropertyInt(string name, int value, bool thread)
        {
            Buffer bufName = new Buffer(name, true);
            int valueLen = sizeof(int);
            Buffer bufValue = new Buffer(valueLen);

            Marshal.WriteInt32(bufValue.ptr, value);

            int rv = PKCS11.flEx.ver4.ETC_SetProperty(bufName.ptr, bufValue.ptr, (int)valueLen, thread ? ETCKF_PROPERTY_THREAD : 0, IntPtr.Zero);
            Exception.check(rv);
        }

        public static void SetPropertyStr(string name, string value)
        {
            SetPropertyStr(name, value, false);
        }

        public static void SetPropertyStr(string name, string value, bool thread)
        {
            Buffer bufName = new Buffer(name, true);
            Buffer bufValue = new Buffer(value, true);

            int rv = PKCS11.flEx.ver4.ETC_SetProperty(bufName.ptr, bufValue.ptr, bufValue.size, thread ? ETCKF_PROPERTY_THREAD : 0, IntPtr.Zero);
            Exception.check(rv);
        }
    }
}