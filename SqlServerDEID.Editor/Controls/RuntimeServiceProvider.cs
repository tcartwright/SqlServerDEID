using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SqlServerDEID.Editor.Controls
{
    /// <summary>
    /// https://localcoder.org/is-there-any-way-to-use-a-collectioneditor-outside-of-the-property-grid
    /// </summary>
    /// <seealso cref="System.IServiceProvider" />
    /// <seealso cref="System.ComponentModel.ITypeDescriptorContext" />
    public class RuntimeServiceProvider : IServiceProvider, ITypeDescriptorContext
    {
        #region IServiceProvider Members

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(IWindowsFormsEditorService))
            {
                return new WindowsFormsEditorService();
            }

            return null;
        }

        class WindowsFormsEditorService : IWindowsFormsEditorService
        {
            #region IWindowsFormsEditorService Members
            public void DropDownControl(System.Windows.Forms.Control control)
            {

            }

            public void CloseDropDown()
            {
            }

            public System.Windows.Forms.DialogResult ShowDialog(Form dialog)
            {
                return dialog.ShowDialog();
            }
            #endregion
        }

        #endregion

        #region ITypeDescriptorContext Members

        public void OnComponentChanged()
        {
        }

        public IContainer Container
        {
            get { return null; }
        }

        public bool OnComponentChanging()
        {
            return true; // true to keep changes, otherwise false
        }

        public object Instance
        {
            get { return null; }
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get { return null; }
        }

        #endregion

    }
}