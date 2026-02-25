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
    public struct SessionInfo
    {
      public Slot slot;
      public int state;
      public int flags;
      public int ulDeviceError;
    }

    public partial struct Session
    {
      internal int id;

      internal Session(int id)
      {
        this.id = id;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      internal struct CK_SESSION_INFO
      {
        public int slotID;
        public int state;
        public int flags;
        public int ulDeviceError;
      }

      public int Login(int type, string pin)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        int rv = PKCS11.fl.C_Login(id, type, pinBuffer.ptr, pinBuffer.size);
        return rv;
      }

      public void Logout()
      {
        PKCS11.fl.C_Logout(id);
      }

      public void Close()
      {
        PKCS11.fl.C_CloseSession(id);
      }

      public void InitTokenFinal()
      {
        int rv = PKCS11.flEx.ver4.ETC_InitTokenFinal(id);
        Exception.check(rv);
      }

      public void InitPIN(string pin, int retry, bool forceChange)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        int rv = PKCS11.flEx.ver4.ETC_InitPIN(id, pinBuffer.ptr, pinBuffer.size, retry, (byte)(forceChange ? 1 : 0));
        Exception.check(rv);
      }

      public void InitPIN(string pin)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        int rv = PKCS11.fl.C_InitPIN(id, pinBuffer.ptr, pinBuffer.size);
        Exception.check(rv);
      }

      public int SetPIN(string oldPin, string newPin)
      {
        Buffer oldPinBuffer = new Buffer(oldPin, false);
        Buffer newPinBuffer = new Buffer(newPin, false);
        int rv = PKCS11.fl.C_SetPIN(id, oldPinBuffer.ptr, oldPinBuffer.size, newPinBuffer.ptr, newPinBuffer.size);
        return rv;
      }

      public SessionInfo GetInfo()
      {
        CK_SESSION_INFO ckInfo;
        int rv = PKCS11.fl.C_GetSessionInfo(id, out ckInfo);
        SessionInfo info;
        info.flags = ckInfo.flags;
        info.state = ckInfo.state;
        info.ulDeviceError = ckInfo.ulDeviceError;
        info.slot = new PKCS11.Slot(ckInfo.slotID);
        return info;
      }

      public string SingleLogonGetPin()
      {
        Buffer pinBuffer = new Buffer(300);
        int rv = PKCS11.flEx.ver4.ETC_SingleLogonGetPin(id, pinBuffer.ptr, out pinBuffer.size);
        Exception.check(rv);
        return pinBuffer.text;
      }

      public void SingleLogonClearPin()
      {
        int rv = PKCS11.flEx.ver4.ETC_SingleLogonClearPin(id);
        Exception.check(rv);
      }

      public byte[] UnlockGetChallenge()
      {
        Buffer buffer = new Buffer(8);
        int rv = PKCS11.flEx.ver4.ETC_UnlockGetChallenge(id, buffer.ptr, out buffer.size);
        Exception.check(rv);
        return buffer.data;
      }

      public void UnlockComplete(byte[] response, string pin, int retry, bool forceChange)
      {
        Buffer buffer = new Buffer(response);
        Buffer pinBuffer = new Buffer(pin, false);
        int rv = PKCS11.flEx.ver4.ETC_UnlockComplete(id, buffer.ptr, buffer.size, pinBuffer.ptr, pinBuffer.size, retry, (byte)(forceChange ? 1 : 0));
        Exception.check(rv);
      }

      public int EvalatePin(string pin, out int percent)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        Buffer percentBuffer = new Buffer(sizeof(int));
        CK_SESSION_INFO ckInfo;
        PKCS11.flEx.ver4.ETC_TokenIOCTL(id, 0, ETCK_IOCTL_PIN_EVALUATE, pinBuffer.ptr, pinBuffer.size, percentBuffer.ptr, out percentBuffer.size);
        int rv = PKCS11.fl.C_GetSessionInfo(id, out ckInfo);
        Exception.check(rv);
        percent = Marshal.ReadInt32(percentBuffer.ptr);
        return ckInfo.ulDeviceError;
      }

      public int EvalatePin()
      {
        CK_SESSION_INFO ckInfo;
        int dummy = 0;
        PKCS11.flEx.ver4.ETC_TokenIOCTL(id, 0, ETCK_IOCTL_PIN_EVALUATE, IntPtr.Zero, 0, IntPtr.Zero, out dummy);
        int rv = PKCS11.fl.C_GetSessionInfo(id, out ckInfo);
        Exception.check(rv);
        return ckInfo.ulDeviceError;
      }
    }

    public static Session OpenSession(Slot slot, int flags)
    {
      checkInitialized();
      int id;
      int rv = PKCS11.fl.C_OpenSession(slot.id, flags, IntPtr.Zero, IntPtr.Zero, out id);
      Exception.check(rv);
      return new Session(id);
    }

    public static Session CreateVirtualSession()
    {
      checkInitialized();
      int id;
      int rv = PKCS11.flEx.ver4.ETC_CreateVirtualSession(out id);
      Exception.check(rv);
      return new Session(id);
    }
  
  }


}
