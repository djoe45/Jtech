﻿using System;
using Oxide.Core;
using Oxide.Plugins;
using System.Collections.Generic;

namespace Oxide.Plugins.JtechCore {

	[AttributeUsage(AttributeTargets.Class)]
	public class JInfoAttribute : Attribute {

		public InfoAttribute PluginInfo { get; }
		public string Name { get; }
		public string IconUrl { get; }
		public string Description { get; }

		/// <summary>
		/// Info about this Custom JDeployable
		/// </summary>
		/// <param name="pluginType">typeof(yourplugin)</param>
		/// <param name="name">Name shown in menus and commands.</param>
		/// <param name="iconUrl">Url for the icon shown in menus. Make it 200x200 with a transparent background.</param>
		public JInfoAttribute(Type pluginType, string name, string iconUrl, string description = null) {
			this.PluginInfo = (InfoAttribute) GetCustomAttribute(pluginType, typeof(InfoAttribute));
			this.Name = name;
			this.IconUrl = iconUrl;
			this.Description = description ?? string.Empty;
		}
	}
}