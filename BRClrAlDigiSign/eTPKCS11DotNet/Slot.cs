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
    public struct Slot
    {
      internal int id;

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      internal struct CK_SLOT_INFO
      {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] slotDescription;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] manufacturerID;
        public int flags;
        public PKCS11.CK_VERSION hardwareVersion;
        public PKCS11.CK_VERSION firmwareVersion;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      internal struct CK_TOKEN_INFO
      {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] label;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] manufacturerID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] serialNumber;
        public int flags;
        public int ulMaxSessionCount;
        public int ulSessionCount;
        public int ulMaxRwSessionCount;
        public int ulRwSessionCount;
        public int ulMaxPinLen;
        public int ulMinPinLen;
        public int ulTotalPublicMemory;
        public int ulFreePublicMemory;
        public int ulTotalPrivateMemory;
        public int ulFreePrivateMemory;
        public PKCS11.CK_VERSION hardwareVersion;
        public PKCS11.CK_VERSION firmwareVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] utcTime;
      }


      internal Slot(int id)
      {
        this.id = id;
      }

      public int Id
      {
        get { return id; }
      }

      public SlotInfo GetSlotInfo()
      {
        CK_SLOT_INFO ckInfo;
        SlotInfo info;
        int rv = PKCS11.fl.C_GetSlotInfo(id, out ckInfo);
        Exception.check(rv);
        info.firmwareVersion = ckInfo.firmwareVersion;
        info.flags = ckInfo.flags;
        info.hardwareVersion = ckInfo.hardwareVersion;
        info.manufacturerID = PKCS11.utf8_to_string(ckInfo.manufacturerID, 32);
        info.slotDescription = PKCS11.utf8_to_string(ckInfo.slotDescription, 64);
        return info;
      }

      public TokenInfo GetTokenInfo()
      {
        CK_TOKEN_INFO ckInfo;
        TokenInfo info;
        int rv = PKCS11.fl.C_GetTokenInfo(id, out ckInfo);
        Exception.check(rv);
        info.firmwareVersion = ckInfo.firmwareVersion;
        info.flags = ckInfo.flags;
        info.hardwareVersion = ckInfo.hardwareVersion;
        info.label = PKCS11.utf8_to_string(ckInfo.label, 32);
        info.manufacturerID = PKCS11.utf8_to_string(ckInfo.manufacturerID, 32);
        info.model = PKCS11.utf8_to_string(ckInfo.model, 16);
        info.serialNumber = PKCS11.utf8_to_string(ckInfo.serialNumber, 16);
        info.ulFreePrivateMemory = ckInfo.ulFreePrivateMemory;
        info.ulFreePublicMemory = ckInfo.ulFreePublicMemory;
        info.ulMaxPinLen = ckInfo.ulMaxPinLen;
        info.ulMaxRwSessionCount = ckInfo.ulMaxRwSessionCount;
        info.ulMaxSessionCount = ckInfo.ulMaxSessionCount;
        info.ulMinPinLen = ckInfo.ulMinPinLen;
        info.ulRwSessionCount = ckInfo.ulRwSessionCount;
        info.ulSessionCount = ckInfo.ulSessionCount;
        info.ulTotalPrivateMemory = ckInfo.ulTotalPrivateMemory;
        info.ulTotalPublicMemory = ckInfo.ulTotalPublicMemory;
        info.utcTime = PKCS11.utf8_to_string(ckInfo.utcTime, 16);
        return info;
      }

      public string GetFullName()
      {
        Buffer buffer = new Buffer(300);
        int rv = PKCS11.flEx.ver4.ETC_DeviceIOCTL(id, ETCK_IODEV_FULL_NAME, IntPtr.Zero, 0, buffer.ptr, out buffer.size);
        Exception.check(rv);
        return buffer.text;
      }

      public void CloseAllSessions()
      {
        PKCS11.fl.C_CloseAllSessions(id);
      }

      public void InitToken(string pin, string label)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        Buffer labelBuffer = new Buffer(label, true);
        int rv = PKCS11.fl.C_InitToken(id, pinBuffer.ptr, pinBuffer.size, labelBuffer.ptr);
        Exception.check(rv);
      }

      public Session InitTokenInit(string pin, int retry, string label)
      {
        Buffer pinBuffer = new Buffer(pin, false);
        Buffer labelBuffer = new Buffer(label, true);

        int sessionID;
        int rv = PKCS11.flEx.ver4.ETC_InitTokenInit(id, pinBuffer.ptr, pinBuffer.size, retry, labelBuffer.ptr, out sessionID); 
        Exception.check(rv);

        return new Session(sessionID);
      }
    }

    public struct SlotInfo
    {
      public string slotDescription;
      public string manufacturerID;
      public int flags;
      public CK_VERSION hardwareVersion;
      public CK_VERSION firmwareVersion;
    }

    public struct TokenInfo
    {
      public string label;
      public string manufacturerID;
      public string model;
      public string serialNumber;
      public int flags;
      public int ulMaxSessionCount;
      public int ulSessionCount;
      public int ulMaxRwSessionCount;
      public int ulRwSessionCount;
      public int ulMaxPinLen;
      public int ulMinPinLen;
      public int ulTotalPublicMemory;
      public int ulFreePublicMemory;
      public int ulTotalPrivateMemory;
      public int ulFreePrivateMemory;
      public CK_VERSION hardwareVersion;
      public CK_VERSION firmwareVersion;
      public string utcTime;
    }

    public static Slot[] GetSlotList(bool present)
    {
      checkInitialized();
      byte tokenPresent = (byte)(present ? 1 : 0);
      int count = 0;
      int rv = PKCS11.fl.C_GetSlotList(tokenPresent, IntPtr.Zero, out count);
      Exception.check(rv);

      if (count == 0) return new Slot[0];

      Slot[] list = null;
      Buffer buffer = new Buffer(count * sizeof(int));
      rv = PKCS11.fl.C_GetSlotList(tokenPresent, buffer.ptr, out count);
      Exception.check(rv);

      list = new Slot[count];
      for (int i = 0; i < count; i++)
      {
        int id = Marshal.ReadInt32(buffer.ptr, i * sizeof(int));
        list[i] = new Slot(id);
      }
      return list;
    }

    public static int WaitForslotEvent(bool dontBlock, out Slot slot)
    {
      checkInitialized();
      int flags = (int)(dontBlock ? 1 : 0);
      int id;
      slot = new Slot();
      int rv = PKCS11.fl.C_WaitForSlotEvent(flags, out id, IntPtr.Zero);
      if (rv != 0) return rv;
      slot = new Slot(id);
      return 0;
    }

    public struct Tracker
    {
      internal int id;

      internal Tracker(int id)
      {
        this.id = id;
      }

      public static Tracker Create()
      {
        checkInitialized();
        int id;
        int rv = PKCS11.flEx.ver4.ETC_CreateTracker(out id, IntPtr.Zero);
        Exception.check(rv);
        return new Tracker(id);
      }

      public void Destroy()
      {
        PKCS11.flEx.ver4.ETC_DestroyTracker(id);
      }

      public int WaitForslotEvent(bool dontBlock, out Slot slot)
      {
        int flags = (int)(dontBlock ? 1 : 0);
        int slotID;
        slot = new Slot();
        int rv = PKCS11.fl.C_WaitForSlotEvent(flags, out slotID, (IntPtr)id);
        if (rv != 0) return rv;
        slot = new Slot(slotID);
        return 0;
      }
    }

  }
}