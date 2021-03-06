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

using System;

namespace OpenGL.Objects
{
	/// <summary>
	/// Texture sampler object.
	/// </summary>
	public class Sampler : GraphicsResource
	{
		#region Constructors

		/// <summary>
		/// Construct the default Sampler.
		/// </summary>
		public Sampler()
		{

		}

		#endregion

		#region Parameters

		/// <summary>
		/// Sampler parameters.
		/// </summary>
		public readonly SamplerParameters Parameters = new SamplerParameters();

		/// <summary>
		/// Currently applied sampler parameters.
		/// </summary>
		private readonly SamplerParameters _ObjectParams = new SamplerParameters();

		#endregion

		#region Texture Unit Parameters

		internal void Bind(GraphicsContext ctx, TextureUnit textureUnit)
		{
			if (ctx.Extensions.SamplerObjects_ARB) {
				// Associate sampler to texture unit
				Gl.BindSampler(textureUnit.Index, ObjectName);
				// Update sampler parameters
				TexParameters(Parameters);
			}
		}

		internal static void Unbind(GraphicsContext ctx, TextureUnit textureUnit)
		{
			if (ctx.Extensions.SamplerObjects_ARB) {
				// Associate sampler to texture unit
				Gl.BindSampler(textureUnit.Index, InvalidObjectName);
			}
		}

		private void TexParameters(SamplerParameters samplerParams)
		{
			if (samplerParams.MinFilter != _ObjectParams.MinFilter) {
				Gl.SamplerParameter(ObjectName, (int)TextureParameterName.TextureMinFilter, (int)samplerParams.MinFilter);
				_ObjectParams.MinFilter = samplerParams.MinFilter;
			}
			
			if (samplerParams.MagFilter != _ObjectParams.MagFilter) {
				Gl.SamplerParameter(ObjectName, (int)TextureParameterName.TextureMagFilter, (int)samplerParams.MagFilter);
				_ObjectParams.MagFilter = samplerParams.MagFilter;
			}
			
			if (samplerParams.WrapCoordR != _ObjectParams.WrapCoordR) {
				Gl.SamplerParameter(ObjectName, (int)TextureParameterName.TextureWrapR, (int)samplerParams.WrapCoordR);
				_ObjectParams.WrapCoordR = samplerParams.WrapCoordR;
			}
			
			if (samplerParams.WrapCoordS != _ObjectParams.WrapCoordS) {
				Gl.SamplerParameter(ObjectName, (int)TextureParameterName.TextureWrapS, (int)samplerParams.WrapCoordS);
				_ObjectParams.WrapCoordS = samplerParams.WrapCoordS;
			}
			
			if (samplerParams.WrapCoordT != _ObjectParams.WrapCoordT) {
				Gl.SamplerParameter(ObjectName, (int)TextureParameterName.TextureWrapT, (int)samplerParams.WrapCoordT);
				_ObjectParams.WrapCoordT = samplerParams.WrapCoordT;
			}
		}

		#endregion

		#region GraphicsResource Overrides

		/// <summary>
		/// Texture object class.
		/// </summary>
		internal static readonly Guid ThisObjectClass = new Guid("8ACF187E-4FF7-4330-A2AA-52362589B07E");

		/// <summary>
		/// Texture object class.
		/// </summary>
		public override Guid ObjectClass { get { return (ThisObjectClass); } }

		/// <summary>
		/// Determine whether this Texture really exists for a specific context.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> that would have created (or a sharing one) the object. This context shall be current to
		/// the calling thread.
		/// </param>
		/// <returns>
		/// It returns a boolean value indicating whether this Texture exists in the object space of <paramref name="ctx"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The object existence is done by checking a valid object by its name <see cref="IGraphicsResource.ObjectName"/>. This routine will test whether
		/// <paramref name="ctx"/> has created this Texture (or is sharing with the creator).
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="ctx"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="ctx"/> is not current to the calling thread.
		/// </exception>
		public override bool Exists(GraphicsContext ctx)
		{
			// Object name space test (and 'ctx' sanity checks)
			if (base.Exists(ctx) == false)
				return (false);

			return (!ctx.Extensions.SamplerObjects_ARB || Gl.IsSampler(ObjectName));
		}

		/// <summary>
		/// Determine whether this object requires a name bound to a context or not.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> used for creating this object name.
		/// </param>
		/// <returns>
		/// .
		/// </returns>
		protected override bool RequiresName(GraphicsContext ctx)
		{
			return (ctx.Extensions.SamplerObjects_ARB);
		}

		/// <summary>
		/// Create a Texture name.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> used for creating this object name.
		/// </param>
		/// <returns>
		/// It returns a valid object name for this Texture.
		/// </returns>
		protected override uint CreateName(GraphicsContext ctx)
		{
			// Generate texture name
			return (Gl.GenSampler());
		}

		/// <summary>
		/// Delete a Texture name.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> used for deleting this object name.
		/// </param>
		/// <param name="name">
		/// A <see cref="UInt32"/> that specify the object name to delete.
		/// </param>
		protected override void DeleteName(GraphicsContext ctx, uint name)
		{
			// Delete texture name
			Gl.DeleteSamplers(name);
		}

		/// <summary>
		/// Actually create this GraphicsResource resources.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> used for allocating resources.
		/// </param>
		protected override void CreateObject(GraphicsContext ctx)
		{
			CheckCurrentContext(ctx);
		}

		#endregion
	}
}
