using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using EDEngineer.Localization;
using EDEngineer.Utils.System;

namespace EDEngineer.Utils.UI
{
    public static class TrayIconManager
    {
        public static IDisposable Init(ContextMenuStrip menu)
        {
            var icon = new NotifyIcon
            {
                Icon = Properties.Resources.elite_dangerous_icon,
                Visible = true,
                Text = "ED - Engineer",
                ContextMenuStrip = menu
            };

            return Disposable.Create(() =>
            {
                icon.Visible = false;
                icon.Icon = null;
                icon.Dispose();
            });
        }

        public static ContextMenuStrip BuildContextMenu(EventHandler showHandler, 
            EventHandler quitHandler, 
            EventHandler configureShortcutHandler, 
            EventHandler unlockWindowHandler, 
            EventHandler resetWindowHandler,
            EventHandler selectLanguageHandler,
            Func<bool> launchServerHandler,
            bool serverRunning,
            EventHandler showReleaseNotesHandler,
            string version,
            EventHandler configureThresholdsHandler,
            EventHandler configureNotificationsHandler,
            EventHandler configureGraphicsHandler)
        {
            var translator = Languages.Instance;

            var showItem = new ToolStripMenuItem { Image = Properties.Resources.elite_dangerous_icon.ToBitmap() };
            showItem.Click += showHandler;

            var unlockItem = new ToolStripMenuItem { Image = Properties.Resources.menu_lock_unlock.ToBitmap() };
            unlockItem.Click += unlockWindowHandler;

            var resetItem = new ToolStripMenuItem { Image = Properties.Resources.menu_reset_position.ToBitmap() };
            resetItem.Click += resetWindowHandler;

            var selectLanguageItem = new ToolStripMenuItem { Image = Properties.Resources.menu_language.ToBitmap() };
            selectLanguageItem.Click += selectLanguageHandler;

            var configureGraphicsItem = new ToolStripMenuItem
            {
                Image = Properties.Resources.menu_graphic_settings.ToBitmap()
            };
            configureGraphicsItem.Click += configureGraphicsHandler;

            var setShortCutItem = new ToolStripMenuItem { Image = Properties.Resources.menu_shortcut.ToBitmap() };
            setShortCutItem.Click += configureShortcutHandler;

            var configureThresholdsItem = new ToolStripMenuItem
            {
                Image = Properties.Resources.menu_thresholds.ToBitmap()
            };
            configureThresholdsItem.Click += configureThresholdsHandler;

            var configureNotificationsItem = new ToolStripMenuItem
            {
                Image = Properties.Resources.menu_notifications.ToBitmap()
            };
            configureNotificationsItem.Click += configureNotificationsHandler;

            var helpItem = new ToolStripMenuItem { Image = Properties.Resources.menu_help.ToBitmap() };
            helpItem.Click += (o,e) => Process.Start("https://github.com/msarilar/EDEngineer/wiki/Troubleshooting-Issues");

            var releaseNotesItem = new ToolStripMenuItem { Image = Properties.Resources.menu_release_notes.ToBitmap() };
            releaseNotesItem.Click += showReleaseNotesHandler;
            releaseNotesItem.Text = $"v{version}";

            var quitItem = new ToolStripMenuItem { Image = Properties.Resources.menu_quit.ToBitmap() };

            var enableSilentLaunch = new ToolStripMenuItem
            {
                Checked = SettingsManager.SilentLaunch,
                Image = Properties.Resources.menu_silent_launch.ToBitmap()
            };

            enableSilentLaunch.Click += (o, e) =>
            {
                enableSilentLaunch.Checked = !enableSilentLaunch.Checked;
                SettingsManager.SilentLaunch = enableSilentLaunch.Checked;
            };

            var launchServerItem = new ToolStripMenuItem
            {
                Checked = serverRunning,
                Image = Properties.Resources.menu_api.ToBitmap()
            };

            launchServerItem.Click += (o, e) =>
            {
                launchServerItem.Checked = launchServerHandler();
            };

            SetItemsText(quitItem, translator, helpItem, setShortCutItem, selectLanguageItem, resetItem, unlockItem, showItem, launchServerItem, configureThresholdsItem, enableSilentLaunch, configureNotificationsItem, configureGraphicsItem);
            translator.PropertyChanged += (o, e) =>
            {
                SetItemsText(quitItem, translator, helpItem, setShortCutItem, selectLanguageItem, resetItem, unlockItem, showItem, launchServerItem, configureThresholdsItem, enableSilentLaunch, configureNotificationsItem, configureGraphicsItem);
            };

            quitItem.Click += quitHandler;

            var menu = new ContextMenuStrip();
            menu.Items.Add(showItem);
            menu.Items.Add(unlockItem);
            menu.Items.Add(resetItem);
            menu.Items.Add("-");
            menu.Items.Add(configureNotificationsItem);
            menu.Items.Add(configureThresholdsItem);
            menu.Items.Add("-");
            menu.Items.Add(enableSilentLaunch);
            menu.Items.Add(setShortCutItem);
            menu.Items.Add(selectLanguageItem);
            menu.Items.Add(configureGraphicsItem);
            menu.Items.Add("-");
            menu.Items.Add(launchServerItem);
            menu.Items.Add("-");
            menu.Items.Add(helpItem);
            menu.Items.Add(releaseNotesItem);
            menu.Items.Add(quitItem);
            return menu;
        }

        private static void SetItemsText(ToolStripMenuItem quitItem, Languages translator, ToolStripMenuItem helpItem, ToolStripMenuItem setShortCutItem,
                                         ToolStripMenuItem selectLanguageItem, ToolStripMenuItem resetItem, ToolStripMenuItem unlockItem, ToolStripMenuItem showItem,
                                         ToolStripMenuItem launchServerItem, ToolStripMenuItem configureThresholdsItem, ToolStripMenuItem enableSilentLaunch,
                                         ToolStripMenuItem configureNotificationsItem, ToolStripMenuItem configureGraphicsItem)
        {
            quitItem.Text = translator.Translate("Quit");
            helpItem.Text = translator.Translate("Help");
            setShortCutItem.Text = translator.Translate("Set Shortcut");
            selectLanguageItem.Text = translator.Translate("Select Language");
            resetItem.Text = translator.Translate("Reset Window Position");
            unlockItem.Text = translator.Translate("Toggle Window Mode (Locked/Unlocked)");
            showItem.Text = translator.Translate("Show");
            launchServerItem.Text = translator.Translate("Launch Local API");
            configureThresholdsItem.Text = translator.Translate("Configure Thresholds");
            enableSilentLaunch.Text = translator.Translate("Silent Launch");
            configureNotificationsItem.Text = translator.Translate("Configure Notifications");
            configureGraphicsItem.Text = translator.Translate("Configure Graphics");
        }
    }
}