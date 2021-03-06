﻿
// Copyright (C) 2017 Luca Piccioni
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

// Symbol for disabling redundant texture unit object binding
#define ENABLE_LAZY_TEXTURE_UNIT_BINDING

using System;
using System.Diagnostics;

namespace OpenGL.Objects
{
	/// <summary>
	/// Texture unit abstraction.
	/// </summary>
	class TextureUnit
	{
		#region Constructors

		/// <summary>
		/// Construct a TextureUnit specifying its index.
		/// </summary>
		/// <param name="index"></param>
		public TextureUnit(uint index)
		{
			Index = index;
		}

		#endregion

		#region Unit Index

		/// <summary>
		/// Set this TextureUnit as the current one.
		/// </summary>
		/// <param name="ctx"></param>
		public void Activate(GraphicsContext ctx)
		{
			ctx.ActiveTexture(Index);
		}

		/// <summary>
		/// The index of the TextureUnit.
		/// </summary>
		public readonly uint Index;

		#endregion

		#region Texture

		/// <summary>
		/// Bind a <see cref="Texture"/> to this TextureUnit.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> managing this TextureUnit.
		/// </param>
		/// <param name="texture">
		/// The <see cref="Texture"/> to be bound to this TextureUnit.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="texture"/> is null.
		/// </exception>
		public void Bind(GraphicsContext ctx, Texture texture)
		{
			GraphicsResource.CheckCurrentContext(ctx);
			if (texture == null)
				throw new ArgumentNullException("texture");

			// Required to be active
			Activate(ctx);

			Texture currTexture = Texture;

			// Bind the texture on the texture unit, if necessary
#if ENABLE_LAZY_TEXTURE_UNIT_BINDING
			if (ReferenceEquals(texture, currTexture) == false) {
#else
			if (true) {
#endif
				// Since here, the texture is bound to the texture unit
				Gl.BindTexture(texture.TextureTarget, texture.ObjectName);

				// Break previous texture relationship, if any
				if (currTexture != null)
					currTexture._ActiveTextureUnits.Remove(Index);

				// Add texture relationship
				_Texture = new WeakReference<Texture>(texture);

				Debug.Assert(texture._ActiveTextureUnits.Contains(Index) == false);
				texture._ActiveTextureUnits.Add(Index);
			} else {
				// Texture already bound to texture unit, Gl.BindTexture not necessary
#if DEBUG
				int textureBindingTarget = texture.GetBindingTarget(ctx);
				int currentTextureObject;
		
				Debug.Assert(textureBindingTarget != 0);
				Gl.Get(textureBindingTarget, out currentTextureObject);

				Debug.Assert(currentTextureObject == texture.ObjectName);
#endif
			}
		}

		/// <summary>
		/// Texture currently bound on texture unit.
		/// </summary>
		public Texture Texture
		{
			get
			{
				Texture currTexture = null;

				if (_Texture != null)
					_Texture.TryGetTarget(out currTexture);

				return (currTexture);
			}
		}

		/// <summary>
		/// Texture currently bound on texture unit.
		/// </summary>
		private WeakReference<Texture> _Texture;

		#endregion

		#region Sampler

		/// <summary>
		/// Bind a <see cref="Sampler"/> to this TextureUnit.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> managing this TextureUnit.
		/// </param>
		/// <param name="sampler">
		/// The <see cref="Sampler"/> to be bound to this TextureUnit. It can be null to break any previous
		/// binding.
		/// </param>
		public void Bind(GraphicsContext ctx, Sampler sampler)
		{
			GraphicsResource.CheckCurrentContext(ctx);

			// Required to be active
			Activate(ctx);

			Sampler currSampler = Sampler;

			// Bind the sampler on the texture unit, if necessary
#if ENABLE_LAZY_TEXTURE_UNIT_BINDING
			if (ReferenceEquals(sampler, currSampler) == false) {
#else
			if (true) {
#endif
				if (sampler != null) {
					// Since here, the texture is bound to the texture unit
					// Note: only when ARB_sampler_objects is implemented
					sampler.Bind(ctx, this);
					// Add sampler relationship
					_Sampler = new WeakReference<Sampler>(sampler);
				} else {
					// Ensure no sampler on texture unit
					Sampler.Unbind(ctx, this);
					// Break sampler relationship
					_Sampler = null;
				}
			} else {
#if DEBUG

#endif
			}
		}

		/// <summary>
		/// Sampler currently bound on texture unit.
		/// </summary>
		public Sampler Sampler
		{
			get
			{
				Sampler currSampler = null;

				if (_Sampler != null)
					_Sampler.TryGetTarget(out currSampler);

				return (currSampler);
			}
		}

		/// <summary>
		/// Texture currently bound on texture unit.
		/// </summary>
		/// <remarks>
		/// From specification: "If a sampler object that is currently bound to one or more texture units is deleted, it
		/// is as though BindSampler is called once for each texture unit to which the sampler is bound, with unit set to the texture
		/// unit and sampler set to zero."
		/// </remarks>
		private WeakReference<Sampler> _Sampler;

		#endregion

		#region Sampler Parameters

		/// <summary>
		/// Update sampler parameters, accorndly to <see cref="Texture"/> and <see cref="Sampler"/> properties.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> managing this TextureUnit.
		/// </param>
		public void SamplerParameters(GraphicsContext ctx)
		{
			GraphicsResource.CheckCurrentContext(ctx);

			Texture texture = Texture;

			if (texture == null)
				throw new InvalidOperationException("no texture bound");

			SamplerParameters samplerParams = texture.SamplerParams;
			Sampler sampler = Sampler;

			// Sampler parameters always overrides texture unit parameters
			if (sampler != null) {
				if (ctx.Extensions.SamplerObjects_ARB) {
					// Ensure sampler parameters in sync
					sampler.Bind(ctx, this);
					// Fast path? No need to update texture unit parameters
					return;		
				} else {
					// Note: even if ARB_sampler_objects is not supported, a Sampler overrides
					samplerParams = sampler.Parameters;
				}
			}

			// Set texture unit parameters
			TexParameters(texture.TextureTarget, samplerParams);
		}

		/// <summary>
		/// Apply texture unit sampler parameteres, using compatibility methods.
		/// </summary>
		/// <param name="textureTarget">
		/// The <see cref="TextureTarget"/> that specifies the texture target to apply sampler.
		/// </param>
		/// <param name="texParameters">
		/// The <see cref="SamplerParameters"/> that specifies the parameters to be applied.
		/// </param>
		private void TexParameters(TextureTarget textureTarget, SamplerParameters texParameters)
		{
			if (texParameters.MinFilter != _CurrentSamplerParams.MinFilter) {
				Gl.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)texParameters.MinFilter);
				_CurrentSamplerParams.MinFilter = texParameters.MinFilter;
			}
			
			if (texParameters.MagFilter != _CurrentSamplerParams.MagFilter) {
				Gl.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)texParameters.MagFilter);
				_CurrentSamplerParams.MagFilter = texParameters.MagFilter;
			}
			
			if (texParameters.WrapCoordR != _CurrentSamplerParams.WrapCoordR) {
				Gl.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)texParameters.WrapCoordR);
				_CurrentSamplerParams.WrapCoordR = texParameters.WrapCoordR;
			}
				
			if (texParameters.WrapCoordS != _CurrentSamplerParams.WrapCoordS) {
				Gl.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)texParameters.WrapCoordS);
				_CurrentSamplerParams.WrapCoordS = texParameters.WrapCoordS;
			}
				
			if (texParameters.WrapCoordT != _CurrentSamplerParams.WrapCoordT) {
				Gl.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)texParameters.WrapCoordT);
				_CurrentSamplerParams.WrapCoordT = texParameters.WrapCoordT;
			}
		}

		/// <summary>
		/// Texture unit sampler parameters.
		/// </summary>
		private readonly SamplerParameters _CurrentSamplerParams = new SamplerParameters();

		#endregion
	}
}
