using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using MCPForUnity.Editor.Constants;
using MCPForUnity.Editor.Helpers;
using NUnit.Framework;
using UnityEditor;

namespace MCPForUnityTests.Editor.Helpers
{
    [TestFixture]
    public class EditorLocalizationTests
    {
        private EditorLanguage originalLanguage;
        private bool hadLanguage;
        private int originalLanguagePreference;
        private bool hadAllowLanBind;
        private bool originalAllowLanBind;

        [SetUp]
        public void SetUp()
        {
            hadLanguage = EditorPrefs.HasKey(EditorPrefKeys.EditorLanguage);
            originalLanguagePreference = EditorPrefs.GetInt(EditorPrefKeys.EditorLanguage);
            originalLanguage = EditorLocalization.CurrentLanguage;
            hadAllowLanBind = EditorPrefs.HasKey(EditorPrefKeys.AllowLanHttpBind);
            originalAllowLanBind = EditorPrefs.GetBool(EditorPrefKeys.AllowLanHttpBind, false);
        }

        [TearDown]
        public void TearDown()
        {
            EditorLocalization.SetLanguage(originalLanguage);
            if (hadLanguage)
            {
                EditorPrefs.SetInt(EditorPrefKeys.EditorLanguage, originalLanguagePreference);
            }
            else
            {
                EditorPrefs.DeleteKey(EditorPrefKeys.EditorLanguage);
            }
            if (hadAllowLanBind)
            {
                EditorPrefs.SetBool(EditorPrefKeys.AllowLanHttpBind, originalAllowLanBind);
            }
            else
            {
                EditorPrefs.DeleteKey(EditorPrefKeys.AllowLanHttpBind);
            }
        }

        [TestCase(0, "Tools")]
        [TestCase(1, "ツール")]
        [TestCase(2, "工具")]
        [TestCase(3, "工具")]
        [TestCase(4, "도구")]
        public void Text_ReturnsExpectedTranslation(int languageValue, string expected)
        {
            EditorLocalization.SetLanguage((EditorLanguage)languageValue);

            Assert.That(EditorLocalization.Text("Tools"), Is.EqualTo(expected));
        }

        [TestCase(SystemLanguage.English, 0)]
        [TestCase(SystemLanguage.Japanese, 1)]
        [TestCase(SystemLanguage.Korean, 4)]
        [TestCase(SystemLanguage.ChineseTraditional, 2)]
        [TestCase(SystemLanguage.ChineseSimplified, 3)]
        [TestCase(SystemLanguage.French, 0)]
        public void SystemLanguage_UsesSupportedDefaultOrEnglish(SystemLanguage systemLanguage, int expected)
        {
            var result = typeof(EditorLocalization).GetMethod("GetSystemLanguage", BindingFlags.Static | BindingFlags.NonPublic)
                .Invoke(null, new object[] { systemLanguage });
            Assert.That(result, Is.EqualTo((EditorLanguage)expected));
        }

        [Test]
        public void LanguageSelector_PreservesIdsAndDisplaysKoreanBeforeChinese()
        {
            Assert.That(EditorLocalization.AvailableLanguageLabels,
                Is.EqualTo(new[] { "English", "日本語", "한국어", "繁體中文", "简体中文" }));
            Assert.That(EditorLocalization.GetLanguageAtIndex(2), Is.EqualTo(EditorLanguage.Korean));
            Assert.That(EditorLocalization.GetLanguageAtIndex(3), Is.EqualTo(EditorLanguage.TraditionalChinese));
            Assert.That(EditorLocalization.GetLanguageAtIndex(4), Is.EqualTo(EditorLanguage.SimplifiedChinese));
            Assert.That(EditorLocalization.GetLanguageAtIndex(-1), Is.EqualTo(EditorLanguage.English));
            Assert.That(EditorLocalization.GetLanguageAtIndex(5), Is.EqualTo(EditorLanguage.English));
            foreach (var language in new[] { EditorLanguage.TraditionalChinese, EditorLanguage.SimplifiedChinese, EditorLanguage.Korean })
            {
                EditorLocalization.SetLanguage(language);
                Assert.That(EditorPrefs.GetInt(EditorPrefKeys.EditorLanguage), Is.EqualTo((int)language));
                Assert.That(EditorLocalization.GetLanguageLabel(language), Is.EqualTo(language == EditorLanguage.Korean ? "한국어" : language == EditorLanguage.TraditionalChinese ? "繁體中文" : "简体中文"));
            }
            Assert.That((int)EditorLanguage.TraditionalChinese, Is.EqualTo(2));
            Assert.That((int)EditorLanguage.SimplifiedChinese, Is.EqualTo(3));
            Assert.That((int)EditorLanguage.Korean, Is.EqualTo(4));
        }

        [Test]
        public void Korean_AllEntriesHaveMatchingFormatPlaceholders()
        {
            var table = (Dictionary<string, string[]>)typeof(EditorLocalization)
                .GetField("Texts", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            EditorLocalization.SetLanguage(EditorLanguage.Korean);
            foreach (var entry in table)
            {
                Assert.That(entry.Value.Length, Is.EqualTo(5), entry.Key);
                Assert.That(entry.Value.All(v => !string.IsNullOrWhiteSpace(v)), Is.True, entry.Key);
                Assert.That(EditorLocalization.Text(entry.Key), Is.EqualTo(entry.Value[2]), entry.Key);
                var expected = Regex.Matches(entry.Key, @"\{\d+(?:[^{}]*)\}").Cast<Match>().Select(m => m.Value).OrderBy(v => v).ToArray();
                foreach (var value in entry.Value)
                {
                    Assert.That(Regex.Matches(value, @"\{\d+(?:[^{}]*)\}").Cast<Match>().Select(m => m.Value).OrderBy(v => v).ToArray(), Is.EqualTo(expected), entry.Key);
                }
            }
        }

        [Test]
        public void Korean_DynamicStatusAndUnknownFallbackPreserveData()
        {
            EditorLocalization.SetLanguage(EditorLanguage.Korean);
            Assert.That(EditorLocalization.Text("Found Python 3.11.9 in PATH"), Does.Contain("3.11.9").And.Contain("PATH"));
            Assert.That(EditorLocalization.Text("Found Python 3.11.9 in PATH"), Is.Not.EqualTo("Found Python 3.11.9 in PATH"));
            Assert.That(EditorLocalization.Format("{0} of {1} resources enabled.", 3, 5), Does.Contain("3").And.Contain("5"));
            Assert.That(EditorLocalization.Text("Unknown future UI text"), Is.EqualTo("Unknown future UI text"));
        }

        [Test]
        public void Text_UnknownSource_FallsBackToEnglishSource()
        {
            EditorLocalization.SetLanguage(EditorLanguage.SimplifiedChinese);

            const string upstreamText = "Future upstream UI text";
            Assert.That(EditorLocalization.Text(upstreamText), Is.EqualTo(upstreamText));
        }

        [Test]
        public void Format_LocalizesTemplateAndPreservesArguments()
        {
            EditorLocalization.SetLanguage(EditorLanguage.SimplifiedChinese);

            Assert.That(
                EditorLocalization.Format("{0} of {1} resources enabled.", 3, 5),
                Is.EqualTo("已启用 5 个资源中的 3 个。"));
        }

        [Test]
        public void Text_LocalizesDynamicDependencyStatus()
        {
            EditorLocalization.SetLanguage(EditorLanguage.SimplifiedChinese);

            Assert.That(
                EditorLocalization.Text("Found Python 3.11.9 in PATH"),
                Is.EqualTo("在 PATH 中找到 Python 3.11.9"));
        }

        [Test]
        public void HttpPolicyResult_RemainsEnglishOutsideUiBoundary()
        {
            EditorLocalization.SetLanguage(EditorLanguage.Korean);
            EditorPrefs.SetBool(EditorPrefKeys.AllowLanHttpBind, false);

            bool allowed = HttpEndpointUtility.IsHttpLocalUrlAllowedForLaunch(
                "http://0.0.0.0:8080",
                out string error);

            Assert.That(allowed, Is.False);
            Assert.That(error, Does.Contain("disabled by default").IgnoreCase);
        }
    }
}
