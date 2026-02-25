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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MechanismInfo
    {
      public int ulMinKeySize;
      public int ulMaxKeySize;
      public int flags;
    };

    public struct Mechanism
    {
      public int id;
      public byte[] iv;
      public int length;

      public Mechanism(int id)
      {
        this.iv = null;
        this.length = 0;
        this.id = id;
      }

      public Mechanism(int id, int length)
      {
        this.iv = null;
        this.length = length;
        this.id = id;
      }

      public Mechanism(int id, byte[] iv)
      {
        this.length = 0;
        this.id = id;
        this.iv = iv;
      }

      public MechanismInfo GetMechanismInfo(Slot slot)
      {
        MechanismInfo info = new MechanismInfo();
        int rv = PKCS11.fl.C_GetMechanismInfo(slot.id, id, out info);
        Exception.check(rv);
        return info;
      }

    }

    public static Mechanism[] GetMechanismList(Slot slot)
    {
      checkInitialized();
      int count = 0;
      int rv = PKCS11.fl.C_GetMechanismList(slot.id, IntPtr.Zero, out count);
      Exception.check(rv);

      if (count == 0) return new Mechanism[0];

      Mechanism[] list = null;
      Buffer buffer = new Buffer(count * sizeof(int));
      rv = PKCS11.fl.C_GetMechanismList(slot.id, buffer.ptr, out count);
      Exception.check(rv);

      list = new Mechanism[count];
      for (int i = 0; i < count; i++)
      {
        int id = Marshal.ReadInt32(buffer.ptr, i * sizeof(int));
        list[i] = new Mechanism(id);
      }

      return list;
    }

    public partial struct Object
    {
      public static Object GenerateKey(Session session, Mechanism mechanism, Attribute[] t)
      {
        int objid;
        Buffer tBuf = template_to_buffer(t, false);
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_GenerateKey(session.id, mech.ptr, tBuf.ptr, t.Length, out objid);
        Exception.check(rv);
        return new Object(objid);
      }
    }

    public partial struct Session
    {
      public void SeedRandom(byte[] data)
      {
        Buffer buffer = new Buffer(data);
        int rv = PKCS11.fl.C_SeedRandom(id, buffer.ptr, buffer.size);
        Exception.check(rv);
      }

      public byte[] GenerateRandom(int size)
      {
        Buffer buffer = new Buffer(size);
        int rv = PKCS11.fl.C_GenerateRandom(id, buffer.ptr, size);
        Exception.check(rv);
        return buffer.data;
      }

      public void EncryptInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_EncryptInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      public void DecryptInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_DecryptInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      public void DigestInit(Mechanism mechanism)
      {
        Buffer mech = mechanism_to_buffer(mechanism); 
        int rv = PKCS11.fl.C_DigestInit(id, mech.ptr);
        Exception.check(rv);
      }

      public void SignInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_SignInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      public void VerifyInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_VerifyInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      public void SignRecoverInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_SignRecoverInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      public void VerifyRecoverInit(Mechanism mechanism, Object key)
      {
        Buffer mech = mechanism_to_buffer(mechanism);
        int rv = PKCS11.fl.C_VerifyRecoverInit(id, mech.ptr, key.id);
        Exception.check(rv);
      }

      internal const int whole = 0;
      internal const int update = 1;
      internal const int final = 2;

      internal const int digest = 0;
      internal const int encrypt = 1;
      internal const int decrypt = 2;
      internal const int sign = 3;
      internal const int verify = 4;
      internal const int sign_recover = 5;
      internal const int verify_recover = 6;
      internal const int digest_encrypt = 7;
      internal const int decrypt_digest = 8;
      internal const int sign_encrypt = 9;
      internal const int decrypt_verify = 10;

      internal class arg
      {
        internal arg(byte[] data, int offset, int size)
        {
          this.data = data;
          this.offset = offset;
          this.size = size;
        }
        internal byte[] data;
        internal int offset;
        internal int size;
      }

      internal int crypt(int operation, int stage, byte[] signature, arg src, arg dst)
      {
        int sid = this.id;
        int rv = 0;
        int srcSize = src==null ? 0 : src.size;
        int dstSize = dst==null ? 0 : dst.size;
        IntPtr srcBuffer = IntPtr.Zero;
        IntPtr dstBuffer = IntPtr.Zero;
        IntPtr signatureBuffer = IntPtr.Zero;

        if ((signature!=null) && (signature.Length>0))
        {
          signatureBuffer = Marshal.AllocCoTaskMem(signature.Length);
          Marshal.Copy(signature, 0, signatureBuffer, signature.Length);
        }
        if ((src!=null) && (src.data!=null) && (src.size>0))
        {
          srcBuffer = Marshal.AllocCoTaskMem(src.size);
          Marshal.Copy(src.data, src.offset, srcBuffer, src.size);
        }
        if ((dst != null) && (dst.data!=null) && (dst.size>0))
        {
          dstBuffer = Marshal.AllocCoTaskMem(dst.size);
        }

        switch (operation)
        {
          case digest:
            switch (stage)
            {
              case whole:  rv = PKCS11.fl.C_Digest(sid, srcBuffer, srcSize, dstBuffer, out dstSize); break;
              case update: rv = PKCS11.fl.C_DigestUpdate(sid, srcBuffer, srcSize);                   break;
              case final:  rv = PKCS11.fl.C_DigestFinal(sid, dstBuffer, out dstSize);                break;
            }
            break;
          case encrypt:
            switch (stage)
            {
              case whole:  rv = PKCS11.fl.C_Encrypt(sid, srcBuffer, srcSize, dstBuffer, out dstSize);        break;
              case update: rv = PKCS11.fl.C_EncryptUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize);  break;
              case final:  rv = PKCS11.fl.C_EncryptFinal(sid, dstBuffer, out dstSize);                       break;
            }
            break;
          case decrypt:
            switch (stage)
            {
              case whole:  rv = PKCS11.fl.C_Decrypt(sid, srcBuffer, srcSize, dstBuffer, out dstSize);        break;
              case update: rv = PKCS11.fl.C_DecryptUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize);  break;
              case final:  rv = PKCS11.fl.C_DecryptFinal(sid, dstBuffer, out dstSize);                       break;
            }
            break;
          case sign:
            switch (stage)
            {
              case whole:  rv = PKCS11.fl.C_Sign(sid, srcBuffer, srcSize, dstBuffer, out dstSize);   break;
              case update: rv = PKCS11.fl.C_SignUpdate(sid, srcBuffer, srcSize);                     break;
              case final:  rv = PKCS11.fl.C_SignFinal(sid, dstBuffer, out dstSize);                  break;
            }
            break;
          case verify:
            switch (stage)
            {
              case whole:  rv = PKCS11.fl.C_Verify(sid, srcBuffer, srcSize, signatureBuffer, signature.Length);  break;
              case update: rv = PKCS11.fl.C_VerifyUpdate(sid, srcBuffer, srcSize);                               break;
              case final:  rv = PKCS11.fl.C_VerifyFinal(sid, signatureBuffer, signature.Length);                 break;
            }
            break;
          case sign_recover:
            rv = PKCS11.fl.C_SignRecover(sid, srcBuffer, srcSize, dstBuffer, out dstSize);
            break;
          case verify_recover:
            rv = PKCS11.fl.C_VerifyRecover(sid, srcBuffer, srcSize, signatureBuffer, signature.Length);
            break;
          case digest_encrypt:
            rv = PKCS11.fl.C_DigestEncryptUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize);
            break;
          case decrypt_digest:
            rv = PKCS11.fl.C_DecryptDigestUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize);
            break;
          case sign_encrypt:
            rv = PKCS11.fl.C_SignEncryptUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize);
            break;
          case decrypt_verify:
            rv = PKCS11.fl.C_DecryptVerifyUpdate(sid, srcBuffer, srcSize, dstBuffer, out dstSize); 
            break;
        }

        if ((dst != null) && (dst.data != null) && (dst.size > 0))
        {
          Marshal.Copy(dstBuffer, dst.data, dst.offset, dst.size);
        }
        Marshal.FreeCoTaskMem(srcBuffer);
        Marshal.FreeCoTaskMem(dstBuffer);
        Exception.check(rv);
        return dstSize;
      }

      public void DigestUpdate(byte[] src, int srcOffset, int srcSize)
      {
        crypt(digest, update, null, new arg(src, srcOffset, srcSize), null);
      }

      public int DigestFinal(byte[] dst, int dstOffset)
      {
        int dstSize = crypt(digest, final, null, null, null);
        if (dst == null) return dstSize;
        return crypt(digest, final, null, null, new arg(dst, dstOffset, dstSize));
      }

      public byte[] Digest(Mechanism mechanism, byte[] src)
      {
        DigestInit(mechanism);

        int dstSize = crypt(digest, whole, null, new arg(null, 0, src.Length), null);
        byte[] dst = new byte[dstSize];
        crypt(digest, whole, null, new arg(src, 0, src.Length), new arg(dst, 0, dst.Length));
        return dst;
      }

      public int EncryptUpdate(byte[] src, int srcOffset, int srcSize, byte[] dst, int dstOffset)
      {
        int dstSize = crypt(encrypt, update, null, new arg(null, 0, srcSize), null);
        if (dst==null) return dstSize;
        return crypt(encrypt, update, null, new arg(src, srcOffset, srcSize), new arg(dst, dstOffset, dstSize));
      }

      public int EncryptFinal(byte[] dst, int dstOffset)
      {
        int dstSize = crypt(encrypt, final, null, null, null);
        if (dst == null) return dstSize;
        return crypt(encrypt, final, null, null, new arg(dst, dstOffset, dstSize));
      }

      public byte[] Encrypt(Mechanism mechanism, Object key, byte[] src)
      {
        EncryptInit(mechanism, key);

        int dstSize = crypt(encrypt, whole, null, new arg(null, 0, src.Length), null);
        byte[] dst = new byte[dstSize];
        crypt(encrypt, whole, null, new arg(src, 0, src.Length), new arg(dst, 0, dst.Length));
        return dst;
      }

      public int DecryptUpdate(byte[] src, int srcOffset, int srcSize, byte[] dst, int dstOffset)
      {
        int dstSize = crypt(decrypt, update, null, new arg(src, srcOffset, srcSize), null);
        if (dst == null) return dstSize;
        return crypt(decrypt, update, null, new arg(src, srcOffset, srcSize), new arg(dst, dstOffset, dstSize));
      }

      public int DecryptFinal(byte[] dst, int dstOffset)
      {
        int dstSize = crypt(decrypt, final, null, null, null);
        if (dst == null) return dstSize;
        return crypt(decrypt, final, null, null, new arg(dst, dstOffset, dstSize));
      }

      public byte[] Decrypt(Mechanism mechanism, Object key, byte[] src)
      {
        DecryptInit(mechanism, key);

        int dstSize = crypt(encrypt, whole, null, new arg(null, 0, src.Length), null);
        byte[] dst = new byte[dstSize];
        crypt(encrypt, whole, null, new arg(src, 0, src.Length), new arg(dst, 0, dst.Length));
        return dst;
      }

      public void SignUpdate(byte[] src, int srcOffset, int srcSize)
      {
        crypt(sign, update, null, new arg(src, srcOffset, srcSize), null);
      }

      public int SignFinal(byte[] dst, int dstOffset)
      {
        int dstSize = crypt(sign, final, null, null, null);
        if (dst == null) return dstSize;
        return crypt(sign, final, null, null, new arg(dst, dstOffset, dstSize));
      }

      public byte[] Sign(Mechanism mechanism, Object key, byte[] src)
      {
        SignInit(mechanism, key);

        int dstSize = crypt(sign, whole, null, new arg(null, 0, src.Length), null);
        byte[] dst = new byte[dstSize];
        crypt(sign, whole, null, new arg(src, 0, src.Length), new arg(dst, 0, dst.Length));
        return dst;
      }

      public void VerifyUpdate(byte[] src, int srcOffset, int srcSize)
      {
        crypt(verify, update, null, new arg(src, srcOffset, srcSize), null);
      }

      public int VerifyFinal(byte[] signature)
      {
        try
        {
          crypt(verify, final, signature, null, null);
        }
        catch (Exception e)
        {
          return e.error;
        }
        return 0;
      }

      public int Verify(Mechanism mechanism, Object key, byte[] src, byte[] signature)
      {
        SignInit(mechanism, key);
        try
        {
          crypt(verify, whole, signature, new arg(src, 0, src.Length), null);
        }
        catch (Exception e)
        {
          return e.error;
        }
        return 0;
      }
    }
      
    internal static Buffer mechanism_to_buffer(Mechanism mechanism)
    {
      bool paramIsIV = false;
      IntPtr param = IntPtr.Zero;
      int paramSize = 0;
      switch (mechanism.id)
      {
        case PKCS11.CKM_AES_CBC:
        case PKCS11.CKM_AES_CBC_PAD:
        case PKCS11.CKM_DES_CBC:
        case PKCS11.CKM_DES_CBC_PAD:
        case PKCS11.CKM_DES3_CBC:
        case PKCS11.CKM_DES3_CBC_PAD:
          paramSize = mechanism.iv.Length;
          paramIsIV = true;
          break;
        case CKM_AES_MAC_GENERAL:
        case CKM_DES_MAC_GENERAL:
        case CKM_DES3_MAC_GENERAL:
        case CKM_MD5_HMAC_GENERAL:
        case CKM_SHA_1_HMAC_GENERAL:
          paramSize = sizeof(int);
          break;
      }
      int structSize = sizeof(int) + IntPtr.Size + sizeof(int);
      Buffer buffer = new Buffer(structSize + paramSize);
      if (paramSize > 0)
      {
        param = add_ptr(buffer.ptr, structSize);
        if (paramIsIV) Marshal.Copy(mechanism.iv, 0, param, paramSize);
        else Marshal.WriteInt32(param, mechanism.length);
      }
      Marshal.WriteInt32(buffer.ptr, mechanism.id);
      Marshal.WriteIntPtr(buffer.ptr, sizeof(int), param);
      Marshal.WriteInt32(buffer.ptr, sizeof(int) + IntPtr.Size, paramSize);
      return buffer;
    }

    internal static void GenerateKeyPair(Session session, Mechanism mechanism, bool genPub, Attribute[] pub, out Object pubKey, Attribute[] prv, out Object prvKey)
    {
      prvKey = new Object();
      pubKey = new Object();
      int prvKeyId;
      Buffer tPub = template_to_buffer(pub, false);
      Buffer tPrv = template_to_buffer(prv, false);
      Buffer mech = mechanism_to_buffer(mechanism);
      IntPtr pubKeyPtr = IntPtr.Zero;
      if (genPub)
      {
        pubKeyPtr = Marshal.AllocCoTaskMem(sizeof(int));
      }
      int rv = PKCS11.fl.C_GenerateKeyPair(session.id, mech.ptr, tPub.ptr, pub.Length, tPrv.ptr, prv.Length, pubKeyPtr, out prvKeyId);
      if (rv == 0)
      {
        prvKey = new Object(prvKeyId);
        if (genPub)
        {
          int pubKeyId = Marshal.ReadInt32(pubKeyPtr);
          pubKey = new Object(pubKeyId); ;
        }
      }
      Marshal.FreeCoTaskMem(pubKeyPtr);
      Exception.check(rv);
    }

    public static void GenerateKeyPair(Session session, Mechanism mechanism, Attribute[] pub, out Object pubKey, Attribute[] prv, out Object prvKey)
    {
      GenerateKeyPair(session, mechanism, true, pub, out pubKey, prv, out prvKey);
    }

    public static Object GenerateKeyPair(Session session, Mechanism mechanism, Attribute[] pub, Attribute[] prv)
    {
      Object pubKey;
      Object prvKey;
      GenerateKeyPair(session, mechanism, false, pub, out pubKey, prv, out prvKey);
      return prvKey;
    }
  }
}