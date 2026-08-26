using System;
using System.Collections.Generic;
using MCPForUnity.Editor.Constants;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MCPForUnity.Editor.Helpers
{
    internal enum EditorLanguage
    {
        English = 0,
        Japanese = 1,
        TraditionalChinese = 2,
        SimplifiedChinese = 3,
    }

    /// <summary>
    /// Localizes MCP for Unity editor UI without changing MCP tool names, schemas, or protocol data.
    /// English source text is used as the lookup key and as the fallback for new upstream UI text.
    /// </summary>
    internal static class EditorLocalization
    {
        private static readonly string[] LanguageLabels =
        {
            "English",
            "日本語",
            "繁體中文",
            "简体中文",
        };

        private static readonly Dictionary<string, string[]> Texts = new Dictionary<string, string[]>
        {
            { "Language:", Values("Language:", "言語:", "語言：", "语言：") },
            { "Advanced", Values("Advanced", "詳細", "進階", "高级") },
            { "Advanced Settings", Values("Advanced Settings", "詳細設定", "進階設定", "高级设置") },
            { "AI Asset Generation", Values("AI Asset Generation", "AI アセット生成", "AI 資產生成", "AI 资源生成") },
            { "Allow Insecure Remote HTTP:", Values("Allow Insecure Remote HTTP:", "安全でないリモート HTTP を許可:", "允許不安全的遠端 HTTP：", "允许不安全的远程 HTTP：") },
            { "Allow LAN Bind (HTTP Local):", Values("Allow LAN Bind (HTTP Local):", "LAN バインドを許可 (HTTP Local):", "允許 LAN 繫結（HTTP Local）：", "允许 LAN 绑定（HTTP Local）：") },
            { "API Key:", Values("API Key:", "API キー:", "API 金鑰：", "API 密钥：") },
            { "Auto-Normalize Imported Models:", Values("Auto-Normalize Imported Models:", "インポートしたモデルを自動正規化:", "自動正規化匯入的模型：", "自动标准化导入的模型：") },
            { "Auto-Start Server on Editor Load:", Values("Auto-Start Server on Editor Load:", "Editor 起動時にサーバーを自動起動:", "編輯器載入時自動啟動伺服器：", "编辑器加载时自动启动服务器：") },
            { "Browse", Values("Browse", "参照", "瀏覽", "浏览") },
            { "Cancel", Values("Cancel", "キャンセル", "取消", "取消") },
            { "Changes apply after reconnecting or re-registering resources.", Values("Changes apply after reconnecting or re-registering resources.", "再接続またはリソースの再登録後に変更が反映されます。", "重新連線或重新註冊資源後，變更才會生效。", "重新连接或重新注册资源后，更改才会生效。") },
            { "Changes apply after reconnecting or re-registering tools.", Values("Changes apply after reconnecting or re-registering tools.", "再接続またはツールの再登録後に変更が反映されます。", "重新連線或重新註冊工具後，變更才會生效。", "重新连接或重新注册工具后，更改才会生效。") },
            { "Claude CLI Path:", Values("Claude CLI Path:", "Claude CLI パス:", "Claude CLI 路徑：", "Claude CLI 路径：") },
            { "Clear", Values("Clear", "クリア", "清除", "清除") },
            { "Client Configuration", Values("Client Configuration", "クライアント設定", "用戶端設定", "客户端配置") },
            { "Client Project Dir:", Values("Client Project Dir:", "クライアントのプロジェクトディレクトリ:", "用戶端專案目錄：", "客户端项目目录：") },
            { "Client:", Values("Client:", "クライアント:", "用戶端：", "客户端：") },
            { "Config Path:", Values("Config Path:", "設定ファイルのパス:", "設定檔路徑：", "配置文件路径：") },
            { "Configuration:", Values("Configuration:", "設定内容:", "設定內容：", "配置内容：") },
            { "Configure", Values("Configure", "設定", "設定", "配置") },
            { "Configure All Detected Clients", Values("Configure All Detected Clients", "検出したすべてのクライアントを設定", "設定所有偵測到的用戶端", "配置所有检测到的客户端") },
            { "Configure MCP Clients", Values("Configure MCP Clients", "MCP クライアントを設定", "設定 MCP 用戶端", "配置 MCP 客户端") },
            { "Configure Selected", Values("Configure Selected", "選択項目を設定", "設定所選項目", "配置所选项") },
            { "Connect", Values("Connect", "接続", "連線", "连接") },
            { "Copy", Values("Copy", "コピー", "複製", "复制") },
            { "Create", Values("Create", "作成", "建立", "创建") },
            { "Debug Logging:", Values("Debug Logging:", "デバッグログ:", "偵錯記錄：", "调试日志：") },
            { "Default 3D Format:", Values("Default 3D Format:", "既定の 3D 形式:", "預設 3D 格式：", "默认 3D 格式：") },
            { "Deploy", Values("Deploy", "配置", "部署", "部署") },
            { "Deps", Values("Deps", "依存関係", "相依套件", "依赖") },
            { "Disable All", Values("Disable All", "すべて無効化", "全部停用", "全部禁用") },
            { "Disconnected", Values("Disconnected", "切断", "已中斷連線", "未连接") },
            { "Discovering resources...", Values("Discovering resources...", "リソースを検出中...", "正在探索資源……", "正在发现资源……") },
            { "Discovering tools...", Values("Discovering tools...", "ツールを検出中...", "正在探索工具……", "正在发现工具……") },
            { "EditorPrefs Manager", Values("EditorPrefs Manager", "EditorPrefs マネージャー", "EditorPrefs 管理員", "EditorPrefs 管理器") },
            { "Enable All", Values("Enable All", "すべて有効化", "全部啟用", "全部启用") },
            { "Enter your own provider API keys. Generation is triggered via MCP tools / CLI, not here. Keys are stored in your OS secure store (Keychain / Credential Manager / libsecret), never in the project.", Values("Enter your own provider API keys. Generation is triggered via MCP tools / CLI, not here. Keys are stored in your OS secure store (Keychain / Credential Manager / libsecret), never in the project.", "各プロバイダーの API キーを入力してください。生成はここではなく MCP ツール / CLI から実行します。キーは OS の安全なストア (Keychain / Credential Manager / libsecret) に保存され、プロジェクトには保存されません。", "請輸入各提供者的 API 金鑰。生成作業由 MCP 工具／CLI 觸發，而不是在此處執行。金鑰會儲存在作業系統的安全儲存區（Keychain／Credential Manager／libsecret），絕不會寫入專案。", "请输入各提供商的 API 密钥。生成操作由 MCP 工具/CLI 触发，而不是在此处执行。密钥存储在操作系统的安全存储区（Keychain/Credential Manager/libsecret），绝不会写入项目。") },
            { "Force Fresh Install:", Values("Force Fresh Install:", "常に新規インストール:", "強制全新安裝：", "强制全新安装：") },
            { "Generative", Values("Generative", "生成", "生成", "生成") },
            { "Get API Key", Values("Get API Key", "API キーを取得", "取得 API 金鑰", "获取 API 密钥") },
            { "GLB import needs the glTFast package — install it from the Dependencies tab.", Values("GLB import needs the glTFast package — install it from the Dependencies tab.", "GLB のインポートには glTFast パッケージが必要です。依存関係タブからインストールしてください。", "匯入 GLB 需要 glTFast 套件，請從相依套件分頁安裝。", "导入 GLB 需要 glTFast 包，请从依赖选项卡安装。") },
            { "HTTP URL:", Values("HTTP URL:", "HTTP URL:", "HTTP URL：", "HTTP URL：") },
            { "Install Skills", Values("Install Skills", "スキルをインストール", "安裝技能", "安装技能") },
            { "Install UV Automatically", Values("Install UV Automatically", "UV を自動インストール", "自動安裝 UV", "自动安装 UV") },
            { "Installation Instructions", Values("Installation Instructions", "インストール手順", "安裝說明", "安装说明") },
            { "Installation Steps:", Values("Installation Steps:", "インストール手順:", "安裝步驟：", "安装步骤：") },
            { "Key", Values("Key", "キー", "鍵", "键") },
            { "Local Server:", Values("Local Server:", "ローカルサーバー:", "本機伺服器：", "本地服务器：") },
            { "Log Record (Assets/mcp.log):", Values("Log Record (Assets/mcp.log):", "ログ記録 (Assets/mcp.log):", "記錄日誌（Assets/mcp.log）：", "日志记录（Assets/mcp.log）：") },
            { "Manage MCP for Unity EditorPrefs. Useful for development and testing.", Values("Manage MCP for Unity EditorPrefs. Useful for development and testing.", "MCP for Unity の EditorPrefs を管理します。開発やテストに便利です。", "管理 MCP for Unity 的 EditorPrefs，適合開發與測試使用。", "管理 MCP for Unity 的 EditorPrefs，适用于开发和测试。") },
            { "Manual Configuration", Values("Manual Configuration", "手動設定", "手動設定", "手动配置") },
            { "Manual Server Launch", Values("Manual Server Launch", "サーバーを手動起動", "手動啟動伺服器", "手动启动服务器") },
            { "MCP for Unity requires Python 3.10+ and UV package manager to function.", Values("MCP for Unity requires Python 3.10+ and UV package manager to function.", "MCP for Unity の動作には Python 3.10 以降と UV パッケージマネージャーが必要です。", "MCP for Unity 需要 Python 3.10 以上版本和 UV 套件管理員才能運作。", "MCP for Unity 需要 Python 3.10 及以上版本和 UV 包管理器才能运行。") },
            { "MCP for Unity Setup", Values("MCP for Unity Setup", "MCP for Unity セットアップ", "MCP for Unity 設定", "MCP for Unity 设置") },
            { "Next", Values("Next", "次へ", "下一步", "下一步") },
            { "Not Configured", Values("Not Configured", "未設定", "尚未設定", "未配置") },
            { "Open", Values("Open", "開く", "開啟", "打开") },
            { "Open Python Install Page", Values("Open Python Install Page", "Python インストールページを開く", "開啟 Python 安裝頁面", "打开 Python 安装页面") },
            { "Open UV Install Page", Values("Open UV Install Page", "UV インストールページを開く", "開啟 UV 安裝頁面", "打开 UV 安装页面") },
            { "Output Root:", Values("Output Root:", "出力ルート:", "輸出根目錄：", "输出根目录：") },
            { "Package Source:", Values("Package Source:", "パッケージソース:", "套件來源：", "包来源：") },
            { "Per-client setup", Values("Per-client setup", "クライアント別設定", "個別用戶端設定", "单独客户端设置") },
            { "Preferences", Values("Preferences", "設定", "偏好設定", "偏好设置") },
            { "Project-Scoped Tools:", Values("Project-Scoped Tools:", "プロジェクト固有ツール:", "專案範圍工具：", "项目范围工具：") },
            { "Reconfigure Clients", Values("Reconfigure Clients", "クライアントを再設定", "重新設定用戶端", "重新配置客户端") },
            { "Refresh", Values("Refresh", "更新", "重新整理", "刷新") },
            { "Rescan", Values("Rescan", "再スキャン", "重新掃描", "重新扫描") },
            { "Resources", Values("Resources", "リソース", "資源", "资源") },
            { "Restore", Values("Restore", "復元", "還原", "还原") },
            { "Save changes", Values("Save changes", "変更を保存", "儲存變更", "保存更改") },
            { "Screenshots Folder:", Values("Screenshots Folder:", "スクリーンショットフォルダー:", "螢幕擷取畫面資料夾：", "截图文件夹：") },
            { "Script Validation", Values("Script Validation", "スクリプト検証", "指令碼驗證", "脚本验证") },
            { "Select", Values("Select", "選択", "選取", "选择") },
            { "Server", Values("Server", "サーバー", "伺服器", "服务器") },
            { "Server Health:", Values("Server Health:", "サーバー状態:", "伺服器健康狀態：", "服务器健康状态：") },
            { "Server Source:", Values("Server Source:", "サーバーソース:", "伺服器來源：", "服务器来源：") },
            { "Skip", Values("Skip", "スキップ", "略過", "跳过") },
            { "Start", Values("Start", "開始", "啟動", "启动") },
            { "Start Server", Values("Start Server", "サーバーを起動", "啟動伺服器", "启动服务器") },
            { "System Requirements", Values("System Requirements", "システム要件", "系統需求", "系统要求") },
            { "Test", Values("Test", "テスト", "測試", "测试") },
            { "Tools", Values("Tools", "ツール", "工具", "工具") },
            { "Transport:", Values("Transport:", "トランスポート:", "傳輸方式：", "传输方式：") },
            { "Type", Values("Type", "種類", "類型", "类型") },
            { "Unity Socket Port:", Values("Unity Socket Port:", "Unity ソケットポート:", "Unity 通訊端連接埠：", "Unity 套接字端口：") },
            { "Unknown", Values("Unknown", "不明", "未知", "未知") },
            { "Use this command to launch the server manually:", Values("Use this command to launch the server manually:", "このコマンドを使用してサーバーを手動で起動します:", "使用此命令手動啟動伺服器：", "使用此命令手动启动服务器：") },
            { "UV Package Manager", Values("UV Package Manager", "UV パッケージマネージャー", "UV 套件管理員", "UV 包管理器") },
            { "UVX Path:", Values("UVX Path:", "UVX パス:", "UVX 路徑：", "UVX 路径：") },
            { "Validation Level:", Values("Validation Level:", "検証レベル:", "驗證層級：", "验证级别：") },
            { "Value", Values("Value", "値", "值", "值") },
            { "We found the following MCP clients on your machine. Select which to configure:", Values("We found the following MCP clients on your machine. Select which to configure:", "このマシンで次の MCP クライアントが見つかりました。設定するものを選択してください:", "在您的電腦上找到以下 MCP 用戶端，請選擇要設定的項目：", "在您的计算机上发现以下 MCP 客户端，请选择要配置的客户端：") },

            { "Connected", Values("Connected", "接続済み", "已連線", "已连接") },
            { "Error", Values("Error", "エラー", "錯誤", "错误") },
            { "OK", Values("OK", "OK", "確定", "确定") },
            { "Install All", Values("Install All", "すべてインストール", "全部安裝", "全部安装") },
            { "Uninstall All", Values("Uninstall All", "すべてアンインストール", "全部解除安裝", "全部卸载") },
            { "Installing...", Values("Installing...", "インストール中...", "正在安裝……", "正在安装……") },
            { "Removing...", Values("Removing...", "削除中...", "正在移除……", "正在移除……") },
            { "Installed", Values("Installed", "インストール済み", "已安裝", "已安装") },
            { "Not installed", Values("Not installed", "未インストール", "尚未安裝", "未安装") },
            { "Optional Dependencies", Values("Optional Dependencies", "オプションの依存関係", "選用相依套件", "可选依赖") },
            { "Some tool groups require optional packages. Install them to unlock additional capabilities.", Values("Some tool groups require optional packages. Install them to unlock additional capabilities.", "一部のツールグループにはオプションパッケージが必要です。追加機能を利用するにはインストールしてください。", "部分工具群組需要選用套件。安裝後可解鎖更多功能。", "部分工具组需要可选包。安装后可解锁更多功能。") },
            { "Start Session", Values("Start Session", "セッションを開始", "啟動工作階段", "启动会话") },
            { "End Session", Values("End Session", "セッションを終了", "結束工作階段", "结束会话") },
            { "No Session", Values("No Session", "セッションなし", "無工作階段", "无会话") },
            { "Starting…", Values("Starting…", "起動中…", "正在啟動……", "正在启动……") },
            { "Stop Server", Values("Stop Server", "サーバーを停止", "停止伺服器", "停止服务器") },
            { "No MCP tools discovered.", Values("No MCP tools discovered.", "MCP ツールが見つかりません。", "未探索到 MCP 工具。", "未发现 MCP 工具。") },
            { "No MCP resources discovered.", Values("No MCP resources discovered.", "MCP リソースが見つかりません。", "未探索到 MCP 資源。", "未发现 MCP 资源。") },
            { "On by default", Values("On by default", "既定で有効", "預設啟用", "默认启用") },
            { "Off by default", Values("Off by default", "既定で無効", "預設停用", "默认禁用") },
            { "Structured output", Values("Structured output", "構造化出力", "結構化輸出", "结构化输出") },
            { "Free-form", Values("Free-form", "自由形式", "自由格式", "自由格式") },
            { "Capture:", Values("Capture:", "キャプチャ:", "擷取：", "捕获：") },
            { "Max commands per batch:", Values("Max commands per batch:", "バッチごとの最大コマンド数:", "每批次命令數上限：", "每批最大命令数：") },
            { "Configuration Failed", Values("Configuration Failed", "設定に失敗", "設定失敗", "配置失败") },
            { "Connection Failed", Values("Connection Failed", "接続に失敗", "連線失敗", "连接失败") },
            { "Connection Error", Values("Connection Error", "接続エラー", "連線錯誤", "连接错误") },
            { "Connection Blocked", Values("Connection Blocked", "接続がブロックされました", "連線遭到封鎖", "连接被阻止") },
            { "Cannot Start HTTP Server", Values("Cannot Start HTTP Server", "HTTP サーバーを起動できません", "無法啟動 HTTP 伺服器", "无法启动 HTTP 服务器") },
            { "Invalid Path", Values("Invalid Path", "無効なパス", "無效的路徑", "无效路径") },
            { "Invalid Source", Values("Invalid Source", "無効なソース", "無效的來源", "无效来源") },
            { "Deployment Failed", Values("Deployment Failed", "配置に失敗", "部署失敗", "部署失败") },
            { "Deployment Complete", Values("Deployment Complete", "配置完了", "部署完成", "部署完成") },
            { "Restore Failed", Values("Restore Failed", "復元に失敗", "還原失敗", "还原失败") },
            { "Restore Complete", Values("Restore Complete", "復元完了", "還原完成", "还原完成") },
            { "Install Skills Failed", Values("Install Skills Failed", "スキルのインストールに失敗", "技能安裝失敗", "技能安装失败") },
            { "Open File", Values("Open File", "ファイルを開く", "開啟檔案", "打开文件") },
            { "Not available", Values("Not available", "利用不可", "無法使用", "不可用") },
            { "Not Found", Values("Not Found", "見つかりません", "找不到", "未找到") },
            { "Blender app detected ✓", Values("Blender app detected ✓", "Blender アプリを検出しました ✓", "已偵測到 Blender 應用程式 ✓", "已检测到 Blender 应用 ✓") },
            { "Blender app not found on this machine", Values("Blender app not found on this machine", "このマシンに Blender アプリが見つかりません", "此電腦上找不到 Blender 應用程式", "此计算机上未找到 Blender 应用") },
            { "Search", Values("Search", "検索", "搜尋", "搜索") },
            { "Refresh prefs", Values("Refresh prefs", "設定を更新", "重新整理偏好設定", "刷新偏好设置") },
            { "Available", Values("Available", "利用可能", "可用", "可用") },
            { "No supported MCP clients detected on this machine. You can configure clients later from Tools → MCP for Unity.", Values("No supported MCP clients detected on this machine. You can configure clients later from Tools → MCP for Unity.", "このマシンでは対応する MCP クライアントが検出されませんでした。後で Tools → MCP for Unity から設定できます。", "此電腦上未偵測到支援的 MCP 用戶端。您可以稍後從 Tools → MCP for Unity 進行設定。", "此计算机上未检测到支持的 MCP 客户端。您可以稍后从 Tools → MCP for Unity 进行配置。") },
            { "No clients were selected. Tick at least one client to continue, or close the window to skip setup.", Values("No clients were selected. Tick at least one client to continue, or close the window to skip setup.", "クライアントが選択されていません。続行するには 1 つ以上選択するか、ウィンドウを閉じて設定をスキップしてください。", "尚未選取任何用戶端。請勾選至少一個用戶端以繼續，或關閉視窗略過設定。", "尚未选择任何客户端。请至少勾选一个客户端以继续，或关闭窗口跳过设置。") },
            { "You're all set. Ask your AI assistant to create a GameObject in the open scene to confirm the connection.", Values("You're all set. Ask your AI assistant to create a GameObject in the open scene to confirm the connection.", "準備が完了しました。AI アシスタントに、開いているシーンへ GameObject を作成するよう依頼して接続を確認してください。", "一切準備就緒。請讓 AI 助理在目前開啟的場景中建立 GameObject，以確認連線。", "一切准备就绪。请让 AI 助手在当前打开的场景中创建一个 GameObject，以确认连接。") },
            { "{0} configured, {1} failed.{2}{3}", Values("{0} configured, {1} failed.{2}{3}", "{0} 件を設定、{1} 件が失敗しました。{2}{3}", "已設定 {0} 個，{1} 個失敗。{2}{3}", "已配置 {0} 个，{1} 个失败。{2}{3}") },
            { "Install UV", Values("Install UV", "UV をインストール", "安裝 UV", "安装 UV") },
            { "Install", Values("Install", "インストール", "安裝", "安装") },
            { "This will download and run the official uv installer:\n\n{0}\n\nContinue?", Values("This will download and run the official uv installer:\n\n{0}\n\nContinue?", "公式の uv インストーラーをダウンロードして実行します:\n\n{0}\n\n続行しますか？", "這將下載並執行官方 uv 安裝程式：\n\n{0}\n\n要繼續嗎？", "这将下载并运行官方 uv 安装程序：\n\n{0}\n\n是否继续？") },
            { "Installing UV…", Values("Installing UV…", "UV をインストール中…", "正在安裝 UV……", "正在安装 UV……") },
            { "Installing uv… this can take a moment.", Values("Installing uv… this can take a moment.", "uv をインストールしています。しばらくお待ちください。", "正在安裝 uv，可能需要一些時間。", "正在安装 uv，可能需要一些时间。") },
            { "uv installed, but it isn't visible on PATH yet. Restart Unity (or your terminal) so it picks up the new PATH, then click Refresh.\n\n{0}", Values("uv installed, but it isn't visible on PATH yet. Restart Unity (or your terminal) so it picks up the new PATH, then click Refresh.\n\n{0}", "uv はインストールされましたが、まだ PATH から見つかりません。Unity（またはターミナル）を再起動して新しい PATH を読み込み、更新をクリックしてください。\n\n{0}", "uv 已安裝，但目前尚未出現在 PATH 中。請重新啟動 Unity（或終端機）以載入新的 PATH，然後按一下重新整理。\n\n{0}", "uv 已安装，但目前尚未出现在 PATH 中。请重启 Unity（或终端）以加载新的 PATH，然后单击刷新。\n\n{0}") },
            { "Install UV Failed", Values("Install UV Failed", "UV のインストールに失敗", "UV 安裝失敗", "UV 安装失败") },
            { "The installer did not complete successfully. You can install uv manually via \"Open UV Install Page\".\n\n{0}", Values("The installer did not complete successfully. You can install uv manually via \"Open UV Install Page\".\n\n{0}", "インストーラーが正常に完了しませんでした。「UV インストールページを開く」から手動で uv をインストールできます。\n\n{0}", "安裝程式未成功完成。您可以透過「開啟 UV 安裝頁面」手動安裝 uv。\n\n{0}", "安装程序未成功完成。您可以通过“打开 UV 安装页面”手动安装 uv。\n\n{0}") },
            { "✓ All requirements met! MCP for Unity is ready to use.", Values("✓ All requirements met! MCP for Unity is ready to use.", "✓ すべての要件を満たしています。MCP for Unity を使用できます。", "✓ 已符合所有需求！MCP for Unity 已可使用。", "✓ 已满足所有要求！MCP for Unity 已可使用。") },
            { "⚠ Missing dependencies. MCP for Unity requires all dependencies to function.", Values("⚠ Missing dependencies. MCP for Unity requires all dependencies to function.", "⚠ 依存関係が不足しています。MCP for Unity の動作にはすべての依存関係が必要です。", "⚠ 缺少相依套件。MCP for Unity 需要所有相依套件才能運作。", "⚠ 缺少依赖。MCP for Unity 需要所有依赖才能运行。") },
            { "Cannot convert '{0}' to int", Values("Cannot convert '{0}' to int", "「{0}」を整数に変換できません", "無法將「{0}」轉換為整數", "无法将“{0}”转换为整数") },
            { "Cannot convert '{0}' to float", Values("Cannot convert '{0}' to float", "「{0}」を浮動小数点数に変換できません", "無法將「{0}」轉換為浮點數", "无法将“{0}”转换为浮点数") },
            { "Cannot convert '{0}' to bool (use 'True' or 'False')", Values("Cannot convert '{0}' to bool (use 'True' or 'False')", "「{0}」を bool に変換できません（'True' または 'False' を使用してください）", "無法將「{0}」轉換為布林值（請使用 'True' 或 'False'）", "无法将“{0}”转换为布尔值（请使用 'True' 或 'False'）") },
            { "Install All Dependencies", Values("Install All Dependencies", "すべての依存関係をインストール", "安裝所有相依套件", "安装所有依赖") },
            { "This will install Roslyn DLLs, ProBuilder, Cinemachine, VFX Graph, and glTFast. Continue?", Values("This will install Roslyn DLLs, ProBuilder, Cinemachine, VFX Graph, and glTFast. Continue?", "Roslyn DLL、ProBuilder、Cinemachine、VFX Graph、glTFast をインストールします。続行しますか？", "這將安裝 Roslyn DLL、ProBuilder、Cinemachine、VFX Graph 和 glTFast。要繼續嗎？", "这将安装 Roslyn DLL、ProBuilder、Cinemachine、VFX Graph 和 glTFast。是否继续？") },
            { "Uninstall All Dependencies", Values("Uninstall All Dependencies", "すべての依存関係をアンインストール", "解除安裝所有相依套件", "卸载所有依赖") },
            { "This will remove Roslyn DLLs, ProBuilder, Cinemachine, VFX Graph, and glTFast. Continue?", Values("This will remove Roslyn DLLs, ProBuilder, Cinemachine, VFX Graph, and glTFast. Continue?", "Roslyn DLL、ProBuilder、Cinemachine、VFX Graph、glTFast を削除します。続行しますか？", "這將移除 Roslyn DLL、ProBuilder、Cinemachine、VFX Graph 和 glTFast。要繼續嗎？", "这将移除 Roslyn DLL、ProBuilder、Cinemachine、VFX Graph 和 glTFast。是否继续？") },
            { "Roslyn (C# 12+ Compiler)", Values("Roslyn (C# 12+ Compiler)", "Roslyn (C# 12+ コンパイラー)", "Roslyn（C# 12+ 編譯器）", "Roslyn（C# 12+ 编译器）") },
            { "Enables modern C# syntax in execute_code tool (scripting_ext group).", Values("Enables modern C# syntax in execute_code tool (scripting_ext group).", "execute_code ツール（scripting_ext グループ）で最新の C# 構文を使用できます。", "讓 execute_code 工具（scripting_ext 群組）支援現代 C# 語法。", "让 execute_code 工具（scripting_ext 组）支持现代 C# 语法。") },
            { "Installed via Plugins/Roslyn — execute_code uses Roslyn", Values("Installed via Plugins/Roslyn — execute_code uses Roslyn", "Plugins/Roslyn にインストール済み — execute_code は Roslyn を使用", "已透過 Plugins/Roslyn 安裝 — execute_code 使用 Roslyn", "已通过 Plugins/Roslyn 安装 — execute_code 使用 Roslyn") },
            { "Available (loaded from NuGet/external) — execute_code uses Roslyn", Values("Available (loaded from NuGet/external) — execute_code uses Roslyn", "利用可能（NuGet/外部から読み込み）— execute_code は Roslyn を使用", "可用（從 NuGet／外部載入）— execute_code 使用 Roslyn", "可用（从 NuGet/外部加载）— execute_code 使用 Roslyn") },
            { "Not installed — execute_code falls back to C# 6 (CodeDom)", Values("Not installed — execute_code falls back to C# 6 (CodeDom)", "未インストール — execute_code は C# 6 (CodeDom) にフォールバック", "尚未安裝 — execute_code 將回退至 C# 6（CodeDom）", "未安装 — execute_code 将回退到 C# 6（CodeDom）") },
            { "Required for the manage_probuilder tool (probuilder group).", Values("Required for the manage_probuilder tool (probuilder group).", "manage_probuilder ツール（probuilder グループ）に必要です。", "manage_probuilder 工具（probuilder 群組）需要此套件。", "manage_probuilder 工具（probuilder 组）需要此包。") },
            { "Enhances manage_camera with virtual camera support (core group).", Values("Enhances manage_camera with virtual camera support (core group).", "manage_camera に仮想カメラ対応を追加します（core グループ）。", "為 manage_camera 增加虛擬攝影機支援（core 群組）。", "为 manage_camera 增加虚拟相机支持（core 组）。") },
            { "Not installed — camera tool works without it", Values("Not installed — camera tool works without it", "未インストール — カメラツールはこれがなくても動作します", "尚未安裝 — 攝影機工具仍可在沒有此套件時運作", "未安装 — 相机工具在没有它时仍可工作") },
            { "Enables VisualEffect support in manage_vfx tool (vfx group).", Values("Enables VisualEffect support in manage_vfx tool (vfx group).", "manage_vfx ツール（vfx グループ）で VisualEffect を使用できます。", "讓 manage_vfx 工具（vfx 群組）支援 VisualEffect。", "让 manage_vfx 工具（vfx 组）支持 VisualEffect。") },
            { "Not installed — VFX tool falls back to ParticleSystem/LineRenderer", Values("Not installed — VFX tool falls back to ParticleSystem/LineRenderer", "未インストール — VFX ツールは ParticleSystem/LineRenderer にフォールバック", "尚未安裝 — VFX 工具將回退至 ParticleSystem／LineRenderer", "未安装 — VFX 工具将回退到 ParticleSystem/LineRenderer") },
            { "glTFast (glTF/GLB import)", Values("glTFast (glTF/GLB import)", "glTFast (glTF/GLB インポート)", "glTFast（匯入 glTF／GLB）", "glTFast（导入 glTF/GLB）") },
            { "Enables .glb/.gltf model import for the AI Asset Generation tools (asset_gen group).", Values("Enables .glb/.gltf model import for the AI Asset Generation tools (asset_gen group).", "AI アセット生成ツール（asset_gen グループ）で .glb/.gltf モデルをインポートできます。", "讓 AI 資產生成工具（asset_gen 群組）支援匯入 .glb／.gltf 模型。", "让 AI 资源生成工具（asset_gen 组）支持导入 .glb/.gltf 模型。") },
            { "Installed — GLB generation/import works", Values("Installed — GLB generation/import works", "インストール済み — GLB の生成/インポートが利用可能", "已安裝 — 可生成／匯入 GLB", "已安装 — 可生成/导入 GLB") },
            { "Not installed — GLB import is unavailable; FBX still works, or install to enable GLB", Values("Not installed — GLB import is unavailable; FBX still works, or install to enable GLB", "未インストール — GLB はインポートできません。FBX は利用可能です。GLB を使うにはインストールしてください", "尚未安裝 — 無法匯入 GLB；FBX 仍可使用，或安裝此套件以啟用 GLB", "未安装 — 无法导入 GLB；FBX 仍可使用，或安装此包以启用 GLB") },
            { "Remove", Values("Remove", "削除", "移除", "移除") },
            { "Uninstall", Values("Uninstall", "アンインストール", "解除安裝", "卸载") },
            { "Remove {0}", Values("Remove {0}", "{0} を削除", "移除 {0}", "移除 {0}") },
            { "Are you sure you want to remove {0}?", Values("Are you sure you want to remove {0}?", "{0} を削除してもよろしいですか？", "確定要移除 {0} 嗎？", "确定要移除 {0} 吗？") },
            { "Installing Packages", Values("Installing Packages", "パッケージをインストール中", "正在安裝套件", "正在安装包") },
            { "Installing {0} package(s)...", Values("Installing {0} package(s)...", "{0} 個のパッケージをインストール中...", "正在安裝 {0} 個套件……", "正在安装 {0} 个包……") },
            { "Removing Packages", Values("Removing Packages", "パッケージを削除中", "正在移除套件", "正在移除包") },
            { "Removing {0} package(s)...", Values("Removing {0} package(s)...", "{0} 個のパッケージを削除中...", "正在移除 {0} 個套件……", "正在移除 {0} 个包……") },
            { "{0} v{1} (pre-release package, using prerelease server channel)", Values("{0} v{1} (pre-release package, using prerelease server channel)", "{0} v{1}（プレリリースパッケージ、プレリリース版サーバーチャンネルを使用）", "{0} v{1}（預發行套件，使用預發行伺服器頻道）", "{0} v{1}（预发布包，使用预发布服务器通道）") },
            { "Update available: v{0}  (current: v{1})", Values("Update available: v{0}  (current: v{1})", "更新があります: v{0}（現在: v{1}）", "有可用更新：v{0}（目前：v{1}）", "有可用更新：v{0}（当前：v{1}）") },
            { "Latest version: v{0}\nCurrent version: v{1}", Values("Latest version: v{0}\nCurrent version: v{1}", "最新バージョン: v{0}\n現在のバージョン: v{1}", "最新版本：v{0}\n目前版本：v{1}", "最新版本：v{0}\n当前版本：v{1}") },
            { "HTTP endpoint URL for the MCP server. Use localhost for local servers.", Values("HTTP endpoint URL for the MCP server. Use localhost for local servers.", "MCP サーバーの HTTP エンドポイント URL。ローカルサーバーには localhost を使用してください。", "MCP 伺服器的 HTTP 端點 URL。本機伺服器請使用 localhost。", "MCP 服务器的 HTTP 端点 URL。本地服务器请使用 localhost。") },
            { "Port for Unity's internal MCP bridge socket. Used for stdio transport.", Values("Port for Unity's internal MCP bridge socket. Used for stdio transport.", "Unity 内部 MCP ブリッジソケットのポート。stdio トランスポートで使用します。", "Unity 內部 MCP 橋接通訊端的連接埠，用於 stdio 傳輸。", "Unity 内部 MCP 桥接套接字的端口，用于 stdio 传输。") },
            { "Start or end the MCP session between Unity and the server.", Values("Start or end the MCP session between Unity and the server.", "Unity とサーバー間の MCP セッションを開始または終了します。", "啟動或結束 Unity 與伺服器之間的 MCP 工作階段。", "启动或结束 Unity 与服务器之间的 MCP 会话。") },
            { "API key for remote-hosted MCP server authentication", Values("API key for remote-hosted MCP server authentication", "リモート MCP サーバー認証用の API キー", "遠端託管 MCP 伺服器驗證所用的 API 金鑰", "远程托管 MCP 服务器身份验证所用的 API 密钥") },
            { "Session Active ({0})", Values("Session Active ({0})", "セッション実行中 ({0})", "工作階段進行中（{0}）", "会话进行中（{0}）") },
            { "Disconnect", Values("Disconnect", "切断", "中斷連線", "断开连接") },
            { "Resuming...", Values("Resuming...", "再開中...", "正在恢復……", "正在恢复……") },
            { "An API key is required for HTTP Remote. Enter one above.", Values("An API key is required for HTTP Remote. Enter one above.", "HTTP Remote には API キーが必要です。上に入力してください。", "HTTP Remote 需要 API 金鑰，請在上方輸入。", "HTTP Remote 需要 API 密钥，请在上方输入。") },
            { "HTTP Remote URL is blocked by current security settings.", Values("HTTP Remote URL is blocked by current security settings.", "HTTP Remote URL は現在のセキュリティ設定によりブロックされています。", "目前的安全性設定已封鎖 HTTP Remote URL。", "当前安全设置已阻止 HTTP Remote URL。") },
            { "HTTP Local URL is blocked by current security settings.", Values("HTTP Local URL is blocked by current security settings.", "HTTP Local URL は現在のセキュリティ設定によりブロックされています。", "目前的安全性設定已封鎖 HTTP Local URL。", "当前安全设置已阻止 HTTP Local URL。") },
            { "HTTP Local requires a loopback URL ({0}).", Values("HTTP Local requires a loopback URL ({0}).", "HTTP Local にはループバック URL ({0}) が必要です。", "HTTP Local 需要回送 URL（{0}）。", "HTTP Local 需要环回 URL（{0}）。") },
            { "Run this command in your shell if you prefer to start the server manually.", Values("Run this command in your shell if you prefer to start the server manually.", "サーバーを手動で起動する場合は、このコマンドをシェルで実行してください。", "若要手動啟動伺服器，請在 Shell 中執行此命令。", "若要手动启动服务器，请在 Shell 中运行此命令。") },
            { "The command is not available with the current configuration.", Values("The command is not available with the current configuration.", "現在の設定ではコマンドを利用できません。", "目前的設定無法使用此命令。", "当前配置无法使用此命令。") },
            { "Failed to toggle local HTTP server:\n\n{0}", Values("Failed to toggle local HTTP server:\n\n{0}", "ローカル HTTP サーバーの切り替えに失敗しました:\n\n{0}", "切換本機 HTTP 伺服器失敗：\n\n{0}", "切换本地 HTTP 服务器失败：\n\n{0}") },
            { "Port Unavailable", Values("Port Unavailable", "ポートを使用できません", "連接埠無法使用", "端口不可用") },
            { "The requested port could not be used:\n\n{0}\n\nReverting to the active Unity port.", Values("The requested port could not be used:\n\n{0}\n\nReverting to the active Unity port.", "要求されたポートを使用できませんでした:\n\n{0}\n\n現在の Unity ポートに戻します。", "無法使用要求的連接埠：\n\n{0}\n\n將還原為目前使用中的 Unity 連接埠。", "无法使用请求的端口：\n\n{0}\n\n将恢复为当前使用中的 Unity 端口。") },
            { "Failed to start the MCP session. Check the server URL and that the server is running.", Values("Failed to start the MCP session. Check the server URL and that the server is running.", "MCP セッションを開始できませんでした。サーバー URL とサーバーが実行中であることを確認してください。", "無法啟動 MCP 工作階段。請檢查伺服器 URL，並確認伺服器正在執行。", "无法启动 MCP 会话。请检查服务器 URL，并确认服务器正在运行。") },
            { "Failed to toggle the MCP connection:\n\n{0}", Values("Failed to toggle the MCP connection:\n\n{0}", "MCP 接続の切り替えに失敗しました:\n\n{0}", "切換 MCP 連線失敗：\n\n{0}", "切换 MCP 连接失败：\n\n{0}") },
            { "API key management is not available for this server. Contact your server administrator.", Values("API key management is not available for this server. Contact your server administrator.", "このサーバーでは API キー管理を利用できません。サーバー管理者にお問い合わせください。", "此伺服器不提供 API 金鑰管理功能，請聯絡伺服器管理員。", "此服务器不提供 API 密钥管理功能，请联系服务器管理员。") },
            { "Failed to get API key login URL:\n\n{0}", Values("Failed to get API key login URL:\n\n{0}", "API キーのログイン URL を取得できませんでした:\n\n{0}", "無法取得 API 金鑰登入 URL：\n\n{0}", "无法获取 API 密钥登录 URL：\n\n{0}") },
            { "{0} is configured for \"{1}\" but server is set to \"{2}\". Click \"Configure\" in Client Configuration to update.", Values("{0} is configured for \"{1}\" but server is set to \"{2}\". Click \"Configure\" in Client Configuration to update.", "{0} は「{1}」に設定されていますが、サーバーは「{2}」です。クライアント設定の「設定」をクリックして更新してください。", "{0} 設定為「{1}」，但伺服器設定為「{2}」。請在用戶端設定中按一下「設定」以更新。", "{0} 配置为“{1}”，但服务器设置为“{2}”。请在客户端配置中单击“配置”进行更新。") },
            { "⚠ {0}: {1}", Values("⚠ {0}: {1}", "⚠ {0}: {1}", "⚠ {0}：{1}", "⚠ {0}：{1}") },
            { "HTTP Local requires a loopback URL (localhost/127.0.0.1/::1).", Values("HTTP Local requires a loopback URL (localhost/127.0.0.1/::1).", "HTTP Local にはループバック URL (localhost/127.0.0.1/::1) が必要です。", "HTTP Local 需要回送 URL（localhost/127.0.0.1/::1）。", "HTTP Local 需要环回 URL（localhost/127.0.0.1/::1）。") },
            { "Invalid URL: {0}", Values("Invalid URL: {0}", "無効な URL: {0}", "無效的 URL：{0}", "无效 URL：{0}") },
            { "Binding to all interfaces (0.0.0.0/::) is disabled by default. Enable \"Allow LAN bind for HTTP Local\" in Advanced Settings to opt in.", Values("Binding to all interfaces (0.0.0.0/::) is disabled by default. Enable \"Allow LAN bind for HTTP Local\" in Advanced Settings to opt in.", "すべてのインターフェイス (0.0.0.0/::) へのバインドは既定で無効です。詳細設定で「HTTP Local の LAN バインドを許可」を有効にしてください。", "預設禁止繫結至所有介面（0.0.0.0/::）。請在進階設定中啟用「允許 HTTP Local 的 LAN 繫結」。", "默认禁止绑定到所有接口（0.0.0.0/::）。请在高级设置中启用“允许 HTTP Local 的 LAN 绑定”。") },
            { "HTTP Remote requires a configured URL.", Values("HTTP Remote requires a configured URL.", "HTTP Remote には URL の設定が必要です。", "HTTP Remote 需要設定 URL。", "HTTP Remote 需要配置 URL。") },
            { "Invalid HTTP Remote URL: {0}", Values("Invalid HTTP Remote URL: {0}", "無効な HTTP Remote URL: {0}", "無效的 HTTP Remote URL：{0}", "无效的 HTTP Remote URL：{0}") },
            { "HTTP Remote requires HTTPS by default. Enable \"Allow insecure HTTP for HTTP Remote\" in Advanced Settings to opt in.", Values("HTTP Remote requires HTTPS by default. Enable \"Allow insecure HTTP for HTTP Remote\" in Advanced Settings to opt in.", "HTTP Remote は既定で HTTPS が必要です。詳細設定で「HTTP Remote の安全でない HTTP を許可」を有効にしてください。", "HTTP Remote 預設需要 HTTPS。請在進階設定中啟用「允許 HTTP Remote 使用不安全的 HTTP」。", "HTTP Remote 默认需要 HTTPS。请在高级设置中启用“允许 HTTP Remote 使用不安全的 HTTP”。") },
            { "Unsupported URL scheme '{0}'. Use https:// (or http:// only with explicit insecure opt-in).", Values("Unsupported URL scheme '{0}'. Use https:// (or http:// only with explicit insecure opt-in).", "未対応の URL スキーム「{0}」です。https:// を使用してください（明示的に許可した場合のみ http:// も使用可能）。", "不支援 URL 配置「{0}」。請使用 https://（僅在明確允許不安全連線時使用 http://）。", "不支持 URL 协议“{0}”。请使用 https://（仅在明确允许不安全连接时使用 http://）。") },
            { "Override path to uvx executable. Leave empty for auto-detection.", Values("Override path to uvx executable. Leave empty for auto-detection.", "uvx 実行ファイルのパスを上書きします。自動検出する場合は空のままにします。", "覆寫 uvx 執行檔路徑。留空則自動偵測。", "覆盖 uvx 可执行文件路径。留空则自动检测。") },
            { "Override server source for uvx --from. Leave empty to use default PyPI package. Example local dev: /path/to/unity-mcp/Server", Values("Override server source for uvx --from. Leave empty to use default PyPI package. Example local dev: /path/to/unity-mcp/Server", "uvx --from のサーバーソースを上書きします。既定の PyPI パッケージを使用する場合は空のままにします。ローカル開発例: /path/to/unity-mcp/Server", "覆寫 uvx --from 的伺服器來源。留空則使用預設 PyPI 套件。本機開發範例：/path/to/unity-mcp/Server", "覆盖 uvx --from 的服务器来源。留空则使用默认 PyPI 包。本地开发示例：/path/to/unity-mcp/Server") },
            { "Enable verbose debug logging to the Unity Console.", Values("Enable verbose debug logging to the Unity Console.", "詳細なデバッグログを Unity Console に出力します。", "在 Unity Console 中啟用詳細偵錯記錄。", "在 Unity Console 中启用详细调试日志。") },
            { "Log every MCP tool execution (tool, action, status, duration) to Assets/UnityMCP/Log/mcp.log.", Values("Log every MCP tool execution (tool, action, status, duration) to Assets/UnityMCP/Log/mcp.log.", "すべての MCP ツール実行（ツール、アクション、状態、所要時間）を Assets/UnityMCP/Log/mcp.log に記録します。", "將每次 MCP 工具執行（工具、動作、狀態、持續時間）記錄至 Assets/UnityMCP/Log/mcp.log。", "将每次 MCP 工具执行（工具、操作、状态、持续时间）记录到 Assets/UnityMCP/Log/mcp.log。") },
            { "When enabled, generated uvx commands add '--no-cache --refresh' before launching (slower startup, but avoids stale cached builds while iterating on the Server).", Values("When enabled, generated uvx commands add '--no-cache --refresh' before launching (slower startup, but avoids stale cached builds while iterating on the Server).", "有効にすると、生成される uvx コマンドに起動前の '--no-cache --refresh' が追加されます（起動は遅くなりますが、サーバー開発中の古いキャッシュを回避できます）。", "啟用後，產生的 uvx 命令會在啟動前加入 '--no-cache --refresh'（啟動較慢，但可避免開發伺服器時使用過時快取）。", "启用后，生成的 uvx 命令会在启动前添加 '--no-cache --refresh'（启动较慢，但可避免开发服务器时使用过期缓存）。") },
            { "Allow HTTP Local to bind on all interfaces (0.0.0.0 / ::). Disabled by default because devices on your LAN may reach MCP tools.", Values("Allow HTTP Local to bind on all interfaces (0.0.0.0 / ::). Disabled by default because devices on your LAN may reach MCP tools.", "HTTP Local がすべてのインターフェイス (0.0.0.0 / ::) にバインドすることを許可します。LAN 上の端末から MCP ツールへアクセスできる可能性があるため、既定では無効です。", "允許 HTTP Local 繫結至所有介面（0.0.0.0／::）。由於 LAN 上的裝置可能存取 MCP 工具，因此預設停用。", "允许 HTTP Local 绑定到所有接口（0.0.0.0/::）。由于局域网中的设备可能访问 MCP 工具，因此默认禁用。") },
            { "Allow HTTP Remote over plaintext http/ws. Disabled by default to require HTTPS/WSS.", Values("Allow HTTP Remote over plaintext http/ws. Disabled by default to require HTTPS/WSS.", "HTTP Remote で平文の http/ws を許可します。HTTPS/WSS を必須にするため既定では無効です。", "允許 HTTP Remote 使用明文 http／ws。預設停用以要求 HTTPS／WSS。", "允许 HTTP Remote 使用明文 http/ws。默认禁用以要求 HTTPS/WSS。") },
            { "Test the connection between Unity and the MCP server.", Values("Test the connection between Unity and the MCP server.", "Unity と MCP サーバー間の接続をテストします。", "測試 Unity 與 MCP 伺服器之間的連線。", "测试 Unity 与 MCP 服务器之间的连接。") },
            { "Default folder for screenshots from manage_camera / manage_ui. Project-relative (e.g. 'Assets/Screenshots' or 'Captures'). Empty = built-in default (Assets/Screenshots). Per-call 'output_folder' parameters always override this.", Values("Default folder for screenshots from manage_camera / manage_ui. Project-relative (e.g. 'Assets/Screenshots' or 'Captures'). Empty = built-in default (Assets/Screenshots). Per-call 'output_folder' parameters always override this.", "manage_camera / manage_ui のスクリーンショット用既定フォルダー。プロジェクト相対（例: 'Assets/Screenshots' または 'Captures'）。空の場合は組み込みの既定値 (Assets/Screenshots)。各呼び出しの 'output_folder' パラメーターが常に優先されます。", "manage_camera／manage_ui 螢幕擷取畫面的預設資料夾。使用專案相對路徑（例如 'Assets/Screenshots' 或 'Captures'）。留空則使用內建預設值（Assets/Screenshots）。每次呼叫的 'output_folder' 參數一律優先。", "manage_camera/manage_ui 截图的默认文件夹。使用项目相对路径（例如 'Assets/Screenshots' 或 'Captures'）。留空则使用内置默认值（Assets/Screenshots）。每次调用的 'output_folder' 参数始终优先。") },
            { "Pick a folder inside the project; the path is stored project-relative.", Values("Pick a folder inside the project; the path is stored project-relative.", "プロジェクト内のフォルダーを選択します。パスはプロジェクト相対で保存されます。", "選取專案內的資料夾；路徑會以專案相對路徑儲存。", "选择项目内的文件夹；路径将以项目相对路径存储。") },
            { "Clear override and use the built-in default (Assets/Screenshots).", Values("Clear override and use the built-in default (Assets/Screenshots).", "上書きをクリアして組み込みの既定値 (Assets/Screenshots) を使用します。", "清除覆寫並使用內建預設值（Assets/Screenshots）。", "清除覆盖并使用内置默认值（Assets/Screenshots）。") },
            { "Copy a MCPForUnity folder into this project's package location.", Values("Copy a MCPForUnity folder into this project's package location.", "MCPForUnity フォルダーをこのプロジェクトのパッケージ場所へコピーします。", "將 MCPForUnity 資料夾複製到此專案的套件位置。", "将 MCPForUnity 文件夹复制到此项目的包位置。") },
            { "Browse for uvx executable", Values("Browse for uvx executable", "uvx 実行ファイルを参照", "瀏覽 uvx 執行檔", "浏览 uvx 可执行文件") },
            { "Clear override and use auto-detection", Values("Clear override and use auto-detection", "上書きをクリアして自動検出を使用", "清除覆寫並使用自動偵測", "清除覆盖并使用自动检测") },
            { "Select local server source folder", Values("Select local server source folder", "ローカルサーバーのソースフォルダーを選択", "選取本機伺服器來源資料夾", "选择本地服务器源文件夹") },
            { "Clear override and use default PyPI package", Values("Clear override and use default PyPI package", "上書きをクリアして既定の PyPI パッケージを使用", "清除覆寫並使用預設 PyPI 套件", "清除覆盖并使用默认 PyPI 包") },
            { "Select MCPForUnity source folder", Values("Select MCPForUnity source folder", "MCPForUnity ソースフォルダーを選択", "選取 MCPForUnity 來源資料夾", "选择 MCPForUnity 源文件夹") },
            { "Clear deployment source path", Values("Clear deployment source path", "配置元のパスをクリア", "清除部署來源路徑", "清除部署源路径") },
            { "Copy MCPForUnity to this project's package location", Values("Copy MCPForUnity to this project's package location", "MCPForUnity をこのプロジェクトのパッケージ場所へコピー", "將 MCPForUnity 複製到此專案的套件位置", "将 MCPForUnity 复制到此项目的包位置") },
            { "Restore the last backup before deployment", Values("Restore the last backup before deployment", "配置前の最新バックアップを復元", "還原部署前的上一份備份", "还原部署前的上一个备份") },
            { "Automatically start the local HTTP server and connect the MCP bridge when the Unity Editor opens. Only applies to HTTP transport (stdio always auto-starts).", Values("Automatically start the local HTTP server and connect the MCP bridge when the Unity Editor opens. Only applies to HTTP transport (stdio always auto-starts).", "Unity Editor 起動時にローカル HTTP サーバーを自動起動し、MCP ブリッジへ接続します。HTTP トランスポートにのみ適用されます（stdio は常に自動起動）。", "Unity 編輯器開啟時自動啟動本機 HTTP 伺服器並連線 MCP 橋接器。僅適用於 HTTP 傳輸（stdio 一律自動啟動）。", "Unity 编辑器打开时自动启动本地 HTTP 服务器并连接 MCP 桥接器。仅适用于 HTTP 传输（stdio 始终自动启动）。") },
            { "Invalid override path: {0} (fallback to uvx path) {1}", Values("Invalid override path: {0} (fallback to uvx path) {1}", "無効な上書きパス: {0}（uvx パスへフォールバック）{1}", "無效的覆寫路徑：{0}（回退至 uvx 路徑）{1}", "无效的覆盖路径：{0}（回退到 uvx 路径）{1}") },
            { "Invalid override path: {0}, no uv found", Values("Invalid override path: {0}, no uv found", "無効な上書きパス: {0}、uv が見つかりません", "無效的覆寫路徑：{0}，找不到 uv", "无效的覆盖路径：{0}，未找到 uv") },
            { "uvx (uses PATH)", Values("uvx (uses PATH)", "uvx (PATH を使用)", "uvx（使用 PATH）", "uvx（使用 PATH）") },
            { "Select uv Executable", Values("Select uv Executable", "uv 実行ファイルを選択", "選取 uv 執行檔", "选择 uv 可执行文件") },
            { "Select Server folder (containing pyproject.toml)", Values("Select Server folder (containing pyproject.toml)", "Server フォルダー（pyproject.toml を含む）を選択", "選取 Server 資料夾（包含 pyproject.toml）", "选择 Server 文件夹（包含 pyproject.toml）") },
            { "Target: {0}", Values("Target: {0}", "対象: {0}", "目標：{0}", "目标：{0}") },
            { "Last backup: {0}", Values("Last backup: {0}", "最新バックアップ: {0}", "上一份備份：{0}", "上一个备份：{0}") },
            { "Last backup: none", Values("Last backup: none", "最新バックアップ: なし", "上一份備份：無", "上一个备份：无") },
            { "Select Screenshots Folder (inside this project)", Values("Select Screenshots Folder (inside this project)", "スクリーンショットフォルダーを選択（このプロジェクト内）", "選取螢幕擷取畫面資料夾（此專案內）", "选择截图文件夹（此项目内）") },
            { "Pick a Subfolder", Values("Pick a Subfolder", "サブフォルダーを選択", "選取子資料夾", "选择子文件夹") },
            { "Please pick a subfolder of the project (for example 'Assets/Screenshots' or 'Captures'). Selecting the project root would mix screenshots in with your project files.", Values("Please pick a subfolder of the project (for example 'Assets/Screenshots' or 'Captures'). Selecting the project root would mix screenshots in with your project files.", "プロジェクトのサブフォルダー（例: 'Assets/Screenshots' または 'Captures'）を選択してください。プロジェクトルートを選ぶとスクリーンショットがプロジェクトファイルと混在します。", "請選取專案的子資料夾（例如 'Assets/Screenshots' 或 'Captures'）。選取專案根目錄會使螢幕擷取畫面與專案檔案混在一起。", "请选择项目的子文件夹（例如 'Assets/Screenshots' 或 'Captures'）。选择项目根目录会使截图与项目文件混在一起。") },
            { "Folder Outside Project", Values("Folder Outside Project", "プロジェクト外のフォルダー", "資料夾位於專案外", "文件夹位于项目外") },
            { "The selected folder is outside the Unity project root.\n\nPicked: {0}\nProject: {1}\n\nPlease pick a folder inside the project.", Values("The selected folder is outside the Unity project root.\n\nPicked: {0}\nProject: {1}\n\nPlease pick a folder inside the project.", "選択したフォルダーは Unity プロジェクトルートの外部です。\n\n選択: {0}\nプロジェクト: {1}\n\nプロジェクト内のフォルダーを選択してください。", "選取的資料夾位於 Unity 專案根目錄之外。\n\n選取：{0}\n專案：{1}\n\n請選取專案內的資料夾。", "选择的文件夹位于 Unity 项目根目录之外。\n\n选择：{0}\n项目：{1}\n\n请选择项目内的文件夹。") },
            { "Select MCPForUnity folder", Values("Select MCPForUnity folder", "MCPForUnity フォルダーを選択", "選取 MCPForUnity 資料夾", "选择 MCPForUnity 文件夹") },
            { "Source set: {0}", Values("Source set: {0}", "ソースを設定しました: {0}", "來源已設定：{0}", "来源已设置：{0}") },
            { "Source selection failed", Values("Source selection failed", "ソースの選択に失敗しました", "來源選取失敗", "来源选择失败") },
            { "Source cleared", Values("Source cleared", "ソースをクリアしました", "來源已清除", "来源已清除") },
            { "Deployment completed.", Values("Deployment completed.", "配置が完了しました。", "部署已完成。", "部署已完成。") },
            { "Restore completed.", Values("Restore completed.", "復元が完了しました。", "還原已完成。", "还原已完成。") },
            { "\nBackup: {0}", Values("\nBackup: {0}", "\nバックアップ: {0}", "\n備份：{0}", "\n备份：{0}") },
            { "Configured", Values("Configured", "設定済み", "已設定", "已配置") },
            { "Running", Values("Running", "実行中", "執行中", "运行中") },
            { "Incorrect Path", Values("Incorrect Path", "パスが正しくありません", "路徑不正確", "路径不正确") },
            { "Communication Error", Values("Communication Error", "通信エラー", "通訊錯誤", "通信错误") },
            { "No Response", Values("No Response", "応答なし", "無回應", "无响应") },
            { "Unsupported OS", Values("Unsupported OS", "未対応の OS", "不支援的作業系統", "不支持的操作系统") },
            { "Missing MCPForUnity Config", Values("Missing MCPForUnity Config", "MCPForUnity 設定がありません", "缺少 MCPForUnity 設定", "缺少 MCPForUnity 配置") },
            { "Version Mismatch", Values("Version Mismatch", "バージョン不一致", "版本不相符", "版本不匹配") },
            { "Transport Mismatch", Values("Transport Mismatch", "トランスポート不一致", "傳輸方式不相符", "传输方式不匹配") },
            { "Configuration steps not available for this client.", Values("Configuration steps not available for this client.", "このクライアントの設定手順はありません。", "沒有此用戶端的設定步驟。", "没有此客户端的配置步骤。") },
            { "Not found - click Browse to select", Values("Not found - click Browse to select", "見つかりません — 参照をクリックして選択してください", "找不到 — 請按一下「瀏覽」選取", "未找到 — 请单击“浏览”选择") },
            { "  (override)", Values("  (override)", "  (上書き)", "  （覆寫）", "  （覆盖）") },
            { "{0} detected client(s) processed. ({1} not installed, skipped.)", Values("{0} detected client(s) processed. ({1} not installed, skipped.)", "検出した {0} 件のクライアントを処理しました。（{1} 件は未インストールのためスキップ）", "已處理 {0} 個偵測到的用戶端。（{1} 個尚未安裝，已略過）", "已处理 {0} 个检测到的客户端。（{1} 个未安装，已跳过）") },
            { "✓ {0} configured, ⚠ {1} failed, ➜ {2} skipped", Values("✓ {0} configured, ⚠ {1} failed, ➜ {2} skipped", "✓ {0} 件設定、⚠ {1} 件失敗、➜ {2} 件スキップ", "✓ 已設定 {0} 個，⚠ {1} 個失敗，➜ 略過 {2} 個", "✓ 已配置 {0} 个，⚠ {1} 个失败，➜ 跳过 {2} 个") },
            { "✓ {0}: Configured successfully", Values("✓ {0}: Configured successfully", "✓ {0}: 設定に成功しました", "✓ {0}：設定成功", "✓ {0}：配置成功") },
            { "Configure Detected Clients", Values("Configure Detected Clients", "検出したクライアントを設定", "設定偵測到的用戶端", "配置检测到的客户端") },
            { "Unregister", Values("Unregister", "登録解除", "取消註冊", "取消注册") },
            { "Unregistering...", Values("Unregistering...", "登録解除中...", "正在取消註冊……", "正在取消注册……") },
            { "Configuring...", Values("Configuring...", "設定中...", "正在設定……", "正在配置……") },
            { "Checking...", Values("Checking...", "確認中...", "正在檢查……", "正在检查……") },
            { "Syncing...", Values("Syncing...", "同期中...", "正在同步……", "正在同步……") },
            { "Skills are already up to date.", Values("Skills are already up to date.", "スキルはすでに最新です。", "技能已是最新版本。", "技能已是最新版本。") },
            { "Added: {0}, Updated: {1}, Deleted: {2}", Values("Added: {0}, Updated: {1}, Deleted: {2}", "追加: {0}、更新: {1}、削除: {2}", "新增：{0}，更新：{1}，刪除：{2}", "新增：{0}，更新：{1}，删除：{2}") },
            { "{0}\n\nInstalled at: {1}", Values("{0}\n\nInstalled at: {1}", "{0}\n\nインストール先: {1}", "{0}\n\n安裝位置：{1}", "{0}\n\n安装位置：{1}") },
            { "Select Claude CLI", Values("Select Claude CLI", "Claude CLI を選択", "選取 Claude CLI", "选择 Claude CLI") },
            { "Select Client Project Directory", Values("Select Client Project Directory", "クライアントのプロジェクトディレクトリを選択", "選取用戶端專案目錄", "选择客户端项目目录") },
            { "The selected directory does not exist.", Values("The selected directory does not exist.", "選択したディレクトリは存在しません。", "選取的目錄不存在。", "选择的目录不存在。") },
            { "The configuration file path does not exist.", Values("The configuration file path does not exist.", "設定ファイルのパスが存在しません。", "設定檔路徑不存在。", "配置文件路径不存在。") },
            { "When enabled, register project-scoped tools with HTTP Local and stdio transports. Allows per-project tool customization.", Values("When enabled, register project-scoped tools with HTTP Local and stdio transports. Allows per-project tool customization.", "有効にすると、プロジェクト固有ツールを HTTP Local と stdio トランスポートに登録します。プロジェクトごとのツールカスタマイズが可能になります。", "啟用後，會向 HTTP Local 和 stdio 傳輸註冊專案範圍工具，允許各專案自訂工具。", "启用后，会向 HTTP Local 和 stdio 传输注册项目范围工具，允许各项目自定义工具。") },
            { "Stdio mode: toggles sync at startup. After changing toggles, ask the AI to run manage_tools with action 'sync' to refresh.", Values("Stdio mode: toggles sync at startup. After changing toggles, ask the AI to run manage_tools with action 'sync' to refresh.", "Stdio モード: トグルは起動時に同期されます。変更後は AI に manage_tools の action 'sync' を実行するよう依頼して更新してください。", "Stdio 模式：切換狀態會在啟動時同步。變更後，請讓 AI 執行 manage_tools 的 'sync' 動作以重新整理。", "Stdio 模式：切换状态会在启动时同步。更改后，请让 AI 执行 manage_tools 的 'sync' 操作以刷新。") },
            { "Core Tools", Values("Core Tools", "コアツール", "核心工具", "核心工具") },
            { "VFX & Shaders", Values("VFX & Shaders", "VFX とシェーダー", "VFX 與著色器", "VFX 与着色器") },
            { "Animation", Values("Animation", "アニメーション", "動畫", "动画") },
            { "UI Toolkit", Values("UI Toolkit", "UI Toolkit", "UI Toolkit", "UI Toolkit") },
            { "Scripting Extensions", Values("Scripting Extensions", "スクリプト拡張", "指令碼擴充", "脚本扩展") },
            { "Testing", Values("Testing", "テスト", "測試", "测试") },
            { "ProBuilder — Experimental", Values("ProBuilder — Experimental", "ProBuilder — 試験的", "ProBuilder — 實驗性", "ProBuilder — 实验性") },
            { "Profiling & Frame Debugger", Values("Profiling & Frame Debugger", "プロファイリングとフレームデバッガー", "效能分析與影格偵錯工具", "性能分析与帧调试器") },
            { "Asset Gen", Values("Asset Gen", "アセット生成", "資產生成", "资源生成") },
            { "Custom Tools", Values("Custom Tools", "カスタムツール", "自訂工具", "自定义工具") },
            { "Other", Values("Other", "その他", "其他", "其他") },
            { "{0} ({1}/{2})", Values("{0} ({1}/{2})", "{0} ({1}/{2})", "{0}（{1}/{2}）", "{0}（{1}/{2}）") },
            { "Toggle all tools in \"{0}\" on or off.", Values("Toggle all tools in \"{0}\" on or off.", "「{0}」のすべてのツールを有効または無効にします。", "開啟或關閉「{0}」中的所有工具。", "开启或关闭“{0}”中的所有工具。") },
            { "ProBuilder support is experimental. Mesh editing operations may produce unexpected results on complex topologies. Always save your scene before performing destructive operations.", Values("ProBuilder support is experimental. Mesh editing operations may produce unexpected results on complex topologies. Always save your scene before performing destructive operations.", "ProBuilder 対応は試験的です。複雑なトポロジーではメッシュ編集が予期しない結果になることがあります。破壊的な操作を行う前に必ずシーンを保存してください。", "ProBuilder 支援仍屬實驗性功能。網格編輯在複雜拓撲上可能產生非預期結果。執行破壞性操作前請務必儲存場景。", "ProBuilder 支持仍属于实验性功能。网格编辑在复杂拓扑上可能产生意外结果。执行破坏性操作前请务必保存场景。") },
            { "Polling: {0}", Values("Polling: {0}", "ポーリング: {0}", "輪詢：{0}", "轮询：{0}") },
            { "✓ {0}: Reconfigured", Values("✓ {0}: Reconfigured", "✓ {0}: 再設定しました", "✓ {0}：已重新設定", "✓ {0}：已重新配置") },
            { "Reconfigured {0} client(s), skipped {1}.", Values("Reconfigured {0} client(s), skipped {1}.", "{0} 件のクライアントを再設定し、{1} 件をスキップしました。", "已重新設定 {0} 個用戶端，略過 {1} 個。", "已重新配置 {0} 个客户端，跳过 {1} 个。") },
            { "Reconfigure Failed", Values("Reconfigure Failed", "再設定に失敗", "重新設定失敗", "重新配置失败") },
            { "{0} of {1} tools will register with connected clients.", Values("{0} of {1} tools will register with connected clients.", "{1} 個中 {0} 個のツールが接続済みクライアントに登録されます。", "將向已連線的用戶端註冊 {1} 個工具中的 {0} 個。", "将向已连接的客户端注册 {1} 个工具中的 {0} 个。") },
            { "No MCP tools found. Add classes decorated with [McpForUnityTool] to expose tools.", Values("No MCP tools found. Add classes decorated with [McpForUnityTool] to expose tools.", "MCP ツールが見つかりません。[McpForUnityTool] で装飾したクラスを追加してツールを公開してください。", "找不到 MCP 工具。請新增以 [McpForUnityTool] 裝飾的類別以公開工具。", "未找到 MCP 工具。请添加使用 [McpForUnityTool] 标记的类以公开工具。") },
            { "Game View", Values("Game View", "ゲームビュー", "遊戲檢視", "游戏视图") },
            { "Scene View", Values("Scene View", "シーンビュー", "場景檢視", "场景视图") },
            { "Multiview", Values("Multiview", "マルチビュー", "多視角", "多视角") },
            { "Capture a game camera screenshot. Default: Assets/Screenshots (configurable in Advanced).", Values("Capture a game camera screenshot. Default: Assets/Screenshots (configurable in Advanced).", "ゲームカメラのスクリーンショットを撮影します。既定: Assets/Screenshots（詳細設定で変更可能）。", "擷取遊戲攝影機畫面。預設：Assets/Screenshots（可在進階設定中變更）。", "捕获游戏相机画面。默认：Assets/Screenshots（可在高级设置中更改）。") },
            { "Capture the active Scene View viewport. Default: Assets/Screenshots (configurable in Advanced).", Values("Capture the active Scene View viewport. Default: Assets/Screenshots (configurable in Advanced).", "アクティブなシーンビューのビューポートを撮影します。既定: Assets/Screenshots（詳細設定で変更可能）。", "擷取目前的場景檢視視埠。預設：Assets/Screenshots（可在進階設定中變更）。", "捕获当前场景视图视口。默认：Assets/Screenshots（可在高级设置中更改）。") },
            { "Capture a 6-angle contact sheet around the scene centre. Default: Assets/Screenshots (configurable in Advanced).", Values("Capture a 6-angle contact sheet around the scene centre. Default: Assets/Screenshots (configurable in Advanced).", "シーン中央を 6 方向から撮影したコンタクトシートを作成します。既定: Assets/Screenshots（詳細設定で変更可能）。", "擷取場景中心周圍六個角度的接觸表。預設：Assets/Screenshots（可在進階設定中變更）。", "捕获场景中心周围六个角度的联系表。默认：Assets/Screenshots（可在高级设置中更改）。") },
            { "Number of commands allowed per batch_execute call (1–{0}). Default: {1}.", Values("Number of commands allowed per batch_execute call (1–{0}). Default: {1}.", "batch_execute 1 回あたりに許可するコマンド数 (1–{0})。既定: {1}。", "每次 batch_execute 呼叫允許的命令數（1–{0}）。預設：{1}。", "每次 batch_execute 调用允许的命令数（1–{0}）。默认：{1}。") },
            { "(max {0})", Values("(max {0})", "(最大 {0})", "（上限 {0}）", "（上限 {0}）") },
            { "Built-in Resources", Values("Built-in Resources", "組み込みリソース", "內建資源", "内置资源") },
            { "Custom Resources", Values("Custom Resources", "カスタムリソース", "自訂資源", "自定义资源") },
            { "{0} ({1})", Values("{0} ({1})", "{0} ({1})", "{0}（{1}）", "{0}（{1}）") },
            { "Built-in", Values("Built-in", "組み込み", "內建", "内置") },
            { "Custom", Values("Custom", "カスタム", "自訂", "自定义") },
            { "No MCP resources found. Add classes decorated with [McpForUnityResource] to expose resources.", Values("No MCP resources found. Add classes decorated with [McpForUnityResource] to expose resources.", "MCP リソースが見つかりません。[McpForUnityResource] で装飾したクラスを追加してリソースを公開してください。", "找不到 MCP 資源。請新增以 [McpForUnityResource] 裝飾的類別以公開資源。", "未找到 MCP 资源。请添加使用 [McpForUnityResource] 标记的类以公开资源。") },
            { "No custom resources detected in loaded assemblies.", Values("No custom resources detected in loaded assemblies.", "読み込まれたアセンブリにカスタムリソースが見つかりません。", "載入的組件中未偵測到自訂資源。", "加载的程序集中未检测到自定义资源。") },
            { "{0} of {1} resources enabled.", Values("{0} of {1} resources enabled.", "{1} 個中 {0} 個のリソースが有効です。", "已啟用 {1} 個資源中的 {0} 個。", "已启用 {1} 个资源中的 {0} 个。") },
            { "3D Models", Values("3D Models", "3D モデル", "3D 模型", "3D 模型") },
            { "2D Images", Values("2D Images", "2D 画像", "2D 圖像", "2D 图像") },
            { "Sound (fal.ai)", Values("Sound (fal.ai)", "サウンド (fal.ai)", "音訊（fal.ai）", "音频（fal.ai）") },
            { "Blender → Unity Handoff", Values("Blender → Unity Handoff", "Blender → Unity 受け渡し", "Blender → Unity 交接", "Blender → Unity 交接") },
            { "Pair Blender with the BlenderMCP server in your AI client, then run the blender-to-unity skill to export the current model — it imports via the import_model_file tool. (BlenderMCP is configured in your AI client and can't be detected here.)", Values("Pair Blender with the BlenderMCP server in your AI client, then run the blender-to-unity skill to export the current model — it imports via the import_model_file tool. (BlenderMCP is configured in your AI client and can't be detected here.)", "AI クライアントで Blender を BlenderMCP サーバーと接続し、blender-to-unity スキルを実行して現在のモデルをエクスポートしてください。モデルは import_model_file ツールを介してインポートされます。（BlenderMCP は AI クライアント側で設定されるため、ここでは検出できません。）", "請在 AI 用戶端中將 Blender 與 BlenderMCP 伺服器配對，接著執行 blender-to-unity 技能匯出目前模型；模型會透過 import_model_file 工具匯入。（BlenderMCP 在 AI 用戶端中設定，因此無法在此偵測。）", "请在 AI 客户端中将 Blender 与 BlenderMCP 服务器配对，然后运行 blender-to-unity 技能导出当前模型；模型会通过 import_model_file 工具导入。（BlenderMCP 在 AI 客户端中配置，因此无法在此检测。）") },
            { "Enabled", Values("Enabled", "有効", "已啟用", "已启用") },
            { "Save", Values("Save", "保存", "儲存", "保存") },
            { "Model", Values("Model", "モデル", "模型", "模型") },
            { "Default container format for generated 3D models.", Values("Default container format for generated 3D models.", "生成する 3D モデルの既定コンテナ形式です。", "生成 3D 模型的預設容器格式。", "生成 3D 模型的默认容器格式。") },
            { "Project-relative folder where generated assets are written. Empty = {0}.", Values("Project-relative folder where generated assets are written. Empty = {0}.", "生成したアセットを書き込むプロジェクト相対フォルダーです。空の場合 = {0}。", "寫入生成資產的專案相對資料夾。留空 = {0}。", "写入生成资源的项目相对文件夹。留空 = {0}。") },
            { "Uniformly scale imported models to the target size on import.", Values("Uniformly scale imported models to the target size on import.", "インポート時にモデルを対象サイズへ均等に拡縮します。", "匯入時將模型等比例縮放至目標尺寸。", "导入时将模型等比例缩放至目标尺寸。") },
            { "Re-check API-key presence and rebuild the provider/model rows. Picks up keys or prefs set elsewhere (CLI, env override). The model list is curated in-package.", Values("Re-check API-key presence and rebuild the provider/model rows. Picks up keys or prefs set elsewhere (CLI, env override). The model list is curated in-package.", "API キーの有無を再確認し、プロバイダー／モデル行を再構築します。他の場所（CLI、環境変数の上書き）で設定したキーや設定も反映します。モデル一覧はパッケージ内で管理されています。", "重新檢查 API 金鑰是否存在，並重建提供者／模型列。會讀取在其他位置（CLI、環境變數覆寫）設定的金鑰或偏好；模型清單由套件內建維護。", "重新检查 API 密钥是否存在，并重建提供商/模型行。会读取在其他位置（CLI、环境变量覆盖）设置的密钥或偏好；模型列表由包内置维护。") },
            { "refreshed — using the built-in model catalog", Values("refreshed — using the built-in model catalog", "更新しました — 組み込みモデルカタログを使用中", "已重新整理 — 正在使用內建模型目錄", "已刷新 — 正在使用内置模型目录") },
            { "Enable the {0} provider for asset generation.", Values("Enable the {0} provider for asset generation.", "アセット生成に {0} プロバイダーを使用します。", "啟用 {0} 提供者來生成資產。", "启用 {0} 提供商来生成资源。") },
            { "Paste your {0} API key, then press Save (or click away). The key is stored in your OS secure store and is never read back into this field.", Values("Paste your {0} API key, then press Save (or click away). The key is stored in your OS secure store and is never read back into this field.", "{0} API キーを貼り付け、保存を押す（またはフィールド外をクリックする）と、OS の安全なストアに保存されます。キーがこのフィールドへ読み戻されることはありません。", "貼上您的 {0} API 金鑰，然後按「儲存」（或點擊欄位外）。金鑰會儲存在作業系統的安全儲存區，且絕不會讀回此欄位。", "粘贴您的 {0} API 密钥，然后按“保存”（或点击字段外）。密钥会存储在操作系统的安全存储区，且绝不会读回此字段。") },
            { "saved ✓", Values("saved ✓", "保存済み ✓", "已儲存 ✓", "已保存 ✓") },
            { "save failed", Values("save failed", "保存に失敗", "儲存失敗", "保存失败") },
            { "not set", Values("not set", "未設定", "未設定", "未设置") },
            { "key present ✓", Values("key present ✓", "キーあり ✓", "已有金鑰 ✓", "已有密钥 ✓") },
            { "no key set", Values("no key set", "キー未設定", "未設定金鑰", "未设置密钥") },
            { "The model generate_* uses for this provider when no explicit model is passed.", Values("The model generate_* uses for this provider when no explicit model is passed.", "モデルが明示されていない場合に generate_* がこのプロバイダーで使用するモデルです。", "未明確指定模型時，generate_* 對此提供者使用的模型。", "未明确指定模型时，generate_* 对此提供商使用的模型。") },
            { "key present ✓ (shared with 2D fal)", Values("key present ✓ (shared with 2D fal)", "キーあり ✓（2D fal と共有）", "已有金鑰 ✓（與 2D fal 共用）", "已有密钥 ✓（与 2D fal 共用）") },
            { "no fal key — set it in 2D Images", Values("no fal key — set it in 2D Images", "fal キーがありません — 2D 画像で設定してください", "沒有 fal 金鑰 — 請在「2D 圖像」中設定", "没有 fal 密钥 — 请在“2D 图像”中设置") },
            { "Basic", Values("Basic", "基本", "基本", "基础") },
            { "Standard", Values("Standard", "標準", "標準", "标准") },
            { "Comprehensive", Values("Comprehensive", "包括的", "完整", "全面") },
            { "Strict", Values("Strict", "厳格", "嚴格", "严格") },
            { "Basic: Validates syntax only. Fast compilation checks.", Values("Basic: Validates syntax only. Fast compilation checks.", "基本: 構文のみを検証します。高速なコンパイルチェックです。", "基本：僅驗證語法，快速進行編譯檢查。", "基础：仅验证语法，快速进行编译检查。") },
            { "Standard (Recommended): Checks syntax + common errors. Balanced speed and coverage.", Values("Standard (Recommended): Checks syntax + common errors. Balanced speed and coverage.", "標準（推奨）: 構文と一般的なエラーを確認します。速度と範囲のバランスを取ります。", "標準（建議）：檢查語法與常見錯誤，兼顧速度和涵蓋範圍。", "标准（推荐）：检查语法和常见错误，兼顾速度和覆盖范围。") },
            { "Comprehensive: Detailed validation including code quality. Slower but thorough.", Values("Comprehensive: Detailed validation including code quality. Slower but thorough.", "包括的: コード品質を含む詳細な検証を行います。低速ですが徹底的です。", "完整：執行包含程式碼品質的詳細驗證，速度較慢但更徹底。", "全面：执行包含代码质量的详细验证，速度较慢但更彻底。") },
            { "Strict: Maximum validation + warnings as errors. Slowest but catches all issues.", Values("Strict: Maximum validation + warnings as errors. Slowest but catches all issues.", "厳格: 最大限の検証を行い、警告もエラーとして扱います。最も低速ですが、すべての問題を検出します。", "嚴格：執行最高層級驗證，並將警告視為錯誤。速度最慢，但能發現所有問題。", "严格：执行最高级别验证，并将警告视为错误。速度最慢，但能发现所有问题。") },
            { "Unknown validation level", Values("Unknown validation level", "不明な検証レベル", "未知的驗證層級", "未知的验证级别") },
            { "Roslyn Already Installed", Values("Roslyn Already Installed", "Roslyn はインストール済みです", "Roslyn 已安裝", "Roslyn 已安装") },
            { "Roslyn DLLs are already present in Assets/{0}.\nReinstall?", Values("Roslyn DLLs are already present in Assets/{0}.\nReinstall?", "Roslyn DLL はすでに Assets/{0} にあります。\n再インストールしますか？", "Roslyn DLL 已存在於 Assets/{0}。\n要重新安裝嗎？", "Roslyn DLL 已存在于 Assets/{0}。\n是否重新安装？") },
            { "Reinstall", Values("Reinstall", "再インストール", "重新安裝", "重新安装") },
            { "Installing Roslyn", Values("Installing Roslyn", "Roslyn をインストール中", "正在安裝 Roslyn", "正在安装 Roslyn") },
            { "Downloading {0} v{1}...", Values("Downloading {0} v{1}...", "{0} v{1} をダウンロード中...", "正在下載 {0} v{1}……", "正在下载 {0} v{1}……") },
            { "Refreshing assets...", Values("Refreshing assets...", "アセットを更新中...", "正在重新整理資產……", "正在刷新资源……") },
            { "Roslyn Installed", Values("Roslyn Installed", "Roslyn をインストールしました", "Roslyn 已安裝", "Roslyn 已安装") },
            { "Roslyn DLLs and dependencies installed to Assets/{0}/.\n\nThe runtime_compilation tool is now available via MCP.", Values("Roslyn DLLs and dependencies installed to Assets/{0}/.\n\nThe runtime_compilation tool is now available via MCP.", "Roslyn DLL と依存関係を Assets/{0}/ にインストールしました。\n\nruntime_compilation ツールが MCP から利用可能になりました。", "Roslyn DLL 與相依套件已安裝至 Assets/{0}/。\n\n現在可透過 MCP 使用 runtime_compilation 工具。", "Roslyn DLL 和依赖已安装到 Assets/{0}/。\n\n现在可通过 MCP 使用 runtime_compilation 工具。") },
            { "Installation Failed", Values("Installation Failed", "インストールに失敗", "安裝失敗", "安装失败") },
            { "Could not download Roslyn DLLs:\n{0}\n\nYou can manually download Microsoft.CodeAnalysis.CSharp from NuGet and place the DLLs in Assets/Plugins/Roslyn/.", Values("Could not download Roslyn DLLs:\n{0}\n\nYou can manually download Microsoft.CodeAnalysis.CSharp from NuGet and place the DLLs in Assets/Plugins/Roslyn/.", "Roslyn DLL をダウンロードできませんでした:\n{0}\n\nNuGet から Microsoft.CodeAnalysis.CSharp を手動でダウンロードし、DLL を Assets/Plugins/Roslyn/ に配置できます。", "無法下載 Roslyn DLL：\n{0}\n\n您可以從 NuGet 手動下載 Microsoft.CodeAnalysis.CSharp，並將 DLL 放入 Assets/Plugins/Roslyn/。", "无法下载 Roslyn DLL：\n{0}\n\n您可以从 NuGet 手动下载 Microsoft.CodeAnalysis.CSharp，并将 DLL 放入 Assets/Plugins/Roslyn/。") },
            { "The server command could not be constructed with the current settings.", Values("The server command could not be constructed with the current settings.", "現在の設定ではサーバーコマンドを構築できませんでした。", "無法使用目前設定建立伺服器命令。", "无法使用当前设置构建服务器命令。") },
            { "Port In Use", Values("Port In Use", "ポートは使用中です", "連接埠使用中", "端口正在使用") },
            { "Cannot start the local HTTP server because port {0} is already in use by PID(s): {1}\n\n{2} will not terminate unrelated processes. Stop the owning process manually or change the HTTP URL.", Values("Cannot start the local HTTP server because port {0} is already in use by PID(s): {1}\n\n{2} will not terminate unrelated processes. Stop the owning process manually or change the HTTP URL.", "ポート {0} が PID {1} によって使用されているため、ローカル HTTP サーバーを起動できません。\n\n{2} は無関係なプロセスを終了しません。所有プロセスを手動で停止するか、HTTP URL を変更してください。", "連接埠 {0} 已由 PID {1} 使用，因此無法啟動本機 HTTP 伺服器。\n\n{2} 不會終止無關的處理程序。請手動停止佔用的處理程序，或變更 HTTP URL。", "端口 {0} 已被 PID {1} 占用，因此无法启动本地 HTTP 服务器。\n\n{2} 不会终止无关进程。请手动停止占用的进程，或更改 HTTP URL。") },
            { "Start Local HTTP Server", Values("Start Local HTTP Server", "ローカル HTTP サーバーを起動", "啟動本機 HTTP 伺服器", "启动本地 HTTP 服务器") },
            { "Start the local MCP server in the background?\n\nIt launches headless (no terminal window) and logs progress to the Unity Console. This confirmation is shown only once.", Values("Start the local MCP server in the background?\n\nIt launches headless (no terminal window) and logs progress to the Unity Console. This confirmation is shown only once.", "ローカル MCP サーバーをバックグラウンドで起動しますか？\n\nターミナルウィンドウなしで起動し、進行状況を Unity Console に記録します。この確認は一度だけ表示されます。", "要在背景啟動本機 MCP 伺服器嗎？\n\n它會以無介面模式啟動（不開啟終端機視窗），並將進度記錄至 Unity Console。此確認只會顯示一次。", "是否在后台启动本地 MCP 服务器？\n\n它将以无界面模式启动（不打开终端窗口），并将进度记录到 Unity Console。此确认只显示一次。") },
            { "Failed to start server: {0}", Values("Failed to start server: {0}", "サーバーの起動に失敗しました: {0}", "啟動伺服器失敗：{0}", "启动服务器失败：{0}") },
            { "Found Python {0} in PATH", Values("Found Python {0} in PATH", "PATH で Python {0} が見つかりました", "在 PATH 中找到 Python {0}", "在 PATH 中找到 Python {0}") },
            { "Found Python {0} via uv", Values("Found Python {0} via uv", "uv 経由で Python {0} が見つかりました", "透過 uv 找到 Python {0}", "通过 uv 找到 Python {0}") },
            { "Found Python {0} at {1}", Values("Found Python {0} at {1}", "{1} で Python {0} が見つかりました", "在 {1} 找到 Python {0}", "在 {1} 找到 Python {0}") },
            { "Python not found in PATH", Values("Python not found in PATH", "PATH に Python が見つかりません", "PATH 中找不到 Python", "PATH 中未找到 Python") },
            { "Python not found in PATH or standard locations", Values("Python not found in PATH or standard locations", "PATH または標準の場所に Python が見つかりません", "PATH 或標準位置中找不到 Python", "PATH 或标准位置中未找到 Python") },
            { "Install Python 3.10+ and ensure it's added to PATH.", Values("Install Python 3.10+ and ensure it's added to PATH.", "Python 3.10 以降をインストールし、PATH に追加されていることを確認してください。", "請安裝 Python 3.10 以上版本，並確認已加入 PATH。", "请安装 Python 3.10 及以上版本，并确保已加入 PATH。") },
            { "Install Python 3.10+ via Homebrew ('brew install python3') and ensure it's in your PATH.", Values("Install Python 3.10+ via Homebrew ('brew install python3') and ensure it's in your PATH.", "Homebrew（'brew install python3'）で Python 3.10 以降をインストールし、PATH に含まれていることを確認してください。", "請透過 Homebrew（'brew install python3'）安裝 Python 3.10 以上版本，並確認已加入 PATH。", "请通过 Homebrew（'brew install python3'）安装 Python 3.10 及以上版本，并确保已加入 PATH。") },
            { "Error detecting Python: {0}", Values("Error detecting Python: {0}", "Python の検出中にエラーが発生しました: {0}", "偵測 Python 時發生錯誤：{0}", "检测 Python 时出错：{0}") },
            { "Found uv {0} at {1}", Values("Found uv {0} at {1}", "{1} で uv {0} が見つかりました", "在 {1} 找到 uv {0}", "在 {1} 找到 uv {0}") },
            { "Found uvx {0} at {1} (fallback)", Values("Found uvx {0} at {1} (fallback)", "{1} で uvx {0} が見つかりました（フォールバック）", "在 {1} 找到 uvx {0}（備用）", "在 {1} 找到 uvx {0}（备用）") },
            { "Found uv {0} in PATH", Values("Found uv {0} in PATH", "PATH で uv {0} が見つかりました", "在 PATH 中找到 uv {0}", "在 PATH 中找到 uv {0}") },
            { "Found uv {0} (fallback to system path)", Values("Found uv {0} (fallback to system path)", "uv {0} が見つかりました（システムパスへフォールバック）", "找到 uv {0}（改用系統路徑）", "找到 uv {0}（改用系统路径）") },
            { "Found uv {0} (override path)", Values("Found uv {0} (override path)", "uv {0} が見つかりました（上書きパス）", "找到 uv {0}（覆寫路徑）", "找到 uv {0}（覆盖路径）") },
            { "Found uv {0} in system path", Values("Found uv {0} in system path", "システムパスで uv {0} が見つかりました", "在系統路徑中找到 uv {0}", "在系统路径中找到 uv {0}") },
            { "Override path not found, using system path", Values("Override path not found, using system path", "上書きパスが見つからないため、システムパスを使用します", "找不到覆寫路徑，改用系統路徑", "未找到覆盖路径，改用系统路径") },
            { "uv not found in PATH", Values("uv not found in PATH", "PATH に uv が見つかりません", "PATH 中找不到 uv", "PATH 中未找到 uv") },
            { "uvx not found", Values("uvx not found", "uvx が見つかりません", "找不到 uvx", "未找到 uvx") },
            { "Install uv package manager and ensure it's added to PATH.", Values("Install uv package manager and ensure it's added to PATH.", "uv パッケージマネージャーをインストールし、PATH に追加されていることを確認してください。", "請安裝 uv 套件管理員，並確認已加入 PATH。", "请安装 uv 包管理器，并确保已加入 PATH。") },
            { "Install uv package manager or configure path override in Advanced Settings.", Values("Install uv package manager or configure path override in Advanced Settings.", "uv パッケージマネージャーをインストールするか、詳細設定でパスを上書きしてください。", "請安裝 uv 套件管理員，或在進階設定中設定覆寫路徑。", "请安装 uv 包管理器，或在高级设置中配置覆盖路径。") },
            { "Error detecting uv: {0}", Values("Error detecting uv: {0}", "uv の検出中にエラーが発生しました: {0}", "偵測 uv 時發生錯誤：{0}", "检测 uv 时出错：{0}") },
            { "Error detecting uvx: {0}", Values("Error detecting uvx: {0}", "uvx の検出中にエラーが発生しました: {0}", "偵測 uvx 時發生錯誤：{0}", "检测 uvx 时出错：{0}") },
            { "Windows Installation Recommendations:", Values("Windows Installation Recommendations:", "Windows のインストール推奨事項:", "Windows 安裝建議：", "Windows 安装建议：") },
            { "macOS Installation Recommendations:", Values("macOS Installation Recommendations:", "macOS のインストール推奨事項:", "macOS 安裝建議：", "macOS 安装建议：") },
            { "Linux Installation Recommendations:", Values("Linux Installation Recommendations:", "Linux のインストール推奨事項:", "Linux 安裝建議：", "Linux 安装建议：") },
            { "1. Python: Install from Microsoft Store or python.org", Values("1. Python: Install from Microsoft Store or python.org", "1. Python: Microsoft Store または python.org からインストール", "1. Python：從 Microsoft Store 或 python.org 安裝", "1. Python：从 Microsoft Store 或 python.org 安装") },
            { "   - Microsoft Store: Search for 'Python 3.10' or higher", Values("   - Microsoft Store: Search for 'Python 3.10' or higher", "   - Microsoft Store: 'Python 3.10' 以降を検索", "   - Microsoft Store：搜尋 'Python 3.10' 或更高版本", "   - Microsoft Store：搜索 'Python 3.10' 或更高版本") },
            { "1. Python: Install via Homebrew (recommended) or python.org", Values("1. Python: Install via Homebrew (recommended) or python.org", "1. Python: Homebrew（推奨）または python.org からインストール", "1. Python：透過 Homebrew（建議）或 python.org 安裝", "1. Python：通过 Homebrew（推荐）或 python.org 安装") },
            { "1. Python: Install via package manager or pyenv", Values("1. Python: Install via package manager or pyenv", "1. Python: パッケージマネージャーまたは pyenv でインストール", "1. Python：透過套件管理員或 pyenv 安裝", "1. Python：通过包管理器或 pyenv 安装") },
            { "2. uv Package Manager: Install via PowerShell", Values("2. uv Package Manager: Install via PowerShell", "2. uv パッケージマネージャー: PowerShell でインストール", "2. uv 套件管理員：透過 PowerShell 安裝", "2. uv 包管理器：通过 PowerShell 安装") },
            { "2. uv Package Manager: Install via curl or Homebrew", Values("2. uv Package Manager: Install via curl or Homebrew", "2. uv パッケージマネージャー: curl または Homebrew でインストール", "2. uv 套件管理員：透過 curl 或 Homebrew 安裝", "2. uv 包管理器：通过 curl 或 Homebrew 安装") },
            { "2. uv Package Manager: Install via curl", Values("2. uv Package Manager: Install via curl", "2. uv パッケージマネージャー: curl でインストール", "2. uv 套件管理員：透過 curl 安裝", "2. uv 包管理器：通过 curl 安装") },
            { "3. MCP Server: Will be installed automatically by MCP for Unity Bridge", Values("3. MCP Server: Will be installed automatically by MCP for Unity Bridge", "3. MCP サーバー: MCP for Unity Bridge により自動的にインストールされます", "3. MCP 伺服器：將由 MCP for Unity Bridge 自動安裝", "3. MCP 服务器：将由 MCP for Unity Bridge 自动安装") },
            { "3. MCP Server: Will be installed automatically by MCP for Unity", Values("3. MCP Server: Will be installed automatically by MCP for Unity", "3. MCP サーバー: MCP for Unity により自動的にインストールされます", "3. MCP 伺服器：將由 MCP for Unity 自動安裝", "3. MCP 服务器：将由 MCP for Unity 自动安装") },
            { "   - Direct download: https://python.org/downloads/windows/", Values("   - Direct download: https://python.org/downloads/windows/", "   - 直接ダウンロード: https://python.org/downloads/windows/", "   - 直接下載：https://python.org/downloads/windows/", "   - 直接下载：https://python.org/downloads/windows/") },
            { "   - Direct download: https://python.org/downloads/macos/", Values("   - Direct download: https://python.org/downloads/macos/", "   - 直接ダウンロード: https://python.org/downloads/macos/", "   - 直接下載：https://python.org/downloads/macos/", "   - 直接下载：https://python.org/downloads/macos/") },
            { "   - Or download from: https://github.com/astral-sh/uv/releases", Values("   - Or download from: https://github.com/astral-sh/uv/releases", "   - または次からダウンロード: https://github.com/astral-sh/uv/releases", "   - 或從此處下載：https://github.com/astral-sh/uv/releases", "   - 或从此处下载：https://github.com/astral-sh/uv/releases") },
            { "   - Or use pyenv: https://github.com/pyenv/pyenv", Values("   - Or use pyenv: https://github.com/pyenv/pyenv", "   - または pyenv を使用: https://github.com/pyenv/pyenv", "   - 或使用 pyenv：https://github.com/pyenv/pyenv", "   - 或使用 pyenv：https://github.com/pyenv/pyenv") },
            { "   - Homebrew: brew install python3", Values("   - Homebrew: brew install python3", "   - Homebrew: brew install python3", "   - Homebrew：brew install python3", "   - Homebrew：brew install python3") },
            { "   - Homebrew: brew install uv", Values("   - Homebrew: brew install uv", "   - Homebrew: brew install uv", "   - Homebrew：brew install uv", "   - Homebrew：brew install uv") },
            { "   - Curl: curl -LsSf https://astral.sh/uv/install.sh | sh", Values("   - Curl: curl -LsSf https://astral.sh/uv/install.sh | sh", "   - Curl: curl -LsSf https://astral.sh/uv/install.sh | sh", "   - Curl：curl -LsSf https://astral.sh/uv/install.sh | sh", "   - Curl：curl -LsSf https://astral.sh/uv/install.sh | sh") },
            { "Note: If using Homebrew, make sure /opt/homebrew/bin is in your PATH.", Values("Note: If using Homebrew, make sure /opt/homebrew/bin is in your PATH.", "注: Homebrew を使用する場合は、/opt/homebrew/bin が PATH に含まれていることを確認してください。", "注意：若使用 Homebrew，請確認 /opt/homebrew/bin 已加入 PATH。", "注意：如使用 Homebrew，请确保 /opt/homebrew/bin 已加入 PATH。") },
            { "Note: Make sure ~/.local/bin is in your PATH for user-local installations.", Values("Note: Make sure ~/.local/bin is in your PATH for user-local installations.", "注: ユーザー単位のインストールでは、~/.local/bin が PATH に含まれていることを確認してください。", "注意：使用者本機安裝時，請確認 ~/.local/bin 已加入 PATH。", "注意：用户本地安装时，请确保 ~/.local/bin 已加入 PATH。") },
            { "Deploy MCP for Unity", Values("Deploy MCP for Unity", "MCP for Unity を配置", "部署 MCP for Unity", "部署 MCP for Unity") },
            { "Creating backup...", Values("Creating backup...", "バックアップを作成中...", "正在建立備份……", "正在创建备份……") },
            { "Replacing package contents...", Values("Replacing package contents...", "パッケージ内容を置換中...", "正在取代套件內容……", "正在替换包内容……") },
            { "Restore MCP for Unity", Values("Restore MCP for Unity", "MCP for Unity を復元", "還原 MCP for Unity", "还原 MCP for Unity") },
            { "Restoring backup...", Values("Restoring backup...", "バックアップを復元中...", "正在還原備份……", "正在还原备份……") },
            { "Dependency check failed: {0}", Values("Dependency check failed: {0}", "依存関係の確認に失敗しました: {0}", "相依套件檢查失敗：{0}", "依赖检查失败：{0}") },
            { "Error getting installation recommendations: {0}", Values("Error getting installation recommendations: {0}", "インストール推奨事項の取得中にエラーが発生しました: {0}", "取得安裝建議時發生錯誤：{0}", "获取安装建议时出错：{0}") },
            { "All dependencies are available. You can start using MCP for Unity.", Values("All dependencies are available. You can start using MCP for Unity.", "すべての依存関係が利用可能です。MCP for Unity を使用できます。", "所有相依套件皆可用，您可以開始使用 MCP for Unity。", "所有依赖均可用，您可以开始使用 MCP for Unity。") },
            { "Install Python 3.10+ from: {0}", Values("Install Python 3.10+ from: {0}", "Python 3.10 以降のインストール先: {0}", "從此處安裝 Python 3.10 以上版本：{0}", "从此处安装 Python 3.10 及以上版本：{0}") },
            { "Install uv package manager from: {0}", Values("Install uv package manager from: {0}", "uv パッケージマネージャーのインストール先: {0}", "從此處安裝 uv 套件管理員：{0}", "从此处安装 uv 包管理器：{0}") },
            { "MCP Server will be installed automatically when needed.", Values("MCP Server will be installed automatically when needed.", "MCP サーバーは必要なときに自動的にインストールされます。", "MCP 伺服器會在需要時自動安裝。", "MCP 服务器会在需要时自动安装。") },
            { "Use the Setup Window (Window > MCP for Unity > Local Setup Window) for guided installation.", Values("Use the Setup Window (Window > MCP for Unity > Local Setup Window) for guided installation.", "ガイド付きインストールにはセットアップウィンドウ（Window > MCP for Unity > Local Setup Window）を使用してください。", "請使用設定視窗（Window > MCP for Unity > Local Setup Window）進行引導式安裝。", "请使用设置窗口（Window > MCP for Unity > Local Setup Window）进行引导式安装。") },
            { "All dependencies are available and ready.", Values("All dependencies are available and ready.", "すべての依存関係が利用可能で準備完了です。", "所有相依套件皆可用且已就緒。", "所有依赖均可用且已就绪。") },
            { "System is ready. {0} optional dependencies are missing.", Values("System is ready. {0} optional dependencies are missing.", "システムは準備完了です。オプションの依存関係が {0} 件不足しています。", "系統已就緒，但缺少 {0} 個選用相依套件。", "系统已就绪，但缺少 {0} 个可选依赖。") },
            { "System is not ready. {0} required dependencies are missing.", Values("System is not ready. {0} required dependencies are missing.", "システムの準備ができていません。必須の依存関係が {0} 件不足しています。", "系統尚未就緒，缺少 {0} 個必要相依套件。", "系统尚未就绪，缺少 {0} 个必需依赖。") },
            { "MCP Setup", Values("MCP Setup", "MCP セットアップ", "MCP 設定", "MCP 设置") },
            { "General image", Values("General image", "一般画像", "一般圖像", "通用图像") },
            { "Fast / cheap image", Values("Fast / cheap image", "高速／低コスト画像", "快速／低成本圖像", "快速/低成本图像") },
            { "Top-quality image", Values("Top-quality image", "最高品質の画像", "最高品質圖像", "最高质量图像") },
            { "Text / image -> 3D", Values("Text / image -> 3D", "テキスト／画像 → 3D", "文字／圖像 → 3D", "文本/图像 → 3D") },
            { "Premium 3D", Values("Premium 3D", "プレミアム 3D", "進階 3D", "高级 3D") },
            { "Music + SFX", Values("Music + SFX", "音楽 + 効果音", "音樂 + 音效", "音乐 + 音效") },
            { "Sound effects", Values("Sound effects", "効果音", "音效", "音效") },
            { "Background music", Values("Background music", "背景音楽", "背景音樂", "背景音乐") },
            { "loopable", Values("loopable", "ループ可能", "可循環", "可循环") },
            { "Free under $1M annual revenue (Stability Community License); an Enterprise license is required at or above $1M.", Values("Free under $1M annual revenue (Stability Community License); an Enterprise license is required at or above $1M.", "年間売上 100 万米ドル未満は無料です（Stability Community License）。100 万米ドル以上では Enterprise ライセンスが必要です。", "年營收低於 100 萬美元可免費使用（Stability Community License）；達到或超過 100 萬美元則需要 Enterprise 授權。", "年营收低于 100 万美元可免费使用（Stability Community License）；达到或超过 100 万美元则需要 Enterprise 许可。") },
        };

        private static bool initialized;
        private static EditorLanguage currentLanguage;

        public static event Action LanguageChanged;

        public static EditorLanguage CurrentLanguage
        {
            get
            {
                EnsureInitialized();
                return currentLanguage;
            }
        }

        public static IReadOnlyList<string> AvailableLanguageLabels => LanguageLabels;

        public static void SetLanguage(EditorLanguage language)
        {
            EnsureInitialized();
            if (!Enum.IsDefined(typeof(EditorLanguage), language))
            {
                language = EditorLanguage.English;
            }

            if (currentLanguage == language)
            {
                return;
            }

            currentLanguage = language;
            EditorPrefs.SetInt(EditorPrefKeys.EditorLanguage, (int)language);
            LanguageChanged?.Invoke();
        }

        public static string GetLanguageLabel(EditorLanguage language)
        {
            int index = Mathf.Clamp((int)language, 0, LanguageLabels.Length - 1);
            return LanguageLabels[index];
        }

        public static string Text(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            EnsureInitialized();
            if (!Texts.TryGetValue(source, out string[] values))
            {
                return TryTranslateDynamic(source, out string localized) ? localized : source;
            }

            int index = Mathf.Clamp((int)currentLanguage, 0, values.Length - 1);
            return values[index];
        }

        public static string Format(string source, params object[] args)
        {
            return string.Format(Text(source), args);
        }

        public static string TextMultiline(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            string[] lines = source.Replace("\r\n", "\n").Split('\n');
            var localized = new string[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                localized[i] = Text(lines[i]);
            }
            return string.Join(Environment.NewLine, localized);
        }

        public static void LocalizeTree(VisualElement root)
        {
            if (root == null || CurrentLanguage == EditorLanguage.English)
            {
                return;
            }

            foreach (TextElement element in root.Query<TextElement>().ToList())
            {
                element.text = Text(element.text);
            }

            foreach (TextField field in root.Query<TextField>().ToList())
            {
                field.label = Text(field.label);
            }

            foreach (DropdownField field in root.Query<DropdownField>().ToList())
            {
                field.label = Text(field.label);
            }

            foreach (VisualElement element in root.Query<VisualElement>().ToList())
            {
                element.tooltip = Text(element.tooltip);
            }
        }

        public static bool DisplayDialog(string title, string message, string ok)
        {
            return EditorUtility.DisplayDialog(Text(title), Text(message), Text(ok));
        }

        public static bool DisplayDialog(string title, string message, string ok, string cancel)
        {
            return EditorUtility.DisplayDialog(Text(title), Text(message), Text(ok), Text(cancel));
        }

        private static string[] Values(string english, string japanese, string traditionalChinese, string simplifiedChinese)
        {
            return new[] { english, japanese, traditionalChinese, simplifiedChinese };
        }

        private static bool TryTranslateDynamic(string source, out string localized)
        {
            localized = null;
            if (currentLanguage == EditorLanguage.English)
            {
                return false;
            }

            if (TryExtract(source, "Found Python ", " in PATH", out string pythonVersion))
            {
                localized = Format("Found Python {0} in PATH", pythonVersion);
                return true;
            }
            if (TryExtract(source, "Found Python ", " via uv", out pythonVersion))
            {
                localized = Format("Found Python {0} via uv", pythonVersion);
                return true;
            }
            if (TryExtractPair(source, "Found Python ", " at ", string.Empty, out pythonVersion, out string pythonPath))
            {
                localized = Format("Found Python {0} at {1}", pythonVersion, pythonPath);
                return true;
            }
            if (TryExtractPair(source, "Found uvx ", " at ", " (fallback)", out string uvxVersion, out string uvxPath))
            {
                localized = Format("Found uvx {0} at {1} (fallback)", uvxVersion, uvxPath);
                return true;
            }
            if (TryExtractPair(source, "Found uv ", " at ", string.Empty, out string uvVersion, out string uvPath))
            {
                localized = Format("Found uv {0} at {1}", uvVersion, uvPath);
                return true;
            }
            if (TryExtract(source, "Found uv ", " in PATH", out uvVersion))
            {
                localized = Format("Found uv {0} in PATH", uvVersion);
                return true;
            }
            if (TryExtract(source, "Found uv ", " (fallback to system path)", out uvVersion))
            {
                localized = Format("Found uv {0} (fallback to system path)", uvVersion);
                return true;
            }
            if (TryExtract(source, "Found uv ", " (override path)", out uvVersion))
            {
                localized = Format("Found uv {0} (override path)", uvVersion);
                return true;
            }
            if (TryExtract(source, "Found uv ", " in system path", out uvVersion))
            {
                localized = Format("Found uv {0} in system path", uvVersion);
                return true;
            }
            if (TryExtract(source, "✓ ", ": Configured successfully", out string clientName))
            {
                localized = Format("✓ {0}: Configured successfully", clientName);
                return true;
            }

            return TryTranslatePrefixed(source, "Error detecting Python: ", "Error detecting Python: {0}", out localized)
                || TryTranslatePrefixed(source, "Error detecting uvx: ", "Error detecting uvx: {0}", out localized)
                || TryTranslatePrefixed(source, "Error detecting uv: ", "Error detecting uv: {0}", out localized)
                || TryTranslatePrefixed(source, "Invalid URL: ", "Invalid URL: {0}", out localized)
                || TryTranslatePrefixed(source, "Invalid HTTP Remote URL: ", "Invalid HTTP Remote URL: {0}", out localized);
        }

        private static bool TryTranslatePrefixed(string source, string prefix, string template, out string localized)
        {
            localized = null;
            if (!source.StartsWith(prefix, StringComparison.Ordinal))
            {
                return false;
            }

            localized = Format(template, source.Substring(prefix.Length));
            return true;
        }

        private static bool TryExtract(string source, string prefix, string suffix, out string value)
        {
            value = null;
            if (!source.StartsWith(prefix, StringComparison.Ordinal)
                || !source.EndsWith(suffix, StringComparison.Ordinal)
                || source.Length < prefix.Length + suffix.Length)
            {
                return false;
            }

            value = source.Substring(prefix.Length, source.Length - prefix.Length - suffix.Length);
            return true;
        }

        private static bool TryExtractPair(
            string source,
            string prefix,
            string separator,
            string suffix,
            out string first,
            out string second)
        {
            first = null;
            second = null;
            if (!source.StartsWith(prefix, StringComparison.Ordinal)
                || !source.EndsWith(suffix, StringComparison.Ordinal))
            {
                return false;
            }

            int separatorIndex = source.IndexOf(separator, prefix.Length, StringComparison.Ordinal);
            if (separatorIndex < 0)
            {
                return false;
            }

            first = source.Substring(prefix.Length, separatorIndex - prefix.Length);
            int secondStart = separatorIndex + separator.Length;
            second = source.Substring(secondStart, source.Length - secondStart - suffix.Length);
            return true;
        }

        private static void EnsureInitialized()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;
            if (EditorPrefs.HasKey(EditorPrefKeys.EditorLanguage))
            {
                int saved = EditorPrefs.GetInt(EditorPrefKeys.EditorLanguage, (int)EditorLanguage.English);
                currentLanguage = Enum.IsDefined(typeof(EditorLanguage), saved)
                    ? (EditorLanguage)saved
                    : EditorLanguage.English;
                return;
            }

            currentLanguage = GetSystemLanguage();
            EditorPrefs.SetInt(EditorPrefKeys.EditorLanguage, (int)currentLanguage);
        }

        private static EditorLanguage GetSystemLanguage()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Japanese:
                    return EditorLanguage.Japanese;
                case SystemLanguage.ChineseTraditional:
                    return EditorLanguage.TraditionalChinese;
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    return EditorLanguage.SimplifiedChinese;
                case SystemLanguage.English:
                default:
                    return EditorLanguage.English;
            }
        }
    }
}
