using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Esperecyan.NCVVCasVideoRequestList.Properties;
using Plugin;

namespace Esperecyan.NCVVCasVideoRequestList
{
    public class Plugin : IPlugin
    {
        private static readonly Regex LuaStringLiteralEscapingPattern = new Regex(@"""|\\");

        /// <summary>
        /// 出力先のVCIデバッグファイルの <see cref="Environment.SpecialFolder.UserProfile"/> からのパス。
        /// </summary>
        private static readonly string VCILuaFilePath
            = @"AppData\LocalLow\infiniteloop Co,Ltd\VirtualCast\EmbeddedScriptWorkspace\動画リクエスト一覧\external-requests.lua";

        public IPluginHost Host { get; set; }

        public bool IsAutoRun => false;

        public string Description => ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyTitleAttribute)
        )).Title;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(fieldCount: 3);

        public string Name => ((AssemblyProductAttribute)Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(),
            typeof(AssemblyProductAttribute)
        )).Product;

        private Window window;
        private IEnumerable<UserSettingInPlugin.UserData> userDataList;
        private string vciLuaFilePath;

        public void AutoRun()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            if (this.window != null && !this.window.IsDisposed)
            {
                this.window.Focus();
                return;
            }

            if (this.vciLuaFilePath == null)
            {
                this.vciLuaFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Plugin.VCILuaFilePath
                );
                Directory.CreateDirectory(Path.GetDirectoryName(this.vciLuaFilePath));
            }

            this.window = new Window()
            {
                Text = this.Name + " " + this.Version,
            };
            this.window.Show();
            this.userDataList = this.Host.GetUserSettingInPlugin().UserDataList;
            Application.ApplicationExit += this.Application_ApplicationExit;
            this.window.Requests.ListChanged += this.Requests_ListChanged;
            Settings.Default.SettingChanging += this.SettingsDefault_SettingChanging;
            try
            {
                this.Host_ReceivedComment(sender: null, new ReceivedCommentEventArgs(this.Host.GetAcquiredComment()));
            }
            catch (ApplicationException exception)
            {
                if (exception.Message != "コメントデータがありません。")
                {
                    throw;
                }
            }
            this.PushToVCI();
            this.Host.ReceivedComment += this.Host_ReceivedComment;
            this.window.FormClosing += (sender, args) =>
            {
                this.Host.ReceivedComment -= this.Host_ReceivedComment;
                this.window.Requests.ListChanged -= this.Requests_ListChanged;
                this.ClearVCI();
                Application.ApplicationExit -= this.Application_ApplicationExit;
                foreach (var request in this.window.Requests)
                {
                    request.Dispose();
                }
            };
            var dataGridView = this.window.DataGridView;
            dataGridView.CellContentClick += (object sender, DataGridViewCellEventArgs e) =>
            {
                var request = this.window.Requests[e.RowIndex];
                var url = request.URL;
                switch (dataGridView.Columns[e.ColumnIndex].Name)
                {
                    case "URL":
                        Process.Start(new ProcessStartInfo()
                        {
                            UseShellExecute = true,
                            FileName = url,
                        });
                        break;
                    case "Copy":
                        Clipboard.SetText(url);
                        request.AlreadyPlayed = true;
                        request.Used = true;
                        break;
                }
            };
        }

        private void Host_ReceivedComment(object sender, ReceivedCommentEventArgs e)
        {
            foreach (var commentData in e.CommentDataList)
            {
                foreach (var request in Request.Create(commentData, this.userDataList))
                {
                    this.window.Requests.Add(request);
                }
            }
        }

        private void Requests_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.PushToVCI();
        }

        private void SettingsDefault_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            if (!new[] {
                nameof(Settings.Default.NotPushingAnonymousCommentToVCI),
            }.Contains(e.SettingName))
            {
                return;
            }

            this.PushToVCI();
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            this.ClearVCI();
        }

        private void PushToVCI()
        {
            File.WriteAllText(this.vciLuaFilePath, "return {\n"
                + string.Join("\n", this.window.Requests
                    .Where(request => !Settings.Default.NotPushingAnonymousCommentToVCI || !request.IsAnonymity)
                    .Select(request =>
                    {
                        return $@"   {{
        userId = {this.EscapeToLuaStringLiteral(request.UserNameOrId)},
        status = {this.EscapeToLuaStringLiteral(
                    request.VirtualCastSupport != "○" ? request.VirtualCastSupport : request.ServiceName
                )},
        url = {this.EscapeToLuaStringLiteral(request.URL)},
        title = {(request.Title != null ? this.EscapeToLuaStringLiteral(request.Title) : "nil")},
        alreadyPlayed = {(request.AlreadyPlayed ? "true" : "false")},
    }},";
                    }))
                    + "\n}");
        }

        private void ClearVCI()
        {
            File.WriteAllText(this.vciLuaFilePath, "return nil");
        }

        /// <summary>
        /// 文字列をエスケープし、Luaの文字列リテラルとして返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns>「"サンプル"」のような「"」で囲まれた文字列を返します。</returns>
        private string EscapeToLuaStringLiteral(string str)
        {
            return "\"" + Plugin.LuaStringLiteralEscapingPattern.Replace(str, @"\$&") + "\"";
        }
    }
}
