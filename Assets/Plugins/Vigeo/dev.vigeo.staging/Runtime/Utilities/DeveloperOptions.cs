using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo {
    
    [Serializable, InlineProperty]
    public struct DeveloperOptions {
        
        [Flags]
        public enum Features {
            None = 0,
            Warning = 1 << 0,
            Notes = 1 << 1,
            NotionDocumentation = 1 << 2
        }

        public string Title => $"Developer Options [{activeFeatures}]";

        [LabelText("Active"), LabelWidth(40)]
        public Features activeFeatures;

        [TitleGroup("Developer Warning", GroupID = "Warning")]
        [LabelText("", SdfIconType.ExclamationTriangleFill), LabelWidth(15)]
        [Tooltip("Contents of this field will be shown as a warning at the top of the inspector.")]
        [ShowIf("ShouldShowWarningField")]
        public string warning;
        
        internal bool ShouldDisplayWarning => warning.IsNullOrWhitespace() == false;
        
        internal bool ShouldShowWarningField => activeFeatures.HasFlag(Features.Warning);
        
        [TitleGroup("Developer Notes", GroupID = "Notes")]
        [TextArea(2, 10), HideLabel]
        [ShowIf("ShouldShowNotesField")]
        public string notes;
        
        internal bool ShouldShowNotesField => activeFeatures.HasFlag(Features.Notes);
        
        internal bool IsEditingNotionURL { get; set; }
        
        [TitleGroup("Notion Documentation", GroupID = "Notion"), HorizontalGroup("Notion/URL")]
        [LabelText("URL"), LabelWidth(25)]
        [ShowIf("ShouldShowNotionDocumentationField"), EnableIf("IsEditingNotionURL")]
        public string notionUrl;
        
        internal bool ShouldShowNotionDocumentationField => activeFeatures.HasFlag(Features.NotionDocumentation);

        [TitleGroup("", GroupID = "Notion"), HorizontalGroup("Notion/URL", Width = 45)]
        [Button("Edit"), PropertySpace(2)]
        [ShowIf("ShouldShowEditNotionUrlButton")]
        private void StartEditingNotionURL() {
            IsEditingNotionURL = true;
        }

        internal bool ShouldShowEditNotionUrlButton => ShouldShowNotionDocumentationField && IsEditingNotionURL == false;
        
        [TitleGroup("Notion Documentation", GroupID = "Notion"), HorizontalGroup("Notion/URL", Width = 45)]
        [Button("Save"), PropertySpace(2)]
        [ShowIf("ShouldShowSaveNotionUrlButton")]
        private void StopEditingNotionURL() {
            IsEditingNotionURL = false;
        }
        
        internal bool ShouldShowSaveNotionUrlButton => ShouldShowNotionDocumentationField && IsEditingNotionURL;
        
        [TitleGroup("Notion Documentation", GroupID = "Notion"), HorizontalGroup("Notion/URL", Width = 20)]
        [Button("Open in Notion", Icon = SdfIconType.ArrowUpRightSquare), PropertySpace(2)]
        [ShowIf("ShouldShowNotionDocumentationField")]
        private void OpenNotionURL() =>
            notionUrl.WithNotEmpty(Application.OpenURL);
    }
}
