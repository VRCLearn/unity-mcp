using System;
using System.Collections.Generic;
using MCPForUnity.Editor.Constants;
using MCPForUnity.Editor.Helpers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MCPForUnity.Editor.Windows.Components.Validation
{
    /// <summary>
    /// Controller for the Script Validation section.
    /// Handles script validation level settings.
    /// </summary>
    public class McpValidationSection
    {
        // UI Elements
        private DropdownField validationLevelField;
        private Label validationDescription;

        // Data
        private ValidationLevel currentValidationLevel = ValidationLevel.Standard;

        // Validation levels
        public enum ValidationLevel
        {
            Basic,
            Standard,
            Comprehensive,
            Strict
        }

        public VisualElement Root { get; private set; }

        public McpValidationSection(VisualElement root)
        {
            Root = root;
            CacheUIElements();
            InitializeUI();
            RegisterCallbacks();
        }

        private void CacheUIElements()
        {
            validationLevelField = Root.Q<DropdownField>("validation-level");
            validationDescription = Root.Q<Label>("validation-description");
        }

        private void InitializeUI()
        {
            int savedLevel = EditorPrefs.GetInt(EditorPrefKeys.ValidationLevel, 1);
            currentValidationLevel = (ValidationLevel)Mathf.Clamp(savedLevel, 0, 3);
            validationLevelField.choices = new List<string>
            {
                EditorLocalization.Text("Basic"),
                EditorLocalization.Text("Standard"),
                EditorLocalization.Text("Comprehensive"),
                EditorLocalization.Text("Strict"),
            };
            validationLevelField.index = (int)currentValidationLevel;
            UpdateValidationDescription();
        }

        private void RegisterCallbacks()
        {
            validationLevelField.RegisterValueChangedCallback(evt =>
            {
                currentValidationLevel = (ValidationLevel)validationLevelField.index;
                EditorPrefs.SetInt(EditorPrefKeys.ValidationLevel, (int)currentValidationLevel);
                UpdateValidationDescription();
            });
        }

        private void UpdateValidationDescription()
        {
            validationDescription.text = EditorLocalization.Text(currentValidationLevel switch
            {
                ValidationLevel.Basic => "Basic: Validates syntax only. Fast compilation checks.",
                ValidationLevel.Standard => "Standard (Recommended): Checks syntax + common errors. Balanced speed and coverage.",
                ValidationLevel.Comprehensive => "Comprehensive: Detailed validation including code quality. Slower but thorough.",
                ValidationLevel.Strict => "Strict: Maximum validation + warnings as errors. Slowest but catches all issues.",
                _ => "Unknown validation level"
            });
        }
    }
}
