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
        private bool hadAllowLanBind;
        private bool originalAllowLanBind;

        [SetUp]
        public void SetUp()
        {
            originalLanguage = EditorLocalization.CurrentLanguage;
            hadAllowLanBind = EditorPrefs.HasKey(EditorPrefKeys.AllowLanHttpBind);
            originalAllowLanBind = EditorPrefs.GetBool(EditorPrefKeys.AllowLanHttpBind, false);
        }

        [TearDown]
        public void TearDown()
        {
            EditorLocalization.SetLanguage(originalLanguage);
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
        public void Text_ReturnsExpectedTranslation(int languageValue, string expected)
        {
            EditorLocalization.SetLanguage((EditorLanguage)languageValue);

            Assert.That(EditorLocalization.Text("Tools"), Is.EqualTo(expected));
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
            EditorLocalization.SetLanguage(EditorLanguage.SimplifiedChinese);
            EditorPrefs.SetBool(EditorPrefKeys.AllowLanHttpBind, false);

            bool allowed = HttpEndpointUtility.IsHttpLocalUrlAllowedForLaunch(
                "http://0.0.0.0:8080",
                out string error);

            Assert.That(allowed, Is.False);
            Assert.That(error, Does.Contain("disabled by default").IgnoreCase);
        }
    }
}
