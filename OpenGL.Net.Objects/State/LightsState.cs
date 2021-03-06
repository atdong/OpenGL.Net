﻿
// Copyright (C) 2016 Luca Piccioni
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
using System.Collections.Generic;

namespace OpenGL.Objects.State
{
	/// <summary>
	/// OpenGL.Net light shading model.
	/// </summary>
	public class LightsState : ShaderUniformStateBase
	{
		#region Constructors

		/// <summary>
		/// Static constructor.
		/// </summary>
		static LightsState()
		{
			// Statically initialize uniform properties
			_UniformProperties = DetectUniformProperties(typeof(LightsState));
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public LightsState()
		{
			LightModel = new LightModelType(ColorRGBAF.ColorWhite);
		}

		#endregion

		#region Structures

		/// <summary>
		/// Light model global parameters.
		/// </summary>
		[ShaderUniformState()]
		public class LightModelType
		{
			/// <summary>
			/// Construct a LightModelType with the default ambient light.
			/// </summary>
			public LightModelType()
			{
				
			}

			/// <summary>
			/// Construct a LightModelType with a specific ambient light.
			/// </summary>
			/// <param name="ambientLighting"></param>
			public LightModelType(ColorRGBAF ambientLighting)
			{
				AmbientLighting = ambientLighting;
			}

			/// <summary>
			/// The ambient lighting.
			/// </summary>
			[ShaderUniformState("AmbientLighting")]
			public ColorRGBAF AmbientLighting = ColorRGBAF.ColorWhite;

			/// <summary>
			/// Apply this MaterialState
			/// </summary>
			/// <param name="ctx">
			/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
			/// </param>
			/// <param name="shaderProgram">
			/// The <see cref="ShaderProgram"/> which has the state set.
			/// </param>
			public void ApplyState(GraphicsContext ctx, ShaderProgram shaderProgram, string prefix)
			{
				shaderProgram.SetUniform(ctx, prefix + ".AmbientLighting", AmbientLighting);
			}
		}

		/// <summary>
		/// Light structure.
		/// </summary>
		[ShaderUniformState()]
		public abstract class Light
		{
			#region Model

			/// <summary>
			/// Light ambient color.
			/// </summary>
			[ShaderUniformState("AmbientColor")]
			public ColorRGBAF AmbientColor;

			/// <summary>
			/// Light diffuse color.
			/// </summary>
			[ShaderUniformState("DiffuseColor")]
			public ColorRGBAF DiffuseColor = ColorRGBAF.ColorWhite;

			/// <summary>
			/// Light specular color.
			/// </summary>
			[ShaderUniformState("SpecularColor")]
			public ColorRGBAF SpecularColor;

			#endregion

			#region Structure State Application

			/// <summary>
			/// Apply this Light
			/// </summary>
			/// <param name="ctx">
			/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
			/// </param>
			/// <param name="shaderProgram">
			/// The <see cref="ShaderProgram"/> which has the state set.
			/// </param>
			public virtual void ApplyState(GraphicsContext ctx, ShaderProgram shaderProgram, string prefix)
			{
				shaderProgram.SetUniform(ctx, prefix + ".AmbientColor", AmbientColor);
				shaderProgram.SetUniform(ctx, prefix + ".DiffuseColor", DiffuseColor);
				shaderProgram.SetUniform(ctx, prefix + ".SpecularColor", SpecularColor);
			}

			#endregion
		}

		[ShaderUniformState()]
		public class LightDirectional : Light
		{
			#region Model

			/// <summary>
			/// The light position vector (used by directional and spot lights).
			/// </summary>
			[ShaderUniformState("Direction")]
			public Vertex3f Direction;

			/// <summary>
			/// The light half-vector (used by directional lights).
			/// </summary>
			[ShaderUniformState("HalfVector")]
			public Vertex3f HalfVector;
			
			#endregion

			#region Light Overrides

			/// <summary>
			/// Apply this Light
			/// </summary>
			/// <param name="ctx">
			/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
			/// </param>
			/// <param name="shaderProgram">
			/// The <see cref="ShaderProgram"/> which has the state set.
			/// </param>
			public override void ApplyState(GraphicsContext ctx, ShaderProgram shaderProgram, string prefix)
			{
				// Base implementation
				base.ApplyState(ctx, shaderProgram, prefix);

				shaderProgram.SetUniform(ctx, prefix + ".Direction", Direction);
				shaderProgram.SetUniform(ctx, prefix + ".HalfVector", HalfVector);
			}

			#endregion
		}

		[ShaderUniformState()]
		public class LightPoint : Light
		{
			#region Model

			/// <summary>
			/// The light position vector (used by point and spot lights).
			/// </summary>
			[ShaderUniformState("Position")]
			public Vertex4f Position;

			/// <summary>
			/// The light attenuation factors (X: constant; Y: linear; Z: quadratic; used by point and spot lights).
			/// </summary>
			[ShaderUniformState("AttenuationFactors")]
			public Vertex3f AttenuationFactors;

			#endregion

			#region Light Overrides

			/// <summary>
			/// Apply this Light
			/// </summary>
			/// <param name="ctx">
			/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
			/// </param>
			/// <param name="shaderProgram">
			/// The <see cref="ShaderProgram"/> which has the state set.
			/// </param>
			public override void ApplyState(GraphicsContext ctx, ShaderProgram shaderProgram, string prefix)
			{
				// Base implementation
				base.ApplyState(ctx, shaderProgram, prefix);

				shaderProgram.SetUniform(ctx, prefix + ".Position", Position);
				shaderProgram.SetUniform(ctx, prefix + ".AttenuationFactors", AttenuationFactors);
				shaderProgram.SetUniform(ctx, prefix + ".FallOff.X", 180.0f);
			}

			#endregion
		}

		[ShaderUniformState()]
		public class LightSpot : LightPoint
		{
			#region Model

			/// <summary>
			/// The light position vector (used by directional and spot lights).
			/// </summary>
			[ShaderUniformState("Direction")]
			public Vertex3f Direction;

			/// <summary>
			/// The spot fall-off parameters (X: fall-off angle in degrees; Y: fall-off exponent, used by spot lights).
			/// </summary>
			[ShaderUniformState("FallOff")]
			public Vertex2f FallOff;

			#endregion

			#region LightPoint Overrides

			/// <summary>
			/// Apply this Light
			/// </summary>
			/// <param name="ctx">
			/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
			/// </param>
			/// <param name="shaderProgram">
			/// The <see cref="ShaderProgram"/> which has the state set.
			/// </param>
			public override void ApplyState(GraphicsContext ctx, ShaderProgram shaderProgram, string prefix)
			{
				// Base implementation
				base.ApplyState(ctx, shaderProgram, prefix);

				shaderProgram.SetUniform(ctx, prefix + ".Direction", Direction);
				shaderProgram.SetUniform(ctx, prefix + ".FallOff", FallOff);
			}

			#endregion
		}

		#endregion

		#region Lights State

		/// <summary>
		/// Get or set the actual light model parameters.
		/// </summary>
		[ShaderUniformState()]
		public LightModelType LightModel
		{
			get { return (_LightModel); }
			set
			{
				_LightModel = value;
			}
		}

		/// <summary>
		/// The actual light model parameters.
		/// </summary>
		private LightModelType _LightModel;

		/// <summary>
		/// Lights
		/// </summary>
		public readonly List<Light> Lights = new List<Light>();

		/// <summary>
		/// Lights state array.
		/// </summary>
		[ShaderUniformState("glo_Light")]
		private Light[] LightsInternal { get { return (Lights.ToArray()); } }

		/// <summary>
		/// The enabled lights count.
		/// </summary>
		[ShaderUniformState()]
		private int LightsCount { get { return (Lights.Count); } }

		#endregion

		#region ShaderUniformState Overrides

		/// <summary>
		/// The identifier for the TransformStateBase derived classes.
		/// </summary>
		public static string StateId = "OpenGL.LightsState";

		/// <summary>
		/// The identifier of this GraphicsState.
		/// </summary>
		public override string StateIdentifier { get { return (StateId); } }

		/// <summary>
		/// Unique index assigned to this GraphicsState.
		/// </summary>
		public static int StateSetIndex { get { return (_StateIndex); } }

		/// <summary>
		/// Unique index assigned to this GraphicsState.
		/// </summary>
		public override int StateIndex { get { return (_StateIndex); } }

		/// <summary>
		/// The index for this GraphicsState.
		/// </summary>
		private static int _StateIndex = NextStateIndex();

		/// <summary>
		/// Flag indicating whether the state is context-bound.
		/// </summary>
		/// <remarks>
		/// It returns always true, since it supports also fixed pipeline.
		/// </remarks>
		public override bool IsContextBound { get { return (true); } }

		/// <summary>
		/// Flag indicating whether the state can be applied on a <see cref="ShaderProgram"/>.
		/// </summary>
		public override bool IsProgramBound { get { return (true); } }

		/// <summary>
		/// The tag the identifies the uniform block.
		/// </summary>
		protected override string UniformBlockTag { get { return ("LightState"); } }

		/// <summary>
		/// Apply this TransformStateBase.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> which has defined the shader program <paramref name="shaderProgram"/>.
		/// </param>
		/// <param name="shaderProgram">
		/// The <see cref="ShaderProgram"/> which has the state set.
		/// </param>
		public override void Apply(GraphicsContext ctx, ShaderProgram shaderProgram)
		{
			GraphicsResource.CheckCurrentContext(ctx);

			if (!ctx.Extensions.UniformBufferObject_ARB) {
				if (shaderProgram == null) {
					// Fixed pipeline rendering requires server state
					throw new NotImplementedException();
				} else {
					// Custom implementation
					ctx.Bind(shaderProgram);

					if (shaderProgram.IsActiveUniform("glo_LightModel"))
						LightModel.ApplyState(ctx, shaderProgram, "glo_LightModel");

					for (int i = 0; i < Lights.Count; i++) {
						string uniformName = "glo_Light[" + i + "]";

						if (shaderProgram.IsActiveUniform(uniformName) == false)
							break;

						Lights[i].ApplyState(ctx, shaderProgram, uniformName);
					}

					shaderProgram.SetUniform(ctx, "glo_LightsCount", LightsCount);
				}
			} else
				base.Apply(ctx, shaderProgram);		// Uniform block
		}

		/// <summary>
		/// Performs a deep copy of this <see cref="IGraphicsState"/>.
		/// </summary>
		/// <returns>
		/// It returns the equivalent of this <see cref="IGraphicsState"/>, but all objects referenced
		/// are not referred by both instances.
		/// </returns>
		public override IGraphicsState Push()
		{
			return (this);
		}

		/// <summary>
		/// Merge this state with another one.
		/// </summary>
		/// <param name="state">
		/// A <see cref="IGraphicsState"/> having the same <see cref="StateIdentifier"/> of this state.
		/// </param>
		/// <remarks>
		public override void Merge(IGraphicsState state)
		{
			if (state == null)
				throw new ArgumentNullException("state");

			throw new NotImplementedException();
		}

		/// <summary>
		/// Get the uniform state associated with this instance.
		/// </summary>
		protected override Dictionary<string, UniformStateMember> UniformState { get { return (_UniformProperties); } }

		/// <summary>
		/// Represents the current <see cref="GraphicsState"/> for logging.
		/// </summary>
		/// <returns>
		/// A <see cref="String"/> that represents the current <see cref="GraphicsState"/>.
		/// </returns>
		public override string ToString()
		{
			return (String.Empty);
		}

		/// <summary>
		/// The uniform state of this TransformStateBase.
		/// </summary>
		private static readonly Dictionary<string, UniformStateMember> _UniformProperties;

		#endregion
	}
}
