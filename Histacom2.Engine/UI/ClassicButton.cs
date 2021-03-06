﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Histacom2.Engine.UI
{
    public class ClassicButton : Control, IButtonControl
    {
        private Color _lightBack;
        private Color _darkBack;

        private bool _pressing = false;

        public DialogResult DialogResult { get; set; }

        public bool AdaptBackColorWithTheme { get; set; }
        public bool AdaptForeColorWithTheme { get; set; }
        public bool AdaptFontWithTheme { get; set; }

        public ClassicButton() : base()
        {
            AdaptBackColorWithTheme = true;
            AdaptForeColorWithTheme = true;
            AdaptFontWithTheme = true;
            if (SaveSystem.currentTheme != null) BackColor = SaveSystem.currentTheme.threeDObjectsColor;
            else BackColor = Color.Silver;
            _lightBack = ControlPaint.Light(BackColor, 50);
            _darkBack = ControlPaint.Dark(BackColor, 50);

            MouseDown += (s, e) => { _pressing = true; Invalidate(); };
            MouseUp += (s, e) => { _pressing = false; Invalidate(); };
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (SaveSystem.currentTheme != null && AdaptBackColorWithTheme) BackColor = SaveSystem.currentTheme.threeDObjectsColor;

            if (AdaptForeColorWithTheme)
            {
                if (SaveSystem.currentTheme != null) ForeColor = SaveSystem.currentTheme.threeDObjectsTextColor;
                else ForeColor = Color.Black;
            }

            if (AdaptFontWithTheme)
            {
                if (SaveSystem.currentTheme != null) Font = SaveSystem.currentTheme.buttonFont;
                else Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
            }

            _lightBack = Paintbrush.GetLightFromColor(BackColor);
            _darkBack = Paintbrush.GetDarkFromColor(BackColor);

            var g = e.Graphics;
            g.Clear(BackColor);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
            
            if (_pressing && Enabled)
            {
                g.FillRectangle(new SolidBrush(_lightBack), new Rectangle(0, 0, Width, Height));
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width - 1, Height - 1));
                g.FillRectangle(new SolidBrush(_darkBack), new Rectangle(1, 1, Width - 2, Height - 2));
                g.FillRectangle(new SolidBrush(BackColor), new Rectangle(2, 2, Width - 3, Height - 3));

                g.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(2, 2, Width - 3, Height - 3), sf);
            }
            else
            {
                g.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width, Height));
                g.FillRectangle(new SolidBrush(_lightBack), new Rectangle(0, 0, Width - 1, Height - 1));
                g.FillRectangle(new SolidBrush(_darkBack), new Rectangle(1, 1, Width - 2, Height - 2));
                g.FillRectangle(new SolidBrush(BackColor), new Rectangle(1, 1, Width - 3, Height - 3));

                if (Enabled) g.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(1, 1, Width - 3, Height - 3), sf);
                else g.DrawString(Text, Font, new SolidBrush(_darkBack), new Rectangle(1, 1, Width - 3, Height - 3), sf);
            }
        }

        public void NotifyDefault(bool value)
        {
            
        }

        public void PerformClick()
        {
            this.OnClick(new EventArgs());
        }
    }
}
