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
    public class Attribute
    {
      public int type;
      public object value;


      public Attribute(int type)
      {
        this.type = type;
        this.value = null;
      }

      public Attribute(int type, object value)
      {
        this.type = type;
        this.value = value;
      }
    }

    public partial struct Object
    {
      internal int id;

      internal Object(int id)
      {
        this.id = id;
      }

      public int GetSize(Session session)
      {
        int size;
        int rv = PKCS11.fl.C_GetObjectSize(session.id, id, out size);
        Exception.check(rv);
        return size;
      }

      public void Destroy(Session session)
      {
        int rv = PKCS11.fl.C_DestroyObject(session.id, id);
        Exception.check(rv);
      }

      public static Object Create(Session session, Attribute[] t)
      {
        int objid;
        Buffer tBuf = template_to_buffer(t, false);
        int rv = PKCS11.fl.C_CreateObject(session.id, tBuf.ptr, t.Length, out objid);
        Exception.check(rv);
        return new Object(objid);
      }

      public Object Copy(Session session, Attribute[] t)
      {
        int objid;
        Buffer tBuf = template_to_buffer(t, false);
        int rv = PKCS11.fl.C_CopyObject(session.id, id, tBuf.ptr, t.Length, out objid);
        Exception.check(rv);
        return new Object(objid);
      }

      public void Set(Session session, Attribute[] t)
      {
        Buffer tBuf = template_to_buffer(t, false);
        int rv = PKCS11.fl.C_SetAttributeValue(session.id, id, tBuf.ptr, t.Length);
        Exception.check(rv);
      }

      public void Set(Session session, Attribute a)
      {
        Set(session, new Attribute[] {a} );
      }

      public void Set(Session session, int type, object value)
      {
        Set(session, new Attribute(type, value));
      }

      internal static IntPtr attr(IntPtr buffer, int index, int offset) { return add_ptr(buffer, index * (sizeof(int) + IntPtr.Size + sizeof(int)) + offset); }
      internal static IntPtr attr_type(IntPtr buffer, int index) { return attr(buffer, index, 0); }
      internal static IntPtr attr_data(IntPtr buffer, int index) { return attr(buffer, index, sizeof(int)); }
      internal static IntPtr attr_size(IntPtr buffer, int index) { return attr(buffer, index, sizeof(int) + IntPtr.Size); }

      internal static int attr_get_type(IntPtr buffer, int index) { return Marshal.ReadInt32(attr_type(buffer, index)); }
      internal static IntPtr attr_get_data(IntPtr buffer, int index) { return Marshal.ReadIntPtr(attr_data(buffer, index)); }
      internal static int attr_get_size(IntPtr buffer, int index) { return Marshal.ReadInt32(attr_size(buffer, index)); }
      internal static void attr_set_type(IntPtr buffer, int index, int v) { Marshal.WriteInt32(attr_type(buffer, index), v); }
      internal static void attr_set_data(IntPtr buffer, int index, IntPtr v) { Marshal.WriteIntPtr(attr_data(buffer, index), v); }
      internal static void attr_set_size(IntPtr buffer, int index, int v) { Marshal.WriteInt32(attr_size(buffer, index), v); }

      public int GetAttributeSize(Session session, int type)
      {
        int size = 0;
        Attribute[] t =  { new Attribute(type) };
        Buffer tBuf = template_to_buffer(t, true);
        int rv = PKCS11.fl.C_GetAttributeValue(session.id, id, tBuf.ptr, 1);
        if (rv == PKCS11.CKR_BUFFER_TOO_SMALL) rv = 0;
        if (rv == 0)
        {
          size = attr_get_size(tBuf.ptr, 0);
          if (size<0) size = 0;
        }
        Exception.check(rv);
        return size;
      }

      public void Get(Session session, Attribute[] t)
      {
        Buffer tBuf = template_to_buffer(t, true);
        int rv = PKCS11.fl.C_GetAttributeValue(session.id, id, tBuf.ptr, t.Length);
        if (rv==PKCS11.CKR_BUFFER_TOO_SMALL) rv = 0;

        if (rv==0)
        {
          int size = 0;
          for (int i=0; i<t.Length; i++)
          {
            int s = attr_get_size(tBuf.ptr, i);
            if (s>0) size+=s;
          }
          IntPtr vBuf = size>0 ? Marshal.AllocCoTaskMem(size) : IntPtr.Zero;

          int offset = 0;
          for (int i = 0; i < t.Length; i++)
          {
            int s = attr_get_size(tBuf.ptr, i);
            if (s > 0) 
            {
              attr_set_data(tBuf.ptr, i, add_ptr(vBuf, offset));
              offset += s;
            }
          }

          rv = PKCS11.fl.C_GetAttributeValue(session.id, id, tBuf.ptr, t.Length);

          if (rv==0)
          {
            for (int i = 0; i < t.Length; i++)
            {
              int s = attr_get_size(tBuf.ptr, i);
              if (s<0) t[i].value = null;
              else
              {
                int type = attr_get_type(tBuf.ptr, i);
                IntPtr data = attr_get_data(tBuf.ptr, i);
                t[i].value = createAttributeValue(type, data, s);
              }
            }
          }

          Marshal.FreeCoTaskMem(vBuf);
        }
        Exception.check(rv);
      }

      public object Get(Session session, int type)
      {
        Attribute[] t = { new Attribute(type) };
        Get(session, t);
        return t[0].value;
      }

    }

    internal static int getAttributeSize(object value)
    {
      if (value == null) return 0;
      TypeCode code;
      Type type = value.GetType();
      if (type.IsArray)
      {
        Type elementType = type.GetElementType();
        code = Type.GetTypeCode(elementType);
        if (code != TypeCode.Byte) return 0;
        return ((byte[])value).Length;
      }
      code = Type.GetTypeCode(type);

      switch (code)
      {
        case TypeCode.Boolean:
          return 1;

        case TypeCode.String:
          char[] temp = ((string)value).ToCharArray();
          Encoder e = Encoding.UTF8.GetEncoder();
          return e.GetByteCount(temp, 0, temp.Length, true);

        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.SByte:
          return 4;
      }
      return 0;
    }

    internal static void storeAttributeValue(IntPtr ptr, object value)
    {
      if (value == null) return;
      TypeCode code;
      Type type = value.GetType();
      if (type.IsArray)
      {
        Type elementType = type.GetElementType();
        code = Type.GetTypeCode(elementType);
        if (code != TypeCode.Byte) return;
        Marshal.Copy((byte[])value, 0, ptr, ((byte[])value).Length);
        return;
      }
      code = Type.GetTypeCode(type);

      switch (code)
      {
        case TypeCode.Boolean:
          Marshal.WriteByte(ptr, (byte)((bool)value ? 1 : 0));
          return;

        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.SByte:
          Marshal.WriteInt32(ptr, (int)value);
          return;

        case TypeCode.String:
          char[] temp = ((string)value).ToCharArray();
          Encoder e = Encoding.UTF8.GetEncoder();
          int size = e.GetByteCount(temp, 0, temp.Length, true);
          byte[] buffer = new byte[size];
          e.GetBytes(temp, 0, temp.Length, buffer, 0, true);
          Marshal.Copy(buffer, 0, ptr, size);
          return;
      }
    }

    internal static object createAttributeValue(int type, IntPtr ptr, int size)
    {
      switch (type)
      {
        case PKCS11.CKA_CLASS:
        case PKCS11.CKA_CERTIFICATE_TYPE:   
        case PKCS11.CKA_CERTIFICATE_CATEGORY:     
        case PKCS11.CKA_KEY_TYPE:                 
        case PKCS11.CKA_MODULUS_BITS:             
        case PKCS11.CKA_VALUE_LEN:                
        case PKCS11.CKA_HW_FEATURE_TYPE:
          return Marshal.ReadInt32(ptr);

        case PKCS11.CKA_TOKEN:                    
        case PKCS11.CKA_PRIVATE:                  
        case PKCS11.CKA_EXTRACTABLE:              
        case PKCS11.CKA_LOCAL:                    
        case PKCS11.CKA_NEVER_EXTRACTABLE:        
        case PKCS11.CKA_ALWAYS_SENSITIVE:         
        case PKCS11.CKA_MODIFIABLE:               
        case PKCS11.CKA_ALWAYS_AUTHENTICATE:      
        case PKCS11.CKA_SENSITIVE:                
        case PKCS11.CKA_ENCRYPT:                  
        case PKCS11.CKA_DECRYPT:                  
        case PKCS11.CKA_WRAP:                     
        case PKCS11.CKA_UNWRAP:                   
        case PKCS11.CKA_SIGN:                     
        case PKCS11.CKA_SIGN_RECOVER:             
        case PKCS11.CKA_VERIFY:                   
        case PKCS11.CKA_VERIFY_RECOVER:           
        case PKCS11.CKA_DERIVE:
          return Marshal.ReadByte(ptr)==0 ? false : true;

        case PKCS11.CKA_LABEL:                    
        case PKCS11.CKA_APPLICATION:              
        case PKCS11.CKA_START_DATE:               
        case PKCS11.CKA_END_DATE:
          return buffer_to_string(ptr, size);
                
        default:
          byte[] result = new byte[size];
          Marshal.Copy(ptr, result, 0, size);
          return result;
      }
    }

    internal static Buffer template_to_buffer(Attribute[] t, bool read)
    {
      int size = 0;

      foreach (Attribute a in t)
      {
        int attrSize = read ? 0 : getAttributeSize(a.value);
        size += attrSize + sizeof(int)+IntPtr.Size+sizeof(int); //sizeof CK_ATTRIBUTE
      }
      Buffer buffer = new PKCS11.Buffer(size);

      IntPtr ptr = (IntPtr)(buffer.ptr.ToInt64() + t.Length * (sizeof(int) + IntPtr.Size + sizeof(int)));
      int offset = 0;
      foreach (Attribute a in t)
      {
        size = read ? 0 : getAttributeSize(a.value);

        IntPtr ptrToSave = ptr;
        if (size == 0) ptrToSave = IntPtr.Zero;
        else storeAttributeValue(ptr, a.value);

        Marshal.WriteInt32(buffer.ptr, offset, a.type); offset += sizeof(int);
        Marshal.WriteIntPtr(buffer.ptr, offset, ptrToSave); offset += IntPtr.Size;
        Marshal.WriteInt32(buffer.ptr, offset, size); offset += sizeof(int);

        ptr = (IntPtr)(ptr.ToInt64() + size);
      }
      return buffer;
    }      

    public partial struct Session
    {
      public Object[] FindObjects(Attribute[] t)
      {
        List<Object> list = new List<Object>();
        Buffer tBuf = template_to_buffer(t, false);

        int rv = PKCS11.fl.C_FindObjectsInit(id, tBuf.ptr, t.Length);
        if (rv==0)
        {
          while (true)
          {
            int count, objid;
            int rv2 = PKCS11.fl.C_FindObjects(id, out objid, 1, out count);
            if (rv2 != 0 || count < 1) break;
            list.Add(new Object(objid));
          }
          PKCS11.fl.C_FindObjectsFinal(id);
        }
 
        Exception.check(rv);
        return list.ToArray();
      }
    }
  }
}