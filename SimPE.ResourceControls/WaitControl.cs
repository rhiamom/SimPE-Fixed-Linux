/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatShop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace SimPe
{
    public partial class WaitControl : UserControl, IWaitingBarControl
    {
        public WaitControl()
        {
            msg = "";
            InitializeComponent();

            Message = "";
            MaxProgress = 0;
            Waiting = false;            
            ShowProgress = false;
            ShowAnimation = true;
            ShowText = true;
            nowp = -1;

            if (SimPe.Helper.XmlRegistry.ShowWaitBarPermanent)
            {
                ThemeManager.Global.AddControl(this.statusStrip1);
            }
        }

        string msg;
        public string Message
        {
            get {
                lock (msg)
                {
                    return msg;
                }
            }
            set
            {
                lock (msg)
                {
                    msg = value;
                }
                Avalonia.Threading.Dispatcher.UIThread.Post(() => { this.tbInfo.Text = this.msg; });
            }
        }

        int max;
        public int MaxProgress
        {
            get { lock (pb) { return max; } }
            set
            {
                lock (pb)
                {
                    max = value;
                }
                var _mp = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => { this.pb.Value = this.pb.Minimum; this.pb.Maximum = Math.Max(Math.Max(1, pb.Minimum), _mp); DoShowProgress(this.pb.Maximum > 1); });
            }
        }

        int val;
        int nowp;
        public int Progress
        {
            get { return val; }
            set
            {
                var _pv = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => SetProgress(_pv));
            }
        }

        private void SetProgress(int value)
        {
            val = Math.Min(pb.Maximum, value);
            this.pb.Value = val;

            //float perc = (((float)val / (float)pb.Maximum) * 100);

            int perc = (val * 100) / pb.Maximum;
            int diff = Math.Abs(nowp - perc);
            if (diff > 0)
            {
                tbPercent.Text = perc.ToString("N0") + "%";
            }

            if (diff >= 10)
            {    
                this.statusStrip1.Refresh();
                nowp = perc;
            }            
        }

        bool wait;        
        public bool Waiting
        {
            get { return wait; }
            set
            {
                var _sw = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => SetWaiting(_sw));
            }
        }

        private void SetWaiting(bool value)
        {
            wait = value;
            if (wait)
            {
                this.Visible = true;
            }
            else
            {
                if (!this.DesignMode && !Helper.XmlRegistry.ShowWaitBarPermanent) this.Visible = false;
                this.Message = "";
                this.Progress = 0;
                this.ShowProgress = false;
            }
        }

        bool spb;
        public bool ShowProgress
        {
            get { return spb; }
            set
            {
                var _sp = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => DoShowProgress(_sp));
            }
        }

        void DoShowProgress(bool value)
        {
            spb = value;
            pb.Visible = spb;
            tbPercent.Visible = spb;
            if (spb)
                tbInfo.BorderSides = ToolStripStatusLabelBorderSides.Left;
            else
                tbInfo.BorderSides = ToolStripStatusLabelBorderSides.None;
        }

        bool sanim;
        public bool ShowAnimation
        {
            get { return sanim; }
            set
            {
                sanim = value;
                var _sa = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => DoShowAnimation(_sa));
            }
        }

        private void DoShowAnimation(bool value)
        {
        }

        bool stxt;
        public bool ShowText
        {
            get { return stxt; }
            set
            {
                var _st = value;
                Avalonia.Threading.Dispatcher.UIThread.Post(() => DoShowText(_st));
            }
        }

        private void DoShowText(bool value)
        {
            stxt = value;
            this.tbInfo.Visible = stxt;
        }

        #region IWaitingBarControl Member

        public bool Running
        {
            get { return Waiting; }
        }

        public Avalonia.Media.Imaging.Bitmap Image
        {
            get { return null; }
            set { }
        }

        public int Value { get; internal set; }
        public int Maximum { get; internal set; }

        public void Wait()
        {
            Message = SimPe.Localization.GetString("Please Wait");
            Image = null;
            Waiting = true;
        }

        public void Wait(int max)
        {
            ShowProgress = true;
            Message = SimPe.Localization.GetString("Please Wait");
            Image = null;
            MaxProgress = max;
            Waiting = true;
        }

        public void Stop()
        {
            ShowProgress = false;
            Waiting = false;
        }

        #endregion
    }
}
