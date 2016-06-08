﻿/*
    Copyright (C) 2014-2016 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.ComponentModel.Composition;
using System.Windows.Media;
using dnSpy.Contracts.Settings;
using dnSpy.Contracts.Text.Editor;
using dnSpy.Shared.Controls;
using dnSpy.Shared.MVVM;

namespace dnSpy.Text.Editor {
	class TextEditorSettings : ViewModelBase, ITextEditorSettings {
		protected virtual void OnModified() { }

		public FontFamily FontFamily {
			get { return fontFamily; }
			set {
				if (fontFamily.Source != value.Source) {
					fontFamily = value;
					OnPropertyChanged(nameof(FontFamily));
					OnModified();
				}
			}
		}
		FontFamily fontFamily = new FontFamily(FontUtils.GetDefaultMonospacedFont());

		public double FontSize {
			get { return fontSize; }
			set {
				if (fontSize != value) {
					fontSize = FontUtils.FilterFontSize(value);
					OnPropertyChanged(nameof(FontSize));
					OnModified();
				}
			}
		}
		double fontSize = FontUtils.DEFAULT_FONT_SIZE;

		public bool ShowLineNumbers {
			get { return showLineNumbers; }
			set {
				if (showLineNumbers != value) {
					showLineNumbers = value;
					OnPropertyChanged(nameof(ShowLineNumbers));
					OnModified();
				}
			}
		}
		bool showLineNumbers = true;

		public bool AutoHighlightRefs {
			get { return autoHighlightRefs; }
			set {
				if (autoHighlightRefs != value) {
					autoHighlightRefs = value;
					OnPropertyChanged(nameof(AutoHighlightRefs));
					OnModified();
				}
			}
		}
		bool autoHighlightRefs = true;

		public bool HighlightCurrentLine {
			get { return highlightCurrentLine; }
			set {
				if (highlightCurrentLine != value) {
					highlightCurrentLine = value;
					OnPropertyChanged(nameof(HighlightCurrentLine));
					OnModified();
				}
			}
		}
		bool highlightCurrentLine = true;

		public WordWrapStyles WordWrap {
			get { return wordWrap; }
			set {
				if (wordWrap != value) {
					wordWrap = value;
					OnPropertyChanged(nameof(WordWrap));
					OnModified();
				}
			}
		}
		WordWrapStyles wordWrap = WordWrapStyles.DefaultDisabled;

		public bool ConvertTabsToSpaces {
			get { return convertTabsToSpaces; }
			set {
				if (convertTabsToSpaces != value) {
					convertTabsToSpaces = value;
					OnPropertyChanged(nameof(ConvertTabsToSpaces));
					OnModified();
				}
			}
		}
		bool convertTabsToSpaces = false;
	}

	[Export, Export(typeof(ITextEditorSettings))]
	sealed class TextEditorSettingsImpl : TextEditorSettings {
		static readonly Guid SETTINGS_GUID = new Guid("9D40E1AD-5922-4BBA-B386-E6BABE5D185D");

		readonly ISettingsManager settingsManager;

		[ImportingConstructor]
		TextEditorSettingsImpl(ISettingsManager settingsManager) {
			this.settingsManager = settingsManager;

			this.disableSave = true;
			var sect = settingsManager.GetOrCreateSection(SETTINGS_GUID);
			this.FontFamily = new FontFamily(sect.Attribute<string>(nameof(FontFamily)) ?? FontUtils.GetDefaultMonospacedFont());
			this.FontSize = sect.Attribute<double?>(nameof(FontSize)) ?? this.FontSize;
			this.ShowLineNumbers = sect.Attribute<bool?>(nameof(ShowLineNumbers)) ?? this.ShowLineNumbers;
			this.AutoHighlightRefs = sect.Attribute<bool?>(nameof(AutoHighlightRefs)) ?? this.AutoHighlightRefs;
			this.HighlightCurrentLine = sect.Attribute<bool?>(nameof(HighlightCurrentLine)) ?? this.HighlightCurrentLine;
			this.WordWrap = sect.Attribute<WordWrapStyles?>(nameof(WordWrap)) ?? this.WordWrap;
			this.ConvertTabsToSpaces = sect.Attribute<bool?>(nameof(ConvertTabsToSpaces)) ?? this.ConvertTabsToSpaces;
			this.disableSave = false;
		}
		readonly bool disableSave;

		protected override void OnModified() {
			if (disableSave)
				return;
			var sect = settingsManager.RecreateSection(SETTINGS_GUID);
			sect.Attribute(nameof(FontFamily), FontFamily.Source);
			sect.Attribute(nameof(FontSize), FontSize);
			sect.Attribute(nameof(ShowLineNumbers), ShowLineNumbers);
			sect.Attribute(nameof(AutoHighlightRefs), AutoHighlightRefs);
			sect.Attribute(nameof(HighlightCurrentLine), HighlightCurrentLine);
			sect.Attribute(nameof(WordWrap), WordWrap);
			sect.Attribute(nameof(ConvertTabsToSpaces), ConvertTabsToSpaces);
		}
	}
}
