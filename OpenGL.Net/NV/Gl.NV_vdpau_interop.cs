
// Copyright (C) 2015 Luca Piccioni
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
// USA

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenGL
{
	public partial class Gl
	{
		/// <summary>
		/// Value of GL_SURFACE_STATE_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_vdpau_interop")]
		public const int SURFACE_STATE_NV = 0x86EB;

		/// <summary>
		/// Value of GL_SURFACE_REGISTERED_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_vdpau_interop")]
		public const int SURFACE_REGISTERED_NV = 0x86FD;

		/// <summary>
		/// Value of GL_SURFACE_MAPPED_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_vdpau_interop")]
		public const int SURFACE_MAPPED_NV = 0x8700;

		/// <summary>
		/// Value of GL_WRITE_DISCARD_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_vdpau_interop")]
		public const int WRITE_DISCARD_NV = 0x88BE;

		/// <summary>
		/// Binding for glVDPAUInitNV.
		/// </summary>
		/// <param name="vdpDevice">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="getProcAddress">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		public static void VDPAUInitNV(IntPtr vdpDevice, IntPtr getProcAddress)
		{
			Debug.Assert(Delegates.pglVDPAUInitNV != null, "pglVDPAUInitNV not implemented");
			Delegates.pglVDPAUInitNV(vdpDevice, getProcAddress);
			CallLog("glVDPAUInitNV({0}, {1})", vdpDevice, getProcAddress);
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAUInitNV.
		/// </summary>
		/// <param name="vdpDevice">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="getProcAddress">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		public static void VDPAUInitNV(Object vdpDevice, Object getProcAddress)
		{
			GCHandle pin_vdpDevice = GCHandle.Alloc(vdpDevice, GCHandleType.Pinned);
			GCHandle pin_getProcAddress = GCHandle.Alloc(getProcAddress, GCHandleType.Pinned);
			try {
				VDPAUInitNV(pin_vdpDevice.AddrOfPinnedObject(), pin_getProcAddress.AddrOfPinnedObject());
			} finally {
				pin_vdpDevice.Free();
				pin_getProcAddress.Free();
			}
		}

		/// <summary>
		/// Binding for glVDPAUFiniNV.
		/// </summary>
		public static void VDPAUNV()
		{
			Debug.Assert(Delegates.pglVDPAUFiniNV != null, "pglVDPAUFiniNV not implemented");
			Delegates.pglVDPAUFiniNV();
			CallLog("glVDPAUFiniNV()");
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAURegisterVideoSurfaceNV.
		/// </summary>
		/// <param name="vdpSurface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="target">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="numTextureNames">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="textureNames">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		public static IntPtr VDPAURegisterVideoSurfaceNV(IntPtr vdpSurface, int target, Int32 numTextureNames, UInt32[] textureNames)
		{
			IntPtr retValue;

			unsafe {
				fixed (UInt32* p_textureNames = textureNames)
				{
					Debug.Assert(Delegates.pglVDPAURegisterVideoSurfaceNV != null, "pglVDPAURegisterVideoSurfaceNV not implemented");
					retValue = Delegates.pglVDPAURegisterVideoSurfaceNV(vdpSurface, target, numTextureNames, p_textureNames);
					CallLog("glVDPAURegisterVideoSurfaceNV({0}, {1}, {2}, {3}) = {4}", vdpSurface, target, numTextureNames, textureNames, retValue);
				}
			}
			DebugCheckErrors();

			return (retValue);
		}

		/// <summary>
		/// Binding for glVDPAURegisterVideoSurfaceNV.
		/// </summary>
		/// <param name="vdpSurface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="target">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="numTextureNames">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="textureNames">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		public static IntPtr VDPAURegisterVideoSurfaceNV(Object vdpSurface, int target, Int32 numTextureNames, UInt32[] textureNames)
		{
			GCHandle pin_vdpSurface = GCHandle.Alloc(vdpSurface, GCHandleType.Pinned);
			try {
				return (VDPAURegisterVideoSurfaceNV(pin_vdpSurface.AddrOfPinnedObject(), target, numTextureNames, textureNames));
			} finally {
				pin_vdpSurface.Free();
			}
		}

		/// <summary>
		/// Binding for glVDPAURegisterOutputSurfaceNV.
		/// </summary>
		/// <param name="vdpSurface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="target">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="numTextureNames">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="textureNames">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		public static IntPtr VDPAURegisterOutputSurfaceNV(IntPtr vdpSurface, int target, Int32 numTextureNames, UInt32[] textureNames)
		{
			IntPtr retValue;

			unsafe {
				fixed (UInt32* p_textureNames = textureNames)
				{
					Debug.Assert(Delegates.pglVDPAURegisterOutputSurfaceNV != null, "pglVDPAURegisterOutputSurfaceNV not implemented");
					retValue = Delegates.pglVDPAURegisterOutputSurfaceNV(vdpSurface, target, numTextureNames, p_textureNames);
					CallLog("glVDPAURegisterOutputSurfaceNV({0}, {1}, {2}, {3}) = {4}", vdpSurface, target, numTextureNames, textureNames, retValue);
				}
			}
			DebugCheckErrors();

			return (retValue);
		}

		/// <summary>
		/// Binding for glVDPAURegisterOutputSurfaceNV.
		/// </summary>
		/// <param name="vdpSurface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="target">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="numTextureNames">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="textureNames">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		public static IntPtr VDPAURegisterOutputSurfaceNV(Object vdpSurface, int target, Int32 numTextureNames, UInt32[] textureNames)
		{
			GCHandle pin_vdpSurface = GCHandle.Alloc(vdpSurface, GCHandleType.Pinned);
			try {
				return (VDPAURegisterOutputSurfaceNV(pin_vdpSurface.AddrOfPinnedObject(), target, numTextureNames, textureNames));
			} finally {
				pin_vdpSurface.Free();
			}
		}

		/// <summary>
		/// Binding for glVDPAUIsSurfaceNV.
		/// </summary>
		/// <param name="surface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		public static bool VDPAUIsSurfaceNV(IntPtr surface)
		{
			bool retValue;

			Debug.Assert(Delegates.pglVDPAUIsSurfaceNV != null, "pglVDPAUIsSurfaceNV not implemented");
			retValue = Delegates.pglVDPAUIsSurfaceNV(surface);
			CallLog("glVDPAUIsSurfaceNV({0}) = {1}", surface, retValue);
			DebugCheckErrors();

			return (retValue);
		}

		/// <summary>
		/// Binding for glVDPAUUnregisterSurfaceNV.
		/// </summary>
		/// <param name="surface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		public static void VDPAUUnregisterSurfaceNV(IntPtr surface)
		{
			Debug.Assert(Delegates.pglVDPAUUnregisterSurfaceNV != null, "pglVDPAUUnregisterSurfaceNV not implemented");
			Delegates.pglVDPAUUnregisterSurfaceNV(surface);
			CallLog("glVDPAUUnregisterSurfaceNV({0})", surface);
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAUGetSurfaceivNV.
		/// </summary>
		/// <param name="surface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="pname">
		/// A <see cref="T:int"/>.
		/// </param>
		/// <param name="bufSize">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="length">
		/// A <see cref="T:Int32[]"/>.
		/// </param>
		/// <param name="values">
		/// A <see cref="T:Int32[]"/>.
		/// </param>
		public static void VDPAUGetSurfaceNV(IntPtr surface, int pname, Int32 bufSize, Int32[] length, Int32[] values)
		{
			unsafe {
				fixed (Int32* p_length = length)
				fixed (Int32* p_values = values)
				{
					Debug.Assert(Delegates.pglVDPAUGetSurfaceivNV != null, "pglVDPAUGetSurfaceivNV not implemented");
					Delegates.pglVDPAUGetSurfaceivNV(surface, pname, bufSize, p_length, p_values);
					CallLog("glVDPAUGetSurfaceivNV({0}, {1}, {2}, {3}, {4})", surface, pname, bufSize, length, values);
				}
			}
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAUSurfaceAccessNV.
		/// </summary>
		/// <param name="surface">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="access">
		/// A <see cref="T:int"/>.
		/// </param>
		public static void VDPAUSurfaceNV(IntPtr surface, int access)
		{
			Debug.Assert(Delegates.pglVDPAUSurfaceAccessNV != null, "pglVDPAUSurfaceAccessNV not implemented");
			Delegates.pglVDPAUSurfaceAccessNV(surface, access);
			CallLog("glVDPAUSurfaceAccessNV({0}, {1})", surface, access);
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAUMapSurfacesNV.
		/// </summary>
		/// <param name="numSurfaces">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="surfaces">
		/// A <see cref="T:IntPtr[]"/>.
		/// </param>
		public static void VDPAUMapSurfaceNV(Int32 numSurfaces, IntPtr[] surfaces)
		{
			unsafe {
				fixed (IntPtr* p_surfaces = surfaces)
				{
					Debug.Assert(Delegates.pglVDPAUMapSurfacesNV != null, "pglVDPAUMapSurfacesNV not implemented");
					Delegates.pglVDPAUMapSurfacesNV(numSurfaces, p_surfaces);
					CallLog("glVDPAUMapSurfacesNV({0}, {1})", numSurfaces, surfaces);
				}
			}
			DebugCheckErrors();
		}

		/// <summary>
		/// Binding for glVDPAUUnmapSurfacesNV.
		/// </summary>
		/// <param name="numSurface">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="surfaces">
		/// A <see cref="T:IntPtr[]"/>.
		/// </param>
		public static void VDPAUUnmapSurfaceNV(Int32 numSurface, IntPtr[] surfaces)
		{
			unsafe {
				fixed (IntPtr* p_surfaces = surfaces)
				{
					Debug.Assert(Delegates.pglVDPAUUnmapSurfacesNV != null, "pglVDPAUUnmapSurfacesNV not implemented");
					Delegates.pglVDPAUUnmapSurfacesNV(numSurface, p_surfaces);
					CallLog("glVDPAUUnmapSurfacesNV({0}, {1})", numSurface, surfaces);
				}
			}
			DebugCheckErrors();
		}

	}

}