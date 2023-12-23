import clr
clr.AddReference("System.Windows.Forms")
import System.Windows.Forms as WinForms

frm=WinForms.Form()
frm.MdiContainer=True

WinForms.Application.Run(frm)